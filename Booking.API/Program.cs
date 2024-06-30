using Booking.API.Application.DTO;
using Booking.API.Application.Mappers;
using Booking.API.Application.Services;
using Booking.API.Application.Validators;
using Booking.API.Core.Interfaces;
using Booking.API.Infrastructure.Data;
using Booking.API.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BookingDb"));

builder.Services.AddAuthorization();
builder.Services.AddControllers();  
builder.Services.AddAutoMapper(typeof(EventMapper));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<EventValidator>();
builder.Services.AddScoped<UserValidator>();
builder.Services.AddScoped<EventUpdateValidator>();
builder.Services.AddFluentValidationClientsideAdapters(); 
builder.Services.AddValidatorsFromAssemblyContaining<EventValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EventUpdateValidator>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
