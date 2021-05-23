using System;

// Token: 0x02000116 RID: 278
public class CursorAttachedObject
{
	// Token: 0x06000616 RID: 1558 RVA: 0x00006936 File Offset: 0x00004B36
	public CursorAttachedObject(DisplayObject displayObject, float xOffset, float yOffset)
	{
		this.displayObject = displayObject;
		this.xOffset = xOffset;
		this.yOffset = yOffset;
	}

	// Token: 0x04000776 RID: 1910
	public DisplayObject displayObject;

	// Token: 0x04000777 RID: 1911
	public float xOffset;

	// Token: 0x04000778 RID: 1912
	public float yOffset;
}
