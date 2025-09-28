using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Represents the response returned by the general zoom information message.
/// </summary>
/// <param name="ZoomPercent">Zoom position in the 0-100 range.</param>
/// <param name="FovPreset">Field of view preset (0: 86°, 1: 78°, 2: 65°).</param>
public record ZoomInfo(int ZoomPercent, int FovPreset) : IOscParsable<ZoomInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/ZoomInfo"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="ZoomInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message that contains the zoom information.</param>
    /// <returns>A <see cref="ZoomInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static ZoomInfo Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 2)
            throw new FormatException("ZoomInfo espera 2 args.");

        return new ZoomInfo(
            ZoomPercent: Convert.ToInt32(message.Arguments.ElementAt(0)),
            FovPreset:   Convert.ToInt32(message.Arguments.ElementAt(1)));
    }
}