using GED_APP.Data;
using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IPvExamen:IDisposable
    {
        ICollection<PvExamen> GetAll();
        ICollection<PvExamen> GetAllByCodeAnne(string code, string annee);
        PvExamen GetById(int id);
        int Add(PvExamen pv);
        int Update(PvExamen pv);
        int Delete(int id);
        int Existe(PvExamen p);
        Task<string> UploadPdfFilePvExamenAsync(IFormFile file, PvExamen p);

    }
}
