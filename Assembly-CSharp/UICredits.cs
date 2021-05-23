using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class UICredits : DisplayObject
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060002A3 RID: 675 RVA: 0x000043FB File Offset: 0x000025FB
	// (remove) Token: 0x060002A4 RID: 676 RVA: 0x00004414 File Offset: 0x00002614
	public event UICredits.UICreditsDelegate UICreditsClosedEvent;

	// Token: 0x060002A5 RID: 677 RVA: 0x0000442D File Offset: 0x0000262D
	protected override void OnStart()
	{
		base.OnStart();
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00019AAC File Offset: 0x00017CAC
	public void ShowCreditsScreen()
	{
		this._creditsScreen = (UnityEngine.Object.Instantiate(this.creditsScreenPrefab) as GameObject).GetComponent<CreditsScreen>();
		this._creditsScreen.Init();
		this._creditsScreen.CreditsScreenClosedEvent += this.HideCreditsScreen;
		base.AddChild(this._creditsScreen);
		GameManager.System.Cursor.SetAbsorber(this, false);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00019B14 File Offset: 0x00017D14
	public void HideCreditsScreen()
	{
		this._creditsScreen.CreditsScreenClosedEvent -= this.HideCreditsScreen;
		UnityEngine.Object.Destroy(this._creditsScreen.gameObj);
		this._creditsScreen = null;
		GameManager.System.Cursor.ClearAbsorber();
		if (this.UICreditsClosedEvent != null)
		{
			this.UICreditsClosedEvent();
		}
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00019B74 File Offset: 0x00017D74
	protected override void Destructor()
	{
		base.Destructor();
		if (this._creditsScreen != null)
		{
			this._creditsScreen.CreditsScreenClosedEvent -= this.HideCreditsScreen;
			UnityEngine.Object.Destroy(this._creditsScreen.gameObj);
		}
		this._creditsScreen = null;
	}

	// Token: 0x04000250 RID: 592
	public GameObject creditsScreenPrefab;

	// Token: 0x04000251 RID: 593
	private CreditsScreen _creditsScreen;

	// Token: 0x02000056 RID: 86
	// (Invoke) Token: 0x060002AB RID: 683
	public delegate void UICreditsDelegate();
}
