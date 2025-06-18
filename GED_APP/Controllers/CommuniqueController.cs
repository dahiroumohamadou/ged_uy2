using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace GED_APP.Controllers
{
   // [Authorize]
    public class CommuniqueController : Controller
    {
        private readonly IDocument _communiqueRepo;
        private readonly ICycle _cycleRepo;
        private readonly IFileUploadService _uploadService;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public string type = "CRP";
        public CommuniqueController(IDocument comRepo, ICycle cyclRepo, IFileUploadService fileUploadService, IWebHostEnvironment environment)
        {
            _communiqueRepo = comRepo;
            _cycleRepo = cyclRepo;  
            _uploadService = fileUploadService;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            type = "CRP";
            ICollection<Doc> cs = null;
            cs = _communiqueRepo.GetAllByType(type);
            ViewBag.DataSource = cs;
            return View(cs);

            
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc c = new Doc();
            OnloadCycle();
            if (id == 0)
            {
                return View(new Doc());
            }
            else
            {
                c=_communiqueRepo.GetById(id);
            }
            return View(c);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Numero, Source, DateSign, Objet, Session, AnneeAcademique, TypeDoc, CycleId, Fichier, Signataire")] Doc c)
        {
            int existe = 0;
            Doc cr = new Doc();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                OnloadCycle();
                c.TypeDoc = "CRP";
                int resp;
                
                if (c.Id == 0)
                {
                    cr = _communiqueRepo.ExisteCrp(c.Source, c.Numero, c.DateSign, c.Session, c.AnneeAcademique, c.CycleId);
                    if (cr!=null)
                    {
                        existe = 1;
                        TempData["AlertMessage"] = "Communiqué existe dejà.....";
                        return RedirectToAction("Index");
                    }
                    if (existe == 0)
                    {
                        resp = _communiqueRepo.Add(c);
                        if(resp > 0)
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
                    resp=_communiqueRepo.Update(c);
                    if(resp>0)
                    {
                        TempData["AlertMessage"] = "Mise à jour effectué avec succès.....";
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
            if (id > 0)
            {
                int resp=_communiqueRepo.Delete(id);
                if(resp>0)
                {
                    TempData["AlertMessage"] = "Suppréssion effectué avec succès.....";
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");

        }
        [NonAction]
        public void OnloadCycle()
        {

            List<Cycle> listCycle = new List<Cycle>();
            if (ModelState.IsValid)
            {
                listCycle = (List<Cycle>)_cycleRepo.GetAll();
                if (listCycle != null)
                {
                    Cycle cycleDefault = new Cycle() { Id = 0, Code = "Choisir cycle" };
                    listCycle.Insert(0, cycleDefault);
                    ViewBag.Cycles = listCycle;
                }

            }
        }
        [HttpGet]
        public IActionResult AddPdf(int id)
        {

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc c = new Doc();
            OnloadCycle();
            c=_communiqueRepo.GetById(id);
            return View(c);
        }
        [HttpPost, ActionName("AddPdf")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Numero, Source, DateSign, Objet, Session, AnneeAcademique, TypeDoc, CycleId, Fichier, Signataire")] Doc c, IFormFile pdf)
        {
            
            if (pdf != null)
            {
                if (ModelState.IsValid)
                {
                    OnloadCycle();

                    c.Fichier = 1;
                    // copy fichier sur le serveur
                    path = await _uploadService.UploadPdfFileCommuniqueAsync(pdf, c);
                    int resp=_communiqueRepo.Update(c);
                    if((path != null) && (resp > 0))
                    {
                        TempData["AlertMessage"] = "Document associé avec succès....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        c.Fichier = 0;
                        resp = _communiqueRepo.Update(c);
                        TempData["AlertMessage"] = "Erreur ajout du document.....";
                        return RedirectToAction("Index");
                    }
                    
                }
                return RedirectToAction("Index");
                
            }
            return View();
        }
        [HttpGet]
        public IActionResult showPdf(int id)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc c = new Doc();
            OnloadCycle();
            c=_communiqueRepo.GetById(id);
            if (c != null)
            {
                string? num = "CRP_" + c.Numero + "_Du " + c.DateSign + "_Ses" + c.Session + "_AnneeAcad" + c.AnneeAcademique + "_cycl" + "_source" + c.Source;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/communiques/", replacement + ".pdf");
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
