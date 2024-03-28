using System.Diagnostics.Metrics;
using MimeKit.Encodings;
using SQLitePCL;
using TicketWebApp.Components.Pages;

public class Meters
{
    //public static Random MJRandom = new Random(); 
    int valueOne = 0;
    int valueTwo = 0;
    int valueThree = 0;

    public static Meter otleMeter = new Meter("OpenTeleApi.Api", "0.0.1");
    public static Counter<int> NumRequestsCounter = otleMeter.CreateCounter<int>("num_hit_get");

    public static Meter Counters = new Meter("Counters", "0.0.1"); 
    public static UpDownCounter<int> upDownCounter = Counters.CreateUpDownCounter<int>("upDownCounter");

    public static ObservableCounter<int> observableCounter = Counters.CreateObservableCounter<int>("observableCounter", () => Home.observableVar);

    public static ObservableUpDownCounter<int> observableUpDownCounter = Counters.CreateObservableUpDownCounter<int>("observableUpDownCounter", () => Home.observableUpDownVar);

    public static ObservableGauge<int> observableGauge = Counters.CreateObservableGauge<int>("observableGauge", () => Home.observableGuageVar);

    public static Histogram<int> histogram = Counters.CreateHistogram<int>("histogram");


    public void CallMetrics()
    {
        NumRequestsCounter?.Add(4);
        upDownCounter?.Add(-5);
        valueOne += 7;
        valueTwo += 10;
        valueThree += 13;
        histogram?.Record(5);
    }



}



// public CarlosMetric(IMeterFactory meterFactory)
// {
//     var meter = meterFactory.Create(MetricName);
//     count = meter.CreateCounter<int>("carlos.randomcounter");
//     upDownCounter = meter.CreateUpDownCounter<int>("carlos.randomupdowncounter");
//     observableCounter = meter.CreateObservableCounter<int>("carlos.randomobservablecounter", () => valueToObserve);
//     observableGauge = meter.CreateObservableGauge<int>("carlos.randomobservablegauge", () => valueForGaugeToObserve);
//     observableUpDownCounter = meter.CreateObservableUpDownCounter<int>("carlos.randomobservableupdowncounter", () => valueForObservableUpDownCounterToObserve);
//     histogram = meter.CreateHistogram<int>("carlos.randomhistogram");
// }
