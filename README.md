# User Directory Solution

This repository contains a full-stack User Directory application with an ASP.NET Core backend and an Angular frontend.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js (v18+ recommended)](https://nodejs.org/)
- [npm](https://www.npmjs.com/)
- (Optional) [MongoDB](https://www.mongodb.com/) and/or SQL Server if you want to use real databases

---

## Backend (ASP.NET Core)

### 1. Restore & Build
```sh
cd UserDirectory
# Restore dependencies
 dotnet restore
# Build the solution
 dotnet build
```

### 2. Database Setup
- By default, the backend uses SQL Server and/or MongoDB. Update `WebApi/appsettings.json` as needed for your environment.
- To apply migrations and seed the database:
```sh
dotnet ef database update --project Infrastructure.Sql --startup-project WebApi
```

### 3. Run the API
```sh
dotnet run --project WebApi
```
- The API will be available at `https://localhost:5001` or `http://localhost:5000` by default.

### 4. Run Integration Tests
```sh
dotnet test WebApi.IntegrationTests/WebApi.IntegrationTests.csproj
```

---

## Frontend (Angular)

### 1. Install Dependencies
```sh
cd user-directory-frontend
npm install
```

### 2. Run the Angular App
```sh
npm start
# or
ng serve
```
- The app will be available at `http://localhost:4200` by default.

### 3. Run Frontend E2E Tests
```sh
# Playwright (recommended)
npx playwright test

# Or, if using Angular's built-in e2e
ng e2e
```

---

## Notes
- Ensure the backend API is running before using the frontend.
- **CORS:** The backend (`Program.cs`) allows requests from `http://localhost:4200` by default. If your frontend runs on a different host or port, update the CORS policy in `UserDirectory/WebApi/Program.cs`:
  ```csharp
  builder.Services.AddCors(options => {
      options.AddPolicy("AllowAngularApp",
          policy => policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
  });
  ```
- **Frontend API URL:** The frontend uses `https://localhost:44385` as the default API base URL (see `user-directory-frontend/src/app/services/api.config.ts`). If your backend runs on a different URL, update this value:
  ```typescript
  export const API_BASE_URL = 'https://localhost:44385';
  ```
- For database connection strings, see `UserDirectory/WebApi/appsettings.json`.

---

## Troubleshooting
- If you encounter issues with missing packages, run `dotnet restore` (backend) or `npm install` (frontend).
- For database errors, check your connection strings and ensure the database server is running.
- For CORS issues, ensure the backend allows requests from the frontend's origin.

---

## Project Structure
- `UserDirectory/` - Backend solution (ASP.NET Core, C#)
- `user-directory-frontend/` - Frontend (Angular, TypeScript) 