using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class Loader
{
	// Token: 0x06000BBD RID: 3005 RVA: 0x0000B6E2 File Offset: 0x000098E2
	public static void Init()
	{
		Loader._Load = new GameObject();
		Loader._Load.AddComponent<Paranoia>();
		UnityEngine.Object.DontDestroyOnLoad(Loader._Load);
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0000B703 File Offset: 0x00009903
	public static void Unload()
	{
		Loader._Unload();
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0000B70A File Offset: 0x0000990A
	private static void _Unload()
	{
		UnityEngine.Object.Destroy(Loader._Load);
	}

	// Token: 0x04000CB7 RID: 3255
	public static GameObject _Load;

	// Token: 0x04000CB8 RID: 3256
	private GameObject _gameObject;
}
