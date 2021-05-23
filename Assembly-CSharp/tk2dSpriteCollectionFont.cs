using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
[Serializable]
public class tk2dSpriteCollectionFont
{
	// Token: 0x0600096B RID: 2411 RVA: 0x000414AC File Offset: 0x0003F6AC
	public void CopyFrom(tk2dSpriteCollectionFont src)
	{
		this.active = src.active;
		this.bmFont = src.bmFont;
		this.texture = src.texture;
		this.dupeCaps = src.dupeCaps;
		this.flipTextureY = src.flipTextureY;
		this.charPadX = src.charPadX;
		this.data = src.data;
		this.editorData = src.editorData;
		this.materialId = src.materialId;
		this.gradientCount = src.gradientCount;
		this.gradientTexture = src.gradientTexture;
		this.useGradient = src.useGradient;
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600096C RID: 2412 RVA: 0x0004154C File Offset: 0x0003F74C
	public string Name
	{
		get
		{
			if (this.bmFont == null || this.texture == null)
			{
				return "Empty";
			}
			if (this.data == null)
			{
				return this.bmFont.name + " (Inactive)";
			}
			return this.bmFont.name;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x000415B4 File Offset: 0x0003F7B4
	public bool InUse
	{
		get
		{
			return this.active && this.bmFont != null && this.texture != null && this.data != null && this.editorData != null;
		}
	}

	// Token: 0x04000A9B RID: 2715
	public bool active;

	// Token: 0x04000A9C RID: 2716
	public UnityEngine.Object bmFont;

	// Token: 0x04000A9D RID: 2717
	public Texture2D texture;

	// Token: 0x04000A9E RID: 2718
	public bool dupeCaps;

	// Token: 0x04000A9F RID: 2719
	public bool flipTextureY;

	// Token: 0x04000AA0 RID: 2720
	public int charPadX;

	// Token: 0x04000AA1 RID: 2721
	public tk2dFontData data;

	// Token: 0x04000AA2 RID: 2722
	public tk2dFont editorData;

	// Token: 0x04000AA3 RID: 2723
	public int materialId;

	// Token: 0x04000AA4 RID: 2724
	public bool useGradient;

	// Token: 0x04000AA5 RID: 2725
	public Texture2D gradientTexture;

	// Token: 0x04000AA6 RID: 2726
	public int gradientCount = 1;
}
