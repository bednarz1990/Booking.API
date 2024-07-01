namespace Booking.API.Application.DTO;

public class EventCreateDto
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfSeats { get; set; }
}