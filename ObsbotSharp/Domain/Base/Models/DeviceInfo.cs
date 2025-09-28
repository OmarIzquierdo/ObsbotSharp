using CoreOSC;
using ObsbotSharp.Infrastructure.Osc.Parsers;

namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Represents the parsed payload returned by the general device information response message.
/// </summary>
/// <param name="Device">Collection of devices reported by the server.</param>
/// <param name="CurrentSelectedDevice">Index of the currently selected device.</param>
/// <param name="CurrentDeviceState">Operational state of the selected device.</param>
public record DeviceInfo(
    List<Device> Device,
    int CurrentSelectedDevice,
    CurrentDeviceState CurrentDeviceState) : IOscParsable<DeviceInfo>
{
    /// <inheritdoc />
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/DeviceInfo"
    ];

    /// <summary>
    /// Parses an OSC message that follows the format described in the OBSBOT OSC specification.
    /// </summary>
    /// <param name="message">OSC message received from the device.</param>
    /// <returns>A populated <see cref="DeviceInfo"/> instance.</returns>
    /// <exception cref="FormatException">Thrown when the message does not include the expected number of arguments.</exception>
    public static DeviceInfo Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 7)
            throw new FormatException($"DeviceInfo expected 7 arguments, but received {message.Arguments.Count()}.");

        return new DeviceInfo(
            Device: GetDeviceNameAndConnectionState(message), 
            CurrentSelectedDevice: Convert.ToInt32(message.Arguments.ElementAt(8)),
            CurrentDeviceState:  (CurrentDeviceState)Convert.ToInt32(message.Arguments.ElementAt(9))
        );
    }

    private static List<Device> GetDeviceNameAndConnectionState(OscMessage message)
    {
        List<Device> devices = new List<Device>();

        for (int iterator = 0; iterator <= 7; iterator += 2)
        {
            var currentDeviceConnectionState = (CurrentDeviceConnectionState)message.Arguments.ElementAt(iterator);
            var currentDeviceName = message.Arguments.ElementAt(iterator + 1).ToString();
            
            if (string.IsNullOrWhiteSpace(currentDeviceName))
            {
                currentDeviceName = "Not device assigned";
            }
            
            devices.Add(new Device(currentDeviceName, currentDeviceConnectionState, (DeviceNumber)(iterator / 2)));
        }
        
        return devices;
    }
}

/// <summary>
/// Represents a device slot returned by the OBSBOT OSC service.
/// </summary>
/// <param name="CurrentName">Friendly name of the device or a placeholder when none is assigned.</param>
/// <param name="CurrentDeviceConnectionState">Connection state.</param>
/// <param name="DeviceNumber">Logical device index.</param>
public record Device(string CurrentName, CurrentDeviceConnectionState CurrentDeviceConnectionState, DeviceNumber DeviceNumber);

/// <summary>
/// Identifies the logical device slots exposed by OBSBOT webcams.
/// </summary>
public enum DeviceNumber
{
    /// <summary>First device slot (<c>0</c> when sent through OSC).</summary>
    Device1,
    /// <summary>Second device slot (<c>1</c>).</summary>
    Device2,
    /// <summary>Third device slot (<c>2</c>).</summary>
    Device3,
    /// <summary>Fourth device slot (<c>3</c>).</summary>
    Device4,
}

/// <summary>
/// Describes whether a device slot is connected or disconnected.
/// </summary>
public enum CurrentDeviceConnectionState
{
    /// <summary>Device is disconnected.</summary>
    Disconnected,
    /// <summary>Device is connected.</summary>
    Connected,
}

/// <summary>
/// Indicates if the currently selected device is sleeping or running.
/// </summary>
public enum CurrentDeviceState
{
    /// <summary>Device is in sleep mode.</summary>
    Sleep,
    /// <summary>Device is running.</summary>
    Run
}
