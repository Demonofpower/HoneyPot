using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class LoadScreen : DisplayObject
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000333 RID: 819 RVA: 0x00004A0A File Offset: 0x00002C0A
	// (remove) Token: 0x06000334 RID: 820 RVA: 0x00004A23 File Offset: 0x00002C23
	public event LoadScreen.LoadScreenDelegate SaveFileSelectedEvent;

	// Token: 0x06000335 RID: 821 RVA: 0x0001CA84 File Offset: 0x0001AC84
	public void Init()
	{
		this.background = (base.GetChildByName("LoadScreenBackground") as SpriteObject);
		this.saveFileContainer = base.GetChildByName("SaveFileContainer");
		this.buttonContainer = base.GetChildByName("LoadScreenButtonContainer");
		this.settingsOverlay = (base.GetChildByName("LoadScreenOverlay") as SpriteObject);
		this.settingsDropdown = base.GetChildByName("LoadScreenSettings");
		this.settingsContainer = this.settingsDropdown.GetChildByName("LoadScreenSettingsContainer");
		this.screenNotes = base.GetChildByName("LoadScreenNotes");
		this.eraseMode = false;
		this.saveFiles = new List<LoadScreenSaveFile>();
		for (int i = 0; i < 4; i++)
		{
			LoadScreenSaveFile loadScreenSaveFile = this.saveFileContainer.GetChildByName("SaveFile" + i.ToString()) as LoadScreenSaveFile;
			loadScreenSaveFile.Init(i, this);
			loadScreenSaveFile.StartGameMaleEvent += this.OnStartGameMale;
			loadScreenSaveFile.StartGameFemaleEvent += this.OnStartGameFemale;
			loadScreenSaveFile.ContinueGameEvent += this.OnContinueGame;
			loadScreenSaveFile.EraseGameEvent += this.OnEraseGame;
			this.saveFiles.Add(loadScreenSaveFile);
		}
		this._creditsButton = (this.buttonContainer.GetChildByName("LoadScreenCreditsButton") as SpriteObject);
		this._galleryButton = (this.buttonContainer.GetChildByName("LoadScreenGalleryButton") as SpriteObject);
		this._settingsButton = (this.buttonContainer.GetChildByName("LoadScreenSettingsButton") as SpriteObject);
		this._eraseButton = (this.buttonContainer.GetChildByName("LoadScreenEraseButton") as SpriteObject);
		this._cancelButton = (this.buttonContainer.GetChildByName("LoadScreenCancelButton") as SpriteObject);
		this._creditsButton.button.ButtonPressedEvent += this.OnCreditsButtonPressed;
		this._galleryButton.button.ButtonPressedEvent += this.OnGalleryButtonPressed;
		this._settingsButton.button.ButtonPressedEvent += this.OnSettingsButtonPressed;
		this._eraseButton.button.ButtonPressedEvent += this.OnEraseButtonPressed;
		this._cancelButton.button.ButtonPressedEvent += this.OnCancelButtonPressed;
		this._noteVersion = (this.screenNotes.GetChildByName("LoadScreenNoteVersion") as LabelObject);
		this._noteCensor = (this.screenNotes.GetChildByName("LoadScreenNoteCensor") as LabelObject);
		this._noteVersion.SetText("Version: " + GameManager.System.Hook.buildVersion);
		if (GameManager.System.settingsCensored)
		{
			this._noteCensor.SetText("Steam Build");
		}
		else
		{
			this._noteCensor.SetText("Uncensored");
		}
		this._settingsCellApp = (UnityEngine.Object.Instantiate(this.settingsCellApp) as GameObject).GetComponent<UICellApp>();
		this._settingsCellApp.Init();
		this.settingsContainer.AddChild(this._settingsCellApp);
		this._settingsCellApp.SetLocalPosition(0f, 0f);
		this._settingsCancelButton = (this.settingsDropdown.GetChildByName("LoadScreenSettingsClose") as SpriteObject);
		this._settingsCancelButton.button.ButtonPressedEvent += this.OnSettingsCancelButtonPressed;
		this._settingsCancelButton.button.Disable();
		this.Refresh();
		this.background.SetAlpha(0f, 0f);
		this.screenNotes.SetChildAlpha(0f, 0f);
		this.buttonContainer.SetChildAlpha(0f, 0f);
		for (int j = 0; j < this.saveFiles.Count; j++)
		{
			this.saveFiles[j].SetChildAlpha(0f, 0f);
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0001CE6C File Offset: 0x0001B06C
	public void Refresh()
	{
		for (int i = 0; i < this.saveFiles.Count; i++)
		{
			this.saveFiles[i].Refresh();
		}
		bool flag = false;
		for (int j = 0; j < 4; j++)
		{
			if (SaveUtils.GetSaveFile(j).started)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this._eraseButton.gameObj.SetActive(false);
			this._cancelButton.gameObj.SetActive(false);
			this.buttonContainer.localX = 600f;
		}
		else
		{
			if (this.eraseMode)
			{
				this._eraseButton.gameObj.SetActive(false);
				this._cancelButton.gameObj.SetActive(true);
			}
			else
			{
				this._eraseButton.gameObj.SetActive(true);
				this._cancelButton.gameObj.SetActive(false);
			}
			this.buttonContainer.localX = 513f;
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0001CF74 File Offset: 0x0001B174
	private void OnStartGameMale(int saveFileIndex)
	{
		SaveFile saveFile = SaveUtils.GetSaveFile(saveFileIndex);
		if (!saveFile.started)
		{
			saveFile.settingsGender = 0;
		}
		if (this.SaveFileSelectedEvent != null)
		{
			this.SaveFileSelectedEvent(saveFileIndex);
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0001CFB4 File Offset: 0x0001B1B4
	private void OnStartGameFemale(int saveFileIndex)
	{
		SaveFile saveFile = SaveUtils.GetSaveFile(saveFileIndex);
		if (!saveFile.started)
		{
			saveFile.settingsGender = 1;
		}
		if (this.SaveFileSelectedEvent != null)
		{
			this.SaveFileSelectedEvent(saveFileIndex);
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00004A3C File Offset: 0x00002C3C
	private void OnContinueGame(int saveFileIndex)
	{
		if (this.SaveFileSelectedEvent != null)
		{
			this.SaveFileSelectedEvent(saveFileIndex);
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00004A55 File Offset: 0x00002C55
	private void OnEraseGame(int saveFileIndex)
	{
		SaveUtils.GetSaveFile(saveFileIndex).ResetFile();
		SaveUtils.Save();
		this.eraseMode = false;
		this.Refresh();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00004A74 File Offset: 0x00002C74
	private void OnCreditsButtonPressed(ButtonObject buttonObject)
	{
		GameManager.Stage.uiCredits.UICreditsClosedEvent += this.OnCreditsScreenClosed;
		GameManager.Stage.uiCredits.ShowCreditsScreen();
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00004AA0 File Offset: 0x00002CA0
	private void OnCreditsScreenClosed()
	{
		GameManager.Stage.uiCredits.UICreditsClosedEvent -= this.OnCreditsScreenClosed;
		GameManager.System.Cursor.SetAbsorber(GameManager.Stage.uiTitle, false);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
	private void OnSettingsButtonPressed(ButtonObject buttonObject)
	{
		TweenUtils.KillSequence(this._settingsSequence, true);
		GameManager.System.Cursor.SetAbsorber(this.settingsDropdown, false);
		this.settingsDropdown.localY = 450f;
		this.settingsOverlay.SetAlpha(0f, 0f);
		this._settingsCancelButton.button.Enable();
		this._settingsSequence = new Sequence();
		this._settingsSequence.Insert(0f, HOTween.To(this.settingsDropdown, 0.5f, new TweenParms().Prop("localY", 0).Ease(EaseType.EaseOutSine)));
		this._settingsSequence.Insert(0f, HOTween.To(this.settingsOverlay, 0.5f, new TweenParms().Prop("spriteAlpha", 0.4f).Ease(EaseType.Linear)));
		this._settingsSequence.Play();
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001D0EC File Offset: 0x0001B2EC
	private void OnSettingsCancelButtonPressed(ButtonObject buttonObject)
	{
		TweenUtils.KillSequence(this._settingsSequence, true);
		this._settingsCancelButton.button.Disable();
		this.settingsDropdown.localY = 0f;
		this.settingsOverlay.SetAlpha(0.4f, 0f);
		GameManager.System.Cursor.SetAbsorber(GameManager.Stage.uiTitle, false);
		GameManager.System.SaveGame(true);
		this._settingsSequence = new Sequence();
		this._settingsSequence.Insert(0f, HOTween.To(this.settingsDropdown, 0.5f, new TweenParms().Prop("localY", 450).Ease(EaseType.EaseInSine)));
		this._settingsSequence.Insert(0f, HOTween.To(this.settingsOverlay, 0.5f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._settingsSequence.Play();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00004AD7 File Offset: 0x00002CD7
	private void OnGalleryButtonPressed(ButtonObject buttonObject)
	{
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent += this.OnPhotoGalleryClosed;
		GameManager.Stage.uiPhotoGallery.ShowPhotoGallery(null, -1, false, true);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00004B07 File Offset: 0x00002D07
	private void OnPhotoGalleryClosed()
	{
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent -= this.OnPhotoGalleryClosed;
		GameManager.System.Cursor.SetAbsorber(GameManager.Stage.uiTitle, false);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00004B3E File Offset: 0x00002D3E
	private void OnEraseButtonPressed(ButtonObject buttonObject)
	{
		this.eraseMode = true;
		this.Refresh();
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00004B4D File Offset: 0x00002D4D
	private void OnCancelButtonPressed(ButtonObject buttonObject)
	{
		this.eraseMode = false;
		this.Refresh();
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._settingsSequence, true);
		this._settingsSequence = null;
		this._eraseButton.gameObj.SetActive(true);
		this._cancelButton.gameObj.SetActive(true);
		for (int i = 0; i < this.saveFiles.Count; i++)
		{
			this.saveFiles[i].StartGameMaleEvent -= this.OnStartGameMale;
			this.saveFiles[i].StartGameFemaleEvent -= this.OnStartGameFemale;
			this.saveFiles[i].ContinueGameEvent -= this.OnContinueGame;
			this.saveFiles[i].EraseGameEvent -= this.OnEraseGame;
		}
		this.saveFiles.Clear();
		this.saveFiles = null;
		this._creditsButton.button.ButtonPressedEvent -= this.OnCreditsButtonPressed;
		this._galleryButton.button.ButtonPressedEvent -= this.OnGalleryButtonPressed;
		this._settingsButton.button.ButtonPressedEvent -= this.OnSettingsButtonPressed;
		this._eraseButton.button.ButtonPressedEvent -= this.OnEraseButtonPressed;
		this._cancelButton.button.ButtonPressedEvent -= this.OnCancelButtonPressed;
		this._settingsCancelButton.button.ButtonPressedEvent -= this.OnSettingsCancelButtonPressed;
	}

	// Token: 0x04000308 RID: 776
	public GameObject settingsCellApp;

	// Token: 0x04000309 RID: 777
	public SpriteObject background;

	// Token: 0x0400030A RID: 778
	public DisplayObject saveFileContainer;

	// Token: 0x0400030B RID: 779
	public DisplayObject buttonContainer;

	// Token: 0x0400030C RID: 780
	public List<LoadScreenSaveFile> saveFiles;

	// Token: 0x0400030D RID: 781
	public SpriteObject settingsOverlay;

	// Token: 0x0400030E RID: 782
	public DisplayObject settingsDropdown;

	// Token: 0x0400030F RID: 783
	public DisplayObject settingsContainer;

	// Token: 0x04000310 RID: 784
	public DisplayObject screenNotes;

	// Token: 0x04000311 RID: 785
	public bool eraseMode;

	// Token: 0x04000312 RID: 786
	private SpriteObject _creditsButton;

	// Token: 0x04000313 RID: 787
	private SpriteObject _galleryButton;

	// Token: 0x04000314 RID: 788
	private SpriteObject _settingsButton;

	// Token: 0x04000315 RID: 789
	private SpriteObject _eraseButton;

	// Token: 0x04000316 RID: 790
	private SpriteObject _cancelButton;

	// Token: 0x04000317 RID: 791
	private LabelObject _noteVersion;

	// Token: 0x04000318 RID: 792
	private LabelObject _noteCensor;

	// Token: 0x04000319 RID: 793
	private UICellApp _settingsCellApp;

	// Token: 0x0400031A RID: 794
	private Sequence _settingsSequence;

	// Token: 0x0400031B RID: 795
	private SpriteObject _settingsCancelButton;

	// Token: 0x0200006D RID: 109
	// (Invoke) Token: 0x06000345 RID: 837
	public delegate void LoadScreenDelegate(int saveFileIndex);
}
