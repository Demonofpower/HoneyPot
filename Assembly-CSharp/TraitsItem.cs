using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class TraitsItem : DisplayObject
{
	// Token: 0x06000264 RID: 612 RVA: 0x00018560 File Offset: 0x00016760
	public void Init(int playerTraitIndex)
	{
		this.background = (base.GetChildByName("TraitsItemBackground") as SpriteObject);
		this.itemInfo = base.GetChildByName("TraitsItemInfo");
		this.itemCostLabel = (this.itemInfo.GetChildByName("TraitsItemCostLabel") as LabelObject);
		this.hunieIcon = (this.itemInfo.GetChildByName("TraitsItemHunieIcon") as SpriteObject);
		this.traitMeterContainer = base.GetChildByName("TraitsItemMeterContainer");
		this._playerTraitType = (PlayerTraitType)playerTraitIndex;
		this.itemSlot = (base.GetChildByName("TraitsItemSlot") as TraitsItemSlot);
		this.itemSlot.Init(this._playerTraitType);
		this._traitMeter = new List<SpriteObject>();
		for (int i = 0; i < 6; i++)
		{
			this._traitMeter.Add(this.traitMeterContainer.GetChildByName("TraitsItemMeter" + i.ToString()) as SpriteObject);
		}
		this._origCostLabelX = this.itemCostLabel.localX - 8f;
		this._origHunieIconX = this.hunieIcon.localX - 8f;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00018680 File Offset: 0x00016880
	public void Refresh()
	{
		int traitLevel = GameManager.System.Player.GetTraitLevel(this._playerTraitType);
		ItemDefinition itemDefinition = this.traitLevelItems[Mathf.Clamp(traitLevel, 0, this.traitLevelItems.Count - 1)];
		this.itemSlot.PopulateItem(itemDefinition);
		if (traitLevel < 6)
		{
			this.hunieIcon.SetAlpha(1f, 0f);
			this.itemCostLabel.SetText(StringUtils.FormatIntAsCurrency(itemDefinition.storeCost, false));
			this.itemCostLabel.localX = this._origCostLabelX + (float)(Mathf.Min(this.itemCostLabel.label.text.Length, 5) * (9 - Mathf.Min(this.itemCostLabel.label.text.Length, 5)));
			this.hunieIcon.localX = this._origHunieIconX - (float)(Mathf.Min(this.itemCostLabel.label.text.Length, 5) * (9 - Mathf.Min(this.itemCostLabel.label.text.Length, 4)));
			if (itemDefinition.storeCost <= GameManager.System.Player.hunie)
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
			this.hunieIcon.SetAlpha(0f, 0f);
			this.itemCostLabel.SetText("MAXED");
			this.itemCostLabel.localX = this._origCostLabelX + 8f;
			this.itemSlot.button.Disable();
		}
		for (int i = 0; i < this._traitMeter.Count; i++)
		{
			if (i < traitLevel)
			{
				this._traitMeter[i].SetAlpha(1f, 0f);
			}
			else
			{
				this._traitMeter[i].SetAlpha(0f, 0f);
			}
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x04000207 RID: 519
	public List<ItemDefinition> traitLevelItems;

	// Token: 0x04000208 RID: 520
	public SpriteObject background;

	// Token: 0x04000209 RID: 521
	public DisplayObject itemInfo;

	// Token: 0x0400020A RID: 522
	public LabelObject itemCostLabel;

	// Token: 0x0400020B RID: 523
	public SpriteObject hunieIcon;

	// Token: 0x0400020C RID: 524
	public DisplayObject traitMeterContainer;

	// Token: 0x0400020D RID: 525
	public TraitsItemSlot itemSlot;

	// Token: 0x0400020E RID: 526
	private PlayerTraitType _playerTraitType;

	// Token: 0x0400020F RID: 527
	private List<SpriteObject> _traitMeter;

	// Token: 0x04000210 RID: 528
	private float _origCostLabelX;

	// Token: 0x04000211 RID: 529
	private float _origHunieIconX;
}
