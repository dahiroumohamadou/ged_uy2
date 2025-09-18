using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class RapportRepo:IRapport
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public RapportRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(Rapport r)
        {
            int res = -1;
            var rp=_context.Rapports
                .Where(rr=>rr.Description==r.Description && rr.Debut==r.Debut && rr.Fin==r.Fin && rr.Origine==r.Origine)
                .FirstOrDefault() ?? null;
            if (rp==null)
            {
                if(r!=null)
                {
                    _context.Rapports.Add(r);
                    _context.SaveChanges();
                    res = r.Id;
                }
            }
            return res;
           
        }

        public int Delete(int id)
        {
            int res = -1;
            var rp=_context.Rapports.Where(r=>r.Id==id).FirstOrDefault() ?? null;
            if(rp!=null)
            {
                _context.Rapports.Remove(rp);
                _context.SaveChanges();
                res = rp.Id;
            }
            return res;
        }

        public void Dispose()
        {
           _context?.Dispose();
        }

        public int Existe(Rapport r)
        {
            int res = -1;
            var rp = _context.Rapports
                .Where(rr => rr.Origine == r.Origine && rr.Description==r.Description && rr.Debut==r.Debut && rr.Fin==r.Fin)
                .FirstOrDefault() ?? null;
            if (rp != null)
            {
                res= rp.Id;
            }
            return res;
        }

        public ICollection<Rapport> GetAll()
        {
            var rps = _context.Rapports
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return rps;
        }

        public ICollection<Rapport> GetAllByCodeAnne(string code, string annee)
        {
            
            var rps = _context.Rapports
                .Where(a => a.Origine == code && a.Debut.Substring(6) == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return rps;
        }

        public Rapport GetById(int id)
        {
            var rp = _context.Rapports
                .Where(r => r.Id == id).FirstOrDefault() ?? null;
            return rp;
        }

        public int Update(Rapport r)
        {
            int res = -1;
            var rp = _context.Rapports.Where(rr=>rr.Id==r.Id).FirstOrDefault() ?? null;
            if (rp != null)
            {
                rp.Description=r.Description;
                rp.Debut=r.Debut;
                rp.Fin=r.Fin;
                rp.Origine=r.Origine;
                rp.Updated=DateTime.Now;
                rp.StructureId=r.StructureId;
                rp.Status=r.Status;
                _context.Rapports.Update(rp);
                _context.SaveChanges();
                res=rp.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileRapportAsync(IFormFile file, Rapport r)
        {
            string? num = "Rapport_" + r.Origine + "_" + r.Periode;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/rapports", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
