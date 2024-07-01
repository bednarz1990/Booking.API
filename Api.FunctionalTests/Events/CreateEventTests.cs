using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Abstractions;
using Booking.API.Application.DTO;
using FluentAssertions;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Api.FunctionalTests.Events;

public class CreateEventTests : BaseFuncTest
{
    public CreateEventTests(FunctionalTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenNumberOfSeatsIsMissing()
    {
        // Arrange
        var request = new EventCreateDto()
        {
            Country = "test",
            StartDate = DateTime.Now,
            Description = "test",
            Name = "test"
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/Event", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        string responseContent = await response.Content.ReadAsStringAsync();
        var validationErrors = JsonConvert.DeserializeObject<List<ValidationFailure>>(responseContent);
        validationErrors.Should().ContainSingle()
            .Which.PropertyName.Should().Be("NumberOfSeats");
    }
}