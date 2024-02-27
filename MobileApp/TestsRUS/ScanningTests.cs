using FluentAssertions;
using Newtonsoft.Json;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Data;
using TicketClassLib.Enums;
using TicketClassLib.Requests;

namespace TestsRUS;
public class ScanningTests : IClassFixture<TicketsApiFactory>
{
    HttpClient client;
    public ScanningTests(TicketsApiFactory factory)
    {
        client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task AddingTicket()
    {
        //Arrange
        var newTicketRequest = new AddTicketRequest()
        {
            EventId = 1,
            UserName = "Tests"
        };

        //Act
        var response = await client.PostAsJsonAsync($"Ticket/newTicket", newTicketRequest);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();

        //Assert
        ticket.Should().NotBeNull();
    }

    [Fact]
    public async Task ScanningATicket()
    {
        //Arrange
        var newTicketRequest = new AddTicketRequest()
        {
            EventId = 1,
            UserName = "Tests"
        };
        var response = await client.PostAsJsonAsync($"Ticket/newTicket", newTicketRequest);
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();
        var ticketId = ticket.Id.ToString(); 

        //Act
        var scanResponse = await client.PatchAsync($"ticket/scanticket/{ticketId}/{1}", null); 
        var scanTicket = await scanResponse.Content.ReadFromJsonAsync<TicketStatus>(); 

        //Assert
        scanTicket.Should().Be(TicketStatus.Success); 
    }

    [Fact]
    public async Task ScanningAnAlreadyScannedTicket()
    {
        //Arrange
        var newTicketRequest = new AddTicketRequest()
        {
            EventId = 1,
            UserName = "Tests"
        };
        var response = await client.PostAsJsonAsync($"Ticket/newTicket", newTicketRequest); //Create a new ticket
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>(); //Read the ticket
        var ticketId = ticket.Id.ToString(); //Get the ticket id
        var scanResponse = await client.PatchAsync($"ticket/scanticket/{ticketId}/{1}", null); //Scan the ticket
        var scanTicket = await scanResponse.Content.ReadFromJsonAsync<TicketStatus>(); //Read the ticket status


        //Act
        var scanResponse2 = await client.PatchAsync($"ticket/scanticket/{ticketId}/{1}", null); //Scan the ticket again
        var scanTicket2 = await scanResponse2.Content.ReadFromJsonAsync<TicketStatus>(); //Read the ticket status

        //Assert
        scanTicket.Should().Be(TicketStatus.Success); //The first scan should be successful
        scanTicket2.Should().Be(TicketStatus.Failed); //The second scan should fail
    }


    // Scanning a ticket that has already been scanned should return a failed status.
    [Fact]
    public async Task Scanning_Already_Scanned_ticket_should_return_failed_status()
    {
        // Arrange
        var newTicketRequest = new AddTicketRequest()
        {
            EventId = 1,
            UserName = "Tests"
        };
        var response = await client.PostAsJsonAsync($"Ticket/newTicket", newTicketRequest); //Create a new ticket
        var ticket = await response.Content.ReadFromJsonAsync<Ticket>(); //Read the ticket
        var ticketId = ticket.Id.ToString(); //Get the ticket id
        var scanResponse = await client.PatchAsync($"ticket/scanticket/{ticketId}/{1}", null); //Scan the ticket

        // Act
        var scanResponse2 = await client.PatchAsync($"ticket/scanticket/{ticketId}/{1}", null); //Scan the ticket again
        var scanTicket2 = await scanResponse2.Content.ReadFromJsonAsync<TicketStatus>(); //Read the ticket status

        // Assert
        scanTicket2.Should().Be(TicketStatus.Failed); //The second scan should fail
    }

}
