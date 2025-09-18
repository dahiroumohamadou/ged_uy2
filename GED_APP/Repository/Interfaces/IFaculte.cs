using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IFaculte:IDisposable
    {
        ICollection<Faculte> GetAll();
        Faculte GetById(int id);
        int Add(Faculte f);
        int Update(Faculte f);
        int Delete(int id);
        int Existe(Faculte f);
    }
}
