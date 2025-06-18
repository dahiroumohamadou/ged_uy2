using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class AutreController : Controller
    {
        private readonly IDocument _autreRepo;
        private readonly IFileUploadService _uploadService;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public string type = "AUTRE";
        public AutreController(IDocument autreRep, IFileUploadService fileUploadService, IWebHostEnvironment environment)
        {
            _autreRepo = autreRep;
            _uploadService = fileUploadService;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Doc> autres = null;
            autres = _autreRepo.GetAllByType(type);
            ViewBag.DataSource = autres;
            return View(autres);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc autre = new Doc();
            if (id == 0)
            {
                return View(new Doc());
            }
            else
            {
                autre=_autreRepo.GetById(id);
            }
            return View(autre);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Source, Objet, DateSign, TypeDoc, Fichier, Signataire")] Doc autre)
        {
            int existe = 0;
            int resp;
            Doc other = new Doc();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                autre.TypeDoc = "AUTRE";
                
                if (autre.Id == 0)
                {
                    other = _autreRepo.ExisteOthers(autre.Source, autre.Numero, autre.DateSign);
                    if (other != null)
                    {
                        existe=1;
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    
                    if (existe == 0)
                    {
                        resp = _autreRepo.Add(autre);
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
                    resp=_autreRepo.Update(autre);
                    if (resp>0)
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
                resp=_autreRepo.Delete(id);
                if(resp > 0)
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
            Doc autre = new Doc();
            autre=_autreRepo.GetById(id);
            return View(autre);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Source, Objet, DateSign, TypeDoc, Fichier, Signataire")] Doc a, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    a.Fichier = 1;
                    // copy fichier sur le serveur
                    path = await _uploadService.UploadPdfFileAutreAsync(pdf, a);
                    int resp=_autreRepo.Update(a);
                    if((resp > 0) && (path!= null)){
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.Fichier = 0;
                        resp = _autreRepo.Update(a);
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
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc aa = new Doc();
            aa = _autreRepo.GetById(id);
            if (aa != null)
            {
                string? num = "Autre_" + aa.Numero + "_Du " + aa.DateSign;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/autres/", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }
            
            return View(aa);
        }
    }
}
