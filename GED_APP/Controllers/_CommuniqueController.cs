using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class _CommuniqueController : Controller
    {
        private readonly _ICommunique _commRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _CommuniqueController(_ICommunique comRepo, IWebHostEnvironment environment)
        {
            _commRepo = comRepo;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Communique> crs = null;
            crs = _commRepo.GetAll();
            ViewBag.DataSource = crs;
            return View(crs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Communique cr = new _Communique();

            if (id == 0)
            {
                return View(new _Communique());
            }
            else
            {
                cr = _commRepo.GetById(id);
            }
            return View(cr);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _Communique c)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (c.Id == 0)
                {
                    existe = _commRepo.Existe(c);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce communiqué existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _commRepo.Add(c);
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
                    resp = _commRepo.Update(c);
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
                resp = _commRepo.Delete(id);
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
            _Communique c = new _Communique();
            c = _commRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated")] _Communique c, IFormFile pdf)
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
                    path = await _commRepo.UploadPdfFileCommuniqueAsync(pdf, c);
                    int resp = _commRepo.Update(c);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.Status = 0;
                        resp = _commRepo.Update(c);
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
            _Communique c = new _Communique();
            c = _commRepo .GetById(id);
            if (c != null)
            {
                string? num = c.Numero + "_" + c.DateSign + "_" + c.Objet;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/communiques", replacement + ".pdf");
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
