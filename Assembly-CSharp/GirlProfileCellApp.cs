using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class GirlProfileCellApp : UICellApp
{
	// Token: 0x060001B7 RID: 439 RVA: 0x000137C0 File Offset: 0x000119C0
	public override void Init()
	{
		if (GameManager.Stage.cellPhone.cellMemory.ContainsKey("cell_memory_profile_girl"))
		{
			this._girlDefinition = GameManager.Data.Girls.Get(GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_girl"]);
		}
		else
		{
			this._girlDefinition = GameManager.System.Location.currentGirl;
		}
		this._girlPlayerData = GameManager.System.Player.GetGirlData(this._girlDefinition);
		this._currentProfileTab = 0;
		if (GameManager.Stage.cellPhone.cellMemory.ContainsKey("cell_memory_profile_tab"))
		{
			this._currentProfileTab = GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_tab"];
		}
		else
		{
			GameManager.Stage.cellPhone.cellMemory.Add("cell_memory_profile_tab", this._currentProfileTab);
		}
		this.girlIcon = (base.GetChildByName("GirlProfileIcon") as SpriteObject);
		this.liveStats = (base.GetChildByName("GirlProfileGirlLiveStats") as GirlLiveStats);
		this.profileTabHeader = (base.GetChildByName("GirlProfileTabHeader") as SpriteObject);
		this.tabPreferences = base.GetChildByName("GirlProfileTabPreferences");
		this.tabDetails = base.GetChildByName("GirlProfileTabDetails");
		this.tabCollection = base.GetChildByName("GirlProfileTabCollection");
		this.tabStyle = base.GetChildByName("GirlProfileTabStyle");
		this.tabPhotos = base.GetChildByName("GirlProfileTabPhotos");
		this.mostTraitIcons = (this.tabPreferences.GetChildByName("GirlProfileMostTraitIcons") as SpriteObject);
		this.leastTraitIcons = (this.tabPreferences.GetChildByName("GirlProfileLeastTraitIcons") as SpriteObject);
		this.girlIcon.sprite.SetSprite(this._girlDefinition.firstName.ToLower() + "_full");
		this.liveStats.Init();
		this.liveStats.RefreshStats(this._girlDefinition, false);
		this._arrowLeft = (base.GetChildByName("GirlProfileArrowLeft") as SpriteObject);
		this._arrowLeft.button.ButtonPressedEvent += this.OnArrowPressed;
		this._arrowRight = (base.GetChildByName("GirlProfileArrowRight") as SpriteObject);
		this._arrowRight.button.ButtonPressedEvent += this.OnArrowPressed;
		this._tabContents = new Dictionary<string, DisplayObject>();
		this._tabContents.Add("cell_app_profile_header_preferences", this.tabPreferences);
		this._tabContents.Add("cell_app_profile_header_details", this.tabDetails);
		this._tabContents.Add("cell_app_profile_header_collection", this.tabCollection);
		this._tabContents.Add("cell_app_profile_header_style", this.tabStyle);
		this._tabContents.Add("cell_app_profile_header_photos", this.tabPhotos);
		this._preferences = new List<GirlProfilePreference>();
		for (int i = 0; i < Enum.GetNames(typeof(GirlProfilePreferenceType)).Length; i++)
		{
			GirlProfilePreference girlProfilePreference = this.tabPreferences.GetChildByName("GirlProfilePreference" + i.ToString()) as GirlProfilePreference;
			girlProfilePreference.Init(this._girlDefinition, i);
			this._preferences.Add(girlProfilePreference);
		}
		this.mostTraitIcons.sprite.SetSprite("cell_app_upgrades_trait_icons_" + this._girlDefinition.mostDesiredTrait.name.ToLower());
		this.leastTraitIcons.sprite.SetSprite("cell_app_upgrades_trait_icons_" + this._girlDefinition.leastDesiredTrait.name.ToLower());
		this.mostTraitIcons.localX += 1f;
		this.leastTraitIcons.localX += 1f;
		this._details = new List<GirlProfileDetail>();
		for (int j = 0; j < Enum.GetNames(typeof(GirlDetailType)).Length; j++)
		{
			GirlProfileDetail girlProfileDetail = this.tabDetails.GetChildByName("GirlProfileDetail" + j.ToString()) as GirlProfileDetail;
			girlProfileDetail.Init(this._girlDefinition, this._girlPlayerData, j);
			this._details.Add(girlProfileDetail);
		}
		DisplayObject childByName = this.tabCollection.GetChildByName("GirlProfileTabCollectionSlots");
		this._collectionSlots = new List<GirlProfileCollectionSlot>();
		for (int k = 0; k < 24; k++)
		{
			GirlProfileCollectionSlot girlProfileCollectionSlot = childByName.GetChildByName("GirlProfileCollectionSlot" + k.ToString()) as GirlProfileCollectionSlot;
			girlProfileCollectionSlot.Init(this._girlDefinition, this._girlPlayerData, k);
			this._collectionSlots.Add(girlProfileCollectionSlot);
			girlProfileCollectionSlot.CollectionSlotPressedEvent += this.OnCollectionSlotPressed;
		}
		DisplayObject childByName2 = this.tabStyle.GetChildByName("GirlProfileStyleHaircut").GetChildByName("GirlProfileStyleSwitches");
		this._hairstyleSwitches = new List<GirlProfileStyleSwitch>();
		for (int l = 0; l < 6; l++)
		{
			GirlProfileStyleSwitch girlProfileStyleSwitch = childByName2.GetChildByName("GirlProfileStyleSwitch" + l.ToString()) as GirlProfileStyleSwitch;
			girlProfileStyleSwitch.Init(this._girlDefinition, this._girlPlayerData, l, true);
			this._hairstyleSwitches.Add(girlProfileStyleSwitch);
			if (girlProfileStyleSwitch.unlocked)
			{
				girlProfileStyleSwitch.StyleSwitchPressedEvent += this.OnHairstyleSwitchPressed;
				if (girlProfileStyleSwitch.styleIndex == this._girlPlayerData.hairstyle)
				{
					this._selectedHairstyleSwitch = girlProfileStyleSwitch;
					girlProfileStyleSwitch.button.Disable();
				}
			}
		}
		DisplayObject childByName3 = this.tabStyle.GetChildByName("GirlProfileStyleOutfit").GetChildByName("GirlProfileStyleSwitches");
		this._outfitSwitches = new List<GirlProfileStyleSwitch>();
		for (int m = 0; m < 6; m++)
		{
			GirlProfileStyleSwitch girlProfileStyleSwitch2 = childByName3.GetChildByName("GirlProfileStyleSwitch" + m.ToString()) as GirlProfileStyleSwitch;
			girlProfileStyleSwitch2.Init(this._girlDefinition, this._girlPlayerData, m, false);
			this._outfitSwitches.Add(girlProfileStyleSwitch2);
			if (girlProfileStyleSwitch2.unlocked)
			{
				girlProfileStyleSwitch2.StyleSwitchPressedEvent += this.OnOutfitSwitchPressed;
				if (girlProfileStyleSwitch2.styleIndex == this._girlPlayerData.outfit)
				{
					this._selectedOutfitSwitch = girlProfileStyleSwitch2;
					girlProfileStyleSwitch2.button.Disable();
				}
			}
		}
		this._photos = new List<GirlProfilePhoto>();
		for (int n = 0; n < 4; n++)
		{
			GirlProfilePhoto girlProfilePhoto = this.tabPhotos.GetChildByName("GirlProfilePhoto" + n.ToString()) as GirlProfilePhoto;
			girlProfilePhoto.Init(n);
			girlProfilePhoto.PhotoPressedEvent += this.OnPhotoClicked;
			this._photos.Add(girlProfilePhoto);
			if (this._girlPlayerData.IsPhotoEarned(n))
			{
				string sprite = this._girlDefinition.photos[n].smallSpriteName[0];
				if (this._girlDefinition.photos[n].hasAlts && GameManager.System.settingsCensored)
				{
					sprite = this._girlDefinition.photos[n].smallSpriteName[1];
				}
				girlProfilePhoto.sprite.SetSprite(sprite);
			}
			else
			{
				girlProfilePhoto.button.Disable();
			}
		}
		this.Refresh();
		GameManager.Stage.cellNotifications.ClearNotificationsOfType(CellNotificationType.PROFILE);
		base.Init();
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00013F44 File Offset: 0x00012144
	public override void Refresh()
	{
		this.profileTabHeader.sprite.SetSprite(GirlProfileCellApp.PROFILE_TABS[this._currentProfileTab]);
		foreach (string text in this._tabContents.Keys)
		{
			if (text == GirlProfileCellApp.PROFILE_TABS[this._currentProfileTab])
			{
				this._tabContents[text].gameObj.SetActive(true);
			}
			else
			{
				this._tabContents[text].gameObj.SetActive(false);
			}
		}
		if (GirlProfileCellApp.PROFILE_TABS[this._currentProfileTab] == "cell_app_profile_header_details" && this._girlDefinition == GameManager.System.Location.currentGirl && GameManager.System.Dialog.ShouldHideGirlDetails())
		{
			GameManager.Stage.cellPhone.ShowCellAppError("Nice try, cheater!", false, -102f);
		}
		else
		{
			GameManager.Stage.cellPhone.HideCellAppError();
		}
		if (GirlProfileCellApp.PROFILE_TABS[this._currentProfileTab] == "cell_app_profile_header_collection")
		{
			for (int i = 0; i < this._collectionSlots.Count; i++)
			{
				this._collectionSlots[i].Refresh();
			}
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000140C8 File Offset: 0x000122C8
	private void OnArrowPressed(ButtonObject buttonObject)
	{
		SpriteObject x = buttonObject.GetDisplayObject() as SpriteObject;
		if (x == this._arrowLeft)
		{
			if (this._currentProfileTab - 1 < 0)
			{
				this._currentProfileTab = GirlProfileCellApp.PROFILE_TABS.Length - 1;
			}
			else
			{
				this._currentProfileTab--;
			}
		}
		else if (GirlProfileCellApp.PROFILE_TABS.Length > this._currentProfileTab + 1)
		{
			this._currentProfileTab++;
		}
		else
		{
			this._currentProfileTab = 0;
		}
		GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_tab"] = this._currentProfileTab;
		this.Refresh();
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0001417C File Offset: 0x0001237C
	private void OnCollectionSlotPressed(GirlProfileCollectionSlot collectionSlot)
	{
		if (collectionSlot.itemDefinition.type == ItemType.GIFT && GameManager.System.GameState == GameState.SIM && GameManager.System.Player.endingSceneShown && !GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory) && GameManager.System.Player.hunie >= 10000)
		{
			GameManager.System.Player.AddItem(collectionSlot.itemDefinition, GameManager.System.Player.inventory, true, false);
			GameManager.System.Player.hunie -= 10000;
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.orderItemSound, false, 1f, true);
			GameManager.Stage.cellNotifications.Notify(CellNotificationType.INVENTORY, GirlManager.FAIRY_PRESENT_NOTIFICATIONS[UnityEngine.Random.Range(0, GirlManager.FAIRY_PRESENT_NOTIFICATIONS.Length)]);
			this.Refresh();
			GameManager.Stage.tooltip.Refresh();
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00014294 File Offset: 0x00012494
	private void OnHairstyleSwitchPressed(GirlProfileStyleSwitch styleSwitch)
	{
		if (this._selectedHairstyleSwitch != null)
		{
			this._selectedHairstyleSwitch.button.Enable();
		}
		this._selectedHairstyleSwitch = null;
		this._selectedHairstyleSwitch = styleSwitch;
		this._selectedHairstyleSwitch.button.Disable();
		this._girlPlayerData.hairstyle = this._selectedHairstyleSwitch.styleIndex;
		if (GameManager.System.Location.currentLocation.type == LocationType.NORMAL && this._girlDefinition == GameManager.System.Location.currentGirl && this._girlPlayerData.hairstyle < this._girlDefinition.hairstyles.Count)
		{
			GameManager.Stage.girl.ChangeStyle(this._girlDefinition.hairstyles[this._girlPlayerData.hairstyle].artIndex, true);
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00014380 File Offset: 0x00012580
	private void OnOutfitSwitchPressed(GirlProfileStyleSwitch styleSwitch)
	{
		if (this._selectedOutfitSwitch != null)
		{
			this._selectedOutfitSwitch.button.Enable();
		}
		this._selectedOutfitSwitch = null;
		this._selectedOutfitSwitch = styleSwitch;
		this._selectedOutfitSwitch.button.Disable();
		this._girlPlayerData.outfit = this._selectedOutfitSwitch.styleIndex;
		if (GameManager.System.Location.currentLocation.type == LocationType.NORMAL && GameManager.System.Location.currentLocation.outfitOverride <= 0 && this._girlDefinition == GameManager.System.Location.currentGirl && this._girlPlayerData.outfit < this._girlDefinition.outfits.Count)
		{
			GameManager.Stage.girl.ChangeStyle(this._girlDefinition.outfits[this._girlPlayerData.outfit].artIndex, true);
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00014484 File Offset: 0x00012684
	private void OnPhotoClicked(GirlProfilePhoto photo)
	{
		GameManager.Stage.cellPhone.Lock(false);
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent += this.OnPhotoGalleryClosed;
		GameManager.Stage.uiPhotoGallery.ShowPhotoGallery(this._girlDefinition, photo.photoIndex, false, false);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00003B9F File Offset: 0x00001D9F
	private void OnPhotoGalleryClosed()
	{
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent -= this.OnPhotoGalleryClosed;
		GameManager.Stage.cellPhone.Unlock();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000144DC File Offset: 0x000126DC
	protected override void Destructor()
	{
		base.Destructor();
		this._preferences.Clear();
		this._preferences = null;
		this._details.Clear();
		this._details = null;
		for (int i = 0; i < this._collectionSlots.Count; i++)
		{
			this._collectionSlots[i].CollectionSlotPressedEvent -= this.OnCollectionSlotPressed;
		}
		this._collectionSlots.Clear();
		this._collectionSlots = null;
		for (int j = 0; j < this._hairstyleSwitches.Count; j++)
		{
			this._hairstyleSwitches[j].StyleSwitchPressedEvent -= this.OnHairstyleSwitchPressed;
		}
		this._hairstyleSwitches.Clear();
		this._hairstyleSwitches = null;
		for (int k = 0; k < this._outfitSwitches.Count; k++)
		{
			this._outfitSwitches[k].StyleSwitchPressedEvent -= this.OnOutfitSwitchPressed;
		}
		this._outfitSwitches.Clear();
		this._outfitSwitches = null;
		for (int l = 0; l < this._photos.Count; l++)
		{
			this._photos[l].PhotoPressedEvent -= this.OnPhotoClicked;
		}
		this._photos.Clear();
		this._photos = null;
		this._selectedHairstyleSwitch = null;
		this._selectedOutfitSwitch = null;
		this._tabContents.Clear();
		this._tabContents = null;
		this._arrowLeft.button.ButtonPressedEvent -= this.OnArrowPressed;
		this._arrowRight.button.ButtonPressedEvent -= this.OnArrowPressed;
	}

	// Token: 0x0400013E RID: 318
	public const string CELL_MEMORY_PROFILE_GIRL = "cell_memory_profile_girl";

	// Token: 0x0400013F RID: 319
	public const string CELL_MEMORY_PROFILE_TAB = "cell_memory_profile_tab";

	// Token: 0x04000140 RID: 320
	public const string PROFILE_TAB_PREFERENCES = "cell_app_profile_header_preferences";

	// Token: 0x04000141 RID: 321
	public const string PROFILE_TAB_DETAILS = "cell_app_profile_header_details";

	// Token: 0x04000142 RID: 322
	public const string PROFILE_TAB_COLLECTION = "cell_app_profile_header_collection";

	// Token: 0x04000143 RID: 323
	public const string PROFILE_TAB_STYLE = "cell_app_profile_header_style";

	// Token: 0x04000144 RID: 324
	public const string PROFILE_TAB_PHOTOS = "cell_app_profile_header_photos";

	// Token: 0x04000145 RID: 325
	private const int ITEM_COLLECTION_COUNT = 24;

	// Token: 0x04000146 RID: 326
	private const int STYLE_SWITCH_COUNT = 6;

	// Token: 0x04000147 RID: 327
	private const int PHOTO_THUMB_COUNT = 4;

	// Token: 0x04000148 RID: 328
	private const string TRAIT_ICON_SPRITE_PREFIX = "cell_app_upgrades_trait_icons_";

	// Token: 0x04000149 RID: 329
	public static readonly string[] PROFILE_TABS = new string[]
	{
		"cell_app_profile_header_preferences",
		"cell_app_profile_header_details",
		"cell_app_profile_header_collection",
		"cell_app_profile_header_style",
		"cell_app_profile_header_photos"
	};

	// Token: 0x0400014A RID: 330
	public AudioDefinition orderItemSound;

	// Token: 0x0400014B RID: 331
	public SpriteObject girlIcon;

	// Token: 0x0400014C RID: 332
	public GirlLiveStats liveStats;

	// Token: 0x0400014D RID: 333
	public SpriteObject profileTabHeader;

	// Token: 0x0400014E RID: 334
	public DisplayObject tabPreferences;

	// Token: 0x0400014F RID: 335
	public DisplayObject tabDetails;

	// Token: 0x04000150 RID: 336
	public DisplayObject tabCollection;

	// Token: 0x04000151 RID: 337
	public DisplayObject tabStyle;

	// Token: 0x04000152 RID: 338
	public DisplayObject tabPhotos;

	// Token: 0x04000153 RID: 339
	public SpriteObject mostTraitIcons;

	// Token: 0x04000154 RID: 340
	public SpriteObject leastTraitIcons;

	// Token: 0x04000155 RID: 341
	private GirlDefinition _girlDefinition;

	// Token: 0x04000156 RID: 342
	private GirlPlayerData _girlPlayerData;

	// Token: 0x04000157 RID: 343
	private int _currentProfileTab;

	// Token: 0x04000158 RID: 344
	private SpriteObject _arrowLeft;

	// Token: 0x04000159 RID: 345
	private SpriteObject _arrowRight;

	// Token: 0x0400015A RID: 346
	private Dictionary<string, DisplayObject> _tabContents;

	// Token: 0x0400015B RID: 347
	private List<GirlProfilePreference> _preferences;

	// Token: 0x0400015C RID: 348
	private List<GirlProfileDetail> _details;

	// Token: 0x0400015D RID: 349
	private List<GirlProfileCollectionSlot> _collectionSlots;

	// Token: 0x0400015E RID: 350
	private List<GirlProfileStyleSwitch> _hairstyleSwitches;

	// Token: 0x0400015F RID: 351
	private List<GirlProfileStyleSwitch> _outfitSwitches;

	// Token: 0x04000160 RID: 352
	private List<GirlProfilePhoto> _photos;

	// Token: 0x04000161 RID: 353
	private GirlProfileStyleSwitch _selectedHairstyleSwitch;

	// Token: 0x04000162 RID: 354
	private GirlProfileStyleSwitch _selectedOutfitSwitch;
}
