using System;

// Token: 0x0200006B RID: 107
public class UIPuzzleStatusPassion : DisplayObject
{
	// Token: 0x06000331 RID: 817 RVA: 0x0001C9E8 File Offset: 0x0001ABE8
	public override bool CanShowTooltip()
	{
		return GameManager.System.Puzzle.Game != null && GameManager.System.Puzzle.Game.currentPassionLevel < GameManager.System.Puzzle.GetMaxPassionLevel() && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.FINISHED && GameManager.System.Puzzle.Game.puzzleGameState != PuzzleGameState.COMPLETE && !GameManager.System.Puzzle.Game.isBonusRound;
	}
}
