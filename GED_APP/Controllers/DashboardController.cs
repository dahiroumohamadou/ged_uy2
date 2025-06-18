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
        private readonly IDocument _docRepo;
        public DashboardController(IDocument docRepo)
        {
          _docRepo = docRepo;

        } 
        public IActionResult Index()
        {
            var email = Convert.ToString(HttpContext.User.FindFirstValue("UserEmail"));
            ViewBag.Id = email;
            ICollection<Doc> documents = new List<Doc>();
            try
            {
                documents = _docRepo.GetAll();
               
                if (documents!=null)
                {
                    ViewBag.arretes = documents.Where(d => d.TypeDoc == "ARR").Count();
                    ViewBag.pvs = documents.Where(d => d.TypeDoc == "PV").Count();
                    ViewBag.communiques = documents.Where(d => d.TypeDoc == "CRP").Count();
                    ViewBag.DossiersChartData = documents
                                                            //.Where(d => d.Created. == DateTime.Today.Year.ToString())
                                                            .GroupBy(j => j.TypeDoc)
                                                            .Select(k => new
                                                            {
                                                                type = k.First().TypeDoc,
                                                                nbeDossiers = k.Count()
                                                            })
                                                            .ToList();
                    ViewBag.DossiersChartDataSpline1 = documents
                                                .Where(d => d.Source == "ENS-YDE")
                                                .GroupBy(j => j.TypeDoc)
                                                .Select(k => new
                                                {
                                                    type = k.First().TypeDoc,
                                                    nbeDossiers = k.Count()
                                                })
                                                .ToList();
                    ViewBag.DossiersChartDataSpline2 = documents
                                               .Where(d => d.Source == "ENS-BAMBILI")
                                               .GroupBy(j => j.TypeDoc)
                                               .Select(k => new
                                               {
                                                   type = k.First().TypeDoc,
                                                   nbeDossiers = k.Count()
                                               })
                                               .ToList();
                    ViewBag.DossiersRecent = documents
                                              .OrderByDescending(d => d.Created)
                                              .Take(5)
                                              .ToList();
                }
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
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
