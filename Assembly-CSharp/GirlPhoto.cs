using System;

// Token: 0x020000D3 RID: 211
[Serializable]
public class GirlPhoto
{
	// Token: 0x04000595 RID: 1429
	public bool hasAlts;

	// Token: 0x04000596 RID: 1430
	public int altCount;

	// Token: 0x04000597 RID: 1431
	public MessageDefinition messageDef;

	// Token: 0x04000598 RID: 1432
	public bool sendAtDaytime;

	// Token: 0x04000599 RID: 1433
	public GameClockDaytime sendDaytime;

	// Token: 0x0400059A RID: 1434
	public string[] fullSpriteName;

	// Token: 0x0400059B RID: 1435
	public string[] smallSpriteName;

	// Token: 0x0400059C RID: 1436
	public string[] thumbnailSpriteName;
}
