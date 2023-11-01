# Example 5 - ASP .Net Angular

# Table of Contents

* [Inspired By](#inspired-by)
* [Create](#create)
* [Project Settings](#project-settings)
* [Project Structure](#project-structure)
* [VSCode](#vscode)
* [Changes](#changes)
  * [Project Structure](#project-structure-1)
    * [Angular](#angular)
    * [Asp.Net](#aspnet)
  * [Books API](#books-api)
  * [Angular Routing](#angular-routing)
  * [Books Module](#books-module)
  * [Books Service](#books-service)
  * [Books Views](#books-views)
    * [Book List View](#book-list-view)
    * [Book Details View](#book-details-view)
    * [Add Book View](#add-book-view)
    * [Update Book View](#update-book-view)
    * [Delete Book View](#delete-book-view)
  * [Books Sub Routing](#books-sub-routing)

# Inspired By

[Building Angular and ASP.NET Core Applications by Ervis Trupja](https://www.linkedin.com/learning-login/share?account=42439657&forceAccount=false&redirect=https%3A%2F%2Fwww.linkedin.com%2Flearning%2Fbuilding-angular-and-asp-dot-net-core-applications%3Ftrk%3Dshare_ent_url%26shareId%3DLGCbJ3RWTU6QfKRiLPcpWw%253D%253D)

# Create

Create a new ASP.NET project with Angular.
~~~
dotnet new angular --no-https --framework net6.0 --name example-5 --output ./src
~~~

Add entry to `.gitignore` file
~~~
.angular
~~~

Change in `src/Properties/launchSettings.json` the applicationUrl in `example_5` to http://0.0.0.0:5080
~~~json
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:13408",
      "sslPort": 0
    }
  },
  "profiles": {
    "example_5": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5080",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy"
      }
    }
  }
}
~~~

# Project Settings

Setting the [`MSBUildProjectExtensionsPath`](https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets), `BaseOutputPath` (`bin`) and `BaseIntermediateOutputPath` (`obj`) properties in the [`Directory.Build.Props`](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2019) file, which is located in the root directory of your solution.
~~~XML
<Project>
  <PropertyGroup>
    <MSBUildProjectExtensionsPath>$(MSBuildProjectDirectory)\..\build\obj\$(MSBuildProjectName)</MSBUildProjectExtensionsPath>
    <BaseOutputPath>$(MSBuildProjectDirectory)\..\build\bin\$(MSBuildProjectName)</BaseOutputPath>
    <BaseIntermediateOutputPath>$(MSBuildProjectDirectory)\..\build\obj\$(MSBuildProjectName)</BaseIntermediateOutputPath>
  </PropertyGroup>
</Project>
~~~

# Project Structure

The `ClientApp` directory contains a standard Angular CLI app. See the official Angular documentation for more information.

By default, the Angular application is created inside a `ClientApp` folder.

It is set inside the `SpaRoot` (Single page application) property in the`*.csproj` file. If you rename the `ClientApp` folder, you must change the property value in this project file as well. 

Angular running port is set inside the `SpaProxyServerUrl` property. If you want to change the Angular running port number, you can change inside this property. At the same time, you must change the `package.json` file of Angular application also.  

`example-5.csproj`:
~~~XML
<PropertyGroup>
    <SpaRoot>ClientApp\</SpaRoot>
    <SpaProxyServerUrl>http://localhost:44463</SpaProxyServerUrl>
    <SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>
</PropertyGroup>
~~~

- `ClientApp/`- Angular application
  - `src/`
    - `app/` - Contains `modules` and `components` for our Angular application.
      - `counter/` - page which allows client side counting
      - `fetch-data/` - page to retrieve data through the web API. 
      - `home/` - page which provides some basic information
      - `nav-menu/`- navigation component to switch pages
      - `app.component.css` - Contains the CSS code for the component.
      - `app.component.html` - HTML file pointing to the app component. It is a template for the angular application. Shows the routing page, which will be home by default.
      - `app.component.spec.ts` - Unit testing file associated with app component. It can be generated using `ng test` command.
      - `app.component.ts` - Entire functional logic is written in this file. 
      - `app.module.ts` - TypeScript file holds all dependencies. Here we will use `NgModule` and define the Bootstrap component when loading the application.
      - `app.server.module.ts` - [used for server-side rendering](https://angular.io/guide/universal#why-use-server-side-rendering).
    - `assets/` - Here we will keep resources such as images, styles, icons, etc.
    - `environments/` - It contains the environment configuration constants that help while building the angular application. It has `environment.ts` and `environment.prod.ts`. These configurations are used in `angular.json` file. 
    - `index.html`
  - `.browserslistrc` - Browser compatibility and versions are mentioned in this file. This configuration is pointed to in our `package.json` file.
  - `.editorconfig` - This file deals with consistency in code editors to organize some basics such as indentation and whitespaces. More like code formatting.
  - `angular.json` - This file defines the structure of our application. It includes settings associated with our application. Also, we can specify the environments on this file. For example, development, production, etc.
  - `karma.conf.js` - This is the configuration file for the Karma Test Runner. It is used in Unit Testing.
  - `package.json` - This is the npm configuration file. All the dependencies mentioned in this file. We can modify dependency versions as per our need on this file.
  - `package-lock.json` - Whenever we change something on the node_modules or package.json, this file will be generated. It is associated with npm.
  - `proxy.conf.js` - configuration file which has the ASP.NET backend controller names.
  - `README.md` - This file is created by default. It contains our project description. It shows how to build and which Angular CLI version has been used.
  - `tsconfig.app.json` - This configuration file overrides the `tsconfig.json` file with relevant app-specific configurations.
  - `tsconfig.json` - TypeScript compiler configuration file. This is responsible for compiling TypeScript to JavaScript so that the browser will understand.
  - `tsconfig.spec.json` - This file overrides the tsconfig.json file with app-specific unit test configurations while running the `ng test` command.
- `Controllers` - The REST API controllers.
- `Pages`
- `Properties`
  - `launchSettings.json` - profiles of debug settings.
- `wwwroot` - Static files can be stored in any folder under the web root and accessed with a relative path to that root.
  - `favicon.ico` - Replaces the favicon of the angular project.
- `appsettings.Development.json`
- `appsettings.json`
- `example-5.csproj` - C# project file.
- `Program.cs` - Main C# application.
- `WeatherForecast.cs` - Model for API function

The project structure is like a normal Angular project, but it has two modules (`app.module.ts` and `app.server.module.ts`)and also a new file named `proxy.conf.js`. This is a [configuration](https://angular.io/guide/build#proxying-to-a-backend-server) file which has the ASP.NET backend controller names. We must add any controller as an entry in the Angular proxy configuration file inside the context property. Otherwise, API call from Angular will fail.  

# VSCode

`launch.json`:
~~~json
{
  "name": "Example 5 Debug",
  "type": "coreclr",
  "request": "launch",
  "preLaunchTask": "build-example-5",
  "program": "${workspaceFolder}/example-5/build/bin/example-5/Debug/net6.0/example-5.dll",
  "args": [],
  "cwd": "${workspaceFolder}/example-5/src",
  "stopAtEntry": false,
  "logging": {
    "moduleLoad": false
  },
  "serverReadyAction": {
    "action": "openExternally",
    "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
  },
  "env": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  },
  "sourceFileMap": {
    "/Views": "${workspaceFolder}/Views"
  }
}
~~~

`task.json`
~~~json
{
  "label": "build-example-5",
  "command": "dotnet",
  "type": "process",
  "args": [
    "build",
    "${workspaceFolder}/example-5/src/example-5.csproj",
    "/property:GenerateFullPaths=true",
    "/consoleloggerparameters:NoSummary"
  ],
  "problemMatcher": "$msCompile"
}
~~~

# Changes

After the initial creation we reorder the files a little bit, add a book service and provide a UI which uses the service. 

## Project Structure

### Angular

The Angular `ClientApp` project struture shall be setup as below:

- `src/`
  - `app/`
    - `<module a>/` - A module containing the basic setup (routing, css, html, ts, module). In our case the `books` module. Each module shall present more or less a page.
    - `<module b>/` - Another module (not created in this example)
    - `components/` - Contains all components
      - `app/` - Components of the root app module.
      - `<module a>/` - Components used only in the module `a`. In our case the `books` module.
      - `<module b>/` - Components used only in the module `b`. Another module (not created in this example).
    - `<common components>` - Components used across modules.
- `services/` - Services used
- `<app module>` - The main module containing the basic setup (routing, css, html, ts, module).

Create the `components/app` directory and move the `nav-menu`, `counter`, `fetch-data`, `home` ` folders inside.

Create the `services` directory.

### Asp.Net

- `ClientApp/` - The angular project.
- `Controllers/` - Contains the controllers to provide the HTTP interface e.g. `WeatherForecastController`. 
- `Models/` - Contains class models e.g. `WeatherForecast`.
- `Pages/`
- `Properties/`
- `Services/` - Contains services which provide functions and data for the controllers.
- `wwwroot`

Create `Models` directory and move `WeatherForecast.cs` inside the folder.

Create `Services` directory.

## Books API

First of all we will create the ASP.NET book service.

Therefore create the book class inside `src/Models/Book.cs`
~~~C#
namespace example_5
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double? Rate { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateRead { get; set; }

    }
}
~~~

Than create the `IBookService` (`src/Services/IBookService.cs`) interface with the `BookService` (`src/Services/BookService.cs`) including sample data (`src/Services/Data.cs`).
~~~C#
namespace example_5
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        void UpdateBook(int id, Book newBook);
        void DeleteBook(int id);
        void AddBook(Book newBook);
    }
}
~~~

~~~C#
namespace example_5
{
    public class BookService : IBookService
    {
        public void AddBook(Book newBook)
        {
            Data.Books.Add(newBook);
        }

        public void DeleteBook(int id)
        {
            var book = Data.Books.FirstOrDefault(n => n.Id == id);
            Data.Books.Remove(book);
        }

        public List<Book> GetAllBooks()
        {
            return Data.Books.ToList();
        }

        public Book GetBookById(int id)
        {
            return Data.Books.FirstOrDefault(n => n.Id == id);
        }

        public void UpdateBook(int id, Book newBook)
        {
            var oldBook = Data.Books.FirstOrDefault(n => n.Id == id);
            if (oldBook != null)
            {
                oldBook.Title = newBook.Title;
                oldBook.Author = newBook.Author;
                oldBook.Description = newBook.Description;
                oldBook.Rate = newBook.Rate;
                oldBook.DateStart = newBook.DateStart;
                oldBook.DateRead = newBook.DateRead;
            }
        }
    }
}
~~~

~~~C#
namespace example_5
{
    public static class Data
    {
        public static List<Book> Books => allBooks;
        static List<Book> allBooks = new List<Book>()
        {
            new Book()
            {
                Id=1,
                Title="Managing Oneself",
                Description="We live in an age of unprecedented opportunity: with ambition, drive, and talent, you can rise to the top of your chosen profession, regardless of where you started out...",
                Author= "Peter Ducker",
                Rate= (double)4.9,
                DateStart = new DateTime(2019,01,20),
                DateRead = null
            },
            new Book()
            {
                Id=2,
                Title="Evolutionary Psychology",
                Description="Evolutionary Psychology: The New Science of the Mind, 5th edition provides students with the conceptual tools of evolutionary psychology, and applies them to empirical research on the human mind...",
                Author= "David Buss",
                Rate= (double)4.8,
                DateStart= null,
                DateRead= null
            },
            new Book()
            {
                Id=3,
                Title="How to Win Friends & Influence People",
                Description="Millions of people around the world have improved their lives based on the teachings of Dale Carnegie. In How to Win Friends and Influence People, he offers practical advice and techniques for how to get out of a mental rut and make life more rewarding...",
                Author= "Dale Carnegie",
                Rate= (double)4.9,
                DateStart= new DateTime(2019,02,23),
                DateRead= new DateTime(2019,03,23)
            },
            new Book()
            {
                Id = 4,
                Title = "The Selfish Gene",
                Description = "Professor Dawkins articulates a gene’s eye view of evolution. A view giving center stage to these persistent units of information, and in which organisms can be seen as vehicles for their replication. This imaginative, powerful, and stylistically brilliant work not only brought the insights of Neo-Darwinism to a wide audience. But galvanized the biology community, generating much debate and stimulating whole new areas of research...",
                Author = "Richard Dawkins",
                Rate = (double)4.4,
                DateStart = null,
                DateRead = null,
            },
            new Book()
            {
                Id = 5,
                Title = "The Lessons of History",
                Description = "Will and Ariel Durant have succeeded in distilling for the reader the accumulated store of knowledge and experience from their five decades of work on the eleven monumental volumes of The Story of Civilization...",
                Author = "Will & Ariel Durant",
                Rate = (double)4.3,
                DateStart = new DateTime(2019,05,16),
                DateRead = null
            },
            new Book()
            {
                Id = 6,
                Title = "Kon Tiki",
                Description = "“Kon-Tiki” is the record of an astonishing adventure across the Pacific Ocean. Intrigued by Polynesian folklore, biologist Thor Heyerdahl suspected that the South Sea Islands had been settled by an ancient race from thousands of miles to the east...",
                Author = "Thor Heyerdahl",
                Rate = (double)4.4,
                DateStart = new DateTime(2019,06,26),
                DateRead = new DateTime(2019,06,26),
            },
            new Book()
            {
                Id = 7,
                Title = "Civilization & It’s Discontents",
                Description = "Sigmund Freud enumerates the fundamental tensions between civilization and the individual. The primary friction stems from the individual’s quest for instinctual freedom and civilization’s contrary demand for conformity and instinctual repression...",
                Author = "Sigmund Freud",
                Rate = (double)4.0,
                DateStart = new DateTime(2019,07,19),
                DateRead = new DateTime(2019,07,19),
            },
            new Book()
            {
                Id = 8,
                Title = "The Story of The Human Body",
                Description = "This ground-breaking book of popular science explores how the way we use our bodies is all wrong...",
                Author = "Daniel Lieberman",
                Rate = (double)4.1,
                DateStart = new DateTime(2019,08,19),
                DateRead = new DateTime(2019,08,19),
            },
            new Book()
            {
                Id = 9,
                Title = "The Story of The Human Body",
                Description = "You want fewer distractions and less on your plate. The daily barrage of e-mails, texts, tweets, messages, and meetings distract you and stress you out. The simultaneous demands of work and family are taking a toll. And what’s the cost? Second-rate work, missed deadlines, smaller paycheques, fewer promotions and lots of stress...",
                Author = "Gary Keller",
                Rate = (double)4.1,
                DateStart = new DateTime(2019,07,18),
                DateRead = new DateTime(2019,09,18),
            },
            new Book()
            {
                Id = 10,
                Title = "Riveted",
                Description = "Professor Jim Davies’ fascinating and highly accessible book, Riveted, reveals the evolutionary underpinnings of why we find things compelling, from art to religion and from sports to superstition...",
                Author = "Jim Davies",
                Rate = (double)4.6,
                DateStart = new DateTime(2019,09,20),
                DateRead = new DateTime(2019,10,28)
            }
        };
    }
}
~~~

Add service in `src/Program.cs`
~~~C#
builder.Services.AddTransient<IBookService, BookService>();
~~~

Add `BooksController` in `src/Controllers/BooksController.cs`
~~~C#
using System;
using Microsoft.AspNetCore.Mvc;

namespace example_5
{
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private IBookService _service;
        public BooksController(IBookService service)
        {
            _service = service;
        }

        //Create/Add a new book
        [HttpPost("AddBook")]
        public IActionResult AddBook([FromBody]Book book)
        {
            try
            {
                if(book.Author != null && book.Title != null && book.Description != null)
                {
                    _service.AddBook(book);
                    return Ok(book);
                }
                return BadRequest("Book was not added");
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //Read all books
        [HttpGet("[action]")]
        public IActionResult GetBooks()
        {
            var allBooks = _service.GetAllBooks();
            return Ok(allBooks);
        }

        //Update an existing book
        [HttpPut("UpdateBook/{id}")]
        public IActionResult UpdateBook(int id, [FromBody]Book book)
        {
            _service.UpdateBook(id, book);
            return Ok(book);
        }

        //Delete a book
        [HttpDelete("DeleteBook/{id}")]
        public IActionResult DeleteBook(int id)
        {
            _service.DeleteBook(id);
            return Ok();
        }

        //Get a single book by id
        [HttpGet("SingleBook/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _service.GetBookById(id);
            return Ok(book);
        }
    }
}
~~~

Add `books/` entry in `src/ClientApp/proxy.conf.js`
~~~
context: [
  "/weatherforecast",
  "/books"
],
~~~

Afterwards you can access the API at:
- http://localhost:44463/books/getbooks
- http://localhost:44463/books/singlebook/1

## Angular Routing

First of all we will move the routing in a new file.

Create `src/app/app-routing.module.ts` and move the routing code from `imports` of `src/app/app.module.ts`.
~~~ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
~~~

Import the `AppRoutingModule` in `src/app/app.module.ts`
~~~ts
import { AppRoutingModule } from './app-routing.module';
...
@NgModule({
  ...
  imports: [
    ...
    AppRoutingModule
  ],
  ...
})
export class AppModule { }
~~~

## Books Module

Now lets create the new `books` module which will be available at route `books/`
~~~
ng generate module books --route books --module app.module
~~~

## Books Service

Create the class/interface to represent a book object `src/app/services/book.ts`
~~~ts
export interface Book
{
    id: number;
    title: string;
    description: string;
    author: string;
    rate?: number;
    dateStart?: Date;
    dateRead? : Date;
}
~~~

Afterwards the http service to send request to the API `src/app/services/book.service.ts`
~~~ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Book } from './book';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  _baseURL: string = "books";

  constructor(private http: HttpClient) { }

  getAllBooks(){
    return this.http.get<Book[]>(this._baseURL+"/GetBooks");
  }

  addBook(book: Book){
    return this.http.post(this._baseURL+"/AddBook/", book);
  }

  getBookById(id: number){
    return this.http.get<Book>(this._baseURL+"/SingleBook/"+id);
  }

  updateBook(book: Book){
    return this.http.put(this._baseURL+"/UpdateBook/"+book.id, book);
  }

  deleteBook(id: number){
    return this.http.delete(this._baseURL+"/DeleteBook/"+id);
  }

}
~~~

## Books Views

We will create the views to show all books, show details of a book, create a new book, update a book and delete a book.
Create the `src/app/components/books` directory for the books components.

We will need to add the components to the declarations and import `FormsModule` in `src/app/books/books.module.ts`
~~~ts
...
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NewBookComponent } from '../components/books/add/new-book.component';
import { ShowBookComponent } from '../components/books/details/show-book.component';
import { UpdateBookComponent } from '../components/books/update/update-book.component';
import { DeleteBookComponent } from '../components/books/delete/delete-book.co
...
@NgModule({
  declarations: [
    ...
    BookListComponent,
    NewBookComponent,
    ShowBookComponent,
    UpdateBookComponent,
    DeleteBookComponent,
    ...
  ],
  imports: [
    ...
    FormsModule,
    ReactiveFormsModule,
    ...
  ]
})
export class BooksModule { }
~~~

### Book List View

Create the `BookListComponent`, see: 
- `src/app/components/books/bookList.component.css`
- `src/app/components/books/bookList.component.html`
- `src/app/components/books/bookList.component.ts`

### Book Details View

Create the `ShowBookComponent`, see:
- `src/app/components/books/details/show-book.component.css`
- `src/app/components/books/details/show-book.component.html`
- `src/app/components/books/details/show-book.component.ts`

### Add Book View

Create the `NewBookComponent`, see:
- `src/app/components/books/add/new-book.component.css`
- `src/app/components/books/add/new-book.component.html`
- `src/app/components/books/add/new-book.component.ts`

### Update Book View

Create the `UpdateBookComponent`, see:
- `src/app/components/books/update/update-book.component.css`
- `src/app/components/books/update/update-book.component.html`
- `src/app/components/books/update/update-book.component.ts`

### Delete Book View

Create the `DeleteBookComponent`, see:
- `src/app/components/books/delete/delete-book.component.css`
- `src/app/components/books/delete/delete-book.component.html`
- `src/app/components/books/delete/delete-book.component.ts`

## Books Sub Routing

Add entry for books in `src/app/components/app/nav-menu/nav-menu.component.html` to allow to open books module view
~~~html
<li class="nav-item" [routerLinkActive]="['link-active']">
  <a class="nav-link text-dark" [routerLink]="['/books']">Books</a>
</li>
~~~

For each view we will create a sub path inside `src/app/books/books-routing.module.ts`
~~~ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BooksComponent } from './books.component';
import { BookListComponent } from '../components/books/bookList.component';
import { NewBookComponent } from '../components/books/add/new-book.component';
import { ShowBookComponent } from '../components/books/details/show-book.component';
import { UpdateBookComponent } from '../components/books/update/update-book.component';
import { DeleteBookComponent } from '../components/books/delete/delete-book.component';

const routes: Routes = [{ path: '', component: BooksComponent, children: [
  { path: '', component: BookListComponent },
  { path: 'add', component: NewBookComponent },
  { path: 'details/:id', component: ShowBookComponent },
  { path: 'update/:id', component: UpdateBookComponent },
  { path: 'delete/:id', component: DeleteBookComponent },
] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
~~~

Change `src/app/books/books.component.html` to show the view for the selected path.
~~~html
<a class="nav-link text-dark" [routerLink]="['add']">Add</a>
<div class="container">
  <router-outlet></router-outlet>
</div>
~~~
