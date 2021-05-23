using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000107 RID: 263
public class PuzzleMatchSet
{
	// Token: 0x060005BA RID: 1466 RVA: 0x00006546 File Offset: 0x00004746
	public PuzzleMatchSet()
	{
		this.matches = new List<PuzzleMatch>();
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002DC14 File Offset: 0x0002BE14
	public void AddMatch(PuzzleMatch match, bool highlight = false)
	{
		if (match != null)
		{
			PuzzleMatch matchWithMatchPositions = this.GetMatchWithMatchPositions(match);
			if (matchWithMatchPositions != null)
			{
				this.RemoveMatch(matchWithMatchPositions);
			}
			this.matches.Add(match);
			this.matches = (from element in this.matches
			orderby element.gridPositions[0].col, element.gridPositions[0].row
			select element).ToList<PuzzleMatch>();
			if (highlight)
			{
				match.HighlightTokens();
			}
		}
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00006559 File Offset: 0x00004759
	public void RemoveMatch(PuzzleMatch match)
	{
		if (this.matches.IndexOf(match) != -1)
		{
			this.matches.Remove(match);
			match.UnhighlightTokens();
		}
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002DCAC File Offset: 0x0002BEAC
	public bool ContainsTokenType(PuzzleTokenType type)
	{
		for (int i = 0; i < this.matches.Count; i++)
		{
			if (this.matches[i].ContainsTokenType(type))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0002DCF0 File Offset: 0x0002BEF0
	public bool ContainsToken(PuzzleTokenDefinition tokenDefinition, bool temp = false)
	{
		for (int i = 0; i < this.matches.Count; i++)
		{
			if (this.matches[i].ContainsToken(tokenDefinition, temp))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0002DD34 File Offset: 0x0002BF34
	public bool HasMatchWithMatchPositions(PuzzleMatch match)
	{
		for (int i = 0; i < match.gridPositions.Count; i++)
		{
			if (this.HasMatchWithGridPosition(match.gridPositions[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00006580 File Offset: 0x00004780
	public bool HasMatchWithGridPosition(PuzzleGridPosition gridPosition)
	{
		return this.GetMatchWithGridPosition(gridPosition) != null;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002DD78 File Offset: 0x0002BF78
	public PuzzleMatch GetMatchWithMatchPositions(PuzzleMatch match)
	{
		for (int i = 0; i < match.gridPositions.Count; i++)
		{
			PuzzleMatch matchWithGridPosition = this.GetMatchWithGridPosition(match.gridPositions[i]);
			if (matchWithGridPosition != null)
			{
				return matchWithGridPosition;
			}
		}
		return null;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
	public PuzzleMatch GetMatchWithGridPosition(PuzzleGridPosition gridPosition)
	{
		for (int i = 0; i < this.matches.Count; i++)
		{
			if (this.matches[i].gridPositions.IndexOf(gridPosition) != -1)
			{
				return this.matches[i];
			}
		}
		return null;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0000658F File Offset: 0x0000478F
	public int MatchCount()
	{
		return this.matches.Count;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0000659C File Offset: 0x0000479C
	public void ClearMatches()
	{
		while (this.MatchCount() > 0)
		{
			this.RemoveMatch(this.matches[this.MatchCount() - 1]);
		}
	}

	// Token: 0x04000735 RID: 1845
	public List<PuzzleMatch> matches;
}
