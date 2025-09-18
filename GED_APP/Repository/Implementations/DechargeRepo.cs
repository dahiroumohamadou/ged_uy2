using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class DechargeRepo : IDecharge
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public DechargeRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public int Add(Decharge dos)
        {
           int res = -1;
            var d=_context.Decharges.Where(dd=>dd.Origine==dos.Origine && dd.Date==dos.Date && dd.Description==dos.Description).FirstOrDefault() ?? null ;
            if (d == null)
            {
                if (dos != null)
                {
                    _context.Decharges.Add(dos);
                    _context.SaveChanges();
                    res = dos.Id;
                }
            }
            return res;

        }
        public int Existe(Decharge dec)
        {
            int res = -1;
            var d = _context.Decharges.Where(dd => dd.Origine == dec.Origine && dd.Date == dec.Date && dd.Description == dec.Description).FirstOrDefault() ?? null;
            if (d!= null)
            {
                res= d.Id;
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var d = _context.Decharges.Where(dd => dd.Id == id)
                .FirstOrDefault() ?? null;
            if (d != null)
            {
                _context.Decharges.Remove(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }

       

        public ICollection<Decharge> GetAll()
        {
            var ds = _context.Decharges
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return ds;
        }

        public Decharge GetById(int id)
        {
            var d = _context.Decharges
                .Where(dd => dd.Id == id)
                .FirstOrDefault() ?? null;
            return d;

        }



        public int Update(Decharge decharge)
        {
            int res = -1;
            var d = _context.Decharges.Where(dd => dd.Id == decharge.Id)
               .FirstOrDefault() ?? null;
            if (d != null)
            {
                d.Description = decharge.Description;
                d.Status = decharge.Status;
                d.Date = decharge.Date;
                d.Origine = decharge.Origine;
                d.StructureId = decharge.StructureId;
                d.Updated = DateTime.Now;
                _context.Decharges.Update(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFileDechargeAsync(IFormFile file, Decharge d)
        {
            string? num = d.Origine + "_Du_" + d.Date + "_" + d.Description;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decharges", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

        public ICollection<Decharge> GetAllByCodeAnne(string code, string annee)
        {
           
            var ds = _context.Decharges
                 .Where(a => a.Origine == code && a.Date.Substring(6) == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return ds;
        }
    }
}
