using System;

namespace Boomlagoon.JSON
{
	// Token: 0x02000005 RID: 5
	public class JSONValue
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002600 File Offset: 0x00000800
		public JSONValue(JSONValueType type)
		{
			this.Type = type;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000260F File Offset: 0x0000080F
		public JSONValue(string str)
		{
			this.Type = JSONValueType.String;
			this.Str = str;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002625 File Offset: 0x00000825
		public JSONValue(double number)
		{
			this.Type = JSONValueType.Number;
			this.Number = number;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000263B File Offset: 0x0000083B
		public JSONValue(JSONObject obj)
		{
			if (obj == null)
			{
				this.Type = JSONValueType.Null;
			}
			else
			{
				this.Type = JSONValueType.Object;
				this.Obj = obj;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002663 File Offset: 0x00000863
		public JSONValue(JSONArray array)
		{
			this.Type = JSONValueType.Array;
			this.Array = array;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002679 File Offset: 0x00000879
		public JSONValue(bool boolean)
		{
			this.Type = JSONValueType.Boolean;
			this.Boolean = boolean;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000B7B4 File Offset: 0x000099B4
		public JSONValue(JSONValue value)
		{
			this.Type = value.Type;
			switch (this.Type)
			{
			case JSONValueType.String:
				this.Str = value.Str;
				break;
			case JSONValueType.Number:
				this.Number = value.Number;
				break;
			case JSONValueType.Object:
				if (value.Obj != null)
				{
					this.Obj = new JSONObject(value.Obj);
				}
				break;
			case JSONValueType.Array:
				this.Array = new JSONArray(value.Array);
				break;
			case JSONValueType.Boolean:
				this.Boolean = value.Boolean;
				break;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000268F File Offset: 0x0000088F
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002697 File Offset: 0x00000897
		public JSONValueType Type { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000026A0 File Offset: 0x000008A0
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000026A8 File Offset: 0x000008A8
		public string Str { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000026B1 File Offset: 0x000008B1
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000026B9 File Offset: 0x000008B9
		public double Number { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026C2 File Offset: 0x000008C2
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000026CA File Offset: 0x000008CA
		public JSONObject Obj { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000026D3 File Offset: 0x000008D3
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000026DB File Offset: 0x000008DB
		public JSONArray Array { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000026E4 File Offset: 0x000008E4
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000026EC File Offset: 0x000008EC
		public bool Boolean { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000026F5 File Offset: 0x000008F5
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000026FD File Offset: 0x000008FD
		public JSONValue Parent { get; set; }

		// Token: 0x06000019 RID: 25 RVA: 0x0000B864 File Offset: 0x00009A64
		public override string ToString()
		{
			switch (this.Type)
			{
			case JSONValueType.String:
				return "\"" + this.Str + "\"";
			case JSONValueType.Number:
				return this.Number.ToString();
			case JSONValueType.Object:
				return this.Obj.ToString();
			case JSONValueType.Array:
				return this.Array.ToString();
			case JSONValueType.Boolean:
				return (!this.Boolean) ? "false" : "true";
			case JSONValueType.Null:
				return "null";
			default:
				return "null";
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002706 File Offset: 0x00000906
		public static implicit operator JSONValue(string str)
		{
			return new JSONValue(str);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000270E File Offset: 0x0000090E
		public static implicit operator JSONValue(double number)
		{
			return new JSONValue(number);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002716 File Offset: 0x00000916
		public static implicit operator JSONValue(JSONObject obj)
		{
			return new JSONValue(obj);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000271E File Offset: 0x0000091E
		public static implicit operator JSONValue(JSONArray array)
		{
			return new JSONValue(array);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002726 File Offset: 0x00000926
		public static implicit operator JSONValue(bool boolean)
		{
			return new JSONValue(boolean);
		}
	}
}
