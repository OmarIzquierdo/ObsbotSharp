using ObsbotSharp.Domain.Base.Models;

namespace ObsbotSharp.Domain.Base;

/// <summary>
/// Defines the shared set of OSC commands available to all OBSBOT webcams as documented in the
/// general command group.
/// </summary>
public interface IBaseSeries
{
    /// <summary>
    /// Selects the target device that subsequent commands act upon.
    /// </summary>
    /// <param name="deviceSlot">Identifier of the device slot to target (0..3).</param>
    Task SelectDevice(DeviceSlot deviceSlot);

    /// <summary>
    /// Sets the zoom level for the active device.
    /// </summary>
    /// <param name="zoomLevel">Value between 0 and 100 representing the zoom percentage.</param>
    Task SetZoomAsync(int zoomLevel);

    /// <summary>
    /// Starts or stops the gimbal pan movement to the left.
    /// </summary>
    /// <param name="speed">Value between 0 and 100 where 0 stops the movement.</param>
    Task MoveCamaraLeftAsync(int speed);

    /// <summary>
    /// Starts or stops the gimbal pan movement to the right.
    /// </summary>
    /// <param name="speed">Value between 0 and 100 where 0 stops the movement.</param>
    Task MoveCamaraRightAsync(int speed);

    /// <summary>
    /// Starts or stops the gimbal tilt movement up.
    /// </summary>
    /// <param name="speed">Value between 0 and 100 where 0 stops the movement.</param>
    Task MoveCamaraUpAsync(int speed);

    /// <summary>
    /// Starts or stops the gimbal tilt movement down.
    /// </summary>
    /// <param name="speed">Value between 0 and 100 where 0 stops the movement.</param>
    Task MoveCamaraDownAsync(int speed);

    /// <summary>
    /// Enables or disables image mirroring.
    /// </summary>
    /// <param name="mirrorMode">Desired mirror state.</param>
    Task SelectMirrorModeAsync(MirrorMode mirrorMode);

    /// <summary>
    /// Starts the OBSBOT WebCam PC recording feature.
    /// </summary>
    Task StartRecordingAsync();

    /// <summary>
    /// Stops the OBSBOT WebCam PC recording feature.
    /// </summary>
    Task StopRecordingAsync();

    /// <summary>
    /// Captures a snapshot from the current video output.
    /// </summary>
    Task TakeSnapshootAsync();

    /// <summary>
    /// Configures exposure mode for the active device.
    /// </summary>
    /// <param name="autoExposureMode">Exposure mode to use.</param>
    Task SelectAutoExposureAsync(AutoExposureMode autoExposureMode);

    /// <summary>
    /// Applies exposure compensation.
    /// </summary>
    /// <param name="exposureCompensation">Compensation preset (-30 .. 30).</param>
    Task SelectExposureCompensateAsync(ExposureCompensation exposureCompensation);

    /// <summary>
    /// Sets the shutter speed using the denominators defined by OBSBOT.
    /// </summary>
    /// <param name="shutterSpeedPreset">Preset that maps to the desired denominator.</param>
    Task SelectShutterSpeedAsync(ShutterSpeedPreset shutterSpeedPreset);

    /// <summary>
    /// Sets the ISO value for the current device.
    /// </summary>
    /// <param name="isoValue">ISO value supported by the device (100..6400).</param>
    Task SetISOAsync(int isoValue);

    /// <summary>
    /// Switches between manual and automatic white balance.
    /// </summary>
    /// <param name="whiteBalanceMode">Requested white balance mode.</param>
    Task SelectAutoWhiteBalanceAsync(WhiteBalanceMode whiteBalanceMode);

    /// <summary>
    /// Sets the color temperature (Kelvin) for manual white balance control.
    /// </summary>
    /// <param name="temperature">Temperature value between 2800 and 6500.</param>
    Task SetColorTemperatureAsync(int temperature);

    /// <summary>
    /// Requests device information and parses the response payload.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<DeviceResponse> GeDeviceResponseAsync(int deviceIndex = 0);

    /// <summary>
    /// Requests zoom information and parses the response payload.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<ZoomStatus> GetZoomStatusAsync(int deviceIndex = 0);

    /// <summary>
    /// Requests the gimbal position and parses the response payload.
    /// </summary>
    /// <param name="deviceIndex">Optional device index to query.</param>
    Task<GimbalPosition> GetGimbalPositionAsync(int deviceIndex = 0);
}