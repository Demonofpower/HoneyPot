using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class DialogTriggerData : IData<DialogTriggerDefinition>
{
	// Token: 0x06000495 RID: 1173 RVA: 0x00024EF8 File Offset: 0x000230F8
	public DialogTriggerData()
	{
		DialogTriggerDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(DialogTriggerDefinition)) as DialogTriggerDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0000585F File Offset: 0x00003A5F
	public DialogTriggerDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F2 RID: 1010
	private Dictionary<int, DialogTriggerDefinition> _definitions = new Dictionary<int, DialogTriggerDefinition>();
}
