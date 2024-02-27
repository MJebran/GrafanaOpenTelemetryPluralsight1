using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TicketClassLib.Data;
using TicketClassLib.Services;
using TicketWebApp.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketWebApp.Services;

public class ApiEventService(IDbContextFactory<PostgresContext> dbFactory) : IEventService
{
    public async Task<Event> AddEvent(string name, DateTime date)
    {
        using var context = await dbFactory.CreateDbContextAsync();
        Event newEvent = new Event()
        {
            Id = 0,
            Name = name,
            Eventdate = date
        };

        var value = await context.Events.AddAsync(newEvent);
        await context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        var value = await context.Events.AddAsync(newEvent);
        await context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<List<Event>> GetAll()
    {
        using var context = await dbFactory.CreateDbContextAsync();

        return await context.Events
            .Include(e => e.Tickets)
            .ToListAsync();
    }

    public async Task<Event?> GetEvent(int id)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        return await context.Events
            .Include(e => e.Tickets)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
}
