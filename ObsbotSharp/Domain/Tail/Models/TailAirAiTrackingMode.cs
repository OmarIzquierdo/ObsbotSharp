namespace ObsbotSharp.Domain.Tail.Models;

/// <summary>
/// Enumerates the AI tracking profiles available on Tail Air devices when invoking the AI mode
/// command.
/// </summary>
public enum TailAirAiTrackingMode
{
    /// <summary>No tracking.</summary>
    NoTacking,
    /// <summary>Standard human tracking.</summary>
    NormalTracking,
    /// <summary>Upper-body tracking.</summary>
    UpperBody,
    /// <summary>Close-up framing.</summary>
    CloseUp,
    /// <summary>Headless mode.</summary>
    Headless,
    /// <summary>Lower-body tracking.</summary>
    LowerBody,
    /// <summary>Animal tracking mode.</summary>
    AnimalTracking
}