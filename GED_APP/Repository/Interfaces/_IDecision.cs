using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _IDecision:IDisposable
    {
        ICollection<_Decision> GetAll();
        ICollection<_Decision> GetAllByAnnee(string annee);
        _Decision GetById(int id);
        int Add(_Decision d);
        int Update(_Decision d);
        int Delete(int id);
        int Existe(_Decision d);
        Task<string> UploadPdfDecisionAsync(IFormFile file, _Decision d);

    }
}
