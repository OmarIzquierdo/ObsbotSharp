namespace ObsbotSharp.Domain.Tail.Models;

/// <summary>
/// Enumerates the AI tracking profiles available on Tail 2/Tail 2s devices when invoking the AI
/// mode command.
/// </summary>
public enum Tail2AiMode
{
    /// <summary>Human tracking - single subject.</summary>
    HumanTrackingSingleMode,
    /// <summary>Human tracking - group mode.</summary>
    HumanTrackingGroupMode,
    /// <summary>Animal tracking - normal distance.</summary>
    AnimalTrackingNormal,
    /// <summary>Animal tracking - close-up.</summary>
    AnimalTrackingCloseUp
}