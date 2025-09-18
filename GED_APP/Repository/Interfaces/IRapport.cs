using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IRapport:IDisposable
    {
        ICollection<Rapport> GetAll();
        ICollection<Rapport> GetAllByCodeAnne(string code, string annee);
        Rapport GetById(int id);
        int Add(Rapport r);
        int Update(Rapport r);
        int Delete(int id);
        int Existe(Rapport r);
        Task<string> UploadPdfFileRapportAsync(IFormFile file, Rapport r);

    }
}
