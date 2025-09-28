using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Tiny.Models;

namespace ObsbotSharp.Domain.Tiny;

public interface ITinySeries
{
    Task SetAutoFocusAsync(AutoFocusType autofocusType);
    Task SetManualFocusAsync(int manualFocusValue);
    Task SelectAITargetStateAsync(AITargetState AITargetState);
    Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset);
    Task SelectAIModeAsync(AIMode AIMode);
    Task SelectTrackingModeAsync(TrackingMode trackingMode);
    Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0);
    Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0);
}