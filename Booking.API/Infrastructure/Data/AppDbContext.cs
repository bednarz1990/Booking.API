using Booking.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventRegistration> EventRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<EventRegistration>().HasKey(er => er.Id);


        modelBuilder.Entity<EventRegistration>()
            .HasOne<Event>(er => er.Event)
            .WithMany(e => e.EventRegistrations)
            .HasForeignKey(er => er.EventId)
            .IsRequired();
    }
}