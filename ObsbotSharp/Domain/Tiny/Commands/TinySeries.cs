using ObsbotSharp.Domain.Base.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tiny.Commands;

internal sealed class TinySeries : ITinySeries
{
    private readonly IObsbotCommandGateway gateway;
    public TinySeries(IObsbotCommandGateway gateway)
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

    public Task SelectAITargetStateAsync(AITargetState AITargetState) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/ToggleAILock",
            args: [ (int)AITargetState ]
        );

    public Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/TriggerPreset",
            args: [ (int)triggerPreset ]
        );

    public Task SelectAIModeAsync(AIMode AIMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/SetAiMode",
            args: [ (int)AIMode ]
        );

    public Task SelectTrackingModeAsync(TrackingMode trackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/Tiny/SetTrackingMode",
            args: [ (int)trackingMode ]
        );

    public Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<AiTrackingInfo>(
            requestAddress: "/OBSBOT/WebCam/Tiny/GetAiTrackingInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<PresetPositionInfo>(
            requestAddress: "/OBSBOT/WebCam/Tiny/GetPresetPositionInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}
