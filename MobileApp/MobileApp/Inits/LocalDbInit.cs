using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Data;
using TicketClassLib.Services;

namespace MobileApp.Inits;

public class LocalDbInit(IDbLocation dbLocation)
{
    private Type[] tables = [typeof(Event), typeof(Ticket)];

    public async Task<SQLiteAsyncConnection> InitializeLocalDatabase()
    {
        var database = EstablishConnection();

        await database.CreateTablesAsync(CreateFlags.None, tables);

        return database;
    }

    public SQLiteAsyncConnection EstablishConnection()
    {
        if (!Directory.Exists(dbLocation.BaseDataDirectory)) Directory.CreateDirectory(dbLocation.BaseDataDirectory);

        var database = new SQLiteAsyncConnection(Path.Combine(dbLocation.BaseDataDirectory, dbLocation.DbName));

        return database;
    }

    public async Task EmptyDatabase()
    {
        var db = EstablishConnection();

        await db.DeleteAllAsync<Ticket>();
        await db.DeleteAllAsync<Event>();
    }
}
