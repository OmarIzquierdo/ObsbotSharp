namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Defines the white balance modes exposed by the auto white balance command.
/// </summary>
public enum WhiteBalanceType
{
    /// <summary>Manual white balance configuration.</summary>
    Manual,
    /// <summary>Automatic white balance mode.</summary>
    Auto
}