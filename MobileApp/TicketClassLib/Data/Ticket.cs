using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TicketClassLib.Data;

public partial class Ticket
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [ForeignKey(typeof(Event)), NotNull]
    public int Eventid { get; set; }

    [NotNull]
    public string Name { get; set; } = null!;

    [NotNull]
    public DateTime Lastupdated { get; set; }

    [NotNull]
    public bool? Isscanned { get; set; }

    [ManyToOne]
    public virtual Event Event { get; set; } = null!;
}
