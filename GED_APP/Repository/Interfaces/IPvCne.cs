using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IPvCne:IDisposable
    {
        ICollection<PvCNE> GetAll();
        ICollection<PvCNE> GetAllByCodeAnne(string code, string annee);
        PvCNE GetById(int id);
        int Add(PvCNE p);
        int Update(PvCNE p);
        int Delete(int id);
        int Existe(PvCNE p);
        Task<string> UploadPdfFilePvCneAsync(IFormFile file, PvCNE p);
    }
}
