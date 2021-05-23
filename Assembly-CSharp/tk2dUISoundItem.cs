using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
[AddComponentMenu("2D Toolkit/UI/tk2dUISoundItem")]
public class tk2dUISoundItem : tk2dUIBaseItemControl
{
	// Token: 0x06000AF6 RID: 2806 RVA: 0x0004AA88 File Offset: 0x00048C88
	private void OnEnable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown += this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp += this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick += this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease += this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0004AB48 File Offset: 0x00048D48
	private void OnDisable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown -= this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp -= this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick -= this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease -= this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0000AAEE File Offset: 0x00008CEE
	private void PlayDownSound()
	{
		this.PlaySound(this.downButtonSound);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0000AAFC File Offset: 0x00008CFC
	private void PlayUpSound()
	{
		this.PlaySound(this.upButtonSound);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0000AB0A File Offset: 0x00008D0A
	private void PlayClickSound()
	{
		this.PlaySound(this.clickButtonSound);
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0000AB18 File Offset: 0x00008D18
	private void PlayReleaseSound()
	{
		this.PlaySound(this.releaseButtonSound);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0000AB26 File Offset: 0x00008D26
	private void PlaySound(AudioClip source)
	{
		tk2dUIAudioManager.Instance.Play(source);
	}

	// Token: 0x04000C23 RID: 3107
	public AudioClip downButtonSound;

	// Token: 0x04000C24 RID: 3108
	public AudioClip upButtonSound;

	// Token: 0x04000C25 RID: 3109
	public AudioClip clickButtonSound;

	// Token: 0x04000C26 RID: 3110
	public AudioClip releaseButtonSound;
}
