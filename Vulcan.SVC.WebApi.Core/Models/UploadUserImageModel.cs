using Microsoft.AspNetCore.Http;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class UploadUserImageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public IFormFile File { get; set; }

        public UploadUserImageModel()
        {
        }

        public UploadUserImageModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }
    }
}
