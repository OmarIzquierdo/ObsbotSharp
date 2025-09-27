using CoreOSC;

namespace ObsbotSharp.Tests.Models.General;

public record ZoomInfo(int ZoomPercent, int FovPreset) : IOscParsable<ZoomInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/ZoomInfo"
    ];

    public static ZoomInfo Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 2) 
            throw new FormatException("ZoomInfo espera 2 args.");
        
        return new ZoomInfo(
            ZoomPercent: Convert.ToInt32(message.Arguments.ElementAt(0)),
            FovPreset:   Convert.ToInt32(message.Arguments.ElementAt(1)));
    }
}