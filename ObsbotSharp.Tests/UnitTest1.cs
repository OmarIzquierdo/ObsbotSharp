using CoreOSC;
using ObsbotSharp.Tests.Models.General;

namespace ObsbotSharp.Tests;

public class ObsbotClientTests
{
    [Fact]
    public async Task MoveCameraToRightAsync_QueuesOscMessage()
    {
        var transport = new FakeOscTransport();
        using var client = new ObsbotClient(transport);

        await client.General.MoveCameraToRightAsync(10);

        var message = Assert.Single(transport.SentMessages);
        Assert.Equal("/OBSBOT/WebCam/General/SetGimbalRight", message.Address);
        Assert.Equal(new object[] { 10 }, message.Arguments);
    }

    [Fact]
    public async Task GetZoomInfoAsync_ParsesResponseFromTransport()
    {
        var transport = new FakeOscTransport();
        using var client = new ObsbotClient(transport);

        transport.EnqueueResponse(
            new OscMessage(
                new Address("/OBSBOT/WebCam/General/ZoomInfo"),
                new object[] { 35, 2 }));

        var zoomInfo = await client.General.GetZoomInfoAsync(deviceIndex: 0);

        Assert.Equal(35, zoomInfo.ZoomPercent);
        Assert.Equal(2, zoomInfo.FovPreset);
    }

    [Fact]
    public async Task Methods_CanBeOverriddenForCustomMocking()
    {
        using var client = new TestableObsbotClient();

        await client.General.MoveCameraDownAsync(5);

        var invocation = Assert.Single(client.SentMessages);
        Assert.Equal("/OBSBOT/WebCam/General/SetGimbalDown", invocation.Address);
        Assert.Equal(new object[] { 5 }, invocation.Arguments);
    }

    private sealed class FakeOscTransport : IOscTransport
    {
        private readonly Queue<OscMessage> responses = new();

        public List<(string Address, object[] Arguments)> SentMessages { get; } = new();

        public Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default)
        {
            SentMessages.Add((address, (args ?? Array.Empty<object>()).ToArray()));
            return Task.CompletedTask;
        }

        public Task<OscMessage> ReceiveAsync(int timeoutMs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (responses.Count == 0)
            {
                throw new TimeoutException($"No OSC message enqueued within {timeoutMs} ms.");
            }

            return Task.FromResult(responses.Dequeue());
        }

        public void EnqueueResponse(OscMessage message) => responses.Enqueue(message);

        public void Dispose()
        {
        }
    }

    private sealed class TestableObsbotClient : ObsbotClient
    {
        public TestableObsbotClient()
            : base(new NullOscTransport())
        {
        }

        public List<(string Address, object[] Arguments)> SentMessages { get; } = new();

        protected override Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default)
        {
            SentMessages.Add((address, (args ?? Array.Empty<object>()).ToArray()));
            return Task.CompletedTask;
        }

        protected override Task<T> SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Wait operations should be mocked explicitly in tests.");
        }

        private sealed class NullOscTransport : IOscTransport
        {
            public Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default) => Task.CompletedTask;

            public Task<OscMessage> ReceiveAsync(int timeoutMs, CancellationToken cancellationToken = default) =>
                throw new NotSupportedException();

            public void Dispose()
            {
            }
        }
    }
}
