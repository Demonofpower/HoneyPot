using System;
using UnityEngine;

// Token: 0x02000150 RID: 336
[AddComponentMenu("2D Toolkit/Backend/tk2dFont")]
public class tk2dFont : MonoBehaviour
{
	// Token: 0x060007DB RID: 2011 RVA: 0x00039C64 File Offset: 0x00037E64
	public void Upgrade()
	{
		if (this.version >= tk2dFont.CURRENT_VERSION)
		{
			return;
		}
		Debug.Log("Font '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = tk2dFont.CURRENT_VERSION;
	}

	// Token: 0x04000909 RID: 2313
	public UnityEngine.Object bmFont;

	// Token: 0x0400090A RID: 2314
	public Material material;

	// Token: 0x0400090B RID: 2315
	public Texture texture;

	// Token: 0x0400090C RID: 2316
	public Texture2D gradientTexture;

	// Token: 0x0400090D RID: 2317
	public bool dupeCaps;

	// Token: 0x0400090E RID: 2318
	public bool flipTextureY;

	// Token: 0x0400090F RID: 2319
	[HideInInspector]
	public bool proxyFont;

	// Token: 0x04000910 RID: 2320
	[HideInInspector]
	private bool useTk2dCamera;

	// Token: 0x04000911 RID: 2321
	[HideInInspector]
	private int targetHeight = 640;

	// Token: 0x04000912 RID: 2322
	[HideInInspector]
	private float targetOrthoSize = 1f;

	// Token: 0x04000913 RID: 2323
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x04000914 RID: 2324
	public int gradientCount = 1;

	// Token: 0x04000915 RID: 2325
	public bool manageMaterial;

	// Token: 0x04000916 RID: 2326
	[HideInInspector]
	public bool loadable;

	// Token: 0x04000917 RID: 2327
	public int charPadX;

	// Token: 0x04000918 RID: 2328
	public tk2dFontData data;

	// Token: 0x04000919 RID: 2329
	public static int CURRENT_VERSION = 1;

	// Token: 0x0400091A RID: 2330
	public int version;
}
