using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IArrete:IDisposable
    {
        ICollection<Arrete> GetAll();
        ICollection<Arrete> GetAllByCodeAnne(string code, string annee);
        Arrete GetById(int id);

        int Add(Arrete item);
        int Update(Arrete item);
        int Delete(int id);
        int Existe(Arrete item);
        Task<string> UploadPdfFileArreteAsync(IFormFile file, Arrete a);
    }
}
