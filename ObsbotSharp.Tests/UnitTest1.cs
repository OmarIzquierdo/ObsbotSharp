using ObsbotSharp.Tests.Models.General;
using ObsbotSharp.Tests.Models.TinySeries;

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

        // await obsbotClient.General.SetMirror(MirrorState.Activated);
        // await obsbotClient.General.MoveCamaraToRight(10);
        // await Task.Delay(1000);
        // await obsbotClient.General.MoveCamaraToRight(0);

        //await obsbotClient.General.SetZoomAsync(0);
        //await obsbotClient.General.SetExposureCompensateAsync(ExposureCompensation.EVm3_0);

        await obsbotClient.Tiny.MoveCamaraToRightAsync(144);
        await Task.Delay(2000);
        await obsbotClient.Tiny.MoveCamaraToRightAsync(0);
        // ZoomInfo zoomInfo = await obsbotClient.Tiny.GetZoomInfoAsync();
        // DeviceInfo deviceInfo = await obsbotClient.Tiny.GeDeviceInfoAsync();
        // await obsbotClient.Tiny.SetMirrorAsync(MirrorState.Deactivated);
        var x = 0;


    }
}