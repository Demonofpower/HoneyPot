using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class PopLabelObject : LabelObject
{
	// Token: 0x06000167 RID: 359 RVA: 0x000124E0 File Offset: 0x000106E0
	public void Init(Vector3 origin, DisplayObject source, bool down = false)
	{
		base.SetGlobalPosition(origin.x, origin.y);
		GameManager.Stage.effects.AddLabel(this, source);
		base.SetAlpha(0f);
		this.gameObj.transform.localScale = new Vector3(0.25f, 0.8f, 1f);
		this._complete = false;
		int num = 64;
		if (down)
		{
			num *= -1;
		}
		this._popSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimationComplete)));
		this._popSequence.Insert(0f, HOTween.To(this, 1.25f, new TweenParms().Prop("localY", base.localY + (float)num).Ease(EaseType.EaseOutCubic)));
		this._popSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this._popSequence.Insert(0f, HOTween.To(this, 0.25f, new TweenParms().Prop("labelAlpha", 0.9f).Ease(EaseType.EaseOutSine)));
		this._popSequence.Insert(1f, HOTween.To(this, 0.25f, new TweenParms().Prop("labelAlpha", 0).Ease(EaseType.EaseInSine)));
		this._popSequence.Play();
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000037D1 File Offset: 0x000019D1
	protected override void OnUpdate()
	{
		if (this._complete)
		{
			UnityEngine.Object.Destroy(this.gameObj);
			return;
		}
		base.OnUpdate();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x000037F0 File Offset: 0x000019F0
	private void OnAnimationComplete()
	{
		this._complete = true;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x000037F9 File Offset: 0x000019F9
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._popSequence != null && !this._popSequence.isPaused)
		{
			this._popSequence.Pause();
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00003833 File Offset: 0x00001A33
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._popSequence != null && this._popSequence.isPaused)
		{
			this._popSequence.Play();
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000386D File Offset: 0x00001A6D
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._popSequence, false);
		this._popSequence = null;
	}

	// Token: 0x0400010F RID: 271
	private Sequence _popSequence;

	// Token: 0x04000110 RID: 272
	private bool _complete;
}
