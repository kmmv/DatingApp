import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';


// km:services needed injectable decorator to
@Injectable({
  providedIn: 'root'
})
export class AuthService {

baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient ) { }

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
        }
      })
    );
}

}
