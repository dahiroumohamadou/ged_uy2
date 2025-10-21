using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _AttestationRepo:_IAttestation
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _AttestationRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Attestation a)
        {
            int res = -1;
            var aaa = _context._Attestations
                .Where(aa => aa.Numero == a.Numero && aa.DateSign == a.DateSign && aa.Code==a.Code && aa.Signataire==a.Signataire)
                .FirstOrDefault() ?? null;
            if (aaa == null)
            {
                if (a != null)
                {
                    _context._Attestations.Add(a);
                    _context.SaveChanges();
                    res = a.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var aaa = _context._Attestations
                 .Where(aa => aa.Id == id)
                 .FirstOrDefault() ?? null;
            if (aaa != null)
            {
                _context._Attestations.Remove(aaa);
                _context.SaveChanges();
                res = aaa.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Attestation a)
        {
            int res = -1;
            var c = _context._Attestations
               .Where(aa => aa.Numero == a.Numero && aa.DateSign == a.DateSign && aa.Code == a.Code && aa.Signataire == a.Signataire)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Attestation> GetAll()
        {
            var aaa = _context._Attestations
                .OrderByDescending(a => a.Created)
                .ToList();
            return aaa;
        }

        public ICollection<_Attestation> GetAllByAnnee(string annee)
        {
            var aaa = _context._Attestations
                .Where(d => d.DateSign.Substring(6) == annee)
               .OrderBy(a => a.DateSign)
               .ToList();
            return aaa;
        }

        public _Attestation GetById(int id)
        {
            var a = _context._Attestations
               .Where(aa => aa.Id == id)
               .FirstOrDefault() ?? null;
            return a;
        }

        public int Update(_Attestation aa)
        {
            int res = -1;
            var a = _context._Attestations.Where(aaa => aaa.Id == aa.Id)
               .FirstOrDefault() ?? null;
            if (a != null)
            {
                a.Code = aa.Code;
                a.Numero = aa.Numero;
                a.DateSign = aa.DateSign;
                a.Status = aa.Status;
                a.Type = aa.Type;
                a.Destinataire = aa.Destinataire;
                a.Signataire = aa.Signataire;
                a.Updated = DateTime.Now;
                _context._Attestations.Update(a);
                _context.SaveChanges();
                res = a.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileAttestationAsync(IFormFile file, _Attestation a)
        {
            string? num = a.Numero + "_" + a.DateSign + "_" + a.Type +"_to_"+a.Destinataire;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/attestations", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}

