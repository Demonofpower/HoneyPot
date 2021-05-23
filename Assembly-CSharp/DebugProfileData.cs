using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class DebugProfileData : IData<DebugProfile>
{
	// Token: 0x06000491 RID: 1169 RVA: 0x00024E38 File Offset: 0x00023038
	public DebugProfileData()
	{
		DebugProfile[] array = Resources.FindObjectsOfTypeAll(typeof(DebugProfile)) as DebugProfile[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0000581D File Offset: 0x00003A1D
	public DebugProfile Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F0 RID: 1008
	private Dictionary<int, DebugProfile> _definitions = new Dictionary<int, DebugProfile>();
}
