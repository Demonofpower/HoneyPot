using System;
using System.Collections.Generic;

// Token: 0x020000C8 RID: 200
[Serializable]
public class DialogTriggerLineSet : SubDefinition
{
	// Token: 0x060004C4 RID: 1220 RVA: 0x00005B85 File Offset: 0x00003D85
	public DialogTriggerLineSet()
	{
		this.lines.Add(new DialogTriggerLine());
	}

	// Token: 0x04000510 RID: 1296
	public bool editorFromJsonSwitch;

	// Token: 0x04000511 RID: 1297
	public string editorFromJsonString;

	// Token: 0x04000512 RID: 1298
	public string label = string.Empty;

	// Token: 0x04000513 RID: 1299
	public List<DialogTriggerLine> lines = new List<DialogTriggerLine>();
}
