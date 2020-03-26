todo:
Extending the user class,
more migrations,
cascade delete,
seeding data into our database,
creating a new repository
using automapper

lab: 70 Extending the User Model
***
*. Modify user class to add more properties. Add photo model/properties and add this to datacontext
*.  dotnet ef migrations add ExtendedUserClass

lab: 71 Exploring Entity Framework migrations
***
*.  dotnet ef migrations help  - to view the help options
*.  dotnet ef migrations list - to view all the existing migrations
*.  dotnet ef migrations remove - to remove latest migration
*.  dotnet ef migrations update - to update the migration to the database

NOTE - solving issues with migrations - 71 Exploring Entity Framework migrations
Attempting to go to earlier migration once the migration is applied.
*. after we update the migrations
*. dotnet ef database drop -- remove database
*. dotnet ef database remove -- will last migrations
*. dotnet ef database update -- will update the database again (no data though)

lab - 72 Entity Framework Relationsships
***
*. On the photo model add virtual User and UserdId so that cascade delete is defined in the migrations
    -this means all the photos related with the user is deleted when the user is deleted
*. dotnet ef migrations add ExtendedUserClass
*. dotnet ef database update

lab - 73 Seeding Data to the database - part 1
***
*. google json generator - https://www.json-generator.com/
*. change it and match our data to make some data
*. use seeding data on the site and get the values
lab -74
***
*.create seed.cs inside the data folder and complete the cs file
*.inside the program.cs - get the services like DI using scope and services
*. call seed.seedusers(context)
*. stop the server and issue dotnet ef database drop (to drop our database)

lab - 75 Creating a new repository for our applied
***
*. Inside the server go to data folder and create a new interface named IDatingRepository
*. 5 methods for IDatingRepository
*. Implement the interface using DatingReposity class on the same folder
*. note:include method on the _context.users to retrieve photos as well
*. add this repository to the startup class to use this - services.AddScoped<IDatingRepository, DatingRepository>();

Users controller so that we can extract data via the new repository to the client
**************
Notes
*****
Newtonsoft.Json is used to serialize the JSON responses before .NET core 3.0
System.Text.Json was introduced by Microsoft to replace Newtonsoft.Json
Newtonsoft.Json is still needed for our application because System.Text.Json hasn't got all features
https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio
https://dotnetcoretutorials.com/2019/12/19/using-newtonsoft-json-in-net-core-3-projects/

lab - 76 Creating the Users controller
***
*. command palete => Microsoft.AspNetCore.Mvc.NewtonsoftJson
*. On the configure services or startup class - services.AddControllers().AddNewtonsoftJson();
*. Add a new controller named UsersController and implement GetUsers
*. Add  opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    -  this line is to elminate the -self referencing loop error

lab -77 Shaping the data to return with DTO
**
*. Create UserForList and UserForDetail dto classes

- UserForList dto is what we show from the values of User. To use the values mapping is required
- it is tedius to map all properties on the controller so we use automapper
- AutoMapper automatically maps all the fields between the model class and the same dto classes
- however if the fields are

lab - 78 Automapper Part 1
***
*. install AutoMapper.Extensions.Microsoft.DependencyInjection using PM
*. Add AutoMapper as service on the startup class
*. using AutoMapper
*. services.AddAutoMapper(typeof(DatingRepository).Assembly); - one of our classes.Assembly
*. On the UserController - bring automapper in the controller
*. _mapper.Map<UserForDetailedDto>(User); to map
*. Create AutoMapperProfiles inside the helper folder
*. Create  CreateMap<User, UserForListDto>(); inside the contructor

To resolve the issues - age is returning 0 and photo is returning user object
lab - 79 AutoMapper Part 2
***
*. after implementing automapper restart the server
*. CreateMap<User, UserForListDto>()  .ForMember(dest =>dest.PhotoUrl, opt => opt.MapFrom(src=> src.Photos.FirstOrDefault(p =>p.IsMain).Url));
*. Similarly CreateMap for date but there is no built-in c# method to calculate age so we need extension property for age
*. Create an extension property to calculate age for DateTime - inside the Helper\Extensions.cs
       - public static int CalculateAge(this DateTime theDatetime)
