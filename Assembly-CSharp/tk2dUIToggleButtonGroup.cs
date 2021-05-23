using System;
using UnityEngine;

// Token: 0x020001B7 RID: 439
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButtonGroup")]
public class tk2dUIToggleButtonGroup : MonoBehaviour
{
	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06000B1E RID: 2846 RVA: 0x0000ADC7 File Offset: 0x00008FC7
	// (remove) Token: 0x06000B1F RID: 2847 RVA: 0x0000ADE0 File Offset: 0x00008FE0
	public event Action<tk2dUIToggleButtonGroup> OnChange;

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0000ADF9 File Offset: 0x00008FF9
	public tk2dUIToggleButton[] ToggleBtns
	{
		get
		{
			return this.toggleBtns;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0000AE01 File Offset: 0x00009001
	// (set) Token: 0x06000B22 RID: 2850 RVA: 0x0000AE09 File Offset: 0x00009009
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			if (this.selectedIndex != value)
			{
				this.selectedIndex = value;
				this.SetToggleButtonUsingSelectedIndex();
			}
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0000AE24 File Offset: 0x00009024
	// (set) Token: 0x06000B24 RID: 2852 RVA: 0x0000AE2C File Offset: 0x0000902C
	public tk2dUIToggleButton SelectedToggleButton
	{
		get
		{
			return this.selectedToggleButton;
		}
		set
		{
			this.ButtonToggle(value);
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0000AE35 File Offset: 0x00009035
	protected virtual void Awake()
	{
		this.Setup();
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0004B248 File Offset: 0x00049448
	protected void Setup()
	{
		foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
		{
			if (tk2dUIToggleButton != null)
			{
				tk2dUIToggleButton.IsInToggleGroup = true;
				tk2dUIToggleButton.IsOn = false;
				tk2dUIToggleButton.OnToggle += this.ButtonToggle;
			}
		}
		this.SetToggleButtonUsingSelectedIndex();
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0000AE3D File Offset: 0x0000903D
	public void AddNewToggleButtons(tk2dUIToggleButton[] newToggleBtns)
	{
		this.ClearExistingToggleBtns();
		this.toggleBtns = newToggleBtns;
		this.Setup();
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0004B2A8 File Offset: 0x000494A8
	private void ClearExistingToggleBtns()
	{
		if (this.toggleBtns != null && this.toggleBtns.Length > 0)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				tk2dUIToggleButton.IsInToggleGroup = false;
				tk2dUIToggleButton.OnToggle -= this.ButtonToggle;
				tk2dUIToggleButton.IsOn = false;
			}
		}
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0004B310 File Offset: 0x00049510
	private void SetToggleButtonUsingSelectedIndex()
	{
		if (this.selectedIndex >= 0 && this.selectedIndex < this.toggleBtns.Length)
		{
			tk2dUIToggleButton tk2dUIToggleButton = this.toggleBtns[this.selectedIndex];
			tk2dUIToggleButton.IsOn = true;
		}
		else
		{
			tk2dUIToggleButton tk2dUIToggleButton = null;
			this.selectedIndex = -1;
			this.ButtonToggle(tk2dUIToggleButton);
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0004B368 File Offset: 0x00049568
	private void ButtonToggle(tk2dUIToggleButton toggleButton)
	{
		if (toggleButton == null || toggleButton.IsOn)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				if (tk2dUIToggleButton != toggleButton)
				{
					tk2dUIToggleButton.IsOn = false;
				}
			}
			if (toggleButton != this.selectedToggleButton)
			{
				this.selectedToggleButton = toggleButton;
				this.SetSelectedIndexFromSelectedToggleButton();
				if (this.OnChange != null)
				{
					this.OnChange(this);
				}
			}
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0004B3F4 File Offset: 0x000495F4
	private void SetSelectedIndexFromSelectedToggleButton()
	{
		this.selectedIndex = -1;
		for (int i = 0; i < this.toggleBtns.Length; i++)
		{
			tk2dUIToggleButton x = this.toggleBtns[i];
			if (x == this.selectedToggleButton)
			{
				this.selectedIndex = i;
				break;
			}
		}
	}

	// Token: 0x04000C3F RID: 3135
	[SerializeField]
	private tk2dUIToggleButton[] toggleBtns;

	// Token: 0x04000C40 RID: 3136
	[SerializeField]
	private int selectedIndex;

	// Token: 0x04000C41 RID: 3137
	private tk2dUIToggleButton selectedToggleButton;
}
