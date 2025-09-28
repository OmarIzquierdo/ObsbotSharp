namespace ObsbotSharp.Domain.Tail.Models;

/// <summary>
/// Indicates whether the pan axis should be locked or unlocked when issuing the pan axis lock
/// command.
/// </summary>
public enum PanAxis
{
    /// <summary>Unlock the pan axis.</summary>
    Unlock,
    /// <summary>Lock the pan axis.</summary>
    Lock
}