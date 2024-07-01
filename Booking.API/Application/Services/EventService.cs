using AutoMapper;
using Booking.API.Application.DTO;
using Booking.API.Core.Entities;
using Booking.API.Core.Interfaces;
using Booking.API.WebAPI.Utilities;

namespace Booking.API.Application.Services;

public class EventService(IEventRepository eventRepository, IMapper mapper) : IEventService
{
    public async Task<Event> GetEventByIdAsync(long eventId)
    {
        return await eventRepository.GetByIdAsync(eventId);
    }

    public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
    {
        var events = await eventRepository.GetAllAsync();
        return events.Select(mapper.Map<EventDto>).ToList();
    }

    public async Task<long> CreateEventAsync(EventDto eventDto)
    {
        var eventEntity = new Event
        {
            Name = eventDto.Name,
            Country = eventDto.Country,
            Description = eventDto.Description,
            StartDate = eventDto.StartDate,
            NumberOfSeats = eventDto.NumberOfSeats
        };

        var newEvent = await eventRepository.AddAsync(eventEntity);
        return newEvent.Id;
    }

    public async Task<EventDto> UpdateEventAsync(long eventId, EventDto eventDto)
    {
        var existingEvent = await eventRepository.GetByIdAsync(eventId);
        if (existingEvent == null) return null;

        existingEvent.Name = eventDto.Name;
        existingEvent.Country = eventDto.Country;
        existingEvent.Description = eventDto.Description;
        existingEvent.StartDate = eventDto.StartDate;
        existingEvent.NumberOfSeats = eventDto.NumberOfSeats;

        await eventRepository.UpdateAsync(existingEvent);

        return mapper.Map<EventDto>(existingEvent);
    }

    public async Task<bool> DeleteEventAsync(long eventId)
    {
        var existingEvent = await eventRepository.GetByIdAsync(eventId);

        if (existingEvent == null) return false;

        await eventRepository.DeleteAsync(existingEvent);

        return true;
    }

    public async Task<IEnumerable<Event>> SearchEventsByCountryAsync(string country)
    {
        return await eventRepository.GetEventsByCountryAsync(country);
    }

    public async Task<Result<long?>> RegisterUserForEventAsync(long eventId, UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email)) return Result<long?>.Failure("Email is required.");
        var existingRegistration =
            await eventRepository.GetRegistrationByEmailAndEventIdAsync(eventId, userDto.Email);
        if (existingRegistration != null)
            return Result<long?>.Failure("This email is already registered for the event.");

        var eventEntity = await eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null) return Result<long?>.Failure("Event not found.");

        var registration = new EventRegistration
        {
            EventId = eventId,
            Email = userDto.Email
        };

        var newRegistration = await eventRepository.AddRegistrationAsync(registration);
        return Result<long?>.Success(newRegistration.Id);
    }
}