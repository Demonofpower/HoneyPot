using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class AskDateOption : DisplayObject
{
	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06000407 RID: 1031 RVA: 0x0000519D File Offset: 0x0000339D
	// (remove) Token: 0x06000408 RID: 1032 RVA: 0x000051B6 File Offset: 0x000033B6
	public event AskDateOption.AskDateOptionDelegate OptionClickedEvent;

	// Token: 0x06000409 RID: 1033 RVA: 0x00023940 File Offset: 0x00021B40
	public void Init()
	{
		this.contentLabel = (base.GetChildByName("AskDateOptionLabel") as LabelObject);
		if (this.contentOptions.Count > 0)
		{
			this.contentLabel.SetText(this.contentOptions[UnityEngine.Random.Range(0, this.contentOptions.Count)]);
		}
		this.origLocalY = base.transform.localPosition.y;
		base.button.ButtonPressedEvent += this.OnButtonPress;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000051CF File Offset: 0x000033CF
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.OptionClickedEvent != null)
		{
			this.OptionClickedEvent(this);
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x000051E8 File Offset: 0x000033E8
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x040003B9 RID: 953
	public List<string> contentOptions = new List<string>();

	// Token: 0x040003BA RID: 954
	public LabelObject contentLabel;

	// Token: 0x040003BB RID: 955
	public float origLocalY;

	// Token: 0x02000089 RID: 137
	// (Invoke) Token: 0x0600040D RID: 1037
	public delegate void AskDateOptionDelegate(AskDateOption askDateOption);
}
