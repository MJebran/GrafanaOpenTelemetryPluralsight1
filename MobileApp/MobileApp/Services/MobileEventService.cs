
using MobileApp.Inits;
using TicketClassLib.Data;
using TicketClassLib.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileApp.Services;

public class MobileEventService : IEventService
{
    LocalDbInit init { get; set; }

    public MobileEventService(LocalDbInit init)
    {
        init.InitializeLocalDatabase();
        this.init = init;
    }
    public async Task<Event> AddEvent(string name, DateTime date)
    {
        var context = init.EstablishConnection();

        Event e = new Event() { Name = name, Eventdate = date };

        await context.InsertAsync(e);

        await context.CloseAsync();
        return e;
    }

    public async Task<List<Event>> GetAll()
    {
        var context = init.EstablishConnection();

        return await context.Table<Event>().ToListAsync();
    }

    public Task<Event?> GetEvent(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        var context = init.EstablishConnection();

        await context.InsertOrReplaceAsync(newEvent);

        await context.CloseAsync();
        return newEvent;
    }
}
