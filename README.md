# ⚙️ Modules Microservice

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](LICENSE.md)
[![Tests](https://img.shields.io/badge/tests-passing-success)](tests/)
[![Coverage](https://img.shields.io/badge/coverage-85%25-green)]()
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](Dockerfile)

A production-ready microservice for managing application modules and their associated services built with .NET 9. Implements Clean Architecture, DDD, and CQRS patterns with support for dynamic module-based feature management and RBAC integration.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#️-technology-stack)
- [Prerequisites](#️-prerequisites)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [Configuration](#️-configuration)
- [Use Cases & Scenarios](#-use-cases--scenarios)
- [Architecture](#️-architecture)
- [Domain Model](#-domain-model)
- [Testing](#-testing)
- [Best Practices](#-best-practices)
- [Troubleshooting](#-troubleshooting)
- [Module Management Flow](#-module-management-flow)
- [Integration Points](#-integration-points)
- [Security](#-security)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

## What is this microservice?

The Modules microservice groups individual API endpoints into meaningful business features (e.g., "Resident Management," "Common Areas," "Billing"). It solves the problem of controlling which features are available to each organization based on their subscription plan. When a license is purchased, the included modules determine what functionality the tenant can access. Platform administrators use it to define and organize features, while the Licenses microservice references modules to compose subscription plans. Together with Services and RBAC, it forms the feature-gating layer of the platform.

---

The Modules microservice provides a centralized system for managing application modules and their associated services (API endpoints, features, permissions). It enables dynamic feature management, role-based access control (RBAC), and modular application architecture.

- **Module Management**: Create, update, delete, and query application modules
- **Service Registry**: Define and manage services (endpoints) within each module
- **Feature Control**: Enable/disable modules dynamically
- **RBAC Integration**: Link modules to permissions and roles
- **Multi-tenancy**: Isolate modules by tenant
- **REST API**: Full CRUD operations with pagination and filtering
- **Event-Driven**: Publishes domain events for module lifecycle changes

### 🚀 Quick Start

```bash
# 1. Start infrastructure services
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d

# 2. Configure Vault secrets
cd ../../tools/vault
./config-vault.sh

# 3. Run the microservice
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Modules.Rest

# 4. Access Swagger UI
open http://localhost:5000/ms-modules/swagger
```

### 📊 High-Level Architecture

```
┌─────────────┐
│   Client    │
│ Application │
└──────┬──────┘
       │ HTTPS + JWT
       │
┌──────▼──────────────────────────────────────────────┐
│         Modules Microservice (REST API)             │
│  ┌──────────────┐  ┌─────────────┐  ┌────────────┐ │
│  │ Controllers  │  │  MediatR    │  │  Handlers  │ │
│  │   (API)      │─▶│   (CQRS)    │─▶│ (Business) │ │
│  └──────────────┘  └─────────────┘  └────┬───────┘ │
│                                           │         │
│  ┌────────────────────────────────────────▼──────┐ │
│  │       Module Aggregate Root                   │ │
│  │  - Name, Description, IsActive                │ │
│  │  - Services Collection                        │ │
│  │  - Domain Events                              │ │
│  └────────────────────────────────────────────────┘ │
└───────┬──────────────────┬──────────────────┬───────┘
        │                  │                  │
   ┌────▼────┐      ┌──────▼──────┐    ┌─────▼─────┐
   │ MongoDB │      │   Redis     │    │ RabbitMQ  │
   │(Modules)│      │  (Cache)    │    │ (Events)  │
   └─────────┘      └─────────────┘    └───────────┘
```

## 🚀 Key Features

### Core Capabilities

- ✅ **Module Management**: CRUD operations for application modules
- ✅ **Service Registry**: Define services (endpoints) with HTTP methods
- ✅ **Dynamic Features**: Enable/disable modules at runtime
- ✅ **Service Operations**: Add/remove services from modules
- ✅ **Rich Metadata**: Store controller, action, and HTTP method details
- ✅ **Status Tracking**: Track active/inactive modules
- ✅ **Multi-Tenancy**: Isolate modules by tenant
- ✅ **Pagination & Filtering**: OData-style queries with criteria
- ✅ **Soft Delete**: Mark modules as deleted without physical removal
- ✅ **Audit Trail**: Track created/updated/deleted timestamps and users
- ✅ **Problem Details**: RFC 7807 compliant error responses

### Technical Features

- Clean Architecture with DDD and CQRS
- Domain events for module state changes
- MongoDB for module persistence
- RabbitMQ for event publishing
- Redis for distributed caching
- OAuth2/OpenID Connect security
- Multi-tenancy support
- Swagger/OpenAPI documentation
- Docker containerization
- Comprehensive test coverage (Unit, Integration)

## 🛠️ Technology Stack

### Core
- **.NET 9** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C# 13** - Programming language

### Storage & Data
- **MongoDB** - Module persistence and queries
- **Redis** - Distributed caching and session storage

### Messaging & Events
- **RabbitMQ** - Event publishing and message broker

### Architecture & Patterns
- **MediatR** - CQRS command/query handling
- **FluentValidation** - Input validation
- **Mapster** - Object mapping
- **NodaTime** - Date/time handling
- **DDD** - Domain-Driven Design
- **CQRS** - Command Query Responsibility Segregation

### Security & Configuration
- **Vault** - Secret management
- **OAuth2/OpenID Connect** - Authentication
- **JWT Bearer** - Token-based security
- **HTTPS** - Encrypted communication

### DevOps & Testing
- **Docker** - Containerization
- **xUnit** - Unit/integration testing
- **Swagger/OpenAPI** - API documentation
- **Helm** - Kubernetes deployment
- **GitHub Actions** - CI/CD pipelines

## ⚙️ Prerequisites

### Required
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker & Docker Compose** - For infrastructure services
- **MongoDB 6.0+** - Document database
- **Redis 7.0+** - Caching layer
- **RabbitMQ 3.12+** - Message broker

### Optional
- **Vault** - Secret management (can use appsettings for local dev)
- **Kubernetes** - For production deployment
- **Helm 3+** - For Kubernetes deployment

## 🚀 Getting Started

The following instructions will help you set up the project on your local machine for development and testing purposes.

### 1. Clone the repository

```bash
git clone <repository-url>
cd CodeDesignPlus.Net.Microservice.Modules
```

### 2. Run infrastructure services

Clone and run the development environment:

```bash
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d
```

This starts:
- MongoDB (localhost:27017)
- Redis (localhost:6379)
- RabbitMQ (localhost:5672, Management UI: localhost:15672)
- Vault (localhost:8200)

### 3. Configure Vault secrets

```bash
cd ../../CodeDesignPlus.Net.Microservice.Modules/tools/vault
./config-vault.sh
```

This script configures:
- MongoDB credentials
- RabbitMQ credentials
- Application secrets

### 4. Build the solution

```bash
dotnet build
```

### 5. Run the microservice

```bash
# REST API (port 5000)
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Modules.Rest

# Access Swagger documentation
open http://localhost:5000/ms-modules/swagger
```

### 6. Verify installation

```bash
# Health check
curl http://localhost:5000/health

# Expected response: {"status": "Healthy"}
```

## 📡 API Endpoints

### Module Operations

#### Create Module
```http
POST /api/module
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "User Management",
  "description": "Module for managing users, roles, and permissions",
  "services": [
    {
      "id": "660e8400-e29b-41d4-a716-446655440001",
      "name": "Get User",
      "controller": "UserController",
      "action": "GetUser",
      "httpMethod": "GET"
    },
    {
      "id": "660e8400-e29b-41d4-a716-446655440002",
      "name": "Create User",
      "controller": "UserController",
      "action": "CreateUser",
      "httpMethod": "POST"
    }
  ]
}
```

**Response**: `204 No Content`

#### Get Module by ID
```http
GET /api/module/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `200 OK` with module details
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "User Management",
  "description": "Module for managing users, roles, and permissions",
  "services": [
    {
      "id": "660e8400-e29b-41d4-a716-446655440001",
      "name": "Get User",
      "controller": "UserController",
      "action": "GetUser",
      "httpMethod": "GET"
    },
    {
      "id": "660e8400-e29b-41d4-a716-446655440002",
      "name": "Create User",
      "controller": "UserController",
      "action": "CreateUser",
      "httpMethod": "POST"
    }
  ],
  "isActive": true
}
```

#### List Modules (Paginated)
```http
GET /api/module?limit=50&skip=0&filter=isActive eq true&orderby=name asc
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Query Parameters**:
- `limit` (optional): Number of items per page (default: 100)
- `skip` (optional): Number of items to skip (default: 0)
- `filter` (optional): OData filter expression
  - `isActive eq true` - Active modules only
  - `name contains 'User'` - Name contains 'User'
  - `createdAt gt 2026-01-01` - Created after date
- `orderby` (optional): OData order expression
  - `name asc` - Sort by name ascending
  - `createdAt desc` - Sort by creation date descending

**Response**: `200 OK` with paginated results
```json
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "User Management",
      "description": "Module for managing users, roles, and permissions",
      "services": [...],
      "isActive": true
    }
  ],
  "totalCount": 42,
  "limit": 50,
  "skip": 0
}
```

#### Update Module
```http
PUT /api/module/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "User Management (Updated)",
  "description": "Enhanced user management module",
  "services": [...],
  "isActive": true
}
```

**Response**: `204 No Content`

#### Delete Module
```http
DELETE /api/module/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

**Note**: This is a soft delete. The module is marked as deleted but not physically removed.

### Service Operations

#### Add Service to Module
```http
POST /api/module/{id}/services
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idService": "770e8400-e29b-41d4-a716-446655440003",
  "name": "Update User",
  "controller": "UserController",
  "action": "UpdateUser",
  "httpMethod": "PUT"
}
```

**Response**: `204 No Content`

#### Remove Service from Module
```http
DELETE /api/module/{id}/services/{serviceId}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response**: `204 No Content`

### HTTP Methods

Supported HTTP method values:
- `GET` - Read operations
- `POST` - Create operations
- `PUT` - Update operations
- `DELETE` - Delete operations
- `PATCH` - Partial update operations
- `None` - Default/undefined

### Error Responses

All errors follow RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Module not found.",
  "extensions": {
    "layer": "Application",
    "error_code": "MOD-404",
    "traceId": "0HMVJ3K7S5Q2K:00000001"
  }
}
```

**Common Status Codes**:
- `200 OK` - Success with response body
- `204 No Content` - Success without response body
- `400 Bad Request` - Invalid input or business rule violation
- `401 Unauthorized` - Missing or invalid token
- `404 Not Found` - Module or service not found
- `409 Conflict` - Module already exists
- `500 Internal Server Error` - Server error

**Domain Error Codes**:
- `101` - IdModuleIsInvalid - The id of the module is invalid
- `102` - NameModuleIsInvalid - The name of the module is invalid
- `103` - DescriptionModuleIsInvalid - The description of the module is invalid
- `104` - IdUserIsInvalid - The id of the user is invalid
- `105` - NameServiceIsInvalid - The name of the service is invalid
- `106` - ControllerServiceIsInvalid - The controller of the service is invalid
- `107` - ActionServiceIsInvalid - The action of the service is invalid
- `108` - ServiceNotFound - The service was not found
- `109` - IdServiceIsInvalid - The id of the service is invalid
- `110` - HttpMethodServiceIsInvalid - The http method of the service is invalid

## ⚙️ Configuration

### Application Settings

Configure the microservice in `appsettings.json`:

```json
{
  "Core": {
    "Id": "18c4d628-6254-47a3-9716-0899cbc1bc70",
    "PathBase": "/ms-modules",
    "AppName": "ms-modules",
    "TypeEntryPoint": "rest",
    "Version": "v1",
    "Description": "Microservice to manage the modules",
    "Business": "CodeDesignPlus",
    "Contact": {
      "Name": "CodeDesignPlus",
      "Email": "support@codedesignplus.com"
    }
  }
}
```

### MongoDB Configuration

```json
{
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-modules",
    "Diagnostic": {
      "Enable": false,
      "EnableCommandText": false
    }
  }
}
```

### Redis Cache Configuration

```json
{
  "Redis": {
    "Instances": {
      "Core": {
        "ConnectionString": "localhost:6379"
      }
    }
  },
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"
  }
}
```

### RabbitMQ Configuration

```json
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass",
    "EnableDiagnostic": false
  }
}
```

### Security Configuration

```json
{
  "Security": {
    "IncludeErrorDetails": true,
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "RequireHttpsMetadata": true,
    "ValidIssuer": "https://your-identity-server.com",
    "ValidAudiences": ["modules-api"],
    "Applications": [],
    "ValidateLicense": false,
    "ValidateRbac": false,
    "ServerRbac": "http://localhost:5001",
    "RefreshRbacInterval": 10
  }
}
```

### Observability Configuration

```json
{
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://localhost:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    },
    "Metrics": {
      "Enable": true,
      "AspNetCore": true
    }
  }
}
```

### Multi-tenancy

The microservice supports multi-tenancy through the `X-Tenant` header. Each request must include a tenant ID:

```http
X-Tenant: 9588813a-7bc0-4be4-a169-293061881cc3
```

Modules are isolated by tenant at the repository level.

### Environment Variables

Key environment variables for Docker deployment:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
MONGO_CONNECTION_STRING=mongodb://mongo:27017
REDIS_CONNECTION_STRING=redis:6379
RABBITMQ_HOST=rabbitmq
VAULT_ADDRESS=http://vault:8200
VAULT_TOKEN=your-vault-token
```

## 🎯 Use Cases & Scenarios

### 1. Dynamic Feature Management

Control application features by enabling/disabling modules:

```bash
# Step 1: Create a module for a new feature
POST /api/module
{
  "id": "...",
  "name": "Advanced Analytics",
  "description": "Advanced analytics and reporting features",
  "services": [...]
}

# Step 2: Feature is enabled by default (isActive: true)
# Users can access analytics features

# Step 3: Temporarily disable feature for maintenance
PUT /api/module/{id}
{
  "name": "Advanced Analytics",
  "description": "Advanced analytics and reporting features",
  "services": [...],
  "isActive": false
}

# Step 4: Re-enable feature after maintenance
PUT /api/module/{id}
{
  "isActive": true
}
```

### 2. RBAC Integration

Link modules to roles and permissions:

```bash
# Step 1: Create module with services
POST /api/module
{
  "name": "User Management",
  "services": [
    {
      "name": "Get Users",
      "controller": "UserController",
      "action": "GetUsers",
      "httpMethod": "GET"
    },
    {
      "name": "Create User",
      "controller": "UserController",
      "action": "CreateUser",
      "httpMethod": "POST"
    }
  ]
}

# Step 2: RBAC service listens to ModuleCreatedDomainEvent
# Creates permissions based on services

# Step 3: Assign permissions to roles
# Admin role: All services
# Viewer role: Only GET services

# Step 4: Users with Viewer role can only access GET endpoints
```

### 3. Multi-Tenant SaaS Application

Isolate modules per tenant:

```bash
# Tenant A's modules
POST /api/module
Headers: X-Tenant: tenant-a-id
{
  "name": "Tenant A - CRM",
  "description": "CRM module for Tenant A"
}

# Tenant B's modules (isolated)
POST /api/module
Headers: X-Tenant: tenant-b-id
{
  "name": "Tenant B - Inventory",
  "description": "Inventory module for Tenant B"
}

# Modules are completely isolated by tenant
GET /api/module
Headers: X-Tenant: tenant-a-id
# Returns only Tenant A's modules
```

### 4. API Documentation Generation

Generate dynamic API documentation from modules:

```bash
# Step 1: Query all active modules
GET /api/module?filter=isActive eq true

# Step 2: Extract services from each module
Response:
{
  "data": [
    {
      "name": "User Management",
      "services": [
        {
          "name": "Get Users",
          "controller": "UserController",
          "action": "GetUsers",
          "httpMethod": "GET"
        }
      ]
    }
  ]
}

# Step 3: Generate API docs
# GET /api/user → UserController.GetUsers
# POST /api/user → UserController.CreateUser
```

### 5. Service Registry & Discovery

Track all available services across the application:

```bash
# Query all services across modules
GET /api/module

# Extract unique controllers
Controllers: UserController, OrderController, PaymentController

# Extract endpoints by HTTP method
GET endpoints: 15
POST endpoints: 8
PUT endpoints: 6
DELETE endpoints: 4

# Use for service mesh configuration
# Use for API gateway routing
```

### 6. Gradual Feature Rollout

Roll out features gradually across tenants:

```bash
# Phase 1: Enable for beta tenants
POST /api/module
Headers: X-Tenant: beta-tenant-id
{
  "name": "AI Assistant",
  "isActive": true
}

# Phase 2: Monitor usage and performance

# Phase 3: Enable for all tenants
# Create module for each tenant
# Or use tenant-agnostic module with feature flags
```

## 🏗️ Architecture

### Clean Architecture Layers

```
src/
├── domain/                          # Domain Layer
│   ├── Domain/                      # Aggregates, Entities, Value Objects
│   │   ├── ModuleAggregate.cs      # Main aggregate root
│   │   ├── Entities/               # ServiceEntity
│   │   ├── Enums/                  # HttpMethod
│   │   ├── DomainEvents/           # Module & Service events
│   │   ├── Repositories/           # IModuleRepository
│   │   └── Errors.cs               # Domain error codes
│   ├── Application/                 # Application Layer
│   │   ├── Module/
│   │   │   ├── Commands/           # CreateModule, UpdateModule, DeleteModule
│   │   │   │                       # AddService, RemoveService
│   │   │   ├── Queries/            # GetModuleById, GetAllModule
│   │   │   ├── DataTransferObjects/# ModuleDto, ServiceDto
│   │   │   └── Setup/              # Application configuration
│   │   └── Common/                 # Shared application logic
│   └── Infrastructure/              # Infrastructure Layer
│       └── Repositories/           # MongoDB implementation
└── entrypoints/                     # Presentation Layer
    └── Rest/                        # REST API
        ├── Controllers/            # ModuleController
        ├── Program.cs              # Startup configuration
        ├── Dockerfile              # Docker configuration
        └── appsettings.json        # Configuration
```

### CQRS Pattern

**Commands** (Write operations):
- `CreateModuleCommand` - Create new module with services
- `UpdateModuleCommand` - Update module details, services, and status
- `DeleteModuleCommand` - Soft delete module
- `AddServiceCommand` - Add service to existing module
- `RemoveServiceCommand` - Remove service from module

**Queries** (Read operations):
- `GetModuleByIdQuery` - Get single module by ID
- `GetAllModuleQuery` - List modules with pagination and filtering

### Domain Events

Published to RabbitMQ after successful operations:

#### Module Events
- `ModuleCreatedDomainEvent` - Module created with initial services
- `ModuleUpdatedDomainEvent` - Module details or services updated
- `ModuleDeletedDomainEvent` - Module soft deleted

#### Service Events
- `ServiceAddedDomainEvent` - New service added to module
- `ServiceRemovedDomainEvent` - Service removed from module

**Event Payload Example**:
```json
{
  "eventId": "770e8400-e29b-41d4-a716-446655440003",
  "aggregateId": "550e8400-e29b-41d4-a716-446655440000",
  "eventType": "ModuleCreatedDomainEvent",
  "occurredAt": "2026-05-15T10:00:00Z",
  "name": "User Management",
  "description": "Module for managing users",
  "services": [...],
  "isActive": true
}
```

## 📐 Domain Model

### Module Aggregate Root

The `ModuleAggregate` is the main aggregate root:

```csharp
public class ModuleAggregate : AggregateRootBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<ServiceEntity> Services { get; private set; }
    public bool IsActive { get; private set; }
    
    // Audit fields (inherited from AggregateRootBase)
    public Guid CreatedBy { get; private set; }
    public Instant CreatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public Instant? UpdatedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }
    public Instant? DeletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
    // Factory method
    public static ModuleAggregate Create(
        Guid id, 
        string name, 
        string description, 
        List<ServiceEntity> services, 
        Guid createdBy);
    
    // Business methods
    public void Update(
        string name, 
        string description, 
        List<ServiceEntity> services, 
        bool isActive, 
        Guid updatedBy);
    
    public void Delete(Guid deletedBy);
    
    public void AddService(
        Guid id, 
        string name, 
        string controller, 
        string action, 
        HttpMethod httpMethod, 
        Guid addedBy);
    
    public void RemoveService(Guid idService, Guid removedBy);
}
```

### Service Entity

The `ServiceEntity` represents an API endpoint or feature:

```csharp
public class ServiceEntity : IEntityBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Controller { get; set; }
    public required string Action { get; set; }
    public HttpMethod HttpMethod { get; set; }
}
```

### Business Rules

The aggregate enforces business rules through guards:

1. **Module Creation**:
   - ID must not be empty
   - Name must not be empty (max 128 characters)
   - Description must not be empty (max 512 characters)
   - CreatedBy user ID must be valid
   - Module must not already exist

2. **Module Update**:
   - Name must not be empty
   - Description must not be empty
   - UpdatedBy user ID must be valid

3. **Module Delete**:
   - DeletedBy user ID must be valid
   - Sets IsDeleted = true and IsActive = false

4. **Service Addition**:
   - Service ID must not be empty
   - Name must not be empty
   - Controller must not be empty
   - Action must not be empty
   - HttpMethod must not be None
   - AddedBy user ID must be valid

5. **Service Removal**:
   - Service must exist in module
   - RemovedBy user ID must be valid

## 🧪 Testing

### Run All Tests
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Test Structure

```
tests/
├── unit/                            # Unit Tests
│   ├── Application.Test/           # Application layer tests
│   │   ├── Commands/               # Command handler tests
│   │   └── Queries/                # Query handler tests
│   ├── Domain.Test/                # Domain layer tests
│   │   ├── ModuleAggregateTest.cs # Aggregate tests
│   │   └── ServiceEntityTest.cs   # Entity tests
│   ├── Infrastructure.Test/        # Infrastructure tests
│   │   └── Repositories/          # Repository tests
│   └── Rest.Test/                  # REST API tests
│       └── Controllers/           # Controller tests
└── integration/                     # Integration Tests
    └── Rest.Test/                  # End-to-end API tests
        ├── ModuleIntegrationTest.cs
        └── ServiceIntegrationTest.cs
```

### Manual Testing with Postman

Import the Postman collection from `docs/postman/` for manual testing.

### Example Test Cases

**Unit Test - Module Creation**:
```csharp
[Fact]
public void Create_ValidModule_Success()
{
    // Arrange
    var id = Guid.NewGuid();
    var name = "User Management";
    var description = "Module for managing users";
    var services = new List<ServiceEntity>();
    var createdBy = Guid.NewGuid();
    
    // Act
    var module = ModuleAggregate.Create(id, name, description, services, createdBy);
    
    // Assert
    Assert.Equal(name, module.Name);
    Assert.Equal(description, module.Description);
    Assert.True(module.IsActive);
    Assert.Single(module.GetDomainEvents()); // ModuleCreatedDomainEvent
}
```

**Integration Test - Create Module API**:
```csharp
[Fact]
public async Task CreateModule_ValidRequest_ReturnsNoContent()
{
    // Arrange
    var command = new CreateModuleDto
    {
        Id = Guid.NewGuid(),
        Name = "Test Module",
        Description = "Test Description",
        Services = []
    };
    
    // Act
    var response = await _client.PostAsJsonAsync("/api/module", command);
    
    // Assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
}
```

## 💡 Best Practices

### Module Design

#### ✅ DO: Use descriptive module names
```csharp
// Good
"User Management"
"Order Processing"
"Payment Gateway"

// Bad
"Module1"
"Feature"
"New"
```

#### ✅ DO: Group related services in modules
```csharp
// Good: User Management module
Services:
- GetUser (GET)
- CreateUser (POST)
- UpdateUser (PUT)
- DeleteUser (DELETE)

// Bad: Mixed concerns
Services:
- GetUser
- ProcessPayment
- SendEmail
```

#### ✅ DO: Use consistent naming conventions
```csharp
// Good
Controller: "UserController"
Action: "GetUser", "CreateUser", "UpdateUser"
Name: "Get User", "Create User", "Update User"

// Bad
Controller: "users"
Action: "get_user", "CREATEUSER"
Name: "Retrieve User Data", "Add New User"
```

#### ❌ DON'T: Create modules for every single endpoint
```csharp
// Bad: Too granular
Module: "Get User"
Module: "Create User"
Module: "Update User"

// Good: Logical grouping
Module: "User Management"
  Services: GetUser, CreateUser, UpdateUser, DeleteUser
```

### Service Management

#### ✅ DO: Store accurate HTTP methods
```csharp
// Good
{ action: "GetUsers", httpMethod: "GET" }
{ action: "CreateUser", httpMethod: "POST" }
{ action: "UpdateUser", httpMethod: "PUT" }
{ action: "DeleteUser", httpMethod: "DELETE" }

// Bad
{ action: "GetUsers", httpMethod: "POST" }
{ action: "CreateUser", httpMethod: "None" }
```

#### ✅ DO: Use idempotent operations
```csharp
// Module updates should be idempotent
// Calling Update with same data multiple times = same result
PUT /api/module/{id}
{
  "name": "User Management",
  "isActive": true
}
```

#### ❌ DON'T: Store sensitive data in module descriptions
```csharp
// Bad
{
  "name": "Admin Module",
  "description": "Admin password: admin123"
}

// Good
{
  "name": "Admin Module",
  "description": "Administrative features for system management"
}
```

### Integration with RBAC

```csharp
// Listen to ModuleCreatedDomainEvent
public async Task Handle(ModuleCreatedDomainEvent @event)
{
    // Create permissions based on services
    foreach (var service in @event.Services)
    {
        var permission = new Permission
        {
            Name = $"{@event.Name}.{service.Name}",
            Resource = service.Controller,
            Action = service.Action,
            HttpMethod = service.HttpMethod
        };
        
        await _permissionRepository.CreateAsync(permission);
    }
}
```

## 🐛 Troubleshooting

### Common Issues

#### Issue: Module already exists error
**Cause**: Attempting to create a module with an ID that already exists.

**Solution**:
```bash
# Check if module exists
GET /api/module/{id}

# Use a different GUID
POST /api/module
{
  "id": "new-guid-here",
  ...
}
```

#### Issue: Service not found when removing
**Cause**: Attempting to remove a service that doesn't exist in the module.

**Solution**:
```bash
# Get module to verify services
GET /api/module/{id}

# Verify service ID in response
DELETE /api/module/{id}/services/{correct-service-id}
```

#### Issue: Validation error for name/description
**Cause**: Name or description exceeds maximum length.

**Solution**:
```csharp
// Name: max 128 characters
// Description: max 512 characters

// Good
{
  "name": "User Management",
  "description": "Module for managing users and permissions"
}

// Bad
{
  "name": "Very long name that exceeds 128 characters...",
  "description": "Very long description that exceeds 512 characters..."
}
```

#### Issue: HTTP method None is invalid
**Cause**: Service created with HttpMethod.None.

**Solution**:
```json
// Bad
{
  "httpMethod": "None"
}

// Good
{
  "httpMethod": "GET"
}
```

#### Issue: MongoDB connection timeout
**Cause**: MongoDB not accessible or wrong connection string.

**Solution**:
```bash
# Test MongoDB connectivity
mongosh "mongodb://localhost:27017"

# Check Docker containers
docker ps | grep mongo

# Verify connection string in appsettings.json
{
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-modules"
  }
}
```

#### Issue: RabbitMQ events not published
**Cause**: RabbitMQ not running or connection failed.

**Solution**:
```bash
# Check RabbitMQ container
docker ps | grep rabbitmq

# Check RabbitMQ logs
docker logs rabbitmq

# Access RabbitMQ management UI
open http://localhost:15672
# Default credentials: user/pass

# Verify configuration
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass"
  }
}
```

### Debug Mode

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logger": {
    "Enable": true,
    "Level": "Debug"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "CodeDesignPlus": "Trace",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Health Checks

Check service health:
```bash
curl http://localhost:5000/health
```

Expected response:
```json
{
  "status": "Healthy",
  "checks": [
    { "name": "MongoDB", "status": "Healthy" },
    { "name": "Redis", "status": "Healthy" },
    { "name": "RabbitMQ", "status": "Healthy" }
  ]
}
```

## 🔄 Module Management Flow

### Complete Module Lifecycle

```
Client creates module
     ↓
[Controller] → Validates request
     ↓
[CreateModuleCommand] → Creates ModuleAggregate
     ↓
[Handler] → Checks if module already exists
     ↓
[ModuleAggregate.Create] → Validates business rules
     ↓
[Repository] → Persists to MongoDB
     ↓
[IPubSub] → Publishes ModuleCreatedDomainEvent
     ↓
Returns 204 No Content
     ↓
RBAC service listens to event
     ↓
Creates permissions based on services
     ↓
Assigns permissions to roles
```

### Service Addition Flow

```
Client adds service to module
     ↓
[Controller] → Validates request
     ↓
[AddServiceCommand] → Identifies module
     ↓
[Handler] → Loads ModuleAggregate from repository
     ↓
[ModuleAggregate.AddService] → Validates business rules
     ↓
Updates services collection
     ↓
[Repository] → Persists changes
     ↓
[IPubSub] → Publishes ServiceAddedDomainEvent
     ↓
Returns 204 No Content
     ↓
RBAC service listens to event
     ↓
Creates new permission for service
```

## 🔗 Integration Points

### RBAC Microservice

The Modules microservice integrates with the RBAC microservice:

```
Modules MS                          RBAC MS
    │                                  │
    │──ModuleCreatedDomainEvent──────▶│
    │                                  │
    │                           Creates Permissions
    │                                  │
    │──ServiceAddedDomainEvent────────▶│
    │                                  │
    │                           Creates Permission
    │                                  │
    │──ModuleDeletedDomainEvent────────▶│
    │                                  │
    │                           Soft-deletes Permissions
```

### Frontend Applications

Frontend apps query modules for dynamic UI generation:

```
Frontend                          Modules MS
    │                                  │
    │────GET /api/module (active)────▶│
    │                                  │
    │◀──────List of active modules────│
    │                                  │
    │   Render navigation based on    │
    │   available modules              │
    │                                  │
    │   Check user permissions         │
    │   against module services        │
```

### API Gateway

API Gateway uses module services for routing:

```
API Gateway                        Modules MS
    │                                  │
    │────GET /api/module──────────────▶│
    │                                  │
    │◀──────All modules with services─│
    │                                  │
    │   Configure routes:              │
    │   GET /api/user → UserController.GetUser
    │   POST /api/user → UserController.CreateUser
```

## 🔒 Security

### Authentication & Authorization

- **OAuth2/OpenID Connect**: JWT bearer token required
- **Multi-tenancy**: X-Tenant header mandatory
- **User Context**: CreatedBy/UpdatedBy tracked via IUserContext
- **RBAC Integration**: Module operations require appropriate permissions

### Security Best Practices

1. **Always validate tenant**: Verify X-Tenant header on every request
2. **Audit trail**: Track who created/updated/deleted modules
3. **Soft delete**: Never physically delete modules for audit purposes
4. **HTTPS only**: Require HTTPS in production
5. **Rate limiting**: Implement rate limiting on module operations
6. **Input validation**: Validate all inputs with FluentValidation
7. **Principle of least privilege**: Grant minimum necessary permissions

### Secure Configuration

```json
{
  "Security": {
    "RequireHttpsMetadata": true,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidIssuer": "https://identity-server.com",
    "ValidAudiences": ["modules-api"]
  }
}
```

## ❓ FAQ

### General Questions

**Q: What is a module?**
A: A module is a logical grouping of related services (API endpoints/features) in your application. For example, "User Management" module contains all user-related services (GetUser, CreateUser, etc.).

**Q: What is a service?**
A: A service represents an individual API endpoint or feature within a module. It stores metadata like controller name, action name, and HTTP method.

**Q: Why separate modules and services?**
A: This separation allows hierarchical organization, easier permission management, and logical grouping of related functionality.

**Q: Can a service belong to multiple modules?**
A: No, services are owned by a single module. If you need to share functionality, create separate service entries in each module.

**Q: What happens when I delete a module?**
A: Soft delete: The module is marked as deleted (IsDeleted=true, IsActive=false) but not physically removed. This preserves audit history.

### Technical Questions

**Q: How does RBAC integration work?**
A: When modules/services are created, domain events are published to RabbitMQ. The RBAC microservice listens to these events and creates corresponding permissions.

**Q: Can I query modules by name?**
A: Yes, use OData filters: `GET /api/module?filter=name contains 'User'`

**Q: What's the maximum number of services per module?**
A: There's no hard limit, but for maintainability, we recommend keeping modules focused with 5-15 services.

**Q: How do I enable/disable a module?**
A: Update the module with `isActive: false` to disable, or `isActive: true` to enable.

**Q: Can I change a module's ID?**
A: No, the ID is immutable. Create a new module if you need a different ID.

**Q: How is multi-tenancy enforced?**
A: The X-Tenant header is mandatory. Repository queries automatically filter by tenant. Modules are isolated.

### Troubleshooting Questions

**Q: Why do I get "Module already exists"?**
A: You're attempting to create a module with an ID that already exists. Use a different GUID.

**Q: Why can't I remove a service?**
A: The service ID doesn't exist in the module. Query the module to get the correct service ID.

**Q: Why are domain events not published?**
A: Check RabbitMQ connectivity. Verify RabbitMQ is running and configuration is correct.

**Q: How do I debug module creation failures?**
A: Enable debug logging, check validation errors, verify MongoDB connectivity, and check user context.

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

### Development Workflow

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/add-bulk-import
   ```

3. **Make your changes**
   - Follow existing code style
   - Add tests for new features
   - Update documentation

4. **Run tests**
   ```bash
   dotnet test
   ```

5. **Commit with conventional commits**
   ```bash
   git commit -m "feat: add bulk module import"
   git commit -m "fix: resolve service duplication issue"
   git commit -m "docs: update API documentation"
   ```

6. **Push and create Pull Request**
   ```bash
   git push origin feature/add-bulk-import
   ```

### Code Standards

- **C# Coding Style**: Follow .editorconfig rules
- **Test Coverage**: Aim for >80% coverage
- **Documentation**: Update README.md for new features
- **Naming Conventions**:
  - Commands: `{Action}Command` (e.g., `CreateModuleCommand`)
  - Queries: `{Action}Query` (e.g., `GetModuleByIdQuery`)
  - Handlers: `{CommandOrQuery}Handler`
  - Tests: `{MethodName}_{Scenario}_{ExpectedResult}`

### Pull Request Checklist

- [ ] Code compiles without warnings
- [ ] All tests pass
- [ ] New features have tests
- [ ] Documentation updated
- [ ] CHANGELOG.md updated (if applicable)
- [ ] No breaking changes (or documented with migration guide)
- [ ] Follows SOLID principles and Clean Architecture

## 📞 Support & Resources

### Getting Help

- **GitHub Issues**: [Report bugs or request features](https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Modules/issues)
- **Discussions**: [Ask questions and share ideas](https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Modules/discussions)
- **Documentation**: [CodeDesignPlus Docs](https://codedesignplus.github.io/)
- **Email**: support@codedesignplus.com

### Related Projects

- **CodeDesignPlus.Net.Sdk**: Core SDK with shared abstractions
- **CodeDesignPlus.Net.Microservice.RBAC**: Role-based access control microservice
- **CodeDesignPlus.Environment.Dev**: Local development environment setup
- **Template Repository**: Microservice scaffolding template

## 📄 License

This project is licensed under the **GNU Lesser General Public License v3.0** - see the [LICENSE.md](LICENSE.md) file for details.

### What This Means

- ✅ **Commercial use**: Use in commercial applications
- ✅ **Modification**: Modify the source code
- ✅ **Distribution**: Distribute the software
- ✅ **Private use**: Use privately
- ⚠️ **Disclose source**: Must disclose source for derivative works
- ⚠️ **License and copyright notice**: Include license and copyright
- ⚠️ **Same license**: Derivative works must use LGPL v3.0

## 🙏 Acknowledgments

Built with:
- **CodeDesignPlus SDK** - Core abstractions and utilities
- **.NET 9** - Microsoft's modern development platform
- **MongoDB** - Flexible document database
- **RabbitMQ** - Reliable message broker
- **Redis** - High-performance caching
- **Open Source Community** - For all the amazing tools and libraries

---

**Made with ❤️ by CodeDesignPlus**

*For questions, suggestions, or contributions, please open an issue or pull request.*
