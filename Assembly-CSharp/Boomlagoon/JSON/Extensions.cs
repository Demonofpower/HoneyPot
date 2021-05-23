using System;
using System.Collections.Generic;

namespace Boomlagoon.JSON
{
	// Token: 0x02000002 RID: 2
	public static class Extensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x0000B788 File Offset: 0x00009988
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}
	}
}
