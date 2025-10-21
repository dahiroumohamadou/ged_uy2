using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _CertificatRepo:_ICertificat
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _CertificatRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Certificat c)
        {
            int res = -1;
            var cor = _context._Certificats
                .Where(cc => cc.Numero == c.Numero && cc.Code==c.Code && cc.Type == c.Type && cc.DateSign == c.DateSign)
                .FirstOrDefault() ?? null;
            if (cor == null)
            {
                if (c != null)
                {
                    _context._Certificats.Add(c);
                    _context.SaveChanges();
                    res = c.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var cor = _context._Certificats
                 .Where(cc => cc.Id == id)
                 .FirstOrDefault() ?? null;
            if (cor != null)
            {
                _context._Certificats.Remove(cor);
                _context.SaveChanges();
                res = cor.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Certificat co)
        {
            int res = -1;
            var c = _context._Certificats
                .Where(cc => cc.Numero == co.Numero && cc.Code==co.Code && cc.Type == co.Type && cc.DateSign == co.DateSign)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Certificat> GetAll()
        {
            var crs = _context._Certificats
                .OrderByDescending(c => c.Created)
                .ToList();
            return crs;
        }

        public ICollection<_Certificat> GetAllByAnnee(string annee)
        {
            var aaa = _context._Certificats
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _Certificat GetById(int id)
        {
            var c = _context._Certificats
               .Where(cr => cr.Id == id)
               .FirstOrDefault() ?? null;
            return c;
        }

        public int Update(_Certificat cr)
        {
            int res = -1;
            var c = _context._Certificats.Where(cor => cor.Id == cr.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Code=cr.Code;
                c.Numero = cr.Numero;
                c.DateSign = cr.DateSign;
                c.Status = cr.Status;
                c.Type = cr.Type;
                c.Destinataire = cr.Destinataire;
                c.Signataire = cr.Signataire;
                c.Updated = DateTime.Now;
                _context._Certificats.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileCertificatAsync(IFormFile file, _Certificat c)
        {
            string? num = c.Numero + "_" + c.Type + "_" + c.DateSign + "_To_" + c.Destinataire;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/certificats", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
