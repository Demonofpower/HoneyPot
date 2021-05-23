using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class PuzzleManager : MonoBehaviour
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x00006352 File Offset: 0x00004552
	public PuzzleGame Game
	{
		get
		{
			return this._activePuzzleGame;
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0000306D File Offset: 0x0000126D
	private void Start()
	{
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002BB94 File Offset: 0x00029D94
	private void Update()
	{
		if (this._paused)
		{
			return;
		}
		if (this._activePuzzleGame != null)
		{
			this._activePuzzleGame.OnUpdate();
		}
		if (this._completeNotifierShown)
		{
			this._completeNotifierShown = false;
			if (!this._activePuzzleGame.isBonusRound)
			{
				this.ShowPuzzleJackpotReward();
			}
			else
			{
				this.BonusRoundFadeOut();
			}
		}
		else if (this._jackpotRollupComplete)
		{
			this._jackpotRollupComplete = false;
			this.ReadDateValedictionDialog();
		}
		else if (this._puzzleGridHidden)
		{
			this._puzzleGridHidden = false;
			this.TravelFromPuzzleLocation();
		}
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0002BC30 File Offset: 0x00029E30
	public void TravelToPuzzleLocation(LocationDefinition location, GirlDefinition girl)
	{
		this._returnToLocation = GameManager.System.Location.currentLocation;
		GameManager.System.Location.LocationDepartEvent += this.OnDepartToPuzzleLocation;
		GameManager.System.Location.TravelTo(location, girl);
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0002BC80 File Offset: 0x00029E80
	private void OnDepartToPuzzleLocation()
	{
		GameManager.System.Location.LocationDepartEvent -= this.OnDepartToPuzzleLocation;
		this._activePuzzleGame = new PuzzleGame(GameManager.System.Location.currentLocation, GameManager.System.Location.currentGirl);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0002BCD4 File Offset: 0x00029ED4
	public void StartPuzzleGame()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		if (!this._activePuzzleGame.isTutorial)
		{
			GameManager.Stage.girl.DialogLineBeginEvent += this.OnGirlDialogLineStart;
			GameManager.Stage.girl.DialogLineReadEvent += this.OnGirlDialogLineEnd;
			int index = 0;
			if (GameManager.System.Location.currentLocation.bonusRoundLocation)
			{
				index = 12;
			}
			else
			{
				for (int i = 0; i < GameManager.System.Location.currentGirl.dateLocations.Count; i++)
				{
					if (GameManager.System.Location.currentGirl.dateLocations[i].location == GameManager.System.Location.currentLocation)
					{
						index = i;
						break;
					}
				}
			}
			GameManager.Stage.girl.DialogLineReadEvent += this.ShowPuzzleGrid;
			GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.dateGreetingDialogTrigger, index, false, -1);
		}
		else
		{
			this.ShowPuzzleGrid();
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002BE08 File Offset: 0x0002A008
	private void ShowPuzzleGrid()
	{
		GameManager.Stage.girl.DialogLineReadEvent -= this.ShowPuzzleGrid;
		TweenUtils.KillSequence(this._gridSequence, true);
		GameManager.Stage.uiPuzzle.puzzleGrid.gameObj.SetActive(true);
		GameManager.Stage.uiPuzzle.puzzleGrid.SetLocalScale(0.9f, 0f, EaseType.Linear);
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBorder.SetAlpha(0f, 0f);
		LocationBackground backgroundByDaytime = GameManager.System.Location.currentLocation.GetBackgroundByDaytime(GameManager.System.Clock.DayTime(-1));
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground.sprite.SetSprite(backgroundByDaytime.puzzleGridBackgroundName);
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBorder.sprite.SetSprite(backgroundByDaytime.puzzleGridBorderName);
		for (int i = 0; i < GameManager.Stage.uiPuzzle.puzzleStatus.puzzleEffectSlots.Count; i++)
		{
			GameManager.Stage.uiPuzzle.puzzleStatus.puzzleEffectSlots[i].background.sprite.SetSprite(backgroundByDaytime.puzzleEffectSlotBackgroundName);
		}
		this._gridSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnPuzzleGridShown)));
		this._gridSequence.Insert(0f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gameObj.transform, 0.75f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this._gridSequence.Insert(0f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground, 0.75f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutCubic)));
		this._gridSequence.Insert(0f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gridBorder, 0.75f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutCubic)));
		this._gridSequence.Play();
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0000635A File Offset: 0x0000455A
	private void OnPuzzleGridShown()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		this._activePuzzleGame.Begin();
		this._activePuzzleGame.PuzzleGameCompleteEvent += this.OnPuzzleGameComplete;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0002C098 File Offset: 0x0002A298
	private void OnPuzzleGameComplete()
	{
		this._activePuzzleGame.PuzzleGameCompleteEvent -= this.OnPuzzleGameComplete;
		TweenUtils.KillSequence(this._gridSequence, true);
		GameManager.System.Pauseable = false;
		if (this._activePuzzleGame.isVictorious && !this._activePuzzleGame.isTutorial)
		{
			GirlPlayerData girlData = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl);
			if (!this._activePuzzleGame.isBonusRound)
			{
				girlData.relationshipLevel++;
				GameManager.System.Player.successfulDateCount++;
				if (GameManager.System.Player.alphaModeActive)
				{
					GameManager.System.Player.alphaModeWins++;
				}
				int num = 0;
				for (int i = 0; i < GameManager.System.Location.currentGirl.dateLocations.Count; i++)
				{
					if (GameManager.System.Location.currentGirl.dateLocations[i].location == GameManager.System.Location.currentLocation)
					{
						num = GameManager.System.Location.currentGirl.dateLocations[i].outfit;
						break;
					}
				}
				if (num < GameManager.System.Location.currentGirl.outfits.Count && !girlData.IsOutfitUnlocked(num))
				{
					girlData.UnlockOutfit(num);
					GameManager.Stage.cellNotifications.Notify(CellNotificationType.PROFILE, string.Concat(new string[]
					{
						"You've unlocked ",
						StringUtils.Possessize(GameManager.System.Location.currentGirl.firstName),
						" \"",
						GameManager.System.Location.currentGirl.outfits[num].styleName,
						"\" outfit!"
					}));
				}
				if (girlData.relationshipLevel == 5 && GameManager.System.Clock.DayTime(-1) == GameClockDaytime.NIGHT)
				{
					this._goToBonusRound = true;
				}
			}
			else if (!girlData.gotPanties)
			{
				girlData.gotPanties = true;
				this._firstTimeBonusSuccess = true;
				girlData.AddPhotoEarned(3);
				if (!GameManager.System.Player.IsInventoryFull(null))
				{
					GameManager.System.Player.AddItem(GameManager.System.Location.currentGirl.pantiesItem, null, false, false);
					GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, "You got " + StringUtils.Possessize(GameManager.System.Location.currentGirl.firstName) + " panties!");
				}
			}
			else if (!GameManager.System.Player.IsInventoryFull(null) && !GameManager.System.Player.pantiesTurnedIn.Contains(GameManager.System.Location.currentGirl.pantiesItem.id) && !GameManager.System.Player.GetItemsOfTypeFromAllInventories(ItemType.PANTIES).Contains(GameManager.System.Location.currentGirl.pantiesItem))
			{
				GameManager.System.Player.AddItem(GameManager.System.Location.currentGirl.pantiesItem, null, false, false);
				GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, "You got " + StringUtils.Possessize(GameManager.System.Location.currentGirl.firstName) + " panties!");
			}
		}
		else if (!this._activePuzzleGame.isVictorious)
		{
			GameManager.System.Player.failureDateCount++;
		}
		if (this._activePuzzleGame.isVictorious)
		{
			GameManager.Stage.uiPuzzle.puzzleGrid.notifier.sprite.SetSprite("ui_puzzlegrid_notifier_success");
			GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.sprite.SetSprite("ui_puzzlegrid_notifier_success");
		}
		else
		{
			GameManager.Stage.uiPuzzle.puzzleGrid.notifier.sprite.SetSprite("ui_puzzlegrid_notifier_failure");
			GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.sprite.SetSprite("ui_puzzlegrid_notifier_failure");
		}
		GameManager.Stage.uiPuzzle.puzzleGrid.notifier.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifier.SetLocalScale(2f, 0f, EaseType.Linear);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.SetLocalScale(2f, 0f, EaseType.Linear);
		this._gridSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnPuzzleCompleteNotifierShown)));
		this._gridSequence.Insert(1f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifier.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(1f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifier, 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(1f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(1f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst, 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(1.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1.25f, 2f, 1f)).Ease(EaseType.EaseOutCubic)));
		this._gridSequence.Insert(1.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst, 0.25f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseOutCubic)));
		if (!this._activePuzzleGame.isBonusRound)
		{
			this._gridSequence.Insert(1.5f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifier, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
			this._gridSequence.InsertCallback(1f, new TweenDelegate.TweenCallback(this.PlayNotifierSoundEffects));
		}
		if (this._activePuzzleGame.isVictorious)
		{
			this._gridSequence.InsertCallback(1.25f, new TweenDelegate.TweenCallback(this.ShowNotifierParticleEffect));
		}
		this._gridSequence.Play();
		if (this._activePuzzleGame.isBonusRound)
		{
			GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.sexualSoundsDialogTrigger, 4, true, -1);
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0002C890 File Offset: 0x0002AA90
	private void PlayNotifierSoundEffects()
	{
		if (this._activePuzzleGame.isVictorious)
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.successSound, false, 1f, true);
		}
		else
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.failureSound, false, 1f, true);
		}
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0002C90C File Offset: 0x0002AB0C
	private void ShowNotifierParticleEffect()
	{
		ParticleEmitter2D component = new GameObject("NotifierParticleEmitter", new Type[]
		{
			typeof(ParticleEmitter2D)
		}).GetComponent<ParticleEmitter2D>();
		GameManager.Stage.effects.AddParticleEffect(component, GameManager.Stage.uiPuzzle.puzzleGrid.notifierContainer);
		component.Init(GameManager.Stage.uiPuzzle.puzzleGrid.notifierEffectEmitter, GameManager.Stage.uiPuzzle.puzzleGrid.notifierEffectSpriteGroup, true);
		component.details.originSpreadX = GameManager.Stage.uiPuzzle.puzzleGrid.notifier.sprite.GetBounds().size.x / 2f;
		component.details.originSpreadY = GameManager.Stage.uiPuzzle.puzzleGrid.notifier.sprite.GetBounds().size.y / 2f;
		component.SetGlobalPosition(GameManager.Stage.uiPuzzle.puzzleGrid.notifierContainer.gameObj.transform.position.x, GameManager.Stage.uiPuzzle.puzzleGrid.notifierContainer.gameObj.transform.position.y);
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0002CA6C File Offset: 0x0002AC6C
	private void OnPuzzleCompleteNotifierShown()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		GameManager.Stage.girl.DialogLineBeginEvent -= this.OnGirlDialogLineStart;
		GameManager.Stage.girl.DialogLineReadEvent -= this.OnGirlDialogLineEnd;
		this._completeNotifierShown = true;
		if (this._activePuzzleGame.isVictorious && !this._activePuzzleGame.isTutorial)
		{
			if (!this._activePuzzleGame.isBonusRound)
			{
				SteamUtils.UnlockAchievement("there_may_be_hope", false);
				if (this._activePuzzleGame.GetResourceValue(PuzzleGameResourceType.MOVES) >= 20)
				{
					SteamUtils.UnlockAchievement("quickie", false);
				}
				if (GameManager.System.Player.alphaModeActive && GameManager.System.Player.alphaModeWins >= 5)
				{
					SteamUtils.UnlockAchievement("alpha_champ", false);
				}
				if (GameManager.System.Player.alphaModeActive && GameManager.System.Player.alphaModeWins >= 10)
				{
					SteamUtils.UnlockAchievement("alpha_master", false);
				}
				SteamUtils.StoreAchievements();
			}
			else
			{
				SteamUtils.UnlockAchievement("vcard_revoked", false);
				if (GameManager.System.Location.currentGirl == GameManager.Stage.uiGirl.goddessGirlDef)
				{
					SteamUtils.UnlockAchievement("truest_player", false);
				}
				if (GameManager.System.Clock.CalendarDay(-1, true, false) <= 4)
				{
					SteamUtils.UnlockAchievement("pickup_artist", false);
				}
				if (GameManager.System.Player.GetTotalMaxRelationships() == 12 && GameManager.System.Clock.CalendarDay(-1, true, false) <= 18)
				{
					SteamUtils.UnlockAchievement("pickup_legend", false);
				}
				if (GameManager.System.Player.GetTotalMaxRelationships() == 12 && GameManager.System.Player.failureDateCount == 0)
				{
					SteamUtils.UnlockAchievement("smooth_as_fuck", false);
				}
				if (GameManager.System.Player.GetTotalMaxRelationships() == 12 && GameManager.System.Player.drinksGivenOut == 0)
				{
					SteamUtils.UnlockAchievement("sobriety", false);
				}
				if (GameManager.System.Player.GetTotalMaxRelationships() == 12 && GameManager.System.Player.chatSessionCount == 0)
				{
					SteamUtils.UnlockAchievement("buying_love", false);
				}
				if (GameManager.System.Player.GetTotalMaxRelationships() >= 3 && GameManager.System.Player.GetTotalTraitUpgradeCount() == 0)
				{
					SteamUtils.UnlockAchievement("lucky_loser", false);
				}
				SteamUtils.StoreAchievements();
			}
		}
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0002CD04 File Offset: 0x0002AF04
	private void ShowPuzzleJackpotReward()
	{
		GameManager.Stage.uiPuzzle.puzzleStatus.UpdatePuzzleEffects(null);
		GameManager.Stage.uiPuzzle.puzzleStatus.passionMax.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleStatus.movesMax.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleStatus.sentimentMax.SetAlpha(0f, 0f);
		float num = 4f;
		if (!this._activePuzzleGame.isVictorious)
		{
			num = 2.5f;
		}
		this._activePuzzleGame.affectionMeterTweener = HOTween.To(this.Game, num, new TweenParms().Prop("currentDisplayAffection", 0).Ease(EaseType.EaseOutSine));
		GameManager.Stage.uiPuzzle.puzzleStatus.SetPassionLevel(0);
		GameManager.Stage.uiPuzzle.puzzleStatus.movesLabel.SetText(0, 0.1f, true, num);
		GameManager.Stage.uiPuzzle.puzzleStatus.sentimentLabel.SetText(0, 0.1f, true, num);
		this._jackpotLabel = DisplayUtils.CreateJackpotLabelObject(GameManager.Stage.uiPuzzle.puzzleGrid.jackpotEffectFont, GameManager.Stage.uiPuzzle.puzzleGrid.jackpotEnergyTrail, "JackpotLabelObject");
		this._jackpotLabel.JackpotLabelRolledEvent += this.OnJackpotRolledUp;
		this._jackpotLabel.JackpotLabelCompleteEvent += this.OnJackpotComplete;
		this._jackpotLabel.Init(GameManager.Stage.uiPuzzle.puzzleStatus.giftZone.gameObj.transform.position + new Vector3(-50f, -40f, 0f), GameManager.Stage.girl, this._activePuzzleGame.GetPuzzleMoneyReward(), "Munie", num, this._activePuzzleGame.isVictorious);
		if (this._activePuzzleGame.isVictorious)
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.successJackpotSound, false, 1f, true);
		}
		else
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.failureJackpotSound, false, 1f, true);
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0000638A File Offset: 0x0000458A
	private void OnJackpotRolledUp()
	{
		this._jackpotLabel.JackpotLabelRolledEvent -= this.OnJackpotRolledUp;
		GameManager.System.Player.money += this._activePuzzleGame.GetPuzzleMoneyReward();
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000063C4 File Offset: 0x000045C4
	private void OnJackpotComplete()
	{
		this._jackpotLabel.JackpotLabelCompleteEvent -= this.OnJackpotComplete;
		this._jackpotLabel = null;
		this._jackpotRollupComplete = true;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0002CF78 File Offset: 0x0002B178
	private void ReadDateValedictionDialog()
	{
		TweenUtils.KillSequence(this._speechBubbleSequence, true);
		int index = 1;
		int subIndex = -1;
		if (this._activePuzzleGame.isVictorious)
		{
			index = 0;
			if (this._goToBonusRound)
			{
				index = 2;
			}
			else if (this._activePuzzleGame.isTutorial)
			{
				subIndex = 4;
			}
		}
		GameManager.Stage.girl.DialogLineReadEvent += this.OnDateValedictionDialogRead;
		GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.dateValedictionDialogTrigger, index, false, subIndex);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x000063EB File Offset: 0x000045EB
	private void OnDateValedictionDialogRead()
	{
		GameManager.Stage.girl.DialogLineReadEvent -= this.OnDateValedictionDialogRead;
		this.HidePuzzleGrid();
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0002D008 File Offset: 0x0002B208
	private void HidePuzzleGrid()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		this._gridSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnPuzzleGridHidden)));
		this._gridSequence.Insert(0.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifier.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(2f, 2f, 1f)).Ease(EaseType.EaseInSine)));
		this._gridSequence.Insert(0.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.notifier, 0.25f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine)));
		this._gridSequence.Insert(0.25f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(0.5f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gameObj.transform, 0.75f, new TweenParms().Prop("localScale", new Vector3(0.9f, 0.9f, 1f)).Ease(EaseType.EaseInBack)));
		this._gridSequence.Insert(0.5f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground, 0.75f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Insert(0.5f, HOTween.To(GameManager.Stage.uiPuzzle.puzzleGrid.gridBorder, 0.75f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInCubic)));
		this._gridSequence.Play();
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0000640E File Offset: 0x0000460E
	private void OnPuzzleGridHidden()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		this._puzzleGridHidden = true;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0002D240 File Offset: 0x0002B440
	private void BonusRoundFadeOut()
	{
		float p_duration = 8f;
		if (!this._firstTimeBonusSuccess)
		{
			p_duration = 10f;
			GameManager.Stage.background.StopBackgroundMusic(10f);
		}
		else
		{
			GameManager.Stage.background.AdjustBackgroundMusicVolume(0.32f, 10f);
		}
		this._gridSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnBonusRoundFadeOutComplete)));
		this._gridSequence.Insert(0.5f, HOTween.To(GameManager.Stage.transitionScreen.overlay, 2.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine)));
		this._gridSequence.Insert(0f, HOTween.To(GameManager.Stage.transitionScreen.overlay, p_duration, new TweenParms().Prop("localX", GameManager.Stage.transitionScreen.overlay.localX).Ease(EaseType.Linear)));
		this._gridSequence.Play();
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002D35C File Offset: 0x0002B55C
	private void OnBonusRoundFadeOutComplete()
	{
		TweenUtils.KillSequence(this._gridSequence, true);
		if (this._firstTimeBonusSuccess)
		{
			GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent += this.OnPhotoGalleryClosed;
			GameManager.Stage.uiPhotoGallery.ShowPhotoGallery(GameManager.System.Location.currentGirl, GameManager.System.Location.currentGirl.photos.Count - 1, true, false);
		}
		else
		{
			this._puzzleGridHidden = true;
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00006423 File Offset: 0x00004623
	private void OnPhotoGalleryClosed()
	{
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent -= this.OnPhotoGalleryClosed;
		this._puzzleGridHidden = true;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0002D3E4 File Offset: 0x0002B5E4
	private void TravelFromPuzzleLocation()
	{
		bool isBonusRound = this._activePuzzleGame.isBonusRound;
		bool isTutorial = this._activePuzzleGame.isTutorial;
		this._activePuzzleGame.Destroy();
		this._activePuzzleGame = null;
		GameManager.Stage.uiPuzzle.puzzleGrid.SetLocalScale(0.9f, 0f, EaseType.Linear);
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBorder.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer.SetChildAlpha(1f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifier.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifier.SetLocalScale(1f, 0f, EaseType.Linear);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.SetAlpha(0f, 0f);
		GameManager.Stage.uiPuzzle.puzzleGrid.notifierBurst.SetLocalScale(1f, 0f, EaseType.Linear);
		GameManager.Stage.uiPuzzle.puzzleGrid.gameObj.SetActive(false);
		if (isBonusRound)
		{
			this._goToBonusRound = false;
			GameManager.System.Location.TravelTo(GameManager.Stage.uiPuzzle.puzzleStatus.postBonusRoundLocation, GameManager.System.Location.currentGirl);
		}
		else if (this._goToBonusRound)
		{
			this._goToBonusRound = false;
			this.TravelToPuzzleLocation(GameManager.Stage.uiPuzzle.puzzleStatus.bonusRoundLocation, GameManager.System.Location.currentGirl);
		}
		else
		{
			if (!isTutorial)
			{
				GameManager.System.Location.SkipNextGreeting();
			}
			GameManager.System.Location.TravelTo(this._returnToLocation, GameManager.System.Location.currentGirl);
		}
		this._returnToLocation = null;
		this._firstTimeBonusSuccess = false;
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00006447 File Offset: 0x00004647
	private void OnGirlDialogLineStart()
	{
		TweenUtils.KillSequence(this._speechBubbleSequence, true);
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002D618 File Offset: 0x0002B818
	private void OnGirlDialogLineEnd()
	{
		TweenUtils.KillSequence(this._speechBubbleSequence, true);
		this._speechBubbleSequence = new Sequence();
		this._speechBubbleSequence.Insert(1f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInBack)));
		this._speechBubbleSequence.Insert(1f, HOTween.To(GameManager.Stage.girl.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.8f, 0.8f, 1f)).Ease(EaseType.EaseInBack)));
		this._speechBubbleSequence.Play();
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002D6F0 File Offset: 0x0002B8F0
	public Vector3 GetVectorByPuzzleDirection(PuzzleDirection direction)
	{
		switch (direction)
		{
		case PuzzleDirection.RIGHT:
			return Vector3.right;
		case PuzzleDirection.UP:
			return Vector3.up;
		case PuzzleDirection.LEFT:
			return Vector3.left;
		case PuzzleDirection.DOWN:
			return Vector3.down;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0002D73C File Offset: 0x0002B93C
	public int GetAffectionGoal(GirlDefinition girlDefinition)
	{
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		List<GirlPlayerData> girls = GameManager.System.Player.girls;
		int num;
		if (GameManager.System.Player.alphaModeActive)
		{
			num = this.ALPHA_AFFECTION_GOALS[Mathf.Clamp(GameManager.System.Player.alphaModeWins, 0, this.ALPHA_AFFECTION_GOALS.Length - 1)];
		}
		else
		{
			int num2 = 0;
			for (int i = 0; i < girls.Count; i++)
			{
				if (girls[i].metStatus == GirlMetStatus.MET)
				{
					num2 += Mathf.Clamp(girls[i].relationshipLevel - 1, 0, 4);
				}
			}
			if (girlData.relationshipLevel >= 5)
			{
				num2--;
			}
			num = this.DATE_AFFECTION_GOALS[Mathf.Clamp(num2, 0, this.DATE_AFFECTION_GOALS.Length - 1)];
			if (GameManager.System.Player.settingsDifficulty == SettingsDifficulty.EASY)
			{
				num = Mathf.RoundToInt((float)num * 0.66f);
			}
			else if (GameManager.System.Player.settingsDifficulty == SettingsDifficulty.HARD)
			{
				num = Mathf.RoundToInt((float)num * 1.33f);
			}
		}
		return num;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00006455 File Offset: 0x00004655
	public float GetAffectionTraitLevelMultiplier(PlayerTraitType traitType)
	{
		return this.TRAIT_LEVEL_MULTIPLIERS[GameManager.System.Player.GetTraitLevel(traitType)];
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0000646E File Offset: 0x0000466E
	public float GetPowerTokenMultiplier()
	{
		return this.POWER_TOKEN_MULTIPLIERS[GameManager.System.Player.GetTraitLevel(PlayerTraitType.CHARISMA)];
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0002D86C File Offset: 0x0002BA6C
	public float GetChanceForPowerToken(int matchTokenCount)
	{
		if (matchTokenCount <= 3)
		{
			return this.POWER_TOKEN_CHANCE_THREE[GameManager.System.Player.GetTraitLevel(PlayerTraitType.LUCK)];
		}
		if (matchTokenCount == 4)
		{
			return this.POWER_TOKEN_CHANCE_FOUR[GameManager.System.Player.GetTraitLevel(PlayerTraitType.LUCK)];
		}
		if (matchTokenCount == 5)
		{
			return this.POWER_TOKEN_CHANCE_FIVE[GameManager.System.Player.GetTraitLevel(PlayerTraitType.LUCK)];
		}
		return 1f;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00006487 File Offset: 0x00004687
	public float GetPassionTraitLevelMultiplier()
	{
		return this.PASSION_LEVEL_MULTIPLIERS[GameManager.System.Player.GetTraitLevel(PlayerTraitType.PASSION)];
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x000064A0 File Offset: 0x000046A0
	public int GetMaxPassionLevel()
	{
		return this.MAX_PASSION_LEVELS[GameManager.System.Player.GetTraitLevel(PlayerTraitType.PASSION)];
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000064B9 File Offset: 0x000046B9
	public float GetPassionLevelMultiplier(int passionLevel)
	{
		return 1f + (float)passionLevel * 0.25f;
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0002D8DC File Offset: 0x0002BADC
	public int GetPassionLevelCost(int passionLevel)
	{
		int num = 0;
		for (int i = 0; i < passionLevel; i++)
		{
			num += 6 + 2 * i;
		}
		return num;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x000064C9 File Offset: 0x000046C9
	public float GetSensitivityTraitLevelMultiplier()
	{
		return this.SENSITIVITY_LEVEL_MULTIPLIERS[GameManager.System.Player.GetTraitLevel(PlayerTraitType.SENSITIVITY)];
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0002D908 File Offset: 0x0002BB08
	public int GetTokenEnergyTrailLimit()
	{
		SettingsLimit settingsLimit = GameManager.System.settingsLimit;
		if (settingsLimit == SettingsLimit.MEDIUM)
		{
			return 6;
		}
		if (settingsLimit != SettingsLimit.LOW)
		{
			return 12;
		}
		return 0;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000064E2 File Offset: 0x000046E2
	public bool IsPaused()
	{
		return this._paused;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0002D93C File Offset: 0x0002BB3C
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		if (this._gridSequence != null && !this._gridSequence.isPaused)
		{
			this._gridSequence.Pause();
		}
		if (this._speechBubbleSequence != null && !this._speechBubbleSequence.isPaused)
		{
			this._speechBubbleSequence.Pause();
		}
		if (this._activePuzzleGame != null)
		{
			this._activePuzzleGame.Pause();
		}
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0002D9C0 File Offset: 0x0002BBC0
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		if (this._gridSequence != null && this._gridSequence.isPaused)
		{
			this._gridSequence.Play();
		}
		if (this._speechBubbleSequence != null && this._speechBubbleSequence.isPaused)
		{
			this._speechBubbleSequence.Play();
		}
		if (this._activePuzzleGame != null)
		{
			this._activePuzzleGame.Unpaused();
		}
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0002DA44 File Offset: 0x0002BC44
	private void OnDestroy()
	{
		TweenUtils.KillSequence(this._gridSequence, false);
		this._gridSequence = null;
		TweenUtils.KillSequence(this._speechBubbleSequence, false);
		this._speechBubbleSequence = null;
		if (this._jackpotLabel != null)
		{
			UnityEngine.Object.Destroy(this._jackpotLabel);
		}
		this._jackpotLabel = null;
	}

	// Token: 0x040006EC RID: 1772
	public const int GRID_ROWS = 7;

	// Token: 0x040006ED RID: 1773
	public const int GRID_COLS = 8;

	// Token: 0x040006EE RID: 1774
	public const int GRID_SIZE = 56;

	// Token: 0x040006EF RID: 1775
	public const float GRID_SQUARE_SIZE = 82f;

	// Token: 0x040006F0 RID: 1776
	public const float GRID_SQUARE_SIZE_HALF = 41f;

	// Token: 0x040006F1 RID: 1777
	public const float LABEL_ANIM_DURATION_PER = 0.1f;

	// Token: 0x040006F2 RID: 1778
	public const float LABEL_ANIM_DURATION_MAX = 1.5f;

	// Token: 0x040006F3 RID: 1779
	public const string PUZZLE_GRID_NOTIFIER_SUCCESS = "ui_puzzlegrid_notifier_success";

	// Token: 0x040006F4 RID: 1780
	public const string PUZZLE_GRID_NOTIFIER_FAILURE = "ui_puzzlegrid_notifier_failure";

	// Token: 0x040006F5 RID: 1781
	private readonly int[] DATE_AFFECTION_GOALS = new int[]
	{
		120,
		150,
		185,
		220,
		260,
		300,
		350,
		395,
		450,
		505,
		565,
		625,
		690,
		760,
		830,
		905,
		985,
		1065,
		1150,
		1240,
		1330,
		1425,
		1520,
		1625,
		1725,
		1835,
		1945,
		2055,
		2175,
		2295,
		2415,
		2545,
		2675,
		2805,
		2940,
		3080,
		3225,
		3370,
		3520,
		3670,
		3825,
		3985,
		4145,
		4310,
		4480,
		4650,
		4825,
		5000
	};

	// Token: 0x040006F6 RID: 1782
	private readonly int[] ALPHA_AFFECTION_GOALS = new int[]
	{
		6000,
		6250,
		6550,
		6900,
		7300,
		7750,
		8250,
		8800,
		9400,
		10050,
		10750,
		11500,
		12300,
		13150,
		14050,
		15000,
		16000,
		17050,
		18150,
		19300,
		20500,
		21750,
		23050,
		24400,
		25800,
		27250,
		28750,
		30300,
		31900,
		33550,
		35250,
		37000,
		38800,
		40650,
		42550,
		44500,
		46500,
		48550,
		50650,
		52800,
		55000,
		57250,
		59550,
		61900,
		64300,
		66750,
		69250,
		71800,
		74400,
		77050,
		79750,
		82500,
		85300,
		88150,
		91050,
		94000,
		97000,
		100050,
		103150,
		106300,
		109500,
		112750,
		116050,
		119400,
		122800,
		126250,
		129750,
		133300,
		136900,
		140550,
		144250,
		148000,
		151800,
		155650,
		159550,
		163500,
		167500,
		171550,
		175650,
		179800,
		184000,
		188250,
		192550,
		196900,
		201300,
		205750,
		210250,
		214800,
		219400,
		224050,
		228750,
		233500,
		238300,
		243150,
		248050,
		253000,
		258000,
		263050,
		268150,
		273300
	};

	// Token: 0x040006F7 RID: 1783
	private readonly float[] TRAIT_LEVEL_MULTIPLIERS = new float[]
	{
		1f,
		2f,
		3f,
		4f,
		5f,
		6f,
		7f
	};

	// Token: 0x040006F8 RID: 1784
	private readonly float[] PASSION_LEVEL_MULTIPLIERS = new float[]
	{
		1f,
		2f,
		3f,
		4f,
		5f,
		6f,
		7f
	};

	// Token: 0x040006F9 RID: 1785
	private readonly int[] MAX_PASSION_LEVELS = new int[]
	{
		4,
		6,
		8,
		10,
		12,
		14,
		16
	};

	// Token: 0x040006FA RID: 1786
	private readonly float[] SENSITIVITY_LEVEL_MULTIPLIERS = new float[]
	{
		0.12f,
		0.11f,
		0.1f,
		0.09f,
		0.08f,
		0.07f,
		0.06f
	};

	// Token: 0x040006FB RID: 1787
	private readonly float[] POWER_TOKEN_MULTIPLIERS = new float[]
	{
		1.5f,
		1.75f,
		2f,
		2.25f,
		2.5f,
		2.75f,
		3f
	};

	// Token: 0x040006FC RID: 1788
	private readonly float[] POWER_TOKEN_CHANCE_THREE = new float[]
	{
		0f,
		0f,
		0f,
		0f,
		0.05f,
		0.1f,
		0.2f
	};

	// Token: 0x040006FD RID: 1789
	private readonly float[] POWER_TOKEN_CHANCE_FOUR = new float[]
	{
		0.2f,
		0.3f,
		0.4f,
		0.5f,
		0.6f,
		0.7f,
		0.8f
	};

	// Token: 0x040006FE RID: 1790
	private readonly float[] POWER_TOKEN_CHANCE_FIVE = new float[]
	{
		0.7f,
		0.8f,
		0.9f,
		1f,
		1f,
		1f,
		1f
	};

	// Token: 0x040006FF RID: 1791
	public readonly List<PuzzleDirection> CHECK_DIRECTIONS = new List<PuzzleDirection>
	{
		PuzzleDirection.LEFT,
		PuzzleDirection.RIGHT,
		PuzzleDirection.UP,
		PuzzleDirection.DOWN
	};

	// Token: 0x04000700 RID: 1792
	public readonly List<List<int>> TUTORIAL_GRID = new List<List<int>>
	{
		new List<int>
		{
			8,
			7,
			1,
			5,
			6,
			8,
			8,
			8,
			8,
			8,
			4,
			5,
			8,
			3,
			6,
			7
		},
		new List<int>
		{
			2,
			4,
			3,
			6,
			1,
			2,
			4
		},
		new List<int>
		{
			7,
			1,
			7,
			2,
			5,
			6,
			3,
			1,
			7,
			2,
			2
		},
		new List<int>
		{
			8,
			3,
			1,
			4,
			2,
			3,
			8
		},
		new List<int>
		{
			5,
			2,
			4,
			8,
			4,
			6,
			5,
			3,
			4,
			6,
			7
		},
		new List<int>
		{
			1,
			1,
			5,
			7,
			3,
			1,
			4,
			4,
			3,
			6,
			7
		},
		new List<int>
		{
			5,
			2,
			6,
			3,
			1,
			5,
			7,
			5
		},
		new List<int>
		{
			5,
			3,
			7,
			2,
			8,
			4,
			2,
			5
		}
	};

	// Token: 0x04000701 RID: 1793
	private PuzzleGame _activePuzzleGame;

	// Token: 0x04000702 RID: 1794
	private Sequence _gridSequence;

	// Token: 0x04000703 RID: 1795
	private Sequence _speechBubbleSequence;

	// Token: 0x04000704 RID: 1796
	private JackpotLabelObject _jackpotLabel;

	// Token: 0x04000705 RID: 1797
	private LocationDefinition _returnToLocation;

	// Token: 0x04000706 RID: 1798
	private bool _completeNotifierShown;

	// Token: 0x04000707 RID: 1799
	private bool _jackpotRollupComplete;

	// Token: 0x04000708 RID: 1800
	private bool _puzzleGridHidden;

	// Token: 0x04000709 RID: 1801
	private bool _goToBonusRound;

	// Token: 0x0400070A RID: 1802
	private bool _firstTimeBonusSuccess;

	// Token: 0x0400070B RID: 1803
	private bool _paused;
}
