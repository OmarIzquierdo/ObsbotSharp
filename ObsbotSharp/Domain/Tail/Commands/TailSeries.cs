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

    public Task SelectAIModeAsync(TailAirAiMode tailAirAiMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiMode ]
        );

    public Task SelectAIModeAsync(Tail2AiMode tailAirAiMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetAiMode",
            args: [ (int)tailAirAiMode ]
        );

    public Task SelectTrackingSpeed(TailAirTrackingSpeed tailAirTrackingSpeed) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeed ]
        );

    public Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
            args: [ (int)tailAirTrackingSpeed ]
        );

    public Task SetPanTrackingSpeed(PanAxis panAxis) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanTrackingSpeed",
            args: [ (int)panAxis ]
        );

    public Task SetPanAxisLock(TiltAxis tiltAxis) =>
        gateway.SendAsync(
            address: "/OBSBOT/Camera/Tail/SetPanAxisLock",
            args: [ (int)tiltAxis ]
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

    public Task TakeScreenshotAsync() =>
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