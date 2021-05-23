using System;
using System.Collections.Generic;

// Token: 0x020000EB RID: 235
public class PuzzleTokenDefinition : Definition
{
	// Token: 0x04000657 RID: 1623
	public new string name;

	// Token: 0x04000658 RID: 1624
	public int weight;

	// Token: 0x04000659 RID: 1625
	public bool bonusRound;

	// Token: 0x0400065A RID: 1626
	public PuzzleTokenType type;

	// Token: 0x0400065B RID: 1627
	public TraitDefinition linkedTrait;

	// Token: 0x0400065C RID: 1628
	public PuzzleGameResourceType resourceType;

	// Token: 0x0400065D RID: 1629
	public bool negateResource;

	// Token: 0x0400065E RID: 1630
	public AudioDefinition matchSound;

	// Token: 0x0400065F RID: 1631
	public AudioDefinition bonusMatchSound;

	// Token: 0x04000660 RID: 1632
	public EnergyTrailDefinition energyTrail;

	// Token: 0x04000661 RID: 1633
	public List<PuzzleTokenLevelDefinition> levels = new List<PuzzleTokenLevelDefinition>();
}
