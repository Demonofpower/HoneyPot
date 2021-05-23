using System;

// Token: 0x0200001E RID: 30
public class Mask : NonDisplayObject
{
	// Token: 0x0600013D RID: 317 RVA: 0x000035D9 File Offset: 0x000017D9
	protected override void OnAwake()
	{
		base.OnAwake();
		this.uiMask = this.gameObj.GetComponent<tk2dUIMask>();
		if (this.uiMask == null)
		{
			this.uiMask = this.gameObj.AddComponent<tk2dUIMask>();
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000117D4 File Offset: 0x0000F9D4
	protected override void OnEditorUpdate()
	{
		base.OnEditorUpdate();
		if (this.uiMask == null)
		{
			this.uiMask = this.gameObj.GetComponent<tk2dUIMask>();
			if (this.uiMask == null)
			{
				this.uiMask = this.gameObj.AddComponent<tk2dUIMask>();
			}
		}
	}

	// Token: 0x040000F1 RID: 241
	public tk2dUIMask uiMask;
}
