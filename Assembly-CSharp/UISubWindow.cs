using System;
using Holoville.HOTween;

// Token: 0x02000093 RID: 147
public class UISubWindow : DisplayObject
{
	// Token: 0x06000448 RID: 1096 RVA: 0x000053A6 File Offset: 0x000035A6
	public virtual void Init()
	{
		this.PreShow();
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0000306D File Offset: 0x0000126D
	public virtual void Refresh()
	{
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0000306D File Offset: 0x0000126D
	public virtual void PreShow()
	{
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000053A6 File Offset: 0x000035A6
	public virtual void Show(Sequence animSequence, float offsetDelay = 0f)
	{
		this.PreShow();
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0000306D File Offset: 0x0000126D
	public virtual void Hide(Sequence animSequence, float offsetDelay = 0f)
	{
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}
}
