using System;

// Token: 0x0200016C RID: 364
[Serializable]
public class tk2dSpriteAnimationFrame
{
	// Token: 0x06000911 RID: 2321 RVA: 0x00008DA9 File Offset: 0x00006FA9
	public void CopyFrom(tk2dSpriteAnimationFrame source)
	{
		this.CopyFrom(source, true);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00008DB3 File Offset: 0x00006FB3
	public void CopyTriggerFrom(tk2dSpriteAnimationFrame source)
	{
		this.triggerEvent = source.triggerEvent;
		this.eventInfo = source.eventInfo;
		this.eventInt = source.eventInt;
		this.eventFloat = source.eventFloat;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00008DE5 File Offset: 0x00006FE5
	public void ClearTrigger()
	{
		this.triggerEvent = false;
		this.eventInt = 0;
		this.eventFloat = 0f;
		this.eventInfo = string.Empty;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00008E0B File Offset: 0x0000700B
	public void CopyFrom(tk2dSpriteAnimationFrame source, bool full)
	{
		this.spriteCollection = source.spriteCollection;
		this.spriteId = source.spriteId;
		if (full)
		{
			this.CopyTriggerFrom(source);
		}
	}

	// Token: 0x04000A01 RID: 2561
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000A02 RID: 2562
	public int spriteId;

	// Token: 0x04000A03 RID: 2563
	public bool triggerEvent;

	// Token: 0x04000A04 RID: 2564
	public string eventInfo = string.Empty;

	// Token: 0x04000A05 RID: 2565
	public int eventInt;

	// Token: 0x04000A06 RID: 2566
	public float eventFloat;
}
