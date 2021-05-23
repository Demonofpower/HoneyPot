using System;

// Token: 0x02000078 RID: 120
public class UITooltipContent : DisplayObject
{
	// Token: 0x06000396 RID: 918 RVA: 0x00004EDD File Offset: 0x000030DD
	public virtual void Init(TooltipObject tooltipObject)
	{
		this._tooltipObject = tooltipObject;
		this._tooltipSource = this._tooltipObject.GetTooltipSource();
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0000306D File Offset: 0x0000126D
	public virtual void Refresh()
	{
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00004EF7 File Offset: 0x000030F7
	public virtual int GetContentWidthPadding()
	{
		return 0;
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00004EF7 File Offset: 0x000030F7
	public virtual int GetContentHeightPadding()
	{
		return 0;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x0400036E RID: 878
	public int width;

	// Token: 0x0400036F RID: 879
	public int height;

	// Token: 0x04000370 RID: 880
	public bool centered;

	// Token: 0x04000371 RID: 881
	public string customTooltipSprite;

	// Token: 0x04000372 RID: 882
	protected TooltipObject _tooltipObject;

	// Token: 0x04000373 RID: 883
	protected DisplayObject _tooltipSource;
}
