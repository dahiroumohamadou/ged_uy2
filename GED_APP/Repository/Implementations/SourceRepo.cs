using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class SourceRepo:ISource
    {
        private readonly AppDbContext _context;
        public SourceRepo(AppDbContext context)
        {
            _context = context;
        }
        public int Add(Source so)
        {
            int res = -1;
            var s = _context.Sources.Where(soc => soc.Code == so.Code && soc.Libele == so.Libele).FirstOrDefault() ?? null;
            if (s == null)
            {
                if (so != null)
                {
                    _context.Sources.Add(so);
                    _context.SaveChanges();
                    res = so.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var s = _context.Sources.Where(so => so.Id == id).FirstOrDefault() ?? null;
            if (s != null)
            {
                _context.Sources.Remove(s);
                _context.SaveChanges();
                res = s.Id;
            }
            return res;
        }

        public ICollection<Source> GetAll()
        {
            var ss = _context.Sources
                .ToList();
            return ss;
        }

        public Source GetById(int id)
        {
            var s = _context.Sources
                .Where(so => so.Id == id).FirstOrDefault() ?? null;
            return s;
        }

        public int Update(Source so)
        {
            int res = -1;
            var s = _context.Sources.Where(sou => sou.Id == so.Id).FirstOrDefault() ?? null;
            if (s != null)
            {
                s.Code = so.Code;
                s.Libele = so.Libele;
                s.Updated = DateTime.Now;
                _context.Sources.Update(s);
                _context.SaveChanges();
                res = s.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Source ss)
        {
            int res = -1;
            var s = _context.Sources.Where(so=> so.Code == ss.Code && so.Libele == ss.Libele).FirstOrDefault() ?? null;
            if (s != null)
            {
                res = s.Id;
            }
            return res;
        }
    }
}
