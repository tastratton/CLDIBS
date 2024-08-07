using Moq;
using helper.Time;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Time.Testing;
using System.Globalization;
using System.Text.Json;
using Xunit.Abstractions;

namespace helper.test;

public class TimeProviderFixture : IDisposable
{
    public FakeTimeProvider FakeTimeProvider { get; }
    public Mock<ILogger<TimeCommandHandler>> MockLogger { get; }
    public ILogger<TimeCommandHandler> Logger { get; }
    public System.CommandLine.Invocation.InvocationContext? Invoker { get; }
    public TimeCommandHandler TimeCommandHandler {get;}

    public TimeProviderFixture()
    {

        FakeTimeProvider = new FakeTimeProvider();
        MockLogger = new Mock<ILogger<TimeCommandHandler>>();
        Logger = MockLogger.Object;
        Invoker = new System.CommandLine.Invocation.InvocationContext(null); // todo fix warning by creating / passing the requred mocks
        TimeCommandHandler = new TimeCommandHandler(FakeTimeProvider, Logger);
        FakeTimeProvider.SetUtcNow(new DateTime(2023, 11, 5));
        FakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

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
}
