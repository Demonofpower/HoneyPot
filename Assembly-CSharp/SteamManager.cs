using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x0200010D RID: 269
internal class SteamManager : MonoBehaviour
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x00006733 File Offset: 0x00004933
	private static SteamManager Instance
	{
		get
		{
			return SteamManager.s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00006750 File Offset: 0x00004950
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0000675C File Offset: 0x0000495C
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.Log("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.Log("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(339800U)))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.Log("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.Log("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0002E5D0 File Offset: 0x0002C7D0
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00006764 File Offset: 0x00004964
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0000678E File Offset: 0x0000498E
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x0400074D RID: 1869
	private static SteamManager s_instance;

	// Token: 0x0400074E RID: 1870
	private bool m_bInitialized;

	// Token: 0x0400074F RID: 1871
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
