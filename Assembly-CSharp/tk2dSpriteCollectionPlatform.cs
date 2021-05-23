using System;

// Token: 0x02000180 RID: 384
[Serializable]
public class tk2dSpriteCollectionPlatform
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x000094EC File Offset: 0x000076EC
	public bool Valid
	{
		get
		{
			return this.name.Length > 0 && this.spriteCollection != null;
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0000950E File Offset: 0x0000770E
	public void CopyFrom(tk2dSpriteCollectionPlatform source)
	{
		this.name = source.name;
		this.spriteCollection = source.spriteCollection;
	}

	// Token: 0x04000AA7 RID: 2727
	public string name = string.Empty;

	// Token: 0x04000AA8 RID: 2728
	public tk2dSpriteCollection spriteCollection;
}
