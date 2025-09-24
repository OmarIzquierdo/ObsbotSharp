namespace ObsbotSharp.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        ObsbotOptions obsbotOptions = new ObsbotOptions()
            .WithHost("26.143.174.43")
            .WithPort(16284);
        ObsbotClient obsbotClient = new ObsbotClient(obsbotOptions);
        await obsbotClient.TiltUpAsync(10);
    }
}