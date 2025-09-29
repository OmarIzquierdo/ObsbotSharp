using ObsbotSharp.Domain.Tail.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tail.Commands;

internal sealed class TailSeries : ITailSeries
{
    private readonly IObsbotCommandGateway gateway;
    public TailSeries(IObsbotCommandGateway gateway)
    {
        this.gateway = gateway;
    }

    public Task SelectAiModeAsync(TailAirAiTrackingMode tailAirAiTrackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiTrackingMode ]
        );

    public Task SelectAiModeAsync(Tail2AiTrackingMode tailAirAiTrackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiTrackingMode ]
        );

    public Task SelectTrackingSpeedModeAsync(TailAirTrackingSpeedMode tailAirTrackingSpeedMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeedMode ]
        );

    public Task SelectTrackingSpeedModeAsync(Tail2TrackingSpeedMode tailAirTrackingSpeedMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeedMode ]
        );

    public Task SelectPanTrackingSpeedModeAsync(PanAxisMode panAxisMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanTrackingSpeed",
            args: [ (int)panAxisMode ]
        );

    public Task SelectPanAxisLockModeAsync(TiltAxisMode tiltAxisMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanAxisLock",
            args: [ (int)tiltAxisMode ]
        );

    public Task SetTiltAxisLockAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTiltAxisLock",
            args: [ speed ]
        );

    public Task StartRecordingAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetRecording",
            args: [ 1 ]
        );

    public Task StopRecordingAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetRecording",
            args: [ 0 ]
        );

    public Task TakeSnapshotAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/Snapshot",
            args: [ 1 ]
        );

    public Task SelectTriggerPresetModeAsync(TriggerPresetMode triggerPresetMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/TriggerPreset",
            args: [ (int)triggerPresetMode ]
        );
}