using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class SpriteObject : DisplayObject
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600017A RID: 378 RVA: 0x000128F4 File Offset: 0x00010AF4
	// (set) Token: 0x0600017B RID: 379 RVA: 0x00012914 File Offset: 0x00010B14
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

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600017C RID: 380 RVA: 0x000038FE File Offset: 0x00001AFE
	// (set) Token: 0x0600017D RID: 381 RVA: 0x0001296C File Offset: 0x00010B6C
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

	// Token: 0x0600017E RID: 382 RVA: 0x0000390B File Offset: 0x00001B0B
	protected override void OnAwake()
	{
		base.OnAwake();
		this.sprite = this.gameObj.GetComponent<tk2dSprite>();
		if (this.sprite == null)
		{
			this.sprite = this.gameObj.AddComponent<tk2dSprite>();
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x000129B4 File Offset: 0x00010BB4
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

	// Token: 0x06000180 RID: 384 RVA: 0x00012A0C File Offset: 0x00010C0C
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

	// Token: 0x06000181 RID: 385 RVA: 0x00012A64 File Offset: 0x00010C64
	public void SetLightness(float lightness, float duration = 0f)
	{
		this.SetColor(new Color(lightness, lightness, lightness, this.sprite.color.a), duration);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00012A94 File Offset: 0x00010C94
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

	// Token: 0x06000183 RID: 387 RVA: 0x00012B04 File Offset: 0x00010D04
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

	// Token: 0x06000184 RID: 388 RVA: 0x00003946 File Offset: 0x00001B46
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillTweener(this._tweenerAlpha, false);
		this._tweenerAlpha = null;
		TweenUtils.KillTweener(this._tweenerColor, false);
		this._tweenerColor = null;
	}

	// Token: 0x04000114 RID: 276
	public tk2dSprite sprite;

	// Token: 0x04000115 RID: 277
	private Tweener _tweenerAlpha;

	// Token: 0x04000116 RID: 278
	private Tweener _tweenerColor;
}
