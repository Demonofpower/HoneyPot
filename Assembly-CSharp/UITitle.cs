using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class UITitle : DisplayObject
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000371 RID: 881 RVA: 0x00004D4C File Offset: 0x00002F4C
	// (remove) Token: 0x06000372 RID: 882 RVA: 0x00004D65 File Offset: 0x00002F65
	public event UITitle.UITitleDelegate SaveFileSelectedEvent;

	// Token: 0x06000373 RID: 883 RVA: 0x0000442D File Offset: 0x0000262D
	protected override void OnStart()
	{
		base.OnStart();
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001F494 File Offset: 0x0001D694
	public void ShowTitleScreen()
	{
		this._titleScreen = (UnityEngine.Object.Instantiate(this.titleScreenPrefab) as GameObject).GetComponent<TitleScreen>();
		this._titleScreen.Init();
		this._titleScreen.SelectSaveFileEvent += this.OnSelectSaveFile;
		base.AddChild(this._titleScreen);
		GameManager.System.Cursor.SetAbsorber(this, false);
		GameManager.Stage.transitionScreen.overlay.SetAlpha(0f, 0f);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00004D7E File Offset: 0x00002F7E
	public void HideTitleScreen()
	{
		UnityEngine.Object.Destroy(this._titleScreen.gameObj);
		this._titleScreen = null;
		GameManager.System.Cursor.ClearAbsorber();
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00004DA6 File Offset: 0x00002FA6
	private void OnSelectSaveFile(int saveFileIndex)
	{
		this._titleScreen.SelectSaveFileEvent -= this.OnSelectSaveFile;
		if (this.SaveFileSelectedEvent != null)
		{
			this.SaveFileSelectedEvent(saveFileIndex);
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0001F51C File Offset: 0x0001D71C
	protected override void Destructor()
	{
		base.Destructor();
		if (this._titleScreen != null)
		{
			this._titleScreen.SelectSaveFileEvent -= this.OnSelectSaveFile;
			UnityEngine.Object.Destroy(this._titleScreen.gameObj);
		}
		this._titleScreen = null;
	}

	// Token: 0x04000351 RID: 849
	public GameObject titleScreenPrefab;

	// Token: 0x04000352 RID: 850
	private TitleScreen _titleScreen;

	// Token: 0x02000073 RID: 115
	// (Invoke) Token: 0x0600037A RID: 890
	public delegate void UITitleDelegate(int saveFileIndex);
}
