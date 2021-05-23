using System;

// Token: 0x02000057 RID: 87
public class UIEffects : DisplayObject
{
	// Token: 0x060002AF RID: 687 RVA: 0x00019BC8 File Offset: 0x00017DC8
	protected override void OnStart()
	{
		base.OnStart();
		this._underEffects = base.GetParent().GetChildByName("UnderEffects");
		this.cursorContainer = base.GetChildByName("CursorContainer");
		this.lowerCursorContainer = this._underEffects.GetChildByName("CursorContainer");
		this._upperLabelContainer = base.GetChildByName("LabelContainer");
		this._lowerLabelContainer = this._underEffects.GetChildByName("LabelContainer");
		this._upperParticleContainer = base.GetChildByName("ParticleContainer");
		this._lowerParticleContainer = this._underEffects.GetChildByName("ParticleContainer");
		this._upperTooltipContainer = base.GetChildByName("TooltipContainer");
		this._lowerTooltipContainer = this._underEffects.GetChildByName("TooltipContainer");
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00019C90 File Offset: 0x00017E90
	public void AddParticleEffect(ParticleEmitter2D particleEmitter, DisplayObject source)
	{
		if (source.gameObj.transform.position.z > this._underEffects.gameObj.transform.position.z)
		{
			this._lowerParticleContainer.AddChild(particleEmitter);
		}
		else
		{
			this._upperParticleContainer.AddChild(particleEmitter);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00019CF4 File Offset: 0x00017EF4
	public void AddTooltip(UITooltip tooltip, DisplayObject source)
	{
		if (source.gameObj.transform.position.z > this._underEffects.gameObj.transform.position.z)
		{
			this._lowerTooltipContainer.AddChild(tooltip);
		}
		else
		{
			this._upperTooltipContainer.AddChild(tooltip);
		}
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00019D58 File Offset: 0x00017F58
	public void AddLabel(LabelObject label, DisplayObject source)
	{
		if (source.gameObj.transform.position.z > this._underEffects.gameObj.transform.position.z)
		{
			this._lowerLabelContainer.AddChild(label);
		}
		else
		{
			this._upperLabelContainer.AddChild(label);
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00019D58 File Offset: 0x00017F58
	public void AddDisplayObject(DisplayObject displayObject, DisplayObject source)
	{
		if (source.gameObj.transform.position.z > this._underEffects.gameObj.transform.position.z)
		{
			this._lowerLabelContainer.AddChild(displayObject);
		}
		else
		{
			this._upperLabelContainer.AddChild(displayObject);
		}
	}

	// Token: 0x04000253 RID: 595
	private DisplayObject _underEffects;

	// Token: 0x04000254 RID: 596
	public DisplayObject cursorContainer;

	// Token: 0x04000255 RID: 597
	public DisplayObject lowerCursorContainer;

	// Token: 0x04000256 RID: 598
	private DisplayObject _upperLabelContainer;

	// Token: 0x04000257 RID: 599
	private DisplayObject _lowerLabelContainer;

	// Token: 0x04000258 RID: 600
	private DisplayObject _upperParticleContainer;

	// Token: 0x04000259 RID: 601
	private DisplayObject _lowerParticleContainer;

	// Token: 0x0400025A RID: 602
	private DisplayObject _upperTooltipContainer;

	// Token: 0x0400025B RID: 603
	private DisplayObject _lowerTooltipContainer;
}
