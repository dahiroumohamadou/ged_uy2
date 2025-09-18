using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class SourceController : Controller
    {
        private readonly ISource _sourceRepo;
        public SourceController(ISource sourceRepo)
        {
            _sourceRepo = sourceRepo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Source> list = null;
            list = _sourceRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Source s = new Source();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Source());
            }
            else
            {
                s = _sourceRepo.GetById(id);
            }
            return View(s);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Libele, Created, Updated")] Source s)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (s.Id == 0)
                {
                    int existe = _sourceRepo.Existe(s);
                    if (existe < 0)
                    {
                        resp = _sourceRepo.Add(s);
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
                    resp = _sourceRepo.Update(s);
                    if (resp > 0)
                    {
                        TempData["AlertMessage"] = "Mise à jour effectuée avec succès.....";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(s);
        }
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            if (id > 0)
            {
                int resp = _sourceRepo.Delete(id);
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
