using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownButton")]
public class tk2dUIUpDownButton : tk2dUIBaseItemControl
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0000AE8F File Offset: 0x0000908F
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0000AE97 File Offset: 0x00009097
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0004B864 File Offset: 0x00049A64
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease += this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp += this.ButtonUp;
			}
		}
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0004B8D8 File Offset: 0x00049AD8
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease -= this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp -= this.ButtonUp;
			}
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0000AE9F File Offset: 0x0000909F
	private void ButtonUp()
	{
		this.isDown = false;
		this.SetState();
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0000AEAE File Offset: 0x000090AE
	private void ButtonDown()
	{
		this.isDown = true;
		this.SetState();
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0000AEBD File Offset: 0x000090BD
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0000AEE4 File Offset: 0x000090E4
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04000C50 RID: 3152
	public GameObject upStateGO;

	// Token: 0x04000C51 RID: 3153
	public GameObject downStateGO;

	// Token: 0x04000C52 RID: 3154
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x04000C53 RID: 3155
	private bool isDown;
}
