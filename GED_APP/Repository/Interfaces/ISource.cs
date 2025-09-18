using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface ISource:IDisposable
    {
        ICollection<Source> GetAll();
        Source GetById(int id);
        int Add(Source s);
        int Update(Source s);
        int Delete(int id);
        int Existe(Source s);
    }
}
