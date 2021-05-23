using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class GameManager
{
	// Token: 0x0600064E RID: 1614 RVA: 0x00031284 File Offset: 0x0002F484
	public GameManager()
	{
		this.settingsMusicVol = 10;
		this.settingsSoundVol = 10;
		this.settingsVoiceVol = 10;
		this.settingsVoice = SettingsVoice.FULL_VOICE;
		this.settingsScreenFull = false;
		this.settingsScreenAspect = false;
		this.settingsScreenSize = -1;
		this.settingsLimit = SettingsLimit.HIGH;
		this.settingsCensored = true;
		this._isGamePauseable = false;
		this._paused = false;
		this._pauseTimestamp = 0f;
		this._totalPausedTime = 0f;
		this._ready = false;
		this._gameState = GameState.LOADING;
		this._Loader = new GameObject();
		this._Loader.AddComponent<Paranoia>();
		UnityEngine.Object.DontDestroyOnLoad(this._Loader);
	}

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x0600064F RID: 1615 RVA: 0x00031330 File Offset: 0x0002F530
	// (remove) Token: 0x06000650 RID: 1616 RVA: 0x00031368 File Offset: 0x0002F568
	public event GameManager.GameManagerDelegate GamePauseEvent;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06000651 RID: 1617 RVA: 0x000313A0 File Offset: 0x0002F5A0
	// (remove) Token: 0x06000652 RID: 1618 RVA: 0x000313D8 File Offset: 0x0002F5D8
	public event GameManager.GameManagerDelegate GameUnpauseEvent;

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000653 RID: 1619 RVA: 0x00006C6E File Offset: 0x00004E6E
	public static GameManager System
	{
		get
		{
			if (GameManager._gameManager == null)
			{
				GameManager._gameManager = new GameManager();
			}
			return GameManager._gameManager;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x00006C86 File Offset: 0x00004E86
	public static GameData Data
	{
		get
		{
			if (GameManager._gameData == null)
			{
				GameManager._gameData = new GameData();
			}
			return GameManager._gameData;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x00006C9E File Offset: 0x00004E9E
	public static Localization Loc
	{
		get
		{
			if (GameManager._localization == null)
			{
				GameManager._localization = new Localization();
			}
			return GameManager._localization;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x00006CB6 File Offset: 0x00004EB6
	public static Stage Stage
	{
		get
		{
			if (GameManager._stage == null)
			{
				GameManager._stage = GameObject.Find("Stage").GetComponent<Stage>();
			}
			return GameManager._stage;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000657 RID: 1623 RVA: 0x00006CDE File Offset: 0x00004EDE
	public PlayerManager Player
	{
		get
		{
			return this._player;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x00006CE6 File Offset: 0x00004EE6
	public AudioManager Audio
	{
		get
		{
			return this._audio;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000659 RID: 1625 RVA: 0x00006CEE File Offset: 0x00004EEE
	public LocationManager Location
	{
		get
		{
			return this._location;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600065A RID: 1626 RVA: 0x00006CF6 File Offset: 0x00004EF6
	public CursorManager Cursor
	{
		get
		{
			return this._cursor;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600065B RID: 1627 RVA: 0x00006CFE File Offset: 0x00004EFE
	public ClockManager Clock
	{
		get
		{
			return this._clock;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600065C RID: 1628 RVA: 0x00006D06 File Offset: 0x00004F06
	public GirlManager Girl
	{
		get
		{
			return this._girl;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x00006D0E File Offset: 0x00004F0E
	public DialogManager Dialog
	{
		get
		{
			return this._dialog;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x00006D16 File Offset: 0x00004F16
	public LogicManager GameLogic
	{
		get
		{
			return this._gameLogic;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x00006D1E File Offset: 0x00004F1E
	public TimerManager Timers
	{
		get
		{
			return this._timers;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x00006D26 File Offset: 0x00004F26
	public PuzzleManager Puzzle
	{
		get
		{
			return this._puzzle;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x00006D2E File Offset: 0x00004F2E
	public GameManagerHook Hook
	{
		get
		{
			return GameManager._gameManagerHook;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x00006D35 File Offset: 0x00004F35
	// (set) Token: 0x06000663 RID: 1635 RVA: 0x00006D3D File Offset: 0x00004F3D
	public GameState GameState
	{
		get
		{
			return this._gameState;
		}
		set
		{
			this._gameState = value;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x00006D46 File Offset: 0x00004F46
	public SaveFile SaveFile
	{
		get
		{
			return this._saveFile;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x00006D4E File Offset: 0x00004F4E
	// (set) Token: 0x06000666 RID: 1638 RVA: 0x00006D56 File Offset: 0x00004F56
	public bool Pauseable
	{
		get
		{
			return this._isGamePauseable;
		}
		set
		{
			this._isGamePauseable = value;
			if (GameManager.Stage.uiTop != null)
			{
				GameManager.Stage.uiTop.RefreshCellPhoneButton();
			}
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00031410 File Offset: 0x0002F610
	public float Lifetime(bool excludePauseTime = true)
	{
		if (excludePauseTime)
		{
			float num = Time.time - this._totalPausedTime;
			if (this._paused)
			{
				num -= Time.time - this._pauseTimestamp;
			}
			return num;
		}
		return Time.time;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0003144C File Offset: 0x0002F64C
	public void Init(GameManagerHook gameManagerHook)
	{
		GameManager._gameManagerHook = gameManagerHook;
		GameManager._gameManagerHookGameObject = GameManager._gameManagerHook.gameObject;
		this._debugProfile = GameManager._gameManagerHook.debugProfile;
		if (this._debugProfile == null)
		{
			SaveUtils.Init();
			SaveData saveData = SaveUtils.GetSaveData();
			this.settingsMusicVol = saveData.settingsMusicVol;
			this.settingsSoundVol = saveData.settingsSoundVol;
			this.settingsVoiceVol = saveData.settingsVoiceVol;
			this.settingsVoice = (SettingsVoice)saveData.settingsVoice;
			this.settingsScreenFull = saveData.settingsScreenFull;
			this.settingsScreenAspect = saveData.settingsScreenAspect;
			this.settingsScreenSize = saveData.settingsScreenSize;
			this.settingsLimit = (SettingsLimit)saveData.settingsLimit;
			this.settingsCensored = saveData.settingsCensored;
			if (this.settingsCensored && (!GameManager._gameManagerHook.censoredVersion || SaveUtils.UncensorPatchExists()))
			{
				this.settingsCensored = false;
				this.SaveGame(true);
			}
		}
		else
		{
			this._saveFile = SaveUtils.CreateSaveFileFromDebugProfile(this._debugProfile);
			this.settingsMusicVol = this._debugProfile.settingsMusicVol;
			this.settingsSoundVol = this._debugProfile.settingsSoundVol;
			this.settingsVoiceVol = this._debugProfile.settingsVoiceVol;
			this.settingsVoice = this._debugProfile.settingsVoice;
			this.settingsScreenFull = false;
			this.settingsScreenAspect = false;
			this.settingsScreenSize = -1;
			this.settingsLimit = this._debugProfile.settingsLimit;
			this.settingsCensored = GameManager._gameManagerHook.censoredVersion;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(GameManager._gameManagerHook.gameCamera) as GameObject;
		gameObject.name = "GameCamera";
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		this.gameCamera = gameObject.GetComponent<GameCamera>();
		this.gameCamera.Init();
		this._player = GameManager._gameManagerHookGameObject.AddComponent<PlayerManager>();
		this._audio = GameManager._gameManagerHookGameObject.AddComponent<AudioManager>();
		this._location = GameManager._gameManagerHookGameObject.AddComponent<LocationManager>();
		this._cursor = GameManager._gameManagerHookGameObject.AddComponent<CursorManager>();
		this._clock = GameManager._gameManagerHookGameObject.AddComponent<ClockManager>();
		this._girl = GameManager._gameManagerHookGameObject.AddComponent<GirlManager>();
		this._dialog = GameManager._gameManagerHookGameObject.AddComponent<DialogManager>();
		this._gameLogic = GameManager._gameManagerHookGameObject.AddComponent<LogicManager>();
		this._timers = GameManager._gameManagerHookGameObject.AddComponent<TimerManager>();
		this._puzzle = GameManager._gameManagerHookGameObject.AddComponent<PuzzleManager>();
		this._dialog.dialogSpacerChar = GameManager._gameManagerHook.dialogSpacerChar;
		if (GameManager._gameManagerHook.steamBuild)
		{
			GameObject.Find("SteamManager").AddComponent("SteamManager");
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00006D80 File Offset: 0x00004F80
	public bool Inited()
	{
		return GameManager._gameManagerHook != null;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000316D8 File Offset: 0x0002F8D8
	public void Update()
	{
		if (!this._ready)
		{
			this._ready = true;
			DisplayUtils.SetPausable(GameManager._stage.uiTop, false);
			DisplayUtils.SetPausable(GameManager._stage.cellPhone, false);
			DisplayUtils.SetPausable(GameManager._stage.cellNotifications, false);
			DisplayUtils.SetPausable(GameManager._stage.uiTitle, false);
			DisplayUtils.SetPausable(GameManager._stage.transitionScreen, false);
			if (this._saveFile != null)
			{
				this.BeginGameSession();
				return;
			}
			this._gameState = GameState.TITLE;
			GameManager._stage.uiTitle.ShowTitleScreen();
			GameManager._stage.uiTitle.SaveFileSelectedEvent += this.OnSaveFileSelected;
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00006D8D File Offset: 0x00004F8D
	private void OnSaveFileSelected(int saveFileIndex)
	{
		GameManager._stage.uiTitle.SaveFileSelectedEvent -= this.OnSaveFileSelected;
		GameManager._stage.uiTitle.HideTitleScreen();
		this._saveFile = SaveUtils.GetSaveFile(saveFileIndex);
		this.BeginGameSession();
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00006DCB File Offset: 0x00004FCB
	private void BeginGameSession()
	{
		this.LoadGame();
		this._gameState = GameState.SIM;
		this._location.TravelTo(this._player.currentLocation, this._player.currentGirl);
		SteamUtils.CheckAchievements();
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00031788 File Offset: 0x0002F988
	private void LoadGame()
	{
		this._clock.SetTotalMinutesElapsed(this._saveFile.totalMinutesElapsed);
		this._player.ReadSaveFile(this._saveFile);
		if (!this._saveFile.started)
		{
			this._saveFile.started = true;
			this._player.failureDateCount = 0;
			this._player.drinksGivenOut = 0;
			this._player.chatSessionCount = 0;
			this._player.RollNewDay();
			CheatData cheatData = SaveUtils.LoadCheatData();
			if (cheatData != null)
			{
				for (int i = 0; i < cheatData.lovePotionCount; i++)
				{
					this._player.AddItem(GameManager._stage.uiGirl.lovePotionDef, GameManager.System._player.inventory, false, true);
				}
			}
			this.SaveGame(false);
			return;
		}
		if (!SaveUtils.IsInited())
		{
			this._player.RollNewDay();
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00031868 File Offset: 0x0002FA68
	public void SaveGame(bool liteSave = false)
	{
		if (SaveUtils.IsInited())
		{
			SaveData saveData = SaveUtils.GetSaveData();
			saveData.settingsMusicVol = this.settingsMusicVol;
			saveData.settingsSoundVol = this.settingsSoundVol;
			saveData.settingsVoiceVol = this.settingsVoiceVol;
			saveData.settingsVoice = (int)this.settingsVoice;
			saveData.settingsScreenFull = this.settingsScreenFull;
			saveData.settingsScreenAspect = this.settingsScreenAspect;
			saveData.settingsScreenSize = this.settingsScreenSize;
			saveData.settingsLimit = (int)this.settingsLimit;
			saveData.settingsCensored = this.settingsCensored;
			if (!liteSave)
			{
				this._saveFile.totalMinutesElapsed = this._clock.TotalMinutesElapsed(0);
				this._player.WriteSaveFile(this._saveFile);
			}
			SaveUtils.Save();
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00031920 File Offset: 0x0002FB20
	public void Pause()
	{
		if (this._paused || !this.Pauseable)
		{
			return;
		}
		this._paused = true;
		this._pauseTimestamp = Time.time;
		GameManager._stage.Pause();
		this._player.Pause();
		this._audio.Pause();
		this._location.Pause();
		this._girl.Pause();
		this._dialog.Pause();
		this._puzzle.Pause();
		this._timers.Pause();
		if (this.GamePauseEvent != null)
		{
			this.GamePauseEvent();
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x000319BC File Offset: 0x0002FBBC
	public void Unpause()
	{
		if (!this._paused || !this.Pauseable)
		{
			return;
		}
		this._paused = false;
		this._totalPausedTime += Time.time - this._pauseTimestamp;
		GameManager._stage.Unpause();
		this._player.Unpause();
		this._audio.Unpause();
		this._location.Unpause();
		this._girl.Unpause();
		this._dialog.Unpause();
		this._puzzle.Unpause();
		this._timers.Unpause();
		if (this.GameUnpauseEvent != null)
		{
			this.GameUnpauseEvent();
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00006E00 File Offset: 0x00005000
	public void PauseToggle()
	{
		if (this._paused)
		{
			this.Unpause();
			return;
		}
		this.Pause();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00006E17 File Offset: 0x00005017
	public bool IsPaused()
	{
		return this._paused;
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00031A64 File Offset: 0x0002FC64
	public void Destroy()
	{
		GameManager._stage.uiTitle.SaveFileSelectedEvent -= this.OnSaveFileSelected;
		this._saveFile = null;
		this._debugProfile = null;
		UnityEngine.Object.Destroy(this.gameCamera.gameObject);
		this.gameCamera = null;
		this._player = null;
		this._audio = null;
		this._location = null;
		this._cursor = null;
		this._clock = null;
		this._girl = null;
		this._dialog = null;
		this._gameLogic = null;
		this._timers = null;
		this._puzzle = null;
		UnityEngine.Object.Destroy(GameManager._gameManagerHookGameObject);
		GameManager._gameManagerHookGameObject = null;
		GameManager._gameManagerHook = null;
	}

	// Token: 0x040007A8 RID: 1960
	private static GameManager _gameManager;

	// Token: 0x040007A9 RID: 1961
	private static GameData _gameData;

	// Token: 0x040007AA RID: 1962
	private static Localization _localization;

	// Token: 0x040007AB RID: 1963
	private static Stage _stage;

	// Token: 0x040007AC RID: 1964
	private static GameManagerHook _gameManagerHook;

	// Token: 0x040007AD RID: 1965
	private static GameObject _gameManagerHookGameObject;

	// Token: 0x040007AE RID: 1966
	private PlayerManager _player;

	// Token: 0x040007AF RID: 1967
	private AudioManager _audio;

	// Token: 0x040007B0 RID: 1968
	private LocationManager _location;

	// Token: 0x040007B1 RID: 1969
	private CursorManager _cursor;

	// Token: 0x040007B2 RID: 1970
	private ClockManager _clock;

	// Token: 0x040007B3 RID: 1971
	private GirlManager _girl;

	// Token: 0x040007B4 RID: 1972
	private DialogManager _dialog;

	// Token: 0x040007B5 RID: 1973
	private LogicManager _gameLogic;

	// Token: 0x040007B6 RID: 1974
	private TimerManager _timers;

	// Token: 0x040007B7 RID: 1975
	private PuzzleManager _puzzle;

	// Token: 0x040007B8 RID: 1976
	public GameCamera gameCamera;

	// Token: 0x040007B9 RID: 1977
	public int settingsMusicVol;

	// Token: 0x040007BA RID: 1978
	public int settingsSoundVol;

	// Token: 0x040007BB RID: 1979
	public int settingsVoiceVol;

	// Token: 0x040007BC RID: 1980
	public SettingsVoice settingsVoice;

	// Token: 0x040007BD RID: 1981
	public bool settingsScreenFull;

	// Token: 0x040007BE RID: 1982
	public bool settingsScreenAspect;

	// Token: 0x040007BF RID: 1983
	public int settingsScreenSize;

	// Token: 0x040007C0 RID: 1984
	public SettingsLimit settingsLimit;

	// Token: 0x040007C1 RID: 1985
	public bool settingsCensored;

	// Token: 0x040007C2 RID: 1986
	private bool _ready;

	// Token: 0x040007C3 RID: 1987
	private GameState _gameState;

	// Token: 0x040007C4 RID: 1988
	private SaveFile _saveFile;

	// Token: 0x040007C5 RID: 1989
	private DebugProfile _debugProfile;

	// Token: 0x040007C6 RID: 1990
	private bool _isGamePauseable;

	// Token: 0x040007C7 RID: 1991
	private bool _paused;

	// Token: 0x040007C8 RID: 1992
	private float _pauseTimestamp;

	// Token: 0x040007C9 RID: 1993
	private float _totalPausedTime;

	// Token: 0x040007CC RID: 1996
	private GameObject _Loader;

	// Token: 0x0200011C RID: 284
	// (Invoke) Token: 0x06000675 RID: 1653
	public delegate void GameManagerDelegate();
}
