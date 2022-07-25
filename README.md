## MOVIES

### Installation
For installation, please do the following steps

1. Clone the repo
   ```sh
   git clone https://github.com/luisf350/Movies.git
   ```
2. Open solution on Visual Studio
3. Restore nuget packages
   ```sh
   dotnet restore
   ```
4. Run the application


### Usage
Once the application start, Swagger will open and you can start using ther application. first thing to do is create an User and the do Login, both in Authentication controller, that way you will have the JWT token to be able to use the other controllers.
The CRUD operation for Movies are in movies controller.
To retrieve a random number, using an external API, please use Number controller
There is also Data controller, to insert dummy data to the database... And also to clean movies entries.

### Packages used
* AutoMapper
* Swashbuckle (Swagger)
* FluentValidation
* EntityFrameworkCore
* Moq

### Database
I decide to use InMemoryDatabase provided by EntityFramework

### Live Demo
To view the application in Azure, please visit [https://moviesapi20220216174420.azurewebsites.net/swagger/index.html](https://moviesapi20220216174420.azurewebsites.net/swagger/index.html)

### Unit Test
* Domain
* Repositories
