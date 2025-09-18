using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class PvCneRepo:IPvCne
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public PvCneRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public int Add(PvCNE ppv)
        {
            int res = -1;
            var pv = _context.PvCNEs
                .Where(p => p.NumeroCne == ppv.NumeroCne && p.DateCne == ppv.DateCne)
                .FirstOrDefault() ?? null;
            if (pv == null)
            {
                if (ppv != null)
                {
                    _context.PvCNEs.Add(ppv);
                    _context.SaveChanges();
                    res = ppv.Id;
                }
            }
            return res;

        }

        public int Delete(int id)
        {
            int res = -1;
            var pv = _context.PvCNEs.Where(p => p.Id == id)
                .FirstOrDefault() ?? null;
            if (pv != null)
            {
                _context.PvCNEs.Remove(pv);
                _context.SaveChanges();
                res = pv.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(PvCNE ppv)
        {
            int res = -1;
            var pv = _context.PvCNEs
               .Where(p => p.NumeroCne == ppv.NumeroCne && p.DateCne == ppv.DateCne)
               .FirstOrDefault() ?? null;
            if (pv != null)
            {
                res = pv.Id;
            }
            return res;
        }

        public ICollection<PvCNE> GetAll()
        {
            var pvs = _context.PvCNEs
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return pvs;
        }

        public ICollection<PvCNE> GetAllByCodeAnne(string code, string annee)
        {
            
             var pvs = _context.PvCNEs
                .Where(a => a.Origine == code && a.DateCne.Substring(6) == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return pvs;
        }

        public PvCNE GetById(int id)
        {
            var pv = _context.PvCNEs
                .Where(p => p.Id == id)
                .FirstOrDefault() ?? null;
            return pv;

        }



        public int Update(PvCNE pp)
        {
            int res = -1;
            var p = _context.PvCNEs.Where(pv => pv.Id == pp.Id)
               .FirstOrDefault() ?? null;
            if (p != null)
            {
                p.Description = pp.Description;
                p.Origine = pp.Origine;
                p.StructureId = pp.StructureId;
                p.NumeroCne = pp.NumeroCne;
                p.DateCne = pp.DateCne;
                p.Status = pp.Status;
                p.Updated = DateTime.Now;
                _context.PvCNEs.Update(p);
                _context.SaveChanges();
                res = p.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFilePvCneAsync(IFormFile file, PvCNE p)
        {
            string? num = "Pv_CNE" + p.NumeroCne + "_" + p.DateCne;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/PvCne", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}

