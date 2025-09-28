using ObsbotSharp.Domain.Base.Models;

namespace ObsbotSharp.Domain.Base.Commands;

internal sealed class BaseSeries : IBaseSeries
{
    private readonly IObsbotCommandGateway gateway;
    public BaseSeries(IObsbotCommandGateway gateway)
    {
        this.gateway = gateway;
    }

    public Task SelectDevice(DeviceNumber deviceNumber) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SelectDevice",
            args: [ (int)deviceNumber] 
        );

    public Task SetZoomAsync(int zoomLevel) => 
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetZoom",
            args: [ zoomLevel ]
        );

    public Task MoveCamaraToLeftAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalLeft",
            args: [ speed ]
        );

    public Task MoveCamaraToRightAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalRight",
            args: [ speed ]
        );

    public Task MoveCamaraToUpAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalUp",
            args: [ speed ]
        );

    public Task MoveCamaraToDownAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalDown",
            args: [ speed ]
        );

    public Task SetMirrorAsync(MirrorState mirrorState) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetMirror",
            args: [ (int)mirrorState ]
        );

    public Task StartRecordingPcAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetPCRecording",
            args: [ 1 ]
        );

    public Task StopRecordingPcAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetPCRecording",
            args: [ 0 ]
        );

    public Task TakeScreenshotAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/PCSnapshot",
            args: [ 1 ]
        );

    public Task SetAutoExposureAsync(AutoExposureType autoExposureType) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoExposure",
            args: [ (int)autoExposureType ]
        );

    public Task SetExposureCompensateAsync(ExposureCompensation exposureCompensation) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetExposureCompensate",
            args: [ (int)exposureCompensation ]
        );

    public Task SetShutterSpeedAsync(ShutterPreset shutterPreset) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetShutterSpeed",
            args: [ shutterPreset.ToDenominator() ]
        );

    public Task SetISOAsync(int isoValue) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetISO",
            args: [ isoValue ]
        );

    public Task SetAutoWhiteBalanceAsync(WhiteBalanceType whiteBalanceType) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoWhiteBalance",
            args: [ (int)whiteBalanceType ]
        );

    public Task SetColorTemperatureAsync(int temperature) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetColorTemperature",
            args: [ 0, temperature ]
        );

    public Task<DeviceInfo> GeDeviceInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<DeviceInfo>(
            requestAddress: "/OBSBOT/WebCam/General/GetDeviceInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<ZoomInfo> GetZoomInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<ZoomInfo>(
            requestAddress: "/OBSBOT/WebCam/General/GetZoomInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<GimbalPosInfo> GetGimbalPosInfoAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<GimbalPosInfo>(
            requestAddress: "/OBSBOT/WebCam/General/GetGimbalPosInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}