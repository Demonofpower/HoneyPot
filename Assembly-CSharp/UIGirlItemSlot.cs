using System;

// Token: 0x0200005B RID: 91
public class UIGirlItemSlot : DisplayObject
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060002D1 RID: 721 RVA: 0x00004585 File Offset: 0x00002785
	// (remove) Token: 0x060002D2 RID: 722 RVA: 0x0000459E File Offset: 0x0000279E
	public event UIGirlItemSlot.UIGirlItemSlotDelegate UIGirlItemSlotDownEvent;

	// Token: 0x060002D3 RID: 723 RVA: 0x0001B0A8 File Offset: 0x000192A8
	public void Init(int itemSlotIndex)
	{
		this.background = (base.GetChildByName("GirlUIItemBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("GirlUIItemIcon") as SpriteObject);
		base.button.ButtonDownEvent += this.OnButtonDown;
		this.slotIndex = itemSlotIndex;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x000045B7 File Offset: 0x000027B7
	public void PopulateSlotItem()
	{
		this.itemDefinition = GameManager.System.Player.gifts[this.slotIndex].itemDefinition;
		this.RefreshSlotItem();
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000045E0 File Offset: 0x000027E0
	public void ConsumeSlotItem()
	{
		GameManager.System.Player.gifts[this.slotIndex].itemDefinition = null;
		this.PopulateSlotItem();
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0001B100 File Offset: 0x00019300
	public void RefreshSlotItem()
	{
		if (this.itemDefinition != null)
		{
			this.itemIcon.sprite.SetSprite(this.itemDefinition.iconName);
		}
		else
		{
			this.itemIcon.sprite.SetSprite("item_blank");
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00004604 File Offset: 0x00002804
	private void OnButtonDown(ButtonObject buttonObject)
	{
		if (this.UIGirlItemSlotDownEvent != null)
		{
			this.UIGirlItemSlotDownEvent(this);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000461D File Offset: 0x0000281D
	public override bool CanShowTooltip()
	{
		return !(this.itemDefinition == null);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00004633 File Offset: 0x00002833
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonDownEvent -= this.OnButtonDown;
	}

	// Token: 0x0400029C RID: 668
	public SpriteObject background;

	// Token: 0x0400029D RID: 669
	public SpriteObject itemIcon;

	// Token: 0x0400029E RID: 670
	public int slotIndex;

	// Token: 0x0400029F RID: 671
	public ItemDefinition itemDefinition;

	// Token: 0x0200005C RID: 92
	// (Invoke) Token: 0x060002DB RID: 731
	public delegate void UIGirlItemSlotDelegate(UIGirlItemSlot itemSlot);
}
