using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class ExamenRepo:IExamen
    {
        private readonly AppDbContext _context;
        public ExamenRepo(AppDbContext context)
        {
            _context = context;
        }
        public int Add(Examen ex)
        {
            int res = -1;
            var e = _context.Examens.Where(ee => ee.Code == ex.Code && ee.Nom == ex.Nom).FirstOrDefault() ?? null;
            if (e== null)
            {
                if (ex != null)
                {
                    _context.Examens.Add(ex);
                    _context.SaveChanges();
                    res = ex.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var ex = _context.Examens.Where(e => e.Id == id).FirstOrDefault() ?? null;
            if (ex != null)
            {
                _context.Examens.Remove(ex);
                _context.SaveChanges();
                res = ex.Id;
            }
            return res;
        }

        public ICollection<Examen> GetAll()
        {
            var exs = _context.Examens
                .ToList();
            return exs;
        }

        public Examen GetById(int id)
        {
            var e = _context.Examens
                .Where(ex => ex.Id == id).FirstOrDefault() ?? null;
            return e;
        }

        public int Update(Examen ex)
        {
            int res = -1;
            var e = _context.Examens.Where(exa => exa.Id == ex.Id).FirstOrDefault() ?? null;
            if (e != null)
            {
                e.Code = ex.Code;
                e.Nom = ex.Nom;
                e.Updated = DateTime.Now;
                _context.Examens.Update(e);
                _context.SaveChanges();
                res = e.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Examen ex)
        {
            int res = -1;
            var e = _context.Examens.Where(exa => exa.Code == ex.Code && exa.Nom == ex.Nom).FirstOrDefault() ?? null;
            if (e != null)
            {
                res = e.Id;
            }
            return res;
        }
    }
}
