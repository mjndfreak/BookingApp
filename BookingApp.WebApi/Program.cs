using System.Text;
using ApiProject.Middlewares;
using BookingApp.Business.DataProtection;
using BookingApp.Business.Operations.Feature;
using BookingApp.Business.Operations.Hotel;
using BookingApp.Business.Operations.Setting;
using BookingApp.Business.Operations.User;
using BookingApp.Data.Entities;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "JwtAuthentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer Token in the Textbox below!",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>()}
    });
});

builder.Services.AddScoped<IDataProtection, DataProtection>();
// Im telling the service container that whenever it sees IDataProtection, it should create an instance of DataProtection.
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath,"AppData","DataProtectionKeys"));
builder.Services.AddDataProtection().SetApplicationName("BookingApp").PersistKeysToFileSystem(keysDirectory);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validate the issuer of the token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Set the valid issuer

        ValidateAudience = true, // Validate the audience of the token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Set the valid audience

        ValidateLifetime = true, // Validate the token's lifetime

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)) // Set the signing key
    };
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingAppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
// Because IRepository is a generic interface, we need to use the AddScoped method with the typeof keyword.
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMaintanenceMiddleware();
// This line of code is responsible for adding the middleware to the pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();