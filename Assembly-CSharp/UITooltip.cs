using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class UITooltip : DisplayObject
{
	// Token: 0x0600037E RID: 894 RVA: 0x00004DD6 File Offset: 0x00002FD6
	protected override void OnStart()
	{
		base.OnStart();
		this._showing = false;
		this._hiding = false;
		this.backgroundSprite = (base.GetChildByName("TooltipBackground") as SlicedSpriteObject);
		this.contentContainer = base.GetChildByName("TooltipContent");
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0001F570 File Offset: 0x0001D770
	public void ShowTooltip(TooltipObject tooltipObject)
	{
		TweenUtils.KillSequence(this._tooltipSequence, false);
		if (this._showing)
		{
			this.OnTooltipHidden();
		}
		this._tooltipSource = tooltipObject.GetTooltipSource();
		this._tooltipContent = (UnityEngine.Object.Instantiate(tooltipObject.contentPrefab) as GameObject).GetComponent<UITooltipContent>();
		this._tooltipContent.Init(tooltipObject);
		this.contentContainer.AddChild(this._tooltipContent);
		this._tooltipContent.SetLocalPosition(0f, 0f);
		if (this._tooltipContent.customTooltipSprite.Trim() != string.Empty)
		{
			this.backgroundSprite.sprite.SetSprite(this._tooltipContent.customTooltipSprite);
		}
		else
		{
			this.backgroundSprite.sprite.SetSprite("tooltip_background");
		}
		DisplayObject childByName = this._tooltipSource.GetChildByName(tooltipObject.layeredInChild);
		if (childByName != null)
		{
			childByName.AddChild(this);
		}
		else
		{
			GameManager.Stage.effects.AddTooltip(this, this._tooltipSource);
		}
		if (tooltipObject.mouseAttahced)
		{
			GameManager.System.Cursor.AttachObject(this, (float)tooltipObject.xOffset, (float)tooltipObject.yOffset);
		}
		else
		{
			base.SetGlobalPosition(this._tooltipSource.globalX + (float)tooltipObject.xOffset, this._tooltipSource.globalY + (float)tooltipObject.yOffset);
		}
		if (tooltipObject.direction == TooltipDirection.UP || tooltipObject.direction == TooltipDirection.DOWN)
		{
			this.backgroundSprite.sprite.dimensions = new Vector2((float)(this._tooltipContent.width + this._tooltipContent.GetContentWidthPadding()), (float)(this._tooltipContent.height + this._tooltipContent.GetContentHeightPadding()));
		}
		else
		{
			this.backgroundSprite.sprite.dimensions = new Vector2((float)Mathf.RoundToInt((float)(this._tooltipContent.height + this._tooltipContent.GetContentHeightPadding()) * 0.95f), (float)Mathf.RoundToInt((float)(this._tooltipContent.width + this._tooltipContent.GetContentWidthPadding()) * 1.05f));
		}
		switch (tooltipObject.direction)
		{
		case TooltipDirection.UP:
			this.backgroundSprite.gameObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			if (this._tooltipContent.centered)
			{
				this.contentContainer.SetLocalPosition(-4f, (float)Mathf.RoundToInt(this.backgroundSprite.sprite.dimensions.y / 2f + 2f));
			}
			else
			{
				this.contentContainer.SetLocalPosition((float)Mathf.RoundToInt(-(this.backgroundSprite.sprite.dimensions.x / 2f) + 26f), (float)Mathf.RoundToInt(this.backgroundSprite.sprite.dimensions.y - 24f));
			}
			break;
		case TooltipDirection.RIGHT:
			this.backgroundSprite.gameObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));
			this.contentContainer.SetLocalPosition((float)Mathf.RoundToInt(this.backgroundSprite.sprite.dimensions.y / 2f + 8f), -4f);
			break;
		case TooltipDirection.DOWN:
			this.backgroundSprite.gameObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
			if (this._tooltipContent.centered)
			{
				this.contentContainer.SetLocalPosition(-4f, (float)Mathf.RoundToInt(-(this.backgroundSprite.sprite.dimensions.y / 2f) - 11f));
			}
			else
			{
				this.contentContainer.SetLocalPosition((float)Mathf.RoundToInt(-(this.backgroundSprite.sprite.dimensions.x / 2f) + 26f), -37f);
			}
			break;
		case TooltipDirection.LEFT:
			this.backgroundSprite.gameObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
			this.contentContainer.SetLocalPosition((float)Mathf.RoundToInt(-(this.backgroundSprite.sprite.dimensions.y / 2f) - 8f), -4f);
			break;
		}
		base.childrenAlpha = 0f;
		base.SetLocalScale(0.5f, 0f, EaseType.Linear);
		this._tooltipSequence = new Sequence();
		this._tooltipSequence.Insert(0f, HOTween.To(this, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.EaseOutSine)));
		this._tooltipSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this._tooltipSequence.Play();
		this._showing = true;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
	public void HideTooltip()
	{
		TweenUtils.KillSequence(this._tooltipSequence, false);
		this._tooltipSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnTooltipHidden)));
		this._tooltipSequence.Insert(0f, HOTween.To(this, 0.125f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseOutSine)));
		this._tooltipSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.125f, new TweenParms().Prop("localScale", new Vector3(0.5f, 0.5f, 1f)).Ease(EaseType.EaseOutSine)));
		this._tooltipSequence.Play();
		this._hiding = true;
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0001FBCC File Offset: 0x0001DDCC
	private void OnTooltipHidden()
	{
		GameManager.System.Cursor.DetachObject(this);
		this.contentContainer.RemoveChild(this._tooltipContent, true);
		this._tooltipContent = null;
		base.SetLocalScale(1f, 0f, EaseType.Linear);
		this.backgroundSprite.gameObj.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		base.RemoveSelf();
		this._tooltipSource = null;
		this._showing = false;
		this._hiding = false;
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0001FC5C File Offset: 0x0001DE5C
	public void Refresh()
	{
		if (this._showing && !this._hiding)
		{
			if (!this._tooltipSource.CanShowTooltip() || !this._tooltipSource.tooltip.IsEnabled() || this._tooltipSource.tooltip == null || this._tooltipSource == null)
			{
				this._tooltipSource.tooltip.HideTooltip();
			}
			else if (this._tooltipContent != null)
			{
				this._tooltipContent.Refresh();
			}
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00004E13 File Offset: 0x00003013
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00004E27 File Offset: 0x00003027
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00004E3B File Offset: 0x0000303B
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._tooltipSequence, false);
		this._tooltipSequence = null;
	}

	// Token: 0x04000354 RID: 852
	private const string DEFAULT_TOOLTIP_SPRITE_NAME = "tooltip_background";

	// Token: 0x04000355 RID: 853
	private SlicedSpriteObject backgroundSprite;

	// Token: 0x04000356 RID: 854
	private DisplayObject contentContainer;

	// Token: 0x04000357 RID: 855
	private bool _showing;

	// Token: 0x04000358 RID: 856
	private bool _hiding;

	// Token: 0x04000359 RID: 857
	private DisplayObject _tooltipSource;

	// Token: 0x0400035A RID: 858
	private Sequence _tooltipSequence;

	// Token: 0x0400035B RID: 859
	private UITooltipContent _tooltipContent;
}
