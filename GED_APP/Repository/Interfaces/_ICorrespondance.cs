using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _ICorrespondance:IDisposable
    {
        ICollection<_Correspondance> GetAll();
        ICollection<_Correspondance> GetAllByAnnee(string annee);
        _Correspondance GetById(int id);
        int Add(_Correspondance c);
        int Update(_Correspondance c);
        int Delete(int id);
        int Existe (_Correspondance c);
        Task<string> UploadPdfFileCorrespondanceAsync(IFormFile file, _Correspondance c);
    }
}
