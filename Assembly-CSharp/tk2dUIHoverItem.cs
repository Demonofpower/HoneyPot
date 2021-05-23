using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
[AddComponentMenu("2D Toolkit/UI/tk2dUIHoverItem")]
public class tk2dUIHoverItem : tk2dUIBaseItemControl
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06000A9D RID: 2717 RVA: 0x0000A49F File Offset: 0x0000869F
	// (remove) Token: 0x06000A9E RID: 2718 RVA: 0x0000A4B8 File Offset: 0x000086B8
	public event Action<tk2dUIHoverItem> OnToggleHover;

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0000A4D1 File Offset: 0x000086D1
	// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0000A4D9 File Offset: 0x000086D9
	public bool IsOver
	{
		get
		{
			return this.isOver;
		}
		set
		{
			if (this.isOver != value)
			{
				this.isOver = value;
				this.SetState();
				if (this.OnToggleHover != null)
				{
					this.OnToggleHover(this);
				}
			}
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0000A50B File Offset: 0x0000870B
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0000A513 File Offset: 0x00008713
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver += this.HoverOver;
			this.uiItem.OnHoverOut += this.HoverOut;
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0000A553 File Offset: 0x00008753
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver -= this.HoverOver;
			this.uiItem.OnHoverOut -= this.HoverOut;
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0000A593 File Offset: 0x00008793
	private void HoverOver()
	{
		this.IsOver = true;
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0000A59C File Offset: 0x0000879C
	private void HoverOut()
	{
		this.IsOver = false;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0000A5A5 File Offset: 0x000087A5
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.overStateGO, this.isOver);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.outStateGO, !this.isOver);
	}

	// Token: 0x04000BDE RID: 3038
	public GameObject outStateGO;

	// Token: 0x04000BDF RID: 3039
	public GameObject overStateGO;

	// Token: 0x04000BE0 RID: 3040
	private bool isOver;
}
