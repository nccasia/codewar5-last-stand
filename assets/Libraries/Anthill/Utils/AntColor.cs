using UnityEngine;

namespace Anthill.Utils
{
	public class AntColor
	{
		/// <summary>
		/// Converts a Color value to Hex format.
		/// </summary>
		/// <param name="aColor">Color value.</param>
		/// <returns>Returns the Hex code of the color in string format.</returns>
		public static string ColorToHex(Color aColor)
		{
			return aColor.r.ToString("X2") + aColor.g.ToString("X2") + aColor.b.ToString("X2");
		}

		/// <summary>
		/// Converts a Hex value to Color format.
		/// </summary>
		/// <param name="aHex">Hex color code in string format.</param>
		/// <returns>Returns a Color value.</returns>
		public static Color HexToColor(string aHex)
		{
			aHex = aHex.Replace("0x", "");
			aHex = aHex.Replace("#", "");

			byte a = 255;
			byte r = byte.Parse(aHex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(aHex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(aHex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

			if (aHex.Length == 8)
			{
				a = byte.Parse(aHex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			}

			return new Color(r, g, b, a);
		}
	}
}