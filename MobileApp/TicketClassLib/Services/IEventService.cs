using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Data;

namespace TicketClassLib.Services;

public interface IEventService
{
    public Task<List<Event>> GetAll();
    public Task<Event> AddEvent(string name, DateTime date);
    public Task<Event> AddEvent(Event newEvent);
    public Task<Event?> GetEvent(int id);
}
