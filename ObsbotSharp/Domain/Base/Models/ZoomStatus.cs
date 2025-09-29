using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Represents the response returned by the general zoom information message.
/// </summary>
/// <param name="ZoomPercent">Zoom position in the 0-100 range.</param>
/// <param name="FovPreset">Field of view preset (0: 86°, 1: 78°, 2: 65°).</param>
public record ZoomStatus(int ZoomPercent, int FovPreset) : IOscParsable<ZoomStatus>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/ZoomInfo"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="ZoomStatus"/> instance.
    /// </summary>
    /// <param name="message">OSC message that contains the zoom information.</param>
    /// <returns>A <see cref="ZoomStatus"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static ZoomStatus Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 2)
            throw new FormatException($"ZoomInfo expected 2 arguments, but received {message.Arguments.Count()}.");

        return new ZoomStatus(
            ZoomPercent: Convert.ToInt32(message.Arguments.ElementAt(0)),
            FovPreset:   Convert.ToInt32(message.Arguments.ElementAt(1)));
    }
}