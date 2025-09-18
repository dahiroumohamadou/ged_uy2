using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IAttestation:IDisposable
    {
        ICollection<_Attestation> GetAll();
        ICollection<_Attestation> GetAllByAnnee(string annee);
        _Attestation GetById(int id);
        int Add(_Attestation a);
        int Update(_Attestation a);
        int Delete(int id);
        int Existe(_Attestation a);
        Task<string> UploadPdfFileAttestationAsync(IFormFile file, _Attestation a);
    }
}
