using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface _INoteService:IDisposable
    {
        ICollection<_NoteService> GetAll();
        ICollection<_NoteService> GetAllByAnnee(string annee);
        _NoteService GetById(int id);
        int Add(_NoteService ns);
        int Update(_NoteService ns);
        int Delete(int id);
        int Existe(_NoteService ns);
        Task<string> UploadPdfFileNoteServiceAsync(IFormFile file, _NoteService ns);
    }
}
