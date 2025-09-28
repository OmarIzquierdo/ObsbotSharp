using CoreOSC;
using ObsbotSharp.Seams;

namespace ObsbotSharp.Tests.Integration;

public class ObsbotClientShould
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
    
    [Fact]
    public async Task MoveCameraToRightAsyncQueuesOscMessage()
    {
        var transport = new OscTransportSeam();
        using var client = new ObsbotClient(transport);

        await client.General.MoveCamaraToRightAsync(10);

        var message = Assert.Single(transport.SentMessages);
        Assert.Equal("/OBSBOT/WebCam/General/SetGimbalRight", message.Address);
        Assert.Equal([ 10 ], message.Arguments);
    }

    [Fact]
    public async Task GetZoomInfoAsyncParsesResponseFromTransport()
    {
        var transport = new OscTransportSeam();
        using var client = new ObsbotClient(transport);

        transport.EnqueueResponse(
            new OscMessage(
                new Address("/OBSBOT/WebCam/General/ZoomInfo"),
                [35, 2]));

        var zoomInfo = await client.General.GetZoomInfoAsync(deviceIndex: 0);

        Assert.Equal(35, zoomInfo.ZoomPercent);
        Assert.Equal(2, zoomInfo.FovPreset);
    }

    [Fact]
    public async Task MethodsCanBeOverriddenForCustomMocking()
    {
        using var client = new ObsbotClientSeam();

        await client.General.MoveCamaraToDownAsync(5);

        var invocation = Assert.Single(client.SentMessages);
        Assert.Equal("/OBSBOT/WebCam/General/SetGimbalDown", invocation.Address);
        Assert.Equal([5], invocation.Arguments);
    }
}