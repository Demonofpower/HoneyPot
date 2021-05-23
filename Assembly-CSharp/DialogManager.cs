using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class DialogManager : MonoBehaviour
{
	// Token: 0x06000627 RID: 1575 RVA: 0x0002F2A4 File Offset: 0x0002D4A4
	private void Update()
	{
		if (this._paused)
		{
			return;
		}
		if (this._proceedToNextStep && (GameManager.System.GameState == GameState.SIM || (GameManager.System.GameState == GameState.PUZZLE && GameManager.System.Puzzle.Game.activeEnergyTrailCount == 0)))
		{
			this._proceedToNextStep = false;
			this.DialogSceneStep();
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0002F310 File Offset: 0x0002D510
	public void PlayDialogScene(DialogSceneDefinition dialogScene)
	{
		if (dialogScene == null || dialogScene.steps.Count <= 0 || this._activeDialogScene != null)
		{
			return;
		}
		this._activeDialogScene = dialogScene;
		this._activeDialogSceneSteps.Add(new DialogSceneStepsProgress(dialogScene.steps));
		this._mainGirlShowing = true;
		this._altGirlShowing = false;
		this._nextLocationStep = null;
		GameManager.Stage.uiWindows.HideActiveWindow();
		GameManager.Stage.girl.ClearDialog();
		this.DialogSceneStep();
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
	private void DialogSceneStep()
	{
		TweenUtils.KillSequence(this._dialogSceneSequence, true);
		DialogSceneStepsProgress activeDialogSceneSteps = this.GetActiveDialogSceneSteps();
		activeDialogSceneSteps.stepIndex++;
		while (activeDialogSceneSteps.stepIndex >= activeDialogSceneSteps.steps.Count)
		{
			this._activeDialogSceneSteps.RemoveAt(this._activeDialogSceneSteps.Count - 1);
			if (this._activeDialogSceneSteps.Count <= 0)
			{
				break;
			}
			activeDialogSceneSteps = this.GetActiveDialogSceneSteps();
			activeDialogSceneSteps.stepIndex++;
		}
		if (this._activeDialogSceneSteps.Count <= 0)
		{
			this._activeDialogSceneSteps.Clear();
			this._activeDialogScene = null;
			if (this._nextLocationStep != null)
			{
				if (this._nextLocationStep.locationDefinition.type == LocationType.NORMAL)
				{
					GameManager.System.Location.TravelTo(this._nextLocationStep.locationDefinition, this._nextLocationStep.girlDefinition);
				}
				else
				{
					GameManager.System.Puzzle.TravelToPuzzleLocation(this._nextLocationStep.locationDefinition, this._nextLocationStep.girlDefinition);
				}
				this._nextLocationStep = null;
			}
			else
			{
				this._dialogSceneSequence = new Sequence();
				this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
				this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
				this._dialogSceneSequence.Play();
				if (GameManager.System.GameState == GameState.SIM)
				{
					bool showWindow = GameManager.Stage.uiWindows.GetActiveWindow() == null;
					GameManager.Stage.uiWindows.SetWindow(GameManager.Stage.uiWindows.defaultWindow, showWindow, false);
				}
				else
				{
					GameManager.System.Puzzle.Game.TutorialFinsihed();
				}
			}
			return;
		}
		DialogSceneStep activeDialogSceneStep = this.GetActiveDialogSceneStep();
		switch (activeDialogSceneStep.type)
		{
		case DialogSceneStepType.DIALOG_LINE:
			if (activeDialogSceneStep.sceneLine.altGirl && this._altGirlShowing)
			{
				GameManager.Stage.altGirl.DialogLineReadEvent += this.OnGirlDialogLineRead;
				GameManager.Stage.altGirl.ReadDialogLine(activeDialogSceneStep.sceneLine.dialogLine, false, false, false, false);
			}
			else
			{
				GameManager.Stage.girl.DialogLineReadEvent += this.OnGirlDialogLineRead;
				GameManager.Stage.girl.ReadDialogLine(activeDialogSceneStep.sceneLine.dialogLine, false, false, false, false);
			}
			break;
		case DialogSceneStepType.RESPONSE_OPTIONS:
		{
			bool showWindow2 = GameManager.Stage.uiWindows.GetActiveWindow() == null;
			TalkWindow talkWindow = GameManager.Stage.uiWindows.SetWindow(GameManager.Stage.uiWindows.talkWindow, showWindow2, false) as TalkWindow;
			talkWindow.ResponseSelectedEvent += this.OnResponseOptionSelected;
			break;
		}
		case DialogSceneStepType.BRANCH_DIALOG:
			for (int i = 0; i < activeDialogSceneStep.conditionalBranchs.Count; i++)
			{
				if (this.IsBranchConditionMet(activeDialogSceneStep.conditionalBranchs[i]) && activeDialogSceneStep.conditionalBranchs[i].steps.Count > 0)
				{
					if (activeDialogSceneStep.hasBestBranch && i == 0)
					{
						this.BestOptionBranchSelected(125);
					}
					this._activeDialogSceneSteps.Add(new DialogSceneStepsProgress(activeDialogSceneStep.conditionalBranchs[i].steps));
					break;
				}
			}
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.SHOW_ALT_GIRL:
			if (this._altGirlShowing)
			{
				if (activeDialogSceneStep.altGirl == GameManager.Stage.altGirl.definition)
				{
					this.DialogSceneStep();
				}
				else
				{
					this.HideAltGirl(false, activeDialogSceneStep.showGirlStyles, activeDialogSceneStep.hideOppositeSpeechBubble);
				}
			}
			else
			{
				this.ShowAltGirl(activeDialogSceneStep.altGirl, true, activeDialogSceneStep.showGirlStyles, activeDialogSceneStep.hideOppositeSpeechBubble);
			}
			break;
		case DialogSceneStepType.HIDE_ALT_GIRL:
			if (this._altGirlShowing)
			{
				this.HideAltGirl(true, null, false);
			}
			else
			{
				this.DialogSceneStep();
			}
			break;
		case DialogSceneStepType.SHOW_GIRL:
			if (this._mainGirlShowing)
			{
				this.DialogSceneStep();
			}
			else
			{
				this.ShowMainGirl(activeDialogSceneStep.showGirlStyles, activeDialogSceneStep.hideOppositeSpeechBubble);
			}
			break;
		case DialogSceneStepType.HIDE_GIRL:
			if (this._mainGirlShowing)
			{
				this.HideMainGirl();
			}
			else
			{
				this.DialogSceneStep();
			}
			break;
		case DialogSceneStepType.INSERT_SCENE:
			if (activeDialogSceneStep.insertScene.steps.Count > 0)
			{
				this._activeDialogSceneSteps.Add(new DialogSceneStepsProgress(activeDialogSceneStep.insertScene.steps));
			}
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.WAIT:
			if (activeDialogSceneStep.waitTime > 0)
			{
				this._waitTimer = GameManager.System.Timers.New((float)activeDialogSceneStep.waitTime, new Action(this.OnWaitTimerComplete));
			}
			else
			{
				this.DialogSceneStep();
			}
			break;
		case DialogSceneStepType.SET_NEXT_LOCATION:
			this._nextLocationStep = activeDialogSceneStep;
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.SET_MET_STATUS:
			if (activeDialogSceneStep.metStatus <= GameManager.System.Player.GetGirlData(activeDialogSceneStep.girlDefinition).metStatus)
			{
				this.DialogSceneStep();
			}
			else
			{
				GameManager.System.Player.GetGirlData(activeDialogSceneStep.girlDefinition).metStatus = activeDialogSceneStep.metStatus;
				if (activeDialogSceneStep.metStatus == GirlMetStatus.MET)
				{
					GameManager.Stage.uiGirl.ShowCurrentGirlStats();
					GameManager.Stage.cellNotifications.Notify(CellNotificationType.PROFILE, activeDialogSceneStep.girlDefinition.firstName + " has been added to the HunieBee!");
					this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnDialogSceneSequenceComplete)));
					this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.5f, new TweenParms().Prop("localY", 151).Ease(EaseType.EaseOutSine)));
					this._dialogSceneSequence.Insert(0.5f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.5f, new TweenParms().Prop("localY", 151).Ease(EaseType.Linear)));
					this._dialogSceneSequence.Play();
					if (activeDialogSceneStep.girlDefinition == GameManager.Stage.uiGirl.fairyGirlDef)
					{
						SteamUtils.UnlockAchievement("date_the_fairy", true);
					}
					else if (activeDialogSceneStep.girlDefinition == GameManager.Stage.uiGirl.catGirlDef)
					{
						SteamUtils.UnlockAchievement("beastiality", true);
					}
					else if (activeDialogSceneStep.girlDefinition == GameManager.Stage.uiGirl.alienGirlDef)
					{
						SteamUtils.UnlockAchievement("ayy_lmao", true);
					}
					else if (activeDialogSceneStep.girlDefinition == GameManager.Stage.uiGirl.goddessGirlDef)
					{
						SteamUtils.UnlockAchievement("heavenly_body", true);
					}
				}
				else
				{
					this.DialogSceneStep();
				}
			}
			break;
		case DialogSceneStepType.DIALOG_TRIGGER:
		{
			DialogLine dialogTriggerLine = GameManager.System.Girl.GetDialogTriggerLine(activeDialogSceneStep.dialogTrigger, Mathf.Clamp(activeDialogSceneStep.dialogTriggerIndex - 1, 0, activeDialogSceneStep.dialogTrigger.lineSets.Count - 1), -1);
			GameManager.Stage.girl.DialogLineReadEvent += this.OnGirlDialogLineRead;
			GameManager.Stage.girl.ReadDialogLine(dialogTriggerLine, false, false, false, false);
			break;
		}
		case DialogSceneStepType.KNOW_GIRL_DETAIL:
			GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).KnowDetail(activeDialogSceneStep.girlDetailType);
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.STEP_BACK:
			for (int j = 0; j < activeDialogSceneStep.stepBackSteps; j++)
			{
				DialogSceneStepsProgress activeDialogSceneSteps2 = this.GetActiveDialogSceneSteps();
				while (activeDialogSceneSteps2.stepIndex <= 0)
				{
					this._activeDialogSceneSteps.RemoveAt(this._activeDialogSceneSteps.Count - 1);
					activeDialogSceneSteps2 = this.GetActiveDialogSceneSteps();
				}
				activeDialogSceneSteps2.stepIndex--;
			}
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.ADD_ITEM:
			if (activeDialogSceneStep.toEquipment && activeDialogSceneStep.itemDefinition.type == ItemType.DATE_GIFT)
			{
				GameManager.System.Player.AddItem(activeDialogSceneStep.itemDefinition, GameManager.System.Player.dateGifts, false, false);
			}
			else if (activeDialogSceneStep.toEquipment && (activeDialogSceneStep.itemDefinition.type == ItemType.FOOD || activeDialogSceneStep.itemDefinition.type == ItemType.DRINK || activeDialogSceneStep.itemDefinition.type == ItemType.GIFT || activeDialogSceneStep.itemDefinition.type == ItemType.UNIQUE_GIFT || activeDialogSceneStep.itemDefinition.type == ItemType.PANTIES || activeDialogSceneStep.itemDefinition.type == ItemType.MISC))
			{
				GameManager.System.Player.AddItem(activeDialogSceneStep.itemDefinition, GameManager.System.Player.gifts, false, false);
			}
			else
			{
				GameManager.System.Player.AddItem(activeDialogSceneStep.itemDefinition, GameManager.System.Player.inventory, activeDialogSceneStep.wrapped, false);
			}
			if (GameManager.System.GameState == GameState.SIM)
			{
				GameManager.Stage.uiGirl.RefreshItemSlots();
			}
			else if (GameManager.System.GameState == GameState.PUZZLE && GameManager.System.Puzzle.Game != null)
			{
				GameManager.System.Puzzle.Game.RefreshDateGiftSlots(true);
			}
			if (GameManager.System.GameState == GameState.SIM)
			{
				GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, string.Concat(new string[]
				{
					"Received \"",
					activeDialogSceneStep.itemDefinition.name,
					"\" from ",
					GameManager.System.Location.currentGirl.firstName,
					"!"
				}));
			}
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.REMOVE_ITEM:
			GameManager.System.Player.RemoveItem(activeDialogSceneStep.itemDefinition);
			if (GameManager.System.GameState == GameState.SIM)
			{
				GameManager.Stage.uiGirl.RefreshItemSlots();
			}
			else if (GameManager.System.GameState == GameState.PUZZLE && GameManager.System.Puzzle.Game != null)
			{
				GameManager.System.Puzzle.Game.RefreshDateGiftSlots(true);
			}
			GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, string.Concat(new string[]
			{
				"You gave the \"",
				activeDialogSceneStep.itemDefinition.name,
				"\" to ",
				GameManager.System.Location.currentGirl.firstName,
				"!"
			}));
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.WAIT_FOR_CELLPHONE_CLOSE:
			GameManager.Stage.uiTop.CellPhoneClosedEvent += this.OnCellPhoneClosed;
			break;
		case DialogSceneStepType.WAIT_FOR_TOKEN_MATCH:
			if (GameManager.System.Puzzle.Game == null)
			{
				this.DialogSceneStep();
			}
			else
			{
				GameManager.System.Puzzle.Game.PuzzleGameReadyEvent += this.OnPuzzleGameReady;
				GameManager.System.Puzzle.Game.TutorialUnlock(activeDialogSceneStep.tokenDefinition, activeDialogSceneStep.gridKey, activeDialogSceneStep.tokenCount);
			}
			break;
		case DialogSceneStepType.WAIT_FOR_DATE_GIFT:
			if (GameManager.System.Puzzle.Game == null)
			{
				this.DialogSceneStep();
			}
			else
			{
				GameManager.System.Puzzle.Game.PuzzleGameReadyEvent += this.OnPuzzleGameReady;
				GameManager.System.Puzzle.Game.TutorialUnlock(null, null, -1);
			}
			break;
		case DialogSceneStepType.UNLOCK_CELLPHONE:
			GameManager.System.Player.cellphoneUnlocked = true;
			GameManager.Stage.cellPhone.SetCellApp(GameManager.Stage.cellPhone.initialCellApp);
			GameManager.Stage.cellNotifications.Notify(CellNotificationType.MESSAGE, "You got the HunieBee from Kyu!");
			this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnDialogSceneSequenceComplete)));
			this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.uiTop.buttonHuniebee, 0.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
			this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.uiTop.buttonHuniebeeOverlay, 0.5f, new TweenParms().Prop("spriteAlpha", 0.64f).Ease(EaseType.Linear)));
			this._dialogSceneSequence.Play();
			break;
		case DialogSceneStepType.MAKE_GAME_PAUSEABLE:
			GameManager.System.Pauseable = true;
			this.DialogSceneStep();
			break;
		case DialogSceneStepType.PARTICLE_EMITTER:
		{
			ParticleEmitter2D component = new GameObject("DialogSceneParticleEmitter", new Type[]
			{
				typeof(ParticleEmitter2D)
			}).GetComponent<ParticleEmitter2D>();
			GameManager.Stage.effects.AddParticleEffect(component, GameManager.Stage);
			component.Init(activeDialogSceneStep.particleEmitterDefinition, activeDialogSceneStep.spriteGroupDefinition, false);
			component.SetGlobalPosition(activeDialogSceneStep.xPos, activeDialogSceneStep.yPos);
			if (activeDialogSceneStep.soundEffect != null && activeDialogSceneStep.soundEffect.clip != null)
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, activeDialogSceneStep.soundEffect, false, 1f, true);
			}
			this.DialogSceneStep();
			break;
		}
		case DialogSceneStepType.SEND_MESSAGE:
			GameManager.System.Player.AddMessage(activeDialogSceneStep.messageDef);
			this.DialogSceneStep();
			break;
		}
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00030204 File Offset: 0x0002E404
	private void OnGirlDialogLineRead()
	{
		if (this.GetActiveDialogSceneStep().sceneLine.altGirl && this._altGirlShowing)
		{
			GameManager.Stage.altGirl.DialogLineReadEvent -= this.OnGirlDialogLineRead;
		}
		else
		{
			GameManager.Stage.girl.DialogLineReadEvent -= this.OnGirlDialogLineRead;
		}
		this.DialogSceneStep();
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00030274 File Offset: 0x0002E474
	private void OnResponseOptionSelected(TalkWindow talkWindow, DialogSceneResponseOption responseOption)
	{
		talkWindow.ResponseSelectedEvent -= this.OnResponseOptionSelected;
		GameManager.Stage.uiWindows.HideActiveWindow();
		DialogSceneStep activeDialogSceneStep = this.GetActiveDialogSceneStep();
		if (activeDialogSceneStep.hasBestOption)
		{
			if (responseOption == activeDialogSceneStep.responseOptions[0])
			{
				this.BestOptionBranchSelected(200);
			}
			else
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.talkBadChoiceSound, false, 1f, true);
			}
		}
		if (responseOption.steps.Count > 0)
		{
			this._activeDialogSceneSteps.Add(new DialogSceneStepsProgress(responseOption.steps));
		}
		this.DialogSceneStep();
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0003032C File Offset: 0x0002E52C
	private bool IsBranchConditionMet(DialogSceneConditionalBranch conditionalBranch)
	{
		bool flag = false;
		int num = 0;
		switch (conditionalBranch.type)
		{
		case DialogSceneBranchConditionType.ELSE:
			flag = true;
			break;
		case DialogSceneBranchConditionType.GIRL_MET_STATUS_COUNT:
		{
			List<GirlPlayerData> girls = GameManager.System.Player.girls;
			for (int i = 0; i < girls.Count; i++)
			{
				if (girls[i].metStatus == conditionalBranch.girlMetStatus)
				{
					num++;
				}
			}
			flag = (num >= conditionalBranch.goalValue);
			break;
		}
		case DialogSceneBranchConditionType.GIRL_DETAIL_KNOWN:
			flag = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).IsDetailKnown(conditionalBranch.girlDetailType);
			break;
		}
		if (conditionalBranch.type != DialogSceneBranchConditionType.ELSE && conditionalBranch.invertCondition)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00030404 File Offset: 0x0002E604
	private void ShowAltGirl(GirlDefinition girl, bool full, string styles, bool hideOppositeSpeechBubble)
	{
		GameManager.Stage.altGirl.girlPieceContainers.localX = -600f;
		GameManager.Stage.altGirl.ShowGirl(girl);
		if (!StringUtils.IsEmpty(styles))
		{
			string[] array = styles.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				GameManager.Stage.altGirl.ChangeStyle(StringUtils.ParseIntValue(array[i]), false);
			}
		}
		this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallbackWParms(this.OnAltGirlShown), new object[]
		{
			full,
			styles
		}));
		float p_time = 0f;
		if (full && GameManager.System.Player.tutorialComplete && GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).metStatus == GirlMetStatus.MET)
		{
			this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.75f, new TweenParms().Prop("localY", -10).Ease(EaseType.EaseInCubic)));
			p_time = 0.5f;
		}
		this._dialogSceneSequence.Insert(p_time, HOTween.To(GameManager.Stage.altGirl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseOutCubic)));
		if (hideOppositeSpeechBubble && this._mainGirlShowing)
		{
			this._dialogSceneSequence.Insert(p_time, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
			this._dialogSceneSequence.Insert(p_time, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
		}
		this._dialogSceneSequence.Play();
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x000069F0 File Offset: 0x00004BF0
	private void OnAltGirlShown(TweenEvent data)
	{
		this._altGirlShowing = true;
		this._proceedToNextStep = true;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0003064C File Offset: 0x0002E84C
	private void HideAltGirl(bool full, string styles, bool hideOppositeSpeechBubble)
	{
		this._altGirlShowing = false;
		this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallbackWParms(this.OnAltGirlHidden), new object[]
		{
			full,
			styles,
			hideOppositeSpeechBubble
		}));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.altGirl.girlPieceContainers, 1f, new TweenParms().Prop("localX", -600).Ease(EaseType.EaseInCubic)));
		if (full && GameManager.System.Player.tutorialComplete && GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).metStatus == GirlMetStatus.MET)
		{
			this._dialogSceneSequence.Insert(0.75f, HOTween.To(GameManager.Stage.uiGirl.stats, 0.75f, new TweenParms().Prop("localY", 151).Ease(EaseType.EaseOutCubic)));
		}
		this._dialogSceneSequence.Play();
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00030828 File Offset: 0x0002EA28
	private void OnAltGirlHidden(TweenEvent data)
	{
		GameManager.Stage.altGirl.ClearGirl();
		bool flag = (bool)data.parms[0];
		string styles = (string)data.parms[1];
		bool hideOppositeSpeechBubble = (bool)data.parms[2];
		if (flag)
		{
			this._proceedToNextStep = true;
		}
		else
		{
			this.ShowAltGirl(this.GetActiveDialogSceneStep().altGirl, flag, styles, hideOppositeSpeechBubble);
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00030894 File Offset: 0x0002EA94
	private void ShowMainGirl(string styles, bool hideOppositeSpeechBubble)
	{
		if (!StringUtils.IsEmpty(styles))
		{
			string[] array = styles.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				GameManager.Stage.girl.ChangeStyle(StringUtils.ParseIntValue(array[i]), false);
			}
		}
		this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnMainGirlShown)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseOutCubic)));
		if (hideOppositeSpeechBubble && this._altGirlShowing)
		{
			this._dialogSceneSequence.Insert(0.5f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
			this._dialogSceneSequence.Insert(0.5f, HOTween.To(GameManager.Stage.altGirl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
		}
		this._dialogSceneSequence.Play();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00030A18 File Offset: 0x0002EC18
	private void HideMainGirl()
	{
		this._mainGirlShowing = false;
		this._dialogSceneSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnMainGirlHidden)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
		this._dialogSceneSequence.Insert(0f, HOTween.To(GameManager.Stage.girl.girlPieceContainers, 1f, new TweenParms().Prop("localX", 600).Ease(EaseType.EaseInCubic)));
		this._dialogSceneSequence.Play();
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00006A00 File Offset: 0x00004C00
	private void OnMainGirlShown()
	{
		this._mainGirlShowing = true;
		this._proceedToNextStep = true;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00006A10 File Offset: 0x00004C10
	private void OnMainGirlHidden()
	{
		this._proceedToNextStep = true;
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00006A10 File Offset: 0x00004C10
	private void OnDialogSceneSequenceComplete()
	{
		this._proceedToNextStep = true;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00006A19 File Offset: 0x00004C19
	private void OnCellPhoneClosed()
	{
		GameManager.Stage.uiTop.CellPhoneClosedEvent -= this.OnCellPhoneClosed;
		this.DialogSceneStep();
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00006A3C File Offset: 0x00004C3C
	private void OnPuzzleGameReady()
	{
		GameManager.System.Puzzle.Game.PuzzleGameReadyEvent -= this.OnPuzzleGameReady;
		GameManager.System.Puzzle.Game.TutoriaLock();
		this._proceedToNextStep = true;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00006A79 File Offset: 0x00004C79
	private void OnWaitTimerComplete()
	{
		if (this._waitTimer != null)
		{
			this._waitTimer.Stop();
		}
		this._waitTimer = null;
		this.DialogSceneStep();
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00006A9E File Offset: 0x00004C9E
	private DialogSceneStepsProgress GetActiveDialogSceneSteps()
	{
		return this._activeDialogSceneSteps[this._activeDialogSceneSteps.Count - 1];
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00006AB8 File Offset: 0x00004CB8
	public DialogSceneStep GetActiveDialogSceneStep()
	{
		return this.GetActiveDialogSceneSteps().steps[this.GetActiveDialogSceneSteps().stepIndex];
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00006AD5 File Offset: 0x00004CD5
	public bool ShouldHideGirlDetails()
	{
		return this._activeDialogScene != null && this._activeDialogScene.conditions != null && this._activeDialogScene.conditions.Count > 0;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00006B0E File Offset: 0x00004D0E
	public bool IsMainGirlShowing()
	{
		return this._activeDialogScene == null || this._mainGirlShowing;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00006B29 File Offset: 0x00004D29
	public bool IsAltGirlShowing()
	{
		return this._altGirlShowing;
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00030B4C File Offset: 0x0002ED4C
	private void BestOptionBranchSelected(int hunie)
	{
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl);
		hunie = Mathf.RoundToInt((float)hunie * (1f + (float)girlData.UniqueGiftCount() * 0.5f) * (1f + (float)girlData.inebriation * 0.1f));
		GameManager.System.Player.hunie += hunie;
		EnergyTrail component = new GameObject("EnergyTrail", new Type[]
		{
			typeof(EnergyTrail)
		}).GetComponent<EnergyTrail>();
		component.Init(GameManager.Stage.uiGirl.itemGiftZone.gameObj.transform.position, GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail, "+" + hunie + " Hunie", EnergyTrailFormat.END, null);
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.extraResourceFlourishSound, false, 1f, true);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00006B31 File Offset: 0x00004D31
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		if (this._dialogSceneSequence != null && !this._dialogSceneSequence.isPaused)
		{
			this._dialogSceneSequence.Pause();
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00006B6C File Offset: 0x00004D6C
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		if (this._dialogSceneSequence != null && this._dialogSceneSequence.isPaused)
		{
			this._dialogSceneSequence.Play();
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00006BA7 File Offset: 0x00004DA7
	private void OnDestroy()
	{
		TweenUtils.KillSequence(this._dialogSceneSequence, false);
		this._dialogSceneSequence = null;
		if (this._waitTimer != null)
		{
			this._waitTimer.Stop();
		}
		this._waitTimer = null;
	}

	// Token: 0x04000785 RID: 1925
	public string dialogSpacerChar;

	// Token: 0x04000786 RID: 1926
	private DialogSceneDefinition _activeDialogScene;

	// Token: 0x04000787 RID: 1927
	private List<DialogSceneStepsProgress> _activeDialogSceneSteps = new List<DialogSceneStepsProgress>();

	// Token: 0x04000788 RID: 1928
	private Sequence _dialogSceneSequence;

	// Token: 0x04000789 RID: 1929
	private Timer _waitTimer;

	// Token: 0x0400078A RID: 1930
	private bool _mainGirlShowing;

	// Token: 0x0400078B RID: 1931
	private bool _altGirlShowing;

	// Token: 0x0400078C RID: 1932
	private DialogSceneStep _nextLocationStep;

	// Token: 0x0400078D RID: 1933
	private bool _paused;

	// Token: 0x0400078E RID: 1934
	private bool _proceedToNextStep;
}
