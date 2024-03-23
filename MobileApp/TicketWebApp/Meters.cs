using System.Diagnostics.Metrics;
using MimeKit.Encodings;

public static class Meters
{
    public static Meter otleMeter = new Meter("OpenTeleApi.Api", "0.0.1");
    public static Counter<int> NumRequestsCounter = otleMeter.CreateCounter<int>("num_hit_get");

    public static string upDownCounterVar = "UpDownCounter";
    public static UpDownCounter<int> upDownCounter = otleMeter.CreateUpDownCounter<int>("UpDownCounter");



}