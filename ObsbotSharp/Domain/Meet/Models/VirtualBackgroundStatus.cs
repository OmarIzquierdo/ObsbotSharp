using CoreOSC;
using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Meet.Models;

/// <summary>
/// Represents the virtual background state returned by the Meet virtual background response
/// message.
/// </summary>
/// <param name="VirtualBackgroundMode">Virtual background mode reported by the device.</param>
public record VirtualBackgroundStatus(VirtualBackgroundMode VirtualBackgroundMode) : IOscParsable<VirtualBackgroundStatus>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Meet/VirtualBackgroundInfo"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="VirtualBackgroundStatus"/> instance.
    /// </summary>
    /// <param name="message">OSC message containing the virtual background information.</param>
    /// <returns>A <see cref="VirtualBackgroundStatus"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static VirtualBackgroundStatus Parse(OscMessage message)
    {
        if (!message.Arguments.Any())
            throw new FormatException($"VirtualBackgroundInfo expected 1 argument, but received {message.Arguments.Count()}.");

        return new VirtualBackgroundStatus(
            VirtualBackgroundMode: (VirtualBackgroundMode)message.Arguments.ElementAt(0)
        );
    }
}

