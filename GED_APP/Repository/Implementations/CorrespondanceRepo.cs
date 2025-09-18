using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace GED_APP.Repository.Implementations
{
    public class CorrespondanceRepo:ICorrespondance
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public CorrespondanceRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public int Add(Correspondance cor)
        {
            int res = -1;
            var c = _context.Correspondances
                .Where(cc => cc.Type == cor.Type && cc.StructureId == cor.StructureId && cc.Date == cor.Date && cc.Origine == cor.Origine)
                .FirstOrDefault() ?? null;
            if (c == null)
            {
                if (cor != null)
                {
                    _context.Correspondances.Add(cor);
                    _context.SaveChanges();
                    res = cor.Id;
                }
            }
            return res;

        }

       

        public int Delete(int id)
        {
            int res = -1;
            var c = _context.Correspondances.Where(cr => cr.Id == id)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                _context.Correspondances.Remove(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Correspondance cor)
        {
            int res = -1;
            var c = _context.Correspondances
                .Where(cc => cc.Type == cor.Type && cc.StructureId == cor.StructureId && cc.Date == cor.Date && cc.Origine == cor.Origine)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }
        public ICollection<Correspondance> GetAll()
        {
            var crs = _context.Correspondances
                .OrderByDescending(c => c.Created)
                .Include(c=>c.Structure)
                .ToList();
            return crs;
        }

        public ICollection<Correspondance> GetAllByCodeAnne(string code, string annee)
        {
           
             var crs = _context.Correspondances
                 .Where(a => a.Origine == code && a.Date.Substring(6) == annee)
                .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return crs;
        }

        public Correspondance GetById(int id)
        {
            var c = _context.Correspondances
                .Include(c=>c.Structure)
                .Where(cr => cr.Id == id)
                .FirstOrDefault() ?? null;
            return c;

        }



        public int Update(Correspondance cr)
        {
            int res = -1;
            var c = _context.Correspondances.Where(cor => cor.Id == cr.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Type = cr.Type;
                c.Numero = cr.Numero;
                c.Status = cr.Status;
                c.Initiateur= cr.Initiateur;
                c.Objet = cr.Objet;
                c.Date = cr.Date;
                c.Origine = cr.Origine;
                c.StructureId = cr.StructureId;
                c.Updated = DateTime.Now;
                _context.Correspondances.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFileCorrespAsync(IFormFile file, Correspondance c)
        {
            string? num = c.Type + "_" + c.Numero + "_" + c.Date + "_" + c.Objet + "_To_" + c.Origine;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/corresp", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }


        

    }
}
