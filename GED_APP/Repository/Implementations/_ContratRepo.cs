using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _ContratRepo:_IContrat
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _ContratRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Contrat c)
        {
            int res = -1;
            var cor = _context._Contrats
                .Where(cc => cc.Numero == c.Numero && cc.Type==c.Type && cc.DateSign == c.DateSign)
                .FirstOrDefault() ?? null;
            if (cor == null)
            {
                if (c != null)
                {
                    _context._Contrats.Add(c);
                    _context.SaveChanges();
                    res = c.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var cor = _context._Contrats
                 .Where(cc => cc.Id == id)
                 .FirstOrDefault() ?? null;
            if (cor != null)
            {
                _context._Contrats.Remove(cor);
                _context.SaveChanges();
                res = cor.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Contrat co)
        {
            int res = -1;
            var c = _context._Contrats
                .Where(cc => cc.Numero == co.Numero && cc.Type == co.Type && cc.DateSign == co.DateSign)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res; 
        }

        public ICollection<_Contrat> GetAll()
        {
            var crs = _context._Contrats
                .OrderByDescending(c => c.DateSign)
                .ToList();
            return crs;
        }

        public ICollection<_Contrat> GetAllByAnnee(string annee)
        {
            var aaa = _context._Contrats
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _Contrat GetById(int id)
        {
            var c = _context._Contrats
               .Where(cr => cr.Id == id)
               .FirstOrDefault() ?? null;
            return c;
        }

        public int Update(_Contrat cr)
        {
            int res = -1;
            var c = _context._Contrats.Where(cor => cor.Id == cr.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Numero = cr.Numero;
                c.DateSign = cr.DateSign;
                c.Type = cr.Type;
                c.Status = cr.Status;
                c.Beneficiaire = cr.Beneficiaire;
                c.Signataire = cr.Signataire;
                c.Updated = DateTime.Now;
                _context._Contrats.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileContratAsync(IFormFile file, _Contrat c)
        {
            string? num = c.Numero + "_" + c.Type + "_" + c.DateSign + "_To_" + c.Beneficiaire;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/contrat", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
