import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()

// resolve array of users
export class MemberListResolver implements Resolve<User[]> {

    constructor(private userService: UserService,
                private router: Router,
                private alertify: AlertifyService) {
    }

    // return Observable array of users
    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        // invoke the userService to get the users
        return this.userService.getUsers()
        .pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                // navigate back to home in case of error
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
