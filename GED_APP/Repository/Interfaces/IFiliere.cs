using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IFiliere : IDisposable
    {
        ICollection<Filiere> GetAll();
        Filiere GetById(int id);
        int Add(Filiere filiere);
        int Update(Filiere filiere);
        int Delete(int id);
    }
}
