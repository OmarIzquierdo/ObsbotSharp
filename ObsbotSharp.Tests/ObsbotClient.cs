using System.Net;
using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;

namespace ObsbotSharp;

/// <summary>
/// Client for controlling an OBSBOT camera using relative operations.
/// </summary>
public class ObsbotClient
{
    private UdpClient _udpClient;

    /// <summary>
    /// Initializes a new instance of the client with the provided connection options.
    /// </summary>
    /// <param name="options">
    /// Connection options containing the target host (IPv4/IPv6 string) and port.
    /// </param>
    public ObsbotClient(ObsbotOptions options)
    {
        _udpClient = new UdpClient(options.Host, options.Port);
    }

    /// <summary>
    /// Tilts the camera upward by a relative angle.
    /// </summary>
    /// <param name="deltaDegrees">
    /// Positive integer, in degrees, indicating how much to tilt up (relative move). Must be greater than 0.
    /// The effective upper bound depends on the device; excessive values may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task TiltUpAsync(int deltaDegrees)
    {
        var address = "/OBSBOT/WebCam/General/SetGimbalUp";
        await Send(address, deltaDegrees);
        await Stop(address);
    }

    /// <summary>
    /// Tilts the camera downward by a relative angle.
    /// </summary>
    /// <param name="deltaDegrees">
    /// Positive integer, in degrees, indicating how much to tilt down (relative move). Must be greater than 0.
    /// The effective upper bound depends on the device; excessive values may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task TiltDownAsync(int deltaDegrees)
    {
        const string address = "/OBSBOT/WebCam/General/SetGimbalDown";
        await Send(address, deltaDegrees);
        await Stop(address);
    }

    private async Task Stop(string address)
    {
        await Task.Delay(2000);
        await Send(address, 0);
    }

    /// <summary>
    /// Pans the camera to the left by a relative angle.
    /// </summary>
    /// <param name="deltaDegrees">
    /// Positive integer, in degrees, indicating how much to pan left (relative move). Must be greater than 0.
    /// The effective upper bound depends on the device; excessive values may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PanLeftAsync(int deltaDegrees)
    {
        var address = "/OBSBOT/WebCam/General/SetGimbalLeft";
        await Send(address, deltaDegrees);
        await Stop(address);
    }

    /// <summary>
    /// Pans the camera to the right by a relative angle.
    /// </summary>
    /// <param name="deltaDegrees">
    /// Positive integer, in degrees, indicating how much to pan right (relative move). Must be greater than 0.
    /// The effective upper bound depends on the device; excessive values may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PanRightAsync(int deltaDegrees)
    {
        var address = "/OBSBOT/WebCam/General/SetGimbalRight";
        await Send(address, deltaDegrees);
        await Stop(address);
    }

    /// <summary>
    /// Zooms in by a relative amount.
    /// </summary>
    /// <param name="delta">
    /// Positive integer indicating the relative zoom increment. Must be greater than 0.
    /// The unit and effective range depend on the device; larger values produce a larger effect and may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ZoomInAsync(int delta)
    {
        var address = "/ptz/zoomIn";
        await Send(address, delta);
        await Stop(address);
    }

    /// <summary>
    /// Zooms out by a relative amount.
    /// </summary>
    /// <param name="delta">
    /// Positive integer indicating the relative zoom decrement. Must be greater than 0.
    /// The unit and effective range depend on the device; larger values produce a larger effect and may be clamped.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ZoomOutAsync(int delta)
    {
        var address = "/ptz/zoomOut";
        await Send(address, delta);
        await Stop(address);
    }

    private Task Send(string address, params object[] args)
    {
        var message = new OscMessage(new Address(address), args);
        return _udpClient.SendMessageAsync(message);
    }
}

/// <summary>
/// Connection and configuration options for <see cref="ObsbotClient"/>.
/// </summary>
public class ObsbotOptions
{
    /// <summary>
    /// Target host expressed as an IPv4 or IPv6 string. Default is <c>"127.0.0.1"</c>.
    /// </summary>
    /// <value>An IP address string (e.g., <c>"192.168.1.10"</c> or <c>"::1"</c>).</value>
    public string Host { get; private set; } = "127.0.0.1";

    /// <summary>
    /// Target UDP port number. Valid range is 1..65535. Default is <c>16284</c>.
    /// </summary>
    /// <value>An integer port within the range 1..65535.</value>
    public int Port { get; private set; } = 16284;

    /// <summary>
    /// Sets the target host.
    /// </summary>
    /// <param name="host">
    /// IPv4 or IPv6 string. Must be non-empty and a valid IP representation (hostnames are not accepted).
    /// </param>
    /// <returns>The same <see cref="ObsbotOptions"/> instance to allow method chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="host"/> is null/empty/whitespace or not a valid IPv4/IPv6 string.
    /// </exception>
    public ObsbotOptions WithHost(string host)
    {
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("El host no puede ser nulo o vacío.", nameof(host));
        if (!IPAddress.TryParse(host, out _))
            throw new ArgumentException("El host no es una IPv4/IPv6 válida.", nameof(host));

        Host = host;
        return this;
    }

    /// <summary>
    /// Sets the target UDP port.
    /// </summary>
    /// <param name="port">
    /// Integer within the range 1..65535.
    /// </param>
    /// <returns>The same <see cref="ObsbotOptions"/> instance to allow method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="port"/> is outside the 1..65535 range.
    /// </exception>
    public ObsbotOptions WithPort(int port)
    {
        if (port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Puerto fuera de rango 1..65535.");

        Port = port;
        return this;
    }
}


public class Test
{
    public async Task Testt()
    {
        var obsbotSharp = new ObsbotClient(
            new ObsbotOptions()
                .WithHost("127.0.0.1")
                .WithPort(16284));

        await obsbotSharp.TiltUpAsync(10);
    }
}
