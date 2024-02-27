using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TicketClassLib.Exceptions;

public class AlreadyScannedTicketException : Exception
{
    public AlreadyScannedTicketException()
    {
    }

    public AlreadyScannedTicketException(string? message) : base(message)
    {
    }

    public AlreadyScannedTicketException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AlreadyScannedTicketException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
