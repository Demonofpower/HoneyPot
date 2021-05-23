using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class GirlManager : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600067D RID: 1661 RVA: 0x00006E87 File Offset: 0x00005087
	private Girl _activeGirl
	{
		get
		{
			if (GameManager.System.Location.currentGirl != null && GameManager.System.Location.IsLocationSettled())
			{
				return GameManager.Stage.girl;
			}
			return null;
		}
	}

    public Girl GetGirl => _activeGirl;

	// Token: 0x0600067E RID: 1662 RVA: 0x00006EC3 File Offset: 0x000050C3
	private void Update()
	{
		if (this._paused || this._activeGirl == null)
		{
			return;
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00031B10 File Offset: 0x0002FD10
	public bool GiveItem(ItemDefinition item)
	{
		if (this._activeGirl == null || (item.type != ItemType.FOOD && item.type != ItemType.DRINK && item.type != ItemType.GIFT && item.type != ItemType.UNIQUE_GIFT && item.type != ItemType.PANTIES && item.type != ItemType.MISC))
		{
			return false;
		}
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(this._activeGirl.definition);
		DialogTriggerDefinition dialogTrigger = null;
		int index = 0;
		bool flag = true;
		switch (item.type)
		{
		case ItemType.FOOD:
			dialogTrigger = GameManager.Stage.uiGirl.givenFoodDialogTrigger;
			if (girlData.appetite == 12)
			{
				index = 0;
				flag = false;
			}
			else if (item.foodType == this._activeGirl.definition.lovesFoodType)
			{
				index = 1;
			}
			else if (item.foodType == this._activeGirl.definition.likesFoodType)
			{
				index = 2;
			}
			else
			{
				index = 3;
				flag = false;
			}
			if (flag)
			{
				girlData.appetite += item.itemFunctionValue;
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveFoodEnergyTrail, "+" + item.itemFunctionValue.ToString() + " Nutrition", EnergyTrailFormat.END, null);
			}
			break;
		case ItemType.DRINK:
			dialogTrigger = GameManager.Stage.uiGirl.givenDrinkDialogTrigger;
			if (girlData.inebriation == 12)
			{
				index = 0;
				flag = false;
			}
			else if (girlData.appetite == 0)
			{
				index = 1;
				flag = false;
			}
			else if (!GameManager.System.Location.currentLocation.drinkingLocation)
			{
				index = 2;
				if (!this._activeGirl.definition.drinksAnytime)
				{
					flag = false;
				}
			}
			else
			{
				index = 3;
			}
			if (flag)
			{
				int num = item.itemFunctionValue;
				switch (this._activeGirl.definition.alcoholTolerance)
				{
				case GirlAlcoholToleranceType.LOW:
					num = Mathf.RoundToInt((float)num * 1.33f);
					break;
				case GirlAlcoholToleranceType.HIGH:
					num = Mathf.RoundToInt((float)num * 0.66f);
					break;
				case GirlAlcoholToleranceType.INSANELY_HIGH:
					num = Mathf.RoundToInt((float)num * 0.33f);
					break;
				}
				girlData.inebriation += num;
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveDrinkEnergyTrail, "+" + num.ToString() + " Intoxication", EnergyTrailFormat.END, null);
				if (item == this._activeGirl.definition.favDrink)
				{
					int num2 = 180 + 180 * girlData.UniqueGiftCount();
					GameManager.System.Player.hunie += num2;
					PopLabelObject popLabelObject = DisplayUtils.CreatePopLabelObject(GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail.popLabelFont, "+" + num2.ToString() + " Hunie", "PopLabelObject");
					popLabelObject.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer, true);
				}
				GameManager.System.Player.drinksGivenOut++;
			}
			break;
		case ItemType.GIFT:
			dialogTrigger = GameManager.Stage.uiGirl.givenGiftDialogTrigger;
			if (girlData.appetite == 0)
			{
				dialogTrigger = GameManager.Stage.uiGirl.isHungryDialogTrigger;
				index = 0;
				flag = false;
			}
			else if (item.giftType == this._activeGirl.definition.lovesGiftType)
			{
				index = 0;
				if (!GameManager.System.Player.IsInventoryFull(null))
				{
					List<ItemDefinition> list = ListUtils.RemoveListElementsFromList<ItemDefinition>(GameManager.Data.Items.GetAllOfType(ItemType.DATE_GIFT, false), GameManager.System.Player.GetItemsOfTypeFromAllInventories(ItemType.DATE_GIFT));
					if (list.Count > 0)
					{
						GameManager.System.Player.AddItem(list[UnityEngine.Random.Range(0, list.Count)], GameManager.System.Player.inventory, true, false);
						GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, GirlManager.FAIRY_PRESENT_NOTIFICATIONS[UnityEngine.Random.Range(0, GirlManager.FAIRY_PRESENT_NOTIFICATIONS.Length)]);
						GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(39));
					}
				}
			}
			else if (item.giftType == this._activeGirl.definition.likesGiftType1)
			{
				index = 1;
			}
			else if (item.giftType == this._activeGirl.definition.likesGiftType2)
			{
				index = 2;
			}
			else
			{
				index = 4;
				flag = false;
			}
			if (flag)
			{
				int num3 = Mathf.RoundToInt((float)(item.itemFunctionValue * 100) * (1f + (float)girlData.inebriation * 0.1f));
				GameManager.System.Player.hunie += num3;
				girlData.AddItemToCollection(item);
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail, "+" + num3.ToString() + " Hunie", EnergyTrailFormat.END, null);
			}
			break;
		case ItemType.UNIQUE_GIFT:
			dialogTrigger = GameManager.Stage.uiGirl.givenGiftDialogTrigger;
			if (girlData.appetite == 0)
			{
				dialogTrigger = GameManager.Stage.uiGirl.isHungryDialogTrigger;
				index = 0;
				flag = false;
			}
			else if (item.specialGiftType == this._activeGirl.definition.uniqueGiftType)
			{
				index = 3;
			}
			else
			{
				index = 4;
				flag = false;
			}
			if (flag)
			{
				girlData.AddItemToUniqueGifts(item);
				girlData.AddItemToCollection(item);
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveUniqueEnergyTrail, null, EnergyTrailFormat.END, null);
			}
			break;
		case ItemType.PANTIES:
			dialogTrigger = GameManager.Stage.uiGirl.kyuSpecialDialogTrigger;
			if (this._activeGirl.definition != GameManager.Stage.uiGirl.fairyGirlDef || !GameManager.System.Player.endingSceneShown || GameManager.System.Player.pantiesTurnedIn.Contains(item.id))
			{
				dialogTrigger = GameManager.Stage.uiGirl.givenGiftDialogTrigger;
				index = 4;
				flag = false;
			}
			else
			{
				index = item.id - 277;
				GameManager.System.Player.pantiesTurnedIn.Add(item.id);
				if (GameManager.System.Player.pantiesTurnedIn.Count == 12)
				{
					index = 12;
					GameManager.System.Player.alphaModeActive = true;
					GameManager.System.Player.settingsDifficulty = SettingsDifficulty.MEDIUM;
					GameManager.Stage.cellNotifications.Notify(CellNotificationType.MESSAGE, "Alpha Mode Activated!");
					SteamUtils.UnlockAchievement("alpha", true);
				}
				if (flag)
				{
					EnergyTrail component = new GameObject("EnergyTrail", new Type[]
					{
						typeof(EnergyTrail)
					}).GetComponent<EnergyTrail>();
					component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail, null, EnergyTrailFormat.END, null);
				}
			}
			break;
		case ItemType.MISC:
			dialogTrigger = GameManager.Stage.uiGirl.givenGiftDialogTrigger;
			if (item == GameManager.Stage.uiGirl.lovePotionDef && !girlData.lovePotionUsed)
			{
				dialogTrigger = GameManager.Stage.uiGirl.givenDateGiftDialogTrigger;
				index = 0;
				girlData.lovePotionUsed = true;
			}
			else if (this._activeGirl.definition != GameManager.Stage.uiGirl.fairyGirlDef)
			{
				index = 4;
				flag = false;
			}
			else if (GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory))
			{
				dialogTrigger = GameManager.Stage.uiGirl.inventoryFullDialogTrigger;
				index = 0;
				flag = false;
			}
			else if (item == GameManager.Stage.uiGirl.dirtyMagazineDef)
			{
				index = 5;
				if (!GameManager.System.Player.IsInventoryFull(GameManager.System.Player.gifts))
				{
					GameManager.System.Player.AddItem(GameManager.Stage.uiGirl.weirdThingDef, GameManager.System.Player.gifts, false, false);
				}
				else
				{
					GameManager.System.Player.AddItem(GameManager.Stage.uiGirl.weirdThingDef, GameManager.System.Player.inventory, false, false);
				}
				GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, "Received a \"Weird Thing\" from Kyu!");
			}
			else if (item == GameManager.Stage.uiGirl.tissueBoxDef)
			{
				dialogTrigger = GameManager.Stage.uiGirl.kyuSpecialDialogTrigger;
				index = 13;
				GameManager.System.Player.AddItem(GameManager.Stage.uiGirl.kyuPlushieDef, GameManager.System.Player.inventory, false, false);
				GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, "Received a \"Kyu Plushie\" from Kyu!");
			}
			else
			{
				index = 4;
				flag = false;
			}
			if (flag)
			{
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail, null, EnergyTrailFormat.END, null);
			}
			break;
		}
		this.TriggerDialog(dialogTrigger, index, false, -1);
		if (flag)
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.itemGiveSuccessSound, false, 1f, true);
			GameManager.Stage.uiWindows.RefreshActiveWindow();
		}
		else
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.itemGiveFailureSound, false, 1f, true);
		}
		GameManager.Stage.uiWindows.RefreshActiveWindow();
		return flag;
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000325E4 File Offset: 0x000307E4
	public void TriggerDialog(DialogTriggerDefinition dialogTrigger, int index = 0, bool hideSpeechBubble = false, int subIndex = -1)
	{
		if (this._activeGirl == null)
		{
			return;
		}
		DialogLine dialogTriggerLine = this.GetDialogTriggerLine(dialogTrigger, index, subIndex);
		this._activeGirl.ReadDialogLine(dialogTriggerLine, !dialogTrigger.skippable, false, hideSpeechBubble, dialogTrigger == GameManager.Stage.uiGirl.sexualSoundsDialogTrigger);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0003263C File Offset: 0x0003083C
	public DialogLine GetDialogTriggerLine(DialogTriggerDefinition dialogTrigger, int index, int subIndex = -1)
	{
		if (this._activeGirl == null)
		{
			return null;
		}
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(this._activeGirl.definition);
		index = Mathf.Clamp(index, 0, dialogTrigger.lineSets.Count - 1);
		List<DialogTriggerLine> list = ListUtils.Copy<DialogTriggerLine>(dialogTrigger.lineSets[index].lines);
		DialogTriggerLine dialogTriggerLine;
		if (subIndex < 0)
		{
			int loggedDialogTriggerLineIndex = girlData.GetLoggedDialogTriggerLineIndex(dialogTrigger.id, index);
			if (loggedDialogTriggerLineIndex >= 0 && list.Count > 1)
			{
				list.RemoveAt(loggedDialogTriggerLineIndex);
			}
			dialogTriggerLine = list[UnityEngine.Random.Range(0, list.Count)];
			girlData.LogDialogTriggerLine(dialogTrigger.id, index, dialogTrigger.lineSets[index].lines.IndexOf(dialogTriggerLine));
		}
		else
		{
			dialogTriggerLine = list[Mathf.Clamp(subIndex, 0, list.Count - 1)];
		}
		return dialogTriggerLine.dialogLine[this._activeGirl.definition.id - 1];
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00032740 File Offset: 0x00030940
	public void TalkWithHer()
	{
		if (this._activeGirl == null)
		{
			return;
		}
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(this._activeGirl.definition);
		if (girlData.appetite >= 2)
		{
			girlData.appetite -= 2;
			List<DialogSceneDefinition> list = new List<DialogSceneDefinition>();
			for (int i = 0; i < this._activeGirl.definition.talkQueries.Count; i++)
			{
				if (GameManager.System.GameLogic.GameConditionsMet(this._activeGirl.definition.talkQueries[i].conditions, false))
				{
					list.Add(this._activeGirl.definition.talkQueries[i]);
					list.Add(this._activeGirl.definition.talkQueries[i]);
					list.Add(this._activeGirl.definition.talkQueries[i]);
					if (girlData.gotPanties && GameManager.System.Player.endingSceneShown)
					{
						list.Add(this._activeGirl.definition.talkQueries[i]);
						list.Add(this._activeGirl.definition.talkQueries[i]);
						list.Add(this._activeGirl.definition.talkQueries[i]);
					}
				}
			}
			for (int j = 0; j < this._activeGirl.definition.talkQuizzes.Count; j++)
			{
				if (GameManager.System.GameLogic.GameConditionsMet(this._activeGirl.definition.talkQuizzes[j].conditions, true) && !girlData.IsRecentQuiz(j))
				{
					list.Add(this._activeGirl.definition.talkQuizzes[j]);
				}
			}
			for (int k = 0; k < this._activeGirl.definition.talkQuestions.Count; k++)
			{
				if (GameManager.System.GameLogic.GameConditionsMet(this._activeGirl.definition.talkQuestions[k].conditions, true) && !girlData.IsRecentQuestion(k))
				{
					list.Add(this._activeGirl.definition.talkQuestions[k]);
				}
			}
			DialogSceneDefinition dialogSceneDefinition = list[UnityEngine.Random.Range(0, list.Count)];
			if (this._activeGirl.definition.talkQuizzes.Contains(dialogSceneDefinition))
			{
				girlData.AddRecentQuiz(this._activeGirl.definition.talkQuizzes.IndexOf(dialogSceneDefinition));
			}
			if (this._activeGirl.definition.talkQuestions.Contains(dialogSceneDefinition))
			{
				girlData.AddRecentQuestion(this._activeGirl.definition.talkQuestions.IndexOf(dialogSceneDefinition));
			}
			GameManager.System.Dialog.PlayDialogScene(dialogSceneDefinition);
			int num = Mathf.RoundToInt(50f * (1f + (float)girlData.UniqueGiftCount() * 0.5f) * (1f + (float)girlData.inebriation * 0.1f));
			GameManager.System.Player.hunie += num;
			EnergyTrail component = new GameObject("EnergyTrail", new Type[]
			{
				typeof(EnergyTrail)
			}).GetComponent<EnergyTrail>();
			component.Init(GameManager.Stage.uiGirl.itemGiftZone.gameObj.transform.position, GameManager.Stage.uiGirl.itemGiveGiftEnergyTrail, "+" + num + " Hunie", EnergyTrailFormat.END, null);
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.extraResourceFlourishSound, false, 1f, true);
			if (girlData.appetite < 2)
			{
				GameManager.System.Player.AddMessage(GameManager.Data.Messages.Get(40));
			}
		}
		else
		{
			this.TriggerDialog(GameManager.Stage.uiGirl.isHungryDialogTrigger, 0, false, -1);
			GameManager.Stage.uiWindows.ResetActiveWindow();
		}
		GameManager.System.Player.chatSessionCount++;
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00032BA4 File Offset: 0x00030DA4
	public void ShowEnergySurge(GirlEnergySurge energySurge, GirlEnergySurge secondaryEnergySurge = null)
	{
		if (this._activeGirl == null)
		{
			return;
		}
		SpriteGroupDefinition spriteGroup = energySurge.spriteGroup;
		string tint = energySurge.tint;
		if (secondaryEnergySurge != null)
		{
			spriteGroup = secondaryEnergySurge.spriteGroup;
			tint = secondaryEnergySurge.tint;
		}
		TweenUtils.KillSequence(this._energySurgeSequence, false);
		if (energySurge.showExpression && !this._activeGirl.IsReadingDialog())
		{
			this._activeGirl.ChangeExpression(energySurge.expression, false, true, false, energySurge.duration);
		}
		this._energySurgeSequence = new Sequence();
		if (energySurge.knockback > 0f)
		{
			this._energySurgeSequence.Insert(0f, HOTween.To(this._activeGirl.girlContainer, energySurge.duration * 0.4f, new TweenParms().Prop("localX", Mathf.Min(GameManager.Stage.girl.girlContainer.localX + energySurge.knockback, 80f)).Ease(EaseType.EaseOutCubic)));
			this._energySurgeSequence.Insert(energySurge.duration * 0.4f, HOTween.To(this._activeGirl.girlContainer, energySurge.duration * 0.6f, new TweenParms().Prop("localX", 0).Ease(EaseType.EaseInCubic)));
		}
		if (tint != null && tint != string.Empty)
		{
			this._energySurgeSequence.Insert(0f, HOTween.To(this._activeGirl.girlContainer, energySurge.duration * 0.4f, new TweenParms().Prop("childrenColor", ColorUtils.HexToColor(tint) * 2f).Ease(EaseType.EaseInOutSine)));
			this._energySurgeSequence.Insert(energySurge.duration * 0.4f, HOTween.To(this._activeGirl.girlContainer, energySurge.duration, new TweenParms().Prop("childrenColor", Color.white).Ease(EaseType.EaseInOutSine)));
		}
		this._energySurgeSequence.Play();
		ParticleEmitter2D component = new GameObject("GirlEnergySurgeParticleEmitter", new Type[]
		{
			typeof(ParticleEmitter2D)
		}).GetComponent<ParticleEmitter2D>();
		this._activeGirl.AddChild(component);
		component.Init(energySurge.particleEmitter, spriteGroup, true);
		component.SetGlobalPosition(948f, 480f);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00006EE2 File Offset: 0x000050E2
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		if (this._energySurgeSequence != null && !this._energySurgeSequence.isPaused)
		{
			this._energySurgeSequence.Pause();
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00006F1D File Offset: 0x0000511D
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		if (this._energySurgeSequence != null && this._energySurgeSequence.isPaused)
		{
			this._energySurgeSequence.Play();
		}
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00006F58 File Offset: 0x00005158
	private void OnDestroy()
	{
		TweenUtils.KillSequence(this._energySurgeSequence, false);
		this._energySurgeSequence = null;
	}

	// Token: 0x040007E7 RID: 2023
	public static readonly string[] FAIRY_PRESENT_NOTIFICATIONS = new string[]
	{
		"You've just received a new present!",
		"You were sent a new present!",
		"A new present has arrived!"
	};

	// Token: 0x040007E8 RID: 2024
	private Sequence _energySurgeSequence;

	// Token: 0x040007E9 RID: 2025
	private bool _paused;
}
