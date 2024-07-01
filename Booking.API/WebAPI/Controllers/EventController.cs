using Booking.API.Application.DTO;
using Booking.API.Application.Validators;
using Booking.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await eventService.GetAllEventsAsync();

        var eventDtos = events.Select(e => new
        {
            e.Name,
            e.Country,
            e.StartDate
        }).ToList();

        return Ok(eventDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEventById(long id)
    {
        var @event = await eventService.GetEventByIdAsync(id);
        if (@event == null) return NotFound();

        var eventDto = new EventDto
        {
            Name = @event.Name,
            Country = @event.Country,
            Description = @event.Description,
            StartDate = @event.StartDate,
            NumberOfSeats = @event.NumberOfSeats
        };

        return Ok(eventDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto, [FromServices] EventValidator validator)
    {
        var validationResult = await validator.ValidateAsync(eventDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        try
        {
            var eventId = await eventService.CreateEventAsync(eventDto);
            return Ok(new { id = eventId });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating event: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(long id, [FromBody] EventDto eventDto,
        [FromServices] EventUpdateValidator validator)
    {
        var validationResult = await validator.ValidateAsync(eventDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updatedEvent = await eventService.UpdateEventAsync(id, eventDto);
        if (updatedEvent == null) return NotFound();

        return Ok(updatedEvent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id)
    {
        var result = await eventService.DeleteEventAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EventDto>>> SearchEventsByCountry([FromQuery] string country)
    {
        var events = await eventService.SearchEventsByCountryAsync(country);
        var eventDtos = events.Select(e => new
        {
            e.Name,
            e.Country,
            e.StartDate
        }).ToList();

        return Ok(eventDtos);
    }

    [HttpPost("{eventId}/register")]
    public async Task<IActionResult> RegisterForEvent(long eventId, [FromBody] UserDto userDto,
        [FromServices] UserValidator validator)
    {
        var validationResult = await validator.ValidateAsync(userDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);


        var eventRegistrationId = await eventService.RegisterUserForEventAsync(eventId, userDto);
        if (eventRegistrationId.IsSuccess) return Ok("User registered successfully for the event.");

        return BadRequest(eventRegistrationId.Errors);
    }
}