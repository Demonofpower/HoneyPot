using System;

// Token: 0x0200012C RID: 300
public class MessagePlayerData
{
	// Token: 0x060006E7 RID: 1767 RVA: 0x00007533 File Offset: 0x00005733
	public void ReadSaveData(MessageSaveData messageSaveData)
	{
		this.messageDefinition = GameManager.Data.Messages.Get(messageSaveData.messageId);
		this.timestamp = messageSaveData.timestamp;
		this.viewed = messageSaveData.viewed;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00007568 File Offset: 0x00005768
	public void WriteSaveData(MessageSaveData messageSaveData)
	{
		messageSaveData.messageId = this.messageDefinition.id;
		messageSaveData.timestamp = this.timestamp;
		messageSaveData.viewed = this.viewed;
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00007593 File Offset: 0x00005793
	// (set) Token: 0x060006EA RID: 1770 RVA: 0x0000759B File Offset: 0x0000579B
	public MessageDefinition messageDefinition
	{
		get
		{
			return this._messageDefinition;
		}
		set
		{
			this._messageDefinition = value;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x000075A4 File Offset: 0x000057A4
	// (set) Token: 0x060006EC RID: 1772 RVA: 0x000075AC File Offset: 0x000057AC
	public int timestamp
	{
		get
		{
			return this._timestamp;
		}
		set
		{
			this._timestamp = value;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x000075B5 File Offset: 0x000057B5
	// (set) Token: 0x060006EE RID: 1774 RVA: 0x000075BD File Offset: 0x000057BD
	public bool viewed
	{
		get
		{
			return this._viewed;
		}
		set
		{
			this._viewed = value;
		}
	}

	// Token: 0x04000819 RID: 2073
	private MessageDefinition _messageDefinition;

	// Token: 0x0400081A RID: 2074
	private int _timestamp;

	// Token: 0x0400081B RID: 2075
	private bool _viewed;
}
