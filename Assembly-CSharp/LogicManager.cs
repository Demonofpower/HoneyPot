using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class LogicManager : MonoBehaviour
{
	// Token: 0x060006A7 RID: 1703 RVA: 0x000345A8 File Offset: 0x000327A8
	public bool GameConditionsMet(List<GameCondition> conditions, bool checkAll = true)
	{
		if (conditions == null || conditions.Count == 0)
		{
			return true;
		}
		if (checkAll)
		{
			for (int i = 0; i < conditions.Count; i++)
			{
				if (!this.GameConditionMet(conditions[i]))
				{
					return false;
				}
			}
			return true;
		}
		for (int j = 0; j < conditions.Count; j++)
		{
			if (this.GameConditionMet(conditions[j]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00034628 File Offset: 0x00032828
	public bool GameConditionMet(GameCondition condition)
	{
		bool flag = false;
		switch (condition.type)
		{
		case GameConditionType.NONE:
			flag = true;
			break;
		case GameConditionType.WITH_GIRL:
			flag = (GameManager.Stage.girl.definition == condition.girlDefinition);
			break;
		case GameConditionType.AT_LOCATION:
			flag = (GameManager.System.Location.currentLocation == condition.locationDefinition);
			break;
		case GameConditionType.GIRL_DETAIL_KNOWN:
			flag = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).IsDetailKnown(condition.girlDetailType);
			break;
		case GameConditionType.GIRL_RELATIONSHIP_LEVEL:
			flag = (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).relationshipLevel >= condition.relationshipLevel);
			break;
		case GameConditionType.GIRL_MET_STATUS:
			flag = (GameManager.System.Player.GetGirlData(condition.girlDefinition).metStatus == condition.metStatus);
			break;
		}
		if (condition.inverse)
		{
			flag = !flag;
		}
		return flag;
	}
}
