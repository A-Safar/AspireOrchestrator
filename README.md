"# AspireOrchestrator

A .NET Aspire application demonstrating microservices orchestration with OpenTelemetry observability.

## Overview

This project showcases a distributed application built with .NET Aspire that orchestrates two microservices with comprehensive observability features including metrics, tracing, and logging using OpenTelemetry.

## Architecture

The application consists of:

- **Aspire Host** (`AspireAppHostPOC`): The orchestrator that manages and coordinates the microservices
- **App1** (`app1`): A Student Management API with CRUD operations and Entity Framework Core
- **App2** (`app2`): A supporting API service that provides additional endpoints

## Features

### App1 - Student Management Service
- Full CRUD operations for student entities
- In-memory database using Entity Framework Core
- Custom metrics for tracking student count
- HTTP client integration for calling external services
- Swagger/OpenAPI documentation

### App2 - Supporting Service
- Basic API endpoints
- OpenTelemetry integration
- Containerized deployment support

### Observability
- **OpenTelemetry Integration**: Comprehensive telemetry data collection
- **Metrics**: Custom counters for business metrics (student count)
- **Tracing**: Distributed tracing across HTTP calls and database operations
- **Logging**: Structured logging with OTLP export
- **Aspire Dashboard**: Built-in observability dashboard

## Prerequisites

- .NET 8.0 SDK
- Docker (for containerized services)
- Visual Studio 2022 or VS Code with C# extension

## Getting Started

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd AspireAppHostPOC
   ```

2. **Run the application**
   ```bash
   dotnet run --project AspireAppHostPOC
   ```

3. **Access the services**
   - Aspire Dashboard: `https://localhost:15888` (check console output for exact URL)
   - App1 Swagger UI: `https://localhost:<port>/swagger` 
   - App2 API: `http://localhost:5001`

## API Endpoints

### App1 - Student Controller
- `GET /Student` - Get all students (also calls App2)
- `GET /Student/{id}` - Get student by ID
- `POST /Student` - Create a new student
- `PUT /Student` - Update an existing student
- `DELETE /Student/{id}` - Delete a student

### App2 - Home Controller
- `GET /Home` - Basic endpoint

## Project Structure

```
AspireAppHostPOC/
├── AspireAppHostPOC/          # Aspire Host orchestrator
│   ├── Program.cs             # Application composition root
│   └── AspireAppHostPOC.csproj
├── app1/                      # Student Management Service
│   ├── Controllers/
│   │   └── HomeController.cs  # Student CRUD operations
│   ├── Program.cs             # Service configuration
│   └── BasicOpenTelemetry-1.csproj
├── app2/                      # Supporting Service
│   ├── Controllers/
│   │   └── HomeController.cs  # Basic API endpoints
│   ├── Program.cs             # Service configuration
│   └── BasicOpenTelemetry.csproj
└── README.md
```

## Configuration

The application uses OpenTelemetry with OTLP exporters configured to send telemetry data to `http://localhost:4317` by default. This can be overridden using the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable.

## Development

### Running Individual Services

You can run individual services for development:

```bash
# Run App1
dotnet run --project app1

# Run App2
dotnet run --project app2
```

### Docker Support

App2 includes Docker support with a Dockerfile for containerized deployment.

## Technologies Used

- .NET 8.0
- ASP.NET Core Web API
- .NET Aspire
- Entity Framework Core (In-Memory)
- OpenTelemetry
- Docker
- Swagger/OpenAPI

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is a proof of concept for demonstrating .NET Aspire capabilities." 
