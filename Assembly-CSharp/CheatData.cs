using System;
using System.Runtime.Serialization;

// Token: 0x02000131 RID: 305
[Serializable]
public class CheatData : ISerializable
{
	// Token: 0x06000745 RID: 1861 RVA: 0x00007869 File Offset: 0x00005A69
	public CheatData()
	{
		this.lovePotionCount = 0;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00007878 File Offset: 0x00005A78
	public CheatData(SerializationInfo info, StreamingContext context)
	{
		this.lovePotionCount = (int)info.GetValue("lovePotionCount", typeof(int));
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x000078A0 File Offset: 0x00005AA0
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("lovePotionCount", this.lovePotionCount);
	}

	// Token: 0x0400084D RID: 2125
	public int lovePotionCount;
}
