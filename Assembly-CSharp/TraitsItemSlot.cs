using System;

// Token: 0x0200004D RID: 77
public class TraitsItemSlot : DisplayObject
{
	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000268 RID: 616 RVA: 0x00004191 File Offset: 0x00002391
	// (remove) Token: 0x06000269 RID: 617 RVA: 0x000041AA File Offset: 0x000023AA
	public event TraitsItemSlot.TraitsItemSlotDelegate TraitsItemSlotPressedEvent;

	// Token: 0x0600026A RID: 618 RVA: 0x0001888C File Offset: 0x00016A8C
	public void Init(PlayerTraitType traitIndex)
	{
		this.background = (base.GetChildByName("TraitsItemSlotBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("TraitsItemSlotItemIcon") as SpriteObject);
		this.playerTraitType = traitIndex;
		base.button.ButtonPressedEvent += this.OnButtonPressed;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x000188E4 File Offset: 0x00016AE4
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

	// Token: 0x0600026C RID: 620 RVA: 0x000041C3 File Offset: 0x000023C3
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.TraitsItemSlotPressedEvent != null)
		{
			this.TraitsItemSlotPressedEvent(this);
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x000041DC File Offset: 0x000023DC
	public override bool CanShowTooltip()
	{
		return !(this.itemDefinition == null);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x000041F2 File Offset: 0x000023F2
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x04000212 RID: 530
	public SpriteObject background;

	// Token: 0x04000213 RID: 531
	public SpriteObject itemIcon;

	// Token: 0x04000214 RID: 532
	public PlayerTraitType playerTraitType;

	// Token: 0x04000215 RID: 533
	public ItemDefinition itemDefinition;

	// Token: 0x0200004E RID: 78
	// (Invoke) Token: 0x06000270 RID: 624
	public delegate void TraitsItemSlotDelegate(TraitsItemSlot traitsItemSlot);
}
