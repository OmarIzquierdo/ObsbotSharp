using ObsbotSharp.Domain.Tail.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tail;

public interface ITailSeries
{
    Task SelectAIModeAsync(TailAirAiMode tailAirAiMode);
    Task SelectAIModeAsync(Tail2AiMode tailAirAiMode);
    Task SelectTrackingSpeed(TailAirTrackingSpeed tailAirTrackingSpeed);
    Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed);
    Task SetPanTrackingSpeed(PanAxis panAxis);
    Task SetPanAxisLock(TiltAxis tiltAxis);
    Task SetTiltAxisLock(int speed);
    Task StartRecordingAsync();
    Task StopRecordingAsync();
    Task TakeScreenshotAsync();
    Task SetTriggerPreset(TriggerPreset triggerPreset);
}