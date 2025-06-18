
using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
namespace GED_APP.Repository.Implementations
{
    public class CycleRepo : ICycle
    {
        private readonly AppDbContext _context;
        public CycleRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public int Add(Cycle cycle)
        {
            int res = -1;
            var c = _context.Cycles.Where(cm => cm.Code == cycle.Code && cm.Libele == cycle.Libele).FirstOrDefault() ?? null;
            if (c == null)
            {
                if (cycle != null)
                {
                    _context.Cycles.Add(cycle);
                    _context.SaveChanges();
                    res = cycle.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var c = _context.Cycles.Where(cy => cy.Id == id).FirstOrDefault() ?? null;
            if (c != null)
            {
                _context.Cycles.Remove(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }

        public ICollection<Cycle> GetAll()
        {
            var cys = _context.Cycles
                .ToList();
            return cys;
        }

        public Cycle GetById(int id)
        {
            var c = _context.Cycles
                .Where(cm => cm.Id == id).FirstOrDefault() ?? null;
            return c;
        }

        public int Update(Cycle cycle)
        {
            int res = -1;
            var c = _context.Cycles.Where(cm => cm.Id == cycle.Id).FirstOrDefault() ?? null;
            if (c != null)
            {
                c.Code = cycle.Code;
                c.Libele = cycle.Libele;
                c.Updated = DateTime.Now;
                _context.Cycles.Update(c);
                _context.SaveChanges();
                res = c.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Cycle cycle)
        {
            int res = -1;
            var c = _context.Cycles.Where(cm => cm.Code == cycle.Code && cm.Libele == cycle.Libele).FirstOrDefault() ?? null;
            if (c != null)
            {
                res = c.Id;
            }
            return res;
        }
    }
}
