using System.Diagnostics;
//namespace 

public class DiagnosticsConfiguration
{
    public static readonly string SourceName1 = "Mustafa_Trace_One";
    public static readonly string SourceName2 = "Mustafa_Trace_Two";
    public static readonly ActivitySource MyActivitySource1 = new(SourceName1);
    public static readonly ActivitySource MyActivitySource2 = new (SourceName2);
}