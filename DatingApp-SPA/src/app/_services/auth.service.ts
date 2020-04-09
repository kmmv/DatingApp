import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../_models/user';

// km:services needed injectable decorator to
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      // km if there is any response, store the token from the body
      map((response: any) => {
        const user = response;
        if (user) {
          // km: https://www.w3schools.com/jsref/prop_win_localstorage.asp
          // localstorage stored the item on the browser without expiration but sessionstorage expires when the browser tab
          // is closed
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          console.log(this.decodedToken);
        }
      })
    );
  }

  // km model is going to store the username and password
  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    // km: We use localStorage as it is persistent storage in the browser that will still be available after a browser refresh
    const token = localStorage.getItem('token');
    // km: if token is not expired - checking using @auth0/angular-jwt
    return !this.jwtHelper.isTokenExpired(token);
  }

}
