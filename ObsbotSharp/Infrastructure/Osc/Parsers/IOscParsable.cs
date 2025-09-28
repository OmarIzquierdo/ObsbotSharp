using CoreOSC;

namespace ObsbotSharp.Infrastructure.Osc.Parsers;

/// <summary>
/// Provides a contract for parsing OSC responses into strongly typed records.
/// </summary>
/// <typeparam name="TSelf">Concrete type that implements the parsing behavior.</typeparam>
public interface IOscParsable<TSelf> where TSelf : IOscParsable<TSelf>
{
    /// <summary>
    /// Gets the OSC reply addresses that the parser is interested in.
    /// </summary>
    static abstract string[] ReplyAddresses { get; }

    /// <summary>
    /// Creates a typed instance from an OSC message.
    /// </summary>
    /// <param name="message">Message received from the transport layer.</param>
    /// <returns>Parsed instance.</returns>
    static abstract TSelf Parse(OscMessage message);
}