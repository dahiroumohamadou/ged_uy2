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
    public class _EtatPaiementController : Controller
    {
        private readonly _IEtatPaiement _etatRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _EtatPaiementController(_IEtatPaiement etatRepo, IWebHostEnvironment environment)
        {
            _etatRepo = etatRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_EtatPaiement> ats = null;
            ats = _etatRepo.GetAll();
            ViewBag.DataSource = ats;
            return View(ats);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _EtatPaiement at = new _EtatPaiement();
            if (id == 0)
            {
                return View(new _EtatPaiement());
            }
            else
            {
                at = _etatRepo.GetById(id);
            }
            return View(at);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated, Code")] _EtatPaiement a)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (a.Id == 0)
                {
                    existe = _etatRepo.Existe(a);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _etatRepo.Add(a);
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
                    resp = _etatRepo.Update(a);
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
                resp = _etatRepo.Delete(id);
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
            _EtatPaiement a = new _EtatPaiement();
            a = _etatRepo.GetById(id);
            return View(a);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Signataire, DateSign, Status, Updated, Code")] _EtatPaiement a, IFormFile pdf)
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
                    path = await _etatRepo.UploadPdfFileEtatPaiementAsync(pdf, a);
                    int resp = _etatRepo.Update(a);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Attestation added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.Status = 0;
                        resp = _etatRepo.Update(a);
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
            _EtatPaiement e = new _EtatPaiement();
            e = _etatRepo.GetById(id);
            if (e != null)
            {
                string? num = e.Numero + "_" + e.DateSign + "_" + e.Objet;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/etatPaiements", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(e);
        }
    }
}
