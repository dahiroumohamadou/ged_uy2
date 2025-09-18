using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _ICommunique:IDisposable
    {
        ICollection<_Communique> GetAll();
        ICollection<_Communique> GetAllByAnnee(string annee);
        _Communique GetById(int id);
        int Add(_Communique c);
        int Update(_Communique c);
        int Delete(int id);
        int Existe (_Communique c);
        Task<string> UploadPdfFileCommuniqueAsync(IFormFile file, _Communique c);
    }
}
