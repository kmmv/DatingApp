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
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            // Have to create a new Cloudinary account using the Account class
            _cloudinary = new Cloudinary(acc);
            
        }

        [HttpGet("{id}" , Name = "GetPhoto")]

        public async  Task<IActionResult> GetPhoto(int id)
        {
            // this will get the firstorDefault photo of the user which is provided by id
            var photoFromRepo = await _repo.GetPhoto(id);

            // the photoFromRepo will have all the user properties from the navigation property
            // we dont want to send the user details so we create a PhotoForReturnDto 
            // PhotoForDetailDtop can be also used instead of creating PhotoForReturnDto
            // map the PhotoForReturnDtop from photoFromRepo
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);

        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
                                        [FromForm]  PhotoForCreationDto photoForCreationDto)
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
                // If the save is successfull we want to return the photo object (photoToReturn)
                // when save is successfull the SqlLite will create an Id on the photo
                // map the photo to the PhotoForReturnDtop
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                // to return the photo object (photoToReturn), the createdAtRoute call is required
                // The CreatedAtRoute overload we are using takes 3 parameters:
                // 1.  The name of the route to get the resource that has been created.
                // 2.  The parameters needed to be passed to the route to get the resource (in this case the ID)
                // 3.  The actual resource we want to return.
                // Parameters 1 and 2 allow the response to contain the route 
                // and params needed to get the individual response in the header 
                // and the third parameter is the photo we are returning.
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
            };

            return BadRequest("Could not add the photo");

        }

        // Another POST method to update the photo as main photo
        // We have used another POST method for RESTful conformity and for the cleaner code
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) )
                return Unauthorized();
            
            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(p=>p.Id == id))
                return Unauthorized();
            
            // get photo object
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");
            
            //  get current main photo from repo
             var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            // set the current main photo isMain flag to false
             currentMainPhoto.IsMain = false;

            // set the photoFromRepo as the main photo
             photoFromRepo.IsMain = true;

             if(await _repo.SaveAll())
                return NoContent();
                
            return BadRequest("Could not set photo to main");
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) )
                return Unauthorized();
            
            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(p=>p.Id == id))
                return Unauthorized();
            
            // get photo object
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            // the photos in cloudinary will have public ID
            if (photoFromRepo.PublicId != null )
            {
                // to delete photo we have to use cloudinary destroy method - ref documentation
                // we need DeletionParams variable to pass to the Destroy method
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinary.Destroy(deleteParams);

                // cloudinary returns a string "ok" if the destroy method is successful
                if(result.Result == "ok") {
                    _repo.Delete(photoFromRepo);
                }

            }
            
            // there are photos which are stored in random user APIs
            if (photoFromRepo.PublicId == null )
            {
                _repo.Delete(photoFromRepo);                
            }

            if( await _repo.SaveAll()) return Ok();

            return BadRequest(" Failed to delete the photo");

        }
    }
}