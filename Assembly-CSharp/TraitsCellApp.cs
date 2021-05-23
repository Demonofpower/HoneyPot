using System;
using System.Collections.Generic;

// Token: 0x0200004B RID: 75
public class TraitsCellApp : UICellApp
{
	// Token: 0x0600025F RID: 607 RVA: 0x000182E8 File Offset: 0x000164E8
	public override void Init()
	{
		this.traitsItemsContainer = base.GetChildByName("TraitsItemsContainer");
		this._traitsItems = new List<TraitsItem>();
		for (int i = 0; i < Enum.GetNames(typeof(PlayerTraitType)).Length; i++)
		{
			TraitsItem traitsItem = this.traitsItemsContainer.GetChildByName("TraitsItem" + i.ToString()) as TraitsItem;
			traitsItem.Init(i);
			traitsItem.itemSlot.TraitsItemSlotPressedEvent += this.OnStoreItemSlotPressed;
			this._traitsItems.Add(traitsItem);
		}
		this.Refresh();
		if (GameManager.System.GameState != GameState.SIM)
		{
			for (int j = 0; j < this._traitsItems.Count; j++)
			{
				this._traitsItems[j].itemSlot.button.Disable();
				this._traitsItems[j].itemSlot.tooltip.Disable();
			}
			GameManager.Stage.cellPhone.ShowCellAppError("Not while on a date!", true, 0f);
		}
		base.Init();
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00018408 File Offset: 0x00016608
	public override void Refresh()
	{
		for (int i = 0; i < this._traitsItems.Count; i++)
		{
			this._traitsItems[i].Refresh();
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00018444 File Offset: 0x00016644
	private void OnStoreItemSlotPressed(TraitsItemSlot traitsItemSlot)
	{
		if (GameManager.System.Player.GetTraitLevel(traitsItemSlot.playerTraitType) < 6 && traitsItemSlot.itemDefinition.storeCost <= GameManager.System.Player.hunie)
		{
			GameManager.System.Player.UpgradeTraitLevel(traitsItemSlot.playerTraitType);
			GameManager.System.Player.hunie -= traitsItemSlot.itemDefinition.storeCost;
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.upgradeTraitSound, false, 1f, true);
		}
		this.Refresh();
		traitsItemSlot.tooltip.Disable();
		traitsItemSlot.tooltip.Enable();
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000184FC File Offset: 0x000166FC
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._traitsItems.Count; i++)
		{
			this._traitsItems[i].itemSlot.TraitsItemSlotPressedEvent -= this.OnStoreItemSlotPressed;
		}
		this._traitsItems.Clear();
		this._traitsItems = null;
	}

	// Token: 0x04000204 RID: 516
	public AudioDefinition upgradeTraitSound;

	// Token: 0x04000205 RID: 517
	public DisplayObject traitsItemsContainer;

	// Token: 0x04000206 RID: 518
	private List<TraitsItem> _traitsItems;
}
