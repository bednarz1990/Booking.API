using Booking.API.Core.Entities;

namespace Booking.API.Core.Interfaces;

public interface IEventRepository
{
    Task<Event> GetByIdAsync(long eventId);
    Task UpdateAsync(Event eventEntity);
    Task DeleteAsync(Event eventEntity);
    Task<Event> AddAsync(Event eventEntity);
    Task<IEnumerable<Event>> GetAllAsync();

    bool IsEventNameUnique(string name);

    Task<IEnumerable<Event>> GetEventsByCountryAsync(string country);

    Task<EventRegistration> GetRegistrationByEmailAndEventIdAsync(long eventId, string email);

    Task<EventRegistration> AddRegistrationAsync(EventRegistration registration);
}