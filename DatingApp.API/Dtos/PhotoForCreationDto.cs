using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{

    // What we are going to send API is different from our photo class because Id, Url are invalid at the time
    // So we are create PhotoForCreation
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        // Represents a file sent with the HttpRequest - from Microsoft.AspNetCore.Http
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}