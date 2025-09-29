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
    /// <param name="tailAirAiTrackingMode">AI profile supported by Tail Air.</param>
    Task SelectAiModeAsync(TailAirAiTrackingMode tailAirAiTrackingMode);

    /// <summary>
    /// Selects the AI mode for Tail 2/Tail 2s devices.
    /// </summary>
    /// <param name="tailAirAiTrackingMode">AI profile supported by Tail 2 and Tail 2s.</param>
    Task SelectAiModeAsync(Tail2AiTrackingMode tailAirAiTrackingMode);

    /// <summary>
    /// Adjusts the tracking speed for Tail Air devices.
    /// </summary>
    /// <param name="tailAirTrackingSpeedMode">Tracking speed preset.</param>
    Task SelectTrackingSpeedModeAsync(TailAirTrackingSpeedMode tailAirTrackingSpeedMode);

    /// <summary>
    /// Adjusts the tracking speed for Tail 2/Tail 2s devices.
    /// </summary>
    /// <param name="tailAirTrackingSpeedMode">Tracking speed preset.</param>
    Task SelectTrackingSpeedModeAsync(Tail2TrackingSpeedMode tailAirTrackingSpeedMode);

    /// <summary>
    /// Configures the pan tracking speed.
    /// </summary>
    /// <param name="panAxisMode">Axis configuration that includes the desired speed value.</param>
    Task SelectPanTrackingSpeedModeAsync(PanAxisMode panAxisMode);

    /// <summary>
    /// Locks or unlocks the pan axis.
    /// </summary>
    /// <param name="tiltAxisMode">Axis lock command.</param>
    Task SelectPanAxisLockModeAsync(TiltAxisMode tiltAxisMode);

    /// <summary>
    /// Locks or unlocks the tilt axis.
    /// </summary>
    /// <param name="speed">Value 0 (unlock) or 1 (lock).</param>
    Task SetTiltAxisLockAsync(int speed);

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
    Task TakeSnapshotAsync();

    /// <summary>
    /// Recalls one of the Tail preset positions.
    /// </summary>
    /// <param name="triggerPresetMode">Preset slot identifier.</param>
    Task SelectTriggerPresetModeAsync(TriggerPresetMode triggerPresetMode);
}