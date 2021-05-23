using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class ButtonStateTransition
{
	// Token: 0x0600006A RID: 106 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
	public ButtonStateTransition(ButtonStateTransitionDef def, DisplayObject displayObj)
	{
		this._def = def;
		this._displayObject = displayObj;
		this._buttonObject = this._displayObject.button;
		this._childCount = this._displayObject.GetChildren(false).Length;
		this._legal = true;
		this._applied = false;
		switch (this._def.type)
		{
		case ButtonStateTransitionType.SPRITE:
			if (this._def.targetChild)
			{
				this._spriteTarget = (this._displayObject.GetChildByName(this._def.childName) as SpriteObject);
			}
			else
			{
				this._spriteTarget = this._displayObject.gameObj.GetComponent<SpriteObject>();
			}
			if (this._spriteTarget != null)
			{
				this._origSprite = this._spriteTarget.sprite.spriteId;
				this._stateSprite = this._spriteTarget.sprite.Collection.GetSpriteIdByName(this._def.spriteName);
			}
			else
			{
				this._legal = false;
			}
			break;
		case ButtonStateTransitionType.ALPHA:
			if (this._childCount <= 0 || !this._def.targetChild)
			{
				this._spriteTarget = this._displayObject.gameObj.GetComponent<SpriteObject>();
				if (this._spriteTarget != null)
				{
					this._origAlpha = this._spriteTarget.sprite.color.a;
				}
				else
				{
					this._legal = false;
				}
			}
			break;
		case ButtonStateTransitionType.LIGHTNESS:
			if (this._childCount <= 0 || !this._def.targetChild)
			{
				this._spriteTarget = this._displayObject.gameObj.GetComponent<SpriteObject>();
				if (this._spriteTarget == null)
				{
					this._legal = false;
				}
			}
			break;
		case ButtonStateTransitionType.FONT:
			if (this._def.targetChild)
			{
				this._labelTarget = (this._displayObject.GetChildByName(this._def.childName) as LabelObject);
			}
			else
			{
				this._labelTarget = this._displayObject.gameObj.GetComponent<LabelObject>();
			}
			if (this._labelTarget != null)
			{
				this._origFont = this._labelTarget.label.font;
			}
			else
			{
				this._legal = false;
			}
			break;
		case ButtonStateTransitionType.TEXT_COLOR:
			if (this._def.targetChild)
			{
				this._labelTarget = (this._displayObject.GetChildByName(this._def.childName) as LabelObject);
			}
			else
			{
				this._labelTarget = this._displayObject.gameObj.GetComponent<LabelObject>();
			}
			if (this._labelTarget != null)
			{
				this._origColor = this._labelTarget.label.color;
				this._stateColor = ColorUtils.HexToColor(this._def.hexColor);
			}
			else
			{
				this._legal = false;
			}
			break;
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0000D3FC File Offset: 0x0000B5FC
	public void Apply()
	{
		if (this._applied || !this._legal)
		{
			return;
		}
		float duration = this._def.duration;
		if (!this._buttonObject.IsStarted())
		{
			duration = 0f;
		}
		switch (this._def.type)
		{
		case ButtonStateTransitionType.SCALE:
			this._displayObject.SetLocalScale(1f + this._def.val, duration, EaseType.Linear);
			break;
		case ButtonStateTransitionType.SPRITE:
			this._spriteTarget.sprite.SetSprite(this._stateSprite);
			break;
		case ButtonStateTransitionType.ALPHA:
			if (this._childCount > 0 && this._def.targetChild)
			{
				this._displayObject.SetChildAlpha(this._def.val, duration);
			}
			else
			{
				this._spriteTarget.SetAlpha(this._def.val, duration);
			}
			break;
		case ButtonStateTransitionType.LIGHTNESS:
			if (this._childCount > 0 && this._def.targetChild)
			{
				this._displayObject.SetChildLightness(this._def.val, duration);
			}
			else
			{
				this._spriteTarget.SetLightness(this._def.val, duration);
			}
			break;
		case ButtonStateTransitionType.FONT:
			this._labelTarget.SetFont(this._def.font);
			break;
		case ButtonStateTransitionType.TEXT_COLOR:
			this._labelTarget.SetColor(this._stateColor);
			break;
		}
		this._applied = true;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000D590 File Offset: 0x0000B790
	public void Reverse()
	{
		if (!this._applied || !this._legal)
		{
			return;
		}
		float duration = this._def.duration;
		if (!this._buttonObject.IsStarted())
		{
			duration = 0f;
		}
		switch (this._def.type)
		{
		case ButtonStateTransitionType.SCALE:
			this._displayObject.SetLocalScale(1f, duration, EaseType.Linear);
			break;
		case ButtonStateTransitionType.SPRITE:
			this._spriteTarget.sprite.SetSprite(this._origSprite);
			break;
		case ButtonStateTransitionType.ALPHA:
			if (this._childCount > 0 && this._def.targetChild)
			{
				this._displayObject.SetChildAlpha(1f, duration);
			}
			else
			{
				this._spriteTarget.SetAlpha(this._origAlpha, duration);
			}
			break;
		case ButtonStateTransitionType.LIGHTNESS:
			if (this._childCount > 0 && this._def.targetChild)
			{
				this._displayObject.SetChildLightness(1f, duration);
			}
			else
			{
				this._spriteTarget.SetLightness(1f, duration);
			}
			break;
		case ButtonStateTransitionType.FONT:
			this._labelTarget.SetFont(this._origFont);
			break;
		case ButtonStateTransitionType.TEXT_COLOR:
			this._labelTarget.SetColor(this._origColor);
			break;
		}
		this._applied = false;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00002ADA File Offset: 0x00000CDA
	public void ChangeOrigSprite(string spriteName)
	{
		if (!this._legal || this._spriteTarget == null)
		{
			return;
		}
		this._origSprite = this._spriteTarget.sprite.Collection.GetSpriteIdByName(spriteName);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00002B15 File Offset: 0x00000D15
	public void ChangeStateSprite(string spriteName)
	{
		if (!this._legal || this._spriteTarget == null)
		{
			return;
		}
		this._stateSprite = this._spriteTarget.sprite.Collection.GetSpriteIdByName(spriteName);
	}

	// Token: 0x0400003E RID: 62
	private ButtonStateTransitionDef _def;

	// Token: 0x0400003F RID: 63
	private DisplayObject _displayObject;

	// Token: 0x04000040 RID: 64
	private ButtonObject _buttonObject;

	// Token: 0x04000041 RID: 65
	private int _childCount;

	// Token: 0x04000042 RID: 66
	private bool _legal;

	// Token: 0x04000043 RID: 67
	private bool _applied;

	// Token: 0x04000044 RID: 68
	private SpriteObject _spriteTarget;

	// Token: 0x04000045 RID: 69
	private int _origSprite;

	// Token: 0x04000046 RID: 70
	private int _stateSprite;

	// Token: 0x04000047 RID: 71
	private float _origAlpha;

	// Token: 0x04000048 RID: 72
	private LabelObject _labelTarget;

	// Token: 0x04000049 RID: 73
	private tk2dFontData _origFont;

	// Token: 0x0400004A RID: 74
	private Color _origColor;

	// Token: 0x0400004B RID: 75
	private Color _stateColor;
}
