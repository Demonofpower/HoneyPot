using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class TalkWindow : UIWindow
{
	// Token: 0x14000035 RID: 53
	// (add) Token: 0x0600043B RID: 1083 RVA: 0x00005355 File Offset: 0x00003555
	// (remove) Token: 0x0600043C RID: 1084 RVA: 0x0000536E File Offset: 0x0000356E
	public event TalkWindow.TalkWindowDelegate ResponseSelectedEvent;

	// Token: 0x0600043D RID: 1085 RVA: 0x000241CC File Offset: 0x000223CC
	public override void Init()
	{
		this.optionsContainer = base.GetChildByName("TalkOptionsContainer");
		List<DialogSceneResponseOption> list;
		if (GameManager.Stage.uiWindows.forceResponseOptions != null)
		{
			list = GameManager.Stage.uiWindows.forceResponseOptions;
			GameManager.Stage.uiWindows.forceResponseOptions = null;
		}
		else
		{
			DialogSceneStep activeDialogSceneStep = GameManager.System.Dialog.GetActiveDialogSceneStep();
			list = ListUtils.Copy<DialogSceneResponseOption>(activeDialogSceneStep.responseOptions);
			if (!activeDialogSceneStep.preventOptionShuffle)
			{
				ListUtils.Shuffle<DialogSceneResponseOption>(list);
			}
		}
		this._talkOptions = new List<TalkOption>();
		for (int i = 0; i < 5; i++)
		{
			TalkOption talkOption = this.optionsContainer.GetChildByName("TalkOption" + i.ToString()) as TalkOption;
			if (i < list.Count)
			{
				talkOption.localY -= (float)(110 * i);
				talkOption.Init(list[i]);
				this._talkOptions.Add(talkOption);
				talkOption.OptionClickedEvent += this.OnOptionClicked;
			}
			else
			{
				UnityEngine.Object.Destroy(talkOption.gameObj);
			}
		}
		this.optionsContainer.localY += (float)(55 * (this._talkOptions.Count - 1));
		base.Init();
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00024314 File Offset: 0x00022514
	protected override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._talkOptions.Count; i++)
		{
			this._talkOptions[i].childrenAlpha = 0f;
			this._talkOptions[i].localY = this._talkOptions[i].origLocalY - 32f;
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00024384 File Offset: 0x00022584
	protected override void Show(Sequence animSequence)
	{
		for (int i = 0; i < this._talkOptions.Count; i++)
		{
			animSequence.Insert((float)i * 0.125f, HOTween.To(this._talkOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert((float)i * 0.125f, HOTween.To(this._talkOptions[i], 0.25f, new TweenParms().Prop("localY", this._talkOptions[i].origLocalY).Ease(EaseType.EaseOutCubic)));
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00024440 File Offset: 0x00022640
	protected override void Hide(Sequence animSequence)
	{
		for (int i = 0; i < this._talkOptions.Count; i++)
		{
			animSequence.Insert((float)i * 0.125f, HOTween.To(this._talkOptions[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			animSequence.Insert((float)i * 0.125f, HOTween.To(this._talkOptions[i], 0.25f, new TweenParms().Prop("localY", this._talkOptions[i].origLocalY + 32f).Ease(EaseType.EaseInCubic)));
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00005387 File Offset: 0x00003587
	private void OnOptionClicked(TalkOption talkOption)
	{
		if (this.ResponseSelectedEvent != null)
		{
			this.ResponseSelectedEvent(this, talkOption.responseOption);
		}
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00024500 File Offset: 0x00022700
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._talkOptions.Count; i++)
		{
			this._talkOptions[i].OptionClickedEvent -= this.OnOptionClicked;
		}
		this._talkOptions.Clear();
		this._talkOptions = null;
	}

	// Token: 0x040003D3 RID: 979
	private const int TALK_OPTIONS_COUNT = 5;

	// Token: 0x040003D4 RID: 980
	public DisplayObject optionsContainer;

	// Token: 0x040003D5 RID: 981
	private List<TalkOption> _talkOptions;

	// Token: 0x02000092 RID: 146
	// (Invoke) Token: 0x06000444 RID: 1092
	public delegate void TalkWindowDelegate(TalkWindow talkWindow, DialogSceneResponseOption responseOption);
}
