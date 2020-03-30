import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()

// resolve an user (1 user)
export class MemberDetailResolver implements Resolve<User> {

    constructor(private userService: UserService,
                private router: Router,
                private alertify: AlertifyService) {
    }

    // return an user (observable)
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // we need to pass an id to resolbe one user
        return this.userService.getUser(route.params.id)
        .pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                // navigate to members page in case of error
                this.router.navigate(['/members']);
                // of is the type observable
                return of(null);
            })
        );
    }
}
