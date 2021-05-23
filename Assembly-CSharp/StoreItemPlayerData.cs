using System;

// Token: 0x02000130 RID: 304
public class StoreItemPlayerData
{
	// Token: 0x0600073E RID: 1854 RVA: 0x000077FC File Offset: 0x000059FC
	public StoreItemPlayerData(StoreItemSaveData storeItemSaveData)
	{
		this.ReadSaveData(storeItemSaveData);
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0003650C File Offset: 0x0003470C
	public void ReadSaveData(StoreItemSaveData storeItemSaveData)
	{
		if (storeItemSaveData.itemId > 0)
		{
			this.itemDefinition = GameManager.Data.Items.Get(storeItemSaveData.itemId);
		}
		else
		{
			this.itemDefinition = null;
		}
		this.soldOut = storeItemSaveData.soldOut;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0000780B File Offset: 0x00005A0B
	public void WriteSaveData(StoreItemSaveData storeItemSaveData)
	{
		if (this.itemDefinition != null)
		{
			storeItemSaveData.itemId = this.itemDefinition.id;
		}
		else
		{
			storeItemSaveData.itemId = -1;
		}
		storeItemSaveData.soldOut = this.soldOut;
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00007847 File Offset: 0x00005A47
	// (set) Token: 0x06000742 RID: 1858 RVA: 0x0000784F File Offset: 0x00005A4F
	public ItemDefinition itemDefinition
	{
		get
		{
			return this._itemDefinition;
		}
		set
		{
			this._itemDefinition = value;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x00007858 File Offset: 0x00005A58
	// (set) Token: 0x06000744 RID: 1860 RVA: 0x00007860 File Offset: 0x00005A60
	public bool soldOut
	{
		get
		{
			return this._soldOut;
		}
		set
		{
			this._soldOut = value;
		}
	}

	// Token: 0x0400084B RID: 2123
	private ItemDefinition _itemDefinition;

	// Token: 0x0400084C RID: 2124
	private bool _soldOut;
}
