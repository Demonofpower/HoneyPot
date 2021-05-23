using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownItem")]
public class tk2dUIDropDownItem : tk2dUIBaseItemControl
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06000A7F RID: 2687 RVA: 0x0000A296 File Offset: 0x00008496
	// (remove) Token: 0x06000A80 RID: 2688 RVA: 0x0000A2AF File Offset: 0x000084AF
	public event Action<tk2dUIDropDownItem> OnItemSelected;

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0000A2C8 File Offset: 0x000084C8
	// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0000A2D0 File Offset: 0x000084D0
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0000A2D9 File Offset: 0x000084D9
	// (set) Token: 0x06000A84 RID: 2692 RVA: 0x0000A2E6 File Offset: 0x000084E6
	public string LabelText
	{
		get
		{
			return this.label.text;
		}
		set
		{
			this.label.text = value;
			this.label.Commit();
		}
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0000A2FF File Offset: 0x000084FF
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ItemSelected;
		}
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0000A328 File Offset: 0x00008528
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ItemSelected;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0000A351 File Offset: 0x00008551
	private void ItemSelected()
	{
		if (this.OnItemSelected != null)
		{
			this.OnItemSelected(this);
		}
	}

	// Token: 0x04000BCE RID: 3022
	public tk2dTextMesh label;

	// Token: 0x04000BCF RID: 3023
	public float height;

	// Token: 0x04000BD0 RID: 3024
	public tk2dUIUpDownHoverButton upDownHoverBtn;

	// Token: 0x04000BD1 RID: 3025
	private int index;
}
