using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Tiny.Models;

/// <summary>
/// Represents the preset information returned by the Tiny preset information response message.
/// </summary>
/// <param name="AiTrackingState">Indicates whether the preset is currently locked.</param>
public record PresetPositionInfo(AiTrackingState AiTrackingState) : IOscParsable<PresetPositionInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Tiny/PresetPositionInfo",
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="PresetPositionInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message containing the preset information.</param>
    /// <returns>A <see cref="PresetPositionInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static PresetPositionInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any())
            throw new FormatException("PresetPositionInfo espera 2 args.");

        return new PresetPositionInfo(
            AiTrackingState: (AiTrackingState)message.Arguments.ElementAt(0)
        );
    }
}