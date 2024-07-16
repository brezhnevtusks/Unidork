using System;
using UnityEngine;

namespace Unidork.Extensions
{
    public static class ColorExtensions
    {
        /// <summary> Converts a hexadecimal value to a Unity Color.</summary>
        /// <param name="target">Color.</param>
        /// <param name="hex">Hexadecimal value.</param>
        /// <returns>
        /// A color matching the passed hexadecimal value.
        /// </returns>
        public static Color FromHex(this Color target, string hex)
        {
            if (hex[0] == '#')
            {
                hex = hex.Substring(1);
            }

            if (hex.Length != 6)
            {
                throw new Exception($"Invalid hex string: {hex}");
            }

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            
            target.r = r / 255f;
            target.g = g / 255f;
            target.b = b / 255f;
            target.a = 1f;

            return target;
        }

        /// <summary>Gets the hexadecimal value of a color.</summary>
        /// <param name="target">Color.</param>
        /// <returns>
        /// A string representing the hexadecimal value of the passed color.
        /// </returns>
        public static string ToHex(this Color target)
        {
            int r = (int)(target.r * 255);
            int g = (int)(target.g * 255);
            int b = (int)(target.b * 255);
            
            return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }
    }
}