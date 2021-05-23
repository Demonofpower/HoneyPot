using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class ClippedSpriteObject : DisplayObject
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000092 RID: 146 RVA: 0x0000DD9C File Offset: 0x0000BF9C
	// (set) Token: 0x06000093 RID: 147 RVA: 0x0000DDBC File Offset: 0x0000BFBC
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

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000094 RID: 148 RVA: 0x00002DE9 File Offset: 0x00000FE9
	// (set) Token: 0x06000095 RID: 149 RVA: 0x0000DE14 File Offset: 0x0000C014
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

	// Token: 0x06000096 RID: 150 RVA: 0x00002DF6 File Offset: 0x00000FF6
	protected override void OnAwake()
	{
		base.OnAwake();
		this.sprite = this.gameObj.GetComponent<tk2dClippedSprite>();
		if (this.sprite == null)
		{
			this.sprite = this.gameObj.AddComponent<tk2dClippedSprite>();
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000DE5C File Offset: 0x0000C05C
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

	// Token: 0x06000098 RID: 152 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
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

	// Token: 0x06000099 RID: 153 RVA: 0x0000DF0C File Offset: 0x0000C10C
	public void SetLightness(float lightness, float duration = 0f)
	{
		this.SetColor(new Color(lightness, lightness, lightness, this.sprite.color.a), duration);
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000DF3C File Offset: 0x0000C13C
	public void SetClip(ClippedSpriteSide clipSide, float clipValue, float duration = 0f)
	{
		if (this._tweenerClip != null)
		{
			this._tweenerClip.Kill();
		}
		Rect clipRect = this.sprite.ClipRect;
		switch (clipSide)
		{
		case ClippedSpriteSide.TOP:
			clipRect.height = clipValue;
			break;
		case ClippedSpriteSide.RIGHT:
			clipRect.width = clipValue;
			break;
		case ClippedSpriteSide.BOTTOM:
			clipRect.y = clipValue;
			break;
		case ClippedSpriteSide.LEFT:
			clipRect.x = clipValue;
			break;
		}
		if (duration <= 0f)
		{
			this.sprite.ClipRect = clipRect;
		}
		else
		{
			this._tweenerClip = HOTween.To(this.sprite, duration, new TweenParms().Prop("ClipRect", clipRect).Ease(EaseType.Linear));
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000E008 File Offset: 0x0000C208
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
		if (this._tweenerClip != null && this._tweenerClip.IsTweening(this.sprite))
		{
			this._tweenerClip.Pause();
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
		if (this._tweenerClip != null && this._tweenerClip.isPaused)
		{
			this._tweenerClip.Play();
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000E138 File Offset: 0x0000C338
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillTweener(this._tweenerAlpha, false);
		this._tweenerAlpha = null;
		TweenUtils.KillTweener(this._tweenerColor, false);
		this._tweenerColor = null;
		TweenUtils.KillTweener(this._tweenerClip, false);
		this._tweenerClip = null;
	}

	// Token: 0x04000077 RID: 119
	public tk2dClippedSprite sprite;

	// Token: 0x04000078 RID: 120
	private Tweener _tweenerAlpha;

	// Token: 0x04000079 RID: 121
	private Tweener _tweenerColor;

	// Token: 0x0400007A RID: 122
	private Tweener _tweenerClip;
}
