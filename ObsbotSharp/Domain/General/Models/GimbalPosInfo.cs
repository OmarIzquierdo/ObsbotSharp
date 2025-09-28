using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.General.Models;

public record GimbalPosInfo(int Roll, int Pitch, int Yaw) : IOscParsable<GimbalPosInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/GetGimbalPosInfoResp"
    ];

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