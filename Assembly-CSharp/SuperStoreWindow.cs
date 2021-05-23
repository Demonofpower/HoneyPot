using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class SuperStoreWindow : UIWindow
{
	// Token: 0x0600042B RID: 1067 RVA: 0x00023EAC File Offset: 0x000220AC
	public override void Init()
	{
		this.storeItemContainer = base.GetChildByName("StoreWindowItemsContainer");
		this._storeItems = new List<StoreWindowItem>();
		for (int i = 0; i < 12; i++)
		{
			StoreWindowItem storeWindowItem = this.storeItemContainer.GetChildByName("StoreWindowItem" + i.ToString()) as StoreWindowItem;
			storeWindowItem.Init();
			this._storeItems.Add(storeWindowItem);
		}
		base.Init();
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00023F24 File Offset: 0x00022124
	protected override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._storeItems.Count; i++)
		{
			this._storeItems[i].childrenAlpha = 0f;
			this._storeItems[i].SetLocalScale(0.5f, 0f, EaseType.Linear);
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00023F88 File Offset: 0x00022188
	protected override void Show(Sequence animSequence)
	{
		for (int i = 0; i < this._storeItems.Count; i++)
		{
			animSequence.Insert((float)i * 0.045f, HOTween.To(this._storeItems[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			animSequence.Insert((float)i * 0.045f, HOTween.To(this._storeItems[i].gameObj.transform, 0.5f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00024050 File Offset: 0x00022250
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

	// Token: 0x0600042F RID: 1071 RVA: 0x000052E3 File Offset: 0x000034E3
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x040003CC RID: 972
	private const int STORE_ITEM_COUNT = 12;

	// Token: 0x040003CD RID: 973
	public DisplayObject storeItemContainer;

	// Token: 0x040003CE RID: 974
	private List<StoreWindowItem> _storeItems;
}
