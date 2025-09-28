using ObsbotSharp.Domain.Base;
using ObsbotSharp.Domain.Base.Commands;
using ObsbotSharp.Domain.Meet;
using ObsbotSharp.Domain.Meet.Commands;
using ObsbotSharp.Domain.Tail;
using ObsbotSharp.Domain.Tail.Commands;
using ObsbotSharp.Domain.Tiny;
using ObsbotSharp.Domain.Tiny.Commands;
using ObsbotSharp.Infrastructure.Osc.Parsers;
using ObsbotSharp.Infrastructure.Osc.Transport;

namespace ObsbotSharp
{
    /// <summary>
    /// Default implementation of <see cref="IObsbotClient"/> that communicates with OBSBOT cameras
    /// through the OSC protocol.
    /// </summary>
    /// <remarks>
    /// The client sends messages that follow the address map documented by OBSBOT and exposes
    /// typed helpers for the Base, Tiny, Tail and Meet product families. By default the
    /// communication is done via UDP using <see cref="UdpOscTransport"/>.
    /// </remarks>
    public  class ObsbotClient : IObsbotClient, IObsbotCommandGateway
    {
        private readonly IOscTransport transport;

        private static readonly string[] NoiseAddresses =
        {
            "/OBSBOT/WebCam/General/ConnectedResp"
        };

        /// <inheritdoc />
        public ITinySeries Tiny { get; }
        /// <inheritdoc />
        public ITailSeries Tail { get; }
        /// <inheritdoc />
        public IMeetSeries Meet { get; }
        /// <inheritdoc />
        public IBaseSeries Base { get; }

        /// <summary>
        /// Creates a new client that uses a UDP transport configured with the provided options.
        /// </summary>
        /// <param name="options">Transport options that describe the OSC endpoint exposed by the camera.</param>
        public ObsbotClient(ObsbotOptions options) : this(new UdpOscTransport(options))
        {
        }

        /// <summary>
        /// Creates a new client using a custom OSC transport implementation.
        /// </summary>
        /// <param name="transport">Transport responsible for sending and receiving OSC messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="transport"/> is <see langword="null"/>.</exception>
        public ObsbotClient(IOscTransport transport)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));

            var gateway = (IObsbotCommandGateway)this;
            
            Base = new BaseSeries(gateway);
            Tiny    = new TinySeries(gateway);
            Tail    = new TailSeries(gateway);
            Meet    = new MeetSeries(gateway);
        }
        
        Task IObsbotCommandGateway.SendAsync(string address, object[]? args) => 
            SendAsync(address, args);
        Task<T> IObsbotCommandGateway.SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs) => 
            SendAndWaitAsync<T>(requestAddress, args, timeoutMs);
        
        /// <summary>
        /// Sends an OSC message without waiting for a response.
        /// </summary>
        /// <param name="address">OSC address documented by OBSBOT.</param>
        /// <param name="args">Arguments following the specification for the selected address.</param>
        protected virtual Task SendAsync(string address, object[]? args) =>
            transport.SendAsync(address, args);

        /// <summary>
        /// Sends an OSC message and waits for a typed response following the parser contract provided by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Parser that knows how to deserialize the reply message.</typeparam>
        /// <param name="requestAddress">Address to send the request to.</param>
        /// <param name="args">Arguments that accompany the message.</param>
        /// <param name="timeoutMs">Maximum time in milliseconds to wait for the reply before timing out.</param>
        protected virtual Task<T> SendAndWaitAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs) where T : IOscParsable<T> =>
            SendAndWaitInternalAsync<T>(requestAddress, args, timeoutMs);

        /// <summary>
        /// Sends a request and forwards the response to the appropriate parser.
        /// </summary>
        private async Task<T> SendAndWaitInternalAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs) where T : IOscParsable<T>
        {
            await SendAsync(requestAddress, args);
            return await WaitForAsync<T>(timeoutMs);
        }

        /// <summary>
        /// Waits for an OSC message that matches any of the reply addresses declared by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Parser used to deserialize the reply.</typeparam>
        /// <param name="timeoutMs">Maximum time in milliseconds to wait for the reply.</param>
        /// <remarks>
        /// The method filters out connection noise such as connection acknowledgements to avoid
        /// interfering with the expected response flow.
        /// </remarks>
        protected virtual async Task<T> WaitForAsync<T>(int timeoutMs) where T : IOscParsable<T>
        {
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            while (true)
            {
                var remaining = (int)Math.Max(1, (deadline - DateTime.UtcNow).TotalMilliseconds);
                var oscMessage = await transport.ReceiveAsync(remaining);

                if (NoiseAddresses.Any(address =>
                        oscMessage.Address.Value.Equals(address, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                if (T.ReplyAddresses.Any(address =>
                        oscMessage.Address.Value.Equals(address, StringComparison.OrdinalIgnoreCase)))
                {
                    return T.Parse(oscMessage);
                }
            }
        }

        /// <summary>
        /// Releases the underlying OSC transport.
        /// </summary>
        public virtual void Dispose() => transport.Dispose();
    }
}
