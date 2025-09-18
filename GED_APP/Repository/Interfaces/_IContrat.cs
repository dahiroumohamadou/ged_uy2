using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IContrat:IDisposable
    {
        ICollection<_Contrat> GetAll();
        ICollection<_Contrat> GetAllByAnnee(string annee);
        _Contrat GetById(int id);
        int Add(_Contrat c);
        int Update(_Contrat c);
        int Delete(int id);
        int Existe(_Contrat c);
        Task<string> UploadPdfFileContratAsync(IFormFile file, _Contrat c);

    }
}
