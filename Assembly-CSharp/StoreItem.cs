using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class StoreItem : DisplayObject
{
	// Token: 0x0600024F RID: 591 RVA: 0x00017D48 File Offset: 0x00015F48
	public void Init(int slotIndex)
	{
		this.background = (base.GetChildByName("StoreItemBackground") as SpriteObject);
		this.itemInfo = base.GetChildByName("StoreItemInfo");
		this.itemNameLabel = (this.itemInfo.GetChildByName("StoreItemNameLabel") as LabelObject);
		this.itemCostLabel = (this.itemInfo.GetChildByName("StoreItemCostLabel") as LabelObject);
		this.moneyIcon = (this.itemInfo.GetChildByName("StoreItemMoneyIcon") as SpriteObject);
		this.starIcon = (this.itemInfo.GetChildByName("StoreItemStarIcon") as SpriteObject);
		this.soldoutStamp = (base.GetChildByName("StoreItemSoldout") as SpriteObject);
		this.itemSlotIndex = slotIndex;
		this.itemSlot = (base.GetChildByName("StoreItemSlot") as StoreItemSlot);
		this.itemSlot.Init(this.itemSlotIndex);
		this._origCostLabelX = this.itemCostLabel.localX - 8f;
		this._origMoneyIconX = this.moneyIcon.localX - 8f;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00017E5C File Offset: 0x0001605C
	public void PopulateItem(StoreItemPlayerData storeItemPlayerData)
	{
		this.gameObj.SetActive(true);
		this.itemSlot.PopulateItem(storeItemPlayerData.itemDefinition);
		if (!storeItemPlayerData.soldOut)
		{
			this.background.SetAlpha(1f, 0f);
			this.itemInfo.SetChildAlpha(1f, 0f);
			this.itemSlot.background.SetAlpha(1f, 0f);
			this.itemSlot.itemIcon.SetAlpha(1f, 0f);
			this.soldoutStamp.SetAlpha(0f, 0f);
			if (storeItemPlayerData.itemDefinition.hasShortName)
			{
				this.itemNameLabel.SetText(storeItemPlayerData.itemDefinition.shortName);
			}
			else
			{
				this.itemNameLabel.SetText(storeItemPlayerData.itemDefinition.name);
			}
			this.itemCostLabel.SetText(storeItemPlayerData.itemDefinition.storeCost, 0f, false, 1f);
			this.itemCostLabel.localX = this._origCostLabelX + (float)(Mathf.Min(this.itemCostLabel.label.text.Length, 4) * (10 - Mathf.Min(this.itemCostLabel.label.text.Length, 4)));
			this.moneyIcon.localX = this._origMoneyIconX - (float)(Mathf.Min(this.itemCostLabel.label.text.Length, 4) * (10 - Mathf.Min(this.itemCostLabel.label.text.Length, 4)));
			if (GameManager.System.Player.tutorialComplete && GameManager.System.Location.currentGirl != null && GameManager.System.Location.currentGirl.WillAcceptItem(storeItemPlayerData.itemDefinition))
			{
				this.starIcon.SetAlpha(1f, 0f);
				if (storeItemPlayerData.itemDefinition.type == ItemType.GIFT && storeItemPlayerData.itemDefinition.giftType == GameManager.System.Location.currentGirl.lovesGiftType)
				{
					this.starIcon.sprite.SetSprite("cell_app_shop_heart");
				}
				else
				{
					this.starIcon.sprite.SetSprite("cell_app_shop_star");
				}
			}
			else
			{
				this.starIcon.SetAlpha(0f, 0f);
			}
			this.itemSlot.tooltip.Enable();
			if ((!GameManager.System.Player.IsInventoryFull(GameManager.System.Player.gifts) || !GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory)) && storeItemPlayerData.itemDefinition.storeCost <= GameManager.System.Player.money)
			{
				this.itemSlot.button.Enable();
			}
			else
			{
				this.itemSlot.button.Disable();
			}
		}
		else
		{
			this.background.SetAlpha(0.5f, 0f);
			this.itemInfo.SetChildAlpha(0f, 0f);
			this.itemSlot.background.SetAlpha(0.5f, 0f);
			this.itemSlot.itemIcon.SetAlpha(0.15f, 0f);
			this.soldoutStamp.SetAlpha(0.32f, 0f);
			this.itemSlot.tooltip.Disable();
			this.itemSlot.button.Disable();
			if (storeItemPlayerData.itemDefinition == null)
			{
				this.gameObj.SetActive(false);
			}
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x040001F4 RID: 500
	public SpriteObject background;

	// Token: 0x040001F5 RID: 501
	public DisplayObject itemInfo;

	// Token: 0x040001F6 RID: 502
	public LabelObject itemNameLabel;

	// Token: 0x040001F7 RID: 503
	public LabelObject itemCostLabel;

	// Token: 0x040001F8 RID: 504
	public SpriteObject moneyIcon;

	// Token: 0x040001F9 RID: 505
	public SpriteObject starIcon;

	// Token: 0x040001FA RID: 506
	public SpriteObject soldoutStamp;

	// Token: 0x040001FB RID: 507
	public int itemSlotIndex;

	// Token: 0x040001FC RID: 508
	public StoreItemSlot itemSlot;

	// Token: 0x040001FD RID: 509
	private float _origCostLabelX;

	// Token: 0x040001FE RID: 510
	private float _origMoneyIconX;
}
