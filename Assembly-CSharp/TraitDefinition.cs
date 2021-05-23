using System;
using System.Collections.Generic;

// Token: 0x020000F0 RID: 240
public class TraitDefinition : Definition
{
	// Token: 0x04000670 RID: 1648
	public new string name;

	// Token: 0x04000671 RID: 1649
	public PlayerTraitType traitType;

	// Token: 0x04000672 RID: 1650
	public bool linkedToResourceType;

	// Token: 0x04000673 RID: 1651
	public PuzzleGameResourceType resourceType;

	// Token: 0x04000674 RID: 1652
	public List<ItemDefinition> levelItems = new List<ItemDefinition>();
}
