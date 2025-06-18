using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace GED_APP.Controllers
{
    //[Authorize]
    public class FiliereController : Controller
    {
      
        private readonly IFiliere _filiereRepo;
        public FiliereController(IFiliere filiereRepo)
        {
            _filiereRepo = filiereRepo;
           
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Filiere> list = null;
            list = _filiereRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Filiere f = new Filiere();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Filiere());
            }
            else
            {
                f=_filiereRepo.GetById(id);
            }
            return View(f);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Libele")] Filiere f)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (f.Id == 0)
                {
                   resp=_filiereRepo.Add(f);
                   if(resp>0) 
                    { 
                        TempData["AlertMessage"] = "Enregistrement effectué avec succès.....";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["AlertMessage"] = "Erreur d'enregistrement ou Filiere existe .....";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    resp = _filiereRepo.Update(f);
                    if (resp > 0)
                    {
                        TempData["AlertMessage"] = "Mise à jour effectuée avec succès.....";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(f);
        }
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken] 
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                int res=_filiereRepo.Delete(id);
                if(res>0)
                {
                    TempData["AlertMessage"] = "Suppréssion effectuée avec succèss.....";
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");

        }
    }
}
