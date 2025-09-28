using CoreOSC;

namespace ObsbotSharp.Seams;

public class ObsbotClientSeam : ObsbotClient
{
    public ObsbotClientSeam()
        : base(new NullOscTransport())
    {
    }

    public List<(string Address, object[] Arguments)> SentMessages { get; } = [];

    protected override Task SendAsync(string address, object[]? args)
    {
        SentMessages.Add((address, (args ?? []).ToArray()));
        return Task.CompletedTask;
    }

    protected override Task<T> SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs)
    {
        throw new NotSupportedException("Wait operations should be mocked explicitly in tests.");
    }

    private sealed class NullOscTransport : IOscTransport
    {
        public Task SendAsync(string address, object[]? args) => Task.CompletedTask;

        public Task<OscMessage> ReceiveAsync(int timeoutMs) =>
            throw new NotSupportedException();

        public void Dispose()
        {
        }
    }
}