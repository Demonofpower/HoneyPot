using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class UICellPhone : DisplayObject
{
	// Token: 0x06000283 RID: 643 RVA: 0x00018E6C File Offset: 0x0001706C
	protected override void OnStart()
	{
		base.OnStart();
		this.cellMemory = new Dictionary<string, int>();
		this.cellPhoneCore = base.GetChildByName("CellPhoneCore");
		this.cellPhoneHeader = this.cellPhoneCore.GetChildByName("CellPhoneHeader");
		this.headerIcons = (this.cellPhoneHeader.GetChildByName("CellPhoneHeaderIcons") as SpriteObject);
		this.headerLabel = (this.cellPhoneHeader.GetChildByName("CellPhoneHeaderLabel") as LabelObject);
		this.backButton = (this.cellPhoneCore.GetChildByName("CellPhoneBackButton") as SpriteObject);
		this.cellAppButtonContainer = this.cellPhoneCore.GetChildByName("CellAppButtonContainer");
		this.cellAppWindow = base.GetChildByName("CellAppWindow");
		this.cellAppBackground = (this.cellAppWindow.GetChildByName("CellAppBackground") as SpriteObject);
		this.cellAppScroller = (this.cellAppWindow.GetChildByName("CellAppScroller") as TiledSpriteObject);
		this.cellAppContainer = this.cellAppWindow.GetChildByName("UICellApps");
		this.cellAppError = this.cellAppWindow.GetChildByName("CellAppError");
		this.cellAppErrorOverlay = (this.cellAppError.GetChildByName("CellAppErrorOverlay") as SpriteObject);
		this.cellAppErrorBackground = (this.cellAppError.GetChildByName("CellAppErrorBackground") as SpriteObject);
		this.cellAppErrorLabel = (this.cellAppError.GetChildByName("CellAppErrorLabel") as LabelObject);
		this._cellAppButtons = new List<UICellAppButton>();
		for (int i = 0; i < 7; i++)
		{
			UICellAppButton uicellAppButton = this.cellAppButtonContainer.GetChildByName("CellAppButton" + i.ToString()) as UICellAppButton;
			this._cellAppButtons.Add(uicellAppButton);
			uicellAppButton.CellAppButtonClickedEvent += this.OnCellAppButtonClicked;
			if (uicellAppButton.cellAppDefinition == this.defaultCellApp)
			{
				this._activeCellAppButton = uicellAppButton;
			}
		}
		this.errorBgOrigY = this.cellAppErrorBackground.localY;
		this.errorLblOrigY = this.cellAppErrorLabel.localY;
		this.HideCellAppError();
		this.backButton.button.ButtonPressedEvent += this.OnCellAppBackButtonClicked;
		this.backButton.button.Disable();
		this.backButton.spriteAlpha = 0f;
		this._closePane = base.GetChildByName("CellPhoneClosePane");
		this._closePane.button.ButtonPressedEvent += this.OnClosePanePressed;
		this._closePane.button.Disable();
		this._closePane.localX = -380f;
		this._secondary = false;
		this._opened = false;
		this._locked = false;
		this._scrollerTimestamp = GameManager.System.Lifetime(false);
		base.interactive = false;
		base.localX = -563f;
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00019140 File Offset: 0x00017340
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._opened && GameManager.System.Lifetime(false) - this._scrollerTimestamp > 0.03f)
		{
			this.cellAppScroller.gameObj.transform.localPosition += new Vector3(-1f, 1f, 0f);
			if (this.cellAppScroller.gameObj.transform.localPosition.x <= -100f)
			{
				this.cellAppScroller.SetLocalPosition(0f, 0f);
			}
			this._scrollerTimestamp = GameManager.System.Lifetime(false);
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00004244 File Offset: 0x00002444
	public void Open()
	{
		if (this._opened || this._locked)
		{
			return;
		}
		this.ChangeCellApp(this._activeCellAppButton, this._secondary);
		this._opened = true;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x000191FC File Offset: 0x000173FC
	public void Close()
	{
		if (!this._opened || this._locked)
		{
			return;
		}
		this._activeCellAppDefinition = null;
		if (this._activeCellApp != null)
		{
			UnityEngine.Object.Destroy(this._activeCellApp.gameObj);
		}
		this._activeCellApp = null;
		this.HideCellAppError();
		this._opened = false;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00004276 File Offset: 0x00002476
	public bool IsOpen()
	{
		return this._opened;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000427E File Offset: 0x0000247E
	private void OnCellAppButtonClicked(UICellAppButton cellAppButton)
	{
		if (this._locked)
		{
			return;
		}
		if (cellAppButton.cellAppDefinition.cellAppUIPrefab != null)
		{
			this.ChangeCellApp(cellAppButton, false);
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000042AA File Offset: 0x000024AA
	private void OnCellAppBackButtonClicked(ButtonObject buttonObject)
	{
		if (this._locked)
		{
			return;
		}
		if (this._activeCellAppDefinition.backToCellApp != null)
		{
			this.ChangeCellApp(this._activeCellAppDefinition.backToCellApp);
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0001925C File Offset: 0x0001745C
	public void ChangeCellApp(CellAppDefinition cellAppDefinition)
	{
		if (this._locked || !this._opened)
		{
			return;
		}
		for (int i = 0; i < this._cellAppButtons.Count; i++)
		{
			if (this._cellAppButtons[i].cellAppDefinition == cellAppDefinition)
			{
				this.ChangeCellApp(this._cellAppButtons[i], false);
				break;
			}
			if (this._cellAppButtons[i].secondaryCellAppDefinition != null && this._cellAppButtons[i].secondaryCellAppDefinition == cellAppDefinition)
			{
				this.ChangeCellApp(this._cellAppButtons[i], true);
				break;
			}
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00019320 File Offset: 0x00017520
	private void ChangeCellApp(UICellAppButton cellAppButton, bool secondary = false)
	{
		if (this._activeCellAppButton != null)
		{
			this._activeCellAppDefinition = null;
			if (this._activeCellApp != null)
			{
				UnityEngine.Object.Destroy(this._activeCellApp.gameObj);
			}
			this._activeCellApp = null;
			this._activeCellAppButton.button.Enable();
		}
		this._activeCellAppButton = null;
		this.HideCellAppError();
		this._activeCellAppButton = cellAppButton;
		this._activeCellAppButton.button.Disable();
		this._activeCellAppDefinition = this._activeCellAppButton.cellAppDefinition;
		this._secondary = false;
		if (secondary && this._activeCellAppButton.secondaryCellAppDefinition != null)
		{
			this._activeCellAppDefinition = this._activeCellAppButton.secondaryCellAppDefinition;
			this._secondary = true;
		}
		this.headerIcons.sprite.SetSprite(this._activeCellAppDefinition.headerSprite);
		this.headerIcons.localX = (float)this._activeCellAppDefinition.headerSpriteOffset;
		this.headerLabel.SetText(this._activeCellAppDefinition.appName.ToUpper());
		this.cellAppBackground.sprite.SetSprite(this._activeCellAppDefinition.bgSpriteName);
		this.cellAppScroller.sprite.SetSprite(this._activeCellAppDefinition.scrollBgSpriteName);
		if (this._activeCellAppDefinition.backToCellApp != null)
		{
			this.backButton.button.Enable();
			this.backButton.spriteAlpha = 1f;
		}
		else
		{
			this.backButton.button.Disable();
			this.backButton.spriteAlpha = 0f;
		}
		this._activeCellApp = (UnityEngine.Object.Instantiate(this._activeCellAppDefinition.cellAppUIPrefab) as GameObject).GetComponent<UICellApp>();
		this._activeCellApp.Init();
		this.cellAppContainer.AddChild(this._activeCellApp);
		this._activeCellApp.SetLocalPosition(0f, 0f);
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00019530 File Offset: 0x00017730
	public void SetCellApp(CellAppDefinition cellAppDefinition)
	{
		if (this._opened || this._locked)
		{
			return;
		}
		if (this._activeCellAppButton != null)
		{
			this._activeCellAppButton.button.Enable();
		}
		this._activeCellAppButton = null;
		for (int i = 0; i < this._cellAppButtons.Count; i++)
		{
			if (this._cellAppButtons[i].cellAppDefinition == cellAppDefinition)
			{
				this._activeCellAppButton = this._cellAppButtons[i];
				this._secondary = false;
				break;
			}
			if (this._cellAppButtons[i].secondaryCellAppDefinition != null && this._cellAppButtons[i].secondaryCellAppDefinition == cellAppDefinition)
			{
				this._activeCellAppButton = this._cellAppButtons[i];
				this._secondary = true;
				break;
			}
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x000042DF File Offset: 0x000024DF
	public void RefreshActiveCellApp()
	{
		if (!this._opened || this._locked || this._activeCellApp == null)
		{
			return;
		}
		this._activeCellApp.Refresh();
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00019628 File Offset: 0x00017828
	public void ShowCellAppError(string errorText, bool showOverlay = true, float yOffset = 0f)
	{
		this.cellAppError.gameObj.SetActive(true);
		if (showOverlay)
		{
			this.cellAppErrorOverlay.SetAlpha(0.5f, 0f);
			this.cellAppErrorBackground.SetAlpha(0.5f, 0f);
		}
		else
		{
			this.cellAppErrorOverlay.SetAlpha(0f, 0f);
			this.cellAppErrorBackground.SetAlpha(0.32f, 0f);
		}
		this.cellAppErrorLabel.SetAlpha(1f);
		this.cellAppErrorLabel.SetText(errorText);
		this.cellAppErrorBackground.localY = this.errorBgOrigY + yOffset;
		this.cellAppErrorLabel.localY = this.errorLblOrigY + yOffset;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00004314 File Offset: 0x00002514
	public void HideCellAppError()
	{
		this.cellAppError.gameObj.SetActive(false);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x000196E8 File Offset: 0x000178E8
	public void Lock(bool disableAppButtons = false)
	{
		if (this._locked)
		{
			return;
		}
		this._locked = true;
		if (this._opened)
		{
			this.cellPhoneCore.interactive = false;
			if (disableAppButtons)
			{
				for (int i = 0; i < this._cellAppButtons.Count; i++)
				{
					if (this._cellAppButtons[i] != this._activeCellAppButton)
					{
						this._cellAppButtons[i].button.Disable();
					}
				}
			}
		}
		GameManager.Stage.uiTop.RefreshCellPhoneButton();
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00019784 File Offset: 0x00017984
	public void Unlock()
	{
		if (!this._locked)
		{
			return;
		}
		this._locked = false;
		if (this._opened)
		{
			this.cellPhoneCore.interactive = true;
			for (int i = 0; i < this._cellAppButtons.Count; i++)
			{
				if (this._cellAppButtons[i] != this._activeCellAppButton)
				{
					this._cellAppButtons[i].button.Enable();
				}
			}
		}
		GameManager.Stage.uiTop.RefreshCellPhoneButton();
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00004327 File Offset: 0x00002527
	public bool IsLocked()
	{
		return this._locked;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000432F File Offset: 0x0000252F
	public void EnableClosePane()
	{
		this._closePane.button.Enable();
		this._closePane.localX = 819f;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00004351 File Offset: 0x00002551
	public void DisableClosePane()
	{
		this._closePane.button.Disable();
		this._closePane.localX = -380f;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00004373 File Offset: 0x00002573
	private void OnClosePanePressed(ButtonObject buttonObject)
	{
		if (!this._opened || this._locked)
		{
			return;
		}
		GameManager.Stage.uiTop.CloseCellPhone();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00019818 File Offset: 0x00017A18
	protected override void Destructor()
	{
		base.Destructor();
		this._closePane.button.ButtonPressedEvent -= this.OnClosePanePressed;
		this._closePane = null;
		this.backButton.button.ButtonPressedEvent -= this.OnCellAppBackButtonClicked;
		this.backButton = null;
		this._activeCellAppButton = null;
		for (int i = 0; i < this._cellAppButtons.Count; i++)
		{
			this._cellAppButtons[i].CellAppButtonClickedEvent -= this.OnCellAppButtonClicked;
		}
		this._cellAppButtons.Clear();
		this._cellAppButtons = null;
	}

	// Token: 0x04000228 RID: 552
	public const int CLOSED_X_POSITION = -563;

	// Token: 0x04000229 RID: 553
	public const int OPEN_X_POSITION = 103;

	// Token: 0x0400022A RID: 554
	private const int CELL_APP_BUTTON_COUNT = 7;

	// Token: 0x0400022B RID: 555
	public CellAppDefinition defaultCellApp;

	// Token: 0x0400022C RID: 556
	public CellAppDefinition initialCellApp;

	// Token: 0x0400022D RID: 557
	public CellAppDefinition messagesCellApp;

	// Token: 0x0400022E RID: 558
	public CellAppDefinition girlProfileCellApp;

	// Token: 0x0400022F RID: 559
	public AudioDefinition openSound;

	// Token: 0x04000230 RID: 560
	public AudioDefinition closeSound;

	// Token: 0x04000231 RID: 561
	public AudioDefinition notificationSound;

	// Token: 0x04000232 RID: 562
	public Dictionary<string, int> cellMemory;

	// Token: 0x04000233 RID: 563
	public DisplayObject cellPhoneCore;

	// Token: 0x04000234 RID: 564
	public DisplayObject cellPhoneHeader;

	// Token: 0x04000235 RID: 565
	public SpriteObject headerIcons;

	// Token: 0x04000236 RID: 566
	public LabelObject headerLabel;

	// Token: 0x04000237 RID: 567
	public SpriteObject backButton;

	// Token: 0x04000238 RID: 568
	public DisplayObject cellAppButtonContainer;

	// Token: 0x04000239 RID: 569
	public DisplayObject cellAppWindow;

	// Token: 0x0400023A RID: 570
	public SpriteObject cellAppBackground;

	// Token: 0x0400023B RID: 571
	public TiledSpriteObject cellAppScroller;

	// Token: 0x0400023C RID: 572
	public DisplayObject cellAppContainer;

	// Token: 0x0400023D RID: 573
	public DisplayObject cellAppError;

	// Token: 0x0400023E RID: 574
	public SpriteObject cellAppErrorOverlay;

	// Token: 0x0400023F RID: 575
	public SpriteObject cellAppErrorBackground;

	// Token: 0x04000240 RID: 576
	public LabelObject cellAppErrorLabel;

	// Token: 0x04000241 RID: 577
	public float errorBgOrigY;

	// Token: 0x04000242 RID: 578
	public float errorLblOrigY;

	// Token: 0x04000243 RID: 579
	private DisplayObject _closePane;

	// Token: 0x04000244 RID: 580
	private List<UICellAppButton> _cellAppButtons;

	// Token: 0x04000245 RID: 581
	private UICellAppButton _activeCellAppButton;

	// Token: 0x04000246 RID: 582
	private UICellApp _activeCellApp;

	// Token: 0x04000247 RID: 583
	private CellAppDefinition _activeCellAppDefinition;

	// Token: 0x04000248 RID: 584
	private bool _secondary;

	// Token: 0x04000249 RID: 585
	private bool _opened;

	// Token: 0x0400024A RID: 586
	private bool _locked;

	// Token: 0x0400024B RID: 587
	private float _scrollerTimestamp;
}
