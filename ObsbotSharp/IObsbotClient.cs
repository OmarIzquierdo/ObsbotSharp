using ObsbotSharp.Domain.Base;
using ObsbotSharp.Domain.Meet;
using ObsbotSharp.Domain.Tail;
using ObsbotSharp.Domain.Tiny;

namespace ObsbotSharp;

/// <summary>
/// Provides strongly-typed access to the different OSC command sets exposed by OBSBOT devices.
/// </summary>
/// <remarks>
/// Each property exposes a façade tailored to a specific product line (Base, Tiny, Tail or Meet)
/// and sends messages following the official OBSBOT OSC specification. Implementations are
/// expected to manage the underlying transport lifecycle and ensure that messages are delivered to
/// the selected device.
/// </remarks>
public interface IObsbotClient : IDisposable
{
    /// <summary>
    /// Gets the command gateway that contains all shared webcam controls that are available across
    /// OBSBOT product families.
    /// </summary>
    IBaseSeries Base { get; }

    /// <summary>
    /// Gets the command gateway that targets the Tiny series specific OSC commands, including AI
    /// tracking, presets and focus related actions.
    /// </summary>
    ITinySeries Tiny { get; }

    /// <summary>
    /// Gets the command gateway that targets the Tail series OSC commands, including AI modes, axis
    /// locks and recording actions.
    /// </summary>
    ITailSeries Tail { get; }

    /// <summary>
    /// Gets the command gateway that targets the Meet series OSC commands for virtual background
    /// and auto-framing features.
    /// </summary>
    IMeetSeries Meet { get; }
}