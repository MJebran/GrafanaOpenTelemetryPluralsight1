using System.Diagnostics.Metrics;

public static class Meters
{
    public static Meter otleMeter = new Meter("OpenTeleApi.Api", "0.0.1");
    public static Counter<int> NumRequestsCounter = otleMeter.CreateCounter<int>("num_hit_get");
}