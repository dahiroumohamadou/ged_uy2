using GED_APP.Models;
using GED_APP.Repository.Interfaces;

namespace GED_APP.Repository.Implementations
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        public LocalFileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        //public async Task<string> UploadFileAsync(IFormFile file, Dossier d)
        //{
        //    // resize image 
        //    var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/img/photos",  + ".png");
        //    using var image=Image.Load(file.OpenReadStream());
        //    image.Mutate(x => x.Resize(180,180));
        //    image.Save(filePath);
        //    // convert and copy
        //    //var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/img/photos", m.Matricule+".png");
        //    //using var fileStream = new FileStream(filePath, FileMode.Create);
        //    //await file.CopyToAsync(fileStream);
        //    return filePath;
        //}
        public async Task<string> UploadPdfFileArreteAsync(IFormFile file, Doc a)
        {

            string? num = "Arr_" + a.Numero + "_Du " + a.DateSign + "_Source" + a.Source;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/arretes", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

        public async Task<string> UploadPdfFileCommuniqueAsync(IFormFile file, Doc c)
        {

            string? num = "CRP_" + c.Numero + "_Du " + c.DateSign + "_Ses" + c.Session + "_AnneeAcad" + c.AnneeAcademique + "_cycl" + "_source" + c.Source;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/communiques", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

        public async Task<string> UploadPdfFilePvsAsync(IFormFile file, Doc p)
        {
            string? num = "Pv_" + p.Promotion + "_A" + p.AnneeSortie + "_S" + p.Session + "_C" + p.CycleId + "_F" + p.FiliereId + "_Source_" + p.Source;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/pvs", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
        public async Task<string> UploadPdfFileAutreAsync(IFormFile file, Doc a)
        {
            string? num = "Autre_" + a.Numero + "_Du " + a.DateSign;
            var replacement = num.Replace('/', '_');
            replacement = replacement.Replace(' ', '_');
            // convert and copy
            var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/doc/pdf/autres", replacement + ".pdf");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
