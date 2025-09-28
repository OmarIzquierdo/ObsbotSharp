using ObsbotSharp.Domain.Common.Models;
using ObsbotSharp.Domain.Meet.Models;

namespace ObsbotSharp.Domain.Meet;

public interface IMeetSeries
{
    Task SetAutoFocusAsync(AutoFocusType autofocusType);
    Task SetManualFocusAsync(int manualFocusValue);
    Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground);
    Task SetAutoFramingAsync(AutoFramingState autoFramingState);
    Task SetStandardModeAsync();
    Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0);
    Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0);
}