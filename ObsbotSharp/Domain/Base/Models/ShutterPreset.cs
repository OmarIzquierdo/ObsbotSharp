namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Lists the shutter speed presets supported by the general shutter speed command.
/// </summary>
public enum ShutterPreset
{
    D6400, D5000, D3200, D2500, D2000, D1600, D1250, D1000,
    D800, D640, D500, D400, D320, D240, D200,
    D160, D120, D100, D80, D60, D50, D40, D30, D25, D20, D15,
    D12_5, D10, D8, D6_25, D5, D4, D3, D2_5
}

/// <summary>
/// Helper methods that convert <see cref="ShutterPreset"/> values to their numeric representation.
/// </summary>
public static class ShutterPresetExtensions
{
    /// <summary>
    /// Converts a <see cref="ShutterPreset"/> into the denominator used by the OSC command.
    /// </summary>
    /// <param name="preset">Preset to convert.</param>
    /// <returns>Denominator value to be sent to the shutter speed command.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the preset is not recognized.</exception>
    public static double ToDenominator(this ShutterPreset preset) => preset switch
    {
        ShutterPreset.D6400 => 6400, ShutterPreset.D5000 => 5000, ShutterPreset.D3200 => 3200,
        ShutterPreset.D2500 => 2500, ShutterPreset.D2000 => 2000, ShutterPreset.D1600 => 1600,
        ShutterPreset.D1250 => 1250, ShutterPreset.D1000 => 1000, ShutterPreset.D800  => 800,
        ShutterPreset.D640  => 640,  ShutterPreset.D500  => 500,  ShutterPreset.D400  => 400,
        ShutterPreset.D320  => 320,  ShutterPreset.D240  => 240,  ShutterPreset.D200  => 200,
        ShutterPreset.D160  => 160,  ShutterPreset.D120  => 120,  ShutterPreset.D100  => 100,
        ShutterPreset.D80   => 80,   ShutterPreset.D60   => 60,   ShutterPreset.D50   => 50,
        ShutterPreset.D40   => 40,   ShutterPreset.D30   => 30,   ShutterPreset.D25   => 25,
        ShutterPreset.D20   => 20,   ShutterPreset.D15   => 15,   ShutterPreset.D12_5 => 12.5,
        ShutterPreset.D10   => 10,   ShutterPreset.D8    => 8,    ShutterPreset.D6_25 => 6.25,
        ShutterPreset.D5    => 5,    ShutterPreset.D4    => 4,    ShutterPreset.D3    => 3,
        ShutterPreset.D2_5  => 2.5,
        _ => throw new ArgumentOutOfRangeException(nameof(preset))
    };

    /// <summary>
    /// Converts a <see cref="ShutterPreset"/> to its equivalent shutter time in seconds.
    /// </summary>
    /// <param name="p">Preset to convert.</param>
    /// <returns>Shutter speed duration in seconds.</returns>
    public static double ToSeconds(this ShutterPreset p) => 1.0 / p.ToDenominator();
}