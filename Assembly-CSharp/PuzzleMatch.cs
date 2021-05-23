using System;
using System.Collections.Generic;

// Token: 0x02000106 RID: 262
public class PuzzleMatch
{
	// Token: 0x060005B4 RID: 1460 RVA: 0x000064EA File Offset: 0x000046EA
	public PuzzleMatch(PuzzleGridPosition matchPosition, List<PuzzleGridPosition> matchPositions, PuzzleAxis matchAxis)
	{
		this.basePosition = matchPosition;
		this.gridPositions = matchPositions;
		this.axis = matchAxis;
		this.hightlightedTokens = new List<PuzzleToken>();
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00006512 File Offset: 0x00004712
	public bool ShouldUpgradeBase()
	{
		return this.gridPositions.Count > 2 && this.basePosition.GetToken(false).definition.levels.Count > 1;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0002DA9C File Offset: 0x0002BC9C
	public bool ContainsTokenType(PuzzleTokenType type)
	{
		for (int i = 0; i < this.gridPositions.Count; i++)
		{
			if (this.gridPositions[i].GetToken(false).definition.type == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0002DAEC File Offset: 0x0002BCEC
	public bool ContainsToken(PuzzleTokenDefinition tokenDefinition, bool temp = false)
	{
		for (int i = 0; i < this.gridPositions.Count; i++)
		{
			if (this.gridPositions[i].GetToken(temp).definition == tokenDefinition)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0002DB3C File Offset: 0x0002BD3C
	public void HighlightTokens()
	{
		if (this.hightlightedTokens.Count > 0)
		{
			this.UnhighlightTokens();
		}
		for (int i = 0; i < this.gridPositions.Count; i++)
		{
			PuzzleToken token = this.gridPositions[i].GetToken(true);
			if (token != null && token != GameManager.System.Puzzle.Game.currentMoveToken)
			{
				token.HighlightToken();
				this.hightlightedTokens.Add(token);
			}
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0002DBCC File Offset: 0x0002BDCC
	public void UnhighlightTokens()
	{
		for (int i = 0; i < this.hightlightedTokens.Count; i++)
		{
			this.hightlightedTokens[i].UnhighlightToken();
		}
		this.hightlightedTokens.Clear();
	}

	// Token: 0x04000731 RID: 1841
	public PuzzleGridPosition basePosition;

	// Token: 0x04000732 RID: 1842
	public List<PuzzleGridPosition> gridPositions;

	// Token: 0x04000733 RID: 1843
	public PuzzleAxis axis;

	// Token: 0x04000734 RID: 1844
	public List<PuzzleToken> hightlightedTokens;
}
