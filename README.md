# CleanArchitecture

[![Build Status](#)](#) [![License](#)](#)

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Build & Run](#build--run)
- [Deployment](#deployment)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgements](#acknowledgements)
- [Contact](#contact)

## Overview
CleanArchitecture is a modular, scalable template for building cloud-ready applications with ASP.NET Core, React, and Azure. It follows Clean Architecture principles to ensure separation of concerns and maintainability.

## Features
- Clean Architecture with clear separation of concerns
- ASP.NET Core Web API
- React and Angular frontends
- Azure Bicep scripts for infrastructure
- MediatR pipeline (logging, caching, authorization)
- Entity Framework Core migrations
- Identity and authentication
- SignalR for real-time features
- Comprehensive test coverage

## Project Structure
- `src/Domain`: Core business logic and entities
- `src/Application`: Application services and use cases
- `src/Infrastructure`: Data access, caching, identity, integrations
- `src/Web`: API host and web clients (React, Angular)
- `infra/`: Azure Bicep scripts for infrastructure
- `tests/`: Unit, integration, functional, and acceptance tests

## Getting Started
### Prerequisites
- [.NET 7+](https://dotnet.microsoft.com/)
- [Node.js & npm](https://nodejs.org/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

### Setup
```bash
git clone <repo-url>
cd CleanArchitecture
dotnet restore
```

## Build & Run
### Backend API
```bash
cd src/Web
dotnet run
```
### React Frontend
```bash
cd src/Web/ClientApp-React
npm install
npm start
```
### Angular Frontend
```bash
cd src/Web/ClientApp
npm install
npm start
```
### Database Migrations
```bash
cd src/Infrastructure
dotnet ef database update
```
### Run Tests
```bash
dotnet test
```

## Deployment
- See `infra/` for Azure Bicep scripts and deployment instructions.

## Usage
- API documentation available at `/swagger` when running the backend.
- Frontend available at `/` (React) or `/ClientApp` (Angular).

## Contributing
- Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License
This project is licensed under the MIT License.

## Acknowledgements
- Inspired by Microsoft Clean Architecture template and community best practices.

## Contact
- Maintainer: [Your Name] (your.email@example.com)
