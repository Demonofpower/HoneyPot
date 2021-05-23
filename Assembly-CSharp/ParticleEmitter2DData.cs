using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class ParticleEmitter2DData : IData<ParticleEmitter2DDefinition>
{
	// Token: 0x060004A6 RID: 1190 RVA: 0x000252D0 File Offset: 0x000234D0
	public ParticleEmitter2DData()
	{
		ParticleEmitter2DDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(ParticleEmitter2DDefinition)) as ParticleEmitter2DDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00005925 File Offset: 0x00003B25
	public ParticleEmitter2DDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F8 RID: 1016
	private Dictionary<int, ParticleEmitter2DDefinition> _definitions = new Dictionary<int, ParticleEmitter2DDefinition>();
}
