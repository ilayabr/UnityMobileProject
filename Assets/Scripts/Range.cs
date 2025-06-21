using System;
using System.Globalization;
using UnityEngine;

/// <summary>
/// A class to define a range between 2 floating point values
/// </summary>
[System.Serializable]
public struct Range : IEquatable<Range>, IFormattable
{
    public float min;
    public float max;
    public Range(float _min, float _max) => (min, max) = (_min, _max);

    /// <summary>
    /// a range between 0 and 1
    /// </summary>
    public static readonly Range r01 = new Range(0f, 1f);

    public bool Equals(Range other) => this.min.Equals(other.min) && this.max.Equals(other.max);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "F2";
        }

        if (formatProvider == null)
        {
            formatProvider = CultureInfo.InvariantCulture.NumberFormat;
        }

        return $"({min.ToString(format, formatProvider)}, {max.ToString(format, formatProvider)})";
    }

    /// <summary>
    /// returns a random value between this ranges min and max values.
    /// </summary>
    public float GetRandom() => UnityEngine.Random.Range(min, max);
    public float Clamp(float value) => Mathf.Clamp(value, min, max);
    public float To01(float value) => value / max;
    public float Evaluate(float value01) => min + value01 * (max - min);
}