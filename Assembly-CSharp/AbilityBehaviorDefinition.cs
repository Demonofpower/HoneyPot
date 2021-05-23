using System;
using System.Collections.Generic;

// Token: 0x020000A8 RID: 168
[Serializable]
public class AbilityBehaviorDefinition : SubDefinition
{
	// Token: 0x040003FC RID: 1020
	public AbilityBehaviorType type = AbilityBehaviorType.SET_VALUE;

	// Token: 0x040003FD RID: 1021
	public string handle;

	// Token: 0x040003FE RID: 1022
	public string groupRef;

	// Token: 0x040003FF RID: 1023
	public string valueRef;

	// Token: 0x04000400 RID: 1024
	public PuzzleGameResourceType resourceType;

	// Token: 0x04000401 RID: 1025
	public PuzzleTokenDefinition tokenDefinition;

	// Token: 0x04000402 RID: 1026
	public bool negative;

	// Token: 0x04000403 RID: 1027
	public PuzzleTokenConditionSet tokenGroupConditionSet;

	// Token: 0x04000404 RID: 1028
	public string limit;

	// Token: 0x04000405 RID: 1029
	public AbilityBehaviorVisualEffectType visualEffectType;

	// Token: 0x04000406 RID: 1030
	public EnergyTrailDefinition energyTrail;

	// Token: 0x04000407 RID: 1031
	public AbilityBehaviorValueType valueType;

	// Token: 0x04000408 RID: 1032
	public int hardValue = 1;

	// Token: 0x04000409 RID: 1033
	public string min;

	// Token: 0x0400040A RID: 1034
	public string max;

	// Token: 0x0400040B RID: 1035
	public List<string> combineValues = new List<string>();

	// Token: 0x0400040C RID: 1036
	public AbilityBehaviorValueOperation combineOperation;

	// Token: 0x0400040D RID: 1037
	public bool resourceMaxValue;

	// Token: 0x0400040E RID: 1038
	public float percentOfValue = 1f;

	// Token: 0x0400040F RID: 1039
	public bool animate = true;

	// Token: 0x04000410 RID: 1040
	public PuzzleTokenType puzzleTokenType;

	// Token: 0x04000411 RID: 1041
	public bool levelSet;

	// Token: 0x04000412 RID: 1042
	public List<PuzzleTokenDefinition> tokenDefinitions = new List<PuzzleTokenDefinition>();

	// Token: 0x04000413 RID: 1043
	public bool weighted;

	// Token: 0x04000414 RID: 1044
	public AbilityBehaviorPuzzleEffectType puzzleEffectType;

	// Token: 0x04000415 RID: 1045
	public ItemDefinition puzzleEffectItemRef;

	// Token: 0x04000416 RID: 1046
	public AudioDefinition soundEffect;
}
