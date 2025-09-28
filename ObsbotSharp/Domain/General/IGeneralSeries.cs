using ObsbotSharp.Domain.General.Models;

namespace ObsbotSharp.Domain.General;

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