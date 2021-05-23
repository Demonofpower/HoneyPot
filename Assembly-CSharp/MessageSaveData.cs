using System;

// Token: 0x02000134 RID: 308
[Serializable]
public class MessageSaveData
{
	// Token: 0x0600074A RID: 1866 RVA: 0x000078B3 File Offset: 0x00005AB3
	public MessageSaveData()
	{
		this.messageId = 0;
		this.timestamp = 0;
		this.viewed = false;
	}

	// Token: 0x04000861 RID: 2145
	public int messageId;

	// Token: 0x04000862 RID: 2146
	public int timestamp;

	// Token: 0x04000863 RID: 2147
	public bool viewed;
}
