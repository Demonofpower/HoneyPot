using System;

// Token: 0x02000053 RID: 83
public class CreditsScreen : DisplayObject
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000298 RID: 664 RVA: 0x0000439B File Offset: 0x0000259B
	// (remove) Token: 0x06000299 RID: 665 RVA: 0x000043B4 File Offset: 0x000025B4
	public event CreditsScreen.CreditsScreenDelegate CreditsScreenClosedEvent;

	// Token: 0x0600029A RID: 666 RVA: 0x000198C4 File Offset: 0x00017AC4
	public void Init()
	{
		this.creditsContainer = base.GetChildByName("CreditsContainer");
		this.voicesLabel = (this.creditsContainer.GetChildByName("CreditLabelVoices") as LabelObject);
		if (SaveUtils.IsInited())
		{
			string text = this.voicesLabel.label.text;
			string[] array = new string[]
			{
				"Momo",
				"Celeste",
				"Venus"
			};
			bool[] array2 = new bool[3];
			int[] array3 = new int[]
			{
				10,
				11,
				12
			};
			for (int i = 0; i < 4; i++)
			{
				SaveFile saveFile = SaveUtils.GetSaveFile(i);
				if (saveFile.started)
				{
					for (int j = 0; j < 3; j++)
					{
						if (saveFile.girls[array3[j]].metStatus == 3)
						{
							array2[j] = true;
						}
					}
				}
			}
			int num = 0;
			for (int k = 0; k < 3; k++)
			{
				num = text.IndexOf("???", num);
				if (array2[k])
				{
					text = text.Remove(num, 3).Insert(num, array[k]);
				}
				num += 4;
			}
			this.voicesLabel.SetText(text);
		}
		this._crawlTimestamp = 0f;
		GameManager.Stage.MouseDownEvent += this.OnStageClicked;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00019A30 File Offset: 0x00017C30
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (GameManager.System.Lifetime(false) - this._crawlTimestamp >= 0.016f)
		{
			this._crawlTimestamp = GameManager.System.Lifetime(false);
			this.creditsContainer.localY += 2f;
		}
		if (this.creditsContainer.localY >= 8352f)
		{
			this.creditsContainer.localY = 0f;
		}
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000043CD File Offset: 0x000025CD
	private void OnStageClicked(DisplayObject displayObject)
	{
		GameManager.Stage.MouseDownEvent -= this.OnStageClicked;
		if (this.CreditsScreenClosedEvent != null)
		{
			this.CreditsScreenClosedEvent();
		}
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x0400024C RID: 588
	public DisplayObject creditsContainer;

	// Token: 0x0400024D RID: 589
	public LabelObject voicesLabel;

	// Token: 0x0400024E RID: 590
	private float _crawlTimestamp;

	// Token: 0x02000054 RID: 84
	// (Invoke) Token: 0x0600029F RID: 671
	public delegate void CreditsScreenDelegate();
}
