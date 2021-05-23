using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Boomlagoon.JSON
{
	// Token: 0x02000006 RID: 6
	public class JSONArray : IEnumerable, IEnumerable<JSONValue>
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000272E File Offset: 0x0000092E
		public JSONArray()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000B900 File Offset: 0x00009B00
		public JSONArray(JSONArray array)
		{
			this.values = new List<JSONValue>();
			foreach (JSONValue value in array.values)
			{
				this.values.Add(new JSONValue(value));
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002741 File Offset: 0x00000941
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002753 File Offset: 0x00000953
		public void Add(JSONValue value)
		{
			this.values.Add(value);
		}

		// Token: 0x17000008 RID: 8
		public JSONValue this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000277E File Offset: 0x0000097E
		public int Length
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000B980 File Offset: 0x00009B80
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			foreach (JSONValue jsonvalue in this.values)
			{
				stringBuilder.Append(jsonvalue.ToString());
				stringBuilder.Append(',');
			}
			if (this.values.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002741 File Offset: 0x00000941
		public IEnumerator<JSONValue> GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000BA28 File Offset: 0x00009C28
		public static JSONArray Parse(string jsonString)
		{
			JSONObject jsonobject = JSONObject.Parse("{ \"array\" :" + jsonString + '}');
			return (jsonobject != null) ? jsonobject.GetValue("array").Array : null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000278B File Offset: 0x0000098B
		public void Clear()
		{
			this.values.Clear();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public void Remove(int index)
		{
			if (index >= 0 && index < this.values.Count)
			{
				this.values.RemoveAt(index);
			}
			else
			{
				JSONLogger.Error(string.Concat(new object[]
				{
					"index out of range: ",
					index,
					" (Expected 0 <= index < ",
					this.values.Count,
					")"
				}));
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000BAE8 File Offset: 0x00009CE8
		public static JSONArray operator +(JSONArray lhs, JSONArray rhs)
		{
			JSONArray jsonarray = new JSONArray(lhs);
			foreach (JSONValue value in rhs.values)
			{
				jsonarray.Add(value);
			}
			return jsonarray;
		}

		// Token: 0x0400000F RID: 15
		private readonly List<JSONValue> values = new List<JSONValue>();
	}
}
