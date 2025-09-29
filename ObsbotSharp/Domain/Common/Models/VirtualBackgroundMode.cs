namespace ObsbotSharp.Domain.Common.Models;

/// <summary>
/// Specifies the virtual background modes exposed by the Meet virtual background command.
/// </summary>
public enum VirtualBackgroundMode
{
    /// <summary>Disable the virtual background.</summary>
    Disable,
    /// <summary>Enable background blur.</summary>
    Blur,
    /// <summary>Enable green/blue screen mode.</summary>
    GreenScreen,
    /// <summary>Enable background replacement.</summary>
    Replacement
}