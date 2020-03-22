In the API project

-In the configuration services on the startup class
-this will allow global exception
// the UseExceptionHandler will pipe all the exception to the builder which is middleware
              app.UseExceptionHandler(builder => {
                  // Run is http request and response
                  builder.Run(async context => {
                      context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                      // the error will store the particular error
                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if(error != null) {
                          // add own our extention method
                          await context.Response.WriteAsync(error.Error.Message);
                      }
                  });

-The issue with the method above is when trying to check the browser
-When we go to exceptionHandler we re executing from different pipeline
-we need to modify the response to that we can add custom error

lab - 52- Setting up the Global (Add extention to context.Response)
***

*.Add Helpers folder to the project
*.Extension.cs inside the Helpers

Type of errors to be handled from Angular
*****************************************
Access-Control-Expose-Headers
Application-Error

lab-53 Handling errors in Angular
***
