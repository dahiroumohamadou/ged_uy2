using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class DossierRepo:IDossier
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public DossierRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int Add(Dossier dossier)
        {
            int res = -1;
            var dos = _context.Dossiers
                .Where(d => d.Objet == dossier.Objet && d.Demandeur == dossier.Demandeur && d.DateEntree == dossier.DateEntree && d.Numero == dossier.Numero)
                .FirstOrDefault() ?? null;
            if (dos == null)
            {
                if (dossier != null)
                {
                    _context.Dossiers.Add(dossier);
                    _context.SaveChanges();
                    res = dossier.Id;
                }
            }
            return res;

        }
        public int Delete(int id)
        {
            int res = -1;
            var d = _context.Dossiers.Where(dos => dos.Id == id)
                .FirstOrDefault() ?? null;
            if (d != null)
            {
                _context.Dossiers.Remove(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Dossier dossier)
        {
            int res = -1;
            var dos = _context.Dossiers
                .Where(d => d.Objet == dossier.Objet && d.Demandeur == dossier.Demandeur && d.DateEntree == dossier.DateEntree && d.Numero == dossier.Numero)
                .FirstOrDefault() ?? null;
            if (dos != null)
            {
                res = dos.Id;
            }
            return res;
        }
        public ICollection<Dossier> GetAll()
        {
            var ds = _context.Dossiers
                .Include(c => c.Structure)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return ds;
        }

        public ICollection<Dossier> GetAllByCodeAnne(string code, string annee)
        {
            

            var ds = _context.Dossiers
                .Where(a => a.Origine == code && a.DateEntree.Substring(6) == annee)
                  .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .ToList();
            return ds;
        }

        public Dossier GetById(int id)
        {
            var d = _context.Dossiers
                .Include(c => c.Structure)
                .Where(dos => dos.Id == id)
                .FirstOrDefault() ?? null;
            return d;

        }



        public int Update(Dossier dos)
        {
            int res = -1;
            var d = _context.Dossiers.Where(doss => doss.Id == dos.Id)
               .FirstOrDefault() ?? null;
            if (d != null)
            {
                d.Numero = dos.Numero;
               d.Demandeur = dos.Demandeur;
                d.Objet=dos.Objet;
                d.DateEntree=dos.DateEntree;
                d.DateSortie=dos.DateSortie;
                d.Status = dos.Status;
                d.Origine= dos.Origine;
                d.StructureId=dos.StructureId;
                d.Updated = DateTime.Now;
                d.PersonneTraite= dos.PersonneTraite;
              
                _context.Dossiers.Update(d);
                _context.SaveChanges();
                res = d.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFileDossierAsync(IFormFile file, Dossier d)
        {
            string? num = d.Numero + "_" + d.Objet + "_" + d.DateEntree + "_" + d.Demandeur+"_"+d.Origine ;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/dossiers", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

    }
}
