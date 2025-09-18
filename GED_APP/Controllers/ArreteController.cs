using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class ArreteController : Controller
    {
        private readonly IArrete _arreteRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string path;

        public ArreteController(IArrete arreteRepo, IStructure structRepo, IWebHostEnvironment environment)
        {
            _arreteRepo = arreteRepo;
            _structureRepo = structRepo;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Arrete> arretes = null;
            // string c = "SDE";
            // string an = "2025";
            //arretes = _arreteRepo.GetAllByCodeAnne(c, an);
            arretes = _arreteRepo.GetAll();
            //arretes = JsonConvert.DeserializeObject<List<Doc>>(arretes);
            ViewBag.DataSource = arretes;
            return View(arretes);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Arrete ar = new Arrete();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Arrete());
            }
            else
            {
                ar = _arreteRepo.GetById(id);

            }
            return View(ar);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, DateSign, Objet, Origine, NumeroCNE, DateCne, StructureId, Status, Updated")] Arrete a)
        {
            int existe = 0;
            int resp;
            Arrete arr = new Arrete();

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(a.Origine);

                if (a.Id == 0)
                {
                    existe = _arreteRepo.Existe(a);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.StructureId = s.Id;
                        resp = _arreteRepo.Add(a);
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
                    a.StructureId = s.Id;
                    resp = _arreteRepo.Update(a);
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
            int res;
            if (id > 0)
            {
                res = _arreteRepo.Delete(id);
                if (res > 0)
                {
                    TempData["AlertMessage"] = "Suppréssion effectué avec succès.....";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

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

        [HttpGet]
        public IActionResult AddPdf(int id)
        {
            int res;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Arrete ar = new Arrete();
            OnloadStructure();
            ar = _arreteRepo.GetById(id);
            return View(ar);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, DateSign, Objet, Origine, NumeroCNE, DateCne, StructureId, Status, Updated")] Arrete a, IFormFile pdf)
        {
            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(a.Origine);
                    a.StructureId = s.Id;
                    a.Status = 1;
                    a.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _arreteRepo.UploadPdfFileArreteAsync(pdf, a);
                    int resp = _arreteRepo.Update(a);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.StructureId = s.Id;
                        a.Status = 0;
                        resp = _arreteRepo.Update(a);
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
            Arrete a = new Arrete();
            a = _arreteRepo.GetById(id);
            if (a != null)
            {
                string? num = "Arrete_" + a.Numero + "_du_" + a.DateSign + "_session_" + a.NumeroCNE;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/arretes", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(a);
        }
    }
}