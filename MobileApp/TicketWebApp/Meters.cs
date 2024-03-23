using System.Diagnostics.Metrics;
using MimeKit.Encodings;
using TicketWebApp.Components.Pages;

public static class Meters
{
    public static Meter otleMeter = new Meter("OpenTeleApi.Api", "0.0.1");
    public static Counter<int> NumRequestsCounter = otleMeter.CreateCounter<int>("num_hit_get");

    public static Meter Counters = new Meter("Counters", "0.0.1"); 
    public static UpDownCounter<int> upDownCounter = Counters.CreateUpDownCounter<int>("upDownCounter");

    public static ObservableCounter<int> observableCounter = Counters.CreateObservableCounter<int>("observableCounter", () => Home.observableVar);

    public static ObservableUpDownCounter<int> observableUpDownCounter = Counters.CreateObservableUpDownCounter<int>("observableUpDownCounter", () => Home.observableUpDownVar);

    public static ObservableGauge<int> observableGauge = Counters.CreateObservableGauge<int>("observableGauge", () => Home.observableGuageVar);

    public static Histogram<int> histogram = Counters.CreateHistogram<int>("histogram"); 



}