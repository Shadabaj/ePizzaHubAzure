# Pizza Ordering Platform

An ASP.NET Core MVC application for an online pizza-ordering workflow, organized into separate core, models, repositories, services, UI, and Azure Functions projects. The solution integrates cloud services for storage, messaging, caching, monitoring, and payment processing.

## Features and architecture

- MVC user interface with cookie-based authentication.
- Catalog, cart, order, and administrative workflows.
- Separated service, repository, model, and core projects.
- Azure Functions for background/event-driven tasks such as email notifications.
- Integration points for SQL Server, Azure Blob Storage, Azure Service Bus, Azure Key Vault, Redis, Application Insights, and Razorpay.
- Structured application logging with Serilog.

## Technology stack

- ASP.NET Core MVC and C#
- Entity/data access and repository/service layers
- SQL Server
- Azure Functions v4
- Azure Blob Storage, Service Bus, Key Vault, and Application Insights
- Redis
- Razorpay integration

> **Runtime note:** The current solution targets .NET 6. .NET 6 reached end of support on November 12, 2024. Plan a tested migration to a currently supported .NET LTS release before using this application in production. See the [official .NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy).

## Solution structure

~~~text
.
├── ePizzaHubAzure.sln
├── ePizzaHub.Core/          # Core entities and database context
├── ePizzaHub.Models/        # View/API models
├── ePizzaHub.Repositories/  # Repository contracts and implementations
├── ePizzaHub.Services/      # Business services and dependency setup
├── ePizzaHub.Tasks/         # Azure Functions and background tasks
└── ePizzaHub.UI/            # ASP.NET Core MVC application
~~~

## Prerequisites

- A .NET SDK compatible with the solution's current target framework
- SQL Server
- Azure resources or local equivalents for services used by the workflow
- Azure Functions Core Tools v4 for the Functions project
- Valid local configuration for database, cache, messaging, storage, Key Vault, email, and payment integrations

## Restore and build

From the repository root:

~~~powershell
dotnet restore .\ePizzaHubAzure.sln
dotnet build .\ePizzaHubAzure.sln --configuration Release
~~~

Run the MVC application:

~~~powershell
dotnet run --project .\ePizzaHub.UI\ePizzaHub.UI.csproj
~~~

The UI initializes its Key Vault client during startup, so configure a valid `KeyVault:VaultUri` before running. Background functions can be run with Azure Functions Core Tools after their local settings are configured.

## Configuration and secrets

Never commit real connection strings, SMTP passwords, payment secrets, storage credentials, or monitoring credentials. Provide configuration locally through .NET user secrets, environment variables, or a secure secret store.

Examples of environment-variable keys used by the application include:

- `ConnectionStrings__DbConnection`
- `ConnectionStrings__RedisCache`
- `ConnectionStrings__ServiceBus`
- `KeyVault__VaultUri`
- `Storage__Connection`
- `RazorPay__Key` and `RazorPay__Secret`
- `ApplicationInsights__ConnectionString`

For the Azure Functions project, copy `ePizzaHub.Tasks/local.settings.example.json` to `local.settings.json` and replace the placeholders with local development values. The real local file is ignored by Git.

## Before production use

- Upgrade from the out-of-support .NET 6 target and verify compatibility across the MVC and Functions projects.
- Ensure all secrets are supplied through secure configuration and rotate any credentials that have previously been committed.
- Restrict CORS, cookies, and network access to the deployment's actual trust boundaries.
- Validate authentication and authorization for customer and administrative actions.
- Add automated tests for ordering, payment, messaging, and failure/retry behavior.

## License

No license file is currently included. Add a license before inviting others to reuse or redistribute the project.
