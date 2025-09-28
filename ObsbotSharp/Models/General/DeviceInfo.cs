using CoreOSC;

namespace ObsbotSharp.Models.General;

public record DeviceInfo(
    List<Device> Device,
    int CurrentSelectedDevice,
    CurrentDeviceState CurrentDeviceState) : IOscParsable<DeviceInfo>
{
    public static string[] ReplyAddresses =>
    [
        "/OBSBOT/WebCam/General/DeviceInfo"
    ];
    
    public static DeviceInfo Parse(OscMessage message)
    {
        if (message.Arguments.Count() < 7) 
            throw new FormatException("DeviceInfo espera 7 args.");

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

public record Device(string CurrentName, CurrentDeviceConnectionState CurrentDeviceConnectionState, DeviceNumber DeviceNumber);

public enum DeviceNumber
{
    Device1,
    Device2,
    Device3,
    Device4,
}

public enum CurrentDeviceConnectionState
{
    Disconnected,
    Connected,
}

public enum CurrentDeviceState
{
    Sleep,
    Run
}
