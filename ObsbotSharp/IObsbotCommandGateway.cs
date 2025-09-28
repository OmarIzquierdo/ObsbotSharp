using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp;

internal interface IObsbotCommandGateway
{
    Task SendAsync(string address, object[]? args);
    Task<T> SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs) where T : IOscParsable<T>;
}