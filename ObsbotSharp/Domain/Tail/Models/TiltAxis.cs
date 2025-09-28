namespace ObsbotSharp.Domain.Tail.Models;

/// <summary>
/// Indicates whether the tilt axis should be locked or unlocked when issuing the tilt axis lock
/// command.
/// </summary>
public enum TiltAxis
{
    /// <summary>Unlock the tilt axis.</summary>
    Unlock,
    /// <summary>Lock the tilt axis.</summary>
    Lock
}