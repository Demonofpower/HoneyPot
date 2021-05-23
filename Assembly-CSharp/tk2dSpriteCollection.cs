using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000181 RID: 385
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollection")]
public class tk2dSpriteCollection : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000972 RID: 2418 RVA: 0x00009528 File Offset: 0x00007728
	// (set) Token: 0x06000973 RID: 2419 RVA: 0x00009530 File Offset: 0x00007730
	public Texture2D[] DoNotUse__TextureRefs
	{
		get
		{
			return this.textureRefs;
		}
		set
		{
			this.textureRefs = value;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x00009539 File Offset: 0x00007739
	public bool HasPlatformData
	{
		get
		{
			return this.platforms.Count > 1;
		}
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x000416DC File Offset: 0x0003F8DC
	public void Upgrade()
	{
		if (this.version == 4)
		{
			return;
		}
		Debug.Log("SpriteCollection '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			if (this.pixelPerfectPointSampled)
			{
				this.filterMode = FilterMode.Point;
			}
			else
			{
				this.filterMode = FilterMode.Bilinear;
			}
			this.userDefinedTextureSettings = true;
		}
		if (this.version < 3 && this.textureRefs != null && this.textureParams != null && this.textureRefs.Length == this.textureParams.Length)
		{
			for (int i = 0; i < this.textureRefs.Length; i++)
			{
				this.textureParams[i].texture = this.textureRefs[i];
			}
			this.textureRefs = null;
		}
		if (this.version < 4)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = 4;
	}

	// Token: 0x04000AA9 RID: 2729
	public const int CURRENT_VERSION = 4;

	// Token: 0x04000AAA RID: 2730
	[SerializeField]
	private tk2dSpriteCollectionDefinition[] textures;

	// Token: 0x04000AAB RID: 2731
	[SerializeField]
	private Texture2D[] textureRefs;

	// Token: 0x04000AAC RID: 2732
	public tk2dSpriteSheetSource[] spriteSheets;

	// Token: 0x04000AAD RID: 2733
	public tk2dSpriteCollectionFont[] fonts;

	// Token: 0x04000AAE RID: 2734
	public tk2dSpriteCollectionDefault defaults;

	// Token: 0x04000AAF RID: 2735
	public List<tk2dSpriteCollectionPlatform> platforms = new List<tk2dSpriteCollectionPlatform>();

	// Token: 0x04000AB0 RID: 2736
	public bool managedSpriteCollection;

	// Token: 0x04000AB1 RID: 2737
	public bool loadable;

	// Token: 0x04000AB2 RID: 2738
	public int maxTextureSize = 1024;

	// Token: 0x04000AB3 RID: 2739
	public bool forceTextureSize;

	// Token: 0x04000AB4 RID: 2740
	public int forcedTextureWidth = 1024;

	// Token: 0x04000AB5 RID: 2741
	public int forcedTextureHeight = 1024;

	// Token: 0x04000AB6 RID: 2742
	public tk2dSpriteCollection.TextureCompression textureCompression;

	// Token: 0x04000AB7 RID: 2743
	public int atlasWidth;

	// Token: 0x04000AB8 RID: 2744
	public int atlasHeight;

	// Token: 0x04000AB9 RID: 2745
	public bool forceSquareAtlas;

	// Token: 0x04000ABA RID: 2746
	public float atlasWastage;

	// Token: 0x04000ABB RID: 2747
	public bool allowMultipleAtlases;

	// Token: 0x04000ABC RID: 2748
	public bool removeDuplicates = true;

	// Token: 0x04000ABD RID: 2749
	public tk2dSpriteCollectionDefinition[] textureParams;

	// Token: 0x04000ABE RID: 2750
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000ABF RID: 2751
	public bool premultipliedAlpha;

	// Token: 0x04000AC0 RID: 2752
	public Material[] altMaterials;

	// Token: 0x04000AC1 RID: 2753
	public Material[] atlasMaterials;

	// Token: 0x04000AC2 RID: 2754
	public Texture2D[] atlasTextures;

	// Token: 0x04000AC3 RID: 2755
	[SerializeField]
	private bool useTk2dCamera;

	// Token: 0x04000AC4 RID: 2756
	[SerializeField]
	private int targetHeight = 640;

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	private float targetOrthoSize = 10f;

	// Token: 0x04000AC6 RID: 2758
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x04000AC7 RID: 2759
	public float globalScale = 1f;

	// Token: 0x04000AC8 RID: 2760
	public float globalTextureRescale = 1f;

	// Token: 0x04000AC9 RID: 2761
	public List<tk2dSpriteCollection.AttachPointTestSprite> attachPointTestSprites = new List<tk2dSpriteCollection.AttachPointTestSprite>();

	// Token: 0x04000ACA RID: 2762
	[SerializeField]
	private bool pixelPerfectPointSampled;

	// Token: 0x04000ACB RID: 2763
	public FilterMode filterMode = FilterMode.Bilinear;

	// Token: 0x04000ACC RID: 2764
	public TextureWrapMode wrapMode = TextureWrapMode.Clamp;

	// Token: 0x04000ACD RID: 2765
	public bool userDefinedTextureSettings;

	// Token: 0x04000ACE RID: 2766
	public bool mipmapEnabled;

	// Token: 0x04000ACF RID: 2767
	public int anisoLevel = 1;

	// Token: 0x04000AD0 RID: 2768
	public float physicsDepth = 0.1f;

	// Token: 0x04000AD1 RID: 2769
	public bool disableTrimming;

	// Token: 0x04000AD2 RID: 2770
	public tk2dSpriteCollection.NormalGenerationMode normalGenerationMode;

	// Token: 0x04000AD3 RID: 2771
	public int padAmount = -1;

	// Token: 0x04000AD4 RID: 2772
	public bool autoUpdate = true;

	// Token: 0x04000AD5 RID: 2773
	public float editorDisplayScale = 1f;

	// Token: 0x04000AD6 RID: 2774
	public int version;

	// Token: 0x04000AD7 RID: 2775
	public string assetName = string.Empty;

	// Token: 0x02000182 RID: 386
	public enum NormalGenerationMode
	{
		// Token: 0x04000AD9 RID: 2777
		None,
		// Token: 0x04000ADA RID: 2778
		NormalsOnly,
		// Token: 0x04000ADB RID: 2779
		NormalsAndTangents
	}

	// Token: 0x02000183 RID: 387
	public enum TextureCompression
	{
		// Token: 0x04000ADD RID: 2781
		Uncompressed,
		// Token: 0x04000ADE RID: 2782
		Reduced16Bit,
		// Token: 0x04000ADF RID: 2783
		Compressed,
		// Token: 0x04000AE0 RID: 2784
		Dithered16Bit_Alpha,
		// Token: 0x04000AE1 RID: 2785
		Dithered16Bit_NoAlpha
	}

	// Token: 0x02000184 RID: 388
	[Serializable]
	public class AttachPointTestSprite
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x00009563 File Offset: 0x00007763
		public bool CompareTo(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			return src.attachPointName == this.attachPointName && src.spriteCollection == this.spriteCollection && src.spriteId == this.spriteId;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000095A2 File Offset: 0x000077A2
		public void CopyFrom(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			this.attachPointName = src.attachPointName;
			this.spriteCollection = src.spriteCollection;
			this.spriteId = src.spriteId;
		}

		// Token: 0x04000AE2 RID: 2786
		public string attachPointName = string.Empty;

		// Token: 0x04000AE3 RID: 2787
		public tk2dSpriteCollectionData spriteCollection;

		// Token: 0x04000AE4 RID: 2788
		public int spriteId = -1;
	}
}
