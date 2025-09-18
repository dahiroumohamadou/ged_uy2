

using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IExamen:IDisposable
    {
        ICollection<Examen> GetAll();
        Examen GetById(int id);
        int Add(Examen e);
        int Update(Examen e);
        int Delete(int id);
        int Existe(Examen e);
    }
}
