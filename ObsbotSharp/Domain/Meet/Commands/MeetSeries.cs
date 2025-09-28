using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.General.Models;
using ObsbotSharp.Domain.Meet.Models;

namespace ObsbotSharp.Domain.Meet.Commands;

internal sealed class MeetSeries : IMeetSeries
{
    private readonly IObsbotCommandGateway gateway;
    public MeetSeries(IObsbotCommandGateway gateway)
    {
        this.gateway = gateway;
    }

    public Task SetAutoFocusAsync(AutoFocusType autofocusType) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoFocus",
            args: [ (int)autofocusType ]
        );

    public Task SetManualFocusAsync(int manualFocusValue) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetManualFocus",
            args: [ manualFocusValue ]
        );

    public Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetVirtualBackground",
            args: [ (int)virtualBackground ]
        );

    public Task SetAutoFramingAsync(AutoFramingState autoFramingState) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetAutoFraming",
            args: [ (int)autoFramingState ]
        );

    public Task SetStandardModeAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetStandardMode",
            args: []
        );

    public Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<VirtualBackgroundInfo>(
            requestAddress: "/OBSBOT/WebCam/Meet/GetVirtualBackgroundInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<AutoFramingInfo>(
            requestAddress: "/OBSBOT/WebCam/Meet/GetAutoFramingInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}