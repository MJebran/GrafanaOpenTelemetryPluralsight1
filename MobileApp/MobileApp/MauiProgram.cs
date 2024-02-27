using Microsoft.Extensions.Logging;
using MobileApp.Inits;
using MobileApp.Services;
using TicketClassLib.Services;
using ZXing.Net.Maui.Controls;

namespace MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddSingleton<IEventService, MobileEventService>();
            builder.Services.AddSingleton<ITicketService, MobileTicketService>();
            builder.Services.AddScoped<LocalDbInit>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IDbLocation, MauiDbLocation>();
            builder.Services.AddScoped<DataSync>();
            builder.Services.AddScoped<IPreferenceSaver, MauiPreferenceSaver>();


    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
    


            return builder.Build();
        }
    }
}
