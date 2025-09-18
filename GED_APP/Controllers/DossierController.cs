using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class DossierController : Controller
    {
        private readonly IDossier _dossierRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string path;

        public DossierController(IDossier dossierRepo, IStructure structureRepo, IWebHostEnvironment environment)
        {
            _dossierRepo = dossierRepo;
            _structureRepo = structureRepo;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Dossier> ds = null;
            ds = _dossierRepo.GetAll();
            ViewBag.DataSource = ds;
            return View(ds);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Dossier d = new Dossier();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Dossier());
            }
            else
            {
                d= _dossierRepo.GetById(id);
            }
            return View(d);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Demandeur, DateEntree, DateSortie, Objet, Origine, PersonneTraite, Status, StructureId, Updated")] Dossier d)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(d.Origine);
                d.StructureId = s.Id;
                if (d.Id == 0)
                {
                    existe = _dossierRepo.Existe(d);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        d.StructureId = s.Id;
                        resp = _dossierRepo.Add(d);
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
                    d.StructureId = s.Id;
                    resp = _dossierRepo.Update(d);
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
                resp = _dossierRepo.Delete(id);
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
            OnloadStructure();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Dossier d = new Dossier();
           d = _dossierRepo.GetById(id);
            return View(d);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Demandeur, DateEntree, DateSortie, Objet, Origine, PersonneTraite, Status, StructureId, Updated")] Dossier d, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(d.Origine);
                    d.StructureId = s.Id;
                    d.Status = 1;
                    d.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _dossierRepo.UploadPdfFileDossierAsync(pdf, d);
                    int resp = _dossierRepo.Update(d);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        d.StructureId = s.Id;
                        d.Status = 0;
                        resp = _dossierRepo.Update(d);
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
            Dossier d = new Dossier();
            d = _dossierRepo.GetById(id);
            if (d != null)
            {
                string? num = d.Numero + "_" + d.Objet + "_" + d.DateEntree + "_" + d.Demandeur + "_" + d.Origine;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/dossiers", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(d);
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
