using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        //private readonly IDocument _docRepo;
        private readonly _ICorrespondance _correspRepo;
        private readonly _ICommunique _commRepo;
        private readonly _IDecision _decisRepo;
        private readonly _IContrat _contratRepo;
        private readonly _IAttestation _attestRepo;
        private readonly _IArrete _arreteRepo;

        public DashboardController(_ICorrespondance correspRepo , _ICommunique commRepo, _IDecision decisRepo, _IContrat contratRepo, _IAttestation attestRepo, _IArrete arreteRepo)
        {
            _correspRepo = correspRepo;
            _commRepo = commRepo;
            _decisRepo = decisRepo;
            _contratRepo = contratRepo;
            _attestRepo = attestRepo;
            _arreteRepo = arreteRepo;
        }
        public IActionResult Index()
        {
            var email = Convert.ToString(HttpContext.User.FindFirstValue("UserEmail"));
            ViewBag.Id = email;
            ICollection<_Correspondance> coresps = new List<_Correspondance>();
            ICollection<_Communique> communiques = new List<_Communique>();
            ICollection<_Decision> decisisons = new List<_Decision>();
            ICollection<_Contrat> contrats = new List<_Contrat>();
            ICollection<_Attestation> attestations = new List<_Attestation>();
            ICollection<_Arrete> arretes=new List<_Arrete>();
            try
            {
                coresps = _correspRepo.GetAll();
                communiques = _commRepo.GetAll();
                decisisons= _decisRepo.GetAll();
                contrats = _contratRepo.GetAll();
                attestations= _attestRepo.GetAll();
                arretes = _arreteRepo.GetAll();

                ViewBag.correspondaces = coresps.Count();
                ViewBag.communiques = communiques.Count();
                ViewBag.arretes = arretes.Count();
             
                    
                    //ViewBag.communiques = documents.Where(d => d.TypeDoc == "CRP").Count();
                    ViewBag.DecisionChartData = decisisons
                                                             //.Where(d => d.Created. == DateTime.Today.Year.ToString())
                                                            //.OrderBy(d => d.DateSortie.Substring(3))
                                                            .GroupBy(j => j.Type)
                                                            .Select(k => new
                                                            {
                                                                type = k.First().Type,
                                                                nbeDossiers = k.Count()
                                                            })
                                                            .ToList();
                    ViewBag.ContratChartDataSpline1 = contrats
                                                .Where(d => d.Type == "Travail")
                                                //.OrderBy(d => d.DateSortie.Substring(3))
                                                .OrderBy(d => d.DateSign.Substring(3))
                                                .GroupBy(j => j.DateSign.Substring(3))
                                                .Select(k => new
                                                {
                                                    type = k.First().DateSign.Substring(3),
                                                    nbeDossiers = k.Count()
                                                })
                                                .ToList();
                   
                    ViewBag.ContratChartDataSpline2 = contrats
                                                .Where(d => d.Type == "Collaboration")
                                                //.OrderBy(d => d.DateSortie.Substring(3))
                                                .OrderBy(d => d.DateSign.Substring(3))
                                                .GroupBy(j => j.DateSign.Substring(3))
                                                .Select(k => new
                                                {
                                                    type = k.First().DateSign.Substring(3),
                                                    nbeDossiers = k.Count()
                                                })
                                                .ToList();
                    ViewBag.ContratChartDataSpline3 = contrats
                                                .Where(d => d.Type == "Encadrement")
                                                //.OrderBy(d => d.DateSortie.Substring(3))
                                                .OrderBy(d => d.DateSign.Substring(3))
                                                .GroupBy(j => j.DateSign.Substring(3))
                                                .Select(k => new
                                                {
                                                    type = k.First().DateSign.Substring(3),
                                                    nbeDossiers = k.Count()
                                                })
                                                .ToList();
                    
                   
                    ViewBag.AttestationRecent = attestations
                                              .OrderByDescending(d => d.Created)
                                              .Take(5)
                                              .ToList();
                
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }

            return View();
        }
        [HttpGet]
        public IActionResult logOut()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.user = "";
            TempData["User"] ="";
            ViewData["isLogin"] = null;
            ViewData["isAccesDenied"] = null;
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
