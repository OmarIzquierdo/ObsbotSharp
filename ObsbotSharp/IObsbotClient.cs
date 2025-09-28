using ObsbotSharp.Domain.General;
using ObsbotSharp.Domain.Meet;
using ObsbotSharp.Domain.Tail;
using ObsbotSharp.Domain.Tiny;

namespace ObsbotSharp;

public interface IObsbotClient : IDisposable
{
    IBaseSeries Base { get; }
    ITinySeries Tiny { get; }
    ITailSeries Tail { get; }
    IMeetSeries Meet { get; }
}