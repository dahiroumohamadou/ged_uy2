using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IChronogramme:IDisposable
    {
        ICollection<Chronogramme> GetAll();
        ICollection<Chronogramme> GetAllByCodeAnne(string code, string annee);
        Chronogramme GetById(int id);
        int Add(Chronogramme chronogramme);
        int Update(Chronogramme chronogramme);
        int Delete(int id);
        int Existe(Chronogramme c);
        Task<string> UploadPdfFileChronoAsync(IFormFile file, Chronogramme c);
       
    }
}

