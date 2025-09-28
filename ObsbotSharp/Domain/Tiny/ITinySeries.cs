using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tiny;

/// <summary>
/// Defines Tiny series specific OSC commands provided by the Tiny namespace in the OSC
/// specification.
/// </summary>
public interface ITinySeries
{
    /// <summary>
    /// Enables manual or automatic focus modes.
    /// </summary>
    /// <param name="autofocusType">Focus mode supported by Tiny devices.</param>
    Task SetAutoFocusAsync(AutoFocusType autofocusType);

    /// <summary>
    /// Sets the manual focus position (0-100).
    /// </summary>
    /// <param name="manualFocusValue">Manual focus position between 0 and 100.</param>
    Task SetManualFocusAsync(int manualFocusValue);

    /// <summary>
    /// Locks or unlocks AI tracking.
    /// </summary>
    /// <param name="AITargetState">Desired AI tracking target state.</param>
    Task SelectAITargetStateAsync(AITargetState AITargetState);

    /// <summary>
    /// Recalls one of the Tiny preset positions.
    /// </summary>
    /// <param name="triggerPreset">Preset slot identifier.</param>
    Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset);

    /// <summary>
    /// Switches the Tiny AI mode.
    /// </summary>
    /// <param name="AIMode">AI tracking profile to activate.</param>
    Task SelectAIModeAsync(AIMode AIMode);

    /// <summary>
    /// Sets the Tiny tracking framing mode.
    /// </summary>
    /// <param name="trackingMode">Desired framing mode.</param>
    Task SelectTrackingModeAsync(TrackingMode trackingMode);

    /// <summary>
    /// Retrieves the AI tracking status.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0);

    /// <summary>
    /// Retrieves information about the configured preset positions.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0);
}