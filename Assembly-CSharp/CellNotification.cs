using System;

// Token: 0x02000029 RID: 41
public class CellNotification
{
	// Token: 0x0600019E RID: 414 RVA: 0x00003A5C File Offset: 0x00001C5C
	public CellNotification(CellNotificationType notificationType, string notificationText)
	{
		this.type = notificationType;
		this.text = notificationText;
	}

	// Token: 0x0400012C RID: 300
	public CellNotificationType type;

	// Token: 0x0400012D RID: 301
	public string text;
}
