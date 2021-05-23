using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
[AddComponentMenu("2D Toolkit/UI/tk2dUIMultiStateToggleButton")]
public class tk2dUIMultiStateToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06000AA8 RID: 2728 RVA: 0x0000A5CC File Offset: 0x000087CC
	// (remove) Token: 0x06000AA9 RID: 2729 RVA: 0x0000A5E5 File Offset: 0x000087E5
	public event Action<tk2dUIMultiStateToggleButton> OnStateToggle;

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0000A5FE File Offset: 0x000087FE
	// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0004939C File Offset: 0x0004759C
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			if (value >= this.states.Length)
			{
				value = this.states.Length;
			}
			if (value < 0)
			{
				value = 0;
			}
			if (this.index != value)
			{
				this.index = value;
				this.SetState();
				if (this.OnStateToggle != null)
				{
					this.OnStateToggle(this);
				}
			}
		}
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0000A606 File Offset: 0x00008806
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0000A60E File Offset: 0x0000880E
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0000A64E File Offset: 0x0000884E
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0000A68E File Offset: 0x0000888E
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0000A6A1 File Offset: 0x000088A1
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0000A6B4 File Offset: 0x000088B4
	private void ButtonToggle()
	{
		if (this.Index + 1 >= this.states.Length)
		{
			this.Index = 0;
		}
		else
		{
			this.Index++;
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x000493FC File Offset: 0x000475FC
	private void SetState()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			GameObject x = this.states[i];
			if (x != null)
			{
				if (i != this.index)
				{
					if (this.states[i].activeInHierarchy)
					{
						this.states[i].SetActive(false);
					}
				}
				else if (!this.states[i].activeInHierarchy)
				{
					this.states[i].SetActive(true);
				}
			}
		}
	}

	// Token: 0x04000BE2 RID: 3042
	public GameObject[] states;

	// Token: 0x04000BE3 RID: 3043
	public bool activateOnPress;

	// Token: 0x04000BE4 RID: 3044
	private int index;
}
