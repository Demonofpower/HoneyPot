using System;

// Token: 0x0200002F RID: 47
public class GirlProfileCollectionSlot : DisplayObject
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060001C1 RID: 449 RVA: 0x00003BCB File Offset: 0x00001DCB
	// (remove) Token: 0x060001C2 RID: 450 RVA: 0x00003BE4 File Offset: 0x00001DE4
	public event GirlProfileCollectionSlot.GirlProfileCollectionSlotDelegate CollectionSlotPressedEvent;

	// Token: 0x060001C3 RID: 451 RVA: 0x00014698 File Offset: 0x00012898
	public void Init(GirlDefinition girlDefinition, GirlPlayerData girlPlayerData, int index)
	{
		this.itemIcon = (base.GetChildByName("GirlProfileCollectionSlotItemIcon") as SpriteObject);
		this.slotIndex = index;
		this.itemDefinition = girlDefinition.collection[this.slotIndex];
		base.button.ButtonPressedEvent += this.OnButtonPressed;
		this.itemIcon.sprite.SetSprite(this.itemDefinition.iconName);
		if (girlPlayerData.IsItemInCollection(this.itemDefinition))
		{
			this._showing = true;
		}
		else
		{
			this.itemIcon.SetLightness(0f, 0f);
			this.itemIcon.SetAlpha(0.25f, 0f);
			this._showing = false;
		}
		this.Refresh();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00014760 File Offset: 0x00012960
	public void Refresh()
	{
		if (this.itemDefinition != null && this.itemDefinition.type == ItemType.GIFT && GameManager.System.GameState == GameState.SIM && GameManager.System.Player.endingSceneShown && !GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory) && GameManager.System.Player.hunie >= 10000)
		{
			base.button.Enable();
		}
		else
		{
			base.button.Disable();
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00003BFD File Offset: 0x00001DFD
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.CollectionSlotPressedEvent != null)
		{
			this.CollectionSlotPressedEvent(this);
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00003C16 File Offset: 0x00001E16
	public override bool CanShowTooltip()
	{
		return this._showing || GameManager.System.Player.endingSceneShown;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00003C3A File Offset: 0x00001E3A
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x04000163 RID: 355
	public SpriteObject itemIcon;

	// Token: 0x04000164 RID: 356
	public int slotIndex;

	// Token: 0x04000165 RID: 357
	public ItemDefinition itemDefinition;

	// Token: 0x04000166 RID: 358
	public bool _showing;

	// Token: 0x02000030 RID: 48
	// (Invoke) Token: 0x060001C9 RID: 457
	public delegate void GirlProfileCollectionSlotDelegate(GirlProfileCollectionSlot collectionSlot);
}
