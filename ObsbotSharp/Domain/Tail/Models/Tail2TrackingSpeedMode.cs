namespace ObsbotSharp.Domain.Tail.Models;

/// <summary>
/// Specifies the Tail 2/Tail 2s tracking speed presets recognised by the tracking speed command.
/// </summary>
public enum Tail2TrackingSpeedMode
{
    /// <summary>Super lazy tracking speed.</summary>
    SuperLazy,
    /// <summary>Lazy tracking speed.</summary>
    Lazy,
    /// <summary>Slow tracking speed.</summary>
    Slow,
    /// <summary>Fast tracking speed.</summary>
    Fast,
    /// <summary>Crazy tracking speed.</summary>
    Crazy,
    /// <summary>Automatic speed selection.</summary>
    Auto
}