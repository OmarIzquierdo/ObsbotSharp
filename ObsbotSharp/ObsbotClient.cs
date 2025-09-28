using ObsbotSharp.Domain.General;
using ObsbotSharp.Domain.General.Commands;
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
    public  class ObsbotClient : IObsbotClient, IObsbotCommandGateway
    {
        private readonly IOscTransport transport;

        private static readonly string[] NoiseAddresses =
        {
            "/OBSBOT/WebCam/General/ConnectedResp"
        };

        public ITinySeries Tiny { get; }
        public ITailSeries Tail { get; }
        public IMeetSeries Meet { get; }
        public IGeneralSeries General { get; }

        public ObsbotClient(ObsbotOptions options) : this(new UdpOscTransport(options))
        {
        }

        public ObsbotClient(IOscTransport transport)
        {
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));
            
            var gateway = (IObsbotCommandGateway)this;
            
            General = new GeneralSeries(gateway);
            Tiny    = new TinySeries(gateway);
            Tail    = new TailSeries(gateway);
            Meet    = new MeetSeries(gateway);
        }
        
        Task IObsbotCommandGateway.SendAsync(string address, object[]? args) => 
            SendAsync(address, args);
        Task<T> IObsbotCommandGateway.SendAndWaitAsync<T>(string requestAddress, object[]? args, int timeoutMs) => 
            SendAndWaitAsync<T>(requestAddress, args, timeoutMs);
        
        protected virtual Task SendAsync(string address, object[]? args) =>
            transport.SendAsync(address, args);

        protected virtual Task<T> SendAndWaitAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs) where T : IOscParsable<T> =>
            SendAndWaitInternalAsync<T>(requestAddress, args, timeoutMs);

        private async Task<T> SendAndWaitInternalAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs) where T : IOscParsable<T>
        {
            await SendAsync(requestAddress, args);
            return await WaitForAsync<T>(timeoutMs);
        }

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

        public virtual void Dispose() => transport.Dispose();
    }
}
