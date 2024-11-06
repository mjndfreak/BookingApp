using BookingApp.Business.DataProtection;
using BookingApp.Business.Operations.User;
using BookingApp.Data.Entities;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataProtection, DataProtection>();
// Im telling the service container that whenever it sees IDataProtection, it should create an instance of DataProtection.
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath,"AppData","DataProtectionKeys"));
builder.Services.AddDataProtection().SetApplicationName("BookingApp").PersistKeysToFileSystem(keysDirectory);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingAppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
// Because IRepository is a generic interface, we need to use the AddScoped method with the typeof keyword.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Im telling the service container that whenever it sees IRepository, it should create an instance of Repository.
builder.Services.AddScoped<IUserService, UserManager>();
// Im telling the service container that whenever it sees IUserService, it should create an instance of UserManager.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
