using System;
using UnityEngine;

namespace Boomlagoon.JSON
{
	// Token: 0x02000003 RID: 3
	internal static class JSONLogger
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000025F0 File Offset: 0x000007F0
		public static void Log(string str)
		{
			Debug.Log(str);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000025F8 File Offset: 0x000007F8
		public static void Error(string str)
		{
			Debug.LogError(str);
		}
	}
}
