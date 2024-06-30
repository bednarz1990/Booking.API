using AutoMapper;
using Booking.API.Application.DTO;
using Booking.API.Core.Entities;

namespace Booking.API.Application.Mappers;

public class EventMapper : Profile
{
    public EventMapper()
    {
        CreateMap<Event, EventDto>().ReverseMap();
    }
}