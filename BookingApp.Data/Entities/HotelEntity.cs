using BookingApp.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class HotelEntity : BaseEntity
{
    public required string Name { get; set; }
    public int Stars { get; set; }
    public required string Location { get; set; }
    public AccomodationType AccomodationType { get; set; }
    
    // Relational properties
    public required ICollection<HotelFeatureEntity> HotelFeatures { get; set; }
    public required ICollection<RoomEntity> Rooms { get; set; }
}

public class HotelConfiguration : BaseConfiguration<HotelEntity>
{
    public override void Configure(EntityTypeBuilder<HotelEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Stars).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}