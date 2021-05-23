using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class PuzzleAbility
{
	// Token: 0x06000501 RID: 1281 RVA: 0x00005E52 File Offset: 0x00004052
	public PuzzleAbility(AbilityDefinition abilityDefinition)
	{
		this.definition = abilityDefinition;
		this._values = new Dictionary<string, int>();
		this._puzzleGroups = new Dictionary<string, PuzzleGroup>();
	}

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06000502 RID: 1282 RVA: 0x00005E77 File Offset: 0x00004077
	// (remove) Token: 0x06000503 RID: 1283 RVA: 0x00005E90 File Offset: 0x00004090
	public event PuzzleAbility.PuzzleAbilityDelegate AbilityCompleteEvent;

	// Token: 0x06000504 RID: 1284 RVA: 0x00005EA9 File Offset: 0x000040A9
	public void SetSelectablePuzzleGroup(PuzzleGroup puzzleGroup)
	{
		this._puzzleGroups.Add("*", puzzleGroup);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0002619C File Offset: 0x0002439C
	public void Use()
	{
		foreach (AbilityBehaviorDefinition abilityBehaviorDefinition in this.definition.behaviors)
		{
			int num = this.ParseIntValue(abilityBehaviorDefinition.valueRef);
			switch (abilityBehaviorDefinition.type)
			{
			case AbilityBehaviorType.SET_TOKEN_GROUP:
			{
				PuzzleGroup puzzleGroup = this.GetPuzzleGroup(abilityBehaviorDefinition.tokenGroupConditionSet);
				this.ExpandPuzzleGroup(puzzleGroup, abilityBehaviorDefinition.tokenGroupConditionSet);
				int num2 = this.ParseIntValue(abilityBehaviorDefinition.limit);
				if (num2 > 0)
				{
					puzzleGroup.EnforceLimit(num2);
				}
				this._puzzleGroups.Add(abilityBehaviorDefinition.handle, puzzleGroup);
				break;
			}
			case AbilityBehaviorType.SET_VALUE:
				this._values.Add(abilityBehaviorDefinition.handle, this.GetValue(abilityBehaviorDefinition));
				break;
			case AbilityBehaviorType.VISUAL_EFFECT:
			{
				AbilityBehaviorVisualEffectType visualEffectType = abilityBehaviorDefinition.visualEffectType;
				if (visualEffectType != AbilityBehaviorVisualEffectType.ENERGY_TRAIL_SURGE)
				{
					if (visualEffectType == AbilityBehaviorVisualEffectType.PING_TOKENS)
					{
						this.ShowTokenPingEffect(this._puzzleGroups[abilityBehaviorDefinition.groupRef], abilityBehaviorDefinition.energyTrail);
					}
				}
				else
				{
					EnergyTrail component = new GameObject("EnergyTrail", new Type[]
					{
						typeof(EnergyTrail)
					}).GetComponent<EnergyTrail>();
					component.Init(GameManager.System.Cursor.GetMousePosition(), abilityBehaviorDefinition.energyTrail, null, EnergyTrailFormat.END, null);
					component.OverrideEnergySurge(GameManager.Stage.uiGirl.itemGiveEnergyTrail.girlEnergySurge);
				}
				break;
			}
			case AbilityBehaviorType.ADD_RESOURCE:
				if (abilityBehaviorDefinition.negative)
				{
					num *= -1;
				}
				GameManager.System.Puzzle.Game.AddResourceValue(abilityBehaviorDefinition.resourceType, num, abilityBehaviorDefinition.animate);
				if (abilityBehaviorDefinition.tokenDefinition != null)
				{
					PuzzleTokenReward puzzleTokenReward = new PuzzleTokenReward(abilityBehaviorDefinition.tokenDefinition, 1, num);
					puzzleTokenReward.OverrideType(abilityBehaviorDefinition.puzzleTokenType);
					EnergyTrail component = new GameObject("EnergyTrail", new Type[]
					{
						typeof(EnergyTrail)
					}).GetComponent<EnergyTrail>();
					component.Init(GameManager.System.Cursor.GetMousePosition(), abilityBehaviorDefinition.tokenDefinition.energyTrail, puzzleTokenReward.GetRewardPopText(true, false), EnergyTrailFormat.END, null);
					component.OverrideEnergySurge(GameManager.Stage.uiGirl.itemGiveEnergyTrail.girlEnergySurge);
				}
				break;
			case AbilityBehaviorType.CONSUME_TOKENS:
				GameManager.System.Puzzle.Game.ConsumePuzzleGroup(this._puzzleGroups[abilityBehaviorDefinition.groupRef], null);
				break;
			case AbilityBehaviorType.DESTROY_TOKENS:
				if (GameManager.System.Puzzle.Game.GetActivePuzzleEffectByType(AbilityBehaviorPuzzleEffectType.BLOCK_DATE_GIFT_TOKEN_CONSUME) == null)
				{
					GameManager.System.Puzzle.Game.DestroyPuzzleGroup(this._puzzleGroups[abilityBehaviorDefinition.groupRef], false);
				}
				break;
			case AbilityBehaviorType.UPGRADE_TOKENS:
			{
				PuzzleGroup puzzleGroup = this._puzzleGroups[abilityBehaviorDefinition.groupRef];
				this.ShowTokenPingEffect(puzzleGroup, null);
				for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
				{
					if (abilityBehaviorDefinition.levelSet)
					{
						puzzleGroup.gridPositions[i].GetToken(false).SetLevel(num);
					}
					else
					{
						puzzleGroup.gridPositions[i].GetToken(false).IncLevel(num);
					}
				}
				break;
			}
			case AbilityBehaviorType.SPAWN_TOKENS:
			{
				PuzzleGroup puzzleGroup = this._puzzleGroups[abilityBehaviorDefinition.groupRef];
				if (abilityBehaviorDefinition.tokenDefinitions.Count > 1 && puzzleGroup.gridPositions[0].GetToken(false) != null)
				{
					this.ShowTokenPingEffect(puzzleGroup, puzzleGroup.gridPositions[0].GetToken(false).definition.energyTrail);
				}
				else
				{
					this.ShowTokenPingEffect(puzzleGroup, abilityBehaviorDefinition.tokenDefinitions[0].energyTrail);
				}
				int level = (!abilityBehaviorDefinition.levelSet) ? -1 : num;
				GameManager.System.Puzzle.Game.SwitchPuzzleGroupTokensWith(puzzleGroup, abilityBehaviorDefinition.tokenDefinitions, level, abilityBehaviorDefinition.weighted);
				break;
			}
			case AbilityBehaviorType.CHANGE_PASSION_LEVEL:
			{
				if (abilityBehaviorDefinition.negative)
				{
					num *= -1;
				}
				int passionLevel = GameManager.System.Puzzle.Game.currentPassionLevel + num;
				GameManager.System.Puzzle.Game.SetResourceValue(PuzzleGameResourceType.PASSION, GameManager.System.Puzzle.GetPassionLevelCost(passionLevel), true);
				break;
			}
			case AbilityBehaviorType.PUZZLE_EFFECT:
				GameManager.System.Puzzle.Game.ApplyPuzzleEffect(abilityBehaviorDefinition);
				break;
			case AbilityBehaviorType.SOUND_EFFECT:
				GameManager.System.Audio.Play(AudioCategory.SOUND, abilityBehaviorDefinition.soundEffect, false, 1f, true);
				break;
			}
		}
		if (this.AbilityCompleteEvent != null)
		{
			this.AbilityCompleteEvent(this);
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00026688 File Offset: 0x00024888
	public PuzzleGroup GetPuzzleGroup(PuzzleTokenConditionSet conditionSet)
	{
		List<PuzzleGridPosition> conditionTokens = this.GetConditionTokens(GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values.ToList<PuzzleGridPosition>(), conditionSet.conditions);
		return new PuzzleGroup(conditionTokens);
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00005EBC File Offset: 0x000040BC
	public PuzzleGroup GetPuzzleGroupByRef(string key)
	{
		if (this._puzzleGroups.ContainsKey(key))
		{
			return this._puzzleGroups[key];
		}
		return null;
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x000266C8 File Offset: 0x000248C8
	private void ExpandPuzzleGroup(PuzzleGroup puzzleGroup, PuzzleTokenConditionSet conditionSet)
	{
		if (!conditionSet.expand)
		{
			return;
		}
		List<PuzzleGridPosition> list = new List<PuzzleGridPosition>();
		switch (conditionSet.tokenGroupExpansionType)
		{
		case TokenConditionExpansionType.ROW:
		{
			int i;
			for (i = 0; i < puzzleGroup.gridPositions.Count; i++)
			{
				list.AddRange((from position in GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values
				where position.row == puzzleGroup.gridPositions[i].row && position.col != puzzleGroup.gridPositions[i].col && Mathf.Abs(position.col - puzzleGroup.gridPositions[i].col) <= conditionSet.expandBy
				select position).ToList<PuzzleGridPosition>());
			}
			break;
		}
		case TokenConditionExpansionType.COL:
		{
			int i;
			for (i = 0; i < puzzleGroup.gridPositions.Count; i++)
			{
				list.AddRange((from position in GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values
				where position.col == puzzleGroup.gridPositions[i].col && position.row != puzzleGroup.gridPositions[i].row && Mathf.Abs(position.row - puzzleGroup.gridPositions[i].row) <= conditionSet.expandBy
				select position).ToList<PuzzleGridPosition>());
			}
			break;
		}
		case TokenConditionExpansionType.CROSS:
		{
			int i;
			for (i = 0; i < puzzleGroup.gridPositions.Count; i++)
			{
				list.AddRange((from position in GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values
				where (position.row == puzzleGroup.gridPositions[i].row && position.col != puzzleGroup.gridPositions[i].col && Mathf.Abs(position.col - puzzleGroup.gridPositions[i].col) <= conditionSet.expandBy) || (position.col == puzzleGroup.gridPositions[i].col && position.row != puzzleGroup.gridPositions[i].row && Mathf.Abs(position.row - puzzleGroup.gridPositions[i].row) <= conditionSet.expandBy)
				select position).ToList<PuzzleGridPosition>());
			}
			break;
		}
		case TokenConditionExpansionType.ADJACENT:
		{
			int i;
			for (i = 0; i < puzzleGroup.gridPositions.Count; i++)
			{
				list.AddRange((from position in GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values
				where Mathf.Abs(position.row - puzzleGroup.gridPositions[i].row) <= conditionSet.expandBy && Mathf.Abs(position.col - puzzleGroup.gridPositions[i].col) <= conditionSet.expandBy && position != puzzleGroup.gridPositions[i]
				select position).ToList<PuzzleGridPosition>());
			}
			break;
		}
		case TokenConditionExpansionType.TYPE:
		{
			int i;
			for (i = 0; i < puzzleGroup.gridPositions.Count; i++)
			{
				list.AddRange((from position in GameManager.System.Puzzle.Game.GetPuzzleGridPositions().Values
				where position.HasToken() && position.GetToken(false).definition == puzzleGroup.gridPositions[i].GetToken(false).definition && position != puzzleGroup.gridPositions[i]
				select position).ToList<PuzzleGridPosition>());
			}
			break;
		}
		}
		if (conditionSet.conditionedExpansion)
		{
			list = this.GetConditionTokens(list, conditionSet.expansionConditions);
		}
		list = list.Distinct<PuzzleGridPosition>().ToList<PuzzleGridPosition>();
		if (conditionSet.inclusive)
		{
			puzzleGroup.gridPositions.AddRange(list);
		}
		else
		{
			puzzleGroup.gridPositions.Clear();
			puzzleGroup.protectedPositions.Clear();
			puzzleGroup.gridPositions.AddRange(list);
		}
		puzzleGroup.SortGroup();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000269E8 File Offset: 0x00024BE8
	public PuzzleGroup GetExpandedPuzzleGroupFromTarget(PuzzleGridPosition gridPosition, PuzzleTokenConditionSet conditionSet)
	{
		PuzzleGroup puzzleGroup = new PuzzleGroup(new List<PuzzleGridPosition>
		{
			gridPosition
		});
		this.ExpandPuzzleGroup(puzzleGroup, conditionSet);
		return puzzleGroup;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00026A14 File Offset: 0x00024C14
	private List<PuzzleGridPosition> GetConditionTokens(List<PuzzleGridPosition> checkPositions, List<PuzzleTokenCondition> conditions)
	{
		List<PuzzleGridPosition> list = new List<PuzzleGridPosition>();
		foreach (PuzzleGridPosition puzzleGridPosition in checkPositions)
		{
			if (this.TokenMeetsConditionSet(puzzleGridPosition, conditions))
			{
				list.Add(puzzleGridPosition);
			}
		}
		return list;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00026A80 File Offset: 0x00024C80
	private bool TokenMeetsConditionSet(PuzzleGridPosition gridPosition, List<PuzzleTokenCondition> conditions)
	{
		for (int i = 0; i < conditions.Count; i++)
		{
			if (!this.TokenMeetsCondition(gridPosition, conditions[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00026ABC File Offset: 0x00024CBC
	private bool TokenMeetsCondition(PuzzleGridPosition gridPosition, PuzzleTokenCondition condition)
	{
		bool flag = false;
		int num = this.ParseIntValue(condition.val);
		switch (condition.type)
		{
		case PuzzleTokenConditionType.TOKEN_TYPE:
			flag = (gridPosition.GetToken(false).definition == condition.tokenDefinition);
			break;
		case PuzzleTokenConditionType.LEVEL:
			flag = MathUtils.CompareNumbers(condition.comparison, (float)gridPosition.GetToken(false).level, (float)num);
			break;
		case PuzzleTokenConditionType.ROW:
			flag = MathUtils.CompareNumbers(condition.comparison, (float)gridPosition.row, (float)num);
			break;
		case PuzzleTokenConditionType.COL:
			flag = MathUtils.CompareNumbers(condition.comparison, (float)gridPosition.col, (float)num);
			break;
		}
		if (condition.inverse)
		{
			return !flag;
		}
		return flag;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00026B7C File Offset: 0x00024D7C
	private int GetValue(AbilityBehaviorDefinition behavior)
	{
		float num = 0f;
		switch (behavior.valueType)
		{
		case AbilityBehaviorValueType.HARD:
			num = (float)behavior.hardValue;
			break;
		case AbilityBehaviorValueType.RANDOM_RANGE:
			num = (float)UnityEngine.Random.Range(this.ParseIntValue(behavior.min), this.ParseIntValue(behavior.max) + 1);
			break;
		case AbilityBehaviorValueType.COMBINE_VALUES:
			num = (float)this.ParseIntValue(behavior.combineValues[0]);
			for (int i = 1; i < behavior.combineValues.Count; i++)
			{
				switch (behavior.combineOperation)
				{
				case AbilityBehaviorValueOperation.ADD:
					num += (float)this.ParseIntValue(behavior.combineValues[i]);
					break;
				case AbilityBehaviorValueOperation.SUBTRACT:
					num -= (float)this.ParseIntValue(behavior.combineValues[i]);
					break;
				case AbilityBehaviorValueOperation.MULTIPLY:
					num *= (float)this.ParseIntValue(behavior.combineValues[i]);
					break;
				case AbilityBehaviorValueOperation.DIVIDE:
					if (this.ParseIntValue(behavior.combineValues[i]) != 0)
					{
						num /= (float)this.ParseIntValue(behavior.combineValues[i]);
					}
					break;
				}
			}
			break;
		case AbilityBehaviorValueType.RESOURCE_VALUE:
			if (behavior.resourceMaxValue)
			{
				num = (float)GameManager.System.Puzzle.Game.GetResourceMax(behavior.resourceType);
			}
			else
			{
				num = (float)GameManager.System.Puzzle.Game.GetResourceValue(behavior.resourceType);
			}
			break;
		case AbilityBehaviorValueType.TOKEN_GROUP_COUNT:
			num = (float)this._puzzleGroups[behavior.groupRef].gridPositions.Count;
			break;
		case AbilityBehaviorValueType.CLAMP_VALUE:
			num = (float)Mathf.Clamp(this.ParseIntValue(behavior.valueRef), this.ParseIntValue(behavior.min), this.ParseIntValue(behavior.max));
			break;
		case AbilityBehaviorValueType.PASSION_LEVEL:
			num = (float)GameManager.System.Puzzle.Game.currentPassionLevel;
			break;
		}
		num = Mathf.Floor(num * behavior.percentOfValue);
		if (behavior.negative)
		{
			num *= -1f;
		}
		return (int)num;
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00026DA8 File Offset: 0x00024FA8
	private int ParseIntValue(string val)
	{
		int result = 0;
		if (!int.TryParse(val, out result) && this._values.ContainsKey(val))
		{
			result = this._values[val];
		}
		return result;
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00026DE8 File Offset: 0x00024FE8
	private void ShowTokenPingEffect(PuzzleGroup puzzleGroup, EnergyTrailDefinition energyTrailOverride = null)
	{
		for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
		{
			PuzzleToken token = puzzleGroup.gridPositions[i].GetToken(false);
			if (!(token == null))
			{
				Vector3 origin = new Vector3(token.gameObj.transform.position.x + 41f, token.gameObj.transform.position.y - 41f, 0f);
				EnergyTrailDefinition energyTrailDefinition = token.definition.energyTrail;
				if (energyTrailOverride != null)
				{
					energyTrailDefinition = energyTrailOverride;
				}
				EnergyTrail component = new GameObject("EnergyTrail", new Type[]
				{
					typeof(EnergyTrail)
				}).GetComponent<EnergyTrail>();
				component.Init(origin, energyTrailDefinition, null, EnergyTrailFormat.START, null);
			}
		}
	}

	// Token: 0x040006A3 RID: 1699
	public AbilityDefinition definition;

	// Token: 0x040006A4 RID: 1700
	private Dictionary<string, int> _values;

	// Token: 0x040006A5 RID: 1701
	private Dictionary<string, PuzzleGroup> _puzzleGroups;

	// Token: 0x020000F7 RID: 247
	// (Invoke) Token: 0x06000511 RID: 1297
	public delegate void PuzzleAbilityDelegate(PuzzleAbility ability);
}
