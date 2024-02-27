using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Services;

namespace MobileApp.Services;

public class MauiDbLocation : IDbLocation
{
    public string DbName { get => "Ticket.db"; }
    public string BaseDataDirectory { get => FileSystem.Current.AppDataDirectory; }
}
