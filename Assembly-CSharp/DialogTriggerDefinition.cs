using System;
using System.Collections.Generic;

// Token: 0x020000C5 RID: 197
public class DialogTriggerDefinition : Definition
{
	// Token: 0x040004FF RID: 1279
	public bool skippable;

	// Token: 0x04000500 RID: 1280
	public EditorDialogTriggerGirl editorGirl;

	// Token: 0x04000501 RID: 1281
	public List<DialogTriggerLineSet> lineSets = new List<DialogTriggerLineSet>();
}
