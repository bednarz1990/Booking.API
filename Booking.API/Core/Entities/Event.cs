namespace Booking.API.Core.Entities;

public class Event
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfSeats { get; set; }
    public ICollection<EventRegistration> EventRegistrations { get; set; }
}