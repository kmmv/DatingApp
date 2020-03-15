How to store the password without compromising?
HASH + salt the password
Salt make sure the same password is stored as different hashes

Repository Pattern
******************
mediates datasource layer and business layers of the application.
Advantages - minimize duplicate query logic, decouple application

************** Lab start

Step1: Create User in model class
Step2 : Update DB using migration: dotnet ef migrations add AddedUserEntity



Step1 : Create IAuthRepository
Step2 : Implement AuthoRepository (Login, Register, UserExists) , Hash/Salt
