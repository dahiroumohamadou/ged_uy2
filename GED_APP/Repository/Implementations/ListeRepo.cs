using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class ListeRepo:IListe
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ListeRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public int Add(Liste li)
        {
            int res = -1;
            var l = _context.Listes
                .Where(lis => lis.NumeroCne == li.NumeroCne && lis.DateCne == li.DateCne && lis.Numero==li.Numero && lis.DateSign==li.DateSign)
                .FirstOrDefault() ?? null;
            if (l== null)
            {
                if (li != null)
                {
                    _context.Listes.Add(li);
                    _context.SaveChanges();
                    res = li.Id;
                }
            }
            return res;

        }

        public int Delete(int id)
        {
            int res = -1;
            var l = _context.Listes.Where(li => li.Id == id)
                .FirstOrDefault() ?? null;
            if (l != null)
            {
                _context.Listes.Remove(l);
                _context.SaveChanges();
                res = l.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Liste li)
        {
            int res = -1;
            var l = _context.Listes
                .Where(lis => lis.NumeroCne == li.NumeroCne && lis.DateCne == li.DateCne && lis.Numero == li.Numero && lis.DateSign == li.DateSign)
                .FirstOrDefault() ?? null;
            if (l != null)
            {
                res = l.Id;
            }
            return res;
        }

        public ICollection<Liste> GetAll()
        {
            var listes = _context.Listes
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return listes;
        }

        public ICollection<Liste> GetAllByCodeAnne(string code, string annee)
        {
             
             var listes = _context.Listes
                .Where(a => a.Origine == code && a.DateSign.Substring(6) == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return listes;
        }

        public Liste GetById(int id)
        {
            var l = _context.Listes
                .Where(ll => ll.Id == id)
                .FirstOrDefault() ?? null;
            return l;

        }



        public int Update(Liste li)
        {
            int res = -1;
            var l = _context.Listes.Where(ll => ll.Id == li.Id)
               .FirstOrDefault() ?? null;
            if (l != null)
            {
                l.Numero = li.Numero;
                l.NumeroCne = li.NumeroCne;
                l.DateCne = li.DateCne;
                l.DateSign = li.DateSign;
                l.Origine = li.Origine;
                l.Updated = DateTime.Now;
                l.Descritpion = li.Descritpion;
                l.StructureId = li.StructureId;
                _context.Listes.Update(l);
                _context.SaveChanges();
                res = l.Id;
            }
            return res;

        }

      

        public async Task<string> UploadPdfFileListeAsync(IFormFile file, Liste l)
        {
            string? num = "Liste_" + l.Numero + "_du_" + l.DateSign + "_session_" + l.NumeroCne;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/Listes", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
