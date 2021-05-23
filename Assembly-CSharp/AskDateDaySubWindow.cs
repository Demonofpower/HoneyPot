using System;
using System.Collections.Generic;
using Holoville.HOTween;

// Token: 0x02000080 RID: 128
public class AskDateDaySubWindow : UISubWindow
{
	// Token: 0x1400002E RID: 46
	// (add) Token: 0x060003D6 RID: 982 RVA: 0x00005024 File Offset: 0x00003224
	// (remove) Token: 0x060003D7 RID: 983 RVA: 0x0000503D File Offset: 0x0000323D
	public event AskDateDaySubWindow.AskDateDaySubWindowDelegate DaySelectedEvent;

	// Token: 0x060003D8 RID: 984 RVA: 0x00023018 File Offset: 0x00021218
	public override void Init()
	{
		this._dayOptions = new List<AskDateOption>();
		for (int i = 0; i < 3; i++)
		{
			AskDateOption askDateOption = base.GetChildByName("AskDateOption" + i.ToString()) as AskDateOption;
			askDateOption.Init();
			this._dayOptions.Add(askDateOption);
			askDateOption.OptionClickedEvent += this.OnOptionClicked;
		}
		base.Init();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0002308C File Offset: 0x0002128C
	public override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._dayOptions.Count; i++)
		{
			this._dayOptions[i].childrenAlpha = 0f;
			this._dayOptions[i].localY = this._dayOptions[i].origLocalY - 32f;
		}
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000230FC File Offset: 0x000212FC
	public override void Show(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Show(animSequence, offsetDelay);
		for (int i = 0; i < this._dayOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._dayOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._dayOptions[i], 0.25f, new TweenParms().Prop("localY", this._dayOptions[i].origLocalY).Ease(EaseType.EaseOutCubic)));
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x000231C4 File Offset: 0x000213C4
	public override void Hide(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Hide(animSequence, offsetDelay);
		for (int i = 0; i < this._dayOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._dayOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.125f, HOTween.To(this._dayOptions[i], 0.25f, new TweenParms().Prop("localY", this._dayOptions[i].origLocalY + 32f).Ease(EaseType.EaseInCubic)));
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00005056 File Offset: 0x00003256
	private void OnOptionClicked(AskDateOption askDateOption)
	{
		if (this.DaySelectedEvent != null)
		{
			this.DaySelectedEvent(this._dayOptions.IndexOf(askDateOption));
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00023290 File Offset: 0x00021490
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._dayOptions.Count; i++)
		{
			this._dayOptions[i].OptionClickedEvent -= this.OnOptionClicked;
		}
		this._dayOptions.Clear();
		this._dayOptions = null;
	}

	// Token: 0x040003AB RID: 939
	private const int DAY_OPTION_COUNT = 3;

	// Token: 0x040003AC RID: 940
	private List<AskDateOption> _dayOptions;

	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x060003DF RID: 991
	public delegate void AskDateDaySubWindowDelegate(int selectedDayIndex);
}
