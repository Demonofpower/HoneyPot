using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class MessageData : IData<MessageDefinition>
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x00025270 File Offset: 0x00023470
	public MessageData()
	{
		MessageDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(MessageDefinition)) as MessageDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00005904 File Offset: 0x00003B04
	public MessageDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x040003F7 RID: 1015
	private Dictionary<int, MessageDefinition> _definitions = new Dictionary<int, MessageDefinition>();
}
