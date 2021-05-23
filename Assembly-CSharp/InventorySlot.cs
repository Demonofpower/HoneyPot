using System;
using System.Collections.Generic;

// Token: 0x0200003B RID: 59
public class InventorySlot : DisplayObject
{
	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060001F9 RID: 505 RVA: 0x00003D8A File Offset: 0x00001F8A
	// (remove) Token: 0x060001FA RID: 506 RVA: 0x00003DA3 File Offset: 0x00001FA3
	public event InventorySlot.InventorySlotDelegate InventorySlotDownEvent;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x060001FB RID: 507 RVA: 0x00003DBC File Offset: 0x00001FBC
	// (remove) Token: 0x060001FC RID: 508 RVA: 0x00003DD5 File Offset: 0x00001FD5
	public event InventorySlot.InventorySlotDelegate InventorySlotPressedEvent;

	// Token: 0x060001FD RID: 509 RVA: 0x00015C80 File Offset: 0x00013E80
	public void Init(int invSlotIndex)
	{
		this.background = (base.GetChildByName("InventorySlotBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("InventorySlotItemIcon") as SpriteObject);
		this.typeIcon = (base.GetChildByName("InventorySlotItemTypeIcon") as SpriteObject);
		base.button.ButtonDownEvent += this.OnButtonDown;
		base.button.ButtonPressedEvent += this.OnButtonPressed;
		this.slotIndex = invSlotIndex;
		this.PopulateSlotItem();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00015D0C File Offset: 0x00013F0C
	public void PopulateSlotItem()
	{
		InventoryItemPlayerData inventoryItemPlayerData = this.GetInventoryList()[this.slotIndex];
		if (inventoryItemPlayerData.presentDefinition != null)
		{
			this.itemDefinition = inventoryItemPlayerData.presentDefinition;
		}
		else
		{
			this.itemDefinition = inventoryItemPlayerData.itemDefinition;
		}
		if (this.itemDefinition != null)
		{
			this.itemIcon.sprite.SetSprite(this.itemDefinition.iconName);
		}
		else
		{
			this.itemIcon.sprite.SetSprite("item_blank");
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00003DEE File Offset: 0x00001FEE
	public void ReplaceSlotItem(ItemDefinition newSlotItem)
	{
		this.GetInventoryList()[this.slotIndex].itemDefinition = newSlotItem;
		this.PopulateSlotItem();
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00015DA0 File Offset: 0x00013FA0
	private InventoryItemPlayerData[] GetInventoryList()
	{
		if (!this.isEquipmentSlot)
		{
			return GameManager.System.Player.inventory;
		}
		string text = InventoryCellApp.EQUIPMENT_TABS[GameManager.Stage.cellPhone.cellMemory["cell_memory_equipment_tab"]];
		string text2 = text;
		if (text2 != null)
		{
			if (InventorySlot.__buildDictionary == null)
			{
				InventorySlot.__buildDictionary = new Dictionary<string, int>(1)
				{
					{
						"cell_app_inventory_header_gifts",
						0
					}
				};
			}
			int num;
			if (InventorySlot.__buildDictionary.TryGetValue(text2, out num))
			{
				if (num == 0)
				{
					return GameManager.System.Player.gifts;
				}
			}
		}
		return GameManager.System.Player.dateGifts;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00015E50 File Offset: 0x00014050
	public bool CanAcceptItem(InventorySlot fromSlot)
	{
		string text = InventoryCellApp.EQUIPMENT_TABS[GameManager.Stage.cellPhone.cellMemory["cell_memory_equipment_tab"]];
		if (this.itemDefinition != null && this.itemDefinition.type == ItemType.PRESENT)
		{
			return false;
		}
		if (this.isEquipmentSlot)
		{
			string text2 = text;
			if (text2 != null)
			{
				if (InventorySlot.__buildDictionary == null)
				{
					InventorySlot.__buildDictionary = new Dictionary<string, int>(1)
					{
						{
							"cell_app_inventory_header_gifts",
							0
						}
					};
				}
				int num;
				if (InventorySlot.__buildDictionary.TryGetValue(text2, out num))
				{
					if (num == 0)
					{
						return fromSlot.itemDefinition.type == ItemType.FOOD || fromSlot.itemDefinition.type == ItemType.DRINK || fromSlot.itemDefinition.type == ItemType.GIFT || fromSlot.itemDefinition.type == ItemType.UNIQUE_GIFT || fromSlot.itemDefinition.type == ItemType.PANTIES || fromSlot.itemDefinition.type == ItemType.MISC;
					}
				}
			}
			return fromSlot.itemDefinition.type == ItemType.DATE_GIFT;
		}
		if (fromSlot.isEquipmentSlot && this.itemDefinition != null)
		{
			string text2 = text;
			if (text2 != null)
			{
				if (InventorySlot.__buildDictionary == null)
				{
					InventorySlot.__buildDictionary = new Dictionary<string, int>(1)
					{
						{
							"cell_app_inventory_header_gifts",
							0
						}
					};
				}
				int num;
				if (InventorySlot.__buildDictionary.TryGetValue(text2, out num))
				{
					if (num == 0)
					{
						return this.itemDefinition.type == ItemType.FOOD || this.itemDefinition.type == ItemType.DRINK || this.itemDefinition.type == ItemType.GIFT || this.itemDefinition.type == ItemType.UNIQUE_GIFT || this.itemDefinition.type == ItemType.PANTIES || this.itemDefinition.type == ItemType.MISC;
					}
				}
			}
			return this.itemDefinition.type == ItemType.DATE_GIFT;
		}
		return true;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00003E09 File Offset: 0x00002009
	public void UnwrapItem()
	{
		this.GetInventoryList()[this.slotIndex].presentDefinition = null;
		this.PopulateSlotItem();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00003E24 File Offset: 0x00002024
	private void OnButtonDown(ButtonObject buttonObject)
	{
		if (this.InventorySlotDownEvent != null)
		{
			this.InventorySlotDownEvent(this);
		}
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00003E3D File Offset: 0x0000203D
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.itemDefinition != null && this.itemDefinition.type == ItemType.PRESENT && this.InventorySlotPressedEvent != null)
		{
			this.InventorySlotPressedEvent(this);
		}
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00003E78 File Offset: 0x00002078
	public override bool CanShowTooltip()
	{
		return !(this.itemDefinition == null);
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00003E8E File Offset: 0x0000208E
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonDownEvent -= this.OnButtonDown;
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x0400019C RID: 412
	public bool isEquipmentSlot;

	// Token: 0x0400019D RID: 413
	public SpriteObject background;

	// Token: 0x0400019E RID: 414
	public SpriteObject itemIcon;

	// Token: 0x0400019F RID: 415
	public SpriteObject typeIcon;

	// Token: 0x040001A0 RID: 416
	public int slotIndex;

	// Token: 0x040001A1 RID: 417
	public ItemDefinition itemDefinition;

	// Token: 0x0200003C RID: 60
	// (Invoke) Token: 0x06000208 RID: 520
	public delegate void InventorySlotDelegate(InventorySlot inventorySlot);
    
	public static Dictionary<string, int> __buildDictionary;
}
