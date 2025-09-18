using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class PvCneController : Controller
    {
        private readonly IPvCne _pvRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public PvCneController(IPvCne pvRepo, IStructure structureRepo, IWebHostEnvironment environment)
        {
            _pvRepo = pvRepo;
            _structureRepo = structureRepo;
            _environment = environment;
        }

        // GET: PvCneController
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<PvCNE> pvs = null;
            pvs = _pvRepo.GetAll();
            ViewBag.DataSource = pvs;
            return View(pvs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            PvCNE pv = new PvCNE();
            OnloadStructure();
            if (id == 0)
            {
                return View(new PvCNE());
            }
            else
            {
                pv = _pvRepo.GetById(id);
            }
            return View(pv);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Origine, Description, NumeroCne, DateCne, Updated, Status, StructureId")] PvCNE p)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(p.Origine);
                if (p.Id == 0)
                {
                    existe = _pvRepo.Existe(p);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        p.StructureId = s.Id;
                        resp = _pvRepo.Add(p);
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
                    p.StructureId = s.Id;
                    resp = _pvRepo.Update(p);
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
                resp = _pvRepo.Delete(id);
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
            PvCNE p = new PvCNE();
            p = _pvRepo.GetById(id);
            return View(p);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Origine, Description, NumeroCne, DateCne, Updated, Status, StructureId")] PvCNE p, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(p.Origine);
                    p.StructureId = s.Id;
                    p.Status = 1;
                    p.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _pvRepo.UploadPdfFilePvCneAsync(pdf, p);
                    int resp = _pvRepo.Update(p);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        p.Status = 0;
                        p.StructureId = s.Id;
                        resp = _pvRepo.Update(p);
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
            PvCNE p = new PvCNE();
            p = _pvRepo.GetById(id);
            if (p != null)
            {
                string? num = "Pv_CNE" + p.NumeroCne + "_" + p.DateCne;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/PvCne", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(p);
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
