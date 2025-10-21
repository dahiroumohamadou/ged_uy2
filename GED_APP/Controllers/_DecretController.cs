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
    public class _DecretController : Controller
    {
        private readonly _IDecret _decretRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _DecretController(_IDecret decretRepo, IWebHostEnvironment environment)
        {
            _decretRepo = decretRepo;
            _environment = environment;
        }

        // GET: DechargeController
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Decret> ds = null;
            ds = _decretRepo.GetAll();
            ViewBag.DataSource = ds;
            return View(ds);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Decret d = new _Decret();
            if (id == 0)
            {
                return View(new _Decret());
            }
            else
            {
                d = _decretRepo.GetById(id);
            }
            return View(d);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated, Code")] _Decret d)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {

                if (d.Id == 0)
                {
                    existe = _decretRepo.Existe(d);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {

                        resp = _decretRepo.Add(d);
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
                    resp = _decretRepo.Update(d);
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
                resp = _decretRepo.Delete(id);
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
            _Decret d = new _Decret();
            d = _decretRepo.GetById(id);
            return View(d);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated, Code")] _Decret d, IFormFile pdf)
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
                    path = await _decretRepo.UploadPdfFileDecretAsync(pdf, d);
                    int resp = _decretRepo.Update(d);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        d.Status = 0;
                        resp = _decretRepo.Update(d);
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
            _Decret d = new _Decret();
            d = _decretRepo.GetById(id);
            if (d != null)
            {
                string? num = d.Numero + "_" + d.DateSign + "_" + d.Objet;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decrets", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(d);
        }
    }
}
