# Il Vecchio Forno API

## Overview

### Objective

Il Vecchio Forno is a RESTful backend API designed to manage pizzas and ingredients for a pizzeria.
This is a portfolio project demonstrating the implementation of a RESTful API following Clean Architecture principles
with ASP.NET Core.

### Features

**Pizza Management**

- List all active pizzas (available on the menu)
- List all disabled pizzas
- Add new pizza
- Update existing pizza details (name, price, description, ingredients and quantities, etc.)
- Disable pizza (remove from the menu without deleting)

**Ingredient Management**

- List all available ingredients
- Add new ingredients to the inventory

**Pizza-Ingredient Relationship**

- Associate ingredients with pizzas
- Manage ingredient quantities per pizza

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 18.1 or later](https://www.postgresql.org/download/) for production

## Getting Started

### Clone the repository

```bash
git clone https://github.com/ThomasDeOliv/IlVecchioForno.git
cd IlVecchioForno
```

### Configure the database

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "IlVecchioFornoContext": "Host=<your_host>;Port=<your_port>;Username=<your_user>;Password=<your_password>;Database=ilvecchioforno"
  }
}
```

Migrations are auto-applied.

### Run the application

```bash
dotnet run --project src/IlVecchioForno.API/IlVecchioForno.API.csproj
```

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.