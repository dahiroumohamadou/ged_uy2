using GED_APP.Data;
using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface ICorrespondance:IDisposable
    {
        ICollection<Correspondance> GetAll();
        ICollection<Correspondance> GetAllByCodeAnne(string code, string annee);
        Correspondance GetById(int id);
        int Add(Correspondance correspondance);
        int Update(Correspondance correspondance);
        int Delete(int id);
        int Existe(Correspondance c);
        Task<string> UploadPdfFileCorrespAsync(IFormFile file, Correspondance c);

    }
}
