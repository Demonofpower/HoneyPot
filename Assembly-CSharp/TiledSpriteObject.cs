using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class TiledSpriteObject : DisplayObject
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000193 RID: 403 RVA: 0x00012E64 File Offset: 0x00011064
	// (set) Token: 0x06000194 RID: 404 RVA: 0x00012E84 File Offset: 0x00011084
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

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000195 RID: 405 RVA: 0x000039E6 File Offset: 0x00001BE6
	// (set) Token: 0x06000196 RID: 406 RVA: 0x00012EDC File Offset: 0x000110DC
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

	// Token: 0x06000197 RID: 407 RVA: 0x000039F3 File Offset: 0x00001BF3
	protected override void OnAwake()
	{
		base.OnAwake();
		this.sprite = this.gameObj.GetComponent<tk2dTiledSprite>();
		if (this.sprite == null)
		{
			this.sprite = this.gameObj.AddComponent<tk2dTiledSprite>();
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00012F24 File Offset: 0x00011124
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

	// Token: 0x06000199 RID: 409 RVA: 0x00012F7C File Offset: 0x0001117C
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

	// Token: 0x0600019A RID: 410 RVA: 0x00012FD4 File Offset: 0x000111D4
	public void SetLightness(float lightness, float duration = 0f)
	{
		this.SetColor(new Color(lightness, lightness, lightness, this.sprite.color.a), duration);
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00013004 File Offset: 0x00011204
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

	// Token: 0x0600019C RID: 412 RVA: 0x00013074 File Offset: 0x00011274
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

	// Token: 0x0600019D RID: 413 RVA: 0x00003A2E File Offset: 0x00001C2E
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillTweener(this._tweenerAlpha, false);
		this._tweenerAlpha = null;
		TweenUtils.KillTweener(this._tweenerColor, false);
		this._tweenerColor = null;
	}

	// Token: 0x04000129 RID: 297
	public tk2dTiledSprite sprite;

	// Token: 0x0400012A RID: 298
	private Tweener _tweenerAlpha;

	// Token: 0x0400012B RID: 299
	private Tweener _tweenerColor;
}
