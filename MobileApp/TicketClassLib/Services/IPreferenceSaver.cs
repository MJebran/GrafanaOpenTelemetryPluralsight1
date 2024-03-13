using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketClassLib.Services;

public interface IPreferenceSaver
{

    public string ApiAddress { get; }
    public bool IsOnline { get; }
    public int RefereshRate { get; }

    public void SetPreferences(string apiAddress, bool isOnline, int refresh);
    public Task DeleteTables();
}
