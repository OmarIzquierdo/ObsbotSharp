using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Meet.Models;

namespace ObsbotSharp.Domain.Meet;

/// <summary>
/// Defines Meet series OSC commands covered by the dedicated Meet namespace in the OSC
/// specification.
/// </summary>
public interface IMeetSeries
{
    /// <summary>
    /// Enables manual or automatic focus modes.
    /// </summary>
    /// <param name="autofocusType">Focus mode supported by Meet devices.</param>
    Task SetAutoFocusAsync(AutoFocusType autofocusType);

    /// <summary>
    /// Sets the manual focus position (0-100).
    /// </summary>
    /// <param name="manualFocusValue">Manual focus position between 0 and 100.</param>
    Task SetManualFocusAsync(int manualFocusValue);

    /// <summary>
    /// Configures the virtual background mode.
    /// </summary>
    /// <param name="virtualBackground">Virtual background state (disable, blur, green screen, replacement).</param>
    Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground);

    /// <summary>
    /// Configures auto-framing.
    /// </summary>
    /// <param name="autoFramingState">Auto-framing mode (disable, single, group).</param>
    Task SetAutoFramingAsync(AutoFramingState autoFramingState);

    /// <summary>
    /// Resets the Meet camera to standard mode.
    /// </summary>
    Task SetStandardModeAsync();

    /// <summary>
    /// Retrieves the current virtual background state.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0);

    /// <summary>
    /// Retrieves the current auto-framing state.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0);
}