using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IStructure:IDisposable
    {
        ICollection<Structure> GetAll();
        Structure GetById(int id);
        Structure GetByCode(string code);
        int Existe(Structure structure);
        int Add(Structure structure);
        int Update(Structure structure);
        int Delete(int id);
        

    }
}
