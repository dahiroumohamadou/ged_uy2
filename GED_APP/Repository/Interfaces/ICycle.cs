using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface ICycle : IDisposable
    {
        ICollection<Cycle> GetAll();
        Cycle GetById(int id);
        int Add(Cycle cycle);
        int Update(Cycle cycle);
        int Delete(int id);
        int Existe(Cycle cycle);
    }
}
