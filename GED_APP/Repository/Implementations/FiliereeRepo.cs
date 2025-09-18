using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class FiliereeRepo:IFilieree
    {
        private readonly AppDbContext _context;
        public FiliereeRepo(AppDbContext context)
        {
            _context = context;
        }
        public int Add(Filieree fi)
        {
            int res = -1;
            var f = _context.Filierees.Where(fil => fil.Code == fi.Code && fil.Description == fi.Description).FirstOrDefault() ?? null;
            if (f == null)
            {
                if (fi != null)
                {
                    _context.Filierees.Add(fi);
                    _context.SaveChanges();
                    res = fi.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var f = _context.Filierees.Where(fi => fi.Id == id).FirstOrDefault() ?? null;
            if (f != null)
            {
                _context.Filierees.Remove(f);
                _context.SaveChanges();
                res = f.Id;
            }
            return res;
        }

        public ICollection<Filieree> GetAll()
        {
            var fs = _context.Filierees
                .ToList();
            return fs;
        }

        public Filieree GetById(int id)
        {
            var f = _context.Filierees
                .Where(fi => fi.Id == id).FirstOrDefault() ?? null;
            return f;
        }

        public int Update(Filieree fil)
        {
            int res = -1;
            var f = _context.Filierees.Where(fi => fi.Id == fil.Id).FirstOrDefault() ?? null;
            if (f != null)
            {
                f.Code = fil.Code;
                f.Description = fil.Description;
                f.Updated = DateTime.Now;
                _context.Filierees.Update(f);
                _context.SaveChanges();
                res = f.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Filieree fi)
        {
            int res = -1;
            var f = _context.Filierees.Where(fil => fil.Code == fi.Code && fil.Description == fi.Description).FirstOrDefault() ?? null;
            if (f != null)
            {
                res = f.Id;
            }
            return res;
        }
    }
}
