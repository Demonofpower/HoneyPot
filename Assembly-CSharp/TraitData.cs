using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class TraitData : IData<TraitDefinition>
{
	// Token: 0x060004AD RID: 1197 RVA: 0x0002546C File Offset: 0x0002366C
	public TraitData()
	{
		TraitDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(TraitDefinition)) as TraitDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00005988 File Offset: 0x00003B88
	public TraitDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003FB RID: 1019
	private Dictionary<int, TraitDefinition> _definitions = new Dictionary<int, TraitDefinition>();
}
