using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    [Authorize]
    public class _CorrespondanceController : Controller
    {
        private readonly _ICorrespondance _correspRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _CorrespondanceController(_ICorrespondance correspondanceRepo, IWebHostEnvironment environment)
        {
            _correspRepo = correspondanceRepo;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Correspondance> crs = null;
            crs = _correspRepo.GetAll();
            ViewBag.DataSource = crs;
            return View(crs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Correspondance cr = new _Correspondance();
           
            if (id == 0)
            {
                return View(new _Correspondance());
            }
            else
            {
                cr = _correspRepo.GetById(id);
            }
            return View(cr);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Reference, Objet, Emetteur, Recepteur, Signataire, DateSign, Status, Code")] _Correspondance c)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (c.Id == 0)
                {
                    existe = _correspRepo.Existe(c);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Cette correspondance existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
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
        [Authorize(Roles = "ADMIN")]
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
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Correspondance c = new _Correspondance();
            c = _correspRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Reference, Objet, Emetteur, Recepteur, Signataire, DateSign, Status, Code")] _Correspondance c, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    c.Status = 1;
                    c.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _correspRepo.UploadPdfFileCorrespondanceAsync(pdf,c);
                    int resp = _correspRepo.Update(c);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.Status = 0;
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
        [Authorize(Roles = "ADMIN, CSC")]
        public IActionResult showPdf(int id)
        {
            _Correspondance c = new _Correspondance();
            c = _correspRepo.GetById(id);
            if (c != null)
            {
                string? num = c.Reference + "_" + c.DateSign + "_" + c.Emetteur + "_To_" + c.Recepteur;
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
       
    }
}

