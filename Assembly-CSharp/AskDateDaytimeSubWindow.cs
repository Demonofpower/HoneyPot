using System;
using System.Collections.Generic;
using Holoville.HOTween;

// Token: 0x02000082 RID: 130
public class AskDateDaytimeSubWindow : UISubWindow
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x060003E3 RID: 995 RVA: 0x0000507A File Offset: 0x0000327A
	// (remove) Token: 0x060003E4 RID: 996 RVA: 0x00005093 File Offset: 0x00003293
	public event AskDateDaytimeSubWindow.AskDateDaytimeSubWindowDelegate DaytimeSelectedEvent;

	// Token: 0x060003E5 RID: 997 RVA: 0x000232F0 File Offset: 0x000214F0
	public override void Init()
	{
		this._daytimeOptions = new List<AskDateOption>();
		for (int i = 0; i < 4; i++)
		{
			AskDateOption askDateOption = base.GetChildByName("AskDateOption" + i.ToString()) as AskDateOption;
			askDateOption.Init();
			this._daytimeOptions.Add(askDateOption);
			askDateOption.OptionClickedEvent += this.OnOptionClicked;
		}
		base.Init();
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00023364 File Offset: 0x00021564
	public override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._daytimeOptions.Count; i++)
		{
			this._daytimeOptions[i].childrenAlpha = 0f;
			this._daytimeOptions[i].localY = this._daytimeOptions[i].origLocalY - 32f;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x000233D4 File Offset: 0x000215D4
	public override void Show(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Show(animSequence, offsetDelay);
		for (int i = 0; i < this._daytimeOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._daytimeOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._daytimeOptions[i], 0.25f, new TweenParms().Prop("localY", this._daytimeOptions[i].origLocalY).Ease(EaseType.EaseOutCubic)));
		}
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0002349C File Offset: 0x0002169C
	public override void Hide(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Hide(animSequence, offsetDelay);
		for (int i = 0; i < this._daytimeOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._daytimeOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._daytimeOptions[i], 0.25f, new TweenParms().Prop("localY", this._daytimeOptions[i].origLocalY + 32f).Ease(EaseType.EaseInCubic)));
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000050AC File Offset: 0x000032AC
	private void OnOptionClicked(AskDateOption askDateOption)
	{
		if (this.DaytimeSelectedEvent != null)
		{
			this.DaytimeSelectedEvent(this._daytimeOptions.IndexOf(askDateOption));
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00023568 File Offset: 0x00021768
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._daytimeOptions.Count; i++)
		{
			this._daytimeOptions[i].OptionClickedEvent -= this.OnOptionClicked;
		}
		this._daytimeOptions.Clear();
		this._daytimeOptions = null;
	}

	// Token: 0x040003AE RID: 942
	private const int DAYTIME_OPTION_COUNT = 4;

	// Token: 0x040003AF RID: 943
	private List<AskDateOption> _daytimeOptions;

	// Token: 0x02000083 RID: 131
	// (Invoke) Token: 0x060003EC RID: 1004
	public delegate void AskDateDaytimeSubWindowDelegate(int selectedDaytimeIndex);
}
