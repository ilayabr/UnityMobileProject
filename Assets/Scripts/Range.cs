using System;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public struct Range : IEquatable<Range>, IFormattable
{
    public float min;
    public float max;
    public Range(float _min, float _max) => (min, max) = (_min, _max);

    public static readonly Range range01 = new Range(0f, 1f);

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
}