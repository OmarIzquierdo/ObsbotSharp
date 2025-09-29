using ObsbotSharp.Domain.Base.Models;
using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Meet.Models;

namespace ObsbotSharp.Domain.Meet.Commands;

internal sealed class MeetSeries : IMeetSeries
{
    private readonly IObsbotCommandGateway gateway;
    public MeetSeries(IObsbotCommandGateway gateway)
    {
        this.gateway = gateway;
    }

    public Task SetAutoFocusAsync(AutoFocusMode autofocusMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoFocus",
            args: [ (int)autofocusMode ]
        );

    public Task SetManualFocusAsync(int manualFocusValue) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetManualFocus",
            args: [ manualFocusValue ]
        );

    public Task SetVirtualBackgroundAsync(VirtualBackgroundMode virtualBackground) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetVirtualBackground",
            args: [ (int)virtualBackground ]
        );

    public Task SetAutoFramingAsync(AutoFramingMode autoFramingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetAutoFraming",
            args: [ (int)autoFramingMode ]
        );

    public Task SetStandardModeAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Meet/SetStandardMode",
            args: []
        );

    public Task<VirtualBackgroundStatus> GetVirtualBackgroundInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<VirtualBackgroundStatus>(
            requestAddress: "/OBSBOT/WebCam/Meet/GetVirtualBackgroundInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<AutoFramingStatus> GetAutoFramingInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<AutoFramingStatus>(
            requestAddress: "/OBSBOT/WebCam/Meet/GetAutoFramingInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}