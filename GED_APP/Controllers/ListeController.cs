using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class ListeController : Controller
    {
        private readonly IListe _listeRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public ListeController(IListe listeRepo, IStructure structureRepo, IWebHostEnvironment environment)
        {
            _listeRepo = listeRepo;
            _structureRepo = structureRepo;
            _environment = environment;
        }

        // GET: ListeController
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Liste> ls = null;
            ls = _listeRepo.GetAll();
            //arretes = JsonConvert.DeserializeObject<List<Doc>>(arretes);
            ViewBag.DataSource = ls;
            return View(ls);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Liste li = new Liste();
            OnloadStructure();
            if (id == 0)
            {
                return View(new Liste());
            }
            else
            {
                li = _listeRepo.GetById(id);

            }
            return View(li);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, DateSign, Descritpion, Origine, NumeroCne, DateCne, StructureId, Status, Updated")] Liste l)
        {
            int existe = 0;
            int resp;
            Liste li = new Liste();

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                Structure s = _structureRepo.GetByCode(l.Origine);
                if (l.Id == 0)
                {
                    existe = _listeRepo.Existe(l);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        l.StructureId = s.Id;
                        resp = _listeRepo.Add(l);
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
                    l.StructureId = s.Id;
                    resp = _listeRepo.Update(l);
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
                res = _listeRepo.Delete(id);
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
        //[NonAction]
        //public void OnloadType()
        //{
        //    //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        //    List<string> types = new List<string>();
        //    if (ModelState.IsValid)
        //    {
        //        types.Insert(0, "Choisir type");
        //        types.Insert(1, "Accordées");
        //        types.Insert(2, "Rejeté");
        //        ViewBag.Types = types;
        //    }
        //}

        [HttpGet]
        public IActionResult AddPdf(int id)
        {
            int res;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Liste li = new Liste();
            OnloadStructure();
           li = _listeRepo.GetById(id);
            return View(li);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, DateSign, Descritpion, Origine, NumeroCne, DateCne, StructureId, Status, Updated")] Liste l, IFormFile pdf)
        {
            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    Structure s = _structureRepo.GetByCode(l.Origine);
                    l.StructureId = s.Id;
                    l.Status = 1;
                    l.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _listeRepo.UploadPdfFileListeAsync(pdf, l);
                    int resp = _listeRepo.Update(l);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        l.StructureId = s.Id;
                        l.Status = 0;
                        resp = _listeRepo.Update(l);
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
            Liste l = new Liste();
            l = _listeRepo.GetById(id);
            if (l != null)
            {
                string? num = "Liste_" + l.Numero + "_du_" + l.DateSign + "_session_" + l.NumeroCne;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/Listes", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(l);
        }
    }
}
