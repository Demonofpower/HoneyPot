using System;
using System.Collections.Generic;

// Token: 0x0200006A RID: 106
public class UIPuzzleStatus : DisplayObject
{
	// Token: 0x0600032B RID: 811 RVA: 0x0001C570 File Offset: 0x0001A770
	protected override void OnStart()
	{
		base.OnStart();
		this.affectionMeterContainer = base.GetChildByName("PuzzleStatusAffectionMeter");
		this.resourcesContainer = base.GetChildByName("PuzzleStatusResources");
		this.itemsContainer = base.GetChildByName("PuzzleStatusItems");
		this.puzzleEffectsContainer = base.GetChildByName("PuzzleStatusEffectsContainer");
		this.giftZone = (base.GetChildByName("PuzzleStatusGiftZone") as PuzzleStatusGiftZone);
		this.affectionMeterBarContainer = this.affectionMeterContainer.GetChildByName("PuzzleStatusAffectionMeterBarContainer");
		this.affectionMeterMask = this.affectionMeterBarContainer.gameObj.transform.FindChild("PuzzleStatusAffectionMeterMaskBar").GetComponent<Mask>();
		this.resourcePassionContainer = (this.resourcesContainer.GetChildByName("PuzzleStatusResourcePassion") as UIPuzzleStatusPassion);
		this.resourceMovesContainer = this.resourcesContainer.GetChildByName("PuzzleStatusResourceMoves");
		this.resourceSentimentContainer = this.resourcesContainer.GetChildByName("PuzzleStatusResourceSentiment");
		this.affectionLabel = (this.affectionMeterContainer.GetChildByName("PuzzleStatusAffectionMeterLabel") as LabelObject);
		this.passionLabel = (this.resourcePassionContainer.GetChildByName("PuzzleStatusResourcePassionLabel") as LabelObject);
		this.movesLabel = (this.resourceMovesContainer.GetChildByName("PuzzleStatusResourceMovesLabel") as LabelObject);
		this.sentimentLabel = (this.resourceSentimentContainer.GetChildByName("PuzzleStatusResourceSentimentLabel") as LabelObject);
		this.passionSubtitle = (this.resourcePassionContainer.GetChildByName("PuzzleStatusResourcePassionSubtitle") as LabelObject);
		this.movesSubtitle = (this.resourceMovesContainer.GetChildByName("PuzzleStatusResourceMovesSubtitle") as LabelObject);
		this.sentimentSubtitle = (this.resourceSentimentContainer.GetChildByName("PuzzleStatusResourceSentimentSubtitle") as LabelObject);
		this.passionMax = (this.resourcePassionContainer.GetChildByName("PuzzleStatusResourcePassionMax") as SpriteObject);
		this.movesMax = (this.resourceMovesContainer.GetChildByName("PuzzleStatusResourceMovesMax") as SpriteObject);
		this.sentimentMax = (this.resourceSentimentContainer.GetChildByName("PuzzleStatusResourceSentimentMax") as SpriteObject);
		this.itemSlots = new List<PuzzleStatusItemSlot>();
		for (int i = 0; i < 6; i++)
		{
			PuzzleStatusItemSlot puzzleStatusItemSlot = this.itemsContainer.GetChildByName("PuzzleStatusItem" + i.ToString()) as PuzzleStatusItemSlot;
			puzzleStatusItemSlot.Init(i);
			this.itemSlots.Add(puzzleStatusItemSlot);
		}
		this.puzzleEffectSlots = new List<PuzzleStatusEffectSlot>();
		for (int j = 0; j < 6; j++)
		{
			PuzzleStatusEffectSlot puzzleStatusEffectSlot = this.puzzleEffectsContainer.GetChildByName("PuzzleStatusEffect" + j.ToString()) as PuzzleStatusEffectSlot;
			puzzleStatusEffectSlot.Init();
			this.puzzleEffectSlots.Add(puzzleStatusEffectSlot);
		}
		this._affectionMeterBar = (this.affectionMeterBarContainer.GetChildByName("PuzzleStatusAffectionMeterBar") as SpriteObject);
		base.localY = -10f;
		this.giftZone.gameObj.SetActive(false);
		this._passionLevel = 0;
		this._meterBarTimestamp = 0f;
		base.interactive = false;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001C860 File Offset: 0x0001AA60
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (GameManager.System.GameState == GameState.PUZZLE && GameManager.System.Lifetime(true) - this._meterBarTimestamp >= 0.01f)
		{
			this._meterBarTimestamp = GameManager.System.Lifetime(true);
			this._affectionMeterBar.localX -= 2f;
			if (this._affectionMeterBar.localX <= -529f)
			{
				this._affectionMeterBar.localX = 0f;
			}
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0001C8EC File Offset: 0x0001AAEC
	public void SetPassionLevel(int passionLevel)
	{
		this._passionLevel = passionLevel;
		this.passionLabel.SetText(string.Concat(new string[]
		{
			"Lvl ",
			this._passionLevel.ToString(),
			"^CFFFFFF80 (+",
			((GameManager.System.Puzzle.GetPassionLevelMultiplier(this._passionLevel) - 1f) * 100f).ToString(),
			"%)"
		}));
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0001C968 File Offset: 0x0001AB68
	public void UpdatePuzzleEffects(List<AbilityBehaviorDefinition> puzzleEffects)
	{
		for (int i = 0; i < this.puzzleEffectSlots.Count; i++)
		{
			if (puzzleEffects != null && puzzleEffects.Count > i)
			{
				this.puzzleEffectSlots[i].PopuatePuzzleEffectItem(puzzleEffects[i].puzzleEffectItemRef);
			}
			else
			{
				this.puzzleEffectSlots[i].PopuatePuzzleEffectItem(null);
			}
		}
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00003C59 File Offset: 0x00001E59
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x040002E9 RID: 745
	public const string ITEM_BLANK_SPRITE = "item_blank";

	// Token: 0x040002EA RID: 746
	public const int AFFECTION_METER_BAR_WIDTH = 529;

	// Token: 0x040002EB RID: 747
	public const int AFFECTION_METER_MASK_WIDTH = 452;

	// Token: 0x040002EC RID: 748
	public tk2dSpriteCollectionData itemsSpriteCollection;

	// Token: 0x040002ED RID: 749
	public LocationDefinition bonusRoundLocation;

	// Token: 0x040002EE RID: 750
	public LocationDefinition postBonusRoundLocation;

	// Token: 0x040002EF RID: 751
	public DisplayObject affectionMeterContainer;

	// Token: 0x040002F0 RID: 752
	public DisplayObject resourcesContainer;

	// Token: 0x040002F1 RID: 753
	public DisplayObject itemsContainer;

	// Token: 0x040002F2 RID: 754
	public DisplayObject puzzleEffectsContainer;

	// Token: 0x040002F3 RID: 755
	public PuzzleStatusGiftZone giftZone;

	// Token: 0x040002F4 RID: 756
	public DisplayObject affectionMeterBarContainer;

	// Token: 0x040002F5 RID: 757
	public Mask affectionMeterMask;

	// Token: 0x040002F6 RID: 758
	public UIPuzzleStatusPassion resourcePassionContainer;

	// Token: 0x040002F7 RID: 759
	public DisplayObject resourceMovesContainer;

	// Token: 0x040002F8 RID: 760
	public DisplayObject resourceSentimentContainer;

	// Token: 0x040002F9 RID: 761
	public LabelObject affectionLabel;

	// Token: 0x040002FA RID: 762
	public LabelObject passionLabel;

	// Token: 0x040002FB RID: 763
	public LabelObject movesLabel;

	// Token: 0x040002FC RID: 764
	public LabelObject sentimentLabel;

	// Token: 0x040002FD RID: 765
	public LabelObject passionSubtitle;

	// Token: 0x040002FE RID: 766
	public LabelObject movesSubtitle;

	// Token: 0x040002FF RID: 767
	public LabelObject sentimentSubtitle;

	// Token: 0x04000300 RID: 768
	public SpriteObject passionMax;

	// Token: 0x04000301 RID: 769
	public SpriteObject movesMax;

	// Token: 0x04000302 RID: 770
	public SpriteObject sentimentMax;

	// Token: 0x04000303 RID: 771
	public List<PuzzleStatusItemSlot> itemSlots;

	// Token: 0x04000304 RID: 772
	public List<PuzzleStatusEffectSlot> puzzleEffectSlots;

	// Token: 0x04000305 RID: 773
	private int _passionLevel;

	// Token: 0x04000306 RID: 774
	private SpriteObject _affectionMeterBar;

	// Token: 0x04000307 RID: 775
	private float _meterBarTimestamp;
}
