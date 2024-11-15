using System.Text;
using ApiProject.Middlewares;
using BookingApp.Business.DataProtection;
using BookingApp.Business.Operations.Feature;
using BookingApp.Business.Operations.Hotel;
using BookingApp.Business.Operations.Setting;
using BookingApp.Business.Operations.User;
using BookingApp.Data.Context;
using BookingApp.Data.Entities;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args); // Creating a new WebApplicationBuilder

// Add services to the container.
builder.Services.AddControllers(); // Adding controllers to the service container

builder.Services.AddEndpointsApiExplorer(); // Adding endpoints API explorer to the service container
builder.Services.AddSwaggerGen(options => // Adding Swagger generation to the service container
{
    var jwtSecurityScheme = new OpenApiSecurityScheme // Creating a new OpenApiSecurityScheme object
    {
        Scheme = "Bearer", // Setting the scheme to Bearer
        BearerFormat = "JWT", // Setting the bearer format to JWT
        Name = "JwtAuthentication", // Setting the name to JwtAuthentication
        In = ParameterLocation.Header, // Setting the parameter location to Header
        Type = SecuritySchemeType.Http, // Setting the type to Http
        Description = "Put **_ONLY_** your JWT Bearer Token in the Textbox below!", // Setting the description
        Reference = new OpenApiReference // Creating a new OpenApiReference object
        {
            Id = JwtBearerDefaults.AuthenticationScheme, // Setting the ID to JwtBearerDefaults.AuthenticationScheme
            Type = ReferenceType.SecurityScheme // Setting the type to SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme); // Adding the security definition to the options
    options.AddSecurityRequirement(new OpenApiSecurityRequirement // Adding the security requirement to the options
    {
        {jwtSecurityScheme, Array.Empty<string>()} // Adding the security scheme to the requirement
    });
});

builder.Services.AddScoped<IDataProtection, DataProtection>(); 
// Im telling the service container that whenever it sees IDataProtection, it should create an instance of DataProtection.
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath,"AppData","DataProtectionKeys")); // Creating a new DirectoryInfo object for the keys directory
builder.Services.AddDataProtection().SetApplicationName("BookingApp").PersistKeysToFileSystem(keysDirectory); // Adding data protection to the service container and setting the application name and keys directory

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => // Adding authentication to the service container with JWT Bearer
{
    options.TokenValidationParameters = new TokenValidationParameters // Setting the token validation parameters
    {
        ValidateIssuer = true, // Validate the issuer of the token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Set the valid issuer

        ValidateAudience = true, // Validate the audience of the token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Set the valid audience

        ValidateLifetime = true, // Validate the token's lifetime
        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)) // Set the signing key
    };
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Getting the connection string from the configuration
builder.Services.AddDbContext<BookingAppDbContext>(options => options.UseNpgsql(connectionString)); // Adding the DbContext to the service container with PostgreSQL as the provider

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
// Im telling the service container that whenever it sees IRepository, it should create an instance of Repository.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Im telling the service container that whenever it sees IRepository, it should create an instance of Repository.
builder.Services.AddScoped<IUserService, UserManager>(); 
// Im telling the service container that whenever it sees IUserService, it should create an instance of UserManager.
builder.Services.AddScoped<IFeatureService, FeatureManager>();
// Im telling the service container that whenever it sees IFeatureService, it should create an instance of FeatureManager.
builder.Services.AddScoped<IHotelService, HotelManager>(); 
// Im telling the service container that whenever it sees IHotelService, it should create an instance of HotelManager.
builder.Services.AddScoped<ISettingService, SettingManager>(); 
// Im telling the service container that whenever it sees ISettingService, it should create an instance of SettingManager.

var app = builder.Build(); // Building the application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Checking if the environment is Development
{
    app.UseSwagger(); // Using Swagger in the development environment
    app.UseSwaggerUI(); // Using Swagger UI in the development environment
}


app.UseMaintanenceMiddleware(); 
// This line of code is responsible for adding the middleware to the pipeline.
// Adding HTTPS redirection to the pipeline
app.UseHttpsRedirection(); 
// Adding authentication to the pipeline
app.UseAuthentication();
// Adding authorization to the pipeline
app.UseAuthorization(); 
// Mapping the controllers to the pipeline
app.MapControllers(); 
// Running the application
app.Run();