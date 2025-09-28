using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;
using ObsbotSharp.Models;
using ObsbotSharp.Models.Common;
using ObsbotSharp.Models.General;
using ObsbotSharp.Models.MeetSeries;
using ObsbotSharp.Models.TailSeries;
using ObsbotSharp.Models.TinySeries;

namespace ObsbotSharp;

public interface IOscTransport : IDisposable
{
    Task SendAsync(string address, object[]? args);
    Task<OscMessage> ReceiveAsync(int timeoutMs);
}

public sealed class UdpOscTransport : IOscTransport
{
    private readonly UdpClient udpClient;

    public UdpOscTransport(ObsbotOptions options)
    {
        if(options is null)
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

public interface IObsbotClient : IDisposable
{
    IGeneralSeries General { get; }
    ITinySeries Tiny { get; }
    ITailSeries Tail { get; }
    IMeetSeries Meet { get; }
}

public interface IGeneralSeries
{
    Task SelectDevice(DeviceNumber deviceNumber);
    Task SetZoomAsync(int zoomLevel);
    Task MoveCamaraToLeftAsync(int speed);
    Task MoveCamaraToRightAsync(int speed);
    Task MoveCamaraToUpAsync(int speed);
    Task MoveCamaraToDownAsync(int speed);
    Task SetMirrorAsync(MirrorState mirrorState);
    Task StartRecordingPcAsync();
    Task StopRecordingPcAsync();
    Task TakeScreenshotAsync();
    Task SetAutoExposureAsync(AutoExposureType autoExposureType);
    Task SetExposureCompensateAsync(ExposureCompensation exposureCompensation);
    Task SetShutterSpeedAsync(ShutterPreset shutterPreset);
    Task SetISOAsync(int isoValue);
    Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType);
    Task SetColorTemperatureAsync(int temperature);
    Task<DeviceInfo> GeDeviceInfoAsync(int deviceIndex = 0);
    Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0);
    Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0);
}

public interface ITinySeries
{
    Task SetAutoFocusAsync(AutoFocusType autofocusType);
    Task SetManualFocusAsync(int manualFocusValue);
    Task SelectAITargetStateAsync(AITargetState AITargetState);
    Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset);
    Task SelectAIModeAsync(AIMode AIMode);
    Task SelectTrackingModeAsync(TrackingMode trackingMode);
    Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0);
    Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0);
}

public interface ITailSeries
{
    Task SelectAIModeAsync(TailAirAiMode tailAirAiMode);
    Task SelectAIModeAsync(Tail2AiMode tailAirAiMode);
    Task SelectTrackingSpeed(TailAirTrackingSpeed tailAirTrackingSpeed);
    Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed);
    Task SetPanTrackingSpeed(PanAxis panAxis);
    Task SetPanAxisLock(TiltAxis tiltAxis);
    Task SetTiltAxisLock(int speed);
    Task StartRecordingAsync();
    Task StopRecordingAsync();
    Task TakeScreenshotAsync();
    Task SetTriggerPreset(TriggerPreset triggerPreset);
}

public interface IMeetSeries
{
    Task SetAutoFocusAsync(AutoFocusType autofocusType);
    Task SetManualFocusAsync(int manualFocusValue);
    Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground);
    Task SetAutoFramingAsync(AutoFramingState autoFramingState);
    Task SetStandardModeAsync();
    Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0);
    Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0);
}

public class ObsbotClient : IObsbotClient
{
    private readonly IOscTransport transport;
    protected virtual IOscTransport Transport => transport;
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

        Tiny = new TinySeries(this);
        Tail = new TailSeries(this);
        Meet = new MeetSeries(this);
        General = new GeneralSeries(this);
    }
    
    protected virtual Task SendAsync(string address, object[]? args)
    {
        return Transport.SendAsync(address, args);
    }

    protected virtual Task<T> SendAndWaitAsync<T>(
        string requestAddress,
        object[]? args,
        int timeoutMs) where T : IOscParsable<T>
    {
        return SendAndWaitInternalAsync<T>(requestAddress, args, timeoutMs);
    }
    
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
    
    private abstract class BaseCommands
    {
        private readonly ObsbotClient obsbotClient;
        protected BaseCommands(ObsbotClient obsbotClient)
        {
            this.obsbotClient = obsbotClient;
        }
        
        protected Task SendAsync(string address, object[]? args) =>
            obsbotClient.SendAsync(address, args);

        protected Task<T> SendAndWaitAsync<T>(
            string requestAddress,
            object[]? args,
            int timeoutMs) where T : IOscParsable<T> =>
            obsbotClient.SendAndWaitAsync<T>(requestAddress, args, timeoutMs);
    }

    private sealed class GeneralSeries : BaseCommands, IGeneralSeries
    {
        public GeneralSeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }
        
        public async Task SelectDevice(DeviceNumber deviceNumber) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SelectDevice",
                args: [ (int)deviceNumber, ]
            );
        
        public async Task SetZoomAsync(int zoomLevel) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetZoom",
                args: [ zoomLevel ]
            );
        
        public async Task MoveCamaraToLeftAsync(int speed) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalLeft",
                args: [ speed ]
            );
        
        public async Task MoveCamaraToRightAsync(int speed) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalRight",
                args: [ speed ]
            );
        
        public async Task MoveCamaraToUpAsync(int speed) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalUp",
                args: [ speed ]
            );

        public async Task MoveCamaraToDownAsync(int speed) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalDown",
                args: [ speed ]
            );
        
        public async Task SetMirrorAsync(MirrorState mirrorState) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetMirror",
                args: [ (int)mirrorState ]
            );
        
        public async Task StartRecordingPcAsync() =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetPCRecording",
                args: [ 1 ]
            );

        public async Task StopRecordingPcAsync() =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetPCRecording",
                args: [ 0 ]
            );
        
        public async Task TakeScreenshotAsync() =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/PCSnapshot",
                args: [ 1 ]
            );
        
        public async Task SetAutoExposureAsync(AutoExposureType autoExposureType) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoExposure",
                args: [ (int)autoExposureType ]
            );
        
        public async Task SetExposureCompensateAsync(ExposureCompensation exposureCompensation) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetExposureCompensate",
                args: [ (int)exposureCompensation ]
            );
              
        public async Task SetShutterSpeedAsync(ShutterPreset shutterPreset) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetShutterSpeed",
                args: [ shutterPreset.ToDenominator() ]
            );
        
        public async Task SetISOAsync(int isoValue) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetISO",
                args: [ isoValue ]
            );
        
        public async Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoWhiteBalance",
                args: [ (int)whiteBalanceType ]
            );
        
        public async Task SetColorTemperatureAsync(int temperature) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetColorTemperature",
                args: [ 0, temperature ]
            );
        
        public Task<DeviceInfo> GeDeviceInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<DeviceInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetDeviceInfo", 
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );       
        
        public Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<ZoomInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetZoomInfo", 
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<GimbalPosInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetGimbalPosInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }
    
    private sealed class TinySeries : BaseCommands, ITinySeries
    {
        public TinySeries(ObsbotClient obsbotClient) : base(obsbotClient)
        {
        }
        
        public async Task SetAutoFocusAsync(AutoFocusType autofocusType) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoFocus",
                args: [ (int)autofocusType ]
            );
        
        public async Task SetManualFocusAsync(int manualFocusValue) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetManualFocus",
                args: [ manualFocusValue ]
            );
        
        public async Task SelectAITargetStateAsync(AITargetState AITargetState) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Tiny/ToggleAILock",
                args: [ (int)AITargetState ]
            );
        
        public async Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Tiny/TriggerPreset",
                args: [ (int)triggerPreset ]
            );
        
        public async Task SelectAIModeAsync(AIMode AIMode) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Tiny/SetAiMode",
                args: [ (int)AIMode ]
            );
        
        public async Task SelectTrackingModeAsync(TrackingMode trackingMode) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Tiny/SetTrackingMode",
                args: [ (int)trackingMode ]
            );
        
        public Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<AiTrackingInfo>(
                requestAddress: "/OBSBOT/WebCam/Tiny/GetAiTrackingInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<PresetPositionInfo>(
                requestAddress: "/OBSBOT/WebCam/Tiny/GetPresetPositionInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }

    private sealed class TailSeries : BaseCommands, ITailSeries
    {
        private ObsbotClient obsbotClient;
        public TailSeries(ObsbotClient obsbotClient) : base(obsbotClient) => this.obsbotClient = obsbotClient;
        
        public async Task SelectAIModeAsync(TailAirAiMode tailAirAiMode) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetAiMode",
                args: [ (int)tailAirAiMode ]
            );

        public async Task SelectAIModeAsync(Tail2AiMode tailAirAiMode) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetAiMode",
                args: [ (int)tailAirAiMode ]
            );
        
        public async Task SelectTrackingSpeed(TailAirTrackingSpeed tailAirTrackingSpeed) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
                args: [ (int)tailAirTrackingSpeed ]
            );
        
        public async Task SelectTrackingSpeed(Tail2TrackingSpeed tailAirTrackingSpeed) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetTrackingSpeed",
                args: [ (int)tailAirTrackingSpeed ]
            ); 
        
        public async Task SetPanTrackingSpeed(PanAxis panAxis) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetPanTrackingSpeed",
                args: [ (int)panAxis ]
            );

        public async Task SetPanAxisLock(TiltAxis tiltAxis) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetPanAxisLock",
                args: [ (int)tiltAxis ]
            );
        
        public async Task SetTiltAxisLock(int speed) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetTiltAxisLock",
                args: [ speed ]
            );
        
        public async Task StartRecordingAsync() =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetRecording",
                args: [ 1 ]
            );
        
        public async Task StopRecordingAsync() =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/SetRecording",
                args: [ 0 ]
            );
        
        public new async Task TakeScreenshotAsync() =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/Snapshot",
                args: [ 1 ]
            );
        
        public async Task SetTriggerPreset(TriggerPreset triggerPreset) =>
            await SendAsync(
                address: "/OBSBOT/Camera/Tail/TriggerPreset",
                args: [ (int)triggerPreset ]
            );
    }

    private sealed class MeetSeries : BaseCommands, IMeetSeries
    {
        private ObsbotClient obsbotClient;
        public MeetSeries(ObsbotClient obsbotClient) : base(obsbotClient) => this.obsbotClient = obsbotClient;
        
        public async Task SetAutoFocusAsync(AutoFocusType autofocusType) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoFocus",
                args: [ (int)autofocusType ]
            );
        
        public async Task SetManualFocusAsync(int manualFocusValue) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/General/SetManualFocus",
                args: [ manualFocusValue ]
            );
        
        public async Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Meet/SetVirtualBackground",
                args: [ (int)virtualBackground ]
            );
        
        public async Task SetAutoFramingAsync(AutoFramingState autoFramingState) =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Meet/SetAutoFraming",
                args: [ (int)autoFramingState ]
            );
        
        public async Task SetStandardModeAsync() =>
            await SendAsync(
                address: "/OBSBOT/WebCam/Meet/SetStandardMode",
                args: []
            );
        
        public Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<VirtualBackgroundInfo>(
                requestAddress: "/OBSBOT/WebCam/Meet/GetVirtualBackgroundInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0) =>
            SendAndWaitAsync<AutoFramingInfo>(
                requestAddress: "/OBSBOT/WebCam/Meet/GetAutoFramingInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }
}

