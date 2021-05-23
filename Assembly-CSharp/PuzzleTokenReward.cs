using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class PuzzleTokenReward
{
	// Token: 0x060005E8 RID: 1512 RVA: 0x000066F8 File Offset: 0x000048F8
	public PuzzleTokenReward(PuzzleTokenDefinition tokenDef, int level, int val)
	{
		this.tokenDefinition = tokenDef;
		this.tokenLevel = level;
		this.rewardValue = val;
		this._overrideType = false;
		this._tokenTypeOverride = PuzzleTokenType.AFFECTION;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00006723 File Offset: 0x00004923
	public void OverrideType(PuzzleTokenType tokenTypeOverride)
	{
		this._overrideType = true;
		this._tokenTypeOverride = tokenTypeOverride;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0002E3DC File Offset: 0x0002C5DC
	public string GetRewardPopText(bool includeUnits = true, bool bonusRound = false)
	{
		string text = string.Empty;
		if (this.rewardValue >= 0)
		{
			text += "+";
		}
		else
		{
			text += "-";
		}
		text += Mathf.Abs(this.rewardValue).ToString();
		if (includeUnits)
		{
			PuzzleTokenType puzzleTokenType = this.tokenDefinition.type;
			if (this._overrideType)
			{
				puzzleTokenType = this._tokenTypeOverride;
			}
			if (!bonusRound)
			{
				switch (puzzleTokenType)
				{
				case PuzzleTokenType.AFFECTION:
					text += " Affection";
					break;
				case PuzzleTokenType.PASSION:
					text += " Passion";
					break;
				case PuzzleTokenType.BROKEN:
					text += " Affection";
					break;
				case PuzzleTokenType.JOY:
					text = text + " " + StringUtils.Pluralize("Move", this.rewardValue);
					break;
				case PuzzleTokenType.SENTIMENT:
					text += " Sentiment";
					break;
				}
			}
			else
			{
				text += " Pleasure";
			}
		}
		return text;
	}

	// Token: 0x04000748 RID: 1864
	public PuzzleTokenDefinition tokenDefinition;

	// Token: 0x04000749 RID: 1865
	public int tokenLevel;

	// Token: 0x0400074A RID: 1866
	public int rewardValue;

	// Token: 0x0400074B RID: 1867
	private bool _overrideType;

	// Token: 0x0400074C RID: 1868
	private PuzzleTokenType _tokenTypeOverride;
}
