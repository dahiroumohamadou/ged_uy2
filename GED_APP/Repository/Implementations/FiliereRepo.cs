using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class FiliereRepo : IFiliere
    {
        private readonly AppDbContext _ctx;
        public FiliereRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public int Add(Filiere filiere)
        {
            int res = -1;

            if (filiere != null)
            {
                var f = _ctx.Filieres.Where(fl => fl.Code == filiere.Code && fl.Libele == filiere.Libele).FirstOrDefault() ?? null;
                if (f == null)
                {
                    _ctx.Filieres.Add(filiere);
                    _ctx.SaveChanges();
                    res = filiere.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var f = _ctx.Filieres.Where(fl => fl.Id == id).FirstOrDefault() ?? null;
            if (f != null)
            {
                _ctx.Filieres.Remove(f);
                _ctx.SaveChanges();
                res = f.Id;
            }
            return res;
        }

        public ICollection<Filiere> GetAll()
        {
            var fls = _ctx.Filieres
                    .ToList();
            return fls;
        }

        public Filiere GetById(int id)
        {
            var f = _ctx.Filieres
                .Where(fl => fl.Id == id).FirstOrDefault() ?? null;
            return f;
        }

        public int Update(Filiere filiere)
        {
            int res = -1;
            var f = _ctx.Filieres.Where(fl => fl.Id == filiere.Id).FirstOrDefault() ?? null;
            if (f != null)
            {
                f.Code = filiere.Code;
                f.Libele = filiere.Libele;
                f.Updated = DateTime.Now;
                _ctx.Filieres.Update(f);
                _ctx.SaveChanges();
                res = f.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }
}
