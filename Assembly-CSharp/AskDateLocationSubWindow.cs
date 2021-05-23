using System;
using System.Collections.Generic;
using Holoville.HOTween;

// Token: 0x02000086 RID: 134
public class AskDateLocationSubWindow : UISubWindow
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x060003FA RID: 1018 RVA: 0x0000513A File Offset: 0x0000333A
	// (remove) Token: 0x060003FB RID: 1019 RVA: 0x00005153 File Offset: 0x00003353
	public event AskDateLocationSubWindow.AskDateLocationSubWindowDelegate LocationSelectedEvent;

	// Token: 0x060003FC RID: 1020 RVA: 0x00023668 File Offset: 0x00021868
	public override void Init()
	{
		this._locationOptions = new List<AskDateLocation>();
		for (int i = 0; i < 9; i++)
		{
			AskDateLocation askDateLocation = base.GetChildByName("AskDateLocation" + i.ToString()) as AskDateLocation;
			askDateLocation.Init();
			this._locationOptions.Add(askDateLocation);
			askDateLocation.LocationOptionClickedEvent += this.OnLocationOptionClicked;
		}
		base.Init();
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x000236DC File Offset: 0x000218DC
	public override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._locationOptions.Count; i++)
		{
			this._locationOptions[i].childrenAlpha = 0f;
			this._locationOptions[i].localY = this._locationOptions[i].origLocalY - 32f;
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0002374C File Offset: 0x0002194C
	public override void Show(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Show(animSequence, offsetDelay);
		for (int i = 0; i < this._locationOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.09375f, HOTween.To(this._locationOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.09375f, HOTween.To(this._locationOptions[i], 0.25f, new TweenParms().Prop("localY", this._locationOptions[i].origLocalY).Ease(EaseType.EaseOutCubic)));
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00023814 File Offset: 0x00021A14
	public override void Hide(Sequence animSequence, float offsetDelay = 0f)
	{
		base.Hide(animSequence, offsetDelay);
		for (int i = 0; i < this._locationOptions.Count; i++)
		{
			animSequence.Insert(offsetDelay + (float)i * 0.09375f, HOTween.To(this._locationOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			animSequence.Insert(offsetDelay + (float)i * 0.09375f, HOTween.To(this._locationOptions[i], 0.25f, new TweenParms().Prop("localY", this._locationOptions[i].origLocalY + 32f).Ease(EaseType.EaseInCubic)));
		}
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0000516C File Offset: 0x0000336C
	private void OnLocationOptionClicked(AskDateLocation askDateLocation)
	{
		if (this.LocationSelectedEvent != null)
		{
			this.LocationSelectedEvent(askDateLocation.locationDefinition);
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x000238E0 File Offset: 0x00021AE0
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._locationOptions.Count; i++)
		{
			this._locationOptions[i].LocationOptionClickedEvent -= this.OnLocationOptionClicked;
		}
		this._locationOptions.Clear();
		this._locationOptions = null;
	}

	// Token: 0x040003B6 RID: 950
	private const int LOCATION_OPTION_COUNT = 9;

	// Token: 0x040003B7 RID: 951
	private List<AskDateLocation> _locationOptions;

	// Token: 0x02000087 RID: 135
	// (Invoke) Token: 0x06000403 RID: 1027
	public delegate void AskDateLocationSubWindowDelegate(LocationDefinition locationDefinition);
}
