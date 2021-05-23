using System;

// Token: 0x0200005F RID: 95
public class PhotoGalleryGirlPhoto
{
	// Token: 0x060002EF RID: 751 RVA: 0x000046E9 File Offset: 0x000028E9
	public PhotoGalleryGirlPhoto(GirlDefinition def, int index, GirlPhoto photo, bool unlock)
	{
		this.girlDef = def;
		this.photoIndex = index;
		this.girlPhoto = photo;
		this.unlocked = unlock;
	}

	// Token: 0x040002B6 RID: 694
	public GirlDefinition girlDef;

	// Token: 0x040002B7 RID: 695
	public int photoIndex;

	// Token: 0x040002B8 RID: 696
	public GirlPhoto girlPhoto;

	// Token: 0x040002B9 RID: 697
	public bool unlocked;
}
