using AspNetCoreGeneratedDocument;
using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GED_APP.Repository.Implementations
{
    public class StructureRepo : IStructure
    {
        private readonly AppDbContext _context;
        public StructureRepo(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Structure structure)
        {
            int res = -1;
            var s=_context.Structures.Where(str=>str.Code==structure.Code).FirstOrDefault() ?? null;
            if (s == null)
            {
                if(structure!=null)
                {
                    _context.Structures.Add(structure);
                    _context.SaveChanges();
                    res = structure.Id;
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var s=_context.Structures.Where(str=>str.Id==id).FirstOrDefault() ?? null;
            if (s != null) { 
                _context.Structures.Remove(s);
                _context.SaveChanges();
                res = s.Id;
            }
            return res;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Existe(Structure structure)
        {
            int res = -1;
            var s = _context.Structures.Where(str => str.Code == structure.Code).FirstOrDefault() ?? null;
            if (s != null)
            {
                res = s.Id;
            }
            return res;
        }

        public ICollection<Structure> GetAll()
        {
          var strs=_context.Structures
                .ToList();
            return strs;
        }

        public Structure GetByCode(string code)
        {
            var s = _context.Structures
                .Where(str => str.Code == code)
                .FirstOrDefault() ?? null;
            return s;
        }

        public Structure GetById(int id)
        {
            var s = _context.Structures.Where(str => str.Id == id)
                .FirstOrDefault() ?? null;
            return s;
        }

        public int Update(Structure structure)
        {
            int res=-1;
            var s = _context.Structures.Where(str => str.Code == structure.Code)
                .FirstOrDefault() ?? null;
            if (s != null)
            {
                s.Code=structure.Code;
                s.Libele=structure.Libele;
                s.Updated=DateTime.Now;
                _context.Structures.Update(s);
                _context.SaveChanges();
                res = s.Id;
            }
            return res;
        }
    }
}
