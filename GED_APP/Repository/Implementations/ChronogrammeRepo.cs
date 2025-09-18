using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GED_APP.Repository.Implementations
{
    public class ChronogrammeRepo:IChronogramme
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ChronogrammeRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(Chronogramme chronogramme)
        {
            int res = -1;
            var c=_context.Chronogrammes
                .Where(chr=>chr.NumeroPTA==chronogramme.NumeroPTA && chr.Origine==chronogramme.Origine && chr.Annee==chronogramme.Annee)
                .FirstOrDefault() ?? null;
            if (c == null)
            {
                if(chronogramme!=null)
                {
                    _context.Chronogrammes.Add(chronogramme);
                    _context.SaveChanges();
                    res=chronogramme.Id;
                }
            } 
            return res;

        }

        public int Delete(int id)
        {
            int res = -1;
            var c=_context.Chronogrammes.Where(chr=>chr.Id==id)
                .FirstOrDefault() ?? null;
            if(c!=null)
            {
                _context.Chronogrammes.Remove(c);
                _context.SaveChanges();
                res= c.Id;
            }
            return res;
        }

        public void Dispose()
        {
           _context?.Dispose();
        }

        public int Existe(Chronogramme chronogramme)
        {
            int res = -1;
            var c = _context.Chronogrammes
                .Where(chr => chr.NumeroPTA == chronogramme.NumeroPTA
                && chr.Annee==chronogramme.Annee 
                && chr.StructureId==chronogramme.StructureId)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res= c.Id;
            }
            return res;
        }

        public ICollection<Chronogramme> GetAll()
        {
            var chrs = _context.Chronogrammes
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return chrs;
        }

        public ICollection<Chronogramme> GetAllByCodeAnne(string code, string annee)
        {
                var chrs = _context.Chronogrammes
                   .Where(a => a.Origine == code && a.Annee == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return chrs;
        }

        public Chronogramme GetById(int id)
        {
            var c = _context.Chronogrammes
                .Where(chr => chr.Id == id)
                .FirstOrDefault() ?? null;
            return c;

        }

       

        public int Update(Chronogramme chronogramme)
        {
            int res = -1;
            var c = _context.Chronogrammes.Where(chr => chr.Id == chronogramme.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.NumeroPTA = chronogramme.NumeroPTA;
                c.Libele = chronogramme.Libele;
                c.Status= chronogramme.Status;
                c.Annee = chronogramme.Annee;
                c.Origine = chronogramme.Origine;
                c.StructureId = chronogramme.StructureId;
                c.Updated=DateTime.Now;
                _context.Chronogrammes.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFileChronoAsync(IFormFile file, Chronogramme c)
        {
            string? num = "Chrono_"+c.NumeroPTA + "_" + c.Origine + "_" + c.Annee;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/chronos", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
        
    }
}
