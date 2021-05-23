using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class ActionMenuItemData : IData<ActionMenuItemDefinition>
{
	// Token: 0x0600048D RID: 1165 RVA: 0x00024D78 File Offset: 0x00022F78
	public ActionMenuItemData()
	{
		ActionMenuItemDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(ActionMenuItemDefinition)) as ActionMenuItemDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000057DB File Offset: 0x000039DB
	public ActionMenuItemDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003EE RID: 1006
	private Dictionary<int, ActionMenuItemDefinition> _definitions = new Dictionary<int, ActionMenuItemDefinition>();
}
