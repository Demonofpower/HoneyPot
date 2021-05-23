using System;

// Token: 0x02000069 RID: 105
public class UIPuzzleGrid : DisplayObject
{
	// Token: 0x06000325 RID: 805 RVA: 0x0001C344 File Offset: 0x0001A544
	protected override void OnStart()
	{
		base.OnStart();
		this.gridBackground = (base.GetChildByName("PuzzleGridBackground") as SpriteObject);
		this.gridBorder = (base.GetChildByName("PuzzleGridBorder") as SpriteObject);
		this.targetPrompt = (base.GetChildByName("PuzzleGridTargetPrompt") as SpriteObject);
		this.tokenContainer = base.GetChildByName("PuzzleTokenContainer");
		this.slideGuideHorizontal = (base.GetChildByName("PuzzleGridSlideGuideHorizontal") as SpriteObject);
		this.slideGuideVertical = (base.GetChildByName("PuzzleGridSlideGuideVertical") as SpriteObject);
		this.notifierContainer = base.GetChildByName("PuzzleGridNotifiers");
		this.notifier = (this.notifierContainer.GetChildByName("PuzzleGridNotifier") as SpriteObject);
		this.notifierBurst = (this.notifierContainer.GetChildByName("PuzzleGridNotifierBurst") as SpriteObject);
		this._slideGuideHorizontalBaseY = this.slideGuideHorizontal.gameObj.transform.localPosition.y;
		this._slideGuideVerticalBaseX = this.slideGuideVertical.gameObj.transform.localPosition.x;
		this.gridBackground.SetAlpha(0f, 0f);
		this.gridBorder.SetAlpha(0f, 0f);
		this.targetPrompt.SetAlpha(0f, 0f);
		this.slideGuideHorizontal.SetAlpha(0f, 0f);
		this.slideGuideVertical.SetAlpha(0f, 0f);
		this.notifier.SetAlpha(0f, 0f);
		this.notifierBurst.SetAlpha(0f, 0f);
		this.gameObj.SetActive(false);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0001C504 File Offset: 0x0001A704
	public void ShowSlideGuides(int row, int col)
	{
		this.slideGuideHorizontal.localY = this._slideGuideHorizontalBaseY - (float)row * 82f;
		this.slideGuideVertical.localX = this._slideGuideVerticalBaseX + (float)col * 82f;
		this.slideGuideHorizontal.SetAlpha(0.5f, 0.2f);
		this.slideGuideVertical.SetAlpha(0.5f, 0.2f);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000049B0 File Offset: 0x00002BB0
	public void HideSlideGuides()
	{
		this.slideGuideHorizontal.SetAlpha(0f, 0.2f);
		this.slideGuideVertical.SetAlpha(0f, 0.2f);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000049DC File Offset: 0x00002BDC
	public void ShowTargetPrompt()
	{
		this.targetPrompt.SetAlpha(1f, 0.2f);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x000049F3 File Offset: 0x00002BF3
	public void HideTargetPrompt()
	{
		this.targetPrompt.SetAlpha(0f, 0.2f);
	}

	// Token: 0x040002D0 RID: 720
	public const string PUZZLE_TOKEN_BLANK_SPRITE = "puzzle_token_blank";

	// Token: 0x040002D1 RID: 721
	public tk2dSpriteCollectionData puzzleTokenSpriteCollection;

	// Token: 0x040002D2 RID: 722
	public AudioDefinition tokenPickupSound;

	// Token: 0x040002D3 RID: 723
	public AudioDefinition badMoveSound;

	// Token: 0x040002D4 RID: 724
	public AudioDefinition successSound;

	// Token: 0x040002D5 RID: 725
	public AudioDefinition failureSound;

	// Token: 0x040002D6 RID: 726
	public AudioDefinition successJackpotSound;

	// Token: 0x040002D7 RID: 727
	public AudioDefinition failureJackpotSound;

	// Token: 0x040002D8 RID: 728
	public ParticleEmitter2DDefinition notifierEffectEmitter;

	// Token: 0x040002D9 RID: 729
	public SpriteGroupDefinition notifierEffectSpriteGroup;

	// Token: 0x040002DA RID: 730
	public tk2dFontData jackpotEffectFont;

	// Token: 0x040002DB RID: 731
	public EnergyTrailDefinition jackpotEnergyTrail;

	// Token: 0x040002DC RID: 732
	public AudioGroup tokenMatchSounds;

	// Token: 0x040002DD RID: 733
	public AudioGroup tokenPowerMatchSounds;

	// Token: 0x040002DE RID: 734
	public SpriteObject gridBackground;

	// Token: 0x040002DF RID: 735
	public SpriteObject gridBorder;

	// Token: 0x040002E0 RID: 736
	public SpriteObject targetPrompt;

	// Token: 0x040002E1 RID: 737
	public DisplayObject tokenContainer;

	// Token: 0x040002E2 RID: 738
	public DisplayObject notifierContainer;

	// Token: 0x040002E3 RID: 739
	public SpriteObject notifier;

	// Token: 0x040002E4 RID: 740
	public SpriteObject notifierBurst;

	// Token: 0x040002E5 RID: 741
	public SpriteObject slideGuideVertical;

	// Token: 0x040002E6 RID: 742
	public SpriteObject slideGuideHorizontal;

	// Token: 0x040002E7 RID: 743
	private float _slideGuideHorizontalBaseY;

	// Token: 0x040002E8 RID: 744
	private float _slideGuideVerticalBaseX;
}
