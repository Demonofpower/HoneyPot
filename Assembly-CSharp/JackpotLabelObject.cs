using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class JackpotLabelObject : LabelObject
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000115 RID: 277 RVA: 0x00003360 File Offset: 0x00001560
	// (remove) Token: 0x06000116 RID: 278 RVA: 0x00003379 File Offset: 0x00001579
	public event JackpotLabelObject.JackpotLabelDelegate JackpotLabelRolledEvent;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000117 RID: 279 RVA: 0x00003392 File Offset: 0x00001592
	// (remove) Token: 0x06000118 RID: 280 RVA: 0x000033AB File Offset: 0x000015AB
	public event JackpotLabelObject.JackpotLabelDelegate JackpotLabelCompleteEvent;

	// Token: 0x06000119 RID: 281 RVA: 0x00010FDC File Offset: 0x0000F1DC
	public void Init(Vector3 origin, DisplayObject source, int jackpotValue, string units, float duration = 1f, bool success = true)
	{
		base.SetGlobalPosition(origin.x, origin.y);
		GameManager.Stage.effects.AddLabel(this, source);
		base.SetAlpha(0f);
		base.SetLocalScale(1f, 0f, EaseType.Linear);
		this.currentDisplayValue = 0;
		this._rollingUp = true;
		this._rollUpComplete = false;
		this._origin = origin;
		this._sign = "-";
		if (jackpotValue >= 0)
		{
			this._sign = "+";
		}
		this._units = units;
		base.SetText(this._sign + 0.ToString() + " " + this._units);
		this._success = success;
		this._complete = false;
		if (duration < 1f)
		{
			duration = 1f;
		}
		this._jackpotSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnJackpotSequenceComplete)));
		this._jackpotSequence.Insert(0f, HOTween.To(this, 1f, new TweenParms().Prop("localY", base.localY + 32f).Ease(EaseType.EaseOutCubic)));
		this._jackpotSequence.Insert(0f, HOTween.To(this, 0.5f, new TweenParms().Prop("labelAlpha", 1).Ease(EaseType.EaseOutCubic)));
		this._jackpotSequence.Insert(0f, HOTween.To(this, duration, new TweenParms().Prop("currentDisplayValue", jackpotValue).Ease(EaseType.EaseOutCubic)));
		this._jackpotSequence.Play();
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0001118C File Offset: 0x0000F38C
	protected override void OnUpdate()
	{
		if (this._complete)
		{
			if (this.JackpotLabelCompleteEvent != null)
			{
				this.JackpotLabelCompleteEvent();
			}
			UnityEngine.Object.Destroy(this.gameObj);
			return;
		}
		if (this._rollingUp && this._jackpotSequence != null && !this._jackpotSequence.isPaused && !this._jackpotSequence.isComplete)
		{
			base.SetText(string.Concat(new object[]
			{
				this._sign,
				Mathf.Abs(this.currentDisplayValue),
				" ",
				this._units
			}));
		}
		else if (this._rollUpComplete)
		{
			this._rollUpComplete = false;
			this.AnimateJackpotRewardSequence();
		}
		base.OnUpdate();
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000033C4 File Offset: 0x000015C4
	private void OnJackpotSequenceComplete()
	{
		TweenUtils.KillSequence(this._jackpotSequence, true);
		this._rollingUp = false;
		this._rollUpComplete = true;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00011260 File Offset: 0x0000F460
	private void AnimateJackpotRewardSequence()
	{
		this._jackpotSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimationComplete)));
		if (this._success)
		{
			this._jackpotSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(1.2f, 1.2f, 1f)).Ease(EaseType.EaseOutCubic)));
			this._jackpotSequence.Insert(0.25f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInCubic)));
		}
		this._jackpotSequence.Insert(1f, HOTween.To(this, 0.25f, new TweenParms().Prop("labelAlpha", 0).Ease(EaseType.EaseInSine)));
		this._jackpotSequence.Play();
		if (this._success)
		{
			EnergyTrail component = new GameObject("EnergyTrail", new Type[]
			{
				typeof(EnergyTrail)
			}).GetComponent<EnergyTrail>();
			component.Init(GameManager.Stage.uiPuzzle.puzzleStatus.giftZone.gameObj.transform.position, this.energyTrailDef, null, EnergyTrailFormat.END, null);
		}
		if (this.JackpotLabelRolledEvent != null)
		{
			this.JackpotLabelRolledEvent();
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000033E0 File Offset: 0x000015E0
	private void OnAnimationComplete()
	{
		this._complete = true;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000033E9 File Offset: 0x000015E9
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._jackpotSequence != null && !this._jackpotSequence.isPaused)
		{
			this._jackpotSequence.Pause();
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00003423 File Offset: 0x00001623
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._jackpotSequence != null && this._jackpotSequence.isPaused)
		{
			this._jackpotSequence.Play();
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000345D File Offset: 0x0000165D
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._jackpotSequence, false);
		this._jackpotSequence = null;
	}

	// Token: 0x040000DC RID: 220
	private Sequence _jackpotSequence;

	// Token: 0x040000DD RID: 221
	private bool _rollingUp;

	// Token: 0x040000DE RID: 222
	private bool _rollUpComplete;

	// Token: 0x040000DF RID: 223
	private Vector3 _origin;

	// Token: 0x040000E0 RID: 224
	private string _sign;

	// Token: 0x040000E1 RID: 225
	private string _units;

	// Token: 0x040000E2 RID: 226
	private bool _success;

	// Token: 0x040000E3 RID: 227
	private bool _complete;

	// Token: 0x040000E4 RID: 228
	public int currentDisplayValue;

	// Token: 0x040000E5 RID: 229
	public EnergyTrailDefinition energyTrailDef;

	// Token: 0x0200001B RID: 27
	// (Invoke) Token: 0x06000122 RID: 290
	public delegate void JackpotLabelDelegate();
}
