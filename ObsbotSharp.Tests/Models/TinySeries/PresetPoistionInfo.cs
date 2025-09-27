using CoreOSC;

namespace ObsbotSharp.Tests.Models.TinySeries;

public record PresetPositionInfo(AiTrackingState AiTrackingState) : IOscParsable<PresetPositionInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/Tiny/PresetPositionInfo",
    ];
    
    public static PresetPositionInfo Parse(OscMessage message)
    {
        if (!message.Arguments.Any()) 
            throw new FormatException("PresetPositionInfo espera 2 args.");

        return new PresetPositionInfo(
            AiTrackingState: (AiTrackingState)message.Arguments.ElementAt(0)
        );
    }
}