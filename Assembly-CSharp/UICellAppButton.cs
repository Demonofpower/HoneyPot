using System;

// Token: 0x0200002A RID: 42
public class UICellAppButton : DisplayObject
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060001A0 RID: 416 RVA: 0x00003A72 File Offset: 0x00001C72
	// (remove) Token: 0x060001A1 RID: 417 RVA: 0x00003A8B File Offset: 0x00001C8B
	public event UICellAppButton.CellAppButtonDelegate CellAppButtonClickedEvent;

	// Token: 0x060001A2 RID: 418 RVA: 0x000130E0 File Offset: 0x000112E0
	protected override void OnStart()
	{
		base.OnStart();
		this.cellAppButtonIcon = (base.GetChildByName("CellAppButtonIcon") as SpriteObject);
		this.cellAppButtonIcon.sprite.SetSprite(this.cellAppDefinition.footerSprite);
		base.button.ButtonPressedEvent += this.OnButtonPress;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00003AAC File Offset: 0x00001CAC
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.CellAppButtonClickedEvent != null)
		{
			this.CellAppButtonClickedEvent(this);
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00003AC5 File Offset: 0x00001CC5
	public override bool CanShowTooltip()
	{
		return GameManager.Stage.cellPhone.IsOpen() && !GameManager.Stage.cellPhone.IsLocked() && base.button.IsEnabled();
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00003B02 File Offset: 0x00001D02
	public override string GetUniqueTooltipMessage()
	{
		return this.cellAppDefinition.appName;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00003B0F File Offset: 0x00001D0F
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x0400012E RID: 302
	public CellAppDefinition cellAppDefinition;

	// Token: 0x0400012F RID: 303
	public CellAppDefinition secondaryCellAppDefinition;

	// Token: 0x04000130 RID: 304
	public SpriteObject cellAppButtonIcon;

	// Token: 0x0200002B RID: 43
	// (Invoke) Token: 0x060001A9 RID: 425
	public delegate void CellAppButtonDelegate(UICellAppButton cellAppButton);
}
