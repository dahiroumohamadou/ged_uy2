using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadPdfFileArreteAsync(IFormFile file, Doc a);
        Task<string> UploadPdfFileCommuniqueAsync(IFormFile file, Doc c);
        Task<string> UploadPdfFilePvsAsync(IFormFile file, Doc pv);
        Task<string> UploadPdfFileAutreAsync(IFormFile file, Doc a);
    }
}
