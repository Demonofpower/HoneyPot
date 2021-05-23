using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class StringUtils
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00007AF7 File Offset: 0x00005CF7
	public static bool IsEmpty(string str)
	{
		return str == null || str == string.Empty || str.Trim() == string.Empty;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00038254 File Offset: 0x00036454
	public static string FormatIntWithDigitCount(int val, int digitCount)
	{
		string text = val.ToString();
		while (text.Length < digitCount)
		{
			text = "0" + text;
		}
		return text;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00038288 File Offset: 0x00036488
	public static string FormatIntAsCurrency(int val, bool sign = true)
	{
		string text = string.Empty;
		string text2 = val.ToString();
		int num = 0;
		for (int i = text2.Length - 1; i >= 0; i--)
		{
			text = text2[i] + text;
			num++;
			if (num % 3 == 0 && num != 0 && num < text2.Length)
			{
				text = "," + text;
			}
		}
		if (sign)
		{
			text = "$" + text;
		}
		return text;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00038310 File Offset: 0x00036510
	public static string FormatIntAsTimeCost(int val)
	{
		string text = string.Empty;
		if (val < 60)
		{
			text = val.ToString() + " Minute";
			if (val > 1)
			{
				text += "s";
			}
		}
		else if (val % 60 == 0)
		{
			text = (val / 60).ToString() + " Hour";
			if (val > 60)
			{
				text += "s";
			}
		}
		else
		{
			int num = val % 60;
			int num2 = (val - num) / 60;
			text = num2.ToString() + " Hr";
			if (num2 > 1)
			{
				text += "s";
			}
			text = text + " : " + num.ToString() + " Min";
			if (num > 1)
			{
				text += "s";
			}
		}
		return text;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x000383EC File Offset: 0x000365EC
	public static string Titleize(string str)
	{
		string text = string.Empty;
		string[] array = str.Replace("_", " ").Split(new char[]
		{
			' '
		});
		for (int i = 0; i < array.Length; i++)
		{
			if (i > 0)
			{
				text += " ";
			}
			text = text + array[i].Substring(0, 1).ToUpper() + array[i].Substring(1).ToLower();
		}
		return text;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00007B22 File Offset: 0x00005D22
	public static string Pluralize(string str, int units)
	{
		if (units == 1)
		{
			return str;
		}
		return str + "s";
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00007B38 File Offset: 0x00005D38
	public static string Possessize(string str)
	{
		if (str[str.Length - 1] == 's')
		{
			return str + "'";
		}
		return str + "'s";
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0003846C File Offset: 0x0003666C
	public static string FlattenColorBunches(string str, string defaultColor)
	{
		string text = "^C" + defaultColor + "FF";
		while (str.Contains("[["))
		{
			int startIndex = str.IndexOf("[[");
			str = str.Remove(startIndex, 2);
			int num = str.IndexOf("]", startIndex);
			str = str.Remove(num, 1);
			int num2 = str.IndexOf("]", num);
			str = str.Remove(num2, 1);
			string value = "^C" + ColorUtils.GetKeyColor(str.Substring(num, num2 - num)) + "FF";
			str = str.Remove(num, num2 - num);
			str = str.Insert(num, text);
			str = str.Insert(startIndex, value);
		}
		return text + str;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0003852C File Offset: 0x0003672C
	public static int ParseIntValue(string val)
	{
		int result = 0;
		bool flag = int.TryParse(val.Trim(), out result);
		if (flag)
		{
			return result;
		}
		return 0;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00038554 File Offset: 0x00036754
	public static string IntToString(int val)
	{
		string[] array = new string[]
		{
			"first",
			"second",
			"third",
			"fourth",
			"fifth",
			"sixth",
			"seventh",
			"eighth",
			"ninth",
			"tenth"
		};
		return array[Mathf.Clamp(val, 0, array.Length - 1)];
	}
}
