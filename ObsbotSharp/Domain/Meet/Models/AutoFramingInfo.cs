using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Meet.Models;
/// <summary>
/// Represents the auto-framing state returned by the Meet auto-framing response message.
/// </summary>
/// <param name="AutoFramingState">Auto-framing mode reported by the device.</param>
public record AutoFramingInfo(AutoFramingState AutoFramingState) : IOscParsable<AutoFramingInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Meet/AutoFramingInfo"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="AutoFramingInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message containing the auto-framing information.</param>
    /// <returns>A <see cref="AutoFramingInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static AutoFramingInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any())
            throw new FormatException($"AutoFramingInfo expected 1 argument, but received {message.Arguments.Count()}.");

        return new AutoFramingInfo(
            AutoFramingState: (AutoFramingState)message.Arguments.ElementAt(0)
        );
    }
}

/// <summary>
/// Describes the available auto-framing modes on Meet series devices.
/// </summary>
public enum AutoFramingState
{
    /// <summary>Disable auto-framing.</summary>
    Disable,
    /// <summary>Single-person auto-framing mode.</summary>
    SingleMode,
    /// <summary>Group auto-framing mode.</summary>
    GroupMode
}