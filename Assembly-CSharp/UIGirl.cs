using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class UIGirl : DisplayObject
{
	// Token: 0x060002BD RID: 701 RVA: 0x0001A898 File Offset: 0x00018A98
	protected override void OnStart()
	{
		base.OnStart();
		this.stats = base.GetChildByName("GirlStatsUI");
		this.statsPic = (this.stats.GetChildByName("GirlStatsPic") as SpriteObject);
		this.liveStats = (this.stats.GetChildByName("GirlLiveStats") as GirlLiveStats);
		this.itemsContainer = this.stats.GetChildByName("GirlUIItems");
		this.itemGiftZone = base.GetChildByName("GirlUIGiftZone");
		this._itemSlots = new List<UIGirlItemSlot>();
		for (int i = 0; i < 6; i++)
		{
			UIGirlItemSlot uigirlItemSlot = this.itemsContainer.GetChildByName("GirlUIItem" + i.ToString()) as UIGirlItemSlot;
			uigirlItemSlot.Init(i);
			uigirlItemSlot.UIGirlItemSlotDownEvent += this.OnUIGirlItemSlotDown;
			this._itemSlots.Add(uigirlItemSlot);
		}
		this.stats.localY = -10f;
		GameManager.System.Player.GirlStatsChangedEvent += this.OnGirlStatsChanged;
		this.stats.interactive = false;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0001A9B4 File Offset: 0x00018BB4
	protected override void OnStageStarted()
	{
		GameManager.Stage.uiWindows.UIWindowShownEvent += this.OnUIWindowShown;
		GameManager.Stage.uiWindows.UIWindowHidingEvent += this.OnUIWindowHiding;
		GameManager.Stage.uiTop.CellPhoneClosedEvent += this.OnCellPhoneClosed;
		this._giftItemCursor = DisplayUtils.CreateSpriteObject(GameManager.Stage.uiPuzzle.puzzleStatus.itemsSpriteCollection, "item_blank", "GiftItemCursor");
		this._giftItemCursor.gameObj.SetActive(false);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0001AA4C File Offset: 0x00018C4C
	public void ShowCurrentGirlStats()
	{
		if (GameManager.System.Location.currentGirl == null)
		{
			return;
		}
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl);
		if (girlData.metStatus == GirlMetStatus.MET)
		{
			this.statsPic.sprite.SetSprite(GameManager.System.Location.currentGirl.firstName.ToLower() + "_trans");
		}
		else
		{
			this.statsPic.sprite.SetSprite(GameManager.System.Location.currentGirl.firstName.ToLower() + "_trans_unknown");
		}
		this.liveStats.RefreshStats(GameManager.System.Location.currentGirl, false);
		this.RefreshItemSlots();
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0001AB30 File Offset: 0x00018D30
	private void OnGirlStatsChanged(GirlDefinition girlDef)
	{
		if (GameManager.System.Location.currentGirl == null || !GameManager.System.Location.IsLocationSettled())
		{
			return;
		}
		if (GameManager.System.Location.currentGirl == girlDef)
		{
			this.liveStats.RefreshStats(GameManager.System.Location.currentGirl, true);
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001ABA4 File Offset: 0x00018DA4
	public void RefreshItemSlots()
	{
		for (int i = 0; i < this._itemSlots.Count; i++)
		{
			this._itemSlots[i].PopulateSlotItem();
			if (GameManager.System.GameState != GameState.SIM || this._giftItemMode || this._itemSlots[i].itemDefinition == null || !GameManager.System.Location.IsLocationSettled() || !GameManager.Stage.uiWindows.IsDefaultWindowActive(true))
			{
				this._itemSlots[i].button.Disable();
				this._itemSlots[i].tooltip.Disable();
			}
			else
			{
				this._itemSlots[i].button.Enable();
				this._itemSlots[i].tooltip.Enable();
			}
		}
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001ACAC File Offset: 0x00018EAC
	private void OnUIGirlItemSlotDown(UIGirlItemSlot itemSlot)
	{
		if (GameManager.System.GameState != GameState.SIM || itemSlot.itemDefinition == null || !GameManager.System.Location.IsLocationSettled() || !GameManager.Stage.uiWindows.IsDefaultWindowActive(true))
		{
			return;
		}
		this.StartItemGive(itemSlot);
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0001AD0C File Offset: 0x00018F0C
	private void StartItemGive(UIGirlItemSlot itemSlot)
	{
		GameManager.Stage.cellPhone.Lock(false);
		this._giftItemMode = true;
		this._giftItemSlot = itemSlot;
		this._giftItemCursor.gameObj.SetActive(true);
		this._giftItemCursor.sprite.SetSprite(this._giftItemSlot.itemDefinition.iconName);
		GameManager.Stage.effects.lowerCursorContainer.AddChild(this._giftItemCursor);
		GameManager.System.Cursor.AttachObject(this._giftItemCursor, 0f, 0f);
		GameManager.System.Cursor.SetAbsorber(this.itemGiftZone, true);
		GameManager.System.Audio.Play(AudioCategory.SOUND, this.pickUpItemSound, false, 1f, true);
		this.RefreshItemSlots();
		this._giftItemSlot.itemIcon.SetAlpha(0f, 0f);
		GameManager.Stage.MouseUpEvent += this.OnStageUp;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x000044C2 File Offset: 0x000026C2
	private void OnStageUp(DisplayObject displayObject)
	{
		GameManager.Stage.MouseUpEvent -= this.OnStageUp;
		this.EndItemGive();
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0001AE0C File Offset: 0x0001900C
	private void EndItemGive()
	{
		TweenUtils.KillTweener(this._giftItemTweener, true);
		GameManager.System.Cursor.DetachObject(this._giftItemCursor);
		DisplayObject mouseTarget = GameManager.System.Cursor.GetMouseTarget();
		if (mouseTarget == this.itemGiftZone && GameManager.System.Girl.GiveItem(this._giftItemSlot.itemDefinition))
		{
			this._giftItemSlot.ConsumeSlotItem();
			this.ItemGiveComplete();
			return;
		}
		this._giftItemTweener = HOTween.To(this._giftItemCursor.gameObj.transform, 0.2f, new TweenParms().Prop("position", new Vector3(this._giftItemSlot.gameObj.transform.position.x, this._giftItemSlot.gameObj.transform.position.y, this._giftItemCursor.gameObj.transform.position.z)).Ease(EaseType.EaseOutCubic).OnComplete(new TweenDelegate.TweenCallback(this.ItemGiveComplete)));
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001AF44 File Offset: 0x00019144
	private void ItemGiveComplete()
	{
		this._giftItemSlot.itemIcon.SetAlpha(1f, 0f);
		this._giftItemSlot = null;
		GameManager.System.Cursor.ClearAbsorber();
		GameManager.Stage.effects.lowerCursorContainer.RemoveChild(this._giftItemCursor, false);
		this._giftItemCursor.gameObj.SetActive(false);
		this._giftItemMode = false;
		GameManager.Stage.cellPhone.Unlock();
		this.RefreshItemSlots();
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000044E0 File Offset: 0x000026E0
	public bool IsInGiftItemMode()
	{
		return this._giftItemMode && (this._giftItemTweener == null || !this._giftItemTweener.IsTweening(this._giftItemCursor.gameObj.transform));
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000451C File Offset: 0x0000271C
	private void OnUIWindowShown()
	{
		this.RefreshItemSlots();
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0000451C File Offset: 0x0000271C
	private void OnUIWindowHiding()
	{
		this.RefreshItemSlots();
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00004524 File Offset: 0x00002724
	private void OnCellPhoneClosed()
	{
		if (GameManager.System.GameState == GameState.SIM)
		{
			this.RefreshItemSlots();
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001AFCC File Offset: 0x000191CC
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._itemSlots.Count; i++)
		{
			this._itemSlots[i].UIGirlItemSlotDownEvent -= this.OnUIGirlItemSlotDown;
		}
		this._itemSlots.Clear();
		this._itemSlots = null;
		UnityEngine.Object.Destroy(this._giftItemCursor.gameObj);
		GameManager.System.Player.GirlStatsChangedEvent -= this.OnGirlStatsChanged;
		GameManager.Stage.uiWindows.UIWindowShownEvent -= this.OnUIWindowShown;
		GameManager.Stage.uiWindows.UIWindowHidingEvent -= this.OnUIWindowHiding;
		GameManager.Stage.uiTop.CellPhoneClosedEvent -= this.OnCellPhoneClosed;
	}

	// Token: 0x0400026A RID: 618
	public const int GIRL_STATS_HIDDEN_Y_POS = -10;

	// Token: 0x0400026B RID: 619
	public const int GIRL_STATS_SHOWN_Y_POS = 151;

	// Token: 0x0400026C RID: 620
	public List<DialogSceneDefinition> openingScenes;

	// Token: 0x0400026D RID: 621
	public DialogSceneDefinition endingScene;

	// Token: 0x0400026E RID: 622
	public GirlDefinition fairyGirlDef;

	// Token: 0x0400026F RID: 623
	public GirlDefinition catGirlDef;

	// Token: 0x04000270 RID: 624
	public GirlDefinition alienGirlDef;

	// Token: 0x04000271 RID: 625
	public GirlDefinition goddessGirlDef;

	// Token: 0x04000272 RID: 626
	public ItemDefinition bagOfFishDef;

	// Token: 0x04000273 RID: 627
	public ItemDefinition tissueBoxDef;

	// Token: 0x04000274 RID: 628
	public ItemDefinition dirtyMagazineDef;

	// Token: 0x04000275 RID: 629
	public ItemDefinition weirdThingDef;

	// Token: 0x04000276 RID: 630
	public ItemDefinition kyuPlushieDef;

	// Token: 0x04000277 RID: 631
	public ItemDefinition lovePotionDef;

	// Token: 0x04000278 RID: 632
	public LocationDefinition alienGirlLocation;

	// Token: 0x04000279 RID: 633
	public DialogTriggerDefinition greetingDialogTrigger;

	// Token: 0x0400027A RID: 634
	public DialogTriggerDefinition valedictionDialogTrigger;

	// Token: 0x0400027B RID: 635
	public DialogTriggerDefinition givenFoodDialogTrigger;

	// Token: 0x0400027C RID: 636
	public DialogTriggerDefinition givenDrinkDialogTrigger;

	// Token: 0x0400027D RID: 637
	public DialogTriggerDefinition givenGiftDialogTrigger;

	// Token: 0x0400027E RID: 638
	public DialogTriggerDefinition isHungryDialogTrigger;

	// Token: 0x0400027F RID: 639
	public DialogTriggerDefinition inventoryFullDialogTrigger;

	// Token: 0x04000280 RID: 640
	public DialogTriggerDefinition askOnDateDialogTrigger;

	// Token: 0x04000281 RID: 641
	public DialogTriggerDefinition dateGreetingDialogTrigger;

	// Token: 0x04000282 RID: 642
	public DialogTriggerDefinition dateValedictionDialogTrigger;

	// Token: 0x04000283 RID: 643
	public DialogTriggerDefinition givenDateGiftDialogTrigger;

	// Token: 0x04000284 RID: 644
	public DialogTriggerDefinition matchTokenDialogTrigger;

	// Token: 0x04000285 RID: 645
	public DialogTriggerDefinition sexualSoundsDialogTrigger;

	// Token: 0x04000286 RID: 646
	public DialogTriggerDefinition kyuSpecialDialogTrigger;

	// Token: 0x04000287 RID: 647
	public AudioDefinition pickUpItemSound;

	// Token: 0x04000288 RID: 648
	public AudioDefinition itemGiveSuccessSound;

	// Token: 0x04000289 RID: 649
	public AudioDefinition itemGiveFailureSound;

	// Token: 0x0400028A RID: 650
	public AudioDefinition extraResourceFlourishSound;

	// Token: 0x0400028B RID: 651
	public AudioDefinition talkBadChoiceSound;

	// Token: 0x0400028C RID: 652
	public AudioGroup girlDialogBoopSounds;

	// Token: 0x0400028D RID: 653
	public EnergyTrailDefinition itemGiveGiftEnergyTrail;

	// Token: 0x0400028E RID: 654
	public EnergyTrailDefinition itemGiveUniqueEnergyTrail;

	// Token: 0x0400028F RID: 655
	public EnergyTrailDefinition itemGiveFoodEnergyTrail;

	// Token: 0x04000290 RID: 656
	public EnergyTrailDefinition itemGiveDrinkEnergyTrail;

	// Token: 0x04000291 RID: 657
	public EnergyTrailDefinition itemGiveEnergyTrail;

	// Token: 0x04000292 RID: 658
	public DisplayObject stats;

	// Token: 0x04000293 RID: 659
	public SpriteObject statsPic;

	// Token: 0x04000294 RID: 660
	public GirlLiveStats liveStats;

	// Token: 0x04000295 RID: 661
	public DisplayObject itemsContainer;

	// Token: 0x04000296 RID: 662
	public DisplayObject itemGiftZone;

	// Token: 0x04000297 RID: 663
	private List<UIGirlItemSlot> _itemSlots;

	// Token: 0x04000298 RID: 664
	private bool _giftItemMode;

	// Token: 0x04000299 RID: 665
	private UIGirlItemSlot _giftItemSlot;

	// Token: 0x0400029A RID: 666
	private SpriteObject _giftItemCursor;

	// Token: 0x0400029B RID: 667
	private Tweener _giftItemTweener;
}
