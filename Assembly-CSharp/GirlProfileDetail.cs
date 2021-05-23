using System;

// Token: 0x02000031 RID: 49
public class GirlProfileDetail : DisplayObject
{
	// Token: 0x060001CD RID: 461 RVA: 0x0001480C File Offset: 0x00012A0C
	public void Init(GirlDefinition girlDefinition, GirlPlayerData girlPlayerData, int detailIndex)
	{
		this.titleLabel = (base.GetChildByName("GirlProfileDetailTitle") as LabelObject);
		this.valueLabel = (base.GetChildByName("GirlProfileDetailLabel") as LabelObject);
		if (detailIndex == 2 && girlDefinition.secretGirl)
		{
			this.titleLabel.SetText("Homeworld");
		}
		if (girlDefinition == GameManager.System.Location.currentGirl && GameManager.System.Dialog.ShouldHideGirlDetails())
		{
			this.valueLabel.SetText("---");
			base.SetChildAlpha(0.5f, 0f);
		}
		else if (girlPlayerData.IsDetailKnown((GirlDetailType)detailIndex))
		{
			this.valueLabel.SetText(girlDefinition.details[detailIndex]);
		}
		else
		{
			this.valueLabel.SetText("???");
			base.SetChildAlpha(0.64f, 0f);
		}
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x04000168 RID: 360
	private LabelObject titleLabel;

	// Token: 0x04000169 RID: 361
	private LabelObject valueLabel;
}
