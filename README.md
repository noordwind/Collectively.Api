# Collectively.Api

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Collectively.Api.svg?branch=master)](https://travis-ci.org/noordwind/Collectively.Api)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Collectively.Api.svg?branch=develop)](https://travis-ci.org/noordwind/Collectively.Api/branches)

**Let's go for the better, Collectively​​.**
----------------

**Collectively** is an open platform to enhance communication between counties and its residents​. It's made as a fully open source & cross-platform solution by [Noordwind](https://noordwind.com).

Find out more at [becollective.ly](http://becollective.ly)

**Collectively.Api**
----------------

The **Collectively.Api** acts as a gateway to the whole Collectively system that was built as a set of distributed microservices following the CQRS pattern.
This is a RESTful API that can be accessed by any type of application or tool that can handle the HTTP requests.

The API itself does not know anything about the underlying microservices and has just a dependency to the RabbitMQ service bus in order to publish the commands from the incoming user requests  and the [Storage](https://github.com/noordwind/Collectively.Services.Storage) in order to fetch the data. 

If you would like to find out more about the available endpoints, please navigate to the [API documentation](http://docs.collectively.apiary.io).

**Quick start**
----------------

## Docker way

Collectively is built as a set of microservices, therefore the easiest way is to run the whole system using the *docker-compose*.

Clone the [Collectively.Docker](https://github.com/noordwind/Collectively.Docker) repository and run the *start.sh* script:

```
git clone https://github.com/noordwind/Collectively.Docker
./start.sh
```

For the list of available services and their endpoints [click here](https://github.com/noordwind/Collectively).

## Classic way

In order to run the **Collectively.Api** you need to have installed:
- [.NET Core](https://dotnet.github.io)
- [RabbitMQ](https://www.rabbitmq.com)

Clone the repository and start the application via *dotnet run* command:

```
git clone https://github.com/noordwind/Collectively.Api
cd Collectively.Api/Collectively.Api
dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/collectively/api/v3/index.json --no-cache
dotnet run --urls "http://*:5000"
```

Once executed, you shall be able to access the API at [http://localhost:5000](http://localhost:5000)

Please note that the following solution will only run the HTTP API which is merely one of the many parts required to run properly the whole Collectively system.

**Configuration**
----------------

Please edit the *appsettings.json* file in order to use the custom application settings. To configure the docker environment update the *dockerfile* - if you would like to change the exposed port, you need to also update it's value that can be found within *Program.cs*.
For the local testing purposes the *.local* or *.docker* configuration files are being used (for both *appsettings* and *dockerfile*), so feel free to create or edit them.

**Tech stack**
----------------
- **[.NET Core](https://dotnet.github.io)** - an open source & cross-platform framework for building applications using C# language.
- **[Nancy](http://nancyfx.org)** - an open source framework for building HTTP API.
- **[RawRabbit](https://github.com/pardahlman/RawRabbit)** - an open source library for integration with [RabbitMQ](https://www.rabbitmq.com) service bus.

**Solution structure**
----------------
- **Collectively.Api** - core and executable project via *dotnet run* command.
- **Collectively.Api.Tests** - unit & integration tests executable via *dotnet test* command.
- **Collectively.Api.Tests.EndToEnd** - End-to-End tests executable via *dotnet test* command.