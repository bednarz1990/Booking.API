using Booking.API.Application.DTO;
using Booking.API.Core.Entities;

namespace Booking.API.Core.Interfaces;

public interface IEventService
{
    Task<long> CreateEventAsync(EventDto eventDto);
    Task<IEnumerable<EventDto>> GetAllEventsAsync();
    Task<Event> GetEventByIdAsync(long eventId);
    Task<EventDto> UpdateEventAsync(long eventId, EventDto eventDto);
    Task<bool> DeleteEventAsync(long eventId);
    Task<IEnumerable<Event>> SearchEventsByCountryAsync(string country);
    Task<long?> RegisterUserForEventAsync(long eventId, UserDto userDto);
}