using System;

// Token: 0x020000AF RID: 175
[Serializable]
public class PuzzleTokenCondition
{
	// Token: 0x04000440 RID: 1088
	public PuzzleTokenConditionType type;

	// Token: 0x04000441 RID: 1089
	public PuzzleTokenDefinition tokenDefinition;

	// Token: 0x04000442 RID: 1090
	public NumberComparisonType comparison;

	// Token: 0x04000443 RID: 1091
	public string val;

	// Token: 0x04000444 RID: 1092
	public bool inverse;
}
