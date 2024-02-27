using MobileApp.Inits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Data;
using TicketClassLib.Enums;
using TicketClassLib.Requests;
using TicketClassLib.Services;
using SQLiteNetExtensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace MobileApp.Services;

public class MobileTicketService : ITicketService
{
    public event EventHandler TicketsHaveChanged;
    public LocalDbInit init { get; set; }

    public MobileTicketService(LocalDbInit init)
    {
        Task.Run(() => init.InitializeLocalDatabase()).Wait();
        this.init = init;
  
    }

    public async Task<Ticket> AddTicket(Ticket ticket)
    {
        var context = init.EstablishConnection();

        await context.InsertOrReplaceAsync(ticket);
        await context.CloseAsync();

        TicketsHaveChanged?.Invoke(this, new EventArgs());
        return ticket;

    }

    public async Task<Ticket> CreateNewTicket(AddTicketRequest newRequest)
    {
        var context = init.EstablishConnection();

        Ticket ticket = new Ticket()
        {
            Eventid = newRequest.EventId,
            Name = newRequest.UserName
        };

        await context.InsertOrReplaceAsync(ticket);
        await context.CloseAsync();

        TicketsHaveChanged?.Invoke(this, new EventArgs());
        return ticket;
    }

    public async Task<List<Ticket>> GetAll()
    {
        var context = init.EstablishConnection();

       
            var tickets = await context.GetAllWithChildrenAsync<Ticket>();
            await context.CloseAsync();
            return tickets;

      
    }

    public async Task<TicketStatus> ScanTicket(Guid TicketId, int EventId)
    {
        var context = init.EstablishConnection();

        var tuc = await context.Table<Ticket>().FirstOrDefaultAsync(t => t.Id == TicketId);

        if(tuc == null || tuc.Eventid != EventId)
        {
            return TicketStatus.Unrecognized;
        }
        else if (tuc?.Isscanned ?? true)
        {
            return TicketStatus.Used;
        }

        tuc.Isscanned = true;
        tuc.Lastupdated = DateTime.UtcNow;
        await context.UpdateAsync(tuc);
        await context.CloseAsync();

        //DONT DO IT, PANDORA
        TicketsHaveChanged?.Invoke(this, new EventArgs());
        return TicketStatus.Success;
    }

    public Task<Ticket> UpdateTicket(Ticket ticket)
    {
        throw new NotImplementedException();
    }
}
