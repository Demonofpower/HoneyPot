using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000153 RID: 339
[AddComponentMenu("2D Toolkit/Backend/tk2dFontData")]
public class tk2dFontData : MonoBehaviour
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x00039CDC File Offset: 0x00037EDC
	public tk2dFontData inst
	{
		get
		{
			if (this.platformSpecificData == null || this.platformSpecificData.materialInst == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.fontPlatforms.Length; i++)
					{
						if (this.fontPlatforms[i] == currentPlatform)
						{
							text = this.fontPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.fontPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dFontData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
				this.platformSpecificData.Init();
			}
			return this.platformSpecificData;
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00039DA0 File Offset: 0x00037FA0
	private void Init()
	{
		if (this.needMaterialInstance)
		{
			if (this.spriteCollection)
			{
				tk2dSpriteCollectionData inst = this.spriteCollection.inst;
				for (int i = 0; i < inst.materials.Length; i++)
				{
					if (inst.materials[i] == this.material)
					{
						this.materialInst = inst.materialInsts[i];
						break;
					}
				}
				if (this.materialInst == null)
				{
					Debug.LogError("Fatal error - font from sprite collection is has an invalid material");
				}
			}
			else
			{
				this.materialInst = (UnityEngine.Object.Instantiate(this.material) as Material);
				this.materialInst.hideFlags = HideFlags.DontSave;
			}
		}
		else
		{
			this.materialInst = this.material;
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00007E60 File Offset: 0x00006060
	public void ResetPlatformData()
	{
		if (this.hasPlatformData && this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInst = null;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00007E8B File Offset: 0x0000608B
	private void OnDestroy()
	{
		if (this.needMaterialInstance && this.spriteCollection == null)
		{
			UnityEngine.Object.DestroyImmediate(this.materialInst);
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00039E6C File Offset: 0x0003806C
	public void InitDictionary()
	{
		if (this.useDictionary && this.charDict == null)
		{
			this.charDict = new Dictionary<int, tk2dFontChar>(this.charDictKeys.Count);
			for (int i = 0; i < this.charDictKeys.Count; i++)
			{
				this.charDict[this.charDictKeys[i]] = this.charDictValues[i];
			}
		}
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00039EE4 File Offset: 0x000380E4
	public void SetDictionary(Dictionary<int, tk2dFontChar> dict)
	{
		this.charDictKeys = new List<int>(dict.Keys);
		this.charDictValues = new List<tk2dFontChar>();
		for (int i = 0; i < this.charDictKeys.Count; i++)
		{
			this.charDictValues.Add(dict[this.charDictKeys[i]]);
		}
	}

	// Token: 0x04000926 RID: 2342
	public const int CURRENT_VERSION = 2;

	// Token: 0x04000927 RID: 2343
	[HideInInspector]
	public int version;

	// Token: 0x04000928 RID: 2344
	public float lineHeight;

	// Token: 0x04000929 RID: 2345
	public tk2dFontChar[] chars;

	// Token: 0x0400092A RID: 2346
	[SerializeField]
	private List<int> charDictKeys;

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	private List<tk2dFontChar> charDictValues;

	// Token: 0x0400092C RID: 2348
	public string[] fontPlatforms;

	// Token: 0x0400092D RID: 2349
	public string[] fontPlatformGUIDs;

	// Token: 0x0400092E RID: 2350
	private tk2dFontData platformSpecificData;

	// Token: 0x0400092F RID: 2351
	public bool hasPlatformData;

	// Token: 0x04000930 RID: 2352
	public bool managedFont;

	// Token: 0x04000931 RID: 2353
	public bool needMaterialInstance;

	// Token: 0x04000932 RID: 2354
	public bool isPacked;

	// Token: 0x04000933 RID: 2355
	public bool premultipliedAlpha;

	// Token: 0x04000934 RID: 2356
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000935 RID: 2357
	public Dictionary<int, tk2dFontChar> charDict;

	// Token: 0x04000936 RID: 2358
	public bool useDictionary;

	// Token: 0x04000937 RID: 2359
	public tk2dFontKerning[] kerning;

	// Token: 0x04000938 RID: 2360
	public float largestWidth;

	// Token: 0x04000939 RID: 2361
	public Material material;

	// Token: 0x0400093A RID: 2362
	[NonSerialized]
	public Material materialInst;

	// Token: 0x0400093B RID: 2363
	public Texture2D gradientTexture;

	// Token: 0x0400093C RID: 2364
	public bool textureGradients;

	// Token: 0x0400093D RID: 2365
	public int gradientCount = 1;

	// Token: 0x0400093E RID: 2366
	public Vector2 texelSize;

	// Token: 0x0400093F RID: 2367
	[HideInInspector]
	public float invOrthoSize = 1f;

	// Token: 0x04000940 RID: 2368
	[HideInInspector]
	public float halfTargetHeight = 1f;
}
