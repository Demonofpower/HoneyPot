using System;
using System.Collections.Generic;

// Token: 0x020000BC RID: 188
[Serializable]
public class DialogSceneConditionalBranch
{
	// Token: 0x040004A4 RID: 1188
	public DialogSceneBranchConditionType type;

	// Token: 0x040004A5 RID: 1189
	public List<DialogSceneStep> steps = new List<DialogSceneStep>();

	// Token: 0x040004A6 RID: 1190
	public int goalValue;

	// Token: 0x040004A7 RID: 1191
	public bool invertCondition;

	// Token: 0x040004A8 RID: 1192
	public GirlMetStatus girlMetStatus;

	// Token: 0x040004A9 RID: 1193
	public GirlDetailType girlDetailType;
}
