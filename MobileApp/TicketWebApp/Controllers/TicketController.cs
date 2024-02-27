using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using TicketClassLib.Data;
using TicketClassLib.Enums;
using TicketClassLib.Requests;
using TicketClassLib.Services;

namespace TicketWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    [HttpGet("getAll")]
    public async Task<List<Ticket>> GetAll()
    {
        var tickets = await ticketService.GetAll();
        return tickets;
    }

    [HttpPatch("scanTicket/{ticketGuid}/{EventId}")]
    public async Task<TicketStatus> ScanTicket(string ticketGuid, int EventId)
    {
        var newTicketGuid = new Guid(ticketGuid);
        var ticketStatus = await ticketService.ScanTicket(newTicketGuid, EventId);
        return ticketStatus;
    }

    [HttpPost("newTicket")]
    public async Task<Ticket> CreateNewTicket( [FromBody] AddTicketRequest ticketRequest)
    {
        var newTicket = await ticketService.CreateNewTicket(ticketRequest); 
        return newTicket;
    }

    [HttpPatch("updateTicket")]
    public async Task<Ticket> UpdateTicket([FromBody] Ticket ticket)
    {
        var updatedTicket = await ticketService.UpdateTicket(ticket);
        return updatedTicket;
    }
}
