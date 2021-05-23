using System;

// Token: 0x0200006E RID: 110
public class LoadScreenSaveFile : DisplayObject
{
	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000349 RID: 841 RVA: 0x00004B5C File Offset: 0x00002D5C
	// (remove) Token: 0x0600034A RID: 842 RVA: 0x00004B75 File Offset: 0x00002D75
	public event LoadScreenSaveFile.LoadScreenSaveFileDelegate StartGameMaleEvent;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x0600034B RID: 843 RVA: 0x00004B8E File Offset: 0x00002D8E
	// (remove) Token: 0x0600034C RID: 844 RVA: 0x00004BA7 File Offset: 0x00002DA7
	public event LoadScreenSaveFile.LoadScreenSaveFileDelegate StartGameFemaleEvent;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x0600034D RID: 845 RVA: 0x00004BC0 File Offset: 0x00002DC0
	// (remove) Token: 0x0600034E RID: 846 RVA: 0x00004BD9 File Offset: 0x00002DD9
	public event LoadScreenSaveFile.LoadScreenSaveFileDelegate ContinueGameEvent;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x0600034F RID: 847 RVA: 0x00004BF2 File Offset: 0x00002DF2
	// (remove) Token: 0x06000350 RID: 848 RVA: 0x00004C0B File Offset: 0x00002E0B
	public event LoadScreenSaveFile.LoadScreenSaveFileDelegate EraseGameEvent;

	// Token: 0x06000351 RID: 849 RVA: 0x0001D388 File Offset: 0x0001B588
	public void Init(int fileIndex, LoadScreen loadScreenRef)
	{
		this.titleLabel = (base.GetChildByName("SaveFileTitleLabel") as LabelObject);
		this.noDataContainer = base.GetChildByName("SaveFileDataNone");
		this.dataContainer = base.GetChildByName("SaveFileData");
		this.dataDateLabel = (this.dataContainer.GetChildByName("SaveFileDataDateLabel") as LabelObject);
		this.dataTimeLabel = (this.dataContainer.GetChildByName("SaveFileDataTimeLabel") as LabelObject);
		this.dataLocationLabel = (this.dataContainer.GetChildByName("SaveFileDataLocationLabel") as LabelObject);
		this.dataGirlLabel = (this.dataContainer.GetChildByName("SaveFileDataGirlLabel") as LabelObject);
		this.moneyLabel = (base.GetChildByName("SaveFileMoneyLabel") as LabelObject);
		this.hunieLabel = (base.GetChildByName("SaveFileHunieLabel") as LabelObject);
		this.percentLabel = (base.GetChildByName("SaveFilePercentLabel") as LabelObject);
		this.noDataButtonContainer = base.GetChildByName("SaveFileDataNoneButtons");
		this.dataButtonContainer = base.GetChildByName("SaveFileDataButtons");
		this._startMaleButton = (this.noDataButtonContainer.GetChildByName("SaveFileStartMaleButton") as SpriteObject);
		this._startFemaleButton = (this.noDataButtonContainer.GetChildByName("SaveFileStartFemaleButton") as SpriteObject);
		this._continueGameButton = (this.dataButtonContainer.GetChildByName("SaveFileContinueButton") as SpriteObject);
		this._eraseGameButton = (this.dataButtonContainer.GetChildByName("SaveFileEraseButton") as SpriteObject);
		this._startMaleButton.button.ButtonPressedEvent += this.OnStartMaleButtonPressed;
		this._startFemaleButton.button.ButtonPressedEvent += this.OnStartFemaleButtonPressed;
		this._continueGameButton.button.ButtonPressedEvent += this.OnContinueButtonPressed;
		this._eraseGameButton.button.ButtonPressedEvent += this.OnEraseButtonPressed;
		this._saveFileIndex = fileIndex;
		this._saveFile = SaveUtils.GetSaveFile(this._saveFileIndex);
		this._loadScreenRef = loadScreenRef;
		this.origY = base.localY;
		base.localY = this.origY - 60f;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0001D5B8 File Offset: 0x0001B7B8
	public void Refresh()
	{
		this.titleLabel.SetText("Save Data #" + (this._saveFileIndex + 1).ToString());
		if (this._saveFile.started)
		{
			this.noDataContainer.gameObj.SetActive(false);
			this.dataContainer.gameObj.SetActive(true);
			string text = StringUtils.Titleize(GameManager.System.Clock.Weekday(this._saveFile.totalMinutesElapsed, true).ToString());
			int num = GameManager.System.Clock.CalendarDay(this._saveFile.totalMinutesElapsed, true, true);
			if (num > 0)
			{
				text = text + " " + StringUtils.FormatIntWithDigitCount(num, 2);
			}
			this.dataDateLabel.SetText(text);
			this.dataTimeLabel.SetText(StringUtils.Titleize(GameManager.System.Clock.DayTime(this._saveFile.totalMinutesElapsed).ToString().ToLower()));
			this.dataLocationLabel.SetText(GameManager.Data.Locations.Get(this._saveFile.currentLocation).fullName);
			this.dataGirlLabel.SetText(GameManager.Data.Girls.Get(this._saveFile.currentGirl).firstName);
			this.moneyLabel.SetText(this._saveFile.money, 0f, false, 1f);
			this.hunieLabel.SetText(this._saveFile.hunie, 0f, false, 1f);
			this.percentLabel.SetText(this._saveFile.GetPercentComplete() + "%");
			this.noDataButtonContainer.gameObj.SetActive(false);
			this.dataButtonContainer.gameObj.SetActive(true);
			if (this._loadScreenRef.eraseMode)
			{
				this._eraseGameButton.gameObj.SetActive(true);
				this._continueGameButton.gameObj.SetActive(false);
			}
			else
			{
				this._eraseGameButton.gameObj.SetActive(false);
				this._continueGameButton.gameObj.SetActive(true);
			}
		}
		else
		{
			this.noDataContainer.gameObj.SetActive(true);
			this.dataContainer.gameObj.SetActive(false);
			this.moneyLabel.SetText(0, 0f, false, 1f);
			this.hunieLabel.SetText(0, 0f, false, 1f);
			this.percentLabel.SetText("0%");
			this.noDataButtonContainer.gameObj.SetActive(true);
			this.dataButtonContainer.gameObj.SetActive(false);
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00004C24 File Offset: 0x00002E24
	private void OnStartMaleButtonPressed(ButtonObject buttonObject)
	{
		if (this.StartGameMaleEvent != null)
		{
			this.StartGameMaleEvent(this._saveFileIndex);
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00004C42 File Offset: 0x00002E42
	private void OnStartFemaleButtonPressed(ButtonObject buttonObject)
	{
		if (this.StartGameFemaleEvent != null)
		{
			this.StartGameFemaleEvent(this._saveFileIndex);
		}
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00004C60 File Offset: 0x00002E60
	private void OnContinueButtonPressed(ButtonObject buttonObject)
	{
		if (this.ContinueGameEvent != null)
		{
			this.ContinueGameEvent(this._saveFileIndex);
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00004C7E File Offset: 0x00002E7E
	private void OnEraseButtonPressed(ButtonObject buttonObject)
	{
		if (this.EraseGameEvent != null)
		{
			this.EraseGameEvent(this._saveFileIndex);
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0001D884 File Offset: 0x0001BA84
	protected override void Destructor()
	{
		base.Destructor();
		this.noDataContainer.gameObj.SetActive(true);
		this.dataContainer.gameObj.SetActive(true);
		this.noDataButtonContainer.gameObj.SetActive(true);
		this.dataButtonContainer.gameObj.SetActive(true);
		this._eraseGameButton.gameObj.SetActive(true);
		this._continueGameButton.gameObj.SetActive(true);
		this._startMaleButton.button.ButtonPressedEvent -= this.OnStartMaleButtonPressed;
		this._startFemaleButton.button.ButtonPressedEvent -= this.OnStartFemaleButtonPressed;
		this._continueGameButton.button.ButtonPressedEvent -= this.OnContinueButtonPressed;
		this._eraseGameButton.button.ButtonPressedEvent -= this.OnEraseButtonPressed;
	}

	// Token: 0x0400031D RID: 797
	public LabelObject titleLabel;

	// Token: 0x0400031E RID: 798
	public DisplayObject noDataContainer;

	// Token: 0x0400031F RID: 799
	public DisplayObject dataContainer;

	// Token: 0x04000320 RID: 800
	public LabelObject dataDateLabel;

	// Token: 0x04000321 RID: 801
	public LabelObject dataTimeLabel;

	// Token: 0x04000322 RID: 802
	public LabelObject dataLocationLabel;

	// Token: 0x04000323 RID: 803
	public LabelObject dataGirlLabel;

	// Token: 0x04000324 RID: 804
	public LabelObject moneyLabel;

	// Token: 0x04000325 RID: 805
	public LabelObject hunieLabel;

	// Token: 0x04000326 RID: 806
	public LabelObject percentLabel;

	// Token: 0x04000327 RID: 807
	public DisplayObject noDataButtonContainer;

	// Token: 0x04000328 RID: 808
	public DisplayObject dataButtonContainer;

	// Token: 0x04000329 RID: 809
	public float origY;

	// Token: 0x0400032A RID: 810
	private SpriteObject _startMaleButton;

	// Token: 0x0400032B RID: 811
	private SpriteObject _startFemaleButton;

	// Token: 0x0400032C RID: 812
	private SpriteObject _continueGameButton;

	// Token: 0x0400032D RID: 813
	private SpriteObject _eraseGameButton;

	// Token: 0x0400032E RID: 814
	private int _saveFileIndex;

	// Token: 0x0400032F RID: 815
	private SaveFile _saveFile;

	// Token: 0x04000330 RID: 816
	private LoadScreen _loadScreenRef;

	// Token: 0x0200006F RID: 111
	// (Invoke) Token: 0x0600035A RID: 858
	public delegate void LoadScreenSaveFileDelegate(int saveFileIndex);
}
