🎮 Game Catalog
A full-stack application for browsing and managing a catalog of video games.

📁 Project Structure
This repository contains the following parts of the application:

game-catalog-frontend/: The Angular frontend application.

GamecatalogAPI/: The .NET Core Web API service.

🛠️ Technology Stack
Frontend: Angular 

Backend: .NET core

Database: SQL Server (EF Core)

Mapping: AutoMapper

API Documentation: Scalar / OpenAPI

🚀 Getting Started
Prerequisites
Node.js (includes npm)

Angular CLI (npm install -g @angular/cli)

SQL Server (LocalDB or azure)

.NET SDK 

💻 Backend Setup 
Configure Connection:
Ensure your connection string in appsettings.json matches your local SQL Server instance.

Prepare the Database:
Open your terminal in the backend folder and run the following commands:

If you want you can use the sql script to get few games setup in the database [sqlscript](./sqlscript)


Bash
# Create the migration (handles the decimal precision fix)
dotnet ef migrations add InitialMigration

# Apply changes to the database
dotnet ef database update
Run the Server:

Bash
dotnet run
The API will be active and you can view the documentation via the Scalar/OpenAPI link provided in the terminal output. available at 'https://localhost:7187/api/Games

🎨 Frontend Setup (game-catalog-frontend)
Navigate & Install:

Bash
cd game-catalog-frontend
npm install
Run Development Server:

Bash
ng serve
The application will be available at http://localhost:4200/.

## 🚀 Future Plans and Improvements

- Build a `Users` table and integrate JWT / Google OpenID login
- Create a `UserGame` table with foreign keys (UserId, GameId)
- Store images in Firebase instead of saving image URLs
- Integrate WebSockets for a shared chat room
- Introduce Redis caching to reduce database calls under high traffic
- Dockerize frontend and backend for cloud deployment
- Use Let's Encrypt for SSL certification

---

## 🔐 Security & Attack Protection

- Protection against SQL Injection (using EF Core parameterized queries)
- XSS protection through DTO usage and model validation
- Authentication secured using JWT tokens

