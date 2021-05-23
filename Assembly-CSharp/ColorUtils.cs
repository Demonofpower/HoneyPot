using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class ColorUtils
{
	// Token: 0x0600076B RID: 1899 RVA: 0x000079C5 File Offset: 0x00005BC5
	public static string GetKeyColor(string colorKey)
	{
		if (ColorUtils._keyColors.ContainsKey(colorKey))
		{
			return ColorUtils._keyColors[colorKey];
		}
		return "FFFFFF";
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000376F8 File Offset: 0x000358F8
	public static string ColorToHex(Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00037740 File Offset: 0x00035940
	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}

	// Token: 0x0400089D RID: 2205
	private static readonly Dictionary<string, string> _keyColors = new Dictionary<string, string>
	{
		{
			"talent",
			"205C9E"
		},
		{
			"flirtation",
			"4A7521"
		},
		{
			"romance",
			"B05C1E"
		},
		{
			"sexuality",
			"991922"
		},
		{
			"heart",
			"BA4992"
		},
		{
			"broken",
			"5E2782"
		},
		{
			"joy",
			"A58537"
		},
		{
			"sentiment",
			"337D91"
		},
		{
			"hunie",
			"C64B91"
		},
		{
			"go",
			"3E89AF"
		},
		{
			"stop",
			"BC3939"
		}
	};
}
