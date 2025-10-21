using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _CommuniqueRepo:_ICommunique
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _CommuniqueRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Communique c)
        {
            int res = -1;
            var com = _context._Communiques
                .Where(cc => cc.Numero == c.Numero && cc.Code==c.Code && cc.DateSign == c.DateSign && cc.Signataire == c.Signataire)
                .FirstOrDefault() ?? null;
            if (com == null)
            {
                if (c != null)
                {
                    _context._Communiques.Add(c);
                    _context.SaveChanges();
                    res = c.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var com = _context._Communiques
                 .Where(cc => cc.Id == id)
                 .FirstOrDefault() ?? null;
            if (com != null)
            {
                _context._Communiques.Remove(com);
                _context.SaveChanges();
                res = com.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Communique co)
        {
            int res = -1;
            var c = _context._Contrats
                 .Where(cc => cc.Numero == co.Numero && cc.Code == co.Code && cc.DateSign == co.DateSign && cc.Signataire==co.Signataire)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_Communique> GetAll()
        {
            var crs = _context._Communiques
                .OrderByDescending(c => c.Created)
                .ToList();
            return crs;
        }

        public ICollection<_Communique> GetAllByAnnee(string annee)
        {
            var aaa = _context._Communiques
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _Communique GetById(int id)
        {
            var c = _context._Communiques
               .Where(cr => cr.Id == id)
               .FirstOrDefault() ?? null;
            return c;
        }

        public int Update(_Communique cm)
        {
            int res = -1;
            var c = _context._Communiques.Where(com => com.Id == cm.Id)
               .FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Code=cm.Code;
                c.Numero = cm.Numero;
                c.DateSign = cm.DateSign;
                c.Status = cm.Status;
                c.Objet = cm.Objet;
                c.Signataire = cm.Signataire;
                c.Updated = DateTime.Now;
                _context._Communiques.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileCommuniqueAsync(IFormFile file, _Communique c)
        {
            string? num = c.Numero + "_" + c.DateSign + "_" + c.Objet;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/communiques", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}

