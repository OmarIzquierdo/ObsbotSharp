using ObsbotSharp.Domain.Base.Models;
using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tiny.Commands;

internal sealed class TinySeries : ITinySeries
{
    private readonly IObsbotCommandGateway gateway;
    public TinySeries(IObsbotCommandGateway gateway)
    {
        this.gateway = gateway;
    }

    public Task SelectAutoFocusAsync(AutoFocusMode autofocusMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoFocus",
            args: [ (int)autofocusMode ]
        );

    public Task SetManualFocusAsync(int manualFocusValue) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetManualFocus",
            args: [ manualFocusValue ]
        );

    public Task SelectAiTargetModeAsync(AiTargetMode aiTargetMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/ToggleAILock",
            args: [ (int)aiTargetMode ]
        );

    public Task SelectTriggerPresetPositionModeAsync(TriggerPresetMode triggerPresetMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/TriggerPreset",
            args: [ (int)triggerPresetMode ]
        );

    public Task SelectAiModeAsync(AIMode aiMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/SetAiMode",
            args: [ (int)aiMode ]
        );

    public Task SelectTrackingModeAsync(TrackingMode trackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/SetTrackingMode",
            args: [ (int)trackingMode ]
        );

    public Task<AiTrackingStatus> GetAiTrackingStatusAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<AiTrackingStatus>(
            requestAddress: "/OBSBOT/WebCam/Tiny/GetAiTrackingInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<PresetPositionStatus> GetPresetPositionStatusAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<PresetPositionStatus>(
            requestAddress: "/OBSBOT/WebCam/Tiny/GetPresetPositionInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}
