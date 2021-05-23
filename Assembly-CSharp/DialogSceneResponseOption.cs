using System;
using System.Collections.Generic;

// Token: 0x020000C0 RID: 192
[Serializable]
public class DialogSceneResponseOption
{
	// Token: 0x040004B4 RID: 1204
	public string text;

	// Token: 0x040004B5 RID: 1205
	public bool secondary;

	// Token: 0x040004B6 RID: 1206
	public string secondaryText;

	// Token: 0x040004B7 RID: 1207
	public List<DialogSceneStep> steps = new List<DialogSceneStep>();

	// Token: 0x040004B8 RID: 1208
	public int specialIndex = -1;
}
