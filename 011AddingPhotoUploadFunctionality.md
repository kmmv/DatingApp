Module 11 - Photo upload

-Where to store photos
-Adding photo controller
-Adding file uploader to out SPA
-Setting the Main photo
-Any to any component communication
-Deleting photos

Notes 105- Where should we store the photos?
- We use cloudinary, the Process - client upload the photos to Server - Server talks to clouddinary
- Cloudinary return the URL and the URL / PublicsId back to DB
- Saved in DB and given SQl Id
- 201 Created response with location header

lab 106- Using cloudinary as a photo storage solution
*. goto https://cloudinary.com/pricing
*. check Upload documentation and signup
*. https://cloudinary.com/documentation/how_to_integrate_cloudinary
*. goto (API) -> appsettings.json and create cloudinary settings
*. goto Helpers folder and create CloudinarySettings.cs file and create CloudinarySettings CloudinarySettings
*. tie up the CloudinarySettings.cs on the Startup.cs -  
      - services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));  
*. Add a prop name PublicId on the photo.cs
*. Add migration to add the new prop to our database -  dotnet ef migrations add AddedPublicId - check the migrations added
*. Update migrations to our database dotnet ef database update

- Now we want to use Cloudinary
lab 107 - Creating the Photos Controller Part 1
*. Create new PhotosController, include attributes and bring DIs and complete controller
*. On the constructor of the PhotosController - create cloudinary account.
*. Create HttpPost method - AddPhotoForUser - call _cloudinary.Upload and save the result to the userFromRepo

lab 108 - Creating the Photos Controller Part 2
- We have to provide a root of object we just created. for this we have to create a HttpGet Method
*. Create a HttpGet method named GetPhoto
*. goto IDatingRepository and add GetPhoto method
*. photoFromRepo will have the userdetails from navigation property so we need to create a PhotoForReturnDto
*. use the automapper to map the photoFromRepo to the PhotoForReturnDto
*. return OK(photo)
*. goto AutoMapperProfile and CreateMap(Photo, PhotoForReturnDto) and also map for PhotoForCreationDto
*. complete the AddPhotoForUser by using SaveAll

lab 109 - Testing the Photo upload with postman
*. run the application on the debug mode, goto postman -
*. add a post request - http://localhost:5000/api/users/1/photos
*. and set Authorization bearer token on the headers
*. on the body add File and values as the image file to upload
*. Click Send button and postman will throw error -
*. to hit the debug point add the attribute [FromForm] to the AddPhotoForUser
*.
