using System;

// Token: 0x020000ED RID: 237
[Serializable]
public class PuzzleTokenLevelDefinition : SubDefinition
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x00005CF8 File Offset: 0x00003EF8
	public string GetSpriteName(bool over, bool bonus)
	{
		if (bonus && this.hasBonusSprite)
		{
			if (over)
			{
				return this.bonusOverSpriteName;
			}
			return this.bonusSpriteName;
		}
		else
		{
			if (over)
			{
				return this.overSpriteName;
			}
			return this.spriteName;
		}
	}

	// Token: 0x04000668 RID: 1640
	public string spriteName;

	// Token: 0x04000669 RID: 1641
	public string overSpriteName;

	// Token: 0x0400066A RID: 1642
	public bool hasBonusSprite;

	// Token: 0x0400066B RID: 1643
	public string bonusSpriteName;

	// Token: 0x0400066C RID: 1644
	public string bonusOverSpriteName;
}
