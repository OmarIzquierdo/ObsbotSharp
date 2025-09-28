using CoreOSC;

namespace ObsbotSharp.Infrastructure.Osc.Transport;

/// <summary>
/// Describes an OSC transport capable of sending and receiving messages.
/// </summary>
public interface IOscTransport : IDisposable
{
    /// <summary>
    /// Sends an OSC message to the underlying transport endpoint.
    /// </summary>
    /// <param name="address">OSC address.</param>
    /// <param name="args">Arguments that accompany the message.</param>
    Task SendAsync(string address, object[]? args);

    /// <summary>
    /// Receives an OSC message, waiting up to <paramref name="timeoutMs"/> milliseconds.
    /// </summary>
    /// <param name="timeoutMs">Maximum wait time in milliseconds.</param>
    /// <returns>The received OSC message.</returns>
    Task<OscMessage> ReceiveAsync(int timeoutMs);
}