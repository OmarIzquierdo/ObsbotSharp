using CoreOSC;

namespace ObsbotSharp.Infrastructure.Osc.Transport;

public interface IOscTransport : IDisposable
{
    Task SendAsync(string address, object[]? args);
    Task<OscMessage> ReceiveAsync(int timeoutMs);
}