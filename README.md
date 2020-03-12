# DatingApp
Two projects
1.WebApi to serve dating app

Prerequisite
************

install dotnet core 3.0
install vscode
install node
install npm (comes with node)
install sqllite

Todo: Walking skelton
  DB->ORM->API->SPA

DotNet core WebAPI using DotNetCLI
**********************************
  dotnet new -h =>different templates to create new apps

Create new webapi application
*****************************
  dotnet new webapi -n DatingApp.API

Tip: Add vscode to the path cmd+shiftP and type code

Tip: hide obj and bin by going to preferences on the vscode

Tip: dotnet watch run like nodemon

Adding EntityFramwork
************************
nuget PM->
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.Design

Add Models and Controller
**************************

 public void ConfigureServices(IServiceCollection services) contains configurations
 add
 "ConnectionStrings" : {
    "DefaultConnection": "DataSource=datingapp.db"
 },

 Add Data->DbContext
 *****************
 cstor-> public DataContext(DbContextOptions<DataContext> options): base (options) {}


On startup.cs=> public void ConfigureServices(IServiceCollection services)
**************
services.AddDbContext<DataContext>(x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

Add DbContext to the ValuesController
**************************************
private readonly DataContext _context;
public ValuesController(DataContext context)
{
    _context = context;

}

EntityFramework Migration
**************************
install dotnet EF tool using this command on terminal
            dotnet tool install --global dotnet-ef 3.0.0
          // dotnet tool uninstall  --global dotnet-ef

 mohan$ dotnet ef migrations add InitialCreate
 //  To undo this action, use 'ef migrations remove'

this will create Migration folder and files - check migration table

Create DB from migration
********************
dotnet ef database update
This will create a .db database file

Create Constructor on the ValuesController for the DbContext
************************************************************

ctor=>

IActionResult //km:IActrionResult will return HTTP return values
