using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
[Serializable]
public class tk2dTextMeshData
{
	// Token: 0x0400094B RID: 2379
	public int version;

	// Token: 0x0400094C RID: 2380
	public tk2dFontData font;

	// Token: 0x0400094D RID: 2381
	public string text = string.Empty;

	// Token: 0x0400094E RID: 2382
	public Color color = Color.white;

	// Token: 0x0400094F RID: 2383
	public Color color2 = Color.white;

	// Token: 0x04000950 RID: 2384
	public bool useGradient;

	// Token: 0x04000951 RID: 2385
	public int textureGradient;

	// Token: 0x04000952 RID: 2386
	public TextAnchor anchor = TextAnchor.LowerLeft;

	// Token: 0x04000953 RID: 2387
	public Vector3 scale = Vector3.one;

	// Token: 0x04000954 RID: 2388
	public bool kerning;

	// Token: 0x04000955 RID: 2389
	public int maxChars = 16;

	// Token: 0x04000956 RID: 2390
	public bool inlineStyling;

	// Token: 0x04000957 RID: 2391
	public bool formatting;

	// Token: 0x04000958 RID: 2392
	public int wordWrapWidth;

	// Token: 0x04000959 RID: 2393
	public float spacing;

	// Token: 0x0400095A RID: 2394
	public float lineSpacing;
}
