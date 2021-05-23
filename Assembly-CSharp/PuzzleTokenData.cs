using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class PuzzleTokenData : IData<PuzzleTokenDefinition>
{
	// Token: 0x060004A8 RID: 1192 RVA: 0x00025330 File Offset: 0x00023530
	public PuzzleTokenData()
	{
		PuzzleTokenDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(PuzzleTokenDefinition)) as PuzzleTokenDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00005946 File Offset: 0x00003B46
	public PuzzleTokenDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00025390 File Offset: 0x00023590
	public PuzzleTokenDefinition[] GetAll(bool bonusRoundOnly = false)
	{
		List<PuzzleTokenDefinition> list = new List<PuzzleTokenDefinition>();
		foreach (PuzzleTokenDefinition puzzleTokenDefinition in this._definitions.Values)
		{
			if (!bonusRoundOnly || puzzleTokenDefinition.bonusRound)
			{
				list.Add(puzzleTokenDefinition);
			}
		}
		return list.ToArray();
	}

	// Token: 0x040003F9 RID: 1017
	private Dictionary<int, PuzzleTokenDefinition> _definitions = new Dictionary<int, PuzzleTokenDefinition>();
}
