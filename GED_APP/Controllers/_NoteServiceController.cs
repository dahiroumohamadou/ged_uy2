using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class _NoteServiceController : Controller
    {
        private readonly _INoteService _noteRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _NoteServiceController(_INoteService noteRepo, IWebHostEnvironment environment)
        {
            _noteRepo = noteRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_NoteService> nts = null;
            nts = _noteRepo.GetAll();
            ViewBag.DataSource = nts;
            return View(nts);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _NoteService nt = new _NoteService();
            if (id == 0)
            {
                return View(new _NoteService());
            }
            else
            {
                nt = _noteRepo.GetById(id);
            }
            return View(nt);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _NoteService n)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (n.Id == 0)
                {
                    existe = _noteRepo.Existe(n);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _noteRepo.Add(n);
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
                    resp = _noteRepo.Update(n);
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
                resp = _noteRepo.Delete(id);
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
            _NoteService n = new _NoteService();
            n = _noteRepo.GetById(id);
            return View(n);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _NoteService n, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    n.Status = 1;
                    n.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _noteRepo.UploadPdfFileNoteServiceAsync(pdf, n);
                    int resp = _noteRepo.Update(n);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Attestation added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        n.Status = 0;
                        resp = _noteRepo.Update(n);
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
            _NoteService n = new _NoteService();
            n = _noteRepo.GetById(id);
            if (n != null)
            {
                string? num = n.Numero + "_" + n.DateSign + "_" + n.Objet;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/noteServices", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(n);
        }
    }
}
