using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class BurstLabelObject : LabelObject
{
	// Token: 0x0600008B RID: 139 RVA: 0x0000DBEC File Offset: 0x0000BDEC
	public void Init(Vector3 origin, DisplayObject source)
	{
		this._burstLabelContainer = new GameObject("BurstLabelObjectContainer", new Type[]
		{
			typeof(DisplayObject)
		}).GetComponent<DisplayObject>();
		this._burstLabelContainer.AddChild(this);
		base.localX -= 14f;
		base.localY -= 4f;
		this._burstLabelContainer.SetGlobalPosition(origin.x, origin.y);
		GameManager.Stage.effects.AddDisplayObject(this._burstLabelContainer, source);
		this._burstLabelContainer.gameObj.transform.localScale = Vector3.one;
		base.SetAlpha(0f);
		this._complete = false;
		this._burstSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimationComplete)));
		this._burstSequence.Insert(0f, HOTween.To(this._burstLabelContainer.gameObj.transform, 0.55f, new TweenParms().Prop("localScale", Vector3.one * 1.8f).Ease(EaseType.EaseOutSine)));
		this._burstSequence.Insert(0f, HOTween.To(this, 0.1f, new TweenParms().Prop("labelAlpha", 1).Ease(EaseType.EaseOutSine)));
		this._burstSequence.Insert(0.15f, HOTween.To(this, 0.4f, new TweenParms().Prop("labelAlpha", 0).Ease(EaseType.EaseInSine)));
		this._burstSequence.Play();
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00002D2D File Offset: 0x00000F2D
	protected override void OnUpdate()
	{
		if (this._complete)
		{
			UnityEngine.Object.Destroy(this._burstLabelContainer.gameObj);
			return;
		}
		base.OnUpdate();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00002D51 File Offset: 0x00000F51
	private void OnAnimationComplete()
	{
		this._complete = true;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00002D5A File Offset: 0x00000F5A
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._burstSequence != null && !this._burstSequence.isPaused)
		{
			this._burstSequence.Pause();
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00002D94 File Offset: 0x00000F94
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._burstSequence != null && this._burstSequence.isPaused)
		{
			this._burstSequence.Play();
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00002DCE File Offset: 0x00000FCE
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._burstSequence, false);
		this._burstSequence = null;
	}

	// Token: 0x04000074 RID: 116
	private DisplayObject _burstLabelContainer;

	// Token: 0x04000075 RID: 117
	private Sequence _burstSequence;

	// Token: 0x04000076 RID: 118
	private bool _complete;
}
