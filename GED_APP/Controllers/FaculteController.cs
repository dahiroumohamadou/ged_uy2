using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class FaculteController : Controller
    {
        private readonly IFaculte _faculteRepo;
        public FaculteController(IFaculte faculteRepo)
        {
            _faculteRepo = faculteRepo;
        }
        public IActionResult Index()
        {
            ICollection<Faculte> list = null;
            list = _faculteRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Faculte f = new Faculte();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Faculte());
            }
            else
            {
                f = _faculteRepo.GetById(id);
            }
            return View(f);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Libele, Created, Updated")] Faculte f)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (f.Id == 0)
                {
                    int existe = _faculteRepo.Existe(f);
                    if (existe < 0)
                    {
                        resp = _faculteRepo.Add(f);
                        if (resp > 0)
                        {
                            TempData["AlertMessage"] = "Enregistrement effectué avec succèss.....";
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
                    resp = _faculteRepo.Update(f);
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
                int resp = _faculteRepo.Delete(id);
                if (resp > 0)
                {
                    TempData["AlertMessage"] = "Suppréssion effectuée avec succès.....";
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");

        }
    }
}
