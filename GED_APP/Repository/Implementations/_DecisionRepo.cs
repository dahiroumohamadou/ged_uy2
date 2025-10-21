using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _DecisionRepo:_IDecision
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _DecisionRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_Decision d)
        {
            int res = -1;
            var dd = _context._Decisions
                .Where(de => de.Numero == d.Numero && de.Code==d.Code && de.Type == d.Type && de.DateSign == d.DateSign)
                .FirstOrDefault() ?? null;
            if (dd == null)
            {
                if (d != null)
                {
                    _context._Decisions.Add(d);
                    _context.SaveChanges();
                    res = d.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var de = _context._Decisions
                 .Where(d => d.Id == id)
                 .FirstOrDefault() ?? null;
            if (de != null)
            {
                _context._Decisions.Remove(de);
                _context.SaveChanges();
                res = de.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_Decision dd)
        {
            int res = -1;
            var d = _context._Decisions
                .Where(de => de.Numero == dd.Numero && de.Code==dd.Code && de.Type == dd.Type && de.DateSign == dd.DateSign)
                .FirstOrDefault() ?? null;
            if (d != null)
            {
                res = d.Id;
            }
            return res; 
        }

        public ICollection<_Decision> GetAll()
        {
            var ds = _context._Decisions
                .OrderByDescending(d => d.Created)
                .ToList();
            return ds;
        }

        public ICollection<_Decision> GetAllByAnnee(string annee)
        {
            var aaa = _context._Decisions
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _Decision GetById(int id)
        {
            var d = _context._Decisions
               .Where(de => de.Id == id)
               .FirstOrDefault() ?? null;
            return d;
        }

        public int Update(_Decision de)
        {
            int res = -1;
            var d = _context._Decisions.Where(dd => dd.Id == de.Id)
               .FirstOrDefault() ?? null;
            if (d != null)
            {
                d.Numero = de.Numero;
                d.Code = de.Code;
                d.DateSign = de.DateSign;
                d.Status = de.Status;
                d.Type = de.Type;
                d.Objet = de.Objet;
                d.Signataire = de.Signataire;
                d.Updated = DateTime.Now;
                _context._Decisions.Update(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfDecisionAsync(IFormFile file, _Decision d)
        {
            string? num = d.Numero + "_" + d.Type + "_" + d.DateSign + "_" + d.Objet;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/decision", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
