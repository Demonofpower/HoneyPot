using System;

// Token: 0x02000065 RID: 101
public class PuzzleStatusGiftZone : DisplayObject
{
	// Token: 0x06000313 RID: 787 RVA: 0x000048B6 File Offset: 0x00002AB6
	public override bool CanShowTooltip()
	{
		return GameManager.System.Puzzle.Game != null && GameManager.System.Puzzle.Game.puzzleGameState == PuzzleGameState.ITEM;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00004565 File Offset: 0x00002765
	public override string GetUniqueTooltipMessage()
	{
		return "Give to " + GameManager.System.Location.currentGirl.firstName;
	}
}
