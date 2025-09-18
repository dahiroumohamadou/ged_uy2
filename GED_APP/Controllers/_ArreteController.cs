using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class _ArreteController : Controller
    {
        private readonly _IArrete _arreteRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _ArreteController(_IArrete arreteRepo, IWebHostEnvironment environment)
        {
            _arreteRepo = arreteRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Arrete> ats = null;
            ats = _arreteRepo.GetAll();
            ViewBag.DataSource = ats;
            return View(ats);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Arrete at = new _Arrete();
            if (id == 0)
            {
                return View(new _Arrete());
            }
            else
            {
                at = _arreteRepo.GetById(id);
            }
            return View(at);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _Arrete a)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (a.Id == 0)
                {
                    existe = _arreteRepo.Existe(a);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
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
            int resp;
            if (id > 0)
            {
                resp = _arreteRepo.Delete(id);
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
            _Arrete a = new _Arrete();
            a = _arreteRepo.GetById(id);
            return View(a);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _Arrete a, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    a.Status = 1;
                    a.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _arreteRepo.UploadPdfFileArreteAsync(pdf, a);
                    int resp = _arreteRepo.Update(a);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Attestation added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
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
            _Arrete a = new _Arrete();
            a = _arreteRepo.GetById(id);
            if (a != null)
            {
                string? num = a.Numero + "_" + a.DateSign + "_" + a.Objet;
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
