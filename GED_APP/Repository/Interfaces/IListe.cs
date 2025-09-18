using GED_APP.Data;
using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IListe:IDisposable
    {
        ICollection<Liste> GetAll();
        ICollection<Liste> GetAllByCodeAnne(string code, string annee);
        Liste GetById(int id);
        int Add(Liste item);
        int Update(Liste item);
        int Delete(int id);
        int Existe(Liste l);
        Task<string> UploadPdfFileListeAsync(IFormFile file, Liste l);

    }
}
