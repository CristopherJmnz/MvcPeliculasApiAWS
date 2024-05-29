using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiAWS.Services;

namespace MvcPeliculasApiAWS.Controllers
{
    public class AWSFilesController : Controller
    {
        private ServiceStorageS3 service;

        public AWSFilesController(ServiceStorageS3 service)
        {
            this.service = service;
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile
            (IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync
                    (file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

    }
}
