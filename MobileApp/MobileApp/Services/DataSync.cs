using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Data;
using TicketClassLib.Services;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using TicketClassLib.Exceptions;
using MobileApp.Components.Pages;
using Microsoft.Maui.Controls;

namespace MobileApp.Services;

public class DataSync
{
    HttpClient client = new();
    ITicketService ticketService;
    IEventService eventService;
    string ApiAddress;

    public DataSync(ITicketService ticketService, IEventService eventService)
    {
        this.ticketService = ticketService;
        this.eventService = eventService;
        ApiAddress = Preferences.Default.Get("apiAddress", "https://localhost:7283");
    }

    public async Task DoSync()
    {
        var alertPage = new Alert();
        var ticks = Preferences.Default.Get("refreshRate", 30);
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(ticks));
        while (Preferences.Default.Get("isOnline", false) && await timer.WaitForNextTickAsync()) 
        {
            var newTicks = Preferences.Default.Get("refreshRate", 30);
            timer = new PeriodicTimer(TimeSpan.FromSeconds(newTicks));
            ApiAddress = Preferences.Default.Get("apiAddress", "https://localhost:7283");
            try
            {
                await SyncEvents();
                await SyncTickets();
            }
            catch (AlreadyScannedTicketException ex) 
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(alertPage);
            }
        }
    }


    public async Task SyncTickets()
    {
        var differences = await GetTicketDifferences();
        foreach ( var difference in differences ) 
        { 
            if(difference.Maui is null && difference.Api is not null)
            { 
                var result = await ticketService.AddTicket(difference.Api);
                if (result is null) 
                {
                    throw new Exception("Ticket was unsuccessfully added");
                }
            }
            else if (difference.Api.Lastupdated < difference.Maui.Lastupdated)
            {
                var mauiTicket = difference.Maui;
                var response = await client.PatchAsJsonAsync($"{ApiAddress}/Ticket/updateTicket", mauiTicket);
                if (!response.IsSuccessStatusCode)
                {
                    throw new AlreadyScannedTicketException("A ticket was scanned twice");
                }
            }
            else if(difference.Api is not null)
            {
                var result = await ticketService.AddTicket(difference.Api);
                if (result is null )
                {
                    throw new Exception("Error moving api data to maui");
                }
            }
        }
    }

    public async Task SyncEvents()
    {
        var differences = await GetEventDifferences();
        foreach (var difference in differences)
        {
            if (difference.Maui is null && difference.Api is not null)
            {
                var result = await eventService.AddEvent(difference.Api);
                if (result is null)
                {
                    throw new Exception("Event was unsuccessfully added");
                }
            }
            else
            {
                var result = await eventService.AddEvent(difference.Api);
                if (result is null)
                {
                    throw new Exception("Error moving api data to maui");
                }
            }
        }
    }

    public async Task<IEnumerable<Differences<Ticket>>> GetTicketDifferences()
    {
        List<Ticket> onlineTickets = await client.GetFromJsonAsync<List<Ticket>>($"{ApiAddress}/ticket/getAll");
        List<Ticket> localTickets = await ticketService.GetAll();

        onlineTickets = onlineTickets.OrderBy(x => x.Id).ToList();  
        localTickets = localTickets.OrderBy(x=> x.Id).ToList();

        HashSet<(Guid, DateTime)> localTicketsHash = new HashSet<(Guid, DateTime)>(localTickets.Select(x => (x.Id, x.Lastupdated)));
    
        var onlineChanges = onlineTickets
            .Where(x => !localTicketsHash.Contains((x.Id, x.Lastupdated)))
            .Select(x => new Differences<Ticket>(localTickets.FirstOrDefault(y => y.Id == x.Id), x))
            .ToList();
        return onlineChanges;
    }

    public async Task<IEnumerable<Differences<Event>>> GetEventDifferences()
    {
        List<Event> onlineEvents = await client.GetFromJsonAsync<List<Event>>($"{ApiAddress}/event/getAll");
        List<Event> localEvents = await eventService.GetAll();

        onlineEvents = onlineEvents.OrderBy(x => x.Id).ToList();
        localEvents = localEvents.OrderBy(x => x.Id).ToList();

        HashSet<(int, DateTime)> localEventsHash = new HashSet<(int, DateTime)>(localEvents.Select(x => (x.Id, x.Lastupdated)));

        var onlineChanges = onlineEvents
            .Where(x => !localEventsHash.Contains((x.Id, x.Lastupdated)))
            .Select(x => new Differences<Event>(localEvents.FirstOrDefault(y => y.Id == x.Id), x))
            .ToList();
        return onlineChanges;
    }
}

public class Differences<T>
{
    public T Maui { get; set; }
    public T Api { get; set; }

    public Differences(T maui, T api)
    {
        Maui = maui;
        Api = api;
    }
}
