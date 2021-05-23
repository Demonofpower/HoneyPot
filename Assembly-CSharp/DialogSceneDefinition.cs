using System;
using System.Collections.Generic;

// Token: 0x020000BE RID: 190
public class DialogSceneDefinition : Definition
{
	// Token: 0x040004AE RID: 1198
	public bool editorFromJsonSwitch;

	// Token: 0x040004AF RID: 1199
	public string editorFromJsonString;

	// Token: 0x040004B0 RID: 1200
	public List<GameCondition> conditions = new List<GameCondition>();

	// Token: 0x040004B1 RID: 1201
	public List<DialogSceneStep> steps = new List<DialogSceneStep>();
}
