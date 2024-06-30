using Booking.API.Application.DTO;
using FluentValidation;

namespace Booking.API.Application.Validators;

public class EventUpdateValidator : AbstractValidator<EventDto>
{
    public EventUpdateValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50);

        RuleFor(x => x.Country)
            .MaximumLength(20);

        RuleFor(x => x.StartDate)
            .NotNull();

        RuleFor(x => x.NumberOfSeats)
            .InclusiveBetween(1, 100);
    }
}
