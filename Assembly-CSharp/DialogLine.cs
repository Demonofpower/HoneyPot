using System;
using System.Collections.Generic;

// Token: 0x020000C3 RID: 195
[Serializable]
public class DialogLine
{
	// Token: 0x060004BF RID: 1215 RVA: 0x00005B14 File Offset: 0x00003D14
	public string GetText()
	{
		if (this.secondary && GameManager.System.Player.settingsGender == SettingsGender.FEMALE)
		{
			return this.secondaryText;
		}
		return this.text;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00005B43 File Offset: 0x00003D43
	public AudioDefinition GetAudio()
	{
		if (this.secondary && GameManager.System.Player.settingsGender == SettingsGender.FEMALE)
		{
			return this.secondaryAudioDefinition;
		}
		return this.audioDefinition;
	}

	// Token: 0x040004F1 RID: 1265
	public string text = string.Empty;

	// Token: 0x040004F2 RID: 1266
	public AudioDefinition audioDefinition = new AudioDefinition();

	// Token: 0x040004F3 RID: 1267
	public bool secondary;

	// Token: 0x040004F4 RID: 1268
	public string secondaryText = string.Empty;

	// Token: 0x040004F5 RID: 1269
	public AudioDefinition secondaryAudioDefinition = new AudioDefinition();

	// Token: 0x040004F6 RID: 1270
	public DialogLineExpression startExpression;

	// Token: 0x040004F7 RID: 1271
	public List<DialogLineExpression> expressions = new List<DialogLineExpression>();

	// Token: 0x040004F8 RID: 1272
	public bool hasEndExpression;

	// Token: 0x040004F9 RID: 1273
	public DialogLineExpression endExpression;
}
