import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';
import { reduce } from 'rxjs/operators';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() photos: Photo[];
uploader: FileUploader;
hasBaseDropZoneOver = false;
baseUrl = environment.apiUrl;

  constructor(private authService: AuthService) {}

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

}
