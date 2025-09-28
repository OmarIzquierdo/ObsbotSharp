using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;

namespace ObsbotSharp.Infrastructure.Osc.Transport;

public sealed class UdpOscTransport : IOscTransport
{
    private readonly UdpClient udpClient;

    public UdpOscTransport(ObsbotOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        udpClient = new UdpClient(options.LocalPort);
        udpClient.Connect(options.Host, options.RemotePort);
    }

    public async Task SendAsync(string address, object[]? args)
    {
        var message = new OscMessage(new Address(address), args ?? []);
        await udpClient.SendMessageAsync(message);
    }

    public async Task<OscMessage> ReceiveAsync(int timeoutMs)
    {
        var receiveTask = udpClient.ReceiveMessageAsync();
        var completedTask = await Task.WhenAny(receiveTask, Task.Delay(timeoutMs));

        if (completedTask != receiveTask)
            throw new TimeoutException($"Timeout OSC ({timeoutMs} ms).");

        return await receiveTask;
    }

    public void Dispose() => udpClient.Dispose();
}