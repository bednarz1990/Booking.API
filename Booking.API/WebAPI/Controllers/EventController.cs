using Booking.API.Application.DTO;
using Booking.API.Application.Validators;
using Booking.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventCreateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventCreateDto>> GetEventById(long id)
    {
        var result = await eventService.GetEventByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        var eventEntity = result.Data;
        var eventDto = new EventCreateDto
        {
            Name = eventEntity.Name,
            Country = eventEntity.Country,
            Description = eventEntity.Description,
            StartDate = eventEntity.StartDate,
            NumberOfSeats = eventEntity.NumberOfSeats
        };

        return Ok(eventDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto eventCreateDto, [FromServices] EventValidator validator)
    {
        var validationResult = await validator.ValidateAsync(eventCreateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await eventService.CreateEventAsync(eventCreateDto);
        return Ok(new { id = result.Data });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventUpdateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEvent(long id, [FromBody] EventUpdateDto eventUpdateDto,
        [FromServices] EventUpdateValidator validator)
    {
        var validationResult = await validator.ValidateAsync(eventUpdateDto);
        if (!validationResult.IsValid) 
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await eventService.UpdateEventAsync(id, eventUpdateDto);
        if (!result.IsSuccess) 
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent(long id)
    {
        var result = await eventService.DeleteEventAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventCreateDto>))]
    public async Task<ActionResult<IEnumerable<EventCreateDto>>> SearchEventsByCountry([FromQuery] string country)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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