using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Represents the gimbal position returned by the general gimbal position response message.
/// </summary>
/// <param name="Roll">Roll axis value (not used currently).</param>
/// <param name="Pitch">Pitch angle (-90° to 90°).</param>
/// <param name="Yaw">Yaw angle (-140° to 140°).</param>
public record GimbalPosInfo(int Roll, int Pitch, int Yaw) : IOscParsable<GimbalPosInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/GetGimbalPosInfoResp"
    ];

    /// <summary>
    /// Parses the OSC message payload into a <see cref="GimbalPosInfo"/> instance.
    /// </summary>
    /// <param name="message">OSC message that contains the gimbal position.</param>
    /// <returns>A <see cref="GimbalPosInfo"/> representation.</returns>
    /// <exception cref="FormatException">Thrown when the message does not contain the expected arguments.</exception>
    public static GimbalPosInfo Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 3)
            throw new FormatException("GimbalPosInfo espera 3 args.");

        return new GimbalPosInfo(
            Roll:  Convert.ToInt32(message.Arguments.ElementAt(0)),
            Pitch: Convert.ToInt32(message.Arguments.ElementAt(1)),
            Yaw:   Convert.ToInt32(message.Arguments.ElementAt(2))
        );
    }
}