using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class ChronogrammeController : Controller
    {
        private readonly IChronogramme _chronoRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public ChronogrammeController(IChronogramme chronoRepo, IStructure structRepo, IWebHostEnvironment environment)
        {
            _chronoRepo = chronoRepo;
            _structureRepo = structRepo;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Chronogramme> chronos = null;
            chronos = _chronoRepo.GetAll();
            ViewBag.DataSource = chronos;
            return View(chronos);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Chronogramme chrono = new Chronogramme();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Chronogramme());
            }
            else
            {
                chrono = _chronoRepo.GetById(id);
            }
            return View(chrono);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, NumeroPTA, Annee, Libele, Origine, Status, StructureId")] Chronogramme c)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {

                if (c.Id == 0)
                {
                    existe = _chronoRepo.Existe(c);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Structure s = _structureRepo.GetByCode(c.Origine);
                        c.StructureId=s.Id;
                        
                        resp = _chronoRepo.Add(c);
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
                    Structure s = _structureRepo.GetByCode(c.Origine);
                    c.StructureId = s.Id;
                    resp = _chronoRepo.Update(c);
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
                resp = _chronoRepo.Delete(id);
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
            Chronogramme c = new Chronogramme();
            c = _chronoRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, NumeroPTA, Annee, Libele, Origine, Status, StructureId")] Chronogramme c, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    c.Status = 1;
                    c.Updated= DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _chronoRepo.UploadPdfFileChronoAsync(pdf, c);
                    Structure s = _structureRepo.GetByCode(c.Origine);
                    c.StructureId = s.Id;
                    int resp = _chronoRepo.Update(c);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        
                        c.StructureId = s.Id;
                        c.Status = 0;
                        resp = _chronoRepo.Update(c);
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
            Chronogramme c = new Chronogramme();
            c = _chronoRepo.GetById(id);
            if (c != null)
            {
                string? num = "Chrono_" + c.NumeroPTA + "_" + c.Origine + "_" + c.Annee;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/chronos", replacement + ".pdf");
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
                structures = (List<Structure>) _structureRepo.GetAll();
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
