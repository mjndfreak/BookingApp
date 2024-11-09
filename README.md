# BookingApp

BookingApp is a web application designed to manage hotel bookings. It provides functionalities for user registration, login, and managing reservations.

## Features

- User Registration
- User Login
- Hotel Management
- Room Management
- Reservation Management

## Technologies Used

- **Backend**: ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Frontend**: Not yet implemented

## Getting Started

### Prerequisites

- .NET 6 SDK
- PostgreSQL

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/BookingApp.git
    ```
2. Navigate to the project directory:
    ```sh
    cd BookingApp
    ```
3. Set up the database:
    - Update the connection string in `appsettings.json`.
    - Apply migrations:
        ```sh
        dotnet ef database update
        ```

### Running the Application

1. Navigate to the `BookingApp.WebApi` directory:
    ```sh
    cd BookingApp.WebApi
    ```
2. Run the application:
    ```sh
    dotnet run
    ```

### API Endpoints

- **User Registration**: `POST /api/auth/register`
- **User Login**: `POST /api/auth/login`
- **Get Hotels**: `GET /api/hotels`
- **Get Rooms**: `GET /api/rooms`
- **Create Reservation**: `POST /api/reservations`

## Contributing

Contributions are welcome! Please fork the repository and create a pull request.

## Status

**Project is in progress**.
