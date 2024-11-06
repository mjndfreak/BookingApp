using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class HotelFeatureEntity : BaseEntity
{
    public int HotelId { get; set; }
    public HotelEntity Hotel { get; set; }
    public int FeatureId { get; set; }
    public FeatureEntity Feature { get; set; }
}

public class HotelFeatureConfiguration : BaseConfiguration<HotelFeatureEntity>
{
    public override void Configure(EntityTypeBuilder<HotelFeatureEntity> builder)
    {
        builder.Ignore(x => x.Id);
        builder.HasKey("HotelId", "FeatureId");
        base.Configure(builder);
        
    }
}