using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class _AttestationController : Controller
    {
        private readonly _IAttestation _attestationRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public _AttestationController(_IAttestation attestationRepo, IWebHostEnvironment environment)
        {
            _attestationRepo = attestationRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<_Attestation> ats = null;
            ats = _attestationRepo.GetAll();
            ViewBag.DataSource = ats;
            return View(ats);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _Attestation at = new _Attestation();
            OnloadType();
            if (id == 0)
            {
                return View(new _Attestation());
            }
            else
            {
                at = _attestationRepo.GetById(id);
            }
            return View(at);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Type, Destinataire, Signataire, DateSign, Status, Updated")] _Attestation a)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                if (a.Id == 0)
                {
                    existe = _attestationRepo.Existe(a);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Ce Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _attestationRepo.Add(a);
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
                    resp = _attestationRepo.Update(a);
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
                resp = _attestationRepo.Delete(id);
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
            _Attestation a = new _Attestation();
            a = _attestationRepo.GetById(id);
            return View(a);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Type, Destinataire, Signataire, DateSign, Status, Updated")] _Attestation a, IFormFile pdf)
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
                    path = await _attestationRepo.UploadPdfFileAttestationAsync(pdf, a);
                    int resp = _attestationRepo.Update(a);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Attestation added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.Status = 0;
                        resp = _attestationRepo.Update(a);
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
            OnloadType();
            _Attestation a = new _Attestation();
            a = _attestationRepo.GetById(id);
            if (a != null)
            {
                string? num = a.Numero + "_" + a.DateSign + "_" + a.Type + "_to_" + a.Destinataire;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/attestations", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(a);
        }
        [NonAction]
        public void OnloadType()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<string> types = new List<string>();
            if (ModelState.IsValid)
            {
                types.Insert(0, "Choisir type");
                types.Insert(1, "A.V.I");
                types.Insert(2, "A.P.E");
                types.Insert(3, "Autre");
                ViewBag.Types = types;
            }
        }
    }
}
