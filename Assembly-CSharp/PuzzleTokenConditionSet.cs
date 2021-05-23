using System;
using System.Collections.Generic;

// Token: 0x020000B2 RID: 178
[Serializable]
public class PuzzleTokenConditionSet
{
	// Token: 0x04000450 RID: 1104
	public List<PuzzleTokenCondition> conditions = new List<PuzzleTokenCondition>();

	// Token: 0x04000451 RID: 1105
	public bool expand;

	// Token: 0x04000452 RID: 1106
	public TokenConditionExpansionType tokenGroupExpansionType;

	// Token: 0x04000453 RID: 1107
	public int expandBy;

	// Token: 0x04000454 RID: 1108
	public bool inclusive = true;

	// Token: 0x04000455 RID: 1109
	public bool conditionedExpansion;

	// Token: 0x04000456 RID: 1110
	public List<PuzzleTokenCondition> expansionConditions = new List<PuzzleTokenCondition>();
}
