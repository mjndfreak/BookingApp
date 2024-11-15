using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class FeatureEntity : BaseEntity
{
    public required string Title { get; set; }
    
    // Relational Properties
    public required ICollection<HotelFeatureEntity> HotelFeatures { get; set; }
}

public class FeatureConfiguration : BaseConfiguration<FeatureEntity>
{
    public override void Configure(EntityTypeBuilder<FeatureEntity> builder)
    {
        base.Configure(builder);
    }
}