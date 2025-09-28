namespace ObsbotSharp.Domain.Tiny.Models;

/// <summary>
/// Enumerates the Tiny series AI tracking profiles supported by the AI mode command.
/// </summary>
public enum AIMode
{
    /// <summary>No AI tracking.</summary>
    NoTracking,
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
    /// <summary>Desk mode framing.</summary>
    DeskMode,
    /// <summary>Whiteboard presentation mode.</summary>
    Whiteboard
}