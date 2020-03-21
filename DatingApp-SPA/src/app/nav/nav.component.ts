import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

// km :model(variable) name of type any with object passed to it
  model: any = {};

  constructor( private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(
            next => {
          console.log('logged in successfully');
        }, error => {
           console.log('Failed to login');
        }
    );
  }

  loggedIn() {
    // km: We use localStorage as it is persistent storage in the browser that will still be available after a browser refresh
      const token = localStorage.getItem('token');
      // km: if something is there in token return true  or return false.
      return !!token;
  }

  logout() {
      localStorage.removeItem('token');
      console.log('Logged out');
  }

}
