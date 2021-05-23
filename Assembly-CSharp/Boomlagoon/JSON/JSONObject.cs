using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Boomlagoon.JSON
{
	// Token: 0x02000007 RID: 7
	public class JSONObject : IEnumerable<KeyValuePair<string, JSONValue>>, IEnumerable
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002798 File Offset: 0x00000998
		public JSONObject()
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000BB4C File Offset: 0x00009D4C
		public JSONObject(JSONObject other)
		{
			this.values = new Dictionary<string, JSONValue>();
			if (other != null)
			{
				foreach (KeyValuePair<string, JSONValue> keyValuePair in other.values)
				{
					this.values[keyValuePair.Key] = new JSONValue(keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000027C7 File Offset: 0x000009C7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool ContainsKey(string key)
		{
			return this.values.ContainsKey(key);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000BBDC File Offset: 0x00009DDC
		public JSONValue GetValue(string key)
		{
			JSONValue result;
			this.values.TryGetValue(key, out result);
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public string GetString(string key)
		{
			JSONValue value = this.GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + "(string) == null");
				return string.Empty;
			}
			return value.Str;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000BC34 File Offset: 0x00009E34
		public double GetNumber(string key)
		{
			JSONValue value = this.GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return double.NaN;
			}
			return value.Number;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000BC70 File Offset: 0x00009E70
		public JSONObject GetObject(string key)
		{
			JSONValue value = this.GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return null;
			}
			return value.Obj;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000BCA4 File Offset: 0x00009EA4
		public bool GetBoolean(string key)
		{
			JSONValue value = this.GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return false;
			}
			return value.Boolean;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		public JSONArray GetArray(string key)
		{
			JSONValue value = this.GetValue(key);
			if (value == null)
			{
				JSONLogger.Error(key + " == null");
				return null;
			}
			return value.Array;
		}

		// Token: 0x1700000A RID: 10
		public JSONValue this[string key]
		{
			get
			{
				return this.GetValue(key);
			}
			set
			{
				this.values[key] = value;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000027EB File Offset: 0x000009EB
		public void Add(string key, JSONValue value)
		{
			this.values[key] = value;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000027FA File Offset: 0x000009FA
		public void Add(KeyValuePair<string, JSONValue> pair)
		{
			this.values[pair.Key] = pair.Value;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public static JSONObject Parse(string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return null;
			}
			JSONValue jsonvalue = null;
			List<string> list = new List<string>();
			JSONObject.JSONParsingState jsonparsingState = JSONObject.JSONParsingState.Object;
			for (int i = 0; i < jsonString.Length; i++)
			{
				i = JSONObject.SkipWhitespace(jsonString, i);
				switch (jsonparsingState)
				{
				case JSONObject.JSONParsingState.Object:
				{
					if (jsonString[i] != '{')
					{
						return JSONObject.Fail('{', i);
					}
					JSONValue jsonvalue2 = new JSONObject();
					if (jsonvalue != null)
					{
						jsonvalue2.Parent = jsonvalue;
					}
					jsonvalue = jsonvalue2;
					jsonparsingState = JSONObject.JSONParsingState.Key;
					break;
				}
				case JSONObject.JSONParsingState.Array:
				{
					if (jsonString[i] != '[')
					{
						return JSONObject.Fail('[', i);
					}
					JSONValue jsonvalue3 = new JSONArray();
					if (jsonvalue != null)
					{
						jsonvalue3.Parent = jsonvalue;
					}
					jsonvalue = jsonvalue3;
					jsonparsingState = JSONObject.JSONParsingState.Value;
					break;
				}
				case JSONObject.JSONParsingState.EndObject:
				{
					if (jsonString[i] != '}')
					{
						return JSONObject.Fail('}', i);
					}
					if (jsonvalue.Parent == null)
					{
						return jsonvalue.Obj;
					}
					JSONValueType type = jsonvalue.Parent.Type;
					if (type != JSONValueType.Object)
					{
						if (type != JSONValueType.Array)
						{
							return JSONObject.Fail("valid object", i);
						}
						jsonvalue.Parent.Array.Add(new JSONValue(jsonvalue.Obj));
					}
					else
					{
						jsonvalue.Parent.Obj.values[list.Pop<string>()] = new JSONValue(jsonvalue.Obj);
					}
					jsonvalue = jsonvalue.Parent;
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				}
				case JSONObject.JSONParsingState.EndArray:
				{
					if (jsonString[i] != ']')
					{
						return JSONObject.Fail(']', i);
					}
					if (jsonvalue.Parent == null)
					{
						return jsonvalue.Obj;
					}
					JSONValueType type = jsonvalue.Parent.Type;
					if (type != JSONValueType.Object)
					{
						if (type != JSONValueType.Array)
						{
							return JSONObject.Fail("valid object", i);
						}
						jsonvalue.Parent.Array.Add(new JSONValue(jsonvalue.Array));
					}
					else
					{
						jsonvalue.Parent.Obj.values[list.Pop<string>()] = new JSONValue(jsonvalue.Array);
					}
					jsonvalue = jsonvalue.Parent;
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				}
				case JSONObject.JSONParsingState.Key:
					if (jsonString[i] == '}')
					{
						i--;
						jsonparsingState = JSONObject.JSONParsingState.EndObject;
					}
					else
					{
						string text = JSONObject.ParseString(jsonString, ref i);
						if (text == null)
						{
							return JSONObject.Fail("key string", i);
						}
						list.Add(text);
						jsonparsingState = JSONObject.JSONParsingState.KeyValueSeparator;
					}
					break;
				case JSONObject.JSONParsingState.Value:
				{
					char c = jsonString[i];
					if (c == '"')
					{
						jsonparsingState = JSONObject.JSONParsingState.String;
					}
					else if (char.IsDigit(c) || c == '-')
					{
						jsonparsingState = JSONObject.JSONParsingState.Number;
					}
					else
					{
						char c2 = c;
						switch (c2)
						{
						case '[':
							jsonparsingState = JSONObject.JSONParsingState.Array;
							break;
						default:
							if (c2 != 'f')
							{
								if (c2 == 'n')
								{
									jsonparsingState = JSONObject.JSONParsingState.Null;
									break;
								}
								if (c2 != 't')
								{
									if (c2 != '{')
									{
										return JSONObject.Fail("beginning of value", i);
									}
									jsonparsingState = JSONObject.JSONParsingState.Object;
									break;
								}
							}
							jsonparsingState = JSONObject.JSONParsingState.Boolean;
							break;
						case ']':
							if (jsonvalue.Type != JSONValueType.Array)
							{
								return JSONObject.Fail("valid array", i);
							}
							jsonparsingState = JSONObject.JSONParsingState.EndArray;
							break;
						}
					}
					i--;
					break;
				}
				case JSONObject.JSONParsingState.KeyValueSeparator:
					if (jsonString[i] != ':')
					{
						return JSONObject.Fail(':', i);
					}
					jsonparsingState = JSONObject.JSONParsingState.Value;
					break;
				case JSONObject.JSONParsingState.ValueSeparator:
				{
					char c2 = jsonString[i];
					if (c2 != ',')
					{
						if (c2 != ']')
						{
							if (c2 != '}')
							{
								return JSONObject.Fail(", } ]", i);
							}
							jsonparsingState = JSONObject.JSONParsingState.EndObject;
							i--;
						}
						else
						{
							jsonparsingState = JSONObject.JSONParsingState.EndArray;
							i--;
						}
					}
					else
					{
						jsonparsingState = ((jsonvalue.Type != JSONValueType.Object) ? JSONObject.JSONParsingState.Value : JSONObject.JSONParsingState.Key);
					}
					break;
				}
				case JSONObject.JSONParsingState.String:
				{
					string text2 = JSONObject.ParseString(jsonString, ref i);
					if (text2 == null)
					{
						return JSONObject.Fail("string value", i);
					}
					JSONValueType type = jsonvalue.Type;
					if (type != JSONValueType.Object)
					{
						if (type != JSONValueType.Array)
						{
							JSONLogger.Error("Fatal error, current JSON value not valid");
							return null;
						}
						jsonvalue.Array.Add(text2);
					}
					else
					{
						jsonvalue.Obj.values[list.Pop<string>()] = new JSONValue(text2);
					}
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				}
				case JSONObject.JSONParsingState.Number:
				{
					double num = JSONObject.ParseNumber(jsonString, ref i);
					if (double.IsNaN(num))
					{
						return JSONObject.Fail("valid number", i);
					}
					JSONValueType type = jsonvalue.Type;
					if (type != JSONValueType.Object)
					{
						if (type != JSONValueType.Array)
						{
							JSONLogger.Error("Fatal error, current JSON value not valid");
							return null;
						}
						jsonvalue.Array.Add(num);
					}
					else
					{
						jsonvalue.Obj.values[list.Pop<string>()] = new JSONValue(num);
					}
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				}
				case JSONObject.JSONParsingState.Boolean:
					if (jsonString[i] == 't')
					{
						if (jsonString.Length < i + 4 || jsonString[i + 1] != 'r' || jsonString[i + 2] != 'u' || jsonString[i + 3] != 'e')
						{
							return JSONObject.Fail("true", i);
						}
						JSONValueType type = jsonvalue.Type;
						if (type != JSONValueType.Object)
						{
							if (type != JSONValueType.Array)
							{
								JSONLogger.Error("Fatal error, current JSON value not valid");
								return null;
							}
							jsonvalue.Array.Add(new JSONValue(true));
						}
						else
						{
							jsonvalue.Obj.values[list.Pop<string>()] = new JSONValue(true);
						}
						i += 3;
					}
					else
					{
						if (jsonString.Length < i + 5 || jsonString[i + 1] != 'a' || jsonString[i + 2] != 'l' || jsonString[i + 3] != 's' || jsonString[i + 4] != 'e')
						{
							return JSONObject.Fail("false", i);
						}
						JSONValueType type = jsonvalue.Type;
						if (type != JSONValueType.Object)
						{
							if (type != JSONValueType.Array)
							{
								JSONLogger.Error("Fatal error, current JSON value not valid");
								return null;
							}
							jsonvalue.Array.Add(new JSONValue(false));
						}
						else
						{
							jsonvalue.Obj.values[list.Pop<string>()] = new JSONValue(false);
						}
						i += 4;
					}
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				case JSONObject.JSONParsingState.Null:
					if (jsonString[i] == 'n')
					{
						if (jsonString.Length < i + 4 || jsonString[i + 1] != 'u' || jsonString[i + 2] != 'l' || jsonString[i + 3] != 'l')
						{
							return JSONObject.Fail("null", i);
						}
						JSONValueType type = jsonvalue.Type;
						if (type != JSONValueType.Object)
						{
							if (type != JSONValueType.Array)
							{
								JSONLogger.Error("Fatal error, current JSON value not valid");
								return null;
							}
							jsonvalue.Array.Add(new JSONValue(JSONValueType.Null));
						}
						else
						{
							jsonvalue.Obj.values[list.Pop<string>()] = new JSONValue(JSONValueType.Null);
						}
						i += 3;
					}
					jsonparsingState = JSONObject.JSONParsingState.ValueSeparator;
					break;
				}
			}
			JSONLogger.Error("Unexpected end of string");
			return null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002815 File Offset: 0x00000A15
		private static int SkipWhitespace(string str, int pos)
		{
			while (pos < str.Length && char.IsWhiteSpace(str[pos]))
			{
				pos++;
			}
			return pos;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000C468 File Offset: 0x0000A668
		private static string ParseString(string str, ref int startPosition)
		{
			if (str[startPosition] != '"' || startPosition + 1 >= str.Length)
			{
				JSONObject.Fail('"', startPosition);
				return null;
			}
			int num = str.IndexOf('"', startPosition + 1);
			if (num <= startPosition)
			{
				JSONObject.Fail('"', startPosition + 1);
				return null;
			}
			while (str[num - 1] == '\\')
			{
				num = str.IndexOf('"', num + 1);
				if (num <= startPosition)
				{
					JSONObject.Fail('"', startPosition + 1);
					return null;
				}
			}
			string text = string.Empty;
			if (num > startPosition + 1)
			{
				text = str.Substring(startPosition + 1, num - startPosition - 1);
			}
			startPosition = num;
			for (;;)
			{
				Match match = JSONObject.unicodeRegex.Match(text);
				if (!match.Success)
				{
					break;
				}
				string text2 = match.Groups[1].Captures[0].Value;
				JSONObject.unicodeBytes[1] = byte.Parse(text2.Substring(0, 2), NumberStyles.HexNumber);
				JSONObject.unicodeBytes[0] = byte.Parse(text2.Substring(2, 2), NumberStyles.HexNumber);
				text2 = Encoding.Unicode.GetString(JSONObject.unicodeBytes);
				text = text.Replace(match.Value, text2);
			}
			return text;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		private static double ParseNumber(string str, ref int startPosition)
		{
			if (startPosition >= str.Length || (!char.IsDigit(str[startPosition]) && str[startPosition] != '-'))
			{
				return double.NaN;
			}
			int num = startPosition + 1;
			while (num < str.Length && str[num] != ',' && str[num] != ']' && str[num] != '}')
			{
				num++;
			}
			double result;
			if (!double.TryParse(str.Substring(startPosition, num - startPosition), NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return double.NaN;
			}
			startPosition = num - 1;
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000283F File Offset: 0x00000A3F
		private static JSONObject Fail(char expected, int position)
		{
			return JSONObject.Fail(new string(expected, 1), position);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000284E File Offset: 0x00000A4E
		private static JSONObject Fail(string expected, int position)
		{
			JSONLogger.Error(string.Concat(new object[]
			{
				"Invalid json string, expecting ",
				expected,
				" at ",
				position
			}));
			return null;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000C668 File Offset: 0x0000A868
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			foreach (KeyValuePair<string, JSONValue> keyValuePair in this.values)
			{
				stringBuilder.Append("\"" + keyValuePair.Key + "\"");
				stringBuilder.Append(':');
				stringBuilder.Append(keyValuePair.Value.ToString());
				stringBuilder.Append(',');
			}
			if (this.values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000027C7 File Offset: 0x000009C7
		public IEnumerator<KeyValuePair<string, JSONValue>> GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000287E File Offset: 0x00000A7E
		public void Clear()
		{
			this.values.Clear();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000288B File Offset: 0x00000A8B
		public void Remove(string key)
		{
			if (this.values.ContainsKey(key))
			{
				this.values.Remove(key);
			}
		}

		// Token: 0x04000010 RID: 16
		private readonly IDictionary<string, JSONValue> values = new Dictionary<string, JSONValue>();

		// Token: 0x04000011 RID: 17
		private static readonly Regex unicodeRegex = new Regex("\\\\u([0-9a-fA-F]{4})");

		// Token: 0x04000012 RID: 18
		private static readonly byte[] unicodeBytes = new byte[2];

		// Token: 0x02000008 RID: 8
		private enum JSONParsingState
		{
			// Token: 0x04000014 RID: 20
			Object,
			// Token: 0x04000015 RID: 21
			Array,
			// Token: 0x04000016 RID: 22
			EndObject,
			// Token: 0x04000017 RID: 23
			EndArray,
			// Token: 0x04000018 RID: 24
			Key,
			// Token: 0x04000019 RID: 25
			Value,
			// Token: 0x0400001A RID: 26
			KeyValueSeparator,
			// Token: 0x0400001B RID: 27
			ValueSeparator,
			// Token: 0x0400001C RID: 28
			String,
			// Token: 0x0400001D RID: 29
			Number,
			// Token: 0x0400001E RID: 30
			Boolean,
			// Token: 0x0400001F RID: 31
			Null
		}
	}
}
