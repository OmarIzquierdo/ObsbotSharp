# ObsbotSharp

ObsbotSharp is an unofficial .NET library that lets you control OBSBOT cameras through their OSC (Open Sound Control) interface.
The package wraps the raw OSC messages that the devices expect and exposes strongly-typed helpers for the Tiny, Tail, Meet and
others webcam series.

> **Disclaimer:** ObsbotSharp is a community project and is not affiliated with, endorsed by, or supported by OBSBOT.

## Installation

ObsbotSharp is distributed as a NuGet package targeting .NET 9.0 and newer. Install it with the tool of your choice:

```bash
# .NET CLI
dotnet add package ObsbotSharp
```

```powershell
# Package Manager Console
Install-Package ObsbotSharp
```

```xml
<!-- Project file -->
<ItemGroup>
  <PackageReference Include="ObsbotSharp" Version="1.0.0" />
</ItemGroup>
```

## Connecting to a camera

OBSBOT cameras expose an OSC server that listens on a UDP port (16284 by default). Use `ObsbotOptions` to configure the endpoint
and create an `ObsbotClient`:

```csharp
using ObsbotSharp;
using ObsbotSharp.Domain.Base.Models;

var options = new ObsbotOptions()
    .WithHost("192.168.1.50")    // Camera IP address
    .WithRemotePort(16284)        // OSC port exposed by the camera
    .WithLocalPort(12000);        // UDP port used by your application

using IObsbotClient client = new ObsbotClient(options);

await client.General.SelectDevice(DeviceSlot.Device1);
```

The default `ObsbotClient` talks to the camera over UDP, but you can supply custom transport by implementing `IOscTransport` if
you need to tunnel through another medium.

## Retrieving device information

You can query the camera for structured data. Each method exposes typed models that map to OBSBOT's OSC responses.

```csharp
// Device metadata and connection state
var deviceInfo = await client.General.GeDeviceResponseAsync();
Console.WriteLine($"Selected device: {deviceInfo.CurrentSelectedDevice}");
foreach (var device in deviceInfo.Device)
{
    Console.WriteLine($"{device.Slot}: {device.Name} ({device.DeviceConnectionStatus})");
}

// Current zoom level
var zoomStatus = await client.General.GetZoomStatusAsync();
Console.WriteLine($"Zoom ratio: {zoomInfo.CurrentZoomValue}/{zoomInfo.MaximumZoomValue}");

// AI tracking info (Tiny series)
var tracking = await client.Tiny.GetAiTrackingStatusAsync();
Console.WriteLine($"Tracking state: {tracking.AiTrackingState}");
```

## Performing camera actions

ObsbotSharp contains helpers for each product family. The following snippets highlight some of the most common operations:

```csharp
// General webcam controls
await client.General.SetZoomAsync(150);                                     // Zoom level (0-1000)
await client.General.MoveCamaraLeftAsync(10);                               // Pan left at speed 10
await client.General.SelectAutoExposureModeAsync(AutoExposureMode.Auto);    // Select ExposureMode
await client.General.TakeSnapshotAsync();                                   // Trigger a snapshot on the host PC

// Tiny series (AI-tracking webcams)
await client.Tiny.SelectAiModeAsync(AIMode.NormalTracking);
await client.Tiny.SelectTrackingModeAsync(TrackingMode.Headroom);
await client.Tiny.SelectTriggerPresetPositionModeAsync(TriggerPresetMode.PresetPositionOne);

// Tail series (PTZ camera)
await client.Tail.SelectAiModeAsync(Tail2AiTrackingMode.HumanTrackingSingleMode);
await client.Tail.SelectTrackingSpeedModeAsync(Tail2TrackingSpeedMode.Fast);
await client.Tail.StartRecordingAsync();

// Meet series (conference camera)
await client.Meet.SelectVirtualBackgroundModeAsync(VirtualBackgroundMode.Blur);
await client.Meet.SelectAutoFramingModeAsync(AutoFramingMode.SingleMode);
```

All commands are asynchronous and return `Task`, so they can be awaited or combined with `Task.WhenAll` depending on your application flow.

## Error handling and timeouts

Calls that expect a reply (for example, `GeDeviceResponseAsync`) wait up to two seconds for the response before timing out. You can
wrap calls in your own cancellation/timeout logic if you need a different behavior. Commands that do not return data simply fire the OSC message and complete when the payload is sent.

## Samples and best practices

* Pair ObsbotSharp with dependency injection by registering `ObsbotClient` as a singleton so all components share the same UDP socket.
* Always select the target device (`General.SelectDevice`) before issuing product-specific commands when multiple cameras are connected.

## License

ObsbotSharp is released under the MIT license. You are free to use the library in commercial and non-commercial projects as long as you credit the original authors.