# ![RealWorld Example App](logo.png)

> ### ASP.NET Core codebase containing real world examples (CRUD, auth, advanced patterns, etc) that adheres to the [RealWorld](https://github.com/gothinkster/realworld-example-apps) spec and API.


### [Demo]()&nbsp;&nbsp;&nbsp;&nbsp;[RealWorld](https://github.com/gothinkster/realworld)


This codebase was created to demonstrate a fully fledged fullstack application built with ASP.NET Core (with Feature orientation) including CRUD operations, authentication, routing, pagination, and more.

We've gone to great lengths to adhere to the ASP.NET Core community styleguides & best practices.

For more information on how to this works with other frontends/backends, head over to the [RealWorld](https://github.com/gothinkster/realworld) repo.


# How it works

This is using ASP.NET Core with:

- CQRS and MediatR
    - [Simplifying Development and Separating Concerns with MediatR](https://blogs.msdn.microsoft.com/cdndevs/2016/01/26/simplifying-development-and-separating-concerns-with-mediatr/)
    - [CQRS with MediatR and AutoMapper](https://lostechies.com/jimmybogard/2015/05/05/cqrs-with-mediatr-and-automapper/)
    - [Thin Controllers with CQRS and MediatR](https://codeopinion.com/thin-controllers-cqrs-mediatr/)
- AutoMapper
- Feature folders and vertical slices
- Entity Framework Core
- Built-in Swagger via [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Cake](http://cakebuild.net/) for building!

# Getting started

Build on OS X and Linux:

`./build.sh build.cake`

Build on Windows:

`./build.ps1 build.cake`

Build Docker and run:

`docker build -t realworld:latest .`
`docker run -p 5000:5000 realworld:latest`

Swagger URL:
`http://localhost:5000/swagger`
