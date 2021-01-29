using Microsoft.AspNetCore.Http;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class UploadContactImageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public string ContactId { get; set; }

        public IFormFile File { get; set; }

        public UploadContactImageModel()
        {
        }

        public UploadContactImageModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }

    }
}