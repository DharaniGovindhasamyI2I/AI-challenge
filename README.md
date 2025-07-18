# Clean Architecture E-Commerce Application

[![Build](https://github.com/jasontaylordev/CleanArchitecture/actions/workflows/build.yml/badge.svg)](https://github.com/jasontaylordev/CleanArchitecture/actions/workflows/build.yml)
[![CodeQL](https://github.com/jasontaylordev/CleanArchitecture/actions/workflows/codeql.yml/badge.svg)](https://github.com/jasontaylordev/CleanArchitecture/actions/workflows/codeql.yml)
[![Nuget](https://img.shields.io/nuget/v/Clean.Architecture.Solution.Template?label=NuGet)](https://www.nuget.org/packages/Clean.Architecture.Solution.Template)
[![Nuget](https://img.shields.io/nuget/dt/Clean.Architecture.Solution.Template?label=Downloads)](https://www.nuget.org/packages/Clean.Architecture.Solution.Template)
![Twitter Follow](https://img.shields.io/twitter/follow/jasontaylordev?label=Follow&style=social)

A comprehensive e-commerce application built with Clean Architecture principles, featuring order management, product catalog, customer management, analytics, and todo management capabilities.

## 🚀 Features

### 📦 **Order Management System**
- **Order Creation**: Create orders with multiple items, shipping addresses, and notes
- **Order Status Management**: Full order lifecycle from Pending → Confirmed → Shipped → Delivered
- **Order Cancellation**: Cancel orders at any stage (except delivered)
- **Payment Processing**: Track payment status (Pending, Paid, Failed)
- **Order Items**: Add, remove, and update quantities of items in orders
- **Order Numbering**: Automatic generation of unique order numbers
- **Order History**: Complete audit trail with timestamps

**API Endpoints:**
```
GET    /api/orders                    - Get paginated orders with filters
GET    /api/orders/{id}               - Get order by ID
POST   /api/orders                    - Create new order
PUT    /api/orders/{id}/status        - Update order status
PUT    /api/orders/{id}/confirm       - Confirm order
PUT    /api/orders/{id}/ship          - Ship order
PUT    /api/orders/{id}/deliver       - Deliver order
PUT    /api/orders/{id}/cancel        - Cancel order
```

### 🛍️ **Product Catalog Management**
- **Product CRUD**: Create, read, update, and delete products
- **Inventory Management**: Track stock levels and update inventory
- **Product Categories**: Organize products by categories
- **Price Management**: Handle product pricing with decimal precision
- **Product Search**: Filter products by name, category, and price range

**API Endpoints:**
```
GET    /api/products                  - Get products with filters
GET    /api/products/{id}             - Get product by ID
POST   /api/products                  - Create new product
PUT    /api/products/{id}/inventory   - Update product inventory
PUT    /api/products/{id}             - Update product (planned)
DELETE /api/products/{id}             - Delete product (planned)
```

### 👥 **Customer Management**
- **Customer CRUD**: Full customer lifecycle management
- **Customer Profiles**: Store customer information and contact details
- **Customer Search**: Find customers by various criteria

**API Endpoints:**
```
GET    /api/customers                 - Get all customers
GET    /api/customers/{id}            - Get customer by ID
POST   /api/customers                 - Create new customer
PUT    /api/customers/{id}            - Update customer
DELETE /api/customers/{id}            - Delete customer
```

### 📊 **Analytics & Reporting**
- **Sales Metrics**: Track sales performance and trends
- **Business Intelligence**: Generate insights from order and product data

**API Endpoints:**
```
GET    /api/analytics/sales-metrics   - Get sales analytics data
```

### ✅ **Todo Management System**
- **Todo Lists**: Create and manage todo lists
- **Todo Items**: Add, update, and complete todo items
- **Priority Management**: Set priorities for todo items
- **Notes & Details**: Add detailed notes to todo items

**API Endpoints:**
```
GET    /api/TodoLists                 - Get todo lists
GET    /api/TodoLists/{id}            - Get todo list by ID
POST   /api/TodoLists                 - Create todo list
PUT    /api/TodoLists/{id}            - Update todo list
DELETE /api/TodoLists/{id}            - Delete todo list

GET    /api/TodoItems                 - Get todo items with pagination
POST   /api/TodoItems                 - Create todo item
PUT    /api/TodoItems/{id}            - Update todo item
PUT    /api/TodoItems/UpdateItemDetails - Update todo item details
DELETE /api/TodoItems/{id}            - Delete todo item
```

### 🔐 **Authentication & Authorization**
- **User Registration**: Create new user accounts
- **User Login**: Secure authentication with JWT tokens
- **Role-based Access**: Administrator and user roles
- **Token Refresh**: Automatic token refresh mechanism

**API Endpoints:**
```
POST   /api/Users/Register            - Register new user
POST   /api/Users/Login               - User login
POST   /api/Users/Refresh             - Refresh JWT token
```

### 🗄️ **Data Persistence & Caching**
- **Entity Framework Core**: ORM with PostgreSQL, SQL Server, or SQLite support
- **Redis Caching**: Distributed caching for improved performance
- **Database Migrations**: Automatic schema management
- **Test Containers**: Isolated test databases

## 🏗️ Project Structure & Architecture

### **Clean Architecture Layers**

```
src/
├── Domain/                    # 🎯 Core Business Logic
│   ├── Entities/             # Business entities (Order, Product, Customer, etc.)
│   ├── ValueObjects/         # Value objects (Address, Money, etc.)
│   ├── Enums/               # Domain enums (OrderStatus, PaymentStatus, etc.)
│   ├── Events/              # Domain events
│   ├── Exceptions/          # Domain-specific exceptions
│   └── Common/              # Shared domain components
│
├── Application/              # 🔧 Application Business Rules
│   ├── Orders/              # Order use cases (Commands & Queries)
│   ├── Products/            # Product use cases
│   ├── Customers/           # Customer use cases
│   ├── Analytics/           # Analytics use cases
│   ├── TodoLists/           # Todo management use cases
│   ├── TodoItems/           # Todo item use cases
│   └── Common/              # Shared application components
│
├── Infrastructure/           # 🌐 External Concerns
│   ├── Data/                # Database context and configurations
│   ├── Identity/            # Authentication and authorization
│   ├── Caching/             # Redis cache implementation
│   ├── Payments/            # Payment processing
│   └── Migrations/          # Database migrations
│
└── Web/                     # 🖥️ Presentation Layer
    ├── Controllers/         # API controllers
    ├── Services/            # Web-specific services
    ├── ClientApp/           # Frontend application (Angular/React)
    └── Infrastructure/      # Web-specific infrastructure
```

### **File Flow & Dependencies**

```
Request Flow:
HTTP Request → Controller → MediatR → Command/Query Handler → Domain Service → Repository → Database

Response Flow:
Database → Repository → Domain Service → Command/Query Handler → DTO → Controller → HTTP Response
```

### **Key Design Patterns**

- **CQRS (Command Query Responsibility Segregation)**: Separate commands and queries
- **Mediator Pattern**: Decoupled communication between components
- **Repository Pattern**: Data access abstraction
- **Domain Events**: Loose coupling between domain operations
- **Value Objects**: Immutable objects representing domain concepts
- **Specification Pattern**: Complex query logic encapsulation

## 🧪 Testing Strategy

### **Testing Pyramid**

```
┌─────────────────────────────────────┐
│           E2E Tests                 │  ← Acceptance Tests (Web.AcceptanceTests)
├─────────────────────────────────────┤
│        Integration Tests            │  ← API Integration Tests (Infrastructure.IntegrationTests)
├─────────────────────────────────────┤
│        Functional Tests             │  ← Application Layer Tests (Application.FunctionalTests)
├─────────────────────────────────────┤
│           Unit Tests                │  ← Component Tests (Application.UnitTests)
└─────────────────────────────────────┘
```

### **Test Types & Coverage**

#### **1. Unit Tests** (`tests/Application.UnitTests/`)
- **Purpose**: Test individual components in isolation
- **Scope**: Commands, queries, validators, behaviors
- **Dependencies**: Mocked using Moq
- **Examples**: 
  - `CreateOrderCommandTests` - Validation logic testing
  - `RequestLoggerTests` - Behavior testing

#### **2. Functional Tests** (`tests/Application.FunctionalTests/`)
- **Purpose**: Test application layer with real database
- **Scope**: Full command/query pipeline with actual data persistence
- **Dependencies**: In-memory or test container database
- **Examples**:
  - `CreateOrderTests` - End-to-end order creation
  - `UpdateOrderStatusTests` - Status transition testing

#### **3. Integration Tests** (`tests/Infrastructure.IntegrationTests/`)
- **Purpose**: Test infrastructure components with external dependencies
- **Scope**: Database operations, external services, API endpoints
- **Dependencies**: Test containers (PostgreSQL, Redis)
- **Examples**:
  - `OrdersControllerIntegrationTests` - Full API testing
  - `PerformanceTests` - Performance benchmarking

#### **4. Acceptance Tests** (`tests/Web.AcceptanceTests/`)
- **Purpose**: Test complete user workflows
- **Scope**: Full application stack through UI/API
- **Dependencies**: Full application with test database
- **Examples**:
  - `Login.feature` - User authentication workflow

### **Running Tests**

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Application.UnitTests
dotnet test tests/Application.FunctionalTests
dotnet test tests/Infrastructure.IntegrationTests
dotnet test tests/Web.AcceptanceTests

# Run tests with specific filter
dotnet test --filter "FullyQualifiedName~Orders"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Using Cake build script
dotnet cake --target=Test        # Run all tests
dotnet cake --target=Basic       # Run basic tests (excluding acceptance)
```

### **Test Data Management**

- **Test Containers**: Isolated PostgreSQL and Redis instances
- **Database Seeding**: Automatic test data creation
- **Test Isolation**: Each test runs in isolation with clean state
- **Performance Testing**: Response time validation

## 🚀 Getting Started

### **Prerequisites**

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for frontend development)
- [Docker](https://www.docker.com/) (for test containers)

### **Installation & Setup**

```bash
# Clone the repository
git clone <repository-url>
cd CleanArchitecture

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run database migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Web

# Launch the application
cd src/Web
dotnet run
```

### **Configuration**

The application supports multiple database providers:

```bash
# PostgreSQL (default)
dotnet run --environment Development

# SQLite
dotnet run --environment SQLite

# SQL Server
dotnet run --environment SQLServer
```

### **API Documentation**

Once running, access the API documentation at:
- **Swagger UI**: `https://localhost:5001/swagger`
- **HTTP Files**: Use the `.http` files in `src/Web/` for testing

## 🛠️ Development Workflow

### **Creating New Features**

1. **Domain Layer**: Define entities, value objects, and domain events
2. **Application Layer**: Create commands/queries and handlers
3. **Infrastructure Layer**: Implement repositories and external services
4. **Web Layer**: Add controllers and endpoints
5. **Testing**: Write comprehensive tests for all layers

### **Code Generation**

```bash
# Create new use case
dotnet new ca-usecase --name CreateOrder --feature-name Orders --usecase-type command --return-type int

# Create new query
dotnet new ca-usecase -n GetOrders -fn Orders -ut query -rt OrdersVm
```

### **Database Migrations**

```bash
# Add new migration
dotnet ef migrations add "AddOrderTable" --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations

# Update database
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

## 🏗️ Architecture Benefits

### **Maintainability**
- Clear separation of concerns
- Single responsibility principle
- Dependency inversion

### **Testability**
- Isolated components
- Mockable dependencies
- Comprehensive test coverage

### **Scalability**
- Modular design
- Loose coupling
- Event-driven architecture

### **Flexibility**
- Database agnostic
- Framework independent
- Easy to extend

## 🛡️ Security Features

- **JWT Authentication**: Secure token-based authentication
- **Role-based Authorization**: Fine-grained access control
- **Input Validation**: Comprehensive request validation
- **SQL Injection Protection**: Parameterized queries
- **CORS Configuration**: Cross-origin resource sharing

## 📊 Performance Features

- **Redis Caching**: Distributed caching for improved performance
- **Database Optimization**: Efficient queries and indexing
- **Async/Await**: Non-blocking operations throughout
- **Pagination**: Efficient data retrieval for large datasets

## 🔧 Technologies Used

### **Backend**
- [ASP.NET Core 9](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- [Entity Framework Core 9](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR) - CQRS implementation
- [AutoMapper](https://automapper.org/) - Object mapping
- [FluentValidation](https://fluentvalidation.net/) - Input validation
- [PostgreSQL](https://www.postgresql.org/) - Primary database
- [Redis](https://redis.io/) - Caching layer

### **Testing**
- [NUnit](https://nunit.org/) - Unit testing framework
- [FluentAssertions](https://fluentassertions.com/) - Readable assertions
- [Moq](https://github.com/devlooped/moq) - Mocking framework
- [Respawn](https://github.com/jbogard/Respawn) - Database reset
- [Testcontainers](https://testcontainers.com/) - Integration testing

### **Frontend** (Optional)
- [Angular 18](https://angular.dev/) or [React 18](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Bootstrap](https://getbootstrap.com/) - UI framework

## 📈 Monitoring & Logging

- **Structured Logging**: Comprehensive application logging
- **Performance Monitoring**: Response time tracking
- **Error Handling**: Global exception handling
- **Health Checks**: Application health monitoring

## 🚀 Deployment

### **Azure Deployment**

```bash
# Install Azure Developer CLI
azd auth login
azd up
```

### **Docker Deployment**

```bash
# Build and run with Docker
docker-compose up -d
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add comprehensive tests
5. Submit a pull request

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🆘 Support

- **Documentation**: Check the inline code documentation
- **Issues**: [Create an issue](https://github.com/jasontaylordev/CleanArchitecture/issues/new/choose)
- **Discussions**: [Join the discussion](https://github.com/jasontaylordev/CleanArchitecture/discussions)

---

**Built with ❤️ using Clean Architecture principles**
