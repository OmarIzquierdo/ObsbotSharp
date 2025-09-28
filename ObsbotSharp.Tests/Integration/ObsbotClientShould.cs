using CoreOSC;
using ObsbotSharp.Tests.Seams;

namespace ObsbotSharp.Tests.Integration;

public class ObsbotClientShould
{
    [Fact]
    public async Task MoveCameraToRightAsyncQueuesOscMessage()
    {
        var transport = new OscTransportSeam();
        using var client = new ObsbotClient(transport);

        await client.Base.MoveCamaraToRightAsync(10);

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

        var zoomInfo = await client.Base.GetZoomInfoAsync(deviceIndex: 0);

        Assert.Equal(35, zoomInfo.ZoomPercent);
        Assert.Equal(2, zoomInfo.FovPreset);
    }

    [Fact]
    public async Task MethodsCanBeOverriddenForCustomMocking()
    {
        using var client = new ObsbotClientSeam();

        await client.Base.MoveCamaraToDownAsync(5);

        var invocation = Assert.Single(client.SentMessages);
        Assert.Equal("/OBSBOT/WebCam/General/SetGimbalDown", invocation.Address);
        Assert.Equal([5], invocation.Arguments);
    }

    [Fact]
    public async Task xxx()
    {
        var options = new ObsbotOptions()
            .WithHost("26.143.174.43")
            .WithLocalPort(10000)
            .WithRemotePort(16284);
        using var client = new ObsbotClient(options);
        var x = await client.Base.GetZoomInfoAsync();
    }
}