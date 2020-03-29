# DatingApp
Two projects
1.WebApi to serve dating app

Major Tips
***********
To find and kill background process
lsof -i @localhost:5000
kill -9 <<PID>>


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


Tip:Install Angular and create new project (Need NodeJS)
--npm install -g @angular/cli
--ng new my-dream-app
--ng servesudo
Tip:Angluar install latest typescript

Create Angular Project
**********************
ng new DatingApp-SPA ('-' on the project name as it is a Javascript project )

Inside Angular Project
**********************
Notes
   app.module.ts @NgModule
   app.module.tx bootstrap [AppComponent]
   app.component.ts -> Angular app component class which lists templateURl and styleUrl
   @ ->Decorator. JS class decorated with angular features
   main.ts -> bootstrap the appmodule component
   platformBrowserDynamic -> Browser application

behind the scenes angular cli use Javascript

ng server to build and run the application

Install extentions

angular typescript snippets     - adds snippets for angular
angular files (angular2-files)  - scaffold, generate component
angular language service        - goto definition, intellisense, completion list
angular2-switcher               - to switch between .ts/html/css/spec
auto rename tag           - rename the closing tag while updating opening tag
bracket pair colorizer          - opening and closing brackets color
debugger for chrome             - debugger shown on vsccode
material icon theme     - for enhanced UI
path intellisense         -
prettier - code formatter - quickly format JS or typescript code
tslint                      - inter those errors alongside the code

ext install danwahlin.angular2-snippets
ext install alexiv.vscode-angular2-files
ext install Angular.ng-template
ext install infinity1207.angular2-switcher
ext install CoenraadS.bracket-pair-colorizer
ext install msjsdiag.debugger-for-chrome
ext install PKief.material-icon-theme
ext install christian-kohler.path-intellisense
ext install esbenp.prettier-vscode
ext install ms-vscode.vscode-typescript-tslint-plugin

Generate ValueComponent
**************************

Import HttpClient on the ValueComponent
*************************************
//Dependency injecttion to get values from WebAPI
constructor(private http: HttpClient) { }
//getValues function on the value component contains the observable
//Add the HttpClientModule on the app.module.ts

<p *ngFor="let value of values">
{{value.id}} , {{value.name}}
</p>


Allow CORS on the startup file of DatingApp.API
*******************************
services.AddCors(); and    
//km: usercors must be above auth and endpoints
app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );


Intall Bootstrap
**********
npm install bootstrap font-awesome
add these lines inside styes.cpp
@import '../node_modules/bootstrap/dist/css/bootstrap-reboot.min.css';
@import '../node_modules/font-awesome/css/font-awesome.min.css';
