using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketClassLib.Requests;

public class AddTicketRequest
{
    public string UserName { get; set; }
    public int EventId { get; set; }
}
