import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';
import { reduce } from 'rxjs/operators';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() photos: Photo[];
@Output() getMemberPhotoChange = new EventEmitter<string>();

uploader: FileUploader;
hasBaseDropZoneOver = false;
baseUrl = environment.apiUrl;
// variable to hold the current photo
currentMain: Photo;

  constructor(private authService: AuthService,
              private userService: UserService,
              private alertify: AlertifyService) {}

  ngOnInit() {
    this.initializeUploader();
  }


  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url:
      this.baseUrl +
      'users/' +
      this.authService.decodedToken.nameid +
      '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    // extend this function to eliminate the CORS error
    // because we are not sending up out file with credentials now wecan get past the CORS error
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    // the following method is to refresh photo after the photo upload without browser refresh
    // the following method will be ivoked when a response is provided from the API after a successful upload
    // building an photo object from the response
    this.uploader.onSuccessItem = (item, response, status, header) => {
        if (response) {
          const res: Photo = JSON.parse(response);
          const photo = {
             id:  res.id,
             url : res.url,
             dateAdded : res.dateAdded,
             description : res.description,
             isMain : res.isMain
          };
          // push the photo into the photos array
          this.photos.push(photo);
        }
    };

    }

    // this method will set the main photo
    setMainPhoto(photo: Photo) {
      this.userService
      // the below line will set the main photo invoking the userservice method
      .setMainPhoto(this.authService.decodedToken.nameid, photo.id)
      .subscribe(
          () => {
            // The following code is required to update the button (to green)
            // array filter method, because it is returning an array we have to set an element by specifying [0]
            this.currentMain = this.photos.filter(p => p.isMain === true)[0];
            this.currentMain.isMain = false;
            photo.isMain = true;
            // this.getMemberPhotoChange.emit(photo.url);
            this.authService.changeMemberPhoto(photo.url);

            // retain after refresh
            this.authService.currentUser.photoUrl = photo.url;
            localStorage.setItem('user', JSON.stringify(this.authService.currentUser));

            // The following will set the photo
            console.log('Successfully set to main');
          }, error => {
             this.alertify.error(error);
          }
      );
    }

}
