namespace ObsbotSharp.Domain.Tiny.Models;

/// <summary>
/// Describes the locking behavior of Tiny AI tracking when toggling the AI lock command.
/// </summary>
public enum AITargetState
{
    /// <summary>Lock the current target.</summary>
    TargetLock,
    /// <summary>Unlock the tracked target.</summary>
    TargetUnlock
}