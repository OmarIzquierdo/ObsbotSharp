using ObsbotSharp.Domain.Base.Models;

namespace ObsbotSharp.Domain.Base.Commands;

internal sealed class BaseSeries(IObsbotCommandGateway gateway) : IBaseSeries
{
    private readonly IObsbotCommandGateway gateway = gateway;

    public Task SelectDevice(DeviceSlot deviceSlot) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SelectDevice",
            args: [ (int)deviceSlot ] 
        );

    public Task SetZoomAsync(int zoomLevel) => 
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetZoom",
            args: [ zoomLevel ]
        );

    public Task MoveCamaraLeftAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalLeft",
            args: [ speed ]
        );

    public Task MoveCamaraRightAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalRight",
            args: [ speed ]
        );

    public Task MoveCamaraUpAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalUp",
            args: [ speed ]
        );

    public Task MoveCamaraDownAsync(int speed) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetGimbalDown",
            args: [ speed ]
        );

    public Task SelectMirrorModeAsync(MirrorMode mirrorMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetMirror",
            args: [ (int)mirrorMode ]
        );

    public Task StartRecordingAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetPCRecording",
            args: [ 1 ]
        );

    public Task StopRecordingAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetPCRecording",
            args: [ 0 ]
        );

    public Task TakeSnapshotAsync() =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/PCSnapshot",
            args: [ 1 ]
        );

    public Task SelectAutoExposureAsync(AutoExposureMode autoExposureMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoExposure",
            args: [ (int)autoExposureMode ]
        );

    public Task SelectExposureCompensateAsync(ExposureCompensation exposureCompensation) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetExposureCompensate",
            args: [ (int)exposureCompensation ]
        );

    public Task SelectShutterSpeedAsync(ShutterSpeedPreset shutterSpeedPreset) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetShutterSpeed",
            args: [ shutterSpeedPreset.ToDenominator() ]
        );

    public Task SetIsoAsync(int isoValue) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetISO",
            args: [ isoValue ]
        );

    public Task SelectAutoWhiteBalanceAsync(WhiteBalanceMode whiteBalanceMode) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetAutoWhiteBalance",
            args: [ (int)whiteBalanceMode ]
        );

    public Task SetColorTemperatureAsync(int temperature) =>
        gateway.SendAsync(
            address: "/OBSBOT/WebCam/General/SetColorTemperature",
            args: [ 0, temperature ]
        );

    public Task<DeviceResponse> GeDeviceResponseAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<DeviceResponse>(
            requestAddress: "/OBSBOT/WebCam/General/GetDeviceInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<ZoomStatus> GetZoomStatusAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<ZoomStatus>(
            requestAddress: "/OBSBOT/WebCam/General/GetZoomInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );

    public Task<GimbalPosition> GetGimbalPositionAsync(int deviceIndex = 0) =>
        gateway.SendAndWaitAsync<GimbalPosition>(
            requestAddress: "/OBSBOT/WebCam/General/GetGimbalPosInfo",
            args: [ deviceIndex ],
            timeoutMs: 2000
        );
}