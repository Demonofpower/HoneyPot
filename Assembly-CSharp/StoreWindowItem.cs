using System;

// Token: 0x0200008C RID: 140
public class StoreWindowItem : DisplayObject
{
	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000420 RID: 1056 RVA: 0x00005279 File Offset: 0x00003479
	// (remove) Token: 0x06000421 RID: 1057 RVA: 0x00005292 File Offset: 0x00003492
	public event StoreWindowItem.StoreWindowItemDelegate ItemClickedEvent;

	// Token: 0x06000422 RID: 1058 RVA: 0x00023E1C File Offset: 0x0002201C
	public void Init()
	{
		if (this.definition == null)
		{
			this.definition = GameManager.Data.Items.Get(1);
		}
		this.itemIcon = (base.GetChildByName("StoreWindowItemIcon") as SpriteObject);
		this.itemNameLabel = (base.GetChildByName("StoreWindowItemNameLabel") as LabelObject);
		this.itemCostLabel = (base.GetChildByName("StoreWindowItemCostLabel") as LabelObject);
		base.button.ButtonPressedEvent += this.OnButtonPress;
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000052AB File Offset: 0x000034AB
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.ItemClickedEvent != null)
		{
			this.ItemClickedEvent(this);
		}
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0000306D File Offset: 0x0000126D
	private void OnDestroy()
	{
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000052C4 File Offset: 0x000034C4
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x040003C7 RID: 967
	public ItemDefinition definition;

	// Token: 0x040003C8 RID: 968
	public SpriteObject itemIcon;

	// Token: 0x040003C9 RID: 969
	public LabelObject itemNameLabel;

	// Token: 0x040003CA RID: 970
	public LabelObject itemCostLabel;

	// Token: 0x0200008D RID: 141
	// (Invoke) Token: 0x06000427 RID: 1063
	public delegate void StoreWindowItemDelegate(StoreWindowItem storeWindowItem);
}
