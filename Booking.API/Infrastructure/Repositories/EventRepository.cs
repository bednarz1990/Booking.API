using Booking.API.Core.Entities;
using Booking.API.Core.Interfaces;
using Booking.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event> GetByIdAsync(long eventId)
    {
        return (await _context.Events.FindAsync(eventId))!;
    }

    public async Task UpdateAsync(Event eventEntity)
    {
        _context.Events.Update(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Event eventEntity)
    {
        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event> AddAsync(Event eventEntity)
    {
        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
        return eventEntity;
    }

    public bool IsEventNameUnique(string eventName)
    {
        return _context.Events.All(e => e.Name != eventName);
    }

    public async Task<IEnumerable<Event>> GetEventsByCountryAsync(string country)
    {
        return await _context.Events
            .Where(e => e.Country == country)
            .ToListAsync();
    }

    public async Task<EventRegistration> GetRegistrationByEmailAndEventIdAsync(long eventId, string email) =>
        (await _context.EventRegistrations
            .FirstOrDefaultAsync(er => er.EventId == eventId && er.Email == email))!;

    public async Task<EventRegistration> AddRegistrationAsync(EventRegistration registration)
    {
        _context.EventRegistrations.Add(registration);
        await _context.SaveChangesAsync();
        return registration;
    }
}