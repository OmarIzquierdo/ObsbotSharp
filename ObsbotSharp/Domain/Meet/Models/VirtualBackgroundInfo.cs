using CoreOSC;
using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Meet.Models;

/// <summary>
/// Represents the virtual background state returned by the Meet virtual background response
/// message.
/// </summary>
/// <param name="VirtualBackgroundState">Virtual background mode reported by the device.</param>
public record VirtualBackgroundInfo(VirtualBackgroundState VirtualBackgroundState) : IOscParsable<VirtualBackgroundInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Meet/VirtualBackgroundInfo"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="VirtualBackgroundInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message containing the virtual background information.</param>
    /// <returns>A <see cref="VirtualBackgroundInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static VirtualBackgroundInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any())
            throw new FormatException("VirtualBackgroundInfo espera 1 arg.");

        return new VirtualBackgroundInfo(
            VirtualBackgroundState: (VirtualBackgroundState)message.Arguments.ElementAt(0)
        );
    }
}

