using System;
using Acme.RemoteFlights.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Acme.RemoteFlights.Domain
{
    public class FlightsDbContext : DbContext
    {
        public FlightsDbContext(DbContextOptions<FlightsDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new FlightConfiguration());
            builder.ApplyConfiguration(new FlightBookingsConfiguration());
            builder.ApplyConfiguration(new PassengerConfiguration());
        }

        internal class FlightConfiguration : IEntityTypeConfiguration<Flight>
        {
            public void Configure(EntityTypeBuilder<Flight> builder)
            {
                builder.ToTable("Flight");
                builder.HasKey(c => c.Id);
                builder.Property(c => c.FlightNumber).HasMaxLength(200);
            }
        }

        internal class FlightBookingsConfiguration : IEntityTypeConfiguration<FlightBooking>
        {
            public void Configure(EntityTypeBuilder<FlightBooking> builder)
            {
                builder.ToTable("FlightBooking");
                builder.HasKey(c => c.Id);
                // builder.HasOne(c => c).WithOne(..HasMaxLength(200);
            }
        }

        internal class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
        {
            public void Configure(EntityTypeBuilder<Passenger> builder)
            {
                builder.ToTable("Passenger");
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Name).HasMaxLength(200);
            }
        }
    }
}
