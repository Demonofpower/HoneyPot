using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class ResourceUtils
{
	// Token: 0x0600078C RID: 1932 RVA: 0x00007A29 File Offset: 0x00005C29
	public static T LoadPrefab<T>(string category, string prefabName) where T : Component
	{
		return (UnityEngine.Object.Instantiate(ResourceUtils.GetPrefab(category, prefabName)) as GameObject).GetComponent<T>();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00007A41 File Offset: 0x00005C41
	public static GameObject GetPrefab(string category, string prefabName)
	{
		return Resources.Load("Prefabs/" + category + "/" + prefabName) as GameObject;
	}

	// Token: 0x0400089E RID: 2206
	public const string PREFAB_CATEGORY_UI = "UI";
}
