using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class CellAppData : IData<CellAppDefinition>
{
	// Token: 0x0600048F RID: 1167 RVA: 0x00024DD8 File Offset: 0x00022FD8
	public CellAppData()
	{
		CellAppDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(CellAppDefinition)) as CellAppDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x000057FC File Offset: 0x000039FC
	public CellAppDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003EF RID: 1007
	private Dictionary<int, CellAppDefinition> _definitions = new Dictionary<int, CellAppDefinition>();
}
