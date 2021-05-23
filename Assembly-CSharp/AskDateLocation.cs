using System;

// Token: 0x02000084 RID: 132
public class AskDateLocation : DisplayObject
{
	// Token: 0x14000030 RID: 48
	// (add) Token: 0x060003F0 RID: 1008 RVA: 0x000050D0 File Offset: 0x000032D0
	// (remove) Token: 0x060003F1 RID: 1009 RVA: 0x000050E9 File Offset: 0x000032E9
	public event AskDateLocation.AskDateLocationDelegate LocationOptionClickedEvent;

	// Token: 0x060003F2 RID: 1010 RVA: 0x000235C8 File Offset: 0x000217C8
	public void Init()
	{
		this.locationThumbnail = (base.GetChildByName("AskDateLocationThumbnail") as SpriteObject);
		this.locationLabel = (base.GetChildByName("AskDateLocationLabel") as LabelObject);
		if (this.locationDefinition != null)
		{
			this.locationLabel.SetText(this.locationDefinition.name);
		}
		else
		{
			base.button.Disable();
		}
		this.origLocalY = base.transform.localPosition.y;
		base.button.ButtonPressedEvent += this.OnButtonPress;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00005102 File Offset: 0x00003302
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.LocationOptionClickedEvent != null)
		{
			this.LocationOptionClickedEvent(this);
		}
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0000511B File Offset: 0x0000331B
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x040003B1 RID: 945
	public LocationDefinition locationDefinition;

	// Token: 0x040003B2 RID: 946
	public SpriteObject locationThumbnail;

	// Token: 0x040003B3 RID: 947
	public LabelObject locationLabel;

	// Token: 0x040003B4 RID: 948
	public float origLocalY;

	// Token: 0x02000085 RID: 133
	// (Invoke) Token: 0x060003F6 RID: 1014
	public delegate void AskDateLocationDelegate(AskDateLocation askDateLocation);
}
