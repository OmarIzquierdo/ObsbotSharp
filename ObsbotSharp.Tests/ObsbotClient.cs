using System.Net.Sockets;
using CoreOSC;
using CoreOSC.IO;
using ObsbotSharp.Tests.Models;
using ObsbotSharp.Tests.Models.General;
using ObsbotSharp.Tests.Models.MeetSeries;
using ObsbotSharp.Tests.Models.TinySeries;

namespace ObsbotSharp.Tests;

public class ObsbotClient : IDisposable
{
    private readonly UdpClient udpClient;
    public TinySeries Tiny { get; }
    public TailSeries Tail { get; }
    public MeetSeries Meet { get; }
    public GeneralSeries General { get; }

    public ObsbotClient(ObsbotOptions options)
    {
        udpClient = new UdpClient(options.LocalPort);
        udpClient.Connect(options.Host, options.RemotePort);

        General = new GeneralSeries(this);
        Tiny    = new TinySeries(this);
        Tail    = new TailSeries(this);
        Meet    = new MeetSeries(this);
    }
    
    private static readonly string[] NoiseAddresses =
    [
        "/OBSBOT/WebCam/General/ConnectedResp"
    ];
    
    private Task SendAsync(string address, params object[]? args) =>
        udpClient.SendMessageAsync(new OscMessage(new Address(address), args ?? []));
    
    private async Task<T> SendAndWaitAsync<T>(
        string requestAddress, 
        object[]? args, 
        int timeoutMs) where T : IOscParsable<T>
    {
        await SendAsync(requestAddress, args ?? []);
        return await WaitForAsync<T>(timeoutMs);
    }
    
    private async Task<T> WaitForAsync<T>(int timeoutMs) where T : IOscParsable<T>
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (true)
        {
            var remaining = (int)Math.Max(1, (deadline - DateTime.UtcNow).TotalMilliseconds);
            var oscMessage = await ReceiveWithTimeoutAsync(remaining);

            if (NoiseAddresses.Any(address => oscMessage.Address.Value.Equals(address, StringComparison.OrdinalIgnoreCase)))
                continue;

            if (T.ReplyAddresses.Any(address => oscMessage.Address.Value.Equals(address, StringComparison.OrdinalIgnoreCase)))
                return T.Parse(oscMessage);
        }
    }
    
    private async Task<OscMessage> ReceiveWithTimeoutAsync(int timeoutMs)
    {
        var messageResponse = udpClient.ReceiveMessageAsync();
        var taskCompleted = await Task.WhenAny(messageResponse, Task.Delay(timeoutMs));
        
        if (taskCompleted != messageResponse) 
            throw new TimeoutException($"Timeout OSC ({timeoutMs} ms).");
        
        return await messageResponse;
    }
    public void Dispose() => udpClient.Dispose();
    
    public class GeneralSeries
    {
        private ObsbotClient obsbotClient;
        public GeneralSeries(ObsbotClient obsbotClient) => this.obsbotClient = obsbotClient;
        
        public async Task SetZoomAsync(int zoomLevel) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetZoom",
                args: [ zoomLevel ]
            );
        
        public async Task MoveCamaraToLeftAsync(int speed) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalLeft",
                args: [ speed ]
            );
        
        public async Task MoveCamaraToRightAsync(int speed) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalRight",
                args: [ speed ]
            );
        
        public async Task MoveCamaraToUpAsync(int speed) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalUp",
                args: [ speed ]
            );

        public async Task MoveCamaraToDownAsync(int speed) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetGimbalDown",
                args: [ speed ]
            );
        
        public async Task SetMirrorAsync(MirrorState mirrorState) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetMirror",
                args: [ (int)mirrorState ]
            );
        
        public async Task StartRecordingPcAsync() =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetPCRecording",
                args: [ 1 ]
            );

        public async Task StopRecordingPcAsync() =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetPCRecording",
                args: [ 0 ]
            );
        
        public async Task TakeScreenshotAsync() =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/PCSnapshot",
                args: [ 1 ]
            );
        
        public async Task SetAutoFocusAsync(AutoFocusType autofocusType) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoFocus",
                args: [ (int)autofocusType ]
            );
        
        public async Task SetManualFocusAsync(int manualFocusValue) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetManualFocus",
                args: [ manualFocusValue ]
            );
        
        public async Task SetAutoExposureAsync(AutoExposureType autoExposureType) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoExposure",
                args: [ (int)autoExposureType ]
            );
        
        public async Task SetExposureCompensateAsync(ExposureCompensation exposureCompensation) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetExposureCompensate",
                args: [ (int)exposureCompensation ]
            );
              
        public async Task SetShutterSpeedAsync(ShutterPreset shutterPreset) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetShutterSpeed",
                args: [ shutterPreset.ToDenominator() ]
            );
        
        public async Task SetISOAsync(int isoValue) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetISO",
                args: [ isoValue ]
            );
        
        public async Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetAutoWhiteBalance",
                args: [ (int)whiteBalanceType ]
            );
        
        public async Task SetColorTemperatureAsync(int temperature) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/General/SetColorTemperature",
                args: [ 0, temperature ]
            );
        
        public Task<DeviceInfo> GeDeviceInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<DeviceInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetDeviceInfo", 
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );       
        
        public Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<ZoomInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetZoomInfo", 
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<GimbalPosInfo>(
                requestAddress: "/OBSBOT/WebCam/General/GetGimbalPosInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }
    
    public class TinySeries
    {
        private ObsbotClient obsbotClient;
        public TinySeries (ObsbotClient obsbotClient) => this.obsbotClient = obsbotClient;
        
        public async Task SelectAITargetStateAsync(AITargetState AITargetState) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Tiny/ToggleAILock",
                args: [ (int)AITargetState ]
            );
        
        public async Task SelectTriggerPresetPositionAsync(TriggerPreset triggerPreset) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Tiny/TriggerPreset",
                args: [ (int)triggerPreset ]
            );
        
        public async Task SelectAIModeAsync(AIMode AIMode) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Tiny/SetAiMode",
                args: [ (int)AIMode ]
            );
        
        public async Task SelectTrackingModeAsync(TrackingMode trackingMode) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Tiny/SetTrackingMode",
                args: [ (int)trackingMode ]
            );
        
        public Task<AiTrackingInfo> GetAiTrackingInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<AiTrackingInfo>(
                requestAddress: "/OBSBOT/WebCam/Tiny/GetAiTrackingInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<PresetPositionInfo> GetPresetPositionInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<PresetPositionInfo>(
                requestAddress: "/OBSBOT/WebCam/Tiny/GetPresetPositionInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }

    public class TailSeries
    {
        private ObsbotClient obsbotClient;
        public TailSeries(ObsbotClient obsbotClient) => this.obsbotClient = obsbotClient;
        
        public async Task SetVirtualBackgroundAsync(VirtualBackgroundState virtualBackground) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Meet/SetVirtualBackground",
                args: [ (int)virtualBackground ]
            );
        
        public async Task SetAutoFramingAsync(AutoFramingState autoFramingState) =>
            await obsbotClient.SendAsync(
                address: "/OBSBOT/WebCam/Meet/SetAutoFraming",
                args: [ (int)autoFramingState ]
            );
        
        public async Task SetStandardModeAsync() =>
            await obsbotClient.SendAsync(address: "/OBSBOT/WebCam/Meet/SetStandardMode");
    }

    public class MeetSeries
    {
        private ObsbotClient obsbotClient;
        public MeetSeries(ObsbotClient obsbotClient) => this.obsbotClient = obsbotClient;
        
        public Task<VirtualBackgroundInfo> GetVirtualBackgroundInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<VirtualBackgroundInfo>(
                requestAddress: "/OBSBOT/WebCam/Meet/GetVirtualBackgroundInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
        
        public Task<AutoFramingInfo> GetAutoFramingInfoAsync(int deviceIndex = 0) =>
            obsbotClient.SendAndWaitAsync<AutoFramingInfo>(
                requestAddress: "/OBSBOT/WebCam/Meet/GetAutoFramingInfo",
                args: [ deviceIndex ], 
                timeoutMs: 2000
            );
    }
}



