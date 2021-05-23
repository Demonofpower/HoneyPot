using System;

// Token: 0x02000049 RID: 73
public class StoreItemSlot : DisplayObject
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000253 RID: 595 RVA: 0x00004111 File Offset: 0x00002311
	// (remove) Token: 0x06000254 RID: 596 RVA: 0x0000412A File Offset: 0x0000232A
	public event StoreItemSlot.StoreItemSlotDelegate StoreItemSlotPressedEvent;

	// Token: 0x06000255 RID: 597 RVA: 0x00018234 File Offset: 0x00016434
	public void Init(int slotIndex)
	{
		this.background = (base.GetChildByName("StoreItemSlotBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("StoreItemSlotItemIcon") as SpriteObject);
		this.itemSlotIndex = slotIndex;
		base.button.ButtonPressedEvent += this.OnButtonPressed;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0001828C File Offset: 0x0001648C
	public void PopulateItem(ItemDefinition item)
	{
		this.itemDefinition = item;
		if (this.itemDefinition != null)
		{
			this.itemIcon.sprite.SetSprite(this.itemDefinition.iconName);
		}
		else
		{
			this.itemIcon.sprite.SetSprite("item_blank");
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00004143 File Offset: 0x00002343
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.StoreItemSlotPressedEvent != null)
		{
			this.StoreItemSlotPressedEvent(this);
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000415C File Offset: 0x0000235C
	public override bool CanShowTooltip()
	{
		return !(this.itemDefinition == null);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00004172 File Offset: 0x00002372
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x040001FF RID: 511
	public SpriteObject background;

	// Token: 0x04000200 RID: 512
	public SpriteObject itemIcon;

	// Token: 0x04000201 RID: 513
	public int itemSlotIndex;

	// Token: 0x04000202 RID: 514
	public ItemDefinition itemDefinition;

	// Token: 0x0200004A RID: 74
	// (Invoke) Token: 0x0600025B RID: 603
	public delegate void StoreItemSlotDelegate(StoreItemSlot storeItemSlot);
}
