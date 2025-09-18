using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class _NoteServiceRepo:_INoteService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public _NoteServiceRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(_NoteService n)
        {
            int res = -1;
            var nnn = _context._NoteServices
                .Where(nn => nn.Numero == n.Numero && nn.DateSign == n.DateSign)
                .FirstOrDefault() ?? null;
            if (nnn == null)
            {
                if (n!= null)
                {
                    _context._NoteServices.Add(n);
                    _context.SaveChanges();
                    res = n.Id;
                }

            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var nnn = _context._NoteServices
                 .Where(aa => aa.Id == id)
                 .FirstOrDefault() ?? null;
            if (nnn != null)
            {
                _context._NoteServices.Remove(nnn);
                _context.SaveChanges();
                res = nnn.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(_NoteService n)
        {
            int res = -1;
            var c = _context._NoteServices
              .Where(aa => aa.Numero == n.Numero && aa.DateSign == n.DateSign && aa.Signataire == n.Signataire)
                .FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }

        public ICollection<_NoteService> GetAll()
        {
            var nnn = _context._NoteServices
                .OrderByDescending(a => a.Created)
                .ToList();
            return nnn;
        }

        public ICollection<_NoteService> GetAllByAnnee(string annee)
        {
            var aaa = _context._NoteServices
               .Where(d => d.DateSign.Substring(6) == annee)
              .OrderBy(a => a.DateSign)
              .ToList();
            return aaa;
        }

        public _NoteService GetById(int id)
        {
            var n = _context._NoteServices
               .Where(nn => nn.Id == id)
               .FirstOrDefault() ?? null;
            return n;
        }

        public int Update(_NoteService nn)
        {
            int res = -1;
            var n = _context._NoteServices.Where(nnn => nnn.Id == nn.Id)
               .FirstOrDefault() ?? null;
            if (n!= null)
            {
                n.Numero = nn.Numero;
                n.DateSign = nn.DateSign;
                n.Status = nn.Status;
                n.Objet = nn.Objet;
                n.Signataire = nn.Signataire;
                n.Updated = DateTime.Now;
                _context._NoteServices.Update(n);
                _context.SaveChanges();
                res = n.Id;
            }
            return res;
        }

        public async Task<string> UploadPdfFileNoteServiceAsync(IFormFile file, _NoteService n)
        {
            string? num = n.Numero + "_" + n.DateSign + "_" + n.Objet;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/noteServices", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
