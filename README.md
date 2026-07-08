# Weather API

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/[username]/[repo-name])
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue.svg)](https://github.com/[username]/[repo-name]/pulls)

## Tech Stack
- **Backend:** .NET 10 , ASP.NET Core Web API 
- **Design Pattern:** MediatR (CQRS pattern)
- **Database:** PostgreSQL 
- **ORM:** Entity Framework Core 10 (EF Core)
- **Authentication & Security:** JWT (JSON Web Tokens)
- **Testing Ecosystem:**
  - **Frameworks:** xUnit, Moq, MockHttp
  - **Integration Testing:** Testcontainers
  - **API Mocking:** WireMock.Net 
  - **Snapshot Testing:** Verify.Xunit 
- **Logging:** ILogger, Serilog

## Features
- **Clean Architecture:** Well-structured codebase separated into clear layers (`Domain`, `Application`, `Infrastructure`, `Presentation`)
- **CQRS Implementation:** Decouples the application's read and write operation into distinct **Queries (Commands)** and **Handlers**. MediatR keeps the API Controllers thin and the business logic focused
- **Secure Token-Based Authentication:** Protect API endpoint using **JWT (Json Web Token) Bearer Authentication**, ensuring only users with a valid access token can use weather data
- **External Weather API Integration:** Consumes real-time weather data from the third-party [WeatherAPI](WeatherAPI.com) via an optimized HTTP client implementation.
- **Advanced Logging with Serilog:** Implements structured logging using .NET's `ILogger` abstraction paired with **Serilog**. Features a **Rolling File Sink** that automatically generates a isolated log file every day
- **Database Persistence:** Fully integrated with PostgreSQL via EF Core for reliable data handling and schema migrations.
- **Advanced Automated Testing Suite:** Built with **xUnit** and **Moq**, leveraging **Testcontainers** for isolated PostgreSQL integration tests, **WireMock.Net** / **MockHttp** for external API simulations, and **Verify.Xunit** for modern snapshot regression testing.
- **API Documentation:** OpenAPI integrated with Scalar UI.
## Getting Started 
Follow these steps to set up the project locally on your machine: 
### Prerequisites
Make sure you have following installed: 
- .NET 10 SDK 
- Docker Engine or Docker Desktop (to set up PostgresSQL Container in Intergration Test via TestContainers)
### Installation 
#### 1. **Clone the repository:**
`git clone`
#### 2. **Create your appsettings file:**
Copy the example configuration file to create your actual development settings file:

`cp WeatherAPI/appsettings.Development.json.example WeatherAPI/appsettings.Development.json`
#### 3. **Configure Environment Variables:**
Open the appsetting.json file in the WeatherAPI project and configure your local settings. 

```Json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432; Database=WeatherDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "SecretKey": "YOUR_JWT_SECRET_KEY"
  },
  "WeatherApi": {
    "ApiKey": "YOUR_WEATHER_API_KEY"
  }
}
```
**ConnectionStrings:Default** is your PostgreSQL connection string.

**Jwt:SecretKey** is a random, secure string of at least 32 characters (256 bits).

**WeatherApi:Apikey** is your personal API Key to fetch live weather data. You can register a free account at WeatherAPI.com to get your key instantly.

#### 4.Start the Database 
Using PostgresSQL Docker Image to run a container or PostgreSQL server
#### 5.Apply database migrations 
Generate the necessary database schemas and tables by running the Entity Framework Core migration command from the solution's root directory:

`dotnet ef database update --project MyWeatherApplication.Infrastructure --startup-project WeatherAPI`

#### 6.Test and Run the application 
Run entire tests: 

`dotnet test`

**NOTICE:** Since this project utilizes Verify for snapshot testing, the first time you run integration tests (or when API responses change), you will see two types of files generated in your test directories:

    *.received.json (The actual current output from the API/Database)

    *.verified.json (The expected baseline approved by the developer)

If a test fails due to a snapshot mismatch, you need to review the changes. If the new output is correct, approve it by renaming/overwriting the .verified. file with the contents of the .received. file.
Then, run `dotnet test` again, it passes all test cases successfully

Finally, Launch the Web API project

`cd WeatherAPI`

`dotnet run`

It will log the local hosting URLs (usually http://localhost:5xxx)

**Accessing the API Documentation**

Once the application is running, you can explore and test the API endpoints using the **Scalar** interactive documentation interface:

- **Scalar UI Endpoint:** `https://localhost:5xxx/scalar` 

## Project structure
## API Endpoint 

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` -Authenticate user and return JWT Token

### Weather
- `GET /api/weather/current` - Get current weather details for a specific location
- `GET /api/weather/forecast` - Get weather forecast for a specific location 

## License 
This project is licensed under the MIT License



