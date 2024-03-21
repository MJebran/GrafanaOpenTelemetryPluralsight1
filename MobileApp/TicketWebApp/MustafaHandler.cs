partial class MustafaHandler(ILogger<MustafaHandler> logger)
{
    public string HandleRequest1()
    {
        LogHandleRequest1(logger, "Mustafa-log-request-1");
        return "Hit the end point-1";
    }

    public string HandleRequest2()
    {
        LogHandleRequest2(logger, "TMustafa-log-request-2");
        return "Hit the end point-2";
    }

    public string HandleRequest3()
    {
        LogHandleRequest3(logger, "Mustafa-log-request-3");
        return "Hit the end point-3";
    }

    public string HandleRequest4()
    {
        LogHandleRequest4(logger, "Mustafa-log-request-4");
        return "Hit the end point-4";
    }

    public string HandleRequest5()
    {
        LogHandleRequest5(logger, "Mustafa-log-request-5");
        return "Hit the end point-5";
    }

    [LoggerMessage(LogLevel.Information, "ExampleHandler.HandleRequest1 was called with {message}")]
    public static partial void LogHandleRequest1(ILogger logger, string message);

    [LoggerMessage(LogLevel.Warning, "ExampleHandler.HandleRequest2 was called with {message}")]
    public static partial void LogHandleRequest2(ILogger logger, string message);

    [LoggerMessage(LogLevel.Debug, "ExampleHandler.HandleRequest3 was called with {message}")]
    public static partial void LogHandleRequest3(ILogger logger, string message);

    [LoggerMessage(LogLevel.Information, "ExampleHandler.HandleRequest4 was called with {message}")]
    public static partial void LogHandleRequest4(ILogger logger, string message);

    [LoggerMessage(LogLevel.Information, "ExampleHandler.HandleRequest5 was called with {message}")]
    public static partial void LogHandleRequest5(ILogger logger, string message);
}