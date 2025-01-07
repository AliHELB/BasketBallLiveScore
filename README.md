# BasketBallLiveScore

## Description
The **Basketball Management System** is a comprehensive application for managing basketball championship activities, including teams, players, matches, and real-time events. The application combines a robust **ASP.NET Core** backend and an **Angular** frontend, providing a seamless and efficient user experience. The system supports role-based access, real-time updates via **SignalR**, and secure authentication powered by **JWT**.

---

## Features

### Backend Features (ASP.NET Core)
- **Team Management**: Create, update, and manage teams with associated players.
- **Player Management**: Add and manage player details linked to their respective teams.
- **Match Management**: Schedule matches, assign teams, and configure match settings such as number of quarters and duration.
- **Event Logging**: Encode real-time match events like baskets, fouls, and substitutions.
- **Real-Time Communication**: Utilize **SignalR** to broadcast match events to viewers instantly.
- **Authentication & Authorization**:
  - **JWT**-based authentication for secure access.
  - Role-based authorization for restricting functionality to specific users (Admin, Encoder, Viewer).
- **RESTful API**: Exposed endpoints for interaction with the frontend.

### Frontend Features (Angular)
- **User Interface**: Clean and responsive interface for match and team management.
- **Role-Specific Views**:
  - Admin: Manage users, teams, matches, and roles.
  - Encoder: Log match events in real-time.
  - Viewer: Follow live match updates and events.
- **Authentication System**: Login and registration with JWT-based token management.
- **Real-Time Updates**:
  - Live updates of match events using **SignalR**.
- **Protected Routes**: Angular Guards ensure role-based access to sensitive areas.

---

## Architecture

### Backend
The backend is built using **ASP.NET Core** with the following key components:
- **Controllers**: Handle HTTP requests and return JSON responses.
- **Services**: Encapsulate business logic and interact with the database.
- **Models**: Represent database entities such as User, Team, Match, Player, etc.
- **SignalR Hubs**: Enable real-time communication for broadcasting match events.

### Frontend
The frontend is developed using **Angular**, leveraging:
- **Components**: Modular units of the user interface for handling specific functionalities (e.g., AuthComponent, ViewerComponent).
- **Services**: Provide reusable logic for interacting with the backend API.
- **Guards and Interceptors**: Enhance security and streamline API communication.

---

## Installation

### Prerequisites
- **.NET Core SDK** (6.0 or later).
- **Node.js** (16 or later).
- **Angular CLI** (14 or later).
- **SQL Server**: For database storage.

### Setup Instructions
1. **Clone the repository**:
   ```bash
   git clone https://github.com/AliHELB/BasketBallLiveScore.git
   cd BasketBallLiveScore
   ```

2. **Backend Setup**:
   - Navigate to the backend folder:
     ```bash
     cd backend
     ```
   - Configure the database connection in `appsettings.json`.
   - Apply migrations:
     ```bash
     dotnet ef database update
     ```
   - Run the backend server:
     ```bash
     dotnet run
     ```

3. **Frontend Setup**:
   - Navigate to the frontend folder:
     ```bash
     cd frontend
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the Angular development server:
     ```bash
     ng serve
     ```

---

## Usage

### Roles and Access Levels
- **Admin**:
  - Full control over all aspects of the system.
  - Manage users, teams, matches, and roles.
- **Encoder**:
  - Log real-time match events (baskets, fouls, substitutions).
- **Viewer**:
  - Follow live match updates and events.

### Key Functionalities
1. **Authentication**:
   - Secure login and registration system.
   - Token-based authentication using **JWT**.

2. **Team and Player Management**:
   - Create and manage teams and their associated players.

3. **Match Scheduling and Management**:
   - Admins can create matches by assigning home and away teams.
   - Set match-specific configurations such as quarters and duration.

4. **Real-Time Event Logging**:
   - Encoders can log baskets, fouls, and substitutions during live matches.
   - Notifications are broadcast in real-time using **SignalR**.

5. **Viewer Experience**:
   - Viewers can follow live match updates and see real-time events.

---

## Technologies Used

### Backend
- **ASP.NET Core**: Provides the main infrastructure for the backend.
- **Entity Framework Core**: Used for database interactions.
- **SignalR**: Enables real-time communication.
- **JWT**: Handles secure authentication and role-based authorization.

### Frontend
- **Angular**: Powers the user interface.
- **RxJS**: Manages asynchronous data streams.
- **Bootstrap**: Adds responsive design.

---
