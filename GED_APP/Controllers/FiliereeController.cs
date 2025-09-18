using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class FiliereeController : Controller
    {
        private readonly IFilieree _filiereeRepo;
        public FiliereeController(IFilieree filiereeRepo)
        {
            _filiereeRepo = filiereeRepo;
        }
        public IActionResult Index()
        {
            ICollection<Filieree> list = null;
            list = _filiereeRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Filieree f = new Filieree();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Filieree());
            }
            else
            {
                f = _filiereeRepo.GetById(id);
            }
            return View(f);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Description, Created, Updated")] Filieree f)
        {
            if (ModelState.IsValid)
            {
                int resp; 
                if (f.Id == 0)
                {
                    int existe = _filiereeRepo.Existe(f);
                    if (existe < 0)
                    {
                        resp = _filiereeRepo.Add(f);
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
                    resp = _filiereeRepo.Update(f);
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
                int resp = _filiereeRepo.Delete(id);
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
