using GED_APP.Models;
using GED_APP.Repository.Implementations;
using GED_APP.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class StructureController : Controller
    {
        private readonly IStructure _structureRepo;
        public StructureController(IStructure structureRepo)
        {
            _structureRepo = structureRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<Structure> list = null;
            list = _structureRepo.GetAll();
            ViewBag.DataSource = list;
            return View(list);
        }
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            Structure s = new Structure();
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            if (id == 0)
            {
                return View(new Structure());
            }
            else
            {
                s = _structureRepo.GetById(id);
            }
            return View(s);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("Id, Code, Libele, Created, Updated")] Structure s)
        {
            if (ModelState.IsValid)
            {
                int resp;
                if (s.Id == 0)
                {
                    int existe = _structureRepo.Existe(s);
                    if (existe < 0) {
                        resp = _structureRepo.Add(s);
                        if (resp > 0)
                        {
                            TempData["AlertMessage"] = "Enregistrement effectué avec succèss.....";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Erreur d'enregistrement  .....";
                            return RedirectToAction("Index");
                        }
                    }
                    
                }
                else
                {
                    resp = _structureRepo.Update(s);
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
                int resp = _structureRepo.Delete(id);
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
