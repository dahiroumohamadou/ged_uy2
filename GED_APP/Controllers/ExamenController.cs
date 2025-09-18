using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class ExamenController : Controller
    {
        private readonly IExamen _examenRepo;
        public ExamenController(IExamen examenRepo)
        {
            _examenRepo = examenRepo;
        }
        public IActionResult Index()
        {
            ICollection<Examen> list = null;
            list = _examenRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Examen e = new Examen();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Examen());
            }
            else
            {
                e = _examenRepo.GetById(id);
            }
            return View(e);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Nom, Created, Updated")] Examen e)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (e.Id == 0)
                {
                    int existe = _examenRepo.Existe(e);
                    if (existe < 0)
                    {
                        resp = _examenRepo.Add(e);
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
                    resp = _examenRepo.Update(e);
                    if (resp > 0)
                    {
                        TempData["AlertMessage"] = "Mise à jour effectuée avec succès.....";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(e);
        }
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            if (id > 0)
            {
                int resp = _examenRepo.Delete(id);
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
