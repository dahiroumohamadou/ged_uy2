using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class DechargeController : Controller
    {
        private readonly IDecharge _dechargeRepo;
        private readonly IStructure _structRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public DechargeController(IDecharge dechargeRepo, IStructure structRepo, IWebHostEnvironment environment)
        {
            _dechargeRepo = dechargeRepo;
            _structRepo = structRepo;
            _environment = environment;
        }



        // GET: DechargeController
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Decharge> ds = null;
            ds = _dechargeRepo.GetAll();
            ViewBag.DataSource = ds;
            return View(ds);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Decharge d = new Decharge();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Decharge());
            }
            else
            {
                d = _dechargeRepo.GetById(id);
            }
            return View(d);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Description, Date, Origine, Status, Updated, StructureId")] Decharge d)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {

                if (d.Id == 0)
                {
                    existe = _dechargeRepo.Existe(d);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Structure s = _structRepo.GetByCode(d.Origine);
                        d.StructureId = s.Id;

                        resp = _dechargeRepo.Add(d);
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
                    Structure s = _structRepo.GetByCode(d.Origine);
                    d.StructureId = s.Id;
                    resp = _dechargeRepo.Update(d);
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
                resp = _dechargeRepo.Delete(id);
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
            Decharge d = new Decharge();
            d = _dechargeRepo.GetById(id);
            return View(d);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Description, Date, Origine, Status, Updated, StructureId")] Decharge d, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    d.Status = 1;
                    d.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _dechargeRepo.UploadPdfFileDechargeAsync(pdf, d);
                    Structure s = _structRepo.GetByCode(d.Origine);
                    d.StructureId = s.Id;
                    int resp = _dechargeRepo.Update(d);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {

                        d.StructureId = s.Id;
                        d.Status = 0;
                        resp = _dechargeRepo.Update(d);
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
            Decharge d = new Decharge();
            d = _dechargeRepo.GetById(id);
            if (d != null)
            {
                String? num = d.Origine + "_Du_" + d.Date + "_" + d.Description;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decharges", replacement + ".pdf");
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
                structures = (List<Structure>)_structRepo.GetAll();
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
