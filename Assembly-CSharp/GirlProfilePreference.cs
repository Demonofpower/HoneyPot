using System;

// Token: 0x02000034 RID: 52
public class GirlProfilePreference : DisplayObject
{
	// Token: 0x060001DA RID: 474 RVA: 0x00014900 File Offset: 0x00012B00
	public void Init(GirlDefinition girlDefinition, int preferenceIndex)
	{
		this.titleLabel = (base.GetChildByName("GirlProfilePreferenceTitle") as LabelObject);
		this.valueLabel = (base.GetChildByName("GirlProfilePreferenceLabel") as LabelObject);
		switch (preferenceIndex)
		{
		case 0:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.mostDesiredTrait.name));
			break;
		case 1:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.leastDesiredTrait.name));
			break;
		case 2:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.lovesGiftType.ToString()));
			break;
		case 3:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.uniqueGiftType.ToString()));
			break;
		case 4:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.likesGiftType1.ToString()) + ", " + StringUtils.Titleize(girlDefinition.likesGiftType2.ToString()));
			break;
		case 5:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.lovesFoodType.ToString()) + ", " + StringUtils.Titleize(girlDefinition.likesFoodType.ToString()));
			break;
		case 6:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.favDrink.name));
			break;
		case 7:
			this.valueLabel.SetText(StringUtils.Titleize(girlDefinition.alcoholTolerance.ToString()));
			break;
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x0400016C RID: 364
	private LabelObject titleLabel;

	// Token: 0x0400016D RID: 365
	private LabelObject valueLabel;
}
