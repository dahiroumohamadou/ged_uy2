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
    public class PvController : Controller
    {
        private readonly IDocument _pvRepo;
        private readonly ICycle _cycleRepo;
        private readonly IFiliere _filiereRepo;
        private readonly IFileUploadService _uploadService;
        private readonly IWebHostEnvironment _environment;

        private int Id;
        public string path;
        public string type = "PV";
        public PvController(IDocument pvRepo, ICycle cycleRepo, IFiliere filiereRepo, IFileUploadService fileUploadService, IWebHostEnvironment environment)
        {
            _pvRepo = pvRepo;
            _cycleRepo = cycleRepo;
            _filiereRepo = filiereRepo;
            _uploadService = fileUploadService;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            type = "PV";
            ICollection<Doc> pvs = null;
            pvs = _pvRepo.GetAllByType(type);
            ViewBag.DataSource = pvs;
            return View(pvs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc pv = new Doc();
            OnloadCycle();
            OnloadFiliere();
            if (id == 0)
            {
                return View(new Doc());
            }
            else
            {
                pv=_pvRepo.GetById(id);
               
            }
            return View(pv);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult test([Bind("Id, Promotion, AnneeSortie, Source, TypeDoc, Session, CycleId, FiliereId")] Doc p)
        {
            TempData["AlertMessage"] = "Id=" + p.Id + " Promo= " + p.Promotion + " Source= " + p.Source + " type doc = " + p.TypeDoc + " session = " + p.Session + " cycle =" + p.CycleId + " filiere = " + p.FiliereId;
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, Promotion, AnneeSortie, Source, Objet, TypeDoc, Session, CycleId, FiliereId")] Doc p)
        {
            Doc pv = new Doc();
            int existe = 0;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {
                OnloadCycle();
                OnloadFiliere();
                p.TypeDoc = "PV";
                if (p.Id == 0)
                {
                    pv = _pvRepo.ExistePv(p.Source, p.Session, p.Promotion, p.AnneeSortie, p.CycleId, p.FiliereId);
                    if (pv!=null)
                    {
                        existe = 1;
                        TempData["AlertMessage"] = "ce Pv existe dejà.....";
                        return RedirectToAction("Index");
                    }
                    
                    if (existe == 0)
                    {
                        int res = _pvRepo.Add(p);
                        if (res > 0)
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
                    int res=_pvRepo.Update(p);
                    if(res> 0)
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
            if (id > 0)
            {
                int res= _pvRepo.Delete(id);
                if (res > 0)
                {
                    TempData["AlertMessage"] = "Suppression effectuée avec succès.....";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessage"] = "Erreur de suppression.....";
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
        [NonAction]
        public void OnloadFiliere()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Filiere> filieres = new List<Filiere>();
            if (ModelState.IsValid)
            {
                filieres= (List<Filiere>)_filiereRepo.GetAll();
                if(filieres != null)
                {
                    Filiere filiereDefault = new Filiere() { Id = 0, Libele = "Choisir filiere" };
                    filieres.Insert(0, filiereDefault);
                    ViewBag.Filieres = filieres;
                }
            }
        }
        [HttpGet]
        public IActionResult AddPdf(int id)
        {

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            Doc p = new Doc();
            OnloadCycle();
            OnloadFiliere();
            p = _pvRepo.GetById(id);
           
            return View(p);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, Source, Objet, TypeDoc, Session, Fichier, Promotion, AnneeSortie, CycleId, FiliereId")] Doc p, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    OnloadCycle();
                    OnloadFiliere();
                    p.Fichier = 1;
                    // copy fichier sur le serveur
                    path = await _uploadService.UploadPdfFilePvsAsync(pdf, p);
                    int resp = _pvRepo.Update(p);
                    if ((path != null) && (resp>0))
                    {
                        TempData["AlertMessage"] = "Document associé avec succès....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        p.Fichier = 0;
                        resp = _pvRepo.Update(p);
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
            Doc p = new Doc();
            OnloadCycle();
            OnloadFiliere();
            p=_pvRepo.GetById(id);
            if(p != null)
            {
                string? num = "Pv_" + p.Promotion + "_A" + p.AnneeSortie + "_S" + p.Session + "_C" + p.CycleId + "_F" + p.FiliereId + "_Source_" + p.Source;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/pvs/", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }
            return View(p);
        }
        //public Boolean existeFichier(int id)
        //{
        //    Boolean resultat = false;
        //    //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        //    Doc p = new Doc();
        //    OnloadCycle();
        //    OnloadFiliere();
        //    HttpResponseMessage res = _httpClient.GetAsync(_httpClient.BaseAddress + "/Documents/" + id).Result;
        //    if (res.IsSuccessStatusCode)
        //    {
        //        string data = res.Content.ReadAsStringAsync().Result;
        //        p = JsonConvert.DeserializeObject<Doc>(data);
        //        string? num = p.Promotion + "_A" + p.AnneeSortie + "_S" + p.Session + "_C" + p.Cycle.Code + "_F" + p.Filiere.Libele;
        //        var replacement = num.Replace('/', '_');
        //        var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/pvs", replacement + ".pdf");
        //        WebClient client = new WebClient();
        //        byte[] FileBuffer = client.DownloadData(filePath);
        //        if (FileBuffer != null)
        //        {
        //            resultat = true;

        //        }
        //    }
        //    return resultat;
        //}
        public void Alert(string message, string notificationType)
        {
            var msg = "swal('" + notificationType.ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }
    }
}
