using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class AbilityData : IData<AbilityDefinition>
{
	// Token: 0x0600048A RID: 1162 RVA: 0x00024D18 File Offset: 0x00022F18
	public AbilityData()
	{
		AbilityDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(AbilityDefinition)) as AbilityDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0000579A File Offset: 0x0000399A
	public AbilityDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000057BB File Offset: 0x000039BB
	public AbilityDefinition GetRandom()
	{
		return this._definitions[UnityEngine.Random.Range(1, this._definitions.Count + 1)];
	}

	// Token: 0x040003ED RID: 1005
	private Dictionary<int, AbilityDefinition> _definitions = new Dictionary<int, AbilityDefinition>();
}
