using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class DialogSceneData : IData<DialogSceneDefinition>
{
	// Token: 0x06000493 RID: 1171 RVA: 0x00024E98 File Offset: 0x00023098
	public DialogSceneData()
	{
		DialogSceneDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(DialogSceneDefinition)) as DialogSceneDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0000583E File Offset: 0x00003A3E
	public DialogSceneDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F1 RID: 1009
	private Dictionary<int, DialogSceneDefinition> _definitions = new Dictionary<int, DialogSceneDefinition>();
}
