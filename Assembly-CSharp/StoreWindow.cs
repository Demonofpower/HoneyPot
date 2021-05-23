using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class StoreWindow : UIWindow
{
	// Token: 0x06000419 RID: 1049 RVA: 0x00023B30 File Offset: 0x00021D30
	public override void Init()
	{
		this.storeItemContainer = base.GetChildByName("StoreWindowItemsContainer");
		this._storeItems = new List<StoreWindowItem>();
		for (int i = 0; i < 6; i++)
		{
			StoreWindowItem storeWindowItem = this.storeItemContainer.GetChildByName("StoreWindowItem" + i.ToString()) as StoreWindowItem;
			storeWindowItem.Init();
			this._storeItems.Add(storeWindowItem);
			storeWindowItem.ItemClickedEvent += this.OnStoreItemClicked;
		}
		base.Init();
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00023BB8 File Offset: 0x00021DB8
	protected override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._storeItems.Count; i++)
		{
			this._storeItems[i].childrenAlpha = 0f;
			this._storeItems[i].SetLocalScale(0.5f, 0f, EaseType.Linear);
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00023C1C File Offset: 0x00021E1C
	protected override void Show(Sequence animSequence)
	{
		for (int i = 0; i < this._storeItems.Count; i++)
		{
			animSequence.Insert((float)i * 0.045f, HOTween.To(this._storeItems[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert((float)i * 0.045f, HOTween.To(this._storeItems[i].gameObj.transform, 0.5f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00023CE4 File Offset: 0x00021EE4
	protected override void Hide(Sequence animSequence)
	{
		int num = 0;
		for (int i = this._storeItems.Count - 1; i >= 0; i--)
		{
			animSequence.Insert((float)num * 0.045f, HOTween.To(this._storeItems[i].gameObj.transform, 0.5f, new TweenParms().Prop("localScale", new Vector3(0.5f, 0.5f, 1f)).Ease(EaseType.EaseInBack)));
			animSequence.Insert(0.25f + (float)num * 0.045f, HOTween.To(this._storeItems[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			num++;
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0000306D File Offset: 0x0000126D
	private void OnStoreItemClicked(StoreWindowItem storeWindowItem)
	{
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00023DBC File Offset: 0x00021FBC
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._storeItems.Count; i++)
		{
			this._storeItems[i].ItemClickedEvent -= this.OnStoreItemClicked;
		}
		this._storeItems.Clear();
		this._storeItems = null;
	}

	// Token: 0x040003C4 RID: 964
	private const int STORE_ITEM_COUNT = 6;

	// Token: 0x040003C5 RID: 965
	public DisplayObject storeItemContainer;

	// Token: 0x040003C6 RID: 966
	private List<StoreWindowItem> _storeItems;
}
