using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IDossier:IDisposable
    {
        ICollection<Dossier> GetAll();
        ICollection<Dossier> GetAllByCodeAnne(string code, string annee);
        Dossier GetById(int id);
        int Add(Dossier d);
        int Update(Dossier d);
        int Delete(int id);
        int Existe(Dossier d);
        Task<string> UploadPdfFileDossierAsync(IFormFile file, Dossier d);
    }
}
