using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class LabelObject : DisplayObject
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000126 RID: 294 RVA: 0x00003478 File Offset: 0x00001678
	// (remove) Token: 0x06000127 RID: 295 RVA: 0x00003491 File Offset: 0x00001691
	public event LabelObject.LabelObjectDelegate ClockLabelDaytimeTickEvent;

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000128 RID: 296 RVA: 0x000113EC File Offset: 0x0000F5EC
	// (set) Token: 0x06000129 RID: 297 RVA: 0x0001140C File Offset: 0x0000F60C
	public float labelAlpha
	{
		get
		{
			return this.label.color.a;
		}
		set
		{
			this.SetColor(new Color(this.label.color.r, this.label.color.g, this.label.color.b, value));
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00011460 File Offset: 0x0000F660
	protected override void OnAwake()
	{
		base.OnAwake();
		this.label = this.gameObj.GetComponent<tk2dTextMesh>();
		if (this.label == null)
		{
			this.label = this.gameObj.AddComponent<tk2dTextMesh>();
		}
		int num;
		bool flag = int.TryParse(this.label.text, out num);
		if (flag)
		{
			this.currentIntValue = num;
		}
		else
		{
			this.currentIntValue = 0;
		}
		this._previousIntValue = this.currentIntValue;
		this._previousSoundIntValue = this._previousIntValue;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000114EC File Offset: 0x0000F6EC
	protected override void OnUpdate()
	{
		if (this.currentIntValue != this._previousIntValue)
		{
			if (this.intFormat == IntFormat.CLOCK_TIME && GameManager.System.Clock.DayTime(this.currentIntValue) != GameManager.System.Clock.DayTime(this._previousIntValue) && this.ClockLabelDaytimeTickEvent != null)
			{
				this.ClockLabelDaytimeTickEvent();
			}
			this._previousIntValue = this.currentIntValue;
			this.SetText(this.FormatCurrentIntValue());
			if (Mathf.Abs(this._previousSoundIntValue - this._previousIntValue) >= 2)
			{
				this._previousSoundIntValue = this._previousIntValue;
				GameManager.System.Audio.Play(AudioCategory.SOUND, this.intValueSound, false, 1f, true);
			}
		}
		base.OnUpdate();
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000034AA File Offset: 0x000016AA
	public void SetText(string text)
	{
		this.label.text = text;
		this.label.inlineStyling = true;
		this.label.Commit();
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000115BC File Offset: 0x0000F7BC
	public float SetText(int val, float duration = 0f, bool durationIsPerUnit = false, float maxDuration = 1f)
	{
		TweenUtils.KillTweener(this._intValueTweener, true);
		int num = Mathf.Abs(this.currentIntValue - val);
		if (durationIsPerUnit)
		{
			duration = Mathf.Min(duration * (float)num, maxDuration);
		}
		if (duration <= 0f || num <= 1)
		{
			this.currentIntValue = val;
			this._previousIntValue = this.currentIntValue;
			this._previousSoundIntValue = this._previousIntValue;
			this.SetText(this.FormatCurrentIntValue());
		}
		else
		{
			this._intValueTweener = HOTween.To(this, duration, new TweenParms().Prop("currentIntValue", val).Ease(EaseType.EaseOutCubic).OnComplete(new TweenDelegate.TweenCallback(this.IntValueTweenerComplete)));
		}
		return duration;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x000034CF File Offset: 0x000016CF
	private void IntValueTweenerComplete()
	{
		if (this.intFormat == IntFormat.CLOCK_TIME && this.ClockLabelDaytimeTickEvent != null)
		{
			this.ClockLabelDaytimeTickEvent();
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000034F3 File Offset: 0x000016F3
	public void SetIntFormat(IntFormat format, bool flag = false)
	{
		this.intFormat = format;
		this.formatFlag = flag;
		this.SetText(this.FormatCurrentIntValue());
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00011674 File Offset: 0x0000F874
	private string FormatCurrentIntValue()
	{
		int num = 1;
		if (this.formatFlag)
		{
			num = 2;
		}
		switch (this.intFormat)
		{
		case IntFormat.CURRENCY:
			return StringUtils.FormatIntAsCurrency(this.currentIntValue, this.formatFlag);
		case IntFormat.CLOCK_TIME:
			return StringUtils.FormatIntWithDigitCount(GameManager.System.Clock.Hour(this.currentIntValue), num) + ":" + StringUtils.FormatIntWithDigitCount(GameManager.System.Clock.Minute(this.currentIntValue), 2);
		case IntFormat.TIME_COST:
			return StringUtils.FormatIntAsTimeCost(this.currentIntValue);
		case IntFormat.LEADING_ZEROS:
			return StringUtils.FormatIntWithDigitCount(this.currentIntValue, num + 1);
		default:
			return this.currentIntValue.ToString();
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000350F File Offset: 0x0000170F
	public void SetFont(tk2dFontData fontData)
	{
		this.label.font = fontData;
		this.label.Commit();
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00011730 File Offset: 0x0000F930
	public void SetAlpha(float alpha)
	{
		this.label.color = new Color(this.label.color.r, this.label.color.g, this.label.color.b, alpha);
		this.label.Commit();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00011794 File Offset: 0x0000F994
	public void SetLightness(float lightness)
	{
		this.label.color = new Color(lightness, lightness, lightness, this.label.color.a);
		this.label.Commit();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00003528 File Offset: 0x00001728
	public void SetColor(Color color)
	{
		this.label.color = color;
		this.label.Commit();
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00003541 File Offset: 0x00001741
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._intValueTweener != null && this._intValueTweener.IsTweening(this))
		{
			this._intValueTweener.Pause();
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000357C File Offset: 0x0000177C
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._intValueTweener != null && this._intValueTweener.isPaused)
		{
			this._intValueTweener.Play();
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x000035B6 File Offset: 0x000017B6
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillTweener(this._intValueTweener, false);
		this._intValueTweener = null;
	}

	// Token: 0x040000E8 RID: 232
	public IntFormat intFormat;

	// Token: 0x040000E9 RID: 233
	public bool formatFlag;

	// Token: 0x040000EA RID: 234
	public AudioDefinition intValueSound;

	// Token: 0x040000EB RID: 235
	public tk2dTextMesh label;

	// Token: 0x040000EC RID: 236
	public int currentIntValue;

	// Token: 0x040000ED RID: 237
	private int _previousIntValue;

	// Token: 0x040000EE RID: 238
	private int _previousSoundIntValue;

	// Token: 0x040000EF RID: 239
	private Tweener _intValueTweener;

	// Token: 0x0200001D RID: 29
	// (Invoke) Token: 0x06000139 RID: 313
	public delegate void LabelObjectDelegate();
}
