How to store the password without compromising?
HASH + salt the password
Salt make sure the same password is stored as different hashes

Repository Pattern
******************
mediates datasource layer and business layers of the application.
Advantages - minimize duplicate query logic, decouple application

************** Lab start

Step1: Create User in model class
Step2 : Add migration files to the project$dotnet ef migrations add AddedUserEntity
        -- Now the migrations files for the users will be Added
Step3: Update DB using migration $dotnet ef database update

-- Next the interface will be the Repository pattern
Step4 : Create IAuthRepository
Step5 : Implement Concreate class for the IAuthRepository
          - AuthoRepository (Login, Register, UserExists) , Hash/Salt
Step6 : Registering the services in the Startup class
        --services.AddScoped<IAuthRepository, AuthRepository>();
Step7 : Create a AuthController and inherit from ControllerBase
Step8 : Create Dtos folder and Create UserForRegisterDto
Step9 : Create Register method with UserForRegisterDto
Step10: Debugger with postman
{
"username":"john",
"password":"password"
}

Token Authentication (Refer: 34-Token authentication )
********************
Usage of token: The server doesn't need to go to database to validate the user
JWT - JSON Web Tokens - structure (Header / Payload / Secret) - can be decoded by anyone
Header contains the info about the encryption type
Payload contains the info
Secret is used to hash the header and payload,  stored on the server (client can decode
  header and payload without knowing the secret)

Lab
***
Step1: Install Microsoft.IdentityModel.Tokens.Jwt and System.IdentityModel.Tokens.Jwt using PM
Step2: Create UserForLoginDto.cs
Step3: Create [HttpPost("login")] method
Step4: inside the post method
            -Check user in the database, if not give an unauthorised error, else create token
            -Create Claim, Create key and Create Credentials
            -Create SecurityTokenDescriptor using Subject, Expires and SigningCredentials
            -Create a JwtSecurityTokenHandler
            -With the token handler CreateToken and then WriteToken
            -and return the result from the WriteToken
Step5: Test login funciton using PostMan and then the token will be retuned back
Step6:Goto https://jwt.io/ and paste the token to see the decoded information

Authentication Middleware
*************************
Everywhere we need to authorize
Lab
***
    Step1:Add PM->Microsoft.AspNetCore.Authentication.JwtBearer
    Step2:Add Authorize attribute in ValuesController
    Step3:How to Authentication, that is Middleware services.AddAuthentication
