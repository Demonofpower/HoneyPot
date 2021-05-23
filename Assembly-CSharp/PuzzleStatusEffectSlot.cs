using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class PuzzleStatusEffectSlot : DisplayObject
{
	// Token: 0x06000309 RID: 777 RVA: 0x000047BD File Offset: 0x000029BD
	public void Init()
	{
		this.background = (base.GetChildByName("PuzzleStatusEffectBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("PuzzleStatusEffectIcon") as SpriteObject);
		this._isShowing = true;
		this.Hide(false);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001BEDC File Offset: 0x0001A0DC
	public void PopuatePuzzleEffectItem(ItemDefinition itemDefinition)
	{
		this._itemDefinition = itemDefinition;
		if (this._itemDefinition != null)
		{
			this.itemIcon.sprite.SetSprite(this._itemDefinition.iconName);
			this.Show(true);
		}
		else
		{
			this.itemIcon.sprite.SetSprite("item_blank");
			this.Hide(true);
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001BF48 File Offset: 0x0001A148
	private void Show(bool animate = false)
	{
		if (this._isShowing)
		{
			return;
		}
		this._isShowing = true;
		TweenUtils.KillSequence(this._showHideSequence, false);
		if (animate)
		{
			base.SetChildAlpha(0f, 0f);
			base.SetLocalScale(0.2f, 0f, EaseType.Linear);
			this._showHideSequence = new Sequence();
			this._showHideSequence.Insert(0f, HOTween.To(this, 0.5f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			this._showHideSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.5f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
			this._showHideSequence.Play();
		}
		else
		{
			base.SetChildAlpha(1f, 0f);
			base.SetLocalScale(1f, 0f, EaseType.Linear);
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001C064 File Offset: 0x0001A264
	private void Hide(bool animate = false)
	{
		if (!this._isShowing)
		{
			return;
		}
		this._isShowing = false;
		TweenUtils.KillSequence(this._showHideSequence, false);
		if (animate)
		{
			base.SetChildAlpha(1f, 0f);
			base.SetLocalScale(1f, 0f, EaseType.Linear);
			this._showHideSequence = new Sequence();
			this._showHideSequence.Insert(0f, HOTween.To(this, 0.5f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			this._showHideSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.5f, new TweenParms().Prop("localScale", new Vector3(0.2f, 0.2f, 1f)).Ease(EaseType.EaseInBack)));
			this._showHideSequence.Play();
		}
		else
		{
			base.SetChildAlpha(0f, 0f);
			base.SetLocalScale(0.2f, 0f, EaseType.Linear);
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0001C180 File Offset: 0x0001A380
	public override bool CanShowTooltip()
	{
		return this._isShowing && !(this._itemDefinition == null) && GameManager.System.Puzzle.Game != null && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.FINISHED && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.COMPLETE && !GameManager.System.Puzzle.Game.isBonusRound;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000047F9 File Offset: 0x000029F9
	public override string GetUniqueTooltipMessage()
	{
		if (this._itemDefinition != null)
		{
			return StringUtils.FlattenColorBunches(this._itemDefinition.description, "545454");
		}
		return string.Empty;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00004827 File Offset: 0x00002A27
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._showHideSequence != null && !this._showHideSequence.isPaused)
		{
			this._showHideSequence.Pause();
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00004861 File Offset: 0x00002A61
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._showHideSequence != null && this._showHideSequence.isPaused)
		{
			this._showHideSequence.Play();
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000489B File Offset: 0x00002A9B
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._showHideSequence, false);
		this._showHideSequence = null;
	}

	// Token: 0x040002C4 RID: 708
	public SpriteObject background;

	// Token: 0x040002C5 RID: 709
	public SpriteObject itemIcon;

	// Token: 0x040002C6 RID: 710
	private bool _isShowing;

	// Token: 0x040002C7 RID: 711
	public ItemDefinition _itemDefinition;

	// Token: 0x040002C8 RID: 712
	private Sequence _showHideSequence;
}
