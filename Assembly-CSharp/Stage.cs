using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[ExecuteInEditMode]
public class Stage : DisplayObject
{
	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000186 RID: 390 RVA: 0x00003974 File Offset: 0x00001B74
	// (remove) Token: 0x06000187 RID: 391 RVA: 0x0000398D File Offset: 0x00001B8D
	public event Stage.StageDelegate StageStartedEvent;

	// Token: 0x06000188 RID: 392 RVA: 0x00012B70 File Offset: 0x00010D70
	protected override void OnStart()
	{
		base.OnStart();
		DisplayObject[] children = base.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			children[i].gameObj.SetActive(true);
		}
		this.background = (base.GetChildByName("Background") as Background);
		this.uiPuzzle = (base.GetChildByName("PuzzleUI") as UIPuzzle);
		this.uiTop = (base.GetChildByName("TopUI") as UITop);
		this.uiGirl = (base.GetChildByName("GirlUI") as UIGirl);
		this.girl = (base.GetChildByName("Girl") as Girl);
		this.altGirl = (base.GetChildByName("AltGirl") as Girl);
		this.uiWindows = (base.GetChildByName("UIWindows") as UIWindows);
		this.cellPhone = (base.GetChildByName("CellPhone") as UICellPhone);
		this.cellNotifications = (base.GetChildByName("CellNotifications") as UICellNotifications);
		this.uiTitle = (base.GetChildByName("TitleUI") as UITitle);
		this.effects = (base.GetChildByName("Effects") as UIEffects);
		this.transitionScreen = (base.GetChildByName("TransitionScreen") as UITransitionScreen);
		this.uiPhotoGallery = (base.GetChildByName("PhotoGalleryUI") as UIPhotoGallery);
		this.uiCredits = (base.GetChildByName("UICredits") as UICredits);
		this.tooltip = (base.GetChildByName("Tooltip") as UITooltip);
		base.RemoveChild(this.tooltip, false);
		if (this.StageStartedEvent != null)
		{
			this.StageStartedEvent();
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x000039A6 File Offset: 0x00001BA6
	private void Update()
	{
		if (this._dirty)
		{
			this._dirty = false;
			this.ReSort();
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x000039C0 File Offset: 0x00001BC0
	public void SetDirty()
	{
		this._dirty = true;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x000039C9 File Offset: 0x00001BC9
	public void ReIndex()
	{
		base.IndexChildren(true);
		this.ReSort();
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000039D8 File Offset: 0x00001BD8
	private void ReSort()
	{
		this.ReSortChildren(this, -1000f);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00012D1C File Offset: 0x00010F1C
	private void ReSortChildren(DisplayObject parent, float zSpace)
	{
		DisplayObject[] children = parent.GetChildren(false);
		float num = zSpace * 0.05f;
		float num2 = zSpace * 0.9f / (float)children.Length;
		foreach (DisplayObject displayObject in children)
		{
			displayObject.gameObj.transform.localPosition = new Vector3(displayObject.gameObj.transform.localPosition.x, displayObject.gameObj.transform.localPosition.y, num + num2 * (float)displayObject.childIndex);
			this.ReSortChildren(displayObject, num2);
		}
		if (parent.hasMasks)
		{
			foreach (Mask mask in parent.GetMasks())
			{
				mask.gameObj.transform.localPosition = new Vector3(mask.gameObj.transform.localPosition.x, mask.gameObj.transform.localPosition.y, zSpace * 0.025f);
				mask.uiMask.depth = Mathf.Abs(zSpace - zSpace * 0.05f);
				mask.uiMask.Build();
			}
		}
	}

	// Token: 0x04000117 RID: 279
	private const float STAGE_Z_SPACE = -1000f;

	// Token: 0x04000118 RID: 280
	private bool _dirty;

	// Token: 0x04000119 RID: 281
	public Background background;

	// Token: 0x0400011A RID: 282
	public UIPuzzle uiPuzzle;

	// Token: 0x0400011B RID: 283
	public UITop uiTop;

	// Token: 0x0400011C RID: 284
	public UIGirl uiGirl;

	// Token: 0x0400011D RID: 285
	public Girl girl;

	// Token: 0x0400011E RID: 286
	public Girl altGirl;

	// Token: 0x0400011F RID: 287
	public UIWindows uiWindows;

	// Token: 0x04000120 RID: 288
	public UICellPhone cellPhone;

	// Token: 0x04000121 RID: 289
	public UICellNotifications cellNotifications;

	// Token: 0x04000122 RID: 290
	public UITitle uiTitle;

	// Token: 0x04000123 RID: 291
	public UIEffects effects;

	// Token: 0x04000124 RID: 292
	public UITransitionScreen transitionScreen;

	// Token: 0x04000125 RID: 293
	public UIPhotoGallery uiPhotoGallery;

	// Token: 0x04000126 RID: 294
	public UICredits uiCredits;

	// Token: 0x04000127 RID: 295
	public new UITooltip tooltip;

	// Token: 0x02000027 RID: 39
	// (Invoke) Token: 0x0600018F RID: 399
	public delegate void StageDelegate();
}
