using System.CommandLine;
//using System.CommandLine.NamingConventionBinder;
//using helper.Time;
//using Microsoft.Extensions.Hosting;
//TimeCommand.CommandHandler;

namespace helper.Time;
public class TimeCommand : Command
{
    public TimeCommand()
        : base("time", "Outputs the current time")
    {
        AddOptions(this);
    }
    public static void AddOptions(Command command)
    {
        var timezoneOption = new Option<double?>(
            aliases: new[] { "--timezone", "-z" },
            description: "The positive or negative GMT offset");

        command.AddOption(timezoneOption);
    }

}

