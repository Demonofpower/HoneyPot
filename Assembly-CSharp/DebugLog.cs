using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020001D0 RID: 464
public class DebugLog
{
	// Token: 0x06000BD0 RID: 3024 RVA: 0x0000B716 File Offset: 0x00009916
	public DebugLog()
	{
		this.number = 0;
		this.messages = new List<string>();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0000B730 File Offset: 0x00009930
	public void AddMessage(string message)
	{
		this.number++;
		this.messages.Add(this.number + ": " + message);
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0000B761 File Offset: 0x00009961
	public List<string> PrintLastMessages()
	{
		return this.messages.Skip(Math.Max(0, this.messages.Count<string>() - 15)).ToList<string>();
	}

	// Token: 0x04000CC5 RID: 3269
	private List<string> messages;

	// Token: 0x04000CC6 RID: 3270
	private int number;
}
