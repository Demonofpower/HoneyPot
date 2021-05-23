using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class SpriteGroupData : IData<SpriteGroupDefinition>
{
	// Token: 0x060004AB RID: 1195 RVA: 0x0002540C File Offset: 0x0002360C
	public SpriteGroupData()
	{
		SpriteGroupDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(SpriteGroupDefinition)) as SpriteGroupDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00005967 File Offset: 0x00003B67
	public SpriteGroupDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003FA RID: 1018
	private Dictionary<int, SpriteGroupDefinition> _definitions = new Dictionary<int, SpriteGroupDefinition>();
}
