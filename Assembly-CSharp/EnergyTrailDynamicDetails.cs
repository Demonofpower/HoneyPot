using System;

// Token: 0x020000F5 RID: 245
public class EnergyTrailDynamicDetails
{
	// Token: 0x06000500 RID: 1280 RVA: 0x00005E1A File Offset: 0x0000401A
	public EnergyTrailDynamicDetails(EnergyTrailDefinition definition)
	{
		this.energyTrailDestX = definition.energyTrailDestX;
		this.energyTrailDestY = definition.energyTrailDestY;
		this.energyTrailDestWidth = definition.energyTrailDestWidth;
		this.energyTrailDestHeight = definition.energyTrailDestHeight;
	}

	// Token: 0x0400069F RID: 1695
	public float energyTrailDestX;

	// Token: 0x040006A0 RID: 1696
	public float energyTrailDestY;

	// Token: 0x040006A1 RID: 1697
	public float energyTrailDestWidth;

	// Token: 0x040006A2 RID: 1698
	public float energyTrailDestHeight;
}
