using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _EtatPaiementRepo:_IEtatPaiement
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _EtatPaiementRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_EtatPaiement e)
        {
            int res = -1;
            var eee = _context._EtatPaiements
                .Where(ee => ee.Numero == e.Numero && ee.DateSign == e.DateSign)
                .FirstOrDefault() ?? null;
            if (eee == null)
            {
                if (e != null)
                {
                    _context._EtatPaiements.Add(e);
                    _context.SaveChanges();
                    res = e.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var eee = _context._EtatPaiements
                 .Where(ee => ee.Id == id)
                 .FirstOrDefault() ?? null;
            if (eee != null)
            {
                _context._EtatPaiements.Remove(eee);
                _context.SaveChanges();
                res = eee.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_EtatPaiement e)
        {
            int res = -1;
            var c = _context._Decrets
             .Where(ee => ee.Numero == e.Numero && ee.DateSign == e.DateSign)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_EtatPaiement> GetAll()
        {
            var eee = _context._EtatPaiements
                .OrderByDescending(e=> e.Created)
                .ToList();
            return eee;
        }

        public ICollection<_EtatPaiement> GetAllByAnnee(string annee)
        {
           var aaa = _context._EtatPaiements
                .Where(d => d.DateSign.Substring(6) == annee)
               .OrderBy(a => a.DateSign)
               .ToList();
            return aaa;
        }

        public _EtatPaiement GetById(int id)
        {
            var e = _context._EtatPaiements
               .Where(ee => ee.Id == id)
               .FirstOrDefault() ?? null;
            return e;
        }

        public int Update(_EtatPaiement ee)
        {
            int res = -1;
            var e = _context._EtatPaiements.Where(eee => eee.Id == ee.Id)
               .FirstOrDefault() ?? null;
            if (e != null)
            {
                e.Numero = ee.Numero;
                e.DateSign = ee.DateSign;
                e.Status = ee.Status;
                e.Objet = ee.Objet;
               e.Signataire = ee.Signataire;
                e.Updated = DateTime.Now;
                _context._EtatPaiements.Update(e);
                _context.SaveChanges();
                res = e.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileEtatPaiementAsync(IFormFile file, _EtatPaiement e)
        {
            string? num = e.Numero + "_" + e.DateSign + "_" + e.Objet;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/etatPaiements", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}

