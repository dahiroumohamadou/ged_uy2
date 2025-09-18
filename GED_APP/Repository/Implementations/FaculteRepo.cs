using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class FaculteRepo:IFaculte
    {
        private readonly AppDbContext _context;
        public FaculteRepo(AppDbContext context)
        {
            _context = context;
        }
        public int Add(Faculte fc)
        {
            int res = -1;
            var f = _context.Facultes.Where(fac => fac.Code == fc.Code && fac.Libele == fc.Libele).FirstOrDefault() ?? null;
            if (f == null)
            {
                if (fc != null)
                {
                    _context.Facultes.Add(fc);
                    _context.SaveChanges();
                    res = fc.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var f = _context.Facultes.Where(fc => fc.Id == id).FirstOrDefault() ?? null;
            if (f != null)
            {
                _context.Facultes.Remove(f);
                _context.SaveChanges();
                res = f.Id;
            }
            return res;
        }

        public ICollection<Faculte> GetAll()
        {
            var fcs = _context.Facultes
                .ToList();
            return fcs;
        }

        public Faculte GetById(int id)
        {
            var f = _context.Facultes
                .Where(fc => fc.Id == id).FirstOrDefault() ?? null;
            return f;
        }

        public int Update(Faculte fac)
        {
            int res = -1;
            var f = _context.Facultes.Where(fc => fc.Id == fac.Id).FirstOrDefault() ?? null;
            if (f != null)
            {
                f.Code = fac.Code;
                f.Libele = fac.Libele;
                f.Updated = DateTime.Now;
                _context.Facultes.Update(f);
                _context.SaveChanges();
                res = f.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Faculte fc)
        {
            int res = -1;
            var f = _context.Facultes.Where(fac => fac.Code == fc.Code && fac.Libele == fc.Libele).FirstOrDefault() ?? null;
            if (f != null)
            {
                res = f.Id;
            }
            return res;
        }
    }
}
