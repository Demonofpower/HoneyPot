using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;

// Token: 0x0200007B RID: 123
public class UITransitionScreen : DisplayObject
{
	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060003B2 RID: 946 RVA: 0x00004F5E File Offset: 0x0000315E
	// (remove) Token: 0x060003B3 RID: 947 RVA: 0x00004F77 File Offset: 0x00003177
	public event UITransitionScreen.TransitionScreenDelegate TransitionCompleteEvent;

	// Token: 0x060003B4 RID: 948 RVA: 0x00021628 File Offset: 0x0001F828
	protected override void OnStart()
	{
		base.OnStart();
		this.overlay = (base.GetChildByName("TransitionScreenOverlay") as SpriteObject);
		this.contents = base.GetChildByName("TransitionScreenContents");
		this.titleBar = this.contents.GetChildByName("TransitionScreenTitleBar");
		this.titleBarLabel = (this.titleBar.GetChildByName("TransitionScreenTitleBarLabel") as LabelObject);
		this.transitionClock = this.contents.GetChildByName("TransitionScreenClock");
		this.midBar = (this.transitionClock.GetChildByName("TransitionScreenMidBar") as SpriteObject);
		this.dayDisplay = this.transitionClock.GetChildByName("TransitionScreenDay");
		this.dayLabel = (this.dayDisplay.GetChildByName("TransitionScreenDayLabel") as LabelObject);
		this.timeDisplay = this.transitionClock.GetChildByName("TransitionScreenTime");
		this.timeLabel = (this.timeDisplay.GetChildByName("TransitionScreenTimeLabel") as LabelObject);
		this.locationLabel = (this.transitionClock.GetChildByName("TransitionScreenLocationLabel") as LabelObject);
		this.savedNote = (this.contents.GetChildByName("TransitionScreenSavedNote") as SpriteObject);
		this.contents.SetChildAlpha(0f, 0f);
		this.titleBar.SetChildAlpha(0f, 0f);
		this.dayDisplay.SetChildAlpha(0f, 0f);
		this.timeDisplay.SetChildAlpha(0f, 0f);
		this.contents.gameObj.SetActive(false);
		this._travelTransitionComplete = false;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x000217C8 File Offset: 0x0001F9C8
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._travelTransitionComplete)
		{
			this._travelTransitionComplete = false;
			TweenUtils.KillSequence(this._transitionSequence, true);
			this._transitionSequence = null;
			this.contents.gameObj.SetActive(false);
			if (this.TransitionCompleteEvent != null)
			{
				this.TransitionCompleteEvent();
			}
		}
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00021828 File Offset: 0x0001FA28
	public void ShowTravelTransition(int fromTime, int toTime, LocationDefinition fromLocation, LocationDefinition toLocation, GirlDefinition fromGirl, GirlDefinition toGirl, bool gameSaved)
	{
		this.contents.gameObj.SetActive(true);
		TweenUtils.KillSequence(this._transitionSequence, true);
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		if (fromLocation != null)
		{
			text = StringUtils.Titleize(GameManager.System.Clock.Weekday(fromTime, true).ToString());
			int num = GameManager.System.Clock.CalendarDay(fromTime, true, true);
			if (num > 0)
			{
				text = text + " " + StringUtils.FormatIntWithDigitCount(num, 2);
			}
			text2 = StringUtils.Titleize(GameManager.System.Clock.DayTime(fromTime).ToString().ToLower());
			text3 = fromLocation.fullName;
		}
		else
		{
			text = StringUtils.Titleize(GameManager.System.Clock.Weekday(toTime, true).ToString());
			int num2 = GameManager.System.Clock.CalendarDay(toTime, true, true);
			if (num2 > 0)
			{
				text = text + " " + StringUtils.FormatIntWithDigitCount(num2, 2);
			}
			text2 = StringUtils.Titleize(GameManager.System.Clock.DayTime(toTime).ToString().ToLower());
			text3 = toLocation.fullName;
		}
		this.dayLabel.SetText(text);
		this.timeLabel.SetText(text2);
		this.locationLabel.SetText(text3);
		if (toLocation.type == LocationType.DATE)
		{
			if (GameManager.System.Player.alphaModeActive)
			{
				this.titleBarLabel.SetText("Alpha Mode Date #" + (GameManager.System.Player.alphaModeWins + 1).ToString());
			}
			else if (GameManager.System.Player.GetGirlData(toGirl).relationshipLevel < 5)
			{
				this.titleBarLabel.SetText(StringUtils.Titleize(StringUtils.IntToString(GameManager.System.Player.GetGirlData(toGirl).relationshipLevel - 1)) + " Date With " + toGirl.firstName);
			}
			else
			{
				this.titleBarLabel.SetText("Date With " + toGirl.firstName);
			}
		}
		this._transitionSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnTravelTransitionComplete)));
		this._transitionSequence.Insert(0f, HOTween.To(this.midBar, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		if (fromLocation != null)
		{
			this._transitionSequence.Insert(0f, HOTween.To(this.dayDisplay, 1f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			this._transitionSequence.Insert(0f, HOTween.To(this.timeDisplay, 1f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			this._transitionSequence.Insert(0f, HOTween.To(this.locationLabel, 1f, new TweenParms().Prop("labelAlpha", 1).Ease(EaseType.Linear)));
			if (toLocation.type == LocationType.NORMAL || toLocation.bonusRoundLocation)
			{
				if (GameManager.System.Clock.CalendarDay(fromTime, true, true) != GameManager.System.Clock.CalendarDay(toTime, true, true))
				{
					this._transitionSequence.Insert(1.5f, HOTween.To(this.dayDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Prop("localY", 34).Ease(EaseType.EaseInSine)));
					this._transitionSequence.InsertCallback(1.75f, new TweenDelegate.TweenCallbackWParms(this.UpdateDayLabel), new object[]
					{
						toTime
					});
					this._transitionSequence.Insert(2f, HOTween.To(this.dayDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Prop("localY", 50).Ease(EaseType.EaseOutSine)));
				}
				if (GameManager.System.Clock.DayTime(fromTime) != GameManager.System.Clock.DayTime(toTime))
				{
					this._transitionSequence.Insert(1.5f, HOTween.To(this.timeDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Prop("localY", 35).Ease(EaseType.EaseInSine)));
					this._transitionSequence.InsertCallback(1.75f, new TweenDelegate.TweenCallbackWParms(this.UpdateTimeLabel), new object[]
					{
						toTime
					});
					this._transitionSequence.Insert(2f, HOTween.To(this.timeDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Prop("localY", 51).Ease(EaseType.EaseOutSine)));
				}
			}
			else if (!toLocation.bonusRoundLocation && GameManager.System.Player.tutorialComplete)
			{
				this.titleBar.localY = 564f;
				this._transitionSequence.Insert(1.5f, HOTween.To(this.titleBar, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Prop("localY", 580).Ease(EaseType.EaseOutSine)));
			}
			if (fromLocation != toLocation && fromLocation.fullName != toLocation.fullName)
			{
				this._transitionSequence.Insert(2.75f, HOTween.To(this.locationLabel, 0.25f, new TweenParms().Prop("labelAlpha", 0).Prop("localY", -37).Ease(EaseType.EaseInSine)));
				this._transitionSequence.InsertCallback(3f, new TweenDelegate.TweenCallbackWParms(this.UpdateLocationLabel), new object[]
				{
					toLocation
				});
				this._transitionSequence.Insert(3.25f, HOTween.To(this.locationLabel, 0.25f, new TweenParms().Prop("labelAlpha", 1).Prop("localY", -54).Ease(EaseType.EaseOutSine)));
			}
			if (gameSaved)
			{
				this._transitionSequence.Insert(1f, HOTween.To(this.savedNote, 0.5f, new TweenParms().Prop("spriteAlpha", 0.75f).Ease(EaseType.EaseOutSine)));
				this._transitionSequence.Insert(2.5f, HOTween.To(this.savedNote, 0.5f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine)));
			}
		}
		else
		{
			this.dayDisplay.localY = 34f;
			this.timeDisplay.localY = 35f;
			this.locationLabel.localY = -37f;
			this._transitionSequence.Insert(1.5f, HOTween.To(this.dayDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Prop("localY", 50).Ease(EaseType.EaseOutSine)));
			this._transitionSequence.Insert(1.5f, HOTween.To(this.timeDisplay, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Prop("localY", 51).Ease(EaseType.EaseOutSine)));
			this._transitionSequence.Insert(2.25f, HOTween.To(this.locationLabel, 0.25f, new TweenParms().Prop("labelAlpha", 1).Prop("localY", -54).Ease(EaseType.EaseOutSine)));
		}
		float fullDuration = this._transitionSequence.fullDuration;
		this._transitionSequence.InsertCallback(fullDuration + 0.25f, new TweenDelegate.TweenCallback(this.PlayerLocationMusic));
		this._transitionSequence.Insert(fullDuration + 1f, HOTween.To(this.midBar, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._transitionSequence.Insert(fullDuration + 1f, HOTween.To(this.titleBar, 1f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
		this._transitionSequence.Insert(fullDuration + 1f, HOTween.To(this.dayDisplay, 1f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
		this._transitionSequence.Insert(fullDuration + 1f, HOTween.To(this.timeDisplay, 1f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
		this._transitionSequence.Insert(fullDuration + 1f, HOTween.To(this.locationLabel, 1f, new TweenParms().Prop("labelAlpha", 0).Ease(EaseType.Linear)));
		this._transitionSequence.Play();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00004F90 File Offset: 0x00003190
	private void OnTravelTransitionComplete()
	{
		this._travelTransitionComplete = true;
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000221EC File Offset: 0x000203EC
	private void UpdateDayLabel(TweenEvent e)
	{
		int minutes = (int)e.parms[0];
		string text = StringUtils.Titleize(GameManager.System.Clock.Weekday(minutes, true).ToString());
		int num = GameManager.System.Clock.CalendarDay(minutes, true, true);
		if (num > 0)
		{
			text = text + " " + StringUtils.FormatIntWithDigitCount(num, 2);
		}
		this.dayLabel.SetText(text);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00022264 File Offset: 0x00020464
	private void UpdateTimeLabel(TweenEvent e)
	{
		int minutes = (int)e.parms[0];
		this.timeLabel.SetText(StringUtils.Titleize(GameManager.System.Clock.DayTime(minutes).ToString().ToLower()));
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000222B0 File Offset: 0x000204B0
	private void UpdateLocationLabel(TweenEvent e)
	{
		LocationDefinition locationDefinition = (LocationDefinition)e.parms[0];
		this.locationLabel.SetText(locationDefinition.fullName);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00004F99 File Offset: 0x00003199
	private void PlayerLocationMusic()
	{
		GameManager.Stage.background.PlayBackgroundMusic();
	}

	// Token: 0x0400038B RID: 907
	private const int DAY_LABEL_Y_SHOWN = 50;

	// Token: 0x0400038C RID: 908
	private const int DAY_LABEL_Y_HIDDEN = 34;

	// Token: 0x0400038D RID: 909
	private const int TIME_LABEL_Y_SHOWN = 51;

	// Token: 0x0400038E RID: 910
	private const int TIME_LABEL_Y_HIDDEN = 35;

	// Token: 0x0400038F RID: 911
	private const int LOCATION_LABEL_Y_SHOWN = -54;

	// Token: 0x04000390 RID: 912
	private const int LOCATION_LABEL_Y_HIDDEN = -37;

	// Token: 0x04000391 RID: 913
	private const int TITLEBAR_Y_SHOWN = 580;

	// Token: 0x04000392 RID: 914
	private const int TITLEBAR_Y_HIDDEN = 564;

	// Token: 0x04000393 RID: 915
	public SpriteObject overlay;

	// Token: 0x04000394 RID: 916
	public DisplayObject contents;

	// Token: 0x04000395 RID: 917
	public DisplayObject titleBar;

	// Token: 0x04000396 RID: 918
	public LabelObject titleBarLabel;

	// Token: 0x04000397 RID: 919
	public DisplayObject transitionClock;

	// Token: 0x04000398 RID: 920
	public SpriteObject midBar;

	// Token: 0x04000399 RID: 921
	public DisplayObject dayDisplay;

	// Token: 0x0400039A RID: 922
	public LabelObject dayLabel;

	// Token: 0x0400039B RID: 923
	public DisplayObject timeDisplay;

	// Token: 0x0400039C RID: 924
	public LabelObject timeLabel;

	// Token: 0x0400039D RID: 925
	public LabelObject locationLabel;

	// Token: 0x0400039E RID: 926
	public SpriteObject savedNote;

	// Token: 0x0400039F RID: 927
	private bool _travelTransitionComplete;

	// Token: 0x040003A0 RID: 928
	private Sequence _transitionSequence;

	// Token: 0x0200007C RID: 124
	// (Invoke) Token: 0x060003BD RID: 957
	public delegate void TransitionScreenDelegate();
}
