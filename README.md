# InlämningWebb1 — ASP.NET Core Web API

A RESTful Web API built with ASP.NET Core 10 following Clean Architecture principles.
The project demonstrates CQRS, the Repository Pattern, JWT authentication, and role-based
authorization using a Product/Category domain.

---

## Technologies

| Technology | Purpose |
|---|---|
| ASP.NET Core 10 Web API | HTTP layer, routing, middleware |
| Clean Architecture | Project structure and dependency rules |
| CQRS + MediatR 14 | Separates reads from writes; decouples controllers from handlers |
| Repository Pattern | Abstracts database access behind interfaces |
| Entity Framework Core 10 | ORM — maps C# classes to SQL Server tables |
| SQL Server / LocalDB | Relational database |
| AutoMapper 16 | Maps domain entities to DTOs |
| FluentValidation 12 | Validates commands before handlers run |
| MediatR Pipeline Behavior | Runs validation automatically for every request |
| JWT Authentication | Stateless Bearer token authentication |
| Role-Based Authorization (RBAC) | Admin vs User access rules on endpoints |
| Scalar / OpenAPI | Interactive API documentation and testing UI |

---

## Architecture Overview

The project is divided into four layers. Dependencies only point **inward** — outer layers
know about inner layers, never the reverse.

```
┌──────────────────────────────────────────────────────┐
│  API Layer  (InlämningWebb1.API)                     │
│  Controllers, Program.cs, JWT middleware             │
│  → depends on Application + Infrastructure           │
├──────────────────────────────────────────────────────┤
│  Application Layer  (InlämningWebb1.Application)     │
│  Use cases: Commands, Queries, Handlers, Validators  │
│  Interfaces: ITokenService, IUserService             │
│  DTOs, AutoMapper Profiles, ValidationBehavior       │
│  → depends on Domain only                            │
├──────────────────────────────────────────────────────┤
│  Infrastructure Layer  (InlämningWebb1.Infrastructure)│
│  EF Core DbContext, Repositories, TokenService,      │
│  UserService, JwtSettings                            │
│  → depends on Application (implements its interfaces)│
├──────────────────────────────────────────────────────┤
│  Domain Layer  (InlämningWebb1.Domain)               │
│  Entities: Product, Category                         │
│  Repository interfaces: IRepository, IProductRepository│
│  → no dependencies                                   │
└──────────────────────────────────────────────────────┘
```

### Request flow

```
HTTP Request
  → Controller (receives DTO)
  → MediatR.Send(Command/Query)
  → ValidationBehavior (FluentValidation runs here)
  → Handler (calls Repository or Service)
  → Repository / TokenService / UserService
  → Database / In-memory store
  → Result mapped to DTO
  → HTTP Response
```

---

## Entity Relationship

```
Category (1) ──────< Product (many)
───────────────       ────────────────
Id       Guid         Id         Guid
Name     string       Name       string
                      Price      decimal
                      CategoryId Guid (FK)
```

- One **Category** has many **Products**.
- A **Product** must belong to exactly one **Category**.
- The relationship is enforced at the database level via a foreign key.

---

## Project Structure

```
InlämningWebb1/
├── InlämningWebb1.Domain/
│   ├── Entities/
│   │   ├── Product.cs
│   │   └── Category.cs
│   └── Interfaces/
│       ├── IRepository.cs
│       ├── IProductRepository.cs
│       └── ICategoryRepository.cs
│
├── InlämningWebb1.Application/
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   └── ValidationBehavior.cs
│   │   ├── Interfaces/
│   │   │   ├── ITokenService.cs
│   │   │   └── IUserService.cs
│   │   ├── Mappings/
│   │   │   └── ProductMappingProfile.cs
│   │   └── Models/
│   │       └── UserRecord.cs
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/Login/
│   │   │   │   ├── LoginCommand.cs
│   │   │   │   ├── LoginCommandHandler.cs
│   │   │   │   └── LoginCommandValidator.cs
│   │   │   └── DTOs/
│   │   │       ├── LoginDto.cs
│   │   │       └── AuthResponseDto.cs
│   │   └── Products/
│   │       ├── Commands/
│   │       │   ├── CreateProduct/  (Command, Handler, Validator)
│   │       │   ├── UpdateProduct/  (Command, Handler, Validator)
│   │       │   └── DeleteProduct/  (Command, Handler)
│   │       ├── Queries/
│   │       │   ├── GetAllProducts/ (Query, Handler)
│   │       │   └── GetProductById/ (Query, Handler)
│   │       └── DTOs/
│   │           ├── ProductDto.cs
│   │           ├── CreateProductDto.cs
│   │           └── UpdateProductDto.cs
│   └── DependencyInjection.cs
│
├── InlämningWebb1.Infrastructure/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   │       ├── ProductConfiguration.cs
│   │       └── CategoryConfiguration.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── ProductRepository.cs
│   │   └── CategoryRepository.cs
│   ├── Services/
│   │   ├── TokenService.cs
│   │   └── UserService.cs
│   ├── Settings/
│   │   └── JwtSettings.cs
│   ├── Migrations/
│   └── DependencyInjection.cs
│
└── InlämningWebb1.API/
    ├── Controllers/
    │   ├── AuthController.cs
    │   └── ProductsController.cs
    ├── appsettings.json
    └── Program.cs
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express or LocalDB (included with Visual Studio)

### 1. Clone and restore

```bash
git clone <repository-url>
cd InlämningWebb1
dotnet restore
```

### 2. Set the JWT signing key (User Secrets)

The JWT key is **never** stored in `appsettings.json`. Set it locally using User Secrets:

```bash
dotnet user-secrets init --project InlämningWebb1.API
dotnet user-secrets set "Jwt:Key" "InlamningWebb1-SuperSecret-Key-AtLeast32Chars!" --project InlämningWebb1.API
```

Verify it was saved:

```bash
dotnet user-secrets list --project InlämningWebb1.API
```

### 3. Apply database migrations

```bash
dotnet ef database update --project InlämningWebb1.Infrastructure --startup-project InlämningWebb1.API
```

### 4. Run the API

```bash
dotnet run --project InlämningWebb1.API
```

### 5. Open Scalar UI

Navigate to `https://localhost:{port}/scalar/v1` in your browser.
The port number is printed in the terminal when the app starts.

---

## API Endpoints

### Authentication

| Method | Endpoint | Auth required | Description |
|---|---|---|---|
| POST | `/api/auth/login` | No | Returns a JWT Bearer token |

**Request body:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGci...",
  "username": "admin",
  "role": "Admin"
}
```

### Products

| Method | Endpoint | Role required | Description |
|---|---|---|---|
| GET | `/api/products` | User or Admin | Get all products |
| GET | `/api/products/{id}` | User or Admin | Get product by ID |
| POST | `/api/products` | Admin only | Create a product |
| PUT | `/api/products/{id}` | Admin only | Update a product |
| DELETE | `/api/products/{id}` | Admin only | Delete a product |

All product endpoints require a valid JWT in the request header:
```
Authorization: Bearer <your-token>
```

**Create / Update request body:**
```json
{
  "name": "Gaming Laptop",
  "price": 12999.00,
  "categoryId": "00000000-0000-0000-0000-000000000001"
}
```

---

## Test Accounts

Two accounts are pre-configured in `UserService.cs` for development and testing:

| Username | Password | Role | Access |
|---|---|---|---|
| `admin` | `Admin123!` | Admin | Full CRUD on products |
| `user` | `User123!` | User | Read-only (GET endpoints) |

**To test role-based authorization:**
1. POST to `/api/auth/login` with admin credentials — copy the token
2. Use the token as `Authorization: Bearer <token>` on product endpoints
3. Repeat with user credentials — POST/PUT/DELETE will return `403 Forbidden`

---

## Security Notes

| Concern | Approach |
|---|---|
| JWT signing key | Stored in ASP.NET Core User Secrets (dev) — never in `appsettings.json` |
| Passwords | Plaintext for this assignment — production apps use bcrypt / Argon2 |
| No ASP.NET Identity | Intentionally omitted to keep the project focused |
| Token expiry | 60 minutes (configurable via `Jwt:ExpiresInMinutes` in `appsettings.json`) |

---

## School Submission Checklist

- [x] Clean Architecture (Domain → Application → Infrastructure → API)
- [x] CQRS with MediatR (Commands and Queries fully separated)
- [x] Repository Pattern with generic base and product-specific extension
- [x] Entity Framework Core with SQL Server and Migrations
- [x] AutoMapper for entity → DTO mapping
- [x] FluentValidation with MediatR Pipeline Behavior
- [x] JWT Authentication with Bearer tokens
- [x] Role-Based Authorization (Admin / User)
- [x] Scalar / OpenAPI for API documentation
- [x] User Secrets for sensitive configuration
- [x] 5+ meaningful Git commits, one per feature branch
