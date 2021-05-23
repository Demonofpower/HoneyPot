using System;
using System.Collections.Generic;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000189 RID: 393
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
public class tk2dSpriteCollectionData : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x000096BD File Offset: 0x000078BD
	// (set) Token: 0x06000982 RID: 2434 RVA: 0x000096C5 File Offset: 0x000078C5
	public bool Transient { get; set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x000096CE File Offset: 0x000078CE
	public int Count
	{
		get
		{
			return this.inst.spriteDefinitions.Length;
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x000096DD File Offset: 0x000078DD
	public int GetSpriteIdByName(string name)
	{
		return this.GetSpriteIdByName(name, 0);
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x000418F0 File Offset: 0x0003FAF0
	public int GetSpriteIdByName(string name, int defaultValue)
	{
		this.inst.InitDictionary();
		int result = defaultValue;
		if (!this.inst.spriteNameLookupDict.TryGetValue(name, out result))
		{
			return defaultValue;
		}
		return result;
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00041928 File Offset: 0x0003FB28
	public tk2dSpriteDefinition GetSpriteDefinition(string name)
	{
		int spriteIdByName = this.GetSpriteIdByName(name, -1);
		if (spriteIdByName == -1)
		{
			return null;
		}
		return this.spriteDefinitions[spriteIdByName];
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00041950 File Offset: 0x0003FB50
	public void InitDictionary()
	{
		if (this.spriteNameLookupDict == null)
		{
			this.spriteNameLookupDict = new Dictionary<string, int>(this.spriteDefinitions.Length);
			for (int i = 0; i < this.spriteDefinitions.Length; i++)
			{
				this.spriteNameLookupDict[this.spriteDefinitions[i].name] = i;
			}
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x000419B0 File Offset: 0x0003FBB0
	public tk2dSpriteDefinition FirstValidDefinition
	{
		get
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (tk2dSpriteDefinition.Valid)
				{
					return tk2dSpriteDefinition;
				}
			}
			return null;
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x000096E7 File Offset: 0x000078E7
	public bool IsValidSpriteId(int id)
	{
		return id >= 0 && id < this.inst.spriteDefinitions.Length && this.inst.spriteDefinitions[id].Valid;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x000419F0 File Offset: 0x0003FBF0
	public int FirstValidDefinitionIndex
	{
		get
		{
			tk2dSpriteCollectionData inst = this.inst;
			for (int i = 0; i < inst.spriteDefinitions.Length; i++)
			{
				if (inst.spriteDefinitions[i].Valid)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00041A34 File Offset: 0x0003FC34
	public void InitMaterialIds()
	{
		if (this.inst.materialIdsValid)
		{
			return;
		}
		int num = -1;
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>();
		for (int i = 0; i < this.inst.materials.Length; i++)
		{
			if (num == -1 && this.inst.materials[i] != null)
			{
				num = i;
			}
			dictionary[this.materials[i]] = i;
		}
		if (num == -1)
		{
			Debug.LogError("Init material ids failed.");
		}
		else
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (!dictionary.TryGetValue(tk2dSpriteDefinition.material, out tk2dSpriteDefinition.materialId))
				{
					tk2dSpriteDefinition.materialId = num;
				}
			}
			this.inst.materialIdsValid = true;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x00041B14 File Offset: 0x0003FD14
	public tk2dSpriteCollectionData inst
	{
		get
		{
			if (this.platformSpecificData == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.spriteCollectionPlatforms.Length; i++)
					{
						if (this.spriteCollectionPlatforms[i] == currentPlatform)
						{
							text = this.spriteCollectionPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.spriteCollectionPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dSpriteCollectionData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
			}
			this.platformSpecificData.Init();
			return this.platformSpecificData;
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00041BC0 File Offset: 0x0003FDC0
	private void Init()
	{
		if (this.materialInsts != null)
		{
			return;
		}
		if (this.spriteDefinitions == null)
		{
			this.spriteDefinitions = new tk2dSpriteDefinition[0];
		}
		if (this.materials == null)
		{
			this.materials = new Material[0];
		}
		this.materialInsts = new Material[this.materials.Length];
		if (this.needMaterialInstance)
		{
			if (tk2dSystem.OverrideBuildMaterial)
			{
				for (int i = 0; i < this.materials.Length; i++)
				{
					this.materialInsts[i] = new Material(Shader.Find("tk2d/BlendVertexColor"));
				}
			}
			else
			{
				for (int j = 0; j < this.materials.Length; j++)
				{
					this.materialInsts[j] = (UnityEngine.Object.Instantiate(this.materials[j]) as Material);
				}
			}
			for (int k = 0; k < this.spriteDefinitions.Length; k++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition = this.spriteDefinitions[k];
				tk2dSpriteDefinition.materialInst = this.materialInsts[tk2dSpriteDefinition.materialId];
			}
		}
		else
		{
			for (int l = 0; l < this.spriteDefinitions.Length; l++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition2 = this.spriteDefinitions[l];
				tk2dSpriteDefinition2.materialInst = tk2dSpriteDefinition2.material;
			}
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00009717 File Offset: 0x00007917
	public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
	{
		return SpriteCollectionGenerator.CreateFromTexture(texture, size, names, regions, anchors);
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00009724 File Offset: 0x00007924
	public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize size, string texturePackerData, Texture texture)
	{
		return SpriteCollectionGenerator.CreateFromTexturePacker(size, texturePackerData, texture);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0000972E File Offset: 0x0000792E
	public void ResetPlatformData()
	{
		if (this.hasPlatformData && this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInsts = null;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00041D08 File Offset: 0x0003FF08
	public void UnloadTextures()
	{
		tk2dSpriteCollectionData inst = this.inst;
		foreach (Texture2D assetToUnload in inst.textures)
		{
			Resources.UnloadAsset(assetToUnload);
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00041D48 File Offset: 0x0003FF48
	private void OnDestroy()
	{
		if (this.Transient)
		{
			foreach (Material obj in this.materials)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
		else if (this.needMaterialInstance)
		{
			foreach (Material obj2 in this.materialInsts)
			{
				UnityEngine.Object.DestroyImmediate(obj2);
			}
		}
		this.ResetPlatformData();
	}

	// Token: 0x04000B0C RID: 2828
	public const int CURRENT_VERSION = 3;

	// Token: 0x04000B0D RID: 2829
	public int version;

	// Token: 0x04000B0E RID: 2830
	public bool materialIdsValid;

	// Token: 0x04000B0F RID: 2831
	public bool needMaterialInstance;

	// Token: 0x04000B10 RID: 2832
	public tk2dSpriteDefinition[] spriteDefinitions;

	// Token: 0x04000B11 RID: 2833
	private Dictionary<string, int> spriteNameLookupDict;

	// Token: 0x04000B12 RID: 2834
	public bool premultipliedAlpha;

	// Token: 0x04000B13 RID: 2835
	public Material material;

	// Token: 0x04000B14 RID: 2836
	public Material[] materials;

	// Token: 0x04000B15 RID: 2837
	[NonSerialized]
	public Material[] materialInsts;

	// Token: 0x04000B16 RID: 2838
	public Texture[] textures;

	// Token: 0x04000B17 RID: 2839
	public bool allowMultipleAtlases;

	// Token: 0x04000B18 RID: 2840
	public string spriteCollectionGUID;

	// Token: 0x04000B19 RID: 2841
	public string spriteCollectionName;

	// Token: 0x04000B1A RID: 2842
	public string assetName = string.Empty;

	// Token: 0x04000B1B RID: 2843
	public bool loadable;

	// Token: 0x04000B1C RID: 2844
	public float invOrthoSize = 1f;

	// Token: 0x04000B1D RID: 2845
	public float halfTargetHeight = 1f;

	// Token: 0x04000B1E RID: 2846
	public int buildKey;

	// Token: 0x04000B1F RID: 2847
	public string dataGuid = string.Empty;

	// Token: 0x04000B20 RID: 2848
	public bool managedSpriteCollection;

	// Token: 0x04000B21 RID: 2849
	public bool hasPlatformData;

	// Token: 0x04000B22 RID: 2850
	public string[] spriteCollectionPlatforms;

	// Token: 0x04000B23 RID: 2851
	public string[] spriteCollectionPlatformGUIDs;

	// Token: 0x04000B24 RID: 2852
	private tk2dSpriteCollectionData platformSpecificData;
}
