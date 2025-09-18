using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IEtatPaiement:IDisposable
    {
        ICollection<_EtatPaiement> GetAll();
        ICollection<_EtatPaiement> GetAllByAnnee(string annee);
        _EtatPaiement GetById(int id);
        int Add(_EtatPaiement ep);
        int Update(_EtatPaiement ep);
        int Delete(int id);
        int Existe(_EtatPaiement ep);
        Task<string> UploadPdfFileEtatPaiementAsync(IFormFile file, _EtatPaiement ep);
    }
}
