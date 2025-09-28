using CoreOSC;
using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Meet.Models;

public record VirtualBackgroundInfo(VirtualBackgroundState VirtualBackgroundState) : IOscParsable<VirtualBackgroundInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Meet/VirtualBackgroundInfo"
    ];
    
    public static VirtualBackgroundInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any()) 
            throw new FormatException("VirtualBackgroundInfo espera 1 arg.");

        return new VirtualBackgroundInfo(
            VirtualBackgroundState: (VirtualBackgroundState)message.Arguments.ElementAt(0)
        );
    }
}

