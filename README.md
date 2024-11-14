# BookingApp

BookingApp is a hotel management system that allows administrators to manage hotels, features, reservations, and settings. This project is built using C# and ASP.NET Core, with Entity Framework Core for data access.

## Table of Contents

- [Project Structure](#project-structure)
- [Data Flow](#data-flow)
- [Entities](#entities)
- [Controllers](#controllers)
- [Services](#services)
- [Filters](#filters)
- [Configuration](#configuration)
- [Unit of Work](#unit-of-work)
- [Repositories](#repositories)
- [Migrations](#migrations)
- [Enums](#enums)
- [Database Context](#database-context)
- [Technologies Used](#technologies-used)
- [Conclusion](#conclusion)

## Project Structure

The project is organized into several main directories:

- `BookingApp.Data`: Contains the data access layer, including entities, configurations, repositories, unit of work, and the database context.
- `BookingApp.Business`: Contains the business logic layer, including services and DTOs.
- `BookingApp.WebApi`: Contains the web API layer, including controllers and filters.

## Data Flow

1. **Request**: A client sends a request to the API.
2. **Controller**: The request is handled by a controller in the `BookingApp.WebApi` project.
3. **Service**: The controller calls a service in the `BookingApp.Business` project to perform the necessary business logic.
4. **Repository**: The service interacts with the repository in the `BookingApp.Data` project to access the database.
5. **Response**: The service returns the result to the controller, which then sends the response back to the client.

## Entities

Entities represent the data models used in the application. They are defined in the `BookingApp.Data.Entities` namespace.

- `HotelEntity`: Represents a hotel with properties like `Name`, `Stars`, `Location`, and `AccomodationType`.
- `FeatureEntity`: Represents a feature that can be associated with a hotel.
- `HotelFeatureEntity`: Represents the relationship between a hotel and a feature.
- `ReservationEntity`: Represents a reservation made by a user.
- `RoomEntity`: Represents a room in a hotel.
- `UserEntity`: Represents a user of the application.
- `SettingEntity`: Represents application settings, such as maintenance mode.

## Controllers

Controllers handle incoming HTTP requests and return responses. They are defined in the `BookingApp.WebApi.Controllers` namespace.

- `HotelsController`: Manages hotel-related operations such as adding, updating, deleting, and retrieving hotels.
- `SettingsController`: Manages application settings, including toggling maintenance mode.

## Services

Services contain the business logic of the application. They are defined in the `BookingApp.Business.Operations` namespace.

- `HotelManager`: Implements the `IHotelService` interface to manage hotel operations.
- `SettingService`: Manages application settings.

## Filters

Filters are used to apply cross-cutting concerns such as authorization and request filtering. They are defined in the `BookingApp.WebApi.Filters` namespace.

- `TimeControlFilter`: Restricts access to certain endpoints based on the time of day.

## Configuration

Configuration classes define the database schema using the Fluent API. They are defined in the `BookingApp.Data.Entities` namespace.

- `HotelConfiguration`: Configures the `HotelEntity` schema.
- `FeatureConfiguration`: Configures the `FeatureEntity` schema.
- `HotelFeatureConfiguration`: Configures the `HotelFeatureEntity` schema.
- `ReservationConfiguration`: Configures the `ReservationEntity` schema.
- `RoomConfiguration`: Configures the `RoomEntity` schema.
- `UserConfiguration`: Configures the `UserEntity` schema.

## Unit of Work

The Unit of Work pattern is used to manage transactions and coordinate changes across multiple repositories. It is defined in the `BookingApp.Data.UnitOfWork` namespace.

- `IUnitOfWork`: Interface defining the contract for the Unit of Work.
- `UnitOfWork`: Implementation of the Unit of Work interface.

## Repositories

Repositories are used to encapsulate data access logic. They are defined in the `BookingApp.Data.Repositories` namespace.

- `IRepository<T>`: Generic interface defining the contract for a repository.
- `Repository<T>`: Implementation of the generic repository interface.

## Migrations

Migrations are used to manage changes to the database schema over time. They are created and applied using Entity Framework Core's migration tools.

## Enums

Enums are used to define a set of named constants. They are defined in the `BookingApp.Data.Enums` namespace.

- `AccomodationType`: Enum representing different types of accommodations (e.g., Hotel, Hostel, Apartment).

## Database Context

The `BookingAppDbContext` class in the `BookingApp.Data.Context` namespace is the main entry point for interacting with the database. It defines the `DbSet` properties for each entity and applies the configurations.

```csharp
public class BookingAppDbContext : DbContext
{
    public BookingAppDbContext(DbContextOptions<BookingAppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FeatureConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new HotelFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.Entity<SettingEntity>().HasData(new SettingEntity
        {
            Id = 1,
            MaintenenceMode = false
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<FeatureEntity> Features => Set<FeatureEntity>();
    public DbSet<HotelEntity> Hotels => Set<HotelEntity>();
    public DbSet<HotelFeatureEntity> HotelFeatures => Set<HotelFeatureEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
    public DbSet<RoomEntity> Rooms => Set<RoomEntity>();
    public DbSet<SettingEntity> Settings { get; set; }
}
```

## Technologies Used

- **C#**: The primary programming language used for the application.
- **ASP.NET Core**: The web framework used to build the API.
- **Entity Framework Core**: The ORM (Object-Relational Mapper) used for data access.
- **Microsoft.AspNetCore.DataProtection**: Used for data protection and encryption.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Used for JWT authentication.
- **Microsoft.IdentityModel.Tokens**: Used for token validation.
- **Swashbuckle.AspNetCore**: Used for generating Swagger documentation.
- **PostgreSQL**: The database used for storing application data.
- **JetBrains Rider**: The IDE used for development.

## Conclusion

This README provides an overview of the BookingApp project, including its structure, data flow, key components, and technologies used. For more detailed information, refer to the individual files and classes in the project.
