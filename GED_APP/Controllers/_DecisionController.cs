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
    public class _DecisionController : Controller
    {
        private readonly _IDecision _decisionRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _DecisionController(_IDecision decisionRepo, IWebHostEnvironment environment)
        {
            _decisionRepo = decisionRepo;
            _environment = environment;
        }

        // GET: DechargeController
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Decision> ds = null;
            ds = _decisionRepo.GetAll();
            ViewBag.DataSource = ds;
            return View(ds);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Decision d = new _Decision();
            OnloadType();
            if (id == 0)
            {
                return View(new _Decision());
            }
            else
            {
                d = _decisionRepo.GetById(id);
            }
            return View(d);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Objet, Type, Signataire, DateSign, Status, Updated, Code")] _Decision d)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {

                if (d.Id == 0)
                {
                    existe = _decisionRepo.Existe(d);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {

                        resp = _decisionRepo.Add(d);
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
                    resp = _decisionRepo.Update(d);
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
        [Authorize(Roles = "ADMIN")]
        //[ValidateAntiForgeryToken] 
        public IActionResult Delete(int id)
        {
            int resp;
            if (id > 0)
            {
                resp = _decisionRepo.Delete(id);
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
            OnloadType();
            _Decision d = new _Decision();
            d = _decisionRepo.GetById(id);
            return View(d);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Objet, Type, Signataire, DateSign, Status, Updated, Code")] _Decision d, IFormFile pdf)
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
                    path = await _decisionRepo.UploadPdfDecisionAsync(pdf, d);
                    int resp = _decisionRepo.Update(d);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        d.Status = 0;
                        resp = _decisionRepo.Update(d);
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
            OnloadType();
            _Decision d = new _Decision();
            d = _decisionRepo.GetById(id);
            if (d != null)
            {
                string? num = d.Numero + "_" + d.Type + "_" + d.DateSign + "_" + d.Objet;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decision", replacement + ".pdf");
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
        public void OnloadType()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> types = new List<string>();
            if (ModelState.IsValid)
            {
                types.Insert(0, "Choisir type");
                types.Insert(1, "Additive");
                types.Insert(2, "Deblocage");
                types.Insert(3, "Autorisation");
                types.Insert(4, "Nommination");
                types.Insert(5, "Designation");
                types.Insert(6, "Autre");
                ViewBag.Types = types;
            }
        }
    }
}
