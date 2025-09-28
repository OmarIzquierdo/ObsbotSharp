using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Tiny.Models;

public record AiTrackingInfo(AiTrackingState AiTrackingState) : IOscParsable<AiTrackingInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Tiny/AiTrackingInfo",
    ];
    
    public static AiTrackingInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any()) 
            throw new FormatException("AiTrackingInfo espera 1 arg.");

        return new AiTrackingInfo(
            AiTrackingState: (AiTrackingState)message.Arguments.ElementAt(0)
        );
    }
}

public enum AiTrackingState
{
    Unlock,
    Lock
}