using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class CorrespondanceController : Controller
    {
        private readonly ICorrespondance _correspRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public CorrespondanceController(ICorrespondance correspRepo, IStructure strRepo, IWebHostEnvironment environment)
        {
            _correspRepo = correspRepo;
            _structureRepo = strRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Correspondance> crs = null;
            crs = _correspRepo.GetAll();
            ViewBag.DataSource = crs;
            return View(crs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Correspondance cr = new Correspondance();
            OnloadStructure();
            OnloadType();
            if (id == 0)
            {
                return View(new Correspondance());
            }
            else
            {
                cr = _correspRepo.GetById(id);
            }
            return View(cr);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Type, Numero, Date, Initiateur, Objet, Origine, Status, StructureId")] Correspondance c)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(c.Origine);
                if (c.Id == 0)
                {
                    existe = _correspRepo.Existe(c);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.StructureId = s.Id;
                        resp = _correspRepo.Add(c);
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
                    c.StructureId = s.Id;
                    resp = _correspRepo.Update(c);
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
                resp = _correspRepo.Delete(id);
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
            OnloadType();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Correspondance c = new Correspondance();
            c = _correspRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Type, Numero, Date, Objet, Initiateur, Origine, Status, StructureId")] Correspondance c, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(c.Origine);
                    c.StructureId = s.Id;
                    c.Status = 1;
                    c.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _correspRepo.UploadPdfFileCorrespAsync(pdf, c);
                    int resp = _correspRepo.Update(c);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.Status = 0;
                        c.StructureId = s.Id;
                        resp = _correspRepo.Update(c);
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
            Correspondance c = new Correspondance();
            c = _correspRepo.GetById(id);
            if (c != null)
            {
                string? num = c.Type + "_" + c.Numero + "_" + c.Date + "_" + c.Objet + "_To_" + c.Origine;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/corresp", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(c);
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
        [NonAction]
        public void OnloadType()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> types = new List<string>();
            if (ModelState.IsValid)
            {
                types.Insert(0, "Choisir type");
                types.Insert(1, "Lettre");
                types.Insert(2, "Reunion");
                types.Insert(3, "Invitation");
                types.Insert(4, "Communique");
                types.Insert(5, "Information");
                types.Insert(6, "Autre");
                ViewBag.Types = types;
            }
        }
    }
}
