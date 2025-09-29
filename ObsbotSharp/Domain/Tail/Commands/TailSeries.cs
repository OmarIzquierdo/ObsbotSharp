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

    public Task SelectAIModeAsync(TailAirAiTrackingMode tailAirAiTrackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiTrackingMode ]
        );

    public Task SelectAIModeAsync(Tail2AiTrackingMode tailAirAiTrackingMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiTrackingMode ]
        );

    public Task SelectTrackingSpeed(TailAirTrackingSpeedMode tailAirTrackingSpeedMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeedMode ]
        );

    public Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeed ]
        );

    public Task SelectPanTrackingSpeedMode(PanAxisMode panAxisMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanTrackingSpeed",
            args: [ (int)panAxisMode ]
        );

    public Task SelectPanAxisLockMode(TiltAxisMode tiltAxisMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanAxisLock",
            args: [ (int)tiltAxisMode ]
        );

    public Task SetTiltAxisLock(int speed) =>
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

    public Task SetTriggerPreset(TriggerPreset triggerPreset) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/TriggerPreset",
            args: [ (int)triggerPreset ]
        );
}