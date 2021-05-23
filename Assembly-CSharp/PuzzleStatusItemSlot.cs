using System;

// Token: 0x02000066 RID: 102
public class PuzzleStatusItemSlot : DisplayObject
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000316 RID: 790 RVA: 0x000048E9 File Offset: 0x00002AE9
	// (remove) Token: 0x06000317 RID: 791 RVA: 0x00004902 File Offset: 0x00002B02
	public event PuzzleStatusItemSlot.PuzzleStatusItemSlotDelegate PuzzleStatusItemSlotDownEvent;

	// Token: 0x06000318 RID: 792 RVA: 0x0001C210 File Offset: 0x0001A410
	public void Init(int itemSlotIndex)
	{
		this.background = (base.GetChildByName("PuzzleStatusItemBackground") as SpriteObject);
		this.itemIcon = (base.GetChildByName("PuzzleStatusItemIcon") as SpriteObject);
		base.button.ButtonDownEvent += this.OnButtonDown;
		this.slotIndex = itemSlotIndex;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0000491B File Offset: 0x00002B1B
	public void PopulateSlotItem()
	{
		this.itemDefinition = GameManager.System.Player.dateGifts[this.slotIndex].itemDefinition;
		this.RefreshSlotItem();
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001C268 File Offset: 0x0001A468
	public void RefreshSlotItem()
	{
		if (this.itemDefinition != null)
		{
			this.itemIcon.sprite.SetSprite(this.itemDefinition.iconName);
		}
		else
		{
			this.itemIcon.sprite.SetSprite("item_blank");
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00004944 File Offset: 0x00002B44
	private void OnButtonDown(ButtonObject buttonObject)
	{
		if (this.PuzzleStatusItemSlotDownEvent != null)
		{
			this.PuzzleStatusItemSlotDownEvent(this);
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
	public override bool CanShowTooltip()
	{
		return !(this.itemDefinition == null) && GameManager.System.Puzzle.Game != null && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.FINISHED && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.COMPLETE && !GameManager.System.Puzzle.Game.isBonusRound;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000495D File Offset: 0x00002B5D
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonDownEvent -= this.OnButtonDown;
	}

	// Token: 0x040002C9 RID: 713
	public SpriteObject background;

	// Token: 0x040002CA RID: 714
	public SpriteObject itemIcon;

	// Token: 0x040002CB RID: 715
	public int slotIndex;

	// Token: 0x040002CC RID: 716
	public ItemDefinition itemDefinition;

	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x0600031F RID: 799
	public delegate void PuzzleStatusItemSlotDelegate(PuzzleStatusItemSlot itemSlot);
}
