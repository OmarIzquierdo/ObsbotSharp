namespace ObsbotSharp.Domain.Base.Models;

/// <summary>
/// Lists the shutter speed presets supported by the general shutter speed command.
/// </summary>
public enum ShutterSpeedPreset
{
    D6400, D5000, D3200, D2500, D2000, D1600, D1250, D1000,
    D800, D640, D500, D400, D320, D240, D200,
    D160, D120, D100, D80, D60, D50, D40, D30, D25, D20, D15,
    D12_5, D10, D8, D6_25, D5, D4, D3, D2_5
}

/// <summary>
/// Helper methods that convert <see cref="ShutterSpeedPreset"/> values to their numeric representation.
/// </summary>
public static class ShutterPresetExtensions
{
    /// <summary>
    /// Converts a <see cref="ShutterSpeedPreset"/> into the denominator used by the OSC command.
    /// </summary>
    /// <param name="speedPreset">Preset to convert.</param>
    /// <returns>Denominator value to be sent to the shutter speed command.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the preset is not recognized.</exception>
    public static double ToDenominator(this ShutterSpeedPreset speedPreset) => speedPreset switch
    {
        ShutterSpeedPreset.D6400 => 6400, ShutterSpeedPreset.D5000 => 5000, ShutterSpeedPreset.D3200 => 3200,
        ShutterSpeedPreset.D2500 => 2500, ShutterSpeedPreset.D2000 => 2000, ShutterSpeedPreset.D1600 => 1600,
        ShutterSpeedPreset.D1250 => 1250, ShutterSpeedPreset.D1000 => 1000, ShutterSpeedPreset.D800  => 800,
        ShutterSpeedPreset.D640  => 640,  ShutterSpeedPreset.D500  => 500,  ShutterSpeedPreset.D400  => 400,
        ShutterSpeedPreset.D320  => 320,  ShutterSpeedPreset.D240  => 240,  ShutterSpeedPreset.D200  => 200,
        ShutterSpeedPreset.D160  => 160,  ShutterSpeedPreset.D120  => 120,  ShutterSpeedPreset.D100  => 100,
        ShutterSpeedPreset.D80   => 80,   ShutterSpeedPreset.D60   => 60,   ShutterSpeedPreset.D50   => 50,
        ShutterSpeedPreset.D40   => 40,   ShutterSpeedPreset.D30   => 30,   ShutterSpeedPreset.D25   => 25,
        ShutterSpeedPreset.D20   => 20,   ShutterSpeedPreset.D15   => 15,   ShutterSpeedPreset.D12_5 => 12.5,
        ShutterSpeedPreset.D10   => 10,   ShutterSpeedPreset.D8    => 8,    ShutterSpeedPreset.D6_25 => 6.25,
        ShutterSpeedPreset.D5    => 5,    ShutterSpeedPreset.D4    => 4,    ShutterSpeedPreset.D3    => 3,
        ShutterSpeedPreset.D2_5  => 2.5,
        _ => throw new ArgumentOutOfRangeException(nameof(speedPreset))
    };

    /// <summary>
    /// Converts a <see cref="ShutterSpeedPreset"/> to its equivalent shutter time in seconds.
    /// </summary>
    /// <param name="p">Preset to convert.</param>
    /// <returns>Shutter speed duration in seconds.</returns>
    public static double ToSeconds(this ShutterSpeedPreset p) => 1.0 / p.ToDenominator();
}