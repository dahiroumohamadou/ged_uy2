using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class ArreteController : Controller
    {
        private readonly IDocument _arreteRepo;
        private readonly ICycle _cycleRepo;
        private readonly IFileUploadService _uploadService;
        private readonly IWebHostEnvironment _environment;
        public string path;
        public string type = "ARR";
        public ArreteController(IDocument arreteRepo, ICycle cycleRepo, IFileUploadService fileUploadService, IWebHostEnvironment environment)
        {
            _arreteRepo = arreteRepo;
            _cycleRepo = cycleRepo;
            _uploadService = fileUploadService;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
           
            type = "ARR";
            ICollection<Doc> arretes = null;
            arretes=_arreteRepo.GetAllByType(type);
            //arretes = JsonConvert.DeserializeObject<List<Doc>>(arretes);
            ViewBag.DataSource = arretes;
            return View(arretes);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc ar = new Doc();
            OnloadCycle();
            if (id == 0)
            {
                return View(new Doc());
            }
            else
            {
                ar=_arreteRepo.GetById(id);

            }
            return View(ar);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Source, Numero, DateSign, Objet, TypeDoc, AnneeAcademique, CycleId, Fichier, Signataire")] Doc a)
        {
            int existe = 0;
            int res;
            Doc arr = new Doc();

            if (ModelState.IsValid)
            {
                
                OnloadCycle();
                a.TypeDoc = "ARR";
                
                if (a.Id == 0)
                {
                    arr = _arreteRepo.ExisteAr(a.Source, a.Numero, a.DateSign, a.AnneeAcademique, a.CycleId);
                    if(arr != null)
                    {
                        existe = 1;
                        TempData["AlertMessage"] = "Arrete  existe deja .....";
                        return RedirectToAction("Index");
                    }
                    if (existe == 0)
                    {
                        res = _arreteRepo.Add(a);
                        if (res > 0)
                        {
                            TempData["AlertMessage"] = "Enregistrement effectué avec succès...";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Erreur d'enregistrement.....";
                            return RedirectToAction("Index");
                        }
                    }

                }
                else
                {
                    res=_arreteRepo.Update(a);
                    if(res > 0)
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
            int res;
            if (id > 0)
            {
                res=_arreteRepo.Delete(id);
               if(res > 0 )
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
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
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
            int res;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc ar = new Doc();
            OnloadCycle();
            ar = _arreteRepo.GetById(id);
            return View(ar);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Source, Numero, DateSign, Objet, TypeDoc, AnneeAcademique, CycleId, Fichier, Signataire")] Doc a, IFormFile pdf)
        {
            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    OnloadCycle();
                    a.Fichier = 1;
                    // copy fichier sur le serveur
                    path = await _uploadService.UploadPdfFileArreteAsync(pdf, a);
                    int res =_arreteRepo.Update(a);

                    if ((res > 0) && (path != null))
                    {
                        // copy fichier sur le serveur
                        path = await _uploadService.UploadPdfFileArreteAsync(pdf, a);
                        TempData["AlertMessage"] = "Document associé avec succès.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        a.Fichier = 0;
                        res = _arreteRepo.Update(a);
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
            OnloadCycle();
            aa=_arreteRepo.GetById(id);
            if (aa!=null)
            { 
                string? num = "Arr_" + aa.Numero + "_Du " + aa.DateSign + "_Source" + aa.Source;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/arretes/", replacement + ".pdf");
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
