using System;
using System.Collections.Generic;

// Token: 0x02000040 RID: 64
public class SettingsCellApp : UICellApp
{
	// Token: 0x0600021A RID: 538 RVA: 0x000168A4 File Offset: 0x00014AA4
	public override void Init()
	{
		this.panelsContainer = base.GetChildByName("SettingsPanelsContainer");
		this.windowSizeArrows = base.GetChildByName("SettingsArrows");
		this._settingsPanels = new List<SettingsSwitchPanel>();
		this._settingsPanelVolume = (this.panelsContainer.GetChildByName("SettingsPanelVolume") as SettingsSwitchPanel);
		this._settingsPanelVolume.Init(-1, -1);
		this._settingsPanels.Add(this._settingsPanelVolume);
		this._settingsPanelVoice = (this.panelsContainer.GetChildByName("SettingsPanelVoice") as SettingsSwitchPanel);
		this._settingsPanelVoice.Init(Enum.GetNames(typeof(SettingsVoice)).Length, (int)GameManager.System.settingsVoice);
		this._settingsPanels.Add(this._settingsPanelVoice);
		this._settingsPanelScreen = (this.panelsContainer.GetChildByName("SettingsPanelScreen") as SettingsSwitchPanel);
		this._settingsPanelScreen.Init(2, (!GameManager.System.gameCamera.IsGameFullScreen()) ? 1 : 0);
		this._settingsPanels.Add(this._settingsPanelScreen);
		this._settingsPanelLimit = (this.panelsContainer.GetChildByName("SettingsPanelLimit") as SettingsSwitchPanel);
		this._settingsPanelLimit.Init(Enum.GetNames(typeof(SettingsLimit)).Length, (int)GameManager.System.settingsLimit);
		this._settingsPanels.Add(this._settingsPanelLimit);
		this._settingsPanelDifficulty = (this.panelsContainer.GetChildByName("SettingsPanelDifficulty") as SettingsSwitchPanel);
		if (GameManager.System.GameState != GameState.TITLE)
		{
			if (!GameManager.System.Player.alphaModeActive)
			{
				this._settingsPanelDifficulty.Init(Enum.GetNames(typeof(SettingsDifficulty)).Length, (int)GameManager.System.Player.settingsDifficulty);
			}
			else
			{
				this._settingsPanelDifficulty.Init(Enum.GetNames(typeof(SettingsDifficulty)).Length, 2);
				this._settingsPanelDifficulty.interactive = false;
				this._settingsPanelDifficulty.SetChildAlpha(0.5f, 0f);
			}
			this._settingsPanels.Add(this._settingsPanelDifficulty);
		}
		else
		{
			this._settingsPanelDifficulty.gameObj.SetActive(false);
		}
		this._settingsPanelGender = (this.panelsContainer.GetChildByName("SettingsPanelGender") as SettingsSwitchPanel);
		if (GameManager.System.GameState != GameState.TITLE)
		{
			this._settingsPanelGender.Init(Enum.GetNames(typeof(SettingsGender)).Length, (int)GameManager.System.Player.settingsGender);
			this._settingsPanels.Add(this._settingsPanelGender);
		}
		else
		{
			this._settingsPanelGender.gameObj.SetActive(false);
		}
		for (int i = 0; i < this._settingsPanels.Count; i++)
		{
			this._settingsPanels[i].SettingsSwitchPanelOptionSelectEvent += this.OnPanelOptionSelected;
		}
		this._settingsMeters = new List<SettingsMeter>();
		this._settingsMeterMusicVol = (base.GetChildByName("SettingsMeterMusic") as SettingsMeter);
		this._settingsMeterMusicVol.Init(GameManager.System.settingsMusicVol);
		this._settingsMeters.Add(this._settingsMeterMusicVol);
		this._settingsMeterSoundVol = (base.GetChildByName("SettingsMeterSound") as SettingsMeter);
		this._settingsMeterSoundVol.Init(GameManager.System.settingsSoundVol);
		this._settingsMeters.Add(this._settingsMeterSoundVol);
		this._settingsMeterVoiceVol = (base.GetChildByName("SettingsMeterVoice") as SettingsMeter);
		this._settingsMeterVoiceVol.Init(GameManager.System.settingsVoiceVol);
		this._settingsMeters.Add(this._settingsMeterVoiceVol);
		for (int j = 0; j < this._settingsMeters.Count; j++)
		{
			this._settingsMeters[j].SettingsMeterChangeEvent += this.OnSettingsMeterChanged;
		}
		this._adjustToggle = base.GetChildByName("SettingsToggle");
		this._adjustToggle.button.ButtonPressedEvent += this.OnAdjustAspectTogglePressed;
		this._screenSmallerArrow = (this.windowSizeArrows.GetChildByName("SettingsDownArrow") as SpriteObject);
		this._screenSmallerArrow.button.ButtonPressedEvent += this.OnScreenSmallerArrowPressed;
		this._screenLargerArrow = (this.windowSizeArrows.GetChildByName("SettingsUpArrow") as SpriteObject);
		this._screenLargerArrow.button.ButtonPressedEvent += this.OnScreenLargerArrowPressed;
		this.Refresh();
		base.Init();
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00016D38 File Offset: 0x00014F38
	public override void Refresh()
	{
		for (int i = 0; i < this._settingsPanels.Count; i++)
		{
			this._settingsPanels[i].Refresh();
		}
		for (int j = 0; j < this._settingsMeters.Count; j++)
		{
			this._settingsMeters[j].Refresh();
		}
		if (GameManager.System.gameCamera.IsGameFullScreen())
		{
			this._adjustToggle.gameObj.SetActive(true);
			this.windowSizeArrows.gameObj.SetActive(false);
		}
		else
		{
			this._adjustToggle.gameObj.SetActive(false);
			this.windowSizeArrows.gameObj.SetActive(true);
			if (GameManager.System.gameCamera.IsWindowMinSize())
			{
				this._screenSmallerArrow.button.Disable();
			}
			else
			{
				this._screenSmallerArrow.button.Enable();
			}
			if (GameManager.System.gameCamera.IsWindowMaxSize())
			{
				this._screenLargerArrow.button.Disable();
			}
			else
			{
				this._screenLargerArrow.button.Enable();
			}
		}
		this._settingsPanelScreen.ChangeSelectedOption((!GameManager.System.gameCamera.IsGameFullScreen()) ? 1 : 0);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00016E94 File Offset: 0x00015094
	private void OnPanelOptionSelected(SettingsSwitchPanel settingsSwitchPanel, int optionIndex)
	{
		if (!(settingsSwitchPanel == this._settingsPanelVolume))
		{
			if (settingsSwitchPanel == this._settingsPanelVoice)
			{
				GameManager.System.settingsVoice = (SettingsVoice)optionIndex;
				GameManager.System.Audio.Refresh();
			}
			else if (settingsSwitchPanel == this._settingsPanelScreen)
			{
				if (optionIndex == 0)
				{
					GameManager.System.gameCamera.SetFullScreen(true);
				}
				else
				{
					GameManager.System.gameCamera.SetFullScreen(false);
				}
			}
			else if (settingsSwitchPanel == this._settingsPanelLimit)
			{
				GameManager.System.settingsLimit = (SettingsLimit)optionIndex;
			}
			else if (settingsSwitchPanel == this._settingsPanelDifficulty)
			{
				GameManager.System.Player.settingsDifficulty = (SettingsDifficulty)optionIndex;
			}
			else
			{
				GameManager.System.Player.settingsGender = (SettingsGender)optionIndex;
			}
		}
		this.Refresh();
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00016F88 File Offset: 0x00015188
	private void OnSettingsMeterChanged(SettingsMeter settingsMeter, int meterValue)
	{
		if (settingsMeter == this._settingsMeterMusicVol)
		{
			GameManager.System.settingsMusicVol = meterValue;
		}
		else if (settingsMeter == this._settingsMeterSoundVol)
		{
			GameManager.System.settingsSoundVol = meterValue;
		}
		else
		{
			GameManager.System.settingsVoiceVol = meterValue;
		}
		GameManager.System.Audio.Refresh();
		this.Refresh();
	}

	// Token: 0x0600021E RID: 542 RVA: 0x00003F36 File Offset: 0x00002136
	private void OnAdjustAspectTogglePressed(ButtonObject buttonObject)
	{
		GameManager.System.gameCamera.AdjustAspect();
		this.Refresh();
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00003F4D File Offset: 0x0000214D
	private void OnScreenSmallerArrowPressed(ButtonObject buttonObject)
	{
		GameManager.System.gameCamera.ShrinkWindowSize();
		this.Refresh();
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00003F64 File Offset: 0x00002164
	private void OnScreenLargerArrowPressed(ButtonObject buttonObject)
	{
		GameManager.System.gameCamera.GrowWindowSize();
		this.Refresh();
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00016FF8 File Offset: 0x000151F8
	protected override void Destructor()
	{
		base.Destructor();
		this._adjustToggle.gameObj.SetActive(true);
		this.windowSizeArrows.gameObj.SetActive(true);
		for (int i = 0; i < this._settingsPanels.Count; i++)
		{
			this._settingsPanels[i].SettingsSwitchPanelOptionSelectEvent -= this.OnPanelOptionSelected;
		}
		this._settingsPanels.Clear();
		this._settingsPanels = null;
		for (int j = 0; j < this._settingsMeters.Count; j++)
		{
			this._settingsMeters[j].SettingsMeterChangeEvent -= this.OnSettingsMeterChanged;
		}
		this._settingsMeters.Clear();
		this._settingsMeters = null;
		this._adjustToggle.button.ButtonPressedEvent -= this.OnAdjustAspectTogglePressed;
		this._screenSmallerArrow.button.ButtonPressedEvent -= this.OnScreenSmallerArrowPressed;
		this._screenLargerArrow.button.ButtonPressedEvent -= this.OnScreenLargerArrowPressed;
	}

	// Token: 0x040001C0 RID: 448
	public DisplayObject panelsContainer;

	// Token: 0x040001C1 RID: 449
	public DisplayObject windowSizeArrows;

	// Token: 0x040001C2 RID: 450
	private List<SettingsSwitchPanel> _settingsPanels;

	// Token: 0x040001C3 RID: 451
	private SettingsSwitchPanel _settingsPanelVolume;

	// Token: 0x040001C4 RID: 452
	private SettingsSwitchPanel _settingsPanelVoice;

	// Token: 0x040001C5 RID: 453
	private SettingsSwitchPanel _settingsPanelScreen;

	// Token: 0x040001C6 RID: 454
	private SettingsSwitchPanel _settingsPanelLimit;

	// Token: 0x040001C7 RID: 455
	private SettingsSwitchPanel _settingsPanelDifficulty;

	// Token: 0x040001C8 RID: 456
	private SettingsSwitchPanel _settingsPanelGender;

	// Token: 0x040001C9 RID: 457
	private List<SettingsMeter> _settingsMeters;

	// Token: 0x040001CA RID: 458
	private SettingsMeter _settingsMeterMusicVol;

	// Token: 0x040001CB RID: 459
	private SettingsMeter _settingsMeterSoundVol;

	// Token: 0x040001CC RID: 460
	private SettingsMeter _settingsMeterVoiceVol;

	// Token: 0x040001CD RID: 461
	private DisplayObject _adjustToggle;

	// Token: 0x040001CE RID: 462
	private SpriteObject _screenSmallerArrow;

	// Token: 0x040001CF RID: 463
	private SpriteObject _screenLargerArrow;
}
