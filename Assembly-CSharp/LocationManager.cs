using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class LocationManager : MonoBehaviour
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x0600068A RID: 1674 RVA: 0x00006F70 File Offset: 0x00005170
	// (remove) Token: 0x0600068B RID: 1675 RVA: 0x00006F89 File Offset: 0x00005189
	public event LocationManager.LocationDelegate LocationDepartEvent;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x0600068C RID: 1676 RVA: 0x00006FA2 File Offset: 0x000051A2
	// (remove) Token: 0x0600068D RID: 1677 RVA: 0x00006FBB File Offset: 0x000051BB
	public event LocationManager.LocationDelegate LocationArriveEvent;

	// Token: 0x0600068E RID: 1678 RVA: 0x00006FD4 File Offset: 0x000051D4
	private void Awake()
	{
		this._waitingToTravel = false;
		this._clearForTravel = false;
		this._isGirlLeaving = false;
		this._skipNextGreeting = false;
		this._showEndingScene = false;
		this._isSettled = false;
		this._justSettled = false;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0000306D File Offset: 0x0000126D
	private void Start()
	{
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00032E10 File Offset: 0x00031010
	private void Update()
	{
		if (this._paused)
		{
			return;
		}
		if (this._justSettled)
		{
			this._justSettled = false;
			if (!GameManager.System.Player.tutorialComplete)
			{
				GameManager.System.Player.tutorialStep++;
			}
			if (GameManager.System.GameState == GameState.PUZZLE)
			{
				GameManager.System.Puzzle.StartPuzzleGame();
			}
			else
			{
				if (this.currentGirl == GameManager.Stage.uiGirl.fairyGirlDef && this.currentLocation.postBonusRoundLocation && GameManager.System.Clock.DayTime(-1) == GameClockDaytime.MORNING && !GameManager.System.Player.endingSceneShown && this._showEndingScene)
				{
					this._showEndingScene = false;
					GameManager.System.Dialog.PlayDialogScene(GameManager.Stage.uiGirl.endingScene);
					GameManager.System.Player.endingSceneShown = true;
				}
				else if (GameManager.System.Player.GetGirlData(this.currentGirl).metStatus == GirlMetStatus.MET)
				{
					GameManager.Stage.uiWindows.SetWindow(GameManager.Stage.uiWindows.defaultWindow, false, false);
					GameManager.Stage.uiWindows.ShowIncomingWindow();
					if (this._skipNextGreeting)
					{
						this._skipNextGreeting = false;
					}
					else if (GameManager.System.Location.currentLocation.postBonusRoundLocation && GameManager.System.Clock.DayTime(-1) == GameClockDaytime.MORNING && GameManager.System.Player.GetGirlData(this.currentGirl).dayDated)
					{
						GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.greetingDialogTrigger, 4, false, -1);
					}
					else
					{
						GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.greetingDialogTrigger, (int)GameManager.System.Clock.DayTime(-1), false, -1);
					}
				}
				else if (!GameManager.System.Player.tutorialComplete)
				{
					GameManager.System.Dialog.PlayDialogScene(GameManager.Stage.uiGirl.openingScenes[GameManager.System.Player.tutorialStep]);
				}
				else
				{
					GameManager.System.Dialog.PlayDialogScene(this.currentGirl.introScene);
				}
				GameManager.Stage.cellNotifications.ProcessNotificationQueue();
				GameManager.Stage.uiTop.RefreshMessageAlert();
			}
			GameManager.Stage.uiGirl.stats.interactive = true;
			GameManager.Stage.uiPuzzle.puzzleStatus.interactive = true;
			if (GameManager.System.Player.cellphoneUnlocked)
			{
				GameManager.System.Pauseable = true;
			}
		}
		if (this._waitingToTravel)
		{
			this._waitingToTravel = false;
			this.TravelTo(this._destinationLocation, this._destinationGirl);
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00033128 File Offset: 0x00031328
	public void TravelTo(LocationDefinition destination, GirlDefinition girl)
	{
		this._destinationLocation = destination;
		this._destinationGirl = girl;
		if (this._paused)
		{
			this._waitingToTravel = true;
			return;
		}
		GameManager.System.Pauseable = false;
		if (this.currentLocation != null)
		{
			if (this.currentLocation.type == LocationType.NORMAL)
			{
				GameManager.Stage.cellNotifications.ClearAllNotifications();
			}
			if (this.currentLocation.type == LocationType.NORMAL && (GameManager.Stage.uiWindows.IsDefaultWindowActive(true) || GameManager.Stage.uiWindows.GetActiveWindow() != null))
			{
				this._isGirlLeaving = true;
				GameManager.Stage.uiWindows.UIWindowsClearEvent += this.OnUIWindowsClearForTravel;
				GameManager.Stage.uiWindows.HideActiveWindow();
				GameManager.Stage.girl.DialogLineReadEvent += this.OnGirlDialogRead;
				if (this._destinationLocation.type == LocationType.DATE)
				{
					GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.askOnDateDialogTrigger, 0, false, -1);
				}
				else
				{
					GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.valedictionDialogTrigger, (int)GameManager.System.Clock.DayTime(-1), false, -1);
				}
			}
			else
			{
				this.DepartLocation();
			}
		}
		else
		{
			this.OnLocationDeparture();
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00007007 File Offset: 0x00005207
	private void OnUIWindowsClearForTravel()
	{
		GameManager.Stage.uiWindows.UIWindowsClearEvent -= this.OnUIWindowsClearForTravel;
		if (this._clearForTravel)
		{
			this.DepartLocation();
		}
		else
		{
			this._clearForTravel = true;
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00007041 File Offset: 0x00005241
	private void OnGirlDialogRead()
	{
		GameManager.Stage.girl.DialogLineReadEvent -= this.OnGirlDialogRead;
		if (this._clearForTravel)
		{
			this.DepartLocation();
		}
		else
		{
			this._clearForTravel = true;
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0003329C File Offset: 0x0003149C
	private void DepartLocation()
	{
		this._isSettled = false;
		this._justSettled = false;
		this._clearForTravel = false;
		this._isGirlLeaving = false;
		TweenUtils.KillSequence(this._sequence, true);
		GameManager.Stage.uiGirl.stats.interactive = false;
		GameManager.Stage.uiPuzzle.puzzleStatus.interactive = false;
		GirlDefinition girlDefinition = null;
		if (this.currentLocation.type == LocationType.NORMAL && this._destinationLocation.type == LocationType.NORMAL)
		{
			girlDefinition = this.CheckForSecretGirlUnlock();
		}
		if (girlDefinition != null)
		{
			this._sequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallbackWParms(this.OnRevealSecretCharacter), new object[]
			{
				girlDefinition
			}));
		}
		else
		{
			this._sequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnLocationDeparture)));
		}
		if (GameManager.Stage.girl.girlPieceContainers.localX != 600f)
		{
			this._sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
			this._sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
			this._sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 600).Ease(EaseType.EaseInCubic)));
			if (GameManager.System.GameState == GameState.SIM && GameManager.System.Player.tutorialComplete && GameManager.System.Player.GetGirlData(this.currentGirl).metStatus == GirlMetStatus.MET)
			{
				this._sequence.Insert(0.25f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.75f, new TweenParms().Prop("localY", -10).Ease(EaseType.EaseInCubic)));
			}
			else if (GameManager.System.GameState == GameState.PUZZLE)
			{
				this._sequence.Insert(0.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleStatus, 0.75f, new TweenParms().Prop("localY", -10).Ease(EaseType.EaseInCubic)));
			}
		}
		if (girlDefinition != null)
		{
			this._sequence.Insert(this._sequence.fullDuration, HOTween.To(GameManager.Stage.transitionScreen.overlay, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
			this._sequence.Play();
		}
		else
		{
			this._sequence.Insert(this._sequence.fullDuration, HOTween.To(GameManager.Stage.transitionScreen.overlay, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInCubic)));
			this._sequence.Insert(this._sequence.fullDuration - 1f, HOTween.To(GameManager.Stage.background.locationBackgrounds.gameObj.transform, 1f, new TweenParms().Prop("localScale", Vector3.one * 0.95f).Ease(EaseType.EaseInCubic)));
			this._sequence.Play();
			LocationBackground backgroundByDaytime = this.currentLocation.GetBackgroundByDaytime(GameManager.System.Clock.DayTime(-1));
			LocationBackground backgroundByDaytime2 = this._destinationLocation.GetBackgroundByDaytime(GameManager.System.Clock.DayTime(GameManager.System.Clock.TotalMinutesElapsed(0) + 360));
			if (GameManager.System.Hook.skipTransitionScreen)
			{
				GameManager.Stage.background.StopBackgroundMusic(2f);
			}
			else if (this._destinationLocation != this.currentLocation || backgroundByDaytime.musicDefinition.clip != backgroundByDaytime2.musicDefinition.clip)
			{
				GameManager.Stage.background.StopBackgroundMusic(4f);
			}
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00033760 File Offset: 0x00031960
	private void OnLocationDeparture()
	{
		TweenUtils.KillSequence(this._sequence, true);
		if (this._destinationLocation.type == LocationType.DATE)
		{
			GameManager.System.GameState = GameState.PUZZLE;
		}
		else
		{
			GameManager.System.GameState = GameState.SIM;
		}
		if (!GameManager.System.Player.tutorialComplete && GameManager.System.Player.tutorialStep >= 3)
		{
			GameManager.System.Player.tutorialComplete = true;
		}
		int num = GameManager.System.Clock.TotalMinutesElapsed(0);
		int num2 = num;
		bool gameSaved = false;
		if (this.currentLocation != null && this._destinationLocation.type == LocationType.NORMAL)
		{
			if (GameManager.System.Player.tutorialComplete && GameManager.System.Player.GetGirlData(this._destinationGirl).metStatus != GirlMetStatus.MET)
			{
				while (GameManager.System.Clock.DayTime(-1) != this._destinationGirl.introDaytime)
				{
					GameManager.System.Clock.ProgressDaytime();
				}
			}
			else if ((this.currentLocation.type == LocationType.NORMAL || this.currentLocation.bonusRoundLocation || (!GameManager.System.Player.tutorialComplete && GameManager.System.Player.tutorialStep == 2)) && (GameManager.System.Player.tutorialComplete || GameManager.System.Player.tutorialStep != 0))
			{
				GameManager.System.Clock.ProgressDaytime();
			}
			GameManager.System.Player.currentLocation = this._destinationLocation;
			GameManager.System.Player.currentGirl = this._destinationGirl;
			num2 = GameManager.System.Clock.TotalMinutesElapsed(0);
			if (GameManager.System.Clock.DayTime(num) != GameManager.System.Clock.DayTime(num2))
			{
				GameManager.System.Player.RollNewDaytime();
				List<GirlDefinition> all = GameManager.Data.Girls.GetAll();
				for (int i = 0; i < all.Count; i++)
				{
					GirlDefinition girlDefinition = all[i];
					GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDefinition);
					if (girlData.metStatus == GirlMetStatus.MET)
					{
						for (int j = 2; j >= 0; j--)
						{
							GirlPhoto girlPhoto = girlDefinition.photos[j];
							if (!girlData.IsPhotoEarned(j) && girlData.relationshipLevel - 1 > j && this._destinationGirl != girlDefinition && (!girlPhoto.sendAtDaytime || GameManager.System.Clock.DayTime(-1) == girlPhoto.sendDaytime))
							{
								girlData.AddPhotoEarned(j);
								if (!girlPhoto.hasAlts && girlPhoto.messageDef != null)
								{
									GameManager.System.Player.AddMessage(girlPhoto.messageDef);
								}
								break;
							}
						}
					}
				}
			}
			if (GameManager.System.Clock.CalendarDay(num, true, true) != GameManager.System.Clock.CalendarDay(num2, true, true))
			{
				GameManager.System.Player.RollNewDay();
			}
			if (GameManager.System.Player.tutorialComplete)
			{
				if (GameManager.System.Player.GetGirlData(this.currentGirl).relationshipLevel >= 4 && !GameManager.System.Player.HasReceivedMessage(GameManager.Data.Messages.Get(42)))
				{
					GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(42));
				}
				else if (this.currentLocation.type == LocationType.DATE && !GameManager.System.Player.HasReceivedMessage(GameManager.Data.Messages.Get(38)))
				{
					GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(38));
				}
				else if (this.currentLocation.type == LocationType.DATE && !GameManager.System.Player.HasReceivedMessage(GameManager.Data.Messages.Get(39)))
				{
					GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(39));
				}
				else if (GameManager.System.Clock.DayTime(-1) == GameClockDaytime.NIGHT && this._destinationLocation.drinkingLocation && !GameManager.System.Player.HasReceivedMessage(GameManager.Data.Messages.Get(41)))
				{
					GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(41));
				}
				else if (GameManager.System.Player.endingSceneShown && !GameManager.System.Player.HasReceivedMessage(GameManager.Data.Messages.Get(43)))
				{
					GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(43));
				}
			}
			GameManager.System.SaveGame(false);
			gameSaved = true;
		}
		else if (this._destinationLocation.type == LocationType.DATE && this._destinationLocation.bonusRoundLocation)
		{
			while (GameManager.System.Clock.DayTime(-1) != GameClockDaytime.NIGHT)
			{
				GameManager.System.Clock.ProgressDaytime();
			}
			num2 = GameManager.System.Clock.TotalMinutesElapsed(0);
		}
		LocationDefinition fromLocation = this.currentLocation;
		GirlDefinition fromGirl = this.currentGirl;
		this.currentLocation = this._destinationLocation;
		this.currentGirl = this._destinationGirl;
		this._destinationLocation = null;
		this._destinationGirl = null;
		GameManager.Stage.background.UpdateLocation();
		GameManager.Stage.uiTop.UpdateClock();
		GameManager.Stage.girl.ShowGirl(this.currentGirl);
		if (GameManager.System.GameState != GameState.PUZZLE)
		{
			GameManager.Stage.uiGirl.ShowCurrentGirlStats();
		}
		Resources.UnloadUnusedAssets();
		GameManager.Stage.transitionScreen.TransitionCompleteEvent += this.ArriveLocation;
		if (GameManager.System.Hook.skipTransitionScreen)
		{
			GameManager.Stage.background.PlayBackgroundMusic();
			this.ArriveLocation();
		}
		else
		{
			GameManager.Stage.transitionScreen.ShowTravelTransition(num, num2, fromLocation, this.currentLocation, fromGirl, this.currentGirl, gameSaved);
		}
		if (this.LocationDepartEvent != null)
		{
			this.LocationDepartEvent();
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00033E40 File Offset: 0x00032040
	private void ArriveLocation()
	{
		GameManager.Stage.transitionScreen.TransitionCompleteEvent -= this.ArriveLocation;
		TweenUtils.KillSequence(this._sequence, true);
		GameManager.Stage.girl.girlPieceContainers.localX = 600f;
		this._sequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnLocationArrival)));
		this._sequence.Insert(0f, HOTween.To(GameManager.Stage.transitionScreen.overlay, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseOutCubic)));
		this._sequence.Insert(0f, HOTween.To(GameManager.Stage.background.locationBackgrounds.gameObj.transform, 1f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutCubic)));
		this._sequence.Play();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00033F54 File Offset: 0x00032154
	private void OnLocationArrival()
	{
		TweenUtils.KillSequence(this._sequence, true);
		GameManager.Stage.background.locationBackgrounds.SetLocalScale(1f, 0f, EaseType.Linear);
		this._sequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnLocationSettled)));
		this._sequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseOutCubic)));
		if (GameManager.System.GameState == GameState.SIM && GameManager.System.Player.tutorialComplete && GameManager.System.Player.GetGirlData(this.currentGirl).metStatus == GirlMetStatus.MET)
		{
			this._sequence.Insert(0f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.75f, new TweenParms().Prop("localY", 151).Ease(EaseType.EaseOutCubic)));
		}
		else if (GameManager.System.GameState == GameState.PUZZLE)
		{
			this._sequence.Insert(0f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleStatus, 0.75f, new TweenParms().Prop("localY", 151).Ease(EaseType.EaseOutCubic)));
		}
		this._sequence.Play();
		if (this.currentLocation.type == LocationType.DATE)
		{
			GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_girl"] = this.currentGirl.id;
			GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_tab"] = 0;
			GameManager.Stage.cellPhone.SetCellApp(GameManager.Stage.cellPhone.girlProfileCellApp);
		}
		if (this.LocationArriveEvent != null)
		{
			this.LocationArriveEvent();
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0000707B File Offset: 0x0000527B
	private void OnLocationSettled()
	{
		TweenUtils.KillSequence(this._sequence, true);
		this._isSettled = true;
		this._justSettled = true;
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0003416C File Offset: 0x0003236C
	private GirlDefinition CheckForSecretGirlUnlock()
	{
		GirlDefinition girlDefinition = GameManager.Stage.uiGirl.fairyGirlDef;
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		if (girlData.metStatus == GirlMetStatus.LOCKED && this.currentLocation == GameManager.Stage.uiPuzzle.puzzleStatus.postBonusRoundLocation)
		{
			int num = 0;
			for (int i = 0; i < GameManager.System.Player.girls.Count; i++)
			{
				if (GameManager.System.Player.girls[i].metStatus == GirlMetStatus.MET && GameManager.System.Player.girls[i].gotPanties)
				{
					num++;
				}
			}
			if (num > 0)
			{
				return girlDefinition;
			}
		}
		if (this.currentGirl == GameManager.Stage.uiGirl.goddessGirlDef && this.currentLocation.postBonusRoundLocation && GameManager.System.Player.GetGirlData(this.currentGirl).gotPanties && GameManager.System.Clock.DayTime(-1) == GameClockDaytime.MORNING && !GameManager.System.Player.endingSceneShown)
		{
			this._showEndingScene = true;
			return girlDefinition;
		}
		girlDefinition = GameManager.Stage.uiGirl.goddessGirlDef;
		girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		if (girlData.metStatus == GirlMetStatus.LOCKED && this.currentLocation == GameManager.Stage.uiPuzzle.puzzleStatus.postBonusRoundLocation)
		{
			int num2 = 0;
			for (int j = 0; j < GameManager.System.Player.girls.Count; j++)
			{
				if ((!GameManager.System.Player.girls[j].GetGirlDefinition().secretGirl || GameManager.System.Player.girls[j].GetGirlDefinition() == GameManager.Stage.uiGirl.fairyGirlDef) && GameManager.System.Player.girls[j].metStatus == GirlMetStatus.MET && GameManager.System.Player.girls[j].gotPanties)
				{
					num2++;
				}
			}
			if (num2 >= 9)
			{
				return girlDefinition;
			}
		}
		girlDefinition = GameManager.Stage.uiGirl.alienGirlDef;
		girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		if (girlData.metStatus == GirlMetStatus.LOCKED && this.currentLocation == GameManager.Stage.uiGirl.alienGirlLocation && GameManager.System.Clock.DayTime(-1) == GameClockDaytime.NIGHT && GameManager.System.Player.HasItem(GameManager.Stage.uiGirl.weirdThingDef))
		{
			return girlDefinition;
		}
		girlDefinition = GameManager.Stage.uiGirl.catGirlDef;
		girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		if (girlData.metStatus == GirlMetStatus.LOCKED && this.currentLocation.outdoorLocation && GameManager.System.Player.HasTossedItem(GameManager.Stage.uiGirl.bagOfFishDef))
		{
			return girlDefinition;
		}
		return null;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x000344CC File Offset: 0x000326CC
	private void OnRevealSecretCharacter(TweenEvent data)
	{
		GirlDefinition girlDefinition = data.parms[0] as GirlDefinition;
		this.currentLocation = this.currentLocation;
		this.currentGirl = girlDefinition;
		this._destinationLocation = null;
		this._destinationGirl = null;
		GameManager.Stage.girl.ShowGirl(this.currentGirl);
		GameManager.Stage.uiGirl.ShowCurrentGirlStats();
		this.OnLocationArrival();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00007097 File Offset: 0x00005297
	public bool IsLocationSettled()
	{
		return this._isSettled;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0000709F File Offset: 0x0000529F
	public bool IsGirlLeaving()
	{
		return this._isGirlLeaving;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000070A7 File Offset: 0x000052A7
	public bool IsWaitingToTravel()
	{
		return this._waitingToTravel;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x000070AF File Offset: 0x000052AF
	public void SkipNextGreeting()
	{
		this._skipNextGreeting = true;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000070B8 File Offset: 0x000052B8
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		if (this._sequence != null && !this._sequence.isPaused)
		{
			this._sequence.Pause();
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x000070F3 File Offset: 0x000052F3
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		if (this._sequence != null && this._sequence.isPaused)
		{
			this._sequence.Play();
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00034534 File Offset: 0x00032734
	private void OnDestroy()
	{
		GameManager.Stage.uiWindows.UIWindowsClearEvent -= this.OnUIWindowsClearForTravel;
		GameManager.Stage.girl.DialogLineReadEvent -= this.OnGirlDialogRead;
		TweenUtils.KillSequence(this._sequence, false);
		this._sequence = null;
		this._destinationLocation = null;
		this.currentLocation = null;
		this._destinationGirl = null;
		this.currentGirl = null;
	}

	// Token: 0x040007EA RID: 2026
	private bool _paused;

	// Token: 0x040007EB RID: 2027
	public LocationDefinition currentLocation;

	// Token: 0x040007EC RID: 2028
	private LocationDefinition _destinationLocation;

	// Token: 0x040007ED RID: 2029
	public GirlDefinition currentGirl;

	// Token: 0x040007EE RID: 2030
	private GirlDefinition _destinationGirl;

	// Token: 0x040007EF RID: 2031
	private Sequence _sequence;

	// Token: 0x040007F0 RID: 2032
	private bool _waitingToTravel;

	// Token: 0x040007F1 RID: 2033
	private bool _clearForTravel;

	// Token: 0x040007F2 RID: 2034
	private bool _isGirlLeaving;

	// Token: 0x040007F3 RID: 2035
	private bool _skipNextGreeting;

	// Token: 0x040007F4 RID: 2036
	private bool _showEndingScene;

	// Token: 0x040007F5 RID: 2037
	private bool _isSettled;

	// Token: 0x040007F6 RID: 2038
	private bool _justSettled;

	// Token: 0x02000126 RID: 294
	// (Invoke) Token: 0x060006A3 RID: 1699
	public delegate void LocationDelegate();
}
