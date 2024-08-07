//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using System.CommandLine;
using System.CommandLine.Invocation;
//using System.CommandLine.NamingConventionBinder;

namespace helper.Time;
//public partial class TimeCommand
//{
public class TimeCommandHandler : ICommandHandler
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<TimeCommandHandler> _logger;
    public double? TimeZone { get; set; }


    public TimeCommandHandler(TimeProvider timeProvider, ILogger<TimeCommandHandler> logger)
    {
        _timeProvider = timeProvider;
        _logger = logger;

    }

    public int Invoke(InvocationContext context)
    {
        DateTimeOffset currentGmt = _timeProvider.GetUtcNow();
        _logger.LogInformation("Called at {Time}", currentGmt);
        Console.WriteLine($"{currentGmt.ToOffset(TimeSpan.FromHours(TimeZone ?? 0)):T}");

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        return Task.FromResult(Invoke(context));
    }
}

//}

