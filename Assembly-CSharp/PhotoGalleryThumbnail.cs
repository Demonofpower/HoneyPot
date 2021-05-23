using System;

// Token: 0x02000060 RID: 96
public class PhotoGalleryThumbnail : SpriteObject
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060002F1 RID: 753 RVA: 0x0000470E File Offset: 0x0000290E
	// (remove) Token: 0x060002F2 RID: 754 RVA: 0x00004727 File Offset: 0x00002927
	public event PhotoGalleryThumbnail.PhotoGalleryThumbnailDelegate PhotoGalleryThumbnailSelectedEvent;

	// Token: 0x060002F3 RID: 755 RVA: 0x0001BA18 File Offset: 0x00019C18
	public void Init(int index, PhotoGalleryGirlPhoto photoGalleryGirlPhoto)
	{
		this.thumbnailIndex = index;
		this.photo = photoGalleryGirlPhoto;
		if (this.photo.unlocked)
		{
			string sprite = this.photo.girlPhoto.thumbnailSpriteName[0];
			if (this.photo.girlPhoto.hasAlts && GameManager.System.settingsCensored)
			{
				sprite = this.photo.girlPhoto.thumbnailSpriteName[1];
			}
			this.sprite.SetSprite(sprite);
		}
		else
		{
			base.button.Disable();
		}
		base.button.ButtonPressedEvent += this.OnButtonPressed;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00004740 File Offset: 0x00002940
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.PhotoGalleryThumbnailSelectedEvent != null)
		{
			this.PhotoGalleryThumbnailSelectedEvent(this);
		}
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00004759 File Offset: 0x00002959
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x040002BA RID: 698
	public int thumbnailIndex;

	// Token: 0x040002BB RID: 699
	public PhotoGalleryGirlPhoto photo;

	// Token: 0x02000061 RID: 97
	// (Invoke) Token: 0x060002F7 RID: 759
	public delegate void PhotoGalleryThumbnailDelegate(PhotoGalleryThumbnail thumbnail);
}
