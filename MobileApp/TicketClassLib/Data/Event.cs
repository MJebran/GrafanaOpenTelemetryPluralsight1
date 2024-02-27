using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TicketClassLib.Data;

namespace TicketClassLib.Data;

public partial class Event
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; } = null!;

    [NotNull]
    public DateTime Lastupdated { get; set; }

    [NotNull]
    public DateTime Eventdate { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
