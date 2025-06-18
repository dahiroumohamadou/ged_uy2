using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
   // [Authorize]
    public class CycleController : Controller
    {
      
        private readonly ICycle _cycleRepo;
        public CycleController(ICycle cycleRepo)
        {
            _cycleRepo = cycleRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Cycle> list = null;
            list = _cycleRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Cycle cycle = new Cycle();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Cycle());
            }
            else
            {
                cycle = _cycleRepo.GetById(id);
            }
            return View(cycle);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Libele")] Cycle c)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (c.Id == 0)
                {
                   resp = _cycleRepo.Add(c);
                   if(resp>0)
                    {
                        TempData["AlertMessage"] = "Enregistrement effectué avec succèss.....";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Erreur d'enregistrement ou parcours existe dejà dans le système .....";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    resp=_cycleRepo.Update(c);
                    if (resp > 0)
                    { 
                        TempData["AlertMessage"] = "Mise à jour effectuée avec succès.....";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(c);
        }
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            if (id > 0)
            {
                int resp= _cycleRepo.Delete(id);
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
