namespace Booking.API.Core.Entities;

public class EventRegistration
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public string Email { get; set; }
    public Event Event { get; set; }
}