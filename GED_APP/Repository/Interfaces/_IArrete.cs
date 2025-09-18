using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IArrete:IDisposable
    {
        ICollection<_Arrete> GetAll();
        ICollection<_Arrete> GetAllByAnnee(string annee);
        _Arrete GetById(int id);
        int Add(_Arrete a);
        int Update(_Arrete a);
        int Delete(int id);
        int Existe(_Arrete a);
        Task<string> UploadPdfFileArreteAsync(IFormFile file, _Arrete a);
    }
}
