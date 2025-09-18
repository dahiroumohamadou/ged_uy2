using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class PvExamenRepo:IPvExamen
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public PvExamenRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public int Add(PvExamen ppv)
        {
            int res = -1;
            var pv = _context.PvExamens
                .Where(p => p.ExamenId == ppv.ExamenId && p.FiliereId == ppv.FiliereId && p.FaculteId == ppv.FaculteId && p.Session==ppv.Session && p.SourceId==ppv.SourceId)
                .FirstOrDefault() ?? null;
            if (pv == null)
            {
                if (ppv != null)
                {
                    _context.PvExamens.Add(ppv);
                    _context.SaveChanges();
                    res = ppv.Id;
                }
            }
            return res;

        }

        public int Delete(int id)
        {
            int res = -1;
            var pv = _context.PvExamens.Where(p => p.Id == id)
                .FirstOrDefault() ?? null;
            if (pv != null)
            {
                _context.PvExamens.Remove(pv);
                _context.SaveChanges();
                res = pv.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(PvExamen ppv)
        {
            int res = -1;
            var pv = _context.PvExamens
               .Where(p => p.ExamenId == ppv.ExamenId && p.FiliereId == ppv.FiliereId && p.FaculteId == ppv.FaculteId && p.Session == ppv.Session && p.SourceId == ppv.SourceId)
               .FirstOrDefault() ?? null;
            if (pv != null)
            {
                res = pv.Id;
            }
            return res;
        }

        public ICollection<PvExamen> GetAll()
        {
            var pvs = _context.PvExamens
                .OrderByDescending(c => c.Created)
                .Include(c => c.Structure)
                .Include(c=>c.Source)
                .Include(c=>c.Examen)
                .Include(c=>c.Faculte)
                .Include(c=>c.Filiere)
                .ToList();
            return pvs;
        }

        public ICollection<PvExamen> GetAllByCodeAnne(string code, string annee)
        {
           
           var pvs = _context.PvExamens
                .Where(a => a.Structure.Code == code && a.Session.Substring(3) == annee)
                .OrderByDescending(c => c.Created)
                .ToList();
            return pvs;
        }

        public PvExamen GetById(int id)
        {
            var pv = _context.PvExamens
                .Include(c => c.Structure)
                .Include(c => c.Source)
                .Include(c => c.Examen)
                .Include(c => c.Faculte)
                .Include(c => c.Filiere)
                .Where(p => p.Id == id)
                .FirstOrDefault() ?? null;
            return pv;

        }



        public int Update(PvExamen pp)
        {
            int res = -1;
            var p = _context.PvExamens.Where(pv => pv.Id == pp.Id)
               .FirstOrDefault() ?? null;
            if (p != null)
            {
                p.ExamenId = pp.ExamenId;
                p.FiliereId = pp.FiliereId;
                p.FaculteId = pp.FaculteId;
                p.SourceId = pp.SourceId;
                p.Session = pp.Session;
                p.Status = pp.Status;
                p.StructureId = pp.StructureId;
                p.Updated = DateTime.Now;
                _context.PvExamens.Update(p);
                _context.SaveChanges();
                res = p.Id;
            }
            return res;

        }

        public async Task<string> UploadPdfFilePvExamenAsync(IFormFile file, PvExamen p)
        {
            string? num = "Pv_" + p.ExamenId + "_" + p.FiliereId + "_" + p.FaculteId+"_"+p.SourceId+"_"+p.Session;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/PvExamens", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
