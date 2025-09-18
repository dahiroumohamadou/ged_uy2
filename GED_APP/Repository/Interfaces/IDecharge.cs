using GED_APP.Migrations;
using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IDecharge:IDisposable
    {
        ICollection<Decharge> GetAll();
        ICollection<Decharge> GetAllByCodeAnne(string code, string annee);
        Decharge GetById(int id);
        int Add(Decharge decharge);
        int Update(Decharge decharge);
        int Delete(int id);
        int Existe(Decharge d);
        Task<string> UploadPdfFileDechargeAsync(IFormFile file, Decharge d);
    }
}
