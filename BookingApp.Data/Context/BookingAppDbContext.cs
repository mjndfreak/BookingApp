using BookingApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Data.Context;

public class BookingAppDbContext : DbContext
{
    public BookingAppDbContext(DbContextOptions<BookingAppDbContext> options) : base(options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configurations applied for entity mappings
        modelBuilder.ApplyConfiguration(new FeatureConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new HotelFeatureConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        // Seeding initial data for the SettingEntity
        modelBuilder.Entity<SettingEntity>().HasData(new SettingEntity
        {
            Id = 1,
            MaintenenceMode = false
        });
        
        base.OnModelCreating(modelBuilder);
    }

    // DbSets for different entities representing tables in the database
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<FeatureEntity> Features => Set<FeatureEntity>();
    public DbSet<HotelEntity> Hotels => Set<HotelEntity>();
    public DbSet<HotelFeatureEntity> HotelFeatures => Set<HotelFeatureEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
    public DbSet<RoomEntity> Rooms => Set<RoomEntity>();
    public DbSet<SettingEntity> Settings { get; set; }
}