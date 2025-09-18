using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _ICertificat:IDisposable
    {
        ICollection<_Certificat> GetAll();
        ICollection<_Certificat> GetAllByAnnee(string annee);
        _Certificat GetById(int id);
        int Add(_Certificat c);
        int Update(_Certificat c);
        int Delete(int id);
        int Existe(_Certificat c);
        Task<string> UploadPdfFileCertificatAsync(IFormFile file, _Certificat c);
    }
}
