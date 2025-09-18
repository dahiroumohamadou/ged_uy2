using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _ArreteRepo:_IArrete
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _ArreteRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Arrete a)
        {
            int res = -1;
            var aaa = _context._Arretes
                .Where(aa => aa.Numero == a.Numero && aa.DateSign == a.DateSign && aa.Signataire==a.Signataire)
                .FirstOrDefault() ?? null;
            if (aaa == null)
            {
                if (a != null)
                {
                    _context._Arretes.Add(a);
                    _context.SaveChanges();
                    res = a.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var aaa = _context._Arretes
                 .Where(aa => aa.Id == id)
                 .FirstOrDefault() ?? null;
            if (aaa != null)
            {
                _context._Arretes.Remove(aaa);
                _context.SaveChanges();
                res = aaa.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Arrete a)
        {
            int res = -1;
            var c = _context._Arretes
              .Where(aa => aa.Numero == a.Numero && aa.DateSign == a.DateSign && aa.Signataire == a.Signataire)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Arrete> GetAll()
        {
            var aaa = _context._Arretes
                .OrderByDescending(a => a.DateSign)
                .ToList();
            return aaa;
        }

        public ICollection<_Arrete> GetAllByAnnee(string? annee)
        {
            var aaa = _context._Arretes
                .Where(d => d.DateSign.Substring(6)==annee)
               .OrderBy(a => a.DateSign)
               .ToList();
            return aaa;
        }

        public _Arrete GetById(int id)
        {
            var a = _context._Arretes
               .Where(aa => aa.Id == id)
               .FirstOrDefault() ?? null;
            return a;
        }

        public int Update(_Arrete aa)
        {
            int res = -1;
            var a = _context._Arretes.Where(aaa => aaa.Id == aa.Id)
               .FirstOrDefault() ?? null;
            if (a != null)
            {
                a.Numero = aa.Numero;
                a.DateSign = aa.DateSign;
                a.Status = aa.Status;
                a.Objet = aa.Objet;
                a.Signataire = aa.Signataire;
                a.Updated = DateTime.Now;
                _context._Arretes.Update(a);
                _context.SaveChanges();
                res = a.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileArreteAsync(IFormFile file, _Arrete a)
        {
            string? num = a.Numero + "_" + a.DateSign + "_" + a.Objet;
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

