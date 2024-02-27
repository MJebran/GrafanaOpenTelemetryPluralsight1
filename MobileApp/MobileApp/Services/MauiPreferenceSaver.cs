using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketClassLib.Services;
using Microsoft.Maui.Storage;
using MobileApp.Inits;

namespace MobileApp.Services;

public class MauiPreferenceSaver : IPreferenceSaver
{
    private LocalDbInit localDbInit;
    public bool IsOnline { get => Preferences.Default.Get("isOnline", false); }
    public string ApiAddress { get => Preferences.Default.Get("apiAddress", "https://localhost:7283"); }
    public int RefereshRate { get => Preferences.Default.Get("refreshRate", 60); }

    public MauiPreferenceSaver(LocalDbInit dbInit)
    {
        localDbInit = dbInit;
    }

    public async Task DeleteTables()
    {
        await localDbInit.EmptyDatabase();
    }

    public void SetPreferences(string apiAddress, bool isOnline, int refresh)
    {
        Preferences.Default.Set("isOnline", isOnline);
        Preferences.Default.Set("apiAddress", apiAddress);
        Preferences.Default.Set("refreshRate", refresh);
    }
}
