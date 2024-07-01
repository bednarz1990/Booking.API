using Booking.API.Application.DTO;
using Booking.API.Core.Entities;
using Booking.API.WebAPI.Utilities;

namespace Booking.API.Core.Interfaces;

public interface IEventService
{
    Task<Result<long>> CreateEventAsync(EventCreateDto eventCreateDto);
    Task<IEnumerable<EventCreateDto>> GetAllEventsAsync();
    Task<Result<Event>> GetEventByIdAsync(long eventId);
    Task<Result<EventUpdateDto>> UpdateEventAsync(long eventId, EventUpdateDto eventCreateDto);
    Task<Result> DeleteEventAsync(long eventId);
    Task<IEnumerable<Event>> SearchEventsByCountryAsync(string country);
    Task<Result<long?>> RegisterUserForEventAsync(long eventId, UserDto userDto);
}