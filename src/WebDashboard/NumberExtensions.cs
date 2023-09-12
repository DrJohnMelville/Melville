using System;

namespace Melville.Hacks;

public static class NumberExtensions
{
    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static double Clamp(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));

    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static int Clamp(this int value, int min, int max) => Math.Max(min, Math.Min(max, value));

    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this double value, double min, double max) => (min <= value) && (value <= max);

    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this int value, int min, int max) => (min <= value) && (value <= max);


}