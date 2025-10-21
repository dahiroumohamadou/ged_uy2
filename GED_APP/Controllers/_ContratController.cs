using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    [Authorize]
    public class _ContratController : Controller
    {
        private readonly _IContrat _contratRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _ContratController(_IContrat contratRepo, IWebHostEnvironment environment)
        {
            _contratRepo = contratRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Contrat> crs = null;
            crs = _contratRepo.GetAll();
            ViewBag.DataSource = crs;
            return View(crs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Contrat cr = new _Contrat();
           
            if (id == 0)
            {
                OnloadType();
                return View(new _Contrat());
            }
            else
            {
                cr = _contratRepo.GetById(id);
            }
            return View(cr);

        }
        
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Type, Beneficiaire, Signataire, DateSign, Status, Code")] _Contrat c)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (c.Id == 0)
                {
                    existe = _contratRepo.Existe(c);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce contrat existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _contratRepo.Add(c);
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
                    resp = _contratRepo.Update(c);
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
        [Authorize(Roles ="ADMIN")]
        public IActionResult Delete(int id)
        {
            int resp;
            if (id > 0)
            {
                resp = _contratRepo.Delete(id);
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
            _Contrat c = new _Contrat();
            c = _contratRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Type, Beneficiaire, Signataire, DateSign, Status, Code")] _Contrat c, IFormFile pdf)
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
                    path = await _contratRepo.UploadPdfFileContratAsync(pdf, c);
                    int resp = _contratRepo.Update(c);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Contrat added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.Status = 0;
                        resp = _contratRepo.Update(c);
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
            _Contrat c = new _Contrat();
            c = _contratRepo.GetById(id);
            if (c != null)
            {
                string? num = c.Numero + "_" + c.Type + "_" + c.DateSign + "_To_" + c.Beneficiaire;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/contrat", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(c);
        }
        [NonAction]
        public void OnloadType()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> types = new List<string>();
            if (ModelState.IsValid)
            {
                types.Insert(0, "Choisir type");
                types.Insert(1, "Travail");
                types.Insert(2, "Collaboration");
                types.Insert(3, "Encadrement");
                ViewBag.Types = types;
            }
        }
    }
}
