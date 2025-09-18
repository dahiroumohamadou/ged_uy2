using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IDecret:IDisposable
    {
        ICollection<_Decret> GetAll();
        ICollection<_Decret> GetAllByAnnee(string annee);
        _Decret GetById(int id);
        int Add(_Decret d);
        int Update(_Decret d);
        int Delete(int id);
        int Existe(_Decret d);
        Task<string> UploadPdfFileDecretAsync(IFormFile file, _Decret d);
    }
}
