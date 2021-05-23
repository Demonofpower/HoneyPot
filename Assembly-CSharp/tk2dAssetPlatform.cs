using System;

// Token: 0x02000160 RID: 352
[Serializable]
public class tk2dAssetPlatform
{
	// Token: 0x06000852 RID: 2130 RVA: 0x00008520 File Offset: 0x00006720
	public tk2dAssetPlatform(string name, float scale)
	{
		this.name = name;
		this.scale = scale;
	}

	// Token: 0x040009B6 RID: 2486
	public string name = string.Empty;

	// Token: 0x040009B7 RID: 2487
	public float scale = 1f;
}
