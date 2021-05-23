using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class ItemDefinition : Definition
{
	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060004D9 RID: 1241 RVA: 0x000257A8 File Offset: 0x000239A8
	public int storeCost
	{
		get
		{
			switch (this.type)
			{
			case ItemType.FOOD:
				return this.itemFunctionValue * 100;
			case ItemType.DRINK:
				return this.itemFunctionValue * 100;
			case ItemType.GIFT:
				return this.itemFunctionValue * 100;
			case ItemType.UNIQUE_GIFT:
				return Mathf.RoundToInt((float)(250 + this.itemFunctionValue * 250 + (this.itemFunctionValue - 1) * 50) * this.specialCostMultiplier);
			case ItemType.DATE_GIFT:
				return 0;
			case ItemType.ACCESSORY:
				return Mathf.RoundToInt((float)(this.itemFunctionValue * 1500 - (this.itemFunctionValue - 1) * 200));
			case ItemType.PANTIES:
				return 0;
			case ItemType.PRESENT:
				return 0;
			default:
				return 0;
			}
		}
	}

	// Token: 0x040005C9 RID: 1481
	public ItemType type;

	// Token: 0x040005CA RID: 1482
	public new string name;

	// Token: 0x040005CB RID: 1483
	public bool hasShortName;

	// Token: 0x040005CC RID: 1484
	public string shortName;

	// Token: 0x040005CD RID: 1485
	public string description;

	// Token: 0x040005CE RID: 1486
	public string iconName;

	// Token: 0x040005CF RID: 1487
	public int itemFunctionValue;

	// Token: 0x040005D0 RID: 1488
	public ItemFoodType foodType;

	// Token: 0x040005D1 RID: 1489
	public ItemGiftType giftType;

	// Token: 0x040005D2 RID: 1490
	public ItemUniqueGiftType specialGiftType;

	// Token: 0x040005D3 RID: 1491
	public float specialCostMultiplier;

	// Token: 0x040005D4 RID: 1492
	public ItemDateGiftType dateGiftType;

	// Token: 0x040005D5 RID: 1493
	public int sentimentCost;

	// Token: 0x040005D6 RID: 1494
	public AbilityDefinition ability;

	// Token: 0x040005D7 RID: 1495
	public bool hidden;

	// Token: 0x040005D8 RID: 1496
	public ItemAccessoryType accessoryType;

	// Token: 0x040005D9 RID: 1497
	public PlayerTraitType playerTraitType;
}
