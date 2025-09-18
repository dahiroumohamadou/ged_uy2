using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class ArreteRepo:IArrete
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ArreteRepo(AppDbContext context, IWebHostEnvironment environment)
        {
           _context = context;
            _environment = environment;
        }

        public int Add(Arrete ar)
        {
            int res = -1;
            var a = _context.Arretes
                .Where(aa => aa.NumeroCNE == ar.NumeroCNE && aa.DateCne == ar.DateCne)
                .FirstOrDefault() ?? null;
            if (a == null)
            {
                if (ar != null)
                {
                    _context.Arretes.Add(ar);
                    _context.SaveChanges();
                    res = ar.Id;
                }
            }
            return res;

        }

        public int Delete(int id)
        {
            int res = -1;
            var a = _context.Arretes.Where(aa => aa.Id == id)
                .FirstOrDefault() ?? null;
            if (a != null)
            {
                _context.Arretes.Remove(a);
                _context.SaveChanges();
                res = a.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Arrete ar)
        {
            int res = -1;
            var a = _context.Arretes
                .Where(aa => aa.NumeroCNE == ar.NumeroCNE && aa.DateCne == ar.DateCne)
                .FirstOrDefault() ?? null;
            if (a != null)
            {
                res = a.Id;
            }
            return res;
        }

        public ICollection<Arrete> GetAll()
        {
            var arts = _context.Arretes
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return arts;
        }

        public ICollection<Arrete> GetAllByCodeAnne(string code, string annee)
        {
            var arts = _context.Arretes
                .Where(a=>a.Origine==code && a.DateSign.Substring(6)==annee)
                 .OrderByDescending(c => c.Created)
               .Include(c => c.Structure)
               .ToList();
            return arts;
        }

        public Arrete GetById(int id)
        {
            var a = _context.Arretes
                .Where(aa => aa.Id == id)
                .FirstOrDefault() ?? null;
            return a;

        }



        public int Update(Arrete ar)
        {
            int res = -1;
            var a = _context.Arretes.Where(aa => aa.Id == ar.Id)
               .FirstOrDefault() ?? null;
            if (a != null)
            {
                a.Numero=ar.Numero;
                a.NumeroCNE=ar.NumeroCNE;
                a.DateCne=ar.DateCne;
                a.DateSign=ar.DateSign;
                a.Origine=ar.Origine;
                a.Updated = DateTime.Now;
                a.Objet=ar.Objet;
                a.StructureId = ar.StructureId;
                _context.Arretes.Update(a );
                _context.SaveChanges();
                res = a.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFileArreteAsync(IFormFile file, Arrete a)
        {
            string? num = "Arrete_" + a.Numero + "_du_" + a.DateSign + "_session_" + a.NumeroCNE ;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/arretes", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
