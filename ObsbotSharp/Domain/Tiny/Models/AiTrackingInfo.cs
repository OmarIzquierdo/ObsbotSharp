using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Tiny.Models;

/// <summary>
/// Represents the AI tracking state returned by the Tiny AI tracking response message.
/// </summary>
/// <param name="AiTrackingState">Tracking state reported by the device.</param>
public record AiTrackingInfo(AiTrackingState AiTrackingState) : IOscParsable<AiTrackingInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Tiny/AiTrackingInfo",
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="AiTrackingInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message containing the AI tracking information.</param>
    /// <returns>A <see cref="AiTrackingInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static AiTrackingInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any())
            throw new FormatException("AiTrackingInfo espera 1 arg.");

        return new AiTrackingInfo(
            AiTrackingState: (AiTrackingState)message.Arguments.ElementAt(0)
        );
    }
}

/// <summary>
/// Possible AI tracking states reported by Tiny series devices.
/// </summary>
public enum AiTrackingState
{
    /// <summary>AI tracking is currently unlocked.</summary>
    Unlock,
    /// <summary>AI tracking is currently locked.</summary>
    Lock
}