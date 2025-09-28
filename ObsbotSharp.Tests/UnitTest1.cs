namespace ObsbotSharp.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        ObsbotOptions obsbotOptions = new ObsbotOptions()
            .WithHost("26.143.174.43")
            .WithRemotePort(16284)
            .WithLocalPort(10000);
        
        ObsbotClient obsbotClient = new ObsbotClient(obsbotOptions);
        await obsbotClient.General.SetZoomAsync(0);
    }
}