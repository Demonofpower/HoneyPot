using System;
using UnityEngine;

// Token: 0x020001BC RID: 444
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownHoverButton")]
public class tk2dUIUpDownHoverButton : tk2dUIBaseItemControl
{
	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06000B46 RID: 2886 RVA: 0x0000AEED File Offset: 0x000090ED
	// (remove) Token: 0x06000B47 RID: 2887 RVA: 0x0000AF06 File Offset: 0x00009106
	public event Action<tk2dUIUpDownHoverButton> OnToggleOver;

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0000AF1F File Offset: 0x0000911F
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0000AF27 File Offset: 0x00009127
	// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0004B94C File Offset: 0x00049B4C
	public bool IsOver
	{
		get
		{
			return this.isDown || this.isHover;
		}
		set
		{
			if (value != this.isDown || this.isHover)
			{
				if (value)
				{
					this.isHover = true;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown && this.isHover)
				{
					this.isDown = false;
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown)
				{
					this.isDown = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else
				{
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
			}
		}
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0000AF3D File Offset: 0x0000913D
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0004BA40 File Offset: 0x00049C40
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
			this.uiItem.OnHoverOver += this.ButtonHoverOver;
			this.uiItem.OnHoverOut += this.ButtonHoverOut;
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0004BAE0 File Offset: 0x00049CE0
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
			this.uiItem.OnHoverOver -= this.ButtonHoverOver;
			this.uiItem.OnHoverOut -= this.ButtonHoverOut;
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0000AF45 File Offset: 0x00009145
	private void ButtonUp()
	{
		if (this.isDown)
		{
			this.isDown = false;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0000AF81 File Offset: 0x00009181
	private void ButtonDown()
	{
		if (!this.isDown)
		{
			this.isDown = true;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0000AFBD File Offset: 0x000091BD
	private void ButtonHoverOver()
	{
		if (!this.isHover)
		{
			this.isHover = true;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0000AFF9 File Offset: 0x000091F9
	private void ButtonHoverOut()
	{
		if (this.isHover)
		{
			this.isHover = false;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0004BB80 File Offset: 0x00049D80
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown && !this.isHover);
		if (this.downStateGO == this.hoverOverStateGO)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown || this.isHover);
		}
		else
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.hoverOverStateGO, this.isHover);
		}
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0000B035 File Offset: 0x00009235
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04000C54 RID: 3156
	public GameObject upStateGO;

	// Token: 0x04000C55 RID: 3157
	public GameObject downStateGO;

	// Token: 0x04000C56 RID: 3158
	public GameObject hoverOverStateGO;

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x04000C58 RID: 3160
	private bool isDown;

	// Token: 0x04000C59 RID: 3161
	private bool isHover;
}
