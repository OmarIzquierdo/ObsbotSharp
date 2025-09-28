using CoreOSC;

namespace ObsbotSharp.Seams;

public class OscTransportSeam : IOscTransport
{
    private readonly Queue<OscMessage> responses = new();
    public List<(string Address, object[] Arguments)> SentMessages { get; } = [];

    public Task SendAsync(string address, object[]? args)
    {
        SentMessages.Add((address, (args ?? []).ToArray()));
        return Task.CompletedTask;
    }
    public Task<OscMessage> ReceiveAsync(int timeoutMs)
    {
        if (responses.Count == 0)
            throw new TimeoutException($"No OSC message enqueued within {timeoutMs} ms.");
            
        return Task.FromResult(responses.Dequeue());
    }
    public void EnqueueResponse(OscMessage message) => responses.Enqueue(message);

    public void Dispose()
    {
    }
}