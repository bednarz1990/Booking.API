using AutoMapper;
using Booking.API.Application.DTO;
using Booking.API.Core.Entities;
using Booking.API.WebAPI.Utilities;

namespace Booking.API.Application.Mappers;

public class EventMapper : Profile
{
    public EventMapper()
    {
        CreateMap<Event, EventCreateDto>().ReverseMap();
        CreateMap<Event, EventUpdateDto>().ReverseMap();

    }
}