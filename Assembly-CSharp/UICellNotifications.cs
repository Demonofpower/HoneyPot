using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class UICellNotifications : DisplayObject
{
	// Token: 0x06000278 RID: 632 RVA: 0x00018940 File Offset: 0x00016B40
	protected override void OnStart()
	{
		base.OnStart();
		this.notificationBackground = (base.GetChildByName("CellNotificationsBackground") as SlicedSpriteObject);
		this.notificationLabel = (base.GetChildByName("CellNotificationsLabel") as LabelObject);
		this._origX = base.localX;
		this._origY = base.localY;
		this._justShown = false;
		this._isHidding = false;
		this._justHidden = false;
		this._notificationQueue = new List<CellNotification>();
		base.SetChildAlpha(0f, 0f);
		base.SetLocalScale(0.25f, 0f, EaseType.Linear);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x000189D8 File Offset: 0x00016BD8
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._justShown)
		{
			this._justShown = false;
			this.HideNotification();
		}
		else if (this._justHidden)
		{
			this._justHidden = false;
			this._currentNotification = null;
			this.ProcessNotificationQueue();
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00018A28 File Offset: 0x00016C28
	public void Notify(CellNotificationType type, string text)
	{
		CellNotification cellNotification = new CellNotification(type, text);
		if (GameManager.System.GameState == GameState.SIM && GameManager.System.Location.IsLocationSettled() && (this._currentNotification == null || this._currentNotification.type == cellNotification.type))
		{
			this.ShowNotification(cellNotification);
		}
		else
		{
			this._notificationQueue.Add(cellNotification);
		}
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00018A9C File Offset: 0x00016C9C
	private void ShowNotification(CellNotification notification)
	{
		TweenUtils.KillSequence(this._notificationSequence, false);
		this._isHidding = false;
		this._currentNotification = notification;
		base.SetChildAlpha(0f, 0f);
		base.SetLocalScale(0.25f, 0f, EaseType.Linear);
		this.notificationLabel.SetText(notification.text);
		this.notificationBackground.sprite.dimensions = new Vector2((float)Mathf.Min(74 + 11 * this.notificationLabel.label.text.Length, 1000), (float)(74 + 25 * Mathf.Max(this.notificationLabel.label.FormattedText.Split(new char[]
		{
			'\n'
		}).Length - 1, 0)));
		base.localY = this._origY - (float)(13 * Mathf.Max(this.notificationLabel.label.FormattedText.Split(new char[]
		{
			'\n'
		}).Length - 1, 0));
		this._notificationSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnNotificationShown)));
		this._notificationSequence.Insert(0f, HOTween.To(this, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.EaseOutSine)));
		this._notificationSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this._notificationSequence.Insert(0.25f, HOTween.To(this, 2f + 0.025f * (float)this.notificationLabel.label.text.Length, new TweenParms().Prop("localX", this._origX).Ease(EaseType.Linear)));
		this._notificationSequence.Play();
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.cellPhone.notificationSound, false, 1f, false);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00004211 File Offset: 0x00002411
	private void OnNotificationShown()
	{
		this._isHidding = false;
		this._justShown = true;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00018CC8 File Offset: 0x00016EC8
	private void HideNotification()
	{
		if (this._currentNotification == null || this._isHidding)
		{
			return;
		}
		TweenUtils.KillSequence(this._notificationSequence, false);
		this._isHidding = true;
		this._notificationSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnNotificationHidden)));
		this._notificationSequence.Insert(0f, HOTween.To(this, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.EaseInSine)));
		this._notificationSequence.Insert(0f, HOTween.To(this.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", new Vector3(0.25f, 0.25f, 1f)).Ease(EaseType.EaseInBack)));
		this._notificationSequence.Play();
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00004221 File Offset: 0x00002421
	private void OnNotificationHidden()
	{
		this._isHidding = false;
		this._justHidden = true;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00018DB8 File Offset: 0x00016FB8
	public void ProcessNotificationQueue()
	{
		if (this._notificationQueue.Count > 0)
		{
			CellNotification cellNotification = this._notificationQueue[0];
			this._notificationQueue.Remove(cellNotification);
			this.ShowNotification(cellNotification);
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00018DF8 File Offset: 0x00016FF8
	public void ClearNotificationsOfType(CellNotificationType type)
	{
		for (int i = 0; i < this._notificationQueue.Count; i++)
		{
			if (this._notificationQueue[i].type == type)
			{
				this._notificationQueue.RemoveAt(i);
				i--;
			}
		}
		if (this._currentNotification != null && this._currentNotification.type == type)
		{
			this.HideNotification();
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00004231 File Offset: 0x00002431
	public void ClearAllNotifications()
	{
		this._notificationQueue.Clear();
		this.HideNotification();
	}

	// Token: 0x04000217 RID: 535
	private const int MIN_WIDTH = 74;

	// Token: 0x04000218 RID: 536
	private const int MAX_WIDTH = 1000;

	// Token: 0x04000219 RID: 537
	private const int MIN_HEIGHT = 74;

	// Token: 0x0400021A RID: 538
	public SlicedSpriteObject notificationBackground;

	// Token: 0x0400021B RID: 539
	public LabelObject notificationLabel;

	// Token: 0x0400021C RID: 540
	private float _origX;

	// Token: 0x0400021D RID: 541
	private float _origY;

	// Token: 0x0400021E RID: 542
	private bool _justShown;

	// Token: 0x0400021F RID: 543
	private bool _isHidding;

	// Token: 0x04000220 RID: 544
	private bool _justHidden;

	// Token: 0x04000221 RID: 545
	private CellNotification _currentNotification;

	// Token: 0x04000222 RID: 546
	private List<CellNotification> _notificationQueue;

	// Token: 0x04000223 RID: 547
	private Sequence _notificationSequence;
}
