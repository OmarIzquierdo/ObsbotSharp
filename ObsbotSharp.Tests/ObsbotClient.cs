using System.Net.Sockets;
using System.Threading;
using CoreOSC;
using CoreOSC.IO;
using ObsbotSharp;
using ObsbotSharp.Tests.Models;
using ObsbotSharp.Tests.Models.General;
using ObsbotSharp.Tests.Models.MeetSeries;
using ObsbotSharp.Tests.Models.TinySeries;

namespace ObsbotSharp.Tests;

public interface IOscTransport : IDisposable
{
    Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default);
    Task<OscMessage> ReceiveAsync(int timeoutMs, CancellationToken cancellationToken = default);
}

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

    public async Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var message = new OscMessage(new Address(address), args ?? Array.Empty<object>());
        await udpClient.SendMessageAsync(message);
    }

    public async Task<OscMessage> ReceiveAsync(int timeoutMs, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var receiveTask = udpClient.ReceiveMessageAsync();
        var delayTask = Task.Delay(timeoutMs, cancellationToken);

        var completedTask = await Task.WhenAny(receiveTask, delayTask);
        if (completedTask != receiveTask)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new TimeoutException($"Timeout OSC ({timeoutMs} ms).");
        }

        return await receiveTask;
    }

    public void Dispose() => udpClient.Dispose();
}

public interface IObsbotClient : IDisposable
{
    IGeneralSeriesClient General { get; }
    ITinySeriesClient Tiny { get; }
    ITailSeriesClient Tail { get; }
    IMeetSeriesClient Meet { get; }
}

public interface IGeneralSeriesClient
{
    Task SetZoomAsync(int zoomLevel, CancellationToken cancellationToken = default);
    Task MoveCameraToLeftAsync(int speed, CancellationToken cancellationToken = default);
    Task MoveCameraToRightAsync(int speed, CancellationToken cancellationToken = default);
    Task MoveCameraUpAsync(int speed, CancellationToken cancellationToken = default);
    Task MoveCameraDownAsync(int speed, CancellationToken cancellationToken = default);
    Task SetMirrorAsync(MirrorState mirrorState, CancellationToken cancellationToken = default);
    Task StartRecordingPcAsync(CancellationToken cancellationToken = default);
    Task StopRecordingPcAsync(CancellationToken cancellationToken = default);
    Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType, CancellationToken cancellationToken = default);
    Task SetColorTemperatureAsync(int temperature, CancellationToken cancellationToken = default);
    Task<DeviceInfo> GetDeviceInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
    Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
    Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
}

public interface ITinySeriesClient
{
    Task SelectAiTargetStateAsync(AITargetState targetState, CancellationToken cancellationToken = default);
    Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset, CancellationToken cancellationToken = default);
    Task SelectAiModeAsync(AIMode mode, CancellationToken cancellationToken = default);
    Task SelectTrackingModeAsync(TrackingMode trackingMode, CancellationToken cancellationToken = default);
    Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
    Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
}

public interface ITailSeriesClient
{
    Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground, CancellationToken cancellationToken = default);
    Task SetAutoFramingAsync(AutoFramingState autoFramingState, CancellationToken cancellationToken = default);
    Task SetStandardModeAsync(CancellationToken cancellationToken = default);
}

public interface IMeetSeriesClient
{
    Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
    Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default);
}

public class ObsbotClient : IObsbotClient
{
    private readonly IOscTransport transport;

    private static readonly string[] NoiseAddresses =
    {
        "/OBSBOT/WebCam/General/ConnectedResp"
    };

    public ObsbotClient(ObsbotOptions options)
        : this(new UdpOscTransport(options))
    {
    }

    internal ObsbotClient(IOscTransport transport)
    {
        this.transport = transport ?? throw new ArgumentNullException(nameof(transport));

        General = CreateGeneralSeries();
        Tiny = CreateTinySeries();
        Tail = CreateTailSeries();
        Meet = CreateMeetSeries();
    }

    protected virtual IOscTransport Transport => transport;

    public IGeneralSeriesClient General { get; }

    public ITinySeriesClient Tiny { get; }

    public ITailSeriesClient Tail { get; }

    public IMeetSeriesClient Meet { get; }

    protected virtual GeneralSeries CreateGeneralSeries() => new(this);

    protected virtual TinySeries CreateTinySeries() => new(this);

    protected virtual TailSeries CreateTailSeries() => new(this);

    protected virtual MeetSeries CreateMeetSeries() => new(this);

    protected virtual Task SendAsync(string address, object[]? args, CancellationToken cancellationToken = default) =>
        Transport.SendAsync(address, args, cancellationToken);

    protected virtual Task<T> SendAndWaitAsync<T>(
        string requestAddress,
        object[]? args,
        int timeoutMs,
        CancellationToken cancellationToken) where T : IOscParsable<T>
    {
        return SendAndWaitInternalAsync<T>(requestAddress, args, timeoutMs, cancellationToken);
    }

    private async Task<T> SendAndWaitInternalAsync<T>(
        string requestAddress,
        object[]? args,
        int timeoutMs,
        CancellationToken cancellationToken) where T : IOscParsable<T>
    {
        await SendAsync(requestAddress, args, cancellationToken);
        return await WaitForAsync<T>(timeoutMs, cancellationToken);
    }

    protected virtual async Task<T> WaitForAsync<T>(int timeoutMs, CancellationToken cancellationToken)
        where T : IOscParsable<T>
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var remaining = (int)Math.Max(1, (deadline - DateTime.UtcNow).TotalMilliseconds);
            var oscMessage = await Transport.ReceiveAsync(remaining, cancellationToken);

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

    public virtual void Dispose() => Transport.Dispose();

    private sealed class GeneralSeries : SeriesBase, IGeneralSeriesClient
    {
        public GeneralSeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }

        public virtual Task SetZoomAsync(int zoomLevel, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetZoom", new object[] { zoomLevel }, cancellationToken);

        public virtual Task MoveCameraToLeftAsync(int speed, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetGimbalLeft", new object[] { speed }, cancellationToken);

        public virtual Task MoveCameraToRightAsync(int speed, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetGimbalRight", new object[] { speed }, cancellationToken);

        public virtual Task MoveCameraUpAsync(int speed, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetGimbalUp", new object[] { speed }, cancellationToken);

        public virtual Task MoveCameraDownAsync(int speed, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetGimbalDown", new object[] { speed }, cancellationToken);

        public virtual Task SetMirrorAsync(MirrorState mirrorState, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetMirror", new object[] { (int)mirrorState }, cancellationToken);

        public virtual Task StartRecordingPcAsync(CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetPCRecording", new object[] { 1 }, cancellationToken);

        public virtual Task StopRecordingPcAsync(CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetPCRecording", new object[] { 0 }, cancellationToken);

        public virtual Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetAutoWhiteBalance", new object[] { (int)whiteBalanceType }, cancellationToken);

        public virtual Task SetColorTemperatureAsync(int temperature, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/General/SetColorTemperature", new object[] { 0, temperature }, cancellationToken);

        public virtual Task<DeviceInfo> GetDeviceInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<DeviceInfo>("/OBSBOT/WebCam/General/GetDeviceInfo", new object[] { deviceIndex }, 2000, cancellationToken);

        public virtual Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<ZoomInfo>("/OBSBOT/WebCam/General/GetZoomInfo", new object[] { deviceIndex }, 2000, cancellationToken);

        public virtual Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<GimbalPosInfo>("/OBSBOT/WebCam/General/GetGimbalPosInfo", new object[] { deviceIndex }, 2000, cancellationToken);
    }

    private sealed class TinySeries : SeriesBase, ITinySeriesClient
    {
        public TinySeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }

        public virtual Task SelectAiTargetStateAsync(AITargetState targetState, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Tiny/ToggleAILock", new object[] { (int)targetState }, cancellationToken);

        public virtual Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Tiny/TriggerPreset", new object[] { (int)triggerPreset }, cancellationToken);

        public virtual Task SelectAiModeAsync(AIMode mode, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Tiny/SetAiMode", new object[] { (int)mode }, cancellationToken);

        public virtual Task SelectTrackingModeAsync(TrackingMode trackingMode, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Tiny/SetTrackingMode", new object[] { (int)trackingMode }, cancellationToken);

        public virtual Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<AiTrackingInfo>("/OBSBOT/WebCam/Tiny/GetAiTrackingInfo", new object[] { deviceIndex }, 2000, cancellationToken);

        public virtual Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<PresetPositionInfo>("/OBSBOT/WebCam/Tiny/GetPresetPositionInfo", new object[] { deviceIndex }, 2000, cancellationToken);
    }

    private sealed class TailSeries : SeriesBase, ITailSeriesClient
    {
        public TailSeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }

        public virtual Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Meet/SetVirtualBackground", new object[] { (int)virtualBackground }, cancellationToken);

        public virtual Task SetAutoFramingAsync(AutoFramingState autoFramingState, CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Meet/SetAutoFraming", new object[] { (int)autoFramingState }, cancellationToken);

        public virtual Task SetStandardModeAsync(CancellationToken cancellationToken = default) =>
            SendAsync("/OBSBOT/WebCam/Meet/SetStandardMode", Array.Empty<object>(), cancellationToken);
    }

    private sealed class MeetSeries : SeriesBase, IMeetSeriesClient
    {
        public MeetSeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }

        public virtual Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<VirtualBackgroundInfo>("/OBSBOT/WebCam/Meet/GetVirtualBackgroundInfo", new object[] { deviceIndex }, 2000, cancellationToken);

        public virtual Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0, CancellationToken cancellationToken = default) =>
            SendAndWaitAsync<AutoFramingInfo>("/OBSBOT/WebCam/Meet/GetAutoFramingInfo", new object[] { deviceIndex }, 2000, cancellationToken);
    }

    private abstract class SeriesBase
    {
        private readonly ObsbotClient obsbotClient;

        protected SeriesBase(ObsbotClient obsbotClient)
        {
            this.obsbotClient = obsbotClient;
        }

        protected Task SendAsync(string address, object[]? args, CancellationToken cancellationToken) =>
            obsbotClient.SendAsync(address, args, cancellationToken);

        protected Task<T> SendAndWaitAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs,
            CancellationToken cancellationToken) where T : IOscParsable<T> =>
            obsbotClient.SendAndWaitAsync<T>(requestAddress, args, timeoutMs, cancellationToken);
    }
}
