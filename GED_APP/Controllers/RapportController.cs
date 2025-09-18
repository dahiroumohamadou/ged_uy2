using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class RapportController : Controller
    {
        private readonly IRapport _rapportRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public RapportController(IRapport rappRepo, IStructure strRepo, IWebHostEnvironment environment)
        {
            _rapportRepo = rappRepo;
            _structureRepo = strRepo;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Rapport> rapports = null;
            rapports = _rapportRepo.GetAll();
            ViewBag.DataSource = rapports;
            return View(rapports);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Rapport r = new Rapport();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Rapport());
            }
            else
            {
                r = _rapportRepo.GetById(id);
            }
            return View(r);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Description, Debut, Fin, Origine, Status, StructureId, Updated")] Rapport r)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(r.Origine);

                if (r.Id == 0)
                {
                    existe = _rapportRepo.Existe(r);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        r.StructureId = s.Id;
                        resp = _rapportRepo.Add(r);
                        if (resp > 0)
                        {
                            TempData["AlertMessage"] = "Enregistrement effectué avec succès.....";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Erreur d'enregistrement .....";
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    r.StructureId = s.Id;
                    resp = _rapportRepo.Update(r);
                    if (resp > 0)
                    {
                        TempData["AlertMessage"] = "Mise à jour effectuée avec succès.....";
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken] 
        public IActionResult Delete(int id)
        {
            int resp;
            if (id > 0)
            {
                resp = _rapportRepo.Delete(id);
                if (resp > 0)
                {
                    TempData["AlertMessage"] = "Suppréssion effectuée avec succès.....";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessage"] = "Erreur de suppréssion.....";
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult AddPdf(int id)
        {

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Rapport r = new Rapport();
            r = _rapportRepo.GetById(id);
            return View(r);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Description, Debut, Fin, Origine, Status, StructureId, Updated")] Rapport r, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(r.Origine);
                    r.StructureId = s.Id;
                    r.Status = 1;
                    r.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _rapportRepo.UploadPdfFileRapportAsync(pdf, r);
                    int resp = _rapportRepo.Update(r);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        r.StructureId = s.Id;
                        r.Status = 0;
                        resp = _rapportRepo.Update(r);
                        TempData["AlertMessage"] = "Erreur ajout du document.....";
                        return RedirectToAction("Index");
                    }

                }
                return RedirectToAction("Index");
                //return RedirectToAction("Details", new { d.Id });
            }
            return View();
        }
        [HttpGet]
        public IActionResult showPdf(int id)
        {
            Rapport r = new Rapport();
            r = _rapportRepo.GetById(id);
            if (r != null)
            {
                string? num = "Rapport_" + r.Origine + "_" + r.Periode;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/rapports", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(r);
        }
        [NonAction]
        public void OnloadStructure()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Structure> structures = new List<Structure>();
            if (ModelState.IsValid)
            {
                structures = (List<Structure>)_structureRepo.GetAll();
                if (structures != null)
                {
                    Structure StructureDefault = new Structure() { Id = 0, Libele = "Choisir structure" };
                    structures.Insert(0, StructureDefault);
                    ViewBag.Structures = structures;
                }
            }
        }
    }
}
