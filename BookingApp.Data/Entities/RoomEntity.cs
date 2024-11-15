using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class RoomEntity : BaseEntity
{
    public int HotelId { get; set; }
    public required string RoomNumber { get; set; }
    
    // Relational Properties
    public required ICollection<ReservationEntity> Reservations { get; set; }
    public required HotelEntity Hotel { get; set; }
}

public class RoomConfiguration : BaseConfiguration<RoomEntity>
{
    public override void Configure(EntityTypeBuilder<RoomEntity> builder)
    {
        base.Configure(builder);
    }
}