using System;

// Token: 0x020000C9 RID: 201
public class EnergyTrailDefinition : Definition
{
	// Token: 0x04000514 RID: 1300
	public SpriteGroupDefinition energyTrailSpriteGroup;

	// Token: 0x04000515 RID: 1301
	public SpriteGroupDefinition energyBurstSpriteGroup;

	// Token: 0x04000516 RID: 1302
	public float energyTrailDestX;

	// Token: 0x04000517 RID: 1303
	public float energyTrailDestY;

	// Token: 0x04000518 RID: 1304
	public float energyTrailDestWidth;

	// Token: 0x04000519 RID: 1305
	public float energyTrailDestHeight;

	// Token: 0x0400051A RID: 1306
	public tk2dFontData popLabelFont;

	// Token: 0x0400051B RID: 1307
	public AudioGroup contactSounds;

	// Token: 0x0400051C RID: 1308
	public bool showEnergySurge;

	// Token: 0x0400051D RID: 1309
	public GirlEnergySurge girlEnergySurge;
}
