using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
[AddComponentMenu("2D Toolkit/UI/tk2dUIDragItem")]
public class tk2dUIDragItem : tk2dUIBaseItemControl
{
	// Token: 0x06000A78 RID: 2680 RVA: 0x0000A211 File Offset: 0x00008411
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			this.uiItem.OnRelease += this.ButtonRelease;
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00048D0C File Offset: 0x00046F0C
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			this.uiItem.OnRelease -= this.ButtonRelease;
		}
		if (this.isBtnActive)
		{
			if (tk2dUIManager.Instance != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
			}
			this.isBtnActive = false;
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0000A251 File Offset: 0x00008451
	private void UpdateBtnPosition()
	{
		base.transform.position = this.CalculateNewPos();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00048D90 File Offset: 0x00046F90
	private Vector3 CalculateNewPos()
	{
		Vector2 position = this.uiItem.Touch.position;
		Vector3 a = tk2dUIManager.Instance.UICamera.ScreenToWorldPoint(new Vector3(position.x, position.y, base.transform.position.z - tk2dUIManager.Instance.UICamera.transform.position.z));
		a.z = base.transform.position.z;
		return a + this.offset;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00048E30 File Offset: 0x00047030
	public void ButtonDown()
	{
		if (!this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.UpdateBtnPosition;
		}
		this.isBtnActive = true;
		this.offset = Vector3.zero;
		Vector3 b = this.CalculateNewPos();
		this.offset = base.transform.position - b;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0000A264 File Offset: 0x00008464
	public void ButtonRelease()
	{
		if (this.isBtnActive)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.UpdateBtnPosition;
		}
		this.isBtnActive = false;
	}

	// Token: 0x04000BCB RID: 3019
	public tk2dUIManager uiManager;

	// Token: 0x04000BCC RID: 3020
	private Vector3 offset = Vector3.zero;

	// Token: 0x04000BCD RID: 3021
	private bool isBtnActive;
}
