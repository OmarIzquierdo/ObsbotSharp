namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Describes the state of the mirror command available in the general control group.
/// </summary>
public enum MirrorState
{
    /// <summary>Mirror mode disabled.</summary>
    Deactivated,
    /// <summary>Mirror mode enabled.</summary>
    Activated
}