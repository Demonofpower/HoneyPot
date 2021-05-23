using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class ItemTooltipContent : UITooltipContent
{
	// Token: 0x06000387 RID: 903 RVA: 0x0001FCFC File Offset: 0x0001DEFC
	public override void Init(TooltipObject tooltipObject)
	{
		base.Init(tooltipObject);
		this.nameLabel = (base.GetChildByName("TooltipItemName") as LabelObject);
		this.typeLabel = (base.GetChildByName("TooltipItemType") as LabelObject);
		this.descriptionLabel = (base.GetChildByName("TooltipItemDescription") as LabelObject);
		this.valueLabel = (base.GetChildByName("TooltipItemValue") as LabelObject);
		this._valueLabelOrigY = this.valueLabel.localY;
		this._hasValue = true;
		this._puzzleStatusItem = false;
		this._storeItem = false;
		this._traitsItem = false;
		this._collectionItem = false;
		this._itemDefinition = null;
		if (this._itemDefinition == null)
		{
			InventorySlot component = this._tooltipSource.gameObj.GetComponent<InventorySlot>();
			if (component != null)
			{
				this._itemDefinition = component.itemDefinition;
			}
		}
		if (this._itemDefinition == null)
		{
			StoreItemSlot component2 = this._tooltipSource.gameObj.GetComponent<StoreItemSlot>();
			if (component2 != null)
			{
				this._itemDefinition = component2.itemDefinition;
				this._storeItem = true;
			}
		}
		if (this._itemDefinition == null)
		{
			TraitsItemSlot component3 = this._tooltipSource.gameObj.GetComponent<TraitsItemSlot>();
			if (component3 != null)
			{
				this._itemDefinition = component3.itemDefinition;
				this._traitsItem = true;
			}
		}
		if (this._itemDefinition == null)
		{
			GirlProfileCollectionSlot component4 = this._tooltipSource.gameObj.GetComponent<GirlProfileCollectionSlot>();
			if (component4 != null)
			{
				this._itemDefinition = component4.itemDefinition;
				this._collectionItem = true;
			}
		}
		if (this._itemDefinition == null)
		{
			UIGirlItemSlot component5 = this._tooltipSource.gameObj.GetComponent<UIGirlItemSlot>();
			if (component5 != null)
			{
				this._itemDefinition = component5.itemDefinition;
			}
		}
		if (this._itemDefinition == null)
		{
			PuzzleStatusItemSlot component6 = this._tooltipSource.gameObj.GetComponent<PuzzleStatusItemSlot>();
			if (component6 != null)
			{
				this._itemDefinition = component6.itemDefinition;
				this._puzzleStatusItem = true;
			}
		}
		this.Refresh();
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001FF28 File Offset: 0x0001E128
	public override void Refresh()
	{
		if (this._itemDefinition == null)
		{
			return;
		}
		this.nameLabel.SetText(this._itemDefinition.name);
		this.descriptionLabel.SetText(StringUtils.FlattenColorBunches(this._itemDefinition.description, "545454"));
		if (this._itemDefinition.type == ItemType.GIFT && this._itemDefinition.id == 106 && GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.catGirlDef).metStatus == GirlMetStatus.LOCKED)
		{
			this.descriptionLabel.SetText(StringUtils.FlattenColorBunches("Please do not discard outdoors as it may attract stray cats.", "545454"));
		}
		switch (this._itemDefinition.type)
		{
		case ItemType.FOOD:
			this.typeLabel.SetText(StringUtils.Titleize(this._itemDefinition.type.ToString()) + "  |  " + StringUtils.Titleize(this._itemDefinition.foodType.ToString()));
			this.valueLabel.SetText("Nutrition Value: " + this._itemDefinition.itemFunctionValue.ToString());
			this.nameLabel.SetColor(ColorUtils.HexToColor("B8562C"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("CC7962"));
			this.valueLabel.SetColor(ColorUtils.HexToColor("9E6A42"));
			break;
		case ItemType.DRINK:
			this.typeLabel.SetText("Food  |  " + StringUtils.Titleize(this._itemDefinition.type.ToString()));
			this.valueLabel.SetText("Alcoholic Value: " + this._itemDefinition.itemFunctionValue.ToString());
			this.nameLabel.SetColor(ColorUtils.HexToColor("326CB3"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("528CDB"));
			this.valueLabel.SetColor(ColorUtils.HexToColor("508591"));
			break;
		case ItemType.GIFT:
			this.typeLabel.SetText(StringUtils.Titleize(this._itemDefinition.type.ToString()) + "  |  " + StringUtils.Titleize(this._itemDefinition.giftType.ToString()));
			this.valueLabel.SetText("Gift Value: " + this._itemDefinition.itemFunctionValue.ToString());
			this.nameLabel.SetColor(ColorUtils.HexToColor("A63E6E"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("C06E94"));
			this.valueLabel.SetColor(ColorUtils.HexToColor("A16799"));
			if (this._collectionItem && GameManager.System.Player.endingSceneShown)
			{
				string text = this.valueLabel.label.text + "\n";
				string defaultColor = ColorUtils.ColorToHex(this.valueLabel.label.color);
				if (GameManager.System.GameState != GameState.SIM)
				{
					text += "[[Can't order while dating.]stop]";
				}
				else if (GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory))
				{
					text += "[[Your inventory is too full.]stop]";
				}
				else if (GameManager.System.Player.hunie < 10000)
				{
					text += "[[Not enough Hunie to order.]stop]";
				}
				else
				{
					text += "[[Click to order (10,000 Hunie).]go]";
				}
				this.valueLabel.SetColor(Color.white);
				this.valueLabel.SetText(StringUtils.FlattenColorBunches(text, defaultColor));
			}
			break;
		case ItemType.UNIQUE_GIFT:
			this.typeLabel.SetText(StringUtils.Titleize(this._itemDefinition.type.ToString()) + "  |  " + StringUtils.Titleize(this._itemDefinition.specialGiftType.ToString()));
			if (!this._storeItem)
			{
				UnityEngine.Object.Destroy(this.valueLabel.gameObj);
				this._hasValue = false;
			}
			this.nameLabel.SetColor(ColorUtils.HexToColor("9E6B21"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("B3884F"));
			break;
		case ItemType.DATE_GIFT:
			this.typeLabel.SetText(StringUtils.Titleize(this._itemDefinition.type.ToString()) + "  |  " + StringUtils.Titleize(this._itemDefinition.dateGiftType.ToString()));
			this.valueLabel.SetText("Sentiment Needed: " + this._itemDefinition.sentimentCost.ToString());
			this.nameLabel.SetColor(ColorUtils.HexToColor("337D91"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("48A1AA"));
			this.valueLabel.SetColor(ColorUtils.HexToColor("3E89AF"));
			if (this._puzzleStatusItem && GameManager.System.Puzzle.Game != null && GameManager.System.Puzzle.Game.GetResourceValue(PuzzleGameResourceType.SENTIMENT) < this._itemDefinition.sentimentCost)
			{
				this.valueLabel.SetColor(ColorUtils.HexToColor("BC3939"));
			}
			break;
		case ItemType.ACCESSORY:
			this.typeLabel.SetText(StringUtils.Titleize(this._itemDefinition.type.ToString()) + "  |  " + StringUtils.Titleize(this._itemDefinition.accessoryType.ToString()));
			this.nameLabel.SetColor(ColorUtils.HexToColor("3A722A"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("598949"));
			if (this._traitsItem && GameManager.System.Player.GetTraitLevel(this._itemDefinition.playerTraitType) < 6)
			{
				if (this._itemDefinition.storeCost > GameManager.System.Player.hunie)
				{
					this.valueLabel.SetText("You can't afford to upgrade.");
					this.valueLabel.SetColor(ColorUtils.HexToColor("BC3939"));
				}
				else
				{
					this.valueLabel.SetText("Click to purchase upgrade.");
					this.valueLabel.SetColor(ColorUtils.HexToColor("3E89AF"));
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.valueLabel.gameObj);
				this._hasValue = false;
			}
			break;
		case ItemType.PANTIES:
			this.typeLabel.SetText("Girls Underwear");
			UnityEngine.Object.Destroy(this.valueLabel.gameObj);
			this._hasValue = false;
			this.nameLabel.SetColor(ColorUtils.HexToColor("803BA1"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("9364B3"));
			break;
		case ItemType.PRESENT:
			this.typeLabel.SetText("Present From Sky Garden");
			this.valueLabel.SetText("Click to open this present!");
			this.nameLabel.SetColor(ColorUtils.HexToColor("A33434"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("B55E62"));
			this.valueLabel.SetColor(ColorUtils.HexToColor("3E89AF"));
			break;
		case ItemType.MISC:
			this.typeLabel.SetText("Special Item");
			UnityEngine.Object.Destroy(this.valueLabel.gameObj);
			this._hasValue = false;
			this.nameLabel.SetColor(ColorUtils.HexToColor("A33434"));
			this.typeLabel.SetColor(ColorUtils.HexToColor("B55E62"));
			break;
		}
		if (this._storeItem)
		{
			string text = this.valueLabel.label.text + "\n";
			if (this._itemDefinition.type == ItemType.UNIQUE_GIFT)
			{
				text = string.Empty;
			}
			string defaultColor = ColorUtils.ColorToHex(this.valueLabel.label.color);
			if (GameManager.System.Player.IsInventoryFull(GameManager.System.Player.gifts) && GameManager.System.Player.IsInventoryFull(GameManager.System.Player.inventory))
			{
				text += "[[Your inventory is too full.]stop]";
			}
			else if (this._itemDefinition.storeCost > GameManager.System.Player.money)
			{
				text += "[[You can't afford this item.]stop]";
			}
			else
			{
				text += "[[Click to purchase this item.]go]";
			}
			this.valueLabel.SetColor(Color.white);
			this.valueLabel.SetText(StringUtils.FlattenColorBunches(text, defaultColor));
		}
		if (this._hasValue)
		{
			this.valueLabel.localY = this._valueLabelOrigY - (float)this.GetContentHeightPadding();
			if ((this._storeItem && this._itemDefinition.type != ItemType.UNIQUE_GIFT) || (this._collectionItem && GameManager.System.Player.endingSceneShown && this._itemDefinition.type == ItemType.GIFT))
			{
				this.valueLabel.localY += 25f;
			}
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00020898 File Offset: 0x0001EA98
	public override int GetContentHeightPadding()
	{
		int num = 23 * Mathf.Max(this.descriptionLabel.label.FormattedText.Split(new char[]
		{
			'\n'
		}).Length - 1, 0);
		if (!this._hasValue)
		{
			num -= 27;
		}
		if ((this._storeItem && this._itemDefinition.type != ItemType.UNIQUE_GIFT) || (this._collectionItem && GameManager.System.Player.endingSceneShown && this._itemDefinition.type == ItemType.GIFT))
		{
			num += 25;
		}
		return num;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00004E5E File Offset: 0x0000305E
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x0400035C RID: 860
	public const string DESC_LABEL_COLOR = "545454";

	// Token: 0x0400035D RID: 861
	public LabelObject nameLabel;

	// Token: 0x0400035E RID: 862
	public LabelObject typeLabel;

	// Token: 0x0400035F RID: 863
	public LabelObject descriptionLabel;

	// Token: 0x04000360 RID: 864
	public LabelObject valueLabel;

	// Token: 0x04000361 RID: 865
	private float _valueLabelOrigY;

	// Token: 0x04000362 RID: 866
	private bool _hasValue;

	// Token: 0x04000363 RID: 867
	private ItemDefinition _itemDefinition;

	// Token: 0x04000364 RID: 868
	private bool _puzzleStatusItem;

	// Token: 0x04000365 RID: 869
	private bool _storeItem;

	// Token: 0x04000366 RID: 870
	private bool _traitsItem;

	// Token: 0x04000367 RID: 871
	private bool _collectionItem;
}
