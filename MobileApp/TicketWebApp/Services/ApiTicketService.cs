using Microsoft.EntityFrameworkCore;
using TicketClassLib.Data;
using TicketClassLib.Enums;
using TicketClassLib.Exceptions;
using TicketClassLib.Requests;
using TicketClassLib.Services;
using TicketWebApp.Data;


namespace TicketWebApp.Services;

public class ApiTicketService(IDbContextFactory<PostgresContext> dbFactory) : ITicketService
{

    public event EventHandler? TicketsHaveChanged;

    public async Task<Ticket> CreateNewTicket(AddTicketRequest newRequest)
    {

        using var context = await dbFactory.CreateDbContextAsync();

        Guid guid = Guid.NewGuid();
        Ticket ticket = new Ticket()
        {
            Id = guid,
            Eventid = newRequest.EventId,
            Name = newRequest.UserName,
            Isscanned = false,
            Lastupdated = DateTime.UtcNow,
        };

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        TicketsHaveChanged?.Invoke(this, new EventArgs());
        return ticket;


    }

    public async Task<List<Ticket>> GetAll()
    {
        using var context = await dbFactory.CreateDbContextAsync();
        return await context.Tickets
            .Include(t => t.Event)
            .ToListAsync();
    }

    public async Task<TicketStatus> ScanTicket(Guid TicketId, int EventId)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        Ticket? tuc = await context.Tickets.FirstOrDefaultAsync(x => x.Id == TicketId);

        if (tuc == null || tuc.Eventid != EventId)
        {
            return TicketStatus.Unrecognized;
        }
        else if (tuc.Isscanned == true)
        {
            return TicketStatus.Used;
        }

        tuc.Isscanned = true;
        tuc.Lastupdated = DateTime.UtcNow;
        context.Tickets.Update(tuc);
        await context.SaveChangesAsync();

        TicketsHaveChanged?.Invoke(this, new EventArgs());

        return TicketStatus.Success;
    }

    public async Task<Ticket> AddTicket(Ticket ticket)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();

        TicketsHaveChanged?.Invoke(this, new EventArgs());

        return ticket;
    }

    public async Task<Ticket> UpdateTicket(Ticket ticket)
    {
        using var context = await dbFactory.CreateDbContextAsync();

        var tuc = await context.Tickets.Where(x => x.Id == ticket.Id).FirstOrDefaultAsync();

        if (tuc is null || (ticket.Isscanned == true && tuc.Isscanned == true))
        {
            throw new AlreadyScannedTicketException("Ticket already scanned");
        }

        tuc.Lastupdated = ticket.Lastupdated.ToUniversalTime();
        tuc.Isscanned = ticket.Isscanned;

        context.Update(tuc);
        try
        {
            await context.SaveChangesAsync();

        }
        catch
        {
            throw new Exception();
        }

        return tuc;
    }
}
