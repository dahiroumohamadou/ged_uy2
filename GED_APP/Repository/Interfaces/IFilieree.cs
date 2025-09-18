
using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IFilieree:IDisposable
    {
        ICollection<Filieree> GetAll();
        Filieree GetById(int id);
        int Add(Filieree f);
        int Update(Filieree f);
        int Delete(int id);
        int Existe(Filieree f);
    }
}
