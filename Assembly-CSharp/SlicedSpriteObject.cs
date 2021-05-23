using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class SlicedSpriteObject : DisplayObject
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600016E RID: 366 RVA: 0x00012678 File Offset: 0x00010878
	// (set) Token: 0x0600016F RID: 367 RVA: 0x00012698 File Offset: 0x00010898
	public float spriteAlpha
	{
		get
		{
			return this.sprite.color.a;
		}
		set
		{
			this.sprite.color = new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, value);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000170 RID: 368 RVA: 0x00003888 File Offset: 0x00001A88
	// (set) Token: 0x06000171 RID: 369 RVA: 0x000126F0 File Offset: 0x000108F0
	public Color spriteColor
	{
		get
		{
			return this.sprite.color;
		}
		set
		{
			this.sprite.color = new Color(value.r, value.g, value.b, this.sprite.color.a);
		}
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00003895 File Offset: 0x00001A95
	protected override void OnAwake()
	{
		base.OnAwake();
		this.sprite = this.gameObj.GetComponent<tk2dSlicedSprite>();
		if (this.sprite == null)
		{
			this.sprite = this.gameObj.AddComponent<tk2dSlicedSprite>();
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00012738 File Offset: 0x00010938
	public void SetAlpha(float alpha, float duration = 0f)
	{
		TweenUtils.KillTweener(this._tweenerAlpha, false);
		if (duration <= 0f)
		{
			this.spriteAlpha = alpha;
		}
		else
		{
			this._tweenerAlpha = HOTween.To(this, duration, new TweenParms().Prop("spriteAlpha", alpha).Ease(EaseType.Linear));
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00012790 File Offset: 0x00010990
	public void SetColor(Color color, float duration = 0f)
	{
		TweenUtils.KillTweener(this._tweenerColor, false);
		if (duration <= 0f)
		{
			this.spriteColor = color;
		}
		else
		{
			this._tweenerColor = HOTween.To(this, duration, new TweenParms().Prop("spriteColor", color).Ease(EaseType.Linear));
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x000127E8 File Offset: 0x000109E8
	public void SetLightness(float lightness, float duration = 0f)
	{
		this.SetColor(new Color(lightness, lightness, lightness, this.sprite.color.a), duration);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00012818 File Offset: 0x00010A18
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._tweenerAlpha != null && this._tweenerAlpha.IsTweening(this))
		{
			this._tweenerAlpha.Pause();
		}
		if (this._tweenerColor != null && this._tweenerColor.IsTweening(this))
		{
			this._tweenerColor.Pause();
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00012888 File Offset: 0x00010A88
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._tweenerAlpha != null && this._tweenerAlpha.isPaused)
		{
			this._tweenerAlpha.Play();
		}
		if (this._tweenerColor != null && this._tweenerColor.isPaused)
		{
			this._tweenerColor.Play();
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x000038D0 File Offset: 0x00001AD0
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillTweener(this._tweenerAlpha, false);
		this._tweenerAlpha = null;
		TweenUtils.KillTweener(this._tweenerColor, false);
		this._tweenerColor = null;
	}

	// Token: 0x04000111 RID: 273
	public tk2dSlicedSprite sprite;

	// Token: 0x04000112 RID: 274
	private Tweener _tweenerAlpha;

	// Token: 0x04000113 RID: 275
	private Tweener _tweenerColor;
}
