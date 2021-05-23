using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class EnergyTrailData : IData<EnergyTrailDefinition>
{
	// Token: 0x06000497 RID: 1175 RVA: 0x00024F58 File Offset: 0x00023158
	public EnergyTrailData()
	{
		EnergyTrailDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(EnergyTrailDefinition)) as EnergyTrailDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00005880 File Offset: 0x00003A80
	public EnergyTrailDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F3 RID: 1011
	private Dictionary<int, EnergyTrailDefinition> _definitions = new Dictionary<int, EnergyTrailDefinition>();
}
