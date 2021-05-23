using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButton")]
public class tk2dUIToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06000B10 RID: 2832 RVA: 0x0000AC75 File Offset: 0x00008E75
	// (remove) Token: 0x06000B11 RID: 2833 RVA: 0x0000AC8E File Offset: 0x00008E8E
	public event Action<tk2dUIToggleButton> OnToggle;

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000B12 RID: 2834 RVA: 0x0000ACA7 File Offset: 0x00008EA7
	// (set) Token: 0x06000B13 RID: 2835 RVA: 0x0000ACAF File Offset: 0x00008EAF
	public bool IsOn
	{
		get
		{
			return this.isOn;
		}
		set
		{
			if (this.isOn != value)
			{
				this.isOn = value;
				this.SetState();
				if (this.OnToggle != null)
				{
					this.OnToggle(this);
				}
			}
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0000ACE1 File Offset: 0x00008EE1
	// (set) Token: 0x06000B15 RID: 2837 RVA: 0x0000ACE9 File Offset: 0x00008EE9
	public bool IsInToggleGroup
	{
		get
		{
			return this.isInToggleGroup;
		}
		set
		{
			this.isInToggleGroup = value;
		}
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0000ACFA File Offset: 0x00008EFA
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0000AD3A File Offset: 0x00008F3A
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0000AD7A File Offset: 0x00008F7A
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0000AD8D File Offset: 0x00008F8D
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0004B1F8 File Offset: 0x000493F8
	private void ButtonToggle()
	{
		if (!this.isOn || !this.isInToggleGroup)
		{
			this.isOn = !this.isOn;
			this.SetState();
			if (this.OnToggle != null)
			{
				this.OnToggle(this);
			}
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0000ADA0 File Offset: 0x00008FA0
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.offStateGO, !this.isOn);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.onStateGO, this.isOn);
	}

	// Token: 0x04000C39 RID: 3129
	public GameObject offStateGO;

	// Token: 0x04000C3A RID: 3130
	public GameObject onStateGO;

	// Token: 0x04000C3B RID: 3131
	public bool activateOnPress;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private bool isOn = true;

	// Token: 0x04000C3D RID: 3133
	private bool isInToggleGroup;
}
