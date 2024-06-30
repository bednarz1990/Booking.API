using Booking.API.Application.DTO;
using Booking.API.Core.Interfaces;
using FluentValidation;

namespace Booking.API.Application.Validators;

public class EventValidator : AbstractValidator<EventDto>
{
    private readonly IEventRepository _eventRepository;

    public EventValidator(IEventRepository eventRepository)

    {
        _eventRepository = eventRepository;

        RuleFor(x => x.Name)
            .MaximumLength(50)
            .Must(BeUniqueName).WithMessage("Event name must be unique.");

        RuleFor(x => x.Country)
            .MaximumLength(20);

        RuleFor(x => x.StartDate)
            .NotNull();

        RuleFor(x => x.NumberOfSeats)
            .InclusiveBetween(1, 100);
    }

    private bool BeUniqueName(string name)
    {
        return _eventRepository.IsEventNameUnique(name);
    }
}