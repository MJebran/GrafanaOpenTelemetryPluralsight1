using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Components;
using TicketClassLib.Data;
using TicketClassLib.Enums;
using TicketClassLib.Requests;

namespace TicketClassLib.Services;

public interface ITicketService
{
    public Task<List<Ticket>> GetAll();
    public Task<TicketStatus> ScanTicket(Guid TicketId, int EventId);
    public Task<Ticket> CreateNewTicket(AddTicketRequest newRequest);
    public Task<Ticket> AddTicket(Ticket ticket);
    public Task<Ticket> UpdateTicket(Ticket ticket);

    public event EventHandler TicketsHaveChanged;
}
