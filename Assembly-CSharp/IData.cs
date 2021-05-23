using System;

// Token: 0x020000A0 RID: 160
internal interface IData<D> where D : Definition
{
	// Token: 0x0600049D RID: 1181
	D Get(int id);
}
