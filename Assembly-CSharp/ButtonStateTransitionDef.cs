using System;

// Token: 0x0200000E RID: 14
[Serializable]
public class ButtonStateTransitionDef
{
	// Token: 0x0400004C RID: 76
	public ButtonStateTransitionType type;

	// Token: 0x0400004D RID: 77
	public float val;

	// Token: 0x0400004E RID: 78
	public float duration;

	// Token: 0x0400004F RID: 79
	public string spriteName;

	// Token: 0x04000050 RID: 80
	public bool targetChild;

	// Token: 0x04000051 RID: 81
	public string childName;

	// Token: 0x04000052 RID: 82
	public tk2dFontData font;

	// Token: 0x04000053 RID: 83
	public string hexColor;
}
