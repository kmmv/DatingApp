using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    // to make sure none the roots are available without authorizatinon to anonymous users
    [Authorize]

    // route to get to this controller - need to inherit from ControllerBase
    [Route("api/users/{userId}/photos")]

    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo,
                                IMapper mapper,
                                // IOptions because we setup this on the startup class
                                IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

           // Fill cloudinary account details on this class
            Account acc  = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            // Have to create a new Cloudinary account using the Account class
            _cloudinary = new Cloudinary(acc);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            // as we are authorising we want to check the token - copy the authorisation frm UsersController
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            // use the variable to store result we get back from cloudinary - comes from CloudinaryDotNet.Actions;
            var uploadResult = new ImageUploadResult();

            // check if the file contains something
            if(file.Length > 0)
            {
                using(var stream =file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        // Another thing we want to transform any image to a square image
                        Transformation = new Transformation()
                                .Width(500).Height(500).Crop("fill").Gravity("face")

                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                    // at this time we will get uploadResult from Cloudinary
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // map the photoForCreationDto into our Photo
            var photo = _mapper.Map<Photo>(photoForCreationDto);

            // If this is first photo then set this as the main photo
            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            // Add the photo to the repo
            userFromRepo.Photos.Add(photo);

            // save the repo
            if(await _repo.SaveAll())
            {
                return Ok();
            };

            return BadRequest("Could not add the photo");

        }


    }
}