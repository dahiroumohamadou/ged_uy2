using AspNetCoreGeneratedDocument;
using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace GED_APP.Repository.Implementations
{
    public class _CorrespondanceRepo : _ICorrespondance
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public _CorrespondanceRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context=context;
            _environment=environment;
        }
        public int Add(_Correspondance c)
        {
            int res = -1;
            var cor = _context._Correspondances
                .Where(cc => cc.Reference == c.Reference && cc.Code==c.Code && cc.DateSign == c.DateSign)
                .FirstOrDefault() ?? null;
            if (cor == null) {
                if (c != null)
                {
                    _context._Correspondances.Add(c);
                    _context.SaveChanges();
                    res = c.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var cor = _context._Correspondances
                 .Where(cc => cc.Id == id)
                 .FirstOrDefault() ?? null;
            if (cor != null) { 
                _context._Correspondances.Remove(cor);
                _context.SaveChanges();
                res = cor.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Correspondance cr)
        {

            int res = -1;
            var c = _context._Correspondances
                 .Where(cc => cc.Reference == cr.Reference && cc.Code==cr.Code && cc.DateSign == cr.DateSign)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Correspondance> GetAll()
        {
            var crs = _context._Correspondances
                .OrderByDescending(c => c.Created)
                .ToList();
            return crs;
        }

        public ICollection<_Correspondance> GetAllByAnnee(string annee)
        {
            var aaa = _context._Correspondances
                .Where(d => d.DateSign.Substring(6) == annee)
               .OrderBy(a => a.DateSign)
               .ToList();
            return aaa;
        }

        public _Correspondance GetById(int id)
        {
            var c = _context._Correspondances
               .Where(cr => cr.Id == id)
               .FirstOrDefault() ?? null;
            return c;
        }

        public int Update(_Correspondance cr)
        {
            int res = -1;
            var c = _context._Correspondances.Where(cor => cor.Id == cr.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Code= cr.Code;
                c.Reference = cr.Reference;
                c.DateSign = cr.DateSign;
                c.Status = cr.Status;
                c.Emetteur = cr.Emetteur;
                c.Objet = cr.Objet;
                c.Recepteur = cr.Recepteur;
                c.Signataire = cr.Signataire;
                c.Updated = DateTime.Now;
                _context._Correspondances.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileCorrespondanceAsync(IFormFile file, _Correspondance c)
        {
            string? num = c.Reference + "_" + c.DateSign + "_"  + c.Emetteur + "_To_" + c.Recepteur;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/corresp", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
