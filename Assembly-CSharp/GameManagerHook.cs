using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class GameManagerHook : MonoBehaviour
{
	// Token: 0x06000679 RID: 1657 RVA: 0x00006E1F File Offset: 0x0000501F
	private void Awake()
	{
		if (GameManager.System.Inited())
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			GameManager.System.Init(this);
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00006E56 File Offset: 0x00005056
	private void Update()
	{
		GameManager.System.Update();
	}

	// Token: 0x040007E0 RID: 2016
	public GameObject gameCamera;

	// Token: 0x040007E1 RID: 2017
	public DebugProfile debugProfile;

	// Token: 0x040007E2 RID: 2018
	public string dialogSpacerChar;

	// Token: 0x040007E3 RID: 2019
	public bool skipTransitionScreen;

	// Token: 0x040007E4 RID: 2020
	public bool steamBuild;

	// Token: 0x040007E5 RID: 2021
	public bool censoredVersion;

	// Token: 0x040007E6 RID: 2022
	public string buildVersion;
}
