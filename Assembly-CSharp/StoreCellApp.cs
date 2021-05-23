using System;
using System.Collections.Generic;

// Token: 0x02000047 RID: 71
public class StoreCellApp : UICellApp
{
	// Token: 0x06000247 RID: 583 RVA: 0x000175C0 File Offset: 0x000157C0
	public override void Init()
	{
		this.storeTabHeader = (base.GetChildByName("StoreHeader") as SpriteObject);
		this.storeItemsContainer = base.GetChildByName("StoreItemsContainer");
		this._currentStoreTab = 0;
		if (GameManager.Stage.cellPhone.cellMemory.ContainsKey("cell_memory_store_tab"))
		{
			this._currentStoreTab = GameManager.Stage.cellPhone.cellMemory["cell_memory_store_tab"];
		}
		else
		{
			GameManager.Stage.cellPhone.cellMemory.Add("cell_memory_store_tab", this._currentStoreTab);
		}
		this._tabButtons = new List<SpriteObject>();
		for (int i = 0; i < StoreCellApp.STORE_TABS.Length; i++)
		{
			SpriteObject spriteObject = base.GetChildByName("StoreTabButton" + i.ToString()) as SpriteObject;
			spriteObject.button.ButtonPressedEvent += this.OnTabButtonPressed;
			this._tabButtons.Add(spriteObject);
		}
		this._arrowLeft = (base.GetChildByName("StoreArrowLeft") as SpriteObject);
		this._arrowLeft.button.ButtonPressedEvent += this.OnArrowPressed;
		this._arrowRight = (base.GetChildByName("StoreArrowRight") as SpriteObject);
		this._arrowRight.button.ButtonPressedEvent += this.OnArrowPressed;
		this._storeItems = new List<StoreItem>();
		for (int j = 0; j < 12; j++)
		{
			StoreItem storeItem = this.storeItemsContainer.GetChildByName("StoreItem" + j.ToString()) as StoreItem;
			storeItem.Init(j);
			storeItem.itemSlot.StoreItemSlotPressedEvent += this.OnStoreItemSlotPressed;
			this._storeItems.Add(storeItem);
		}
		this._origItemsContainerY = this.storeItemsContainer.localY;
		this.Refresh();
		if (GameManager.System.GameState != GameState.SIM)
		{
			for (int k = 0; k < this._tabButtons.Count; k++)
			{
				this._tabButtons[k].button.Disable();
			}
			this._arrowLeft.button.Disable();
			this._arrowRight.button.Disable();
			for (int l = 0; l < this._storeItems.Count; l++)
			{
				this._storeItems[l].itemSlot.button.Disable();
				this._storeItems[l].itemSlot.tooltip.Disable();
			}
			GameManager.Stage.cellPhone.ShowCellAppError("Not while on a date!", true, 0f);
		}
		base.Init();
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00017884 File Offset: 0x00015A84
	public override void Refresh()
	{
		for (int i = 0; i < this._tabButtons.Count; i++)
		{
			this._tabButtons[i].button.Enable();
		}
		this._tabButtons[this._currentStoreTab].button.Disable();
		this.storeTabHeader.sprite.SetSprite(StoreCellApp.STORE_TABS[this._currentStoreTab]);
		StoreItemPlayerData[] currentStoreList = this.GetCurrentStoreList(StoreCellApp.STORE_TABS[this._currentStoreTab]);
		for (int j = 0; j < currentStoreList.Length; j++)
		{
			this._storeItems[j].PopulateItem(currentStoreList[j]);
		}
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00017938 File Offset: 0x00015B38
	private StoreItemPlayerData[] GetCurrentStoreList(string storeTab)
	{
		switch (storeTab)
		{
		case "cell_app_shop_header_food":
			return GameManager.System.Player.storeFood;
		case "cell_app_shop_header_unique":
			return GameManager.System.Player.storeUnique;
		case "cell_app_shop_header_drinks":
			return GameManager.System.Player.storeDrinks;
		}
		return GameManager.System.Player.storeGifts;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000179F0 File Offset: 0x00015BF0
	private void OnStoreItemSlotPressed(StoreItemSlot storeItemSlot)
	{
		StoreItemPlayerData storeItemPlayerData = this.GetCurrentStoreList(StoreCellApp.STORE_TABS[this._currentStoreTab])[storeItemSlot.itemSlotIndex];
		if ((!GameManager.System.Player.IsInventoryFull(GameManager.System.Player.gifts) || !GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory)) && storeItemPlayerData.itemDefinition.storeCost <= GameManager.System.Player.money && !storeItemPlayerData.soldOut)
		{
			if (GameManager.System.Player.IsInventoryFull(GameManager.System.Player.gifts))
			{
				GameManager.System.Player.AddItem(storeItemPlayerData.itemDefinition, GameManager.System.Player.inventory, false, false);
			}
			else
			{
				GameManager.System.Player.AddItem(storeItemPlayerData.itemDefinition, GameManager.System.Player.gifts, false, false);
			}
			storeItemPlayerData.soldOut = true;
			GameManager.System.Player.money -= storeItemPlayerData.itemDefinition.storeCost;
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.purchaseItemSound, false, 1f, true);
		}
		this.Refresh();
		GameManager.Stage.uiGirl.RefreshItemSlots();
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00017B58 File Offset: 0x00015D58
	private void OnTabButtonPressed(ButtonObject buttonObject)
	{
		SpriteObject item = buttonObject.GetDisplayObject() as SpriteObject;
		this._currentStoreTab = this._tabButtons.IndexOf(item);
		GameManager.Stage.cellPhone.cellMemory["cell_memory_store_tab"] = this._currentStoreTab;
		this.Refresh();
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00017BA8 File Offset: 0x00015DA8
	private void OnArrowPressed(ButtonObject buttonObject)
	{
		SpriteObject x = buttonObject.GetDisplayObject() as SpriteObject;
		if (x == this._arrowLeft)
		{
			if (this._currentStoreTab - 1 < 0)
			{
				this._currentStoreTab = StoreCellApp.STORE_TABS.Length - 1;
			}
			else
			{
				this._currentStoreTab--;
			}
		}
		else if (StoreCellApp.STORE_TABS.Length > this._currentStoreTab + 1)
		{
			this._currentStoreTab++;
		}
		else
		{
			this._currentStoreTab = 0;
		}
		GameManager.Stage.cellPhone.cellMemory["cell_memory_store_tab"] = this._currentStoreTab;
		this.Refresh();
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00017C5C File Offset: 0x00015E5C
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._tabButtons.Count; i++)
		{
			this._tabButtons[i].button.ButtonPressedEvent -= this.OnTabButtonPressed;
		}
		this._tabButtons.Clear();
		this._tabButtons = null;
		this._arrowLeft.button.ButtonPressedEvent -= this.OnArrowPressed;
		this._arrowRight.button.ButtonPressedEvent -= this.OnArrowPressed;
		for (int j = 0; j < this._storeItems.Count; j++)
		{
			this._storeItems[j].itemSlot.StoreItemSlotPressedEvent -= this.OnStoreItemSlotPressed;
		}
		this._storeItems.Clear();
		this._storeItems = null;
	}

	// Token: 0x040001E4 RID: 484
	public const string CELL_MEMORY_STORE_TAB = "cell_memory_store_tab";

	// Token: 0x040001E5 RID: 485
	public const string STORE_TAB_GIFTS = "cell_app_shop_header_gifts";

	// Token: 0x040001E6 RID: 486
	public const string STORE_TAB_UNIQUE = "cell_app_shop_header_unique";

	// Token: 0x040001E7 RID: 487
	public const string STORE_TAB_FOOD = "cell_app_shop_header_food";

	// Token: 0x040001E8 RID: 488
	public const string STORE_TAB_DRINKS = "cell_app_shop_header_drinks";

	// Token: 0x040001E9 RID: 489
	public static readonly string[] STORE_TABS = new string[]
	{
		"cell_app_shop_header_gifts",
		"cell_app_shop_header_unique",
		"cell_app_shop_header_food",
		"cell_app_shop_header_drinks"
	};

	// Token: 0x040001EA RID: 490
	public AudioDefinition purchaseItemSound;

	// Token: 0x040001EB RID: 491
	public SpriteObject storeTabHeader;

	// Token: 0x040001EC RID: 492
	public DisplayObject storeItemsContainer;

	// Token: 0x040001ED RID: 493
	private int _currentStoreTab;

	// Token: 0x040001EE RID: 494
	private List<SpriteObject> _tabButtons;

	// Token: 0x040001EF RID: 495
	private SpriteObject _arrowLeft;

	// Token: 0x040001F0 RID: 496
	private SpriteObject _arrowRight;

	// Token: 0x040001F1 RID: 497
	private List<StoreItem> _storeItems;

	// Token: 0x040001F2 RID: 498
	private float _origItemsContainerY;
}
