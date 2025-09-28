using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp;

/// <summary>
/// Internal abstraction used by command groups to send OSC messages through the client.
/// </summary>
internal interface IObsbotCommandGateway
{
    /// <summary>
    /// Sends an OSC command without expecting a response.
    /// </summary>
    Task SendAsync(string address, object[]? args);

    /// <summary>
    /// Sends an OSC command and waits for a typed response.
    /// </summary>
    /// <typeparam name="T">Type that knows how to parse the response.</typeparam>
    /// <param name="requestAddress">Request address to send.</param>
    /// <param name="args">Arguments for the request.</param>
    /// <param name="timeoutMs">Timeout for the response in milliseconds.</param>
    Task<T> SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs) where T : IOscParsable<T>;
}