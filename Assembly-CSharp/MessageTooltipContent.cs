using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class MessageTooltipContent : UITooltipContent
{
	// Token: 0x0600038C RID: 908 RVA: 0x00004E66 File Offset: 0x00003066
	public override void Init(TooltipObject tooltipObject)
	{
		base.Init(tooltipObject);
		this.contentLabel = (base.GetChildByName("TooltipMessageContent") as LabelObject);
		this.Refresh();
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00004E8B File Offset: 0x0000308B
	public override void Refresh()
	{
		this.contentLabel.SetText(StringUtils.FlattenColorBunches(this._tooltipSource.GetUniqueTooltipMessage(), this.labelColor));
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00020938 File Offset: 0x0001EB38
	public override int GetContentWidthPadding()
	{
		return Mathf.Clamp(Mathf.RoundToInt(this.contentLabel.label.GetMeshDimensionsForString(this.contentLabel.label.text).x - (float)this.labelWidth), 0, 117);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00004EAE File Offset: 0x000030AE
	public override int GetContentHeightPadding()
	{
		return 25 * Mathf.Max(this.contentLabel.label.FormattedText.Split(new char[]
		{
			'\n'
		}).Length - 1, 0);
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00004E5E File Offset: 0x0000305E
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x04000368 RID: 872
	public int labelWidth;

	// Token: 0x04000369 RID: 873
	public string labelColor;

	// Token: 0x0400036A RID: 874
	public LabelObject contentLabel;
}
