using ObsbotSharp.Domain.Tail.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tail;

/// <summary>
/// Defines Tail series OSC commands provided by the Tail camera namespace within the OSC
/// specification.
/// </summary>
public interface ITailSeries
{
    /// <summary>
    /// Selects the AI mode for Tail Air devices.
    /// </summary>
    /// <param name="tailAirAiMode">AI profile supported by Tail Air.</param>
    Task SelectAIModeAsync(TailAirAiMode tailAirAiMode);

    /// <summary>
    /// Selects the AI mode for Tail 2/Tail 2s devices.
    /// </summary>
    /// <param name="tailAirAiMode">AI profile supported by Tail 2 and Tail 2s.</param>
    Task SelectAIModeAsync(Tail2AiMode tailAirAiMode);

    /// <summary>
    /// Adjusts the tracking speed for Tail Air devices.
    /// </summary>
    /// <param name="tailAirTrackingSpeed">Tracking speed preset.</param>
    Task SelectTrackingSpeed(TailAirTrackingSpeed tailAirTrackingSpeed);

    /// <summary>
    /// Adjusts the tracking speed for Tail 2/Tail 2s devices.
    /// </summary>
    /// <param name="tailAirTrackingSpeed">Tracking speed preset.</param>
    Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed);

    /// <summary>
    /// Configures the pan tracking speed.
    /// </summary>
    /// <param name="panAxis">Axis configuration that includes the desired speed value.</param>
    Task SetPanTrackingSpeed(PanAxis panAxis);

    /// <summary>
    /// Locks or unlocks the pan axis.
    /// </summary>
    /// <param name="tiltAxis">Axis lock command.</param>
    Task SetPanAxisLock(TiltAxis tiltAxis);

    /// <summary>
    /// Locks or unlocks the tilt axis.
    /// </summary>
    /// <param name="speed">Value 0 (unlock) or 1 (lock).</param>
    Task SetTiltAxisLock(int speed);

    /// <summary>
    /// Starts on-device recording.
    /// </summary>
    Task StartRecordingAsync();

    /// <summary>
    /// Stops on-device recording.
    /// </summary>
    Task StopRecordingAsync();

    /// <summary>
    /// Captures a snapshot from the active Tail device.
    /// </summary>
    Task TakeScreenshotAsync();

    /// <summary>
    /// Recalls one of the Tail preset positions.
    /// </summary>
    /// <param name="triggerPreset">Preset slot identifier.</param>
    Task SetTriggerPreset(TriggerPreset triggerPreset);
}