using System;

// Token: 0x020000C7 RID: 199
[Serializable]
public class DialogTriggerLine
{
	// Token: 0x060004C3 RID: 1219 RVA: 0x00025524 File Offset: 0x00023724
	public DialogTriggerLine()
	{
		this.dialogLine = new DialogLine[Enum.GetNames(typeof(EditorDialogTriggerGirl)).Length];
		for (int i = 0; i < this.dialogLine.Length; i++)
		{
			DialogLine dialogLine = new DialogLine();
			dialogLine.text = "Dialog...";
			this.dialogLine[i] = dialogLine;
		}
	}

	// Token: 0x0400050F RID: 1295
	public DialogLine[] dialogLine;
}
