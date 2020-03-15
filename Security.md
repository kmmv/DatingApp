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
