using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class ItemData : IData<ItemDefinition>
{
	// Token: 0x0600049E RID: 1182 RVA: 0x000250C4 File Offset: 0x000232C4
	public ItemData()
	{
		ItemDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(ItemDefinition)) as ItemDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000058C2 File Offset: 0x00003AC2
	public ItemDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00025124 File Offset: 0x00023324
	public List<ItemDefinition> GetAllOfType(ItemType itemType, bool includeHidden = false)
	{
		List<ItemDefinition> list = new List<ItemDefinition>();
		for (int i = 1; i <= this._definitions.Count; i++)
		{
			if (this._definitions[i].type == itemType && (!this._definitions[i].hidden || includeHidden))
			{
				list.Add(this._definitions[i]);
			}
		}
		return list;
	}

	// Token: 0x040003F5 RID: 1013
	private Dictionary<int, ItemDefinition> _definitions = new Dictionary<int, ItemDefinition>();
}
