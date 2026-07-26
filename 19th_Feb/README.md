# CropDeal - .NET Web API with Angular Frontend

A full-stack crop trading platform built with ASP.NET Core Web API and Angular.

## Project Structure

```
CROPDEAL/
├── Controllers/          # API Controllers
├── Models/              # Data Models and DTOs
├── Services/            # Business Logic Services
├── Data/                # Entity Framework DbContext
├── cropdeal-frontend/   # Angular Frontend Application
├── wwwroot/             # Static files (built Angular app)
├── Program.cs           # API Configuration
└── CROPDEAL.csproj      # .NET Project File
```

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access
- **Crop Management**: CRUD operations for crop listings
- **User Management**: Registration and login functionality
- **Responsive UI**: Modern Angular frontend with responsive design
- **API Documentation**: Swagger/OpenAPI documentation

## Prerequisites

- .NET 9.0 SDK
- Node.js (v18 or higher)
- SQL Server (LocalDB or full instance)
- Angular CLI (`npm install -g @angular/cli`)

## Quick Start

### Development Mode (Separate Servers)

1. **Clone and Setup**:
   ```bash
   git clone <repository-url>
   cd CROPDEAL
   ```

2. **Run Development Setup**:
   ```bash
   start-dev.bat
   ```
   This will:
   - Install Angular dependencies
   - Start Angular dev server on http://localhost:4200
   - Start .NET API on https://localhost:7000

### Production Build

1. **Build Angular Frontend**:
   ```bash
   build-frontend.bat
   ```

2. **Run .NET Application**:
   ```bash
   dotnet run
   ```
   Access the application at https://localhost:7000

## Manual Setup

### Backend (.NET API)

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Update database**:
   ```bash
   dotnet ef database update
   ```

3. **Run API**:
   ```bash
   dotnet run
   ```

### Frontend (Angular)

1. **Navigate to frontend directory**:
   ```bash
   cd cropdeal-frontend
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start development server**:
   ```bash
   ng serve
   ```

## API Endpoints

- `POST /api/Registration/Login` - User login
- `POST /api/Registration/Register` - User registration
- `GET /api/Crops` - Get all crops
- `POST /api/Crops` - Add new crop
- `DELETE /api/Crops/{id}` - Delete crop

## Configuration

### Database Connection
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CropDealDB;Trusted_Connection=true;"
  }
}
```

### JWT Settings
Configure JWT in `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "CropDeal",
    "Audience": "CropDealUsers"
  }
}
```

## User Roles

- **Admin**: Full access to all features
- **Farmer**: Can add, edit, and delete their own crops
- **Buyer**: Can view and purchase crops

## Technologies Used

### Backend
- ASP.NET Core 9.0
- Entity Framework Core
- JWT Authentication
- AutoMapper
- Swagger/OpenAPI

### Frontend
- Angular 19
- TypeScript
- RxJS
- Angular Forms
- HTTP Client

## Development Notes

- CORS is configured to allow requests from `http://localhost:4200`
- JWT tokens are stored in localStorage
- HTTP interceptor automatically adds Authorization headers
- Route guards protect authenticated routes
- Form validation is implemented on both client and server side

## Troubleshooting

1. **CORS Issues**: Ensure Angular dev server runs on port 4200
2. **Database Issues**: Run `dotnet ef database update`
3. **JWT Issues**: Check JWT configuration in appsettings.json
4. **Build Issues**: Ensure Node.js and .NET SDK are properly installed

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request