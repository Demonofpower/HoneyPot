using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public static class tk2dUITime
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0000B643 File Offset: 0x00009843
	public static float deltaTime
	{
		get
		{
			return tk2dUITime._deltaTime;
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0000B64A File Offset: 0x0000984A
	public static void Init()
	{
		tk2dUITime.lastRealTime = (double)Time.realtimeSinceStartup;
		tk2dUITime._deltaTime = Time.maximumDeltaTime;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0004D404 File Offset: 0x0004B604
	public static void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (Time.timeScale < 0.001f)
		{
			tk2dUITime._deltaTime = Mathf.Min(0.06666667f, (float)((double)realtimeSinceStartup - tk2dUITime.lastRealTime));
		}
		else
		{
			tk2dUITime._deltaTime = Time.deltaTime / Time.timeScale;
		}
		tk2dUITime.lastRealTime = (double)realtimeSinceStartup;
	}

	// Token: 0x04000C9E RID: 3230
	private static double lastRealTime;

	// Token: 0x04000C9F RID: 3231
	private static float _deltaTime = 0.016666668f;
}
