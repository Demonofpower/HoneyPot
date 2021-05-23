using System;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class tk2dSystem : ScriptableObject
{
	// Token: 0x06000853 RID: 2131 RVA: 0x0003CE50 File Offset: 0x0003B050
	private tk2dSystem()
	{
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000855 RID: 2133 RVA: 0x0003CEB4 File Offset: 0x0003B0B4
	public static tk2dSystem inst
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = (Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem);
				if (tk2dSystem._inst == null)
				{
					tk2dSystem._inst = ScriptableObject.CreateInstance<tk2dSystem>();
				}
				UnityEngine.Object.DontDestroyOnLoad(tk2dSystem._inst);
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x00008558 File Offset: 0x00006758
	public static tk2dSystem inst_NoCreate
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = (Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem);
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x0000858D File Offset: 0x0000678D
	// (set) Token: 0x06000858 RID: 2136 RVA: 0x00008594 File Offset: 0x00006794
	public static string CurrentPlatform
	{
		get
		{
			return tk2dSystem.currentPlatform;
		}
		set
		{
			if (value != tk2dSystem.currentPlatform)
			{
				tk2dSystem.currentPlatform = value;
			}
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000859 RID: 2137 RVA: 0x00004EF7 File Offset: 0x000030F7
	public static bool OverrideBuildMaterial
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0003CF18 File Offset: 0x0003B118
	public static tk2dAssetPlatform GetAssetPlatform(string platform)
	{
		tk2dSystem inst_NoCreate = tk2dSystem.inst_NoCreate;
		if (inst_NoCreate == null)
		{
			return null;
		}
		for (int i = 0; i < inst_NoCreate.assetPlatforms.Length; i++)
		{
			if (inst_NoCreate.assetPlatforms[i].name == platform)
			{
				return inst_NoCreate.assetPlatforms[i];
			}
		}
		return null;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0003CF74 File Offset: 0x0003B174
	private T LoadResourceByGUIDImpl<T>(string guid) where T : UnityEngine.Object
	{
		tk2dResource tk2dResource = Resources.Load("tk2d/tk2d_" + guid, typeof(tk2dResource)) as tk2dResource;
		if (tk2dResource != null)
		{
			return tk2dResource.objectReference as T;
		}
		return (T)((object)null);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0003CFC4 File Offset: 0x0003B1C4
	private T LoadResourceByNameImpl<T>(string name) where T : UnityEngine.Object
	{
		for (int i = 0; i < this.allResourceEntries.Length; i++)
		{
			if (this.allResourceEntries[i] != null && this.allResourceEntries[i].assetName == name)
			{
				return this.LoadResourceByGUIDImpl<T>(this.allResourceEntries[i].assetGUID);
			}
		}
		return (T)((object)null);
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x000085AC File Offset: 0x000067AC
	public static T LoadResourceByGUID<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByGUIDImpl<T>(guid);
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000085B9 File Offset: 0x000067B9
	public static T LoadResourceByName<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByNameImpl<T>(guid);
	}

	// Token: 0x040009B8 RID: 2488
	public const string guidPrefix = "tk2d/tk2d_";

	// Token: 0x040009B9 RID: 2489
	public const string assetName = "tk2d/tk2dSystem";

	// Token: 0x040009BA RID: 2490
	public const string assetFileName = "tk2dSystem.asset";

	// Token: 0x040009BB RID: 2491
	[NonSerialized]
	public tk2dAssetPlatform[] assetPlatforms = new tk2dAssetPlatform[]
	{
		new tk2dAssetPlatform("1x", 1f),
		new tk2dAssetPlatform("2x", 2f),
		new tk2dAssetPlatform("4x", 4f)
	};

	// Token: 0x040009BC RID: 2492
	private static tk2dSystem _inst;

	// Token: 0x040009BD RID: 2493
	private static string currentPlatform = string.Empty;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private tk2dResourceTocEntry[] allResourceEntries = new tk2dResourceTocEntry[0];
}
