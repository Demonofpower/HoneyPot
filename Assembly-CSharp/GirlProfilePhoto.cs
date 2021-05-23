using System;

// Token: 0x02000032 RID: 50
public class GirlProfilePhoto : SpriteObject
{
	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060001D0 RID: 464 RVA: 0x00003C61 File Offset: 0x00001E61
	// (remove) Token: 0x060001D1 RID: 465 RVA: 0x00003C7A File Offset: 0x00001E7A
	public event GirlProfilePhoto.GirlProfilePhotoDelegate PhotoPressedEvent;

	// Token: 0x060001D2 RID: 466 RVA: 0x00003C93 File Offset: 0x00001E93
	public void Init(int index)
	{
		this.photoIndex = index;
		base.button.ButtonPressedEvent += this.OnButtonPressed;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00003CB3 File Offset: 0x00001EB3
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.PhotoPressedEvent != null)
		{
			this.PhotoPressedEvent(this);
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00003CCC File Offset: 0x00001ECC
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x0400016A RID: 362
	public int photoIndex;

	// Token: 0x02000033 RID: 51
	// (Invoke) Token: 0x060001D6 RID: 470
	public delegate void GirlProfilePhotoDelegate(GirlProfilePhoto photo);
}
