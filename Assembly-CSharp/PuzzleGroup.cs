using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class PuzzleGroup
{
	// Token: 0x0600057C RID: 1404 RVA: 0x00006297 File Offset: 0x00004497
	public PuzzleGroup(List<PuzzleGridPosition> positions)
	{
		this.gridPositions = positions;
		this.protectedPositions = new List<PuzzleGridPosition>();
		this.SortGroup();
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x000062B7 File Offset: 0x000044B7
	public void ProtectPosition(PuzzleGridPosition position)
	{
		if (this.gridPositions.IndexOf(position) != -1)
		{
			this.protectedPositions.Add(position);
		}
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x000062D7 File Offset: 0x000044D7
	public void ShuffleGroup()
	{
		ListUtils.Shuffle<PuzzleGridPosition>(this.gridPositions);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0002B044 File Offset: 0x00029244
	public void SortGroup()
	{
		this.gridPositions = (from element in this.gridPositions
		orderby element.col, element.row
		select element).ToList<PuzzleGridPosition>();
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000062E4 File Offset: 0x000044E4
	public void EnforceLimit(int limit)
	{
		if (this.gridPositions.Count <= limit)
		{
			return;
		}
		this.ShuffleGroup();
		this.gridPositions.RemoveRange(0, this.gridPositions.Count - limit);
		this.SortGroup();
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0002B0A8 File Offset: 0x000292A8
	public List<PuzzleTokenDefinition> GetTokenTypesInGroup()
	{
		List<PuzzleTokenDefinition> list = new List<PuzzleTokenDefinition>();
		for (int i = 0; i < this.gridPositions.Count; i++)
		{
			PuzzleToken token = this.gridPositions[i].GetToken(false);
			if (token != null && !list.Contains(token.definition))
			{
				list.Add(token.definition);
			}
		}
		return list;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0002B114 File Offset: 0x00029314
	public Dictionary<PuzzleGridPosition, PuzzleTokenReward> GetGroupRewards(AbilityBehaviorDefinition switchMatchTypePuzzleEffect = null)
	{
		Dictionary<PuzzleGridPosition, PuzzleTokenReward> dictionary = new Dictionary<PuzzleGridPosition, PuzzleTokenReward>();
		List<List<PuzzleGridPosition>> positionChains = this.GetPositionChains();
		float num = 0f;
		for (int i = 0; i < positionChains.Count; i++)
		{
			List<PuzzleGridPosition> list = positionChains[i];
			PuzzleTokenDefinition puzzleTokenDefinition = list[0].GetToken(false).definition;
			if (puzzleTokenDefinition.type == PuzzleTokenType.AFFECTION)
			{
				for (int j = 0; j < list.Count; j++)
				{
					float rewardMultiplier = list[j].GetToken(false).GetRewardMultiplier();
					if (rewardMultiplier > 1f)
					{
						num += rewardMultiplier;
					}
				}
			}
		}
		for (int k = 0; k < positionChains.Count; k++)
		{
			List<PuzzleGridPosition> list = positionChains[k];
			PuzzleTokenDefinition puzzleTokenDefinition = list[0].GetToken(false).definition;
			if (switchMatchTypePuzzleEffect != null && puzzleTokenDefinition.type == switchMatchTypePuzzleEffect.puzzleTokenType)
			{
				puzzleTokenDefinition = switchMatchTypePuzzleEffect.tokenDefinition;
			}
			int num2 = list.Count;
			if (!GameManager.System.Puzzle.Game.isBonusRound)
			{
				switch (puzzleTokenDefinition.type)
				{
				case PuzzleTokenType.AFFECTION:
					num2 *= Mathf.Max(list.Count - 2, 1);
					num2 = (int)Mathf.Ceil((float)num2 * GameManager.System.Puzzle.GetAffectionTraitLevelMultiplier(puzzleTokenDefinition.linkedTrait.traitType));
					num2 = (int)Mathf.Ceil((float)num2 * GameManager.System.Puzzle.GetPassionLevelMultiplier(GameManager.System.Puzzle.Game.currentPassionLevel));
					num2 = (int)Mathf.Ceil((float)num2 * Mathf.Max(num, 1f));
					if (puzzleTokenDefinition.linkedTrait == GameManager.System.Location.currentGirl.mostDesiredTrait)
					{
						num2 = Mathf.RoundToInt((float)(num2 * 2));
					}
					else if (puzzleTokenDefinition.linkedTrait == GameManager.System.Location.currentGirl.leastDesiredTrait)
					{
						num2 = Mathf.RoundToInt((float)num2 * 0.5f);
					}
					break;
				case PuzzleTokenType.PASSION:
					num2 *= Mathf.Max(list.Count - 2, 1);
					num2 = (int)Mathf.Ceil((float)num2 * GameManager.System.Puzzle.GetPassionTraitLevelMultiplier());
					break;
				case PuzzleTokenType.BROKEN:
					num2 = Mathf.RoundToInt(Mathf.Min((float)num2 * GameManager.System.Puzzle.GetSensitivityTraitLevelMultiplier(), 1f) * (float)GameManager.System.Puzzle.Game.GetResourceValue(PuzzleGameResourceType.AFFECTION));
					if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).lovePotionUsed)
					{
						num2 = 0;
					}
					break;
				case PuzzleTokenType.JOY:
					num2 = Mathf.Max(num2 - 2, 0);
					break;
				case PuzzleTokenType.SENTIMENT:
					num2 *= Mathf.Max(list.Count - 2, 1);
					break;
				}
				if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).lovePotionUsed)
				{
					num2 *= 2;
				}
			}
			else
			{
				num2 *= 10;
			}
			List<PuzzleGridPosition> list2 = ListUtils.Copy<PuzzleGridPosition>(list);
            var puzzleGridPositions = from gridPosition in list2
                orderby gridPosition.row + gridPosition.col
                select gridPosition;
            list.Clear();
			while (list2.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list2.Count / 2));
				list.Add(list2[index]);
				list2.RemoveAt(index);
			}
			for (int l = 0; l < list.Count; l++)
			{
				if (num2 <= 0)
				{
					break;
				}
				int num3 = Mathf.Max(Mathf.RoundToInt((float)(num2 / (list.Count - l))), 1);
				num2 -= num3;
				if (puzzleTokenDefinition.negateResource)
				{
					num3 *= -1;
				}
				dictionary.Add(list[l], new PuzzleTokenReward(puzzleTokenDefinition, list[l].GetToken(false).level, num3));
			}
		}
		return dictionary;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0002B544 File Offset: 0x00029744
	private List<List<PuzzleGridPosition>> GetPositionChains()
	{
		List<List<PuzzleGridPosition>> list = new List<List<PuzzleGridPosition>>();
		List<PuzzleGridPosition> list2 = new List<PuzzleGridPosition>();
		for (int i = 0; i < this.gridPositions.Count; i++)
		{
			if (!list2.Contains(this.gridPositions[i]))
			{
				List<PuzzleGridPosition> list3;
				if (this.gridPositions[i].GetToken(false).definition.type == PuzzleTokenType.JOY)
				{
					list3 = this.gridPositions.FindAll((PuzzleGridPosition position) => position.GetToken(false).definition.type == PuzzleTokenType.JOY);
				}
				else
				{
					list3 = this.GetChainedPositions(this.gridPositions[i], new List<PuzzleGridPosition>
					{
						this.gridPositions[i]
					});
				}
				list.Add(list3);
				list2.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002B624 File Offset: 0x00029824
	private List<PuzzleGridPosition> GetChainedPositions(PuzzleGridPosition gridPosition, List<PuzzleGridPosition> ignorePositions)
	{
		List<PuzzleGridPosition> list = new List<PuzzleGridPosition>();
		list.Add(gridPosition);
		List<PuzzleGridPosition> list2 = this.gridPositions.FindAll((PuzzleGridPosition position) => position.GetToken(false).definition == gridPosition.GetToken(false).definition && ((Mathf.Abs(position.row - gridPosition.row) == 1 && position.col == gridPosition.col) || (Mathf.Abs(position.col - gridPosition.col) == 1 && position.row == gridPosition.row)) && ignorePositions.IndexOf(position) == -1);
		foreach (PuzzleGridPosition item in list2)
		{
			ignorePositions.Add(item);
		}
		foreach (PuzzleGridPosition gridPosition2 in list2)
		{
			list.AddRange(this.GetChainedPositions(gridPosition2, ignorePositions));
		}
		return list;
	}

	// Token: 0x040006E4 RID: 1764
	public List<PuzzleGridPosition> gridPositions;

	// Token: 0x040006E5 RID: 1765
	public List<PuzzleGridPosition> protectedPositions;
}
