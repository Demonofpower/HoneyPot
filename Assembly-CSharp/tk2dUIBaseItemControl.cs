using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[AddComponentMenu("2D Toolkit/UI/tk2dUIBaseItemControl")]
public abstract class tk2dUIBaseItemControl : MonoBehaviour
{
	// Token: 0x06000A75 RID: 2677 RVA: 0x0000A1E0 File Offset: 0x000083E0
	public static void ChangeGameObjectActiveState(GameObject go, bool isActive)
	{
		go.SetActive(isActive);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0000A1E9 File Offset: 0x000083E9
	public static void ChangeGameObjectActiveStateWithNullCheck(GameObject go, bool isActive)
	{
		if (go != null)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(go, isActive);
		}
	}

	// Token: 0x04000BCA RID: 3018
	public tk2dUIItem uiItem;
}
