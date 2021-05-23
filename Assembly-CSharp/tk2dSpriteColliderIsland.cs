using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
[Serializable]
public class tk2dSpriteColliderIsland
{
	// Token: 0x0600095E RID: 2398 RVA: 0x00009426 File Offset: 0x00007626
	public bool IsValid()
	{
		if (this.connected)
		{
			return this.points.Length >= 3;
		}
		return this.points.Length >= 2;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x000409DC File Offset: 0x0003EBDC
	public void CopyFrom(tk2dSpriteColliderIsland src)
	{
		this.connected = src.connected;
		this.points = new Vector2[src.points.Length];
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = src.points[i];
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00040A44 File Offset: 0x0003EC44
	public bool CompareTo(tk2dSpriteColliderIsland src)
	{
		if (this.connected != src.connected)
		{
			return false;
		}
		if (this.points.Length != src.points.Length)
		{
			return false;
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			if (this.points[i] != src.points[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000A29 RID: 2601
	public bool connected = true;

	// Token: 0x04000A2A RID: 2602
	public Vector2[] points;
}
