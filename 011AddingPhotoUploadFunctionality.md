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

lab 110 - Creating the Photo upload component in Angular
*. goto SPA app and inside the members folder
*. add new component photo-editor and add the component to as app.module.ts
*. Add @input Photo[] and goto to .html file using Shift+Alt+O
*. complete the photo-editor.component.html to display the photos of the array
*. goto the member-edit.compoenent.html and add  <app-photo-editor  under the Edit photos tab
*. goto the photo-editor.component.css add stylings

- Now we need to add the functionality to upload the photos via SPA.

lab 111 - Adding a 3rd party File Uploader
*. https://valor-software.com/ng2-file-upload/
*. install ng2-file-upload, add variables and complete photo-editor.component.ts
*. add FileUploadModule to the imports section of the app.Module to resolve the [uploader] error

- solving CORS error
- on the startup.cs
- usercors must be above auth and endpoints - even though this allows everthing this is not a security risk
- because we are not storing the credentials

lab 112 Configuring the 3rd Party file Uploader
***
*. from the github issues page - recommend to extend our Uploader
*. on the photo-editor.component.ts this.uploader.onAfterAddingFile = (file) = {file.withCredentials = false;}
*. complete the html on the photo-editor.component.html


- To display which is the main photo
-
lab 113 Adding the Set Main photo functionality
*. on the API controller create POST method SetMainPhoto on the PhotoController
*. Add GetMainPhotoForUser on the IDatingRepository to support SetMainPhoto
*. After completing SetMainPhoto, update the main photo by testing using PostMan

lab 114 Adding the Set Main photo functionary on the SPA
*. On the SPA, src\_services\user.services.ts ass setMainPhoto
*. Goto photo-editor.component.ts import userService in the constructor
*. And add setMainPhoto to set the photo
*. goto photo-editor.component.html
*. add (click)="setMainPhoto(photo)" , disabled and styling functionality to the button

- now the main photo is set but this is only visible when the browser is refreshed
- array filter method, because it is returning an array we have to set an element by specifying [0]

lab 115  Using the array filter method to provide instant feedback in the SPA
*. inside the subscribe method of setMainPhoto
*. The button is changed to green and the currentMain variable is set by the following code
*. this.currentMain = this.photos.filter(p => p.isMain === true)[0];

- Now we need to display the photo on the main photo box without refreshing the browser

lab 116 Output properties revisited
*. ON the photo-editor.component add an output property  
*. @Output() getMemberPhotoChange = new EventEmitter<string>();
*. Add emitter function this.getMemberPhotoChange.emit(photo.url); inside setMainPhoto
*. Implement the (getMemberPhotoChange)="updateMainPhoto($event)" inside the member-edit.compoenent.html
*. Implement the updateMainPhoto (from above line) insdiee member-edit.compoenent.ts

-Different options for Navbar - we have to think of how to get the user photo and display on the Navbar
- Network call is possible and pull the photo but this is expensive so we avoid this method
- Another way we are going to use - inside AuthController along returning  claims, we return photo
lab 117 Adding the main photo to the Nav bar
*. Bring IMapper on the contructor of AuthController
*. map a userListDto for the purpose of returning with the token -  _mapper.Map<UserForListDto>(userFromRepo);
*. return the token and user.
*. In order for the user object to contain the photo add Users.Include(p=>p.Photos) inside Login method
*. add currentUser to the AuthService
*. on the ngInit of the app.component.ts set AuthService.currentUser
*. logout method inside nav.component.ts add
        - localStorage.removeItem('user') and authService.currentUser = null;
*. goto to navcomponent.html and inside the loggedIn() method add  image to display photos
        - <img src="{{authService.currentUser.photoUrl}}" alt="">
*. Lastly style the img tag to adjust the size inside nav.compoenent.class

118 Any to Any component communicate in Angular
- Now we have to fix next issue - when we change the main photo using edit
- it needs to change the nav bar photo as well
- Sometimes we need to communicate to another component where there is no relationship
- Services are designed to communication with any component
- We need to create a  (Behaviour  Subject ) which is a type of Observable

119 Using BehaviourSubject to add any to any communication to our app
*. goto Authservice.ts import {BehaviorSubject } from 'rxjs'
*. create photoUrl = new BehaviorSubject<string>('../../assets/user.png');
*. add the user.png under assets
*. create currentPhotoUrl = this.photoUrl.asObservable();
*. Add   changeMemberPhoto(photoUrl: string) to the AuthService login Method
*. goto nav.component.ts and add photoUrl
*. in ngOnInit add this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
*. goto nav.component.html use photoUrl on img tag
*. goto app.component.ts add this.authService.changeMemberPhoto(user.photoUrl); inside ngOnInit
*. goto member-edit.component add variable photoUrl
*. goto NgInit and this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
*. goto member-MemberEditComponent.html change user.photoUrl to photoUrl inside subscribe method
*. set currentuser.photourl and localhost inside the subscribe method
