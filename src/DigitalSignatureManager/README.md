# Digital signature manager

## Tech
* [ASP.NET Core 5] - Popular web-development framework for building web apps on the .NET 
* [EF Core 5] - Modern and mature ORM for .NET
* [Fluent Validation] - .NET library for building strongly-typed validation rules
* [Swashbuckle] - OpenAPI implementations for .NET
* [Moq] - Friendly mocking framework for .NET
* [NUnit] - Unit testing framework for .NET

## Prerequisites
You should have either **.NET 5 SDK and runtime** installed along with **SQL Server** or **Docker** on your machine.

Since HTTPS redirections are used make sure to generate the self-signed certificate for development purposes and mark it as trusted. You can find more info about it [here](https://docs.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide).
```sh
$ dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p yourPassword
$ dotnet dev-certs https --trust
```

## Testing
Position yourself in the `/src` directory and execute the following command:
```sh
$ dotnet test
```

## How to run
Position yourself in the `/src` directory and execute the following command:
```sh
$ dotnet run --project Api
```
The application is up and running on `localhost:4201` with swagger documentation available at `localhost:4201/swagger/index.html`.

## Docker
The project has Docker support
[![Docker](https://cdn4.iconfinder.com/data/icons/logos-and-brands/512/97_Docker_logo_logos-128.png)](https://www.docker.com/)

### Building an image
Position yourself in the `/src` directory and execute the following command:
```sh
$ docker image build -t digital-signature-online-manager -f Api\Dockerfile .
```

### How to run
It is very easy to run the application using Docker Compose.
Position yourself in the root directory of the project and execute the following command:
```sh
$ docker-compose up
```

[ASP.NET Core 5]: <https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core>
[EF Core 5]: <https://docs.microsoft.com/en-us/ef/core/>
[Fluent Validation]: <https://fluentvalidation.net/>
[Swashbuckle]: <https://github.com/domaindrivendev/Swashbuckle.AspNetCore>
[Moq]: <https://github.com/moq>
[NUnit]: <https://nunit.org/>