using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class UITop : DisplayObject
{
	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600039C RID: 924 RVA: 0x00004EFA File Offset: 0x000030FA
	// (remove) Token: 0x0600039D RID: 925 RVA: 0x00004F13 File Offset: 0x00003113
	public event UITop.UITopDelegate CellPhoneOpenedEvent;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600039E RID: 926 RVA: 0x00004F2C File Offset: 0x0000312C
	// (remove) Token: 0x0600039F RID: 927 RVA: 0x00004F45 File Offset: 0x00003145
	public event UITop.UITopDelegate CellPhoneClosedEvent;

	// Token: 0x060003A0 RID: 928 RVA: 0x00020AE0 File Offset: 0x0001ECE0
	protected override void OnStart()
	{
		base.OnStart();
		this.pauseOverlay = (base.GetChildByName("PauseOverlay") as SpriteObject);
		this.labelDay = (base.GetChildByName("DayLabel") as LabelObject);
		this.labelDaytime = (base.GetChildByName("DaytimeLabel") as LabelObject);
		this.labelMoney = (base.GetChildByName("MoneyLabel") as LabelObject);
		this.labelHunie = (base.GetChildByName("HunieLabel") as LabelObject);
		this.buttonHuniebee = (base.GetChildByName("HuniebeeButton") as SpriteObject);
		this.buttonHuniebeeOverlay = (base.GetChildByName("HuniebeeButtonOverlay") as SpriteObject);
		this.messageAlertIcon = (base.GetChildByName("CellMessageAlertIcon") as SpriteObject);
		this.buttonHuniebee.button.ButtonPressedEvent += this.OnHuniebeeButtonPress;
		this._messageButton = (base.GetChildByName("CellMessageAlertButton") as SpriteObject);
		this._messageButton.button.ButtonPressedEvent += this.OnMessageButtonPress;
		this._messageButton.button.Disable();
		this.buttonHuniebee.button.Disable();
		this.buttonHuniebeeOverlay.SetAlpha(0.64f, 0f);
		this.messageAlertIcon.SetAlpha(0f, 0f);
		this._cellButtonDisabled = true;
		this._messageAlertActive = false;
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00020C4C File Offset: 0x0001EE4C
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._messageButton.button.IsEnabled() && (this._cellButtonDisabled || !this._messageAlertActive))
		{
			this._messageButton.button.Disable();
		}
		else if (!this._messageButton.button.IsEnabled() && !this._cellButtonDisabled && this._messageAlertActive)
		{
			this._messageButton.button.Enable();
		}
		if (!this.buttonHuniebee.interactive)
		{
			return;
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.OnHuniebeeButtonPress(this.buttonHuniebee.button);
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00020D08 File Offset: 0x0001EF08
	public void UpdateClock()
	{
		string text = StringUtils.Titleize(GameManager.System.Clock.Weekday(-1, true).ToString().Substring(0, 3));
		int num = GameManager.System.Clock.CalendarDay(-1, true, true);
		if (num > 0)
		{
			text = text + " " + StringUtils.FormatIntWithDigitCount(num, 2);
		}
		string text2 = StringUtils.Titleize(GameManager.System.Clock.DayTime(-1).ToString().ToLower());
		this.labelDay.SetText(text);
		this.labelDaytime.SetText(text2);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00020DA8 File Offset: 0x0001EFA8
	private void OnHuniebeeButtonPress(ButtonObject buttonObject)
	{
		if (!this.buttonHuniebee.interactive)
		{
			return;
		}
		if (GameManager.System.Pauseable && !GameManager.Stage.cellPhone.IsLocked() && this.buttonHuniebee.button.IsEnabled())
		{
			if (GameManager.System.IsPaused())
			{
				this.CloseCellPhone();
			}
			else if (!GameManager.System.Location.IsWaitingToTravel())
			{
				this.OpenCellPhone();
			}
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00020E38 File Offset: 0x0001F038
	public void RefreshCellPhoneButton()
	{
		if (!GameManager.System.Player.cellphoneUnlocked)
		{
			return;
		}
		if (!GameManager.System.Pauseable || GameManager.Stage.cellPhone.IsLocked() || !this.buttonHuniebee.interactive)
		{
			if (!this._cellButtonDisabled)
			{
				this._cellButtonDisabled = true;
				this.buttonHuniebee.button.Disable();
				TweenUtils.KillTweener(this._cellButtonTweener, false);
				this._cellButtonTweener = HOTween.To(this.buttonHuniebeeOverlay, 0.2f, new TweenParms().Prop("spriteAlpha", 0.64f).Ease(EaseType.Linear));
			}
		}
		else if (this._cellButtonDisabled)
		{
			this._cellButtonDisabled = false;
			this.buttonHuniebee.button.Enable();
			TweenUtils.KillTweener(this._cellButtonTweener, false);
			this._cellButtonTweener = HOTween.To(this.buttonHuniebeeOverlay, 0.2f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear));
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00020F54 File Offset: 0x0001F154
	public void OpenCellPhone()
	{
		GameManager.System.Pause();
		base.ShiftSelf(5);
		GameManager.Stage.cellPhone.Open();
		TweenUtils.KillSequence(this._cellPhoneSequence, true);
		this.buttonHuniebee.interactive = false;
		GameManager.Stage.cellPhone.interactive = false;
		this.buttonHuniebee.button.ChangeOrigSpriteOfStateTransition(ButtonState.DOWN, 0, "ui_topbar_huniebee_back_button_up");
		this.buttonHuniebee.button.ChangeStateSpriteOfStateTransition(ButtonState.DOWN, 0, "ui_topbar_huniebee_back_button_down");
		this.buttonHuniebee.button.ChangeOrigSpriteOfStateTransition(ButtonState.OVER, 0, "ui_topbar_huniebee_back_button_up");
		this.buttonHuniebee.button.ChangeStateSpriteOfStateTransition(ButtonState.OVER, 0, "ui_topbar_huniebee_back_button_over");
		this.buttonHuniebee.sprite.SetSprite("ui_topbar_huniebee_back_button_up");
		this._cellPhoneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.CellPhoneOpened)));
		this._cellPhoneSequence.Insert(0f, HOTween.To(GameManager.Stage.cellPhone, 0.5f, new TweenParms().Prop("localX", 103).Ease(EaseType.EaseOutCubic)));
		this._cellPhoneSequence.Insert(0f, HOTween.To(this.pauseOverlay, 0.25f, new TweenParms().Prop("spriteAlpha", 0.5f).Ease(EaseType.Linear)));
		this._cellPhoneSequence.Play();
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.cellPhone.openSound, false, 1f, true);
		GameManager.Stage.background.AdjustBackgroundMusicVolume(0.25f, 0.5f);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00021108 File Offset: 0x0001F308
	private void CellPhoneOpened()
	{
		this.buttonHuniebee.interactive = true;
		GameManager.Stage.cellPhone.interactive = true;
		GameManager.Stage.cellPhone.EnableClosePane();
		if (this.CellPhoneOpenedEvent != null)
		{
			this.CellPhoneOpenedEvent();
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00021158 File Offset: 0x0001F358
	public void CloseCellPhone()
	{
		TweenUtils.KillSequence(this._cellPhoneSequence, true);
		this.buttonHuniebee.interactive = false;
		GameManager.Stage.cellPhone.interactive = false;
		GameManager.Stage.cellPhone.DisableClosePane();
		this.buttonHuniebee.button.ChangeOrigSpriteOfStateTransition(ButtonState.DOWN, 0, "ui_topbar_huniebee_button_up");
		this.buttonHuniebee.button.ChangeStateSpriteOfStateTransition(ButtonState.DOWN, 0, "ui_topbar_huniebee_button_down");
		this.buttonHuniebee.button.ChangeOrigSpriteOfStateTransition(ButtonState.OVER, 0, "ui_topbar_huniebee_button_up");
		this.buttonHuniebee.button.ChangeStateSpriteOfStateTransition(ButtonState.OVER, 0, "ui_topbar_huniebee_button_over");
		this.buttonHuniebee.sprite.SetSprite("ui_topbar_huniebee_button_up");
		this._cellPhoneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.CellPhoneClosed)));
		this._cellPhoneSequence.Insert(0f, HOTween.To(GameManager.Stage.cellPhone, 0.5f, new TweenParms().Prop("localX", -563).Ease(EaseType.EaseInCubic)));
		this._cellPhoneSequence.Insert(0.25f, HOTween.To(this.pauseOverlay, 0.25f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._cellPhoneSequence.Play();
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.cellPhone.closeSound, false, 1f, true);
		GameManager.Stage.background.AdjustBackgroundMusicVolume(1f, 0.5f);
		if (GameManager.Stage.cellPhone.IsLocked())
		{
			GameManager.Stage.cellPhone.Unlock();
		}
		GameManager.Stage.uiWindows.RefreshActiveWindow();
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0002132C File Offset: 0x0001F52C
	private void CellPhoneClosed()
	{
		this.buttonHuniebee.interactive = true;
		GameManager.Stage.cellPhone.interactive = false;
		GameManager.Stage.cellPhone.Close();
		base.ShiftSelf(-5);
		GameManager.System.Unpause();
		if (this.CellPhoneClosedEvent != null)
		{
			this.CellPhoneClosedEvent();
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0002138C File Offset: 0x0001F58C
	public void RefreshMessageAlert()
	{
		if (!this._messageAlertActive && GameManager.System.Player.messages.Count > 0 && !GameManager.System.Player.messages[0].viewed)
		{
			this._messageAlertActive = true;
			TweenUtils.KillTweener(this._messageAlertIconTweener, false);
			this._messageAlertIconTweener = HOTween.To(this.messageAlertIcon, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine).OnComplete(new TweenDelegate.TweenCallback(this.PulseMessageAlertIcon)));
		}
		else if (this._messageAlertActive && (GameManager.System.Player.messages.Count == 0 || GameManager.System.Player.messages[0].viewed))
		{
			this._messageAlertActive = false;
			TweenUtils.KillTweener(this._messageAlertIconTweener, false);
			this._messageAlertIconTweener = HOTween.To(this.messageAlertIcon, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseOutSine));
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x000214C0 File Offset: 0x0001F6C0
	private void PulseMessageAlertIcon()
	{
		if (!this._messageAlertActive)
		{
			return;
		}
		TweenUtils.KillTweener(this._messageAlertIconTweener, false);
		this._messageAlertIconTweener = HOTween.To(this.messageAlertIcon, 1f, new TweenParms().Prop("spriteAlpha", 0.25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo));
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00021524 File Offset: 0x0001F724
	private void OnMessageButtonPress(ButtonObject buttonObject)
	{
		if (!this._messageAlertActive)
		{
			return;
		}
		if (GameManager.Stage.cellPhone.IsOpen())
		{
			GameManager.Stage.cellPhone.ChangeCellApp(GameManager.Stage.cellPhone.messagesCellApp);
		}
		else
		{
			GameManager.Stage.cellPhone.SetCellApp(GameManager.Stage.cellPhone.messagesCellApp);
			GameManager.Stage.uiTop.OpenCellPhone();
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x000215A4 File Offset: 0x0001F7A4
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._cellPhoneSequence, false);
		this._cellPhoneSequence = null;
		TweenUtils.KillTweener(this._cellButtonTweener, false);
		this._cellButtonTweener = null;
		TweenUtils.KillTweener(this._messageAlertIconTweener, false);
		this._messageAlertIconTweener = null;
		this._messageButton.button.ButtonPressedEvent -= this.OnMessageButtonPress;
		this.buttonHuniebee.button.ButtonPressedEvent -= this.OnHuniebeeButtonPress;
	}

	// Token: 0x04000374 RID: 884
	public const string CELL_BUTTON_CLOSED_UP = "ui_topbar_huniebee_button_up";

	// Token: 0x04000375 RID: 885
	public const string CELL_BUTTON_CLOSED_OVER = "ui_topbar_huniebee_button_over";

	// Token: 0x04000376 RID: 886
	public const string CELL_BUTTON_CLOSED_DOWN = "ui_topbar_huniebee_button_down";

	// Token: 0x04000377 RID: 887
	public const string CELL_BUTTON_OPEN_UP = "ui_topbar_huniebee_back_button_up";

	// Token: 0x04000378 RID: 888
	public const string CELL_BUTTON_OPEN_OVER = "ui_topbar_huniebee_back_button_over";

	// Token: 0x04000379 RID: 889
	public const string CELL_BUTTON_OPEN_DOWN = "ui_topbar_huniebee_back_button_down";

	// Token: 0x0400037A RID: 890
	public tk2dSpriteCollectionData uiSpriteCollection;

	// Token: 0x0400037B RID: 891
	public SpriteObject pauseOverlay;

	// Token: 0x0400037C RID: 892
	public LabelObject labelDay;

	// Token: 0x0400037D RID: 893
	public LabelObject labelDaytime;

	// Token: 0x0400037E RID: 894
	public LabelObject labelMoney;

	// Token: 0x0400037F RID: 895
	public LabelObject labelHunie;

	// Token: 0x04000380 RID: 896
	public SpriteObject buttonHuniebee;

	// Token: 0x04000381 RID: 897
	public SpriteObject buttonHuniebeeOverlay;

	// Token: 0x04000382 RID: 898
	public SpriteObject messageAlertIcon;

	// Token: 0x04000383 RID: 899
	private bool _cellButtonDisabled;

	// Token: 0x04000384 RID: 900
	private bool _messageAlertActive;

	// Token: 0x04000385 RID: 901
	private SpriteObject _messageButton;

	// Token: 0x04000386 RID: 902
	private Sequence _cellPhoneSequence;

	// Token: 0x04000387 RID: 903
	private Tweener _cellButtonTweener;

	// Token: 0x04000388 RID: 904
	private Tweener _messageAlertIconTweener;

	// Token: 0x0200007A RID: 122
	// (Invoke) Token: 0x060003AE RID: 942
	public delegate void UITopDelegate();
}
