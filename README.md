# BasketBallLiveScore

## Description
The **Basketball Management System** is a comprehensive application for managing teams, players, matches, and real-time events in a basketball championship. This project combines a **ASP.NET Core** backend and an **Angular** frontend, with authentication powered by **JWT** and real-time notifications via **SignalR**.

---

## Features

### Backend (ASP.NET Core)
- Management of users, teams, players, and matches.
- Role-based authentication and authorization (admin, encoder, viewer).
- Management of match events (baskets, fouls, substitutions).
- Real-time notifications using **SignalR** to inform viewers about live match events.
- RESTful API exposed for frontend interaction.

### Frontend (Angular)
- Reactive user interface to manage matches, teams, and players.
- Real-time view of match events powered by SignalR.
- Role management for users.
- Authentication with login and registration forms, using **JWT** tokens.

---

## Installation

### Prerequisites
- **.NET Core SDK**: Version 6.0 or higher.
- **Node.js**: Version 16 or higher.
- **Angular CLI**: Version 14 or higher.
- **SQL Server**: For the database.

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/AliHELB/BasketBallLiveScore.git
   cd BasketBallLiveScore
   ```

2. **Backend**:
   - Navigate to the backend folder:
     ```bash
     cd backend
     ```
   - Configure the connection string in `appsettings.json`.
   - Apply migrations:
     ```bash
     dotnet ef database update
     ```
   - Start the server:
     ```bash
     dotnet run
     ```

3. **Frontend**:
   - Navigate to the frontend folder:
     ```bash
     cd frontend
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the Angular server:
     ```bash
     ng serve
     ```

---

## Usage

### Available Roles
- **Admin**: Full access to manage users, teams, and matches.
- **Encoder**: Responsible for encoding match events in real-time.
- **Viewer**: Can only view matches and events.

### Key Features
1. **User Management**
   - Create an account via the registration form.
   - Log in to access the application.
   - Admins can manage roles via the "Manage Roles" section.

2. **Match Management**
   - Admins can create matches with teams and their players.
   - Encoders can log real-time events (baskets, fouls, substitutions).
   - Viewers can follow matches in real-time using **SignalR**.

3. **Real-Time Notifications**
   - Events (baskets, fouls, substitutions) are broadcast in real-time to viewers via **SignalR**.

4. **Authentication System**
   - Authentication powered by **JWT** with specific roles (admin, encoder, viewer).
   - Protected routes with Angular guards.

---

## Technologies Used

### Backend
- **ASP.NET Core**: Main framework.
- **Entity Framework Core**: ORM for database management.
- **SignalR**: Real-time notifications.
- **JWT**: Authentication and authorization.

### Frontend
- **Angular**: Main frontend framework.
- **RxJS**: Data stream management.
- **Bootstrap**: For design.

---