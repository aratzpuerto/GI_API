# GI_API

**GI_API** is a well‑structured RESTful API developed with **ASP.NET Core**, designed for managing tasks and related entities. It demonstrates clean architecture, strong separation of concerns, testability, and professional backend design. This project is part of my personal portfolio showcasing backend systems and API development skills.

---

## Project Overview

GI_API provides a modular and maintainable backend API capable of handling CRUD operations related to tasks and task types. It is built with industry best practices and standards such as dependency injection, middleware for cross‑cutting concerns, layered architecture, and thorough unit testing of core logic.

The API runs inside Docker containers alongside a SQL Server database using Docker Compose to support reproducible development and deployment environments.

---

## Architecture

GI_API follows a layered architecture that cleanly separates responsibilities:

**Controllers → Services → Data**

### ✔ Models

Located under `Models/`, these classes define the domain entities such as:

* **Task**
* **TaskType**

Models define the shape of data used throughout the application.

### ✔ Services

Under `Services/`, services encapsulate the application’s business logic and data access. Each service interfaces with `DbService` as a single point of interaction with the database.

* Interfaces (e.g., `ITaskService`) define contracts.
* Concrete services (e.g., `TaskService`) implement the logic and are registered with DI.

This separation enables modularity and ease of testing.

### ✔ Controllers

Controllers expose API endpoints using ASP.NET Core MVC. They receive HTTP requests, delegate logic to services, and return responses.

Each controller strictly focuses on *presentation logic* and defers business rules to services.

---

## Technologies Used

GI_API leverages a modern backend stack:

| Feature | Technology / Implementation |
| :--- | :--- |
| **Web Framework** | ASP.NET Core MVC / Minimal API |
| **Language** | C# |
| **API Documentation** | Swagger / OpenAPI |
| **Database** | SQL Server  |
| **Containerization** | Docker / Docker Compose |
| **Logging** | **NLog** (registered via `UseNLog()` + ASP.NET Core logging abstraction) |
| **Configuration** | `appsettings.json`, environment variables, options pattern |
| **Testing** | xUnit + Moq |
| **Dependency Injection** | Built‑in ASP.NET Core DI |
| **Middleware** | Custom middleware for global exception handling & logging |

---

## Dependency Injection

The application uses ASP.NET Core’s built-in DI container. Key services are registered in `Program.cs`, including:

```csharp
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<TaskTypeService>();
builder.Services.AddScoped<DbService>();
```

DI ensures:
* Loose coupling
* Easier mocking in tests
* Clean service replacement or extension

---
## Logging

GI_API implements **centralized, structured logging** using **NLog** as the primary logging provider.

### Logging Stack & Technology

* **Framework Logging Abstraction**: `Microsoft.Extensions.Logging` — the built‑in ASP.NET Core logging abstractions.
* **Actual Logging Provider**: **NLog** (`NLog.Extensions.Logging` / `NLog.Web.AspNetCore`)  
  — configured via `builder.Host.UseNLog()` in `Program.cs`, so all `.LogInformation`, `.LogError`, etc. calls are handled by NLog.
* **Targets & Outputs**: NLog supports rich, configurable output targets such as:
  * Console
  * File
  * Database
  * Custom sinks (configurable via NLog config or appsettings) :contentReference[oaicite:1]{index=1}

### Why This Matters

Using **Microsoft’s logging abstraction** with **NLog as the provider** gives you:

* Centralized logging throughout the application
* Structured and configurable output
* Flexibility to change targets without modifying code

---

## Middleware

GI_API includes custom middleware to handle concerns that cut across endpoints.

### Global Exception Handling

Any unhandled exceptions thrown by the application are intercepted by a global handler. This middleware:

* Catches all exceptions
* Logs errors in a centralized way
* Returns structured error responses to clients
* Prevents internal stack traces from leaking to consumers

This improves reliability and client experience.

---

## Running the Project (Docker)

GI_API is fully containerized using Docker Compose.

### Requirements

* Docker installed locally
* Docker Compose

### Start API + Database

From the project root:

```bash
docker-compose up --build
```

This command:

* Builds the API image
* Starts a SQL Server container
* Links both services via Docker's network

No additional configuration is required.

---

## Unit Testing

The project includes unit tests focused on business logic and service behavior using:

* **xUnit** — Test framework
* **Moq** — Mocking dependencies

Tests cover:
* Service behavior
* Controller actions
* Error conditions

Run all tests with:

```bash
dotnet test
```

Unit tests make the codebase robust and safe to refactor as requirements evolve.

---

## API Documentation (Swagger)

GI_API uses Swagger for interactive documentation.
Once the API is running in Docker:

Open your browser and go to: `http://localhost:8080/swagger/index.html`

Swagger displays all endpoints, models, and parameter schemas — ideal for exploring the API or testing operations interactively.

⚠️ *If Swagger is disabled in production mode, ensure it’s enabled in `Program.cs`:*

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

---

## Key Code Organization

```text
/GI_API
├─ Controllers/           # API endpoints / HTTP handlers
├─ Models/                # Domain models and entity classes
├─ Services/              # Business logic & database interaction
├─ Contracts/             # Service interfaces for abstraction and TDD
├─ Middlewares/           # Global cross‑cutting handlers
├─ Program.cs             # Application setup, DI, middleware pipeline
├─ Dockerfile             # Container build definition
├─ docker-compose.yml     # Local orchestration of API + database
└─ Tests/                 # Unit tests for service/controller logic
```

---

## Professional Highlights

* **Clean Architecture** – Clear separation of API layers
* **Testable Design** – Service interfaces + unit tests
* **Middleware Implementation** – For logging and global errors
* **Containerization** – Docker for consistent environments
* **Interactive API Docs** – Swagger with OpenAPI support

This combination makes GI_API a strong example of modern backend engineering and a valuable portfolio project.

---
## Security

Currently, GI_API does not implement authentication or authorization.
Future plans may include:

* JWT Bearer authentication
* Role-based access control
* HTTPS enforcement
---

## Get Involved / Extend

Contributions or extensions (e.g., additional endpoints, authentication, integrations) are straightforward due to the modular structure — making this project suitable as a base for more complex systems.

Contributions are welcome! Guidelines:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature-name`).
3. Commit changes (`git commit -m 'Add new feature'`).
4. Push branch (`git push origin feature-name`).
5. Open a pull request.

---

**Author:** Aratz Puerto  
**Repository:** GI_API