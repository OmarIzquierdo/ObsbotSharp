using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Meet.Models;
public record AutoFramingInfo(AutoFramingState AutoFramingState) : IOscParsable<AutoFramingInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Meet/AutoFramingInfo"
    ];
    
    public static AutoFramingInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any()) 
            throw new FormatException("AutoFramingInfo espera 1 arg.");

        return new AutoFramingInfo(
            AutoFramingState: (AutoFramingState)message.Arguments.ElementAt(0)
        );
    }
}

public enum AutoFramingState
{
    Disable, 
    SingleMode,
    GroupMode
}