using Moq;
using helper.Time;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Time.Testing;
using System.Globalization;
using System.Text.Json;
using Xunit.Abstractions;
using System;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.CommandLine.Parsing;
using System.CommandLine;
//using System.CommandLine.NamingConventionBinder;

namespace helper.test;

public class TimeProviderFixture : IDisposable
{
    public FakeTimeProvider FakeTimeProvider { get; }
    public Mock<ILogger<TimeCommandHandler>> MockLogger { get; }
    public ILogger<TimeCommandHandler> Logger { get; }
    public System.CommandLine.Invocation.InvocationContext? Invoker { get; }
    public TimeCommandHandler TimeCommandHandler { get; }

    public System.CommandLine.Parsing.ParseResult ParseResult { get; }

    private readonly TimeCommand _timeCommand;
    private readonly Parser _parser;
    private readonly ParseResult _parseResult;



    public TimeProviderFixture()
    {

        FakeTimeProvider = new FakeTimeProvider();
        MockLogger = new Mock<ILogger<TimeCommandHandler>>();
        Logger = MockLogger.Object;
        //_timeCommand = new TimeCommand();
        //_timeCommand.Handler = CommandHandler.Create(() => true);
        //_parser = new Parser(_timeCommand);
        //var wasCalled = false;
        //var command = new Command("command");
        //command.Handler = CommandHandler.Create(() => wasCalled = true);
        //var parser = new Parser(command);

        //await parser.InvokeAsync("command", _console);

        //_parseResult = new ParseResult();

        //var parser = new Parser(TimeCommand);
        //var parseResult = parser.Parse(stringCommand);
        //var invocationContext = new InvocationContext(parseResult, _console);
        var command = new TimeCommand();
        var config = new System.CommandLine.CommandLineConfiguration(command);
        command.Handler = new TimeCommandHandler(FakeTimeProvider);
        ParseResult parseResult = command.Parse("time -z -5");
        TimeCommandHandler = new TimeCommandHandler(FakeTimeProvider, Logger);
        FakeTimeProvider.SetUtcNow(new DateTime(2023, 11, 5));
        FakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));
        Invoker = new System.CommandLine.Invocation.InvocationContext(parseResult); 


    }

    public void Dispose()
{

}
}
public class TimeProvider : IClassFixture<TimeProviderFixture>
{
    private readonly ITestOutputHelper _testOutputHelper;
    TimeProviderFixture fixture;
    public TimeProvider(TimeProviderFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        this.fixture = fixture;
    }

    [Fact]
    public void ExitCode()
    {

        /*
        var mockTimeProvider = new Mock<TimeProvider>();
        mockTimeProvider.Setup(x => x.GetLocalNow()).Returns(new DateTimeOffset(2025, 12, 31, 23, 59, 59, TimeSpan.Zero));
        var mockedTimeProvider = mockTimeProvider.Object;
        */
        // Arrange

        // Act

        // Set the current UTC time
        //fixture.FakeTimeProvider.SetUtcNow(new DateTime(2023, 11, 5));
        //fixture.FakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

        //var fakeTimeProviderAsJson = System.Text.Json.JsonSerializer.Serialize(fakeTimeProvider);  //to get fakeTimeJsonStringLiteral from debug
        //string fakeTimeJsonStringLiteral = """{ "Start":"2023-11-05T00:00:00+00:00","AutoAdvanceAmount":"00:00:00","LocalTimeZone":{ "Id":"Greenwich Standard Time","HasIanaId":false,"DisplayName":"(UTC\u002B00:00) Monrovia, Reykjavik","StandardName":"Greenwich Standard Time","DaylightName":"Greenwich Daylight Time","BaseUtcOffset":"00:00:00","SupportsDaylightSavingTime":false},"TimestampFrequency":10000000}""";
        //var compareFakeTime = System.Text.Json.JsonSerializer.Deserialize<FakeTimeProvider>(fakeTimeJsonStringLiteral);
        //var compareFakeTime =  new FakeTimeProvider();
        //compareFakeTime.SetUtcNow(new DateTime(2023, 11, 5));
        //compareFakeTime.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

        //var fakeTimeGetUtc = fixture.FakeTimeProvider.GetUtcNow();
        //var compareFakeTimeGetUtc = compareFakeTime.GetUtcNow();

        // TODO - restructure code so we are doing an actual test through our code!
        // right now the code except for return code is comparing 2 mock objects that aren't coming from our code


        //Assert        
        var result = fixture.TimeCommandHandler.Invoke(fixture.Invoker);
        /*
        Assert.Multiple(
            () => Assert.True(result == 0, "Exit Code is 0"),
            () => Assert.True(fakeTimeGetUtc.Equals(compareFakeTimeGetUtc)),
            () => Assert.True(fixture.FakeTimeProvider.LocalTimeZone.Equals(compareFakeTime.LocalTimeZone))
        );
        */
        Assert.True(result == 0, "Exit Code is 0");
        _testOutputHelper.WriteLine("Assert Exit Code is 0");

    }
    [Fact]
    public void ExpectedDate()
    {
        var compareFakeTime = new FakeTimeProvider();
        compareFakeTime.SetUtcNow(new DateTime(2023, 11, 5));
        compareFakeTime.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

        var fakeTimeGetUtc = fixture.FakeTimeProvider.GetUtcNow();
        var compareFakeTimeGetUtc = compareFakeTime.GetUtcNow();
        Assert.True(fakeTimeGetUtc.Equals(compareFakeTimeGetUtc));
        _testOutputHelper.WriteLine("Date is 2023-11-05");

    }
    [Fact]
    public void ExpectedTimeZone()
    {
        var compareFakeTime = new FakeTimeProvider();
        compareFakeTime.SetUtcNow(new DateTime(2023, 11, 5));
        compareFakeTime.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

        var fakeTimeGetUtc = fixture.FakeTimeProvider.GetUtcNow();
        var compareFakeTimeGetUtc = compareFakeTime.GetUtcNow();
        Assert.True(fakeTimeGetUtc.Equals(compareFakeTimeGetUtc));
        _testOutputHelper.WriteLine("Local TimeZone is Greenwich Standard Time");

    }

    [Fact]
    public void ConsoleOutput()
    {
        StringWriter testconsole = new StringWriter();
        Console.SetOut(testconsole);

        fixture.TimeCommandHandler.Invoke(fixture.Invoker);
        Assert.Equal("5:00:00 AM\r\n", testconsole.ToString());

    }
    [Fact]
    public void ParseResult()
    {
        StringWriter testconsole = new StringWriter();
        Console.SetOut(testconsole);
        Console.SetError(testconsole);

        var command = new TimeCommand();
        var config = new System.CommandLine.CommandLineConfiguration(command);
        command.Handler = new TimeCommandHandler(fixture.FakeTimeProvider);
        ParseResult parseResult = command.Parse("time -z -4");
        Assert.NotNull(parseResult);
/*
        int result = command.Parse("time -z -4").Invoke();
        _testOutputHelper.WriteLine(boundParseResult.ToString());
        //var boundParseResult.GetValueForArgument(new Argument<Int>());
        //boundParseResult.ValueForOption(option).Should().Be(123);
        Assert.Equal(1, 1);
*/
    }

}
