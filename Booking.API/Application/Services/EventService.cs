using AutoMapper;
using Booking.API.Application.DTO;
using Booking.API.Core.Entities;
using Booking.API.Core.Interfaces;
using Booking.API.WebAPI.Utilities;

namespace Booking.API.Application.Services;

public class EventService(IEventRepository eventRepository, IMapper mapper) : IEventService
{
    public async Task<Result<Event>> GetEventByIdAsync(long eventId)
    {
        var existingEvent = await GetExistingEventById(eventId);

        if (existingEvent == null) return Result<Event>.Failure("Event not found.");
        return Result<Event>.Success(existingEvent);
    }

    public async Task<IEnumerable<EventCreateDto>> GetAllEventsAsync()
    {
        var events = await eventRepository.GetAllAsync();
        return events.Select(mapper.Map<EventCreateDto>).ToList();
    }

    public async Task<Result<long>> CreateEventAsync(EventCreateDto eventCreateDto)
    {
        var eventEntity = new Event
        {
            Name = eventCreateDto.Name,
            Country = eventCreateDto.Country,
            Description = eventCreateDto.Description,
            StartDate = eventCreateDto.StartDate,
            NumberOfSeats = eventCreateDto.NumberOfSeats
        };

        var newEvent = await eventRepository.AddAsync(eventEntity);
        return Result<long>.Success(newEvent.Id);
    }

    public async Task<Result<EventUpdateDto>> UpdateEventAsync(long eventId, EventUpdateDto eventCreateDto)
    {
        var existingEvent = await GetExistingEventById(eventId);
        if (existingEvent == null) return Result<EventUpdateDto>.Failure("Event not found.");

        existingEvent.Name = eventCreateDto.Name;
        existingEvent.Country = eventCreateDto.Country;
        existingEvent.Description = eventCreateDto.Description;
        existingEvent.StartDate = eventCreateDto.StartDate;
        existingEvent.NumberOfSeats = eventCreateDto.NumberOfSeats;

        await eventRepository.UpdateAsync(existingEvent);
        var updatedDto = mapper.Map<EventUpdateDto>(existingEvent);

        return Result<EventUpdateDto>.Success(updatedDto);
    }

    public async Task<Result> DeleteEventAsync(long eventId)
    {
        var existingEvent = await GetExistingEventById(eventId);

        if (existingEvent == null) return Result.Failure("Event not found.");

        await eventRepository.DeleteAsync(existingEvent);
        return Result.Success();
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

        var existingEvent = await GetExistingEventById(eventId);
        if (existingEvent == null) return Result<long?>.Failure("Event not found.");

        var registration = new EventRegistration
        {
            EventId = eventId,
            Email = userDto.Email
        };

        var newRegistration = await eventRepository.AddRegistrationAsync(registration);
        return Result<long?>.Success(newRegistration.Id);
    }

    private async Task<Event> GetExistingEventById(long eventId)
    {
        var existingEvent = await eventRepository.GetByIdAsync(eventId);
        return existingEvent;
    }
}