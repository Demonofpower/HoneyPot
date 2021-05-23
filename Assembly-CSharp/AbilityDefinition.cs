using System;
using System.Collections.Generic;

// Token: 0x020000AE RID: 174
public class AbilityDefinition : Definition
{
	// Token: 0x0400043A RID: 1082
	public bool selectableTarget;

	// Token: 0x0400043B RID: 1083
	public bool postRehighlightTarget;

	// Token: 0x0400043C RID: 1084
	public EnergyTrailDefinition selectableEnergyTrail;

	// Token: 0x0400043D RID: 1085
	public PuzzleTokenConditionSet targetConditionSet;

	// Token: 0x0400043E RID: 1086
	public int targetMinimumCount = 1;

	// Token: 0x0400043F RID: 1087
	public List<AbilityBehaviorDefinition> behaviors = new List<AbilityBehaviorDefinition>();
}
