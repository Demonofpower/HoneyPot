using System;

// Token: 0x0200012B RID: 299
public class InventoryItemPlayerData
{
	// Token: 0x060006DF RID: 1759 RVA: 0x000074FB File Offset: 0x000056FB
	public InventoryItemPlayerData(InventoryItemSaveData inventoryItemSaveData)
	{
		this.ReadSaveData(inventoryItemSaveData);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00034C48 File Offset: 0x00032E48
	public void ReadSaveData(InventoryItemSaveData inventoryItemSaveData)
	{
		this.itemDefinition = GameManager.Data.Items.Get(inventoryItemSaveData.itemId);
		if (this.itemDefinition != null)
		{
			this.presentDefinition = GameManager.Data.Items.Get(inventoryItemSaveData.presentId);
		}
		else
		{
			this.presentDefinition = null;
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00034CA8 File Offset: 0x00032EA8
	public void WriteSaveData(InventoryItemSaveData inventoryItemSaveData)
	{
		if (this.itemDefinition != null)
		{
			inventoryItemSaveData.itemId = this.itemDefinition.id;
		}
		else
		{
			inventoryItemSaveData.itemId = 0;
		}
		if (this.presentDefinition != null && this.itemDefinition != null)
		{
			inventoryItemSaveData.presentId = this.presentDefinition.id;
		}
		else
		{
			inventoryItemSaveData.presentId = 0;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0000750A File Offset: 0x0000570A
	// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00007512 File Offset: 0x00005712
	public ItemDefinition itemDefinition
	{
		get
		{
			return this._itemDefinition;
		}
		set
		{
			this._itemDefinition = value;
			this._presentDefinition = null;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00007522 File Offset: 0x00005722
	// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0000752A File Offset: 0x0000572A
	public ItemDefinition presentDefinition
	{
		get
		{
			return this._presentDefinition;
		}
		set
		{
			this._presentDefinition = value;
		}
	}

	// Token: 0x04000817 RID: 2071
	private ItemDefinition _itemDefinition;

	// Token: 0x04000818 RID: 2072
	private ItemDefinition _presentDefinition;
}
