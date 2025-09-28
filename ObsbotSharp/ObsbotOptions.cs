using System.Net;

namespace ObsbotSharp;

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
    public int RemotePort { get; private set; } = 16284;
    
    /// <summary>
    /// Local UDP port used to bind the OSC client socket. Default is <c>100000</c> which allows the
    /// operating system to pick an ephemeral port at runtime.
    /// </summary>
    public int LocalPort { get; private set; } = 100000;

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
            throw new ArgumentException("Host can't be null or empty.", nameof(host));
        
        if (!IPAddress.TryParse(host, out _))
            throw new ArgumentException("Host is not a IPv4/IPv6 valid.", nameof(host));

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
    public ObsbotOptions WithRemotePort(int port)
    {
        if (port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port is out of range (1..65535).");

        RemotePort = port;
        return this;
    }
    
    /// <summary>
    /// Sets the local UDP port used to receive OSC responses.
    /// </summary>
    /// <param name="port">Integer within the range 1..65535.</param>
    /// <returns>The same <see cref="ObsbotOptions"/> instance to allow method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="port"/> is outside the 1..65535 range.
    /// </exception>
    public ObsbotOptions WithLocalPort(int port)
    {
        if (port is < 1 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port is out of range (1..65535).");

        LocalPort = port;
        return this;
    }
}