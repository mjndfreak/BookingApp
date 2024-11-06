using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class ReservationEntity : BaseEntity
{
    public int RoomId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int GuestCount { get; set; }
    
    //Relational Properties
    public UserEntity User { get; set; }
    public RoomEntity Room { get; set; }
}

public class ReservationConfiguration : BaseConfiguration<ReservationEntity>
{
    public override void Configure(EntityTypeBuilder<ReservationEntity> builder)
    {
        base.Configure(builder);
    }
}
    