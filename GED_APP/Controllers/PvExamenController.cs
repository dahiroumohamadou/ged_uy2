using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GED_APP.Controllers
{
    public class PvExamenController : Controller
    {
        private readonly IPvExamen _pvRepo;
        private readonly ISource _soureRepo;
        private readonly IFilieree _filiereRepo;
        private readonly IFaculte _faculteRepo;
        private readonly IExamen _examenRepo;
        private readonly IStructure _structureRepo;
        private readonly IWebHostEnvironment _environment;
        public string? path;
        public PvExamenController(IPvExamen pvRepo, ISource soureRepo, IFilieree filiereRepo, IFaculte faculteRepo, IExamen examenRepo, IStructure structureRepo, IWebHostEnvironment environment)
        {
            _pvRepo = pvRepo;
            _soureRepo = soureRepo;
            _filiereRepo = filiereRepo;
            _faculteRepo = faculteRepo;
            _examenRepo = examenRepo;
            _structureRepo = structureRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<PvExamen> pvs = null;
            pvs = _pvRepo.GetAll();
            ViewBag.DataSource = pvs;
            return View(pvs);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            PvExamen pv = new PvExamen();
            OnloadStructure();
            OnloadExamens();
            OnloadFacultes();
            OnloadFiliere();
            OnloadSource();
            if (id == 0)
            {
                return View(new PvExamen());
            }
            else
            {
                pv= _pvRepo.GetById(id);
            }
            return View(pv);

        }
        [HttpPost, ActionName("AddOrEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id, SourceId, FiliereId, FaculteId, ExamenId, Session, Updated, Status, StructureId")] PvExamen p)
        {
            int existe = 0;
            int resp;
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (ModelState.IsValid)
            {

                if (p.Id == 0)
                {
                    existe = _pvRepo.Existe(p);
                    if (existe > 0)
                    {
                        TempData["AlertMessage"] = "Document existe deja.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        resp = _pvRepo.Add(p);
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
                    resp = _pvRepo.Update(p);
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
                resp = _pvRepo.Delete(id);
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
            PvExamen p = new PvExamen();
            p = _pvRepo.GetById(id);
            return View(p);
        }
        [HttpPost, ActionName("AddPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPdf([Bind("Id, SourceId, FiliereId, FaculteId, ExamenId, Updated, Status, Session, StructureId")] PvExamen p, IFormFile pdf)
        {

            if (pdf != null)
            {
                // update fichier
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                if (ModelState.IsValid)
                {
                    p.Status = 1;
                    p.Updated = DateTime.Now;
                    // copy fichier sur le serveur
                    path = await _pvRepo.UploadPdfFilePvExamenAsync(pdf, p);
                    int resp = _pvRepo.Update(p);
                    if ((resp > 0) && (path != null))
                    {
                        TempData["AlertMessage"] = "Document added successfully.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        p.Status = 0;
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
            PvExamen p = new PvExamen();
            p = _pvRepo.GetById(id);
            if (p != null)
            {
                string? num = "Pv_" + p.ExamenId + "_" + p.FiliereId + "_" + p.FaculteId + "_" + p.SourceId + "_" + p.Session;
                var replacement = num.Replace('/', '_');
                replacement = replacement.Replace(' ', '_');
                // convert and copy
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/PvExamens", replacement + ".pdf");
                WebClient client = new WebClient();
                byte[] FileBuffer = client.DownloadData(filePath);
                if (FileBuffer != null)
                {
                    return File(FileBuffer, "application/pdf");

                }
            }

            return View(p);
        }
        [NonAction]
        public void OnloadStructure()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Structure> structures = new List<Structure>();
            if (ModelState.IsValid)
            {
                structures = (List<Structure>)_structureRepo.GetAll();
                if (structures != null)
                {
                    Structure StructureDefault = new Structure() { Id = 0, Libele = "Choisir structure" };
                    structures.Insert(0, StructureDefault);
                    ViewBag.Structures = structures;
                }
            }
        }
        [NonAction]
        public void OnloadExamens()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Examen> exs = new List<Examen>();
            if (ModelState.IsValid)
            {
                exs = (List<Examen>)_examenRepo.GetAll();
                if (exs != null)
                {
                    Examen ExamenDefault = new Examen() { Id = 0, Code = "Choisir le type d'examen" };
                    exs.Insert(0, ExamenDefault);
                    ViewBag.Examens = exs;
                }
            }
        }
        [NonAction]
        public void OnloadFiliere()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Filieree> fls = new List<Filieree>();
            if (ModelState.IsValid)
            {
                fls = (List<Filieree>)_filiereRepo.GetAll();
                if (fls != null)
                {
                    Filieree filiereDefault = new Filieree() { Id = 0, Code = "Choisir Filiere" };
                    fls.Insert(0, filiereDefault);
                    ViewBag.Filieres = fls;
                }
            }
        }
        [NonAction]
        public void OnloadFacultes()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Faculte> facs = new List<Faculte>();
            if (ModelState.IsValid)
            {
                facs = (List<Faculte>)_faculteRepo.GetAll();
                if (facs != null)
                {
                    Faculte facDefault = new Faculte() { Id = 0, Libele = "Choisir faculte" };
                    facs.Insert(0, facDefault);
                    ViewBag.Facultes = facs;
                }
            }
        }
        [NonAction]
        public void OnloadSource()
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<Source> srs = new List<Source>();
            if (ModelState.IsValid)
            {
                srs = (List<Source>)_soureRepo.GetAll();
                if (srs != null)
                {
                    Source SourceDefault = new Source() { Id = 0, Libele = "Choisir source du diplôme" };
                    srs.Insert(0, SourceDefault);
                    ViewBag.Sources = srs;
                }
            }
        }

    }
}
