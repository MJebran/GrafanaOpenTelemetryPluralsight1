using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TicketClassLib.Data;
using TicketClassLib.Services;

namespace TicketWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController(IEventService eventService) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<List<Event>> GetAll()
    {
        var events = await eventService.GetAll();
        return events;
    }

    [HttpPost("/newEvent/{name}")]
    public async Task<Event> AddEvent([FromBody] DateTime date, string name)
    {
        var newEvent = await eventService.AddEvent(name, date);
        return newEvent;
    }
}
