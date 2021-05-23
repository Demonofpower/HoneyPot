using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class LocationData : IData<LocationDefinition>
{
	// Token: 0x060004A1 RID: 1185 RVA: 0x0002519C File Offset: 0x0002339C
	public LocationData()
	{
		LocationDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(LocationDefinition)) as LocationDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x000058E3 File Offset: 0x00003AE3
	public LocationDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x000251FC File Offset: 0x000233FC
	public List<LocationDefinition> GetLocationsByType(LocationType type)
	{
		List<LocationDefinition> list = new List<LocationDefinition>();
		foreach (LocationDefinition locationDefinition in this._definitions.Values)
		{
			if (locationDefinition.type == type)
			{
				list.Add(locationDefinition);
			}
		}
		return list;
	}

	// Token: 0x040003F6 RID: 1014
	private Dictionary<int, LocationDefinition> _definitions = new Dictionary<int, LocationDefinition>();
}
