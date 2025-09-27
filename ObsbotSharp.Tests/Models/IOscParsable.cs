using CoreOSC;

namespace ObsbotSharp.Tests.Models;

public interface IOscParsable<TSelf> where TSelf : IOscParsable<TSelf>
{
    static abstract string[] ReplyAddresses { get; }
    static abstract TSelf Parse(OscMessage message);
}