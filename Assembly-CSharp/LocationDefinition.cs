using System;
using System.Collections.Generic;

// Token: 0x020000E5 RID: 229
public class LocationDefinition : Definition
{
	// Token: 0x060004DC RID: 1244 RVA: 0x0002585C File Offset: 0x00023A5C
	public LocationBackground GetBackgroundByDaytime(GameClockDaytime daytime)
	{
		if (this.backgrounds.Count > 0)
		{
			for (int i = 0; i < this.backgrounds.Count; i++)
			{
				if (this.backgrounds[i].daytime == daytime)
				{
					return this.backgrounds[i];
				}
			}
			return this.backgrounds[0];
		}
		return null;
	}

	// Token: 0x0400061C RID: 1564
	public new string name;

	// Token: 0x0400061D RID: 1565
	public string fullName;

	// Token: 0x0400061E RID: 1566
	public LocationType type;

	// Token: 0x0400061F RID: 1567
	public bool outdoorLocation;

	// Token: 0x04000620 RID: 1568
	public bool drinkingLocation;

	// Token: 0x04000621 RID: 1569
	public int outfitOverride;

	// Token: 0x04000622 RID: 1570
	public bool bonusRoundLocation;

	// Token: 0x04000623 RID: 1571
	public bool postBonusRoundLocation;

	// Token: 0x04000624 RID: 1572
	public string spriteCollectionName;

	// Token: 0x04000625 RID: 1573
	public float backgroundYOffset;

	// Token: 0x04000626 RID: 1574
	public List<LocationBackground> backgrounds = new List<LocationBackground>();
}
