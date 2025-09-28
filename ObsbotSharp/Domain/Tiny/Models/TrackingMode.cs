namespace ObsbotSharp.Domain.Tiny.Models;

/// <summary>
/// Specifies the Tiny framing behavior for the tracking mode command.
/// </summary>
public enum TrackingMode
{
    /// <summary>Headroom framing mode.</summary>
    Headroom,
    /// <summary>Standard framing mode.</summary>
    Standard,
    /// <summary>Motion framing mode.</summary>
    Motion
}