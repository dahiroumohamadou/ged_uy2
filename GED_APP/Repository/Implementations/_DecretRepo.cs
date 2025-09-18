using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _DecretRepo:_IDecret
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _DecretRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Decret d)
        {
            int res = -1;
            var dd = _context._Decrets
                .Where(de => de.Numero == d.Numero && de.Objet == d.Objet && de.DateSign == d.DateSign)
                .FirstOrDefault() ?? null;
            if (dd == null)
            {
                if (d != null)
                {
                    _context._Decrets.Add(d);
                    _context.SaveChanges();
                    res = d.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var de = _context._Decrets
                 .Where(d => d.Id == id)
                 .FirstOrDefault() ?? null;
            if (de != null)
            {
                _context._Decrets.Remove(de);
                _context.SaveChanges();
                res = de.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Decret d)
        {
            int res = -1;
            var c = _context._Decrets
             .Where(de => de.Numero == d.Numero && de.Objet == d.Objet && de.DateSign == d.DateSign)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Decret> GetAll()
        {
            var ds = _context._Decrets
                .OrderByDescending(d => d.Created)
                .ToList();
            return ds;
        }

        public ICollection<_Decret> GetAllByAnnee(string annee)
        {
            var aaa = _context._Decrets
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _Decret GetById(int id)
        {
            var d = _context._Decrets
               .Where(de => de.Id == id)
               .FirstOrDefault() ?? null;
            return d;
        }

        public int Update(_Decret de)
        {
            int res = -1;
            var d = _context._Decrets.Where(dd => dd.Id == de.Id)
               .FirstOrDefault() ?? null;
            if (d != null)
            {
                d.Numero = de.Numero;
                d.DateSign = de.DateSign;
                d.Status = de.Status;
                d.Objet = de.Objet;
                d.Signataire = de.Signataire;
                d.Updated = DateTime.Now;
                _context._Decrets.Update(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileDecretAsync(IFormFile file, _Decret d)
        {
            string? num = d.Numero + "_" + d.DateSign + "_" + d.Objet;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decrets", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
