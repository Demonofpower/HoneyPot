using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class GirlData : IData<GirlDefinition>
{
	// Token: 0x06000499 RID: 1177 RVA: 0x00024FB8 File Offset: 0x000231B8
	public GirlData()
	{
		GirlDefinition[] array = Resources.FindObjectsOfTypeAll(typeof(GirlDefinition)) as GirlDefinition[];
		for (int i = 0; i < array.Length; i++)
		{
			this._definitions.Add(array[i].id, array[i]);
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000058A1 File Offset: 0x00003AA1
	public GirlDefinition Get(int id)
	{
		if (this._definitions.ContainsKey(id))
		{
			return this._definitions[id];
		}
		return null;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00025018 File Offset: 0x00023218
	public List<GirlDefinition> GetAll()
	{
		List<GirlDefinition> list = new List<GirlDefinition>();
		for (int i = 1; i <= this._definitions.Count; i++)
		{
			list.Add(this._definitions[i]);
		}
		return list;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0002505C File Offset: 0x0002325C
	public List<GirlDefinition> GetGirlsAtLocationWhen(LocationDefinition location, GameClockWeekday weekday, GameClockDaytime daytime)
	{
		List<GirlDefinition> list = new List<GirlDefinition>();
		for (int i = 1; i <= this._definitions.Count; i++)
		{
			GirlDefinition girlDefinition = this._definitions[i];
			GirlScheduleDaytime girlScheduleDaytime = girlDefinition.schedule[(int)weekday].daytimes[(int)daytime];
			if (girlScheduleDaytime.location == location)
			{
				list.Add(girlDefinition);
			}
		}
		return list;
	}

	// Token: 0x040003F4 RID: 1012
	private Dictionary<int, GirlDefinition> _definitions = new Dictionary<int, GirlDefinition>();
}
