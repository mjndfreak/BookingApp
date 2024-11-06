using BookingApp.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Data.Entities;

public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public UserType UserType { get; set; }
    
    // Relational Properties
    public ICollection<ReservationEntity> Reservations { get; set; }
}

public class UserConfiguration : BaseConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(40).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(40).IsRequired();
    }
}