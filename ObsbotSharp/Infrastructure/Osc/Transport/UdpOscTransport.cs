using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;

namespace ObsbotSharp.Infrastructure.Osc.Transport;

/// <summary>
/// UDP implementation of <see cref="IOscTransport"/> compatible with OBSBOT OSC endpoints.
/// </summary>
public sealed class UdpOscTransport : IOscTransport
{
    private readonly UdpClient udpClient;

    /// <summary>
    /// Initializes a new transport using the provided options to configure the UDP client.
    /// </summary>
    /// <param name="options">Connection options describing local and remote endpoints.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
    public UdpOscTransport(ObsbotOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        udpClient = new UdpClient(options.LocalPort);
        udpClient.Connect(options.Host, options.RemotePort);
    }

    /// <inheritdoc />
    public async Task SendAsync(string address, object[]? args)
    {
        var message = new OscMessage(new Address(address), args ?? []);
        await udpClient.SendMessageAsync(message);
    }

    /// <inheritdoc />
    public async Task<OscMessage> ReceiveAsync(int timeoutMs)
    {
        var receiveTask = udpClient.ReceiveMessageAsync();
        var completedTask = await Task.WhenAny(receiveTask, Task.Delay(timeoutMs));

        if (completedTask != receiveTask)
            throw new TimeoutException($"Timeout OSC ({timeoutMs} ms).");

        return await receiveTask;
    }

    /// <inheritdoc />
    public void Dispose() => udpClient.Dispose();
}