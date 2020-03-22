import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';


@Injectable()

export class ErrorInterceptor implements HttpInterceptor {
    intercept(
        req: import('@angular/common/http').HttpRequest<any>,
        next: import('@angular/common/http').HttpHandler

    ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {

                // km: 401 error handling
                if (error.status === 401) {
                    return throwError(error.statusText);
                }

                // km: 500 type error
                if (error instanceof HttpErrorResponse) {
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        return throwError(applicationError);
                    }
                }

                // model state error
                // error.error is what we get from as angular response
                const serverError = error.error;
                let modalstateErrors = '';
                // looking for objects inside the errors array
                if (serverError.errors && typeof serverError.errors === 'object') {
                    for (const key in serverError.errors) {
                        if (serverError.errors[key]) {
                            modalstateErrors += serverError.errors[key] + '\n';
                        }
                    }
                }

                return throwError(modalstateErrors || serverError || 'Server Error');

            })
        );
    }
}


// km: export our error interceptor providers to add the prooviders insode the app module
export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
