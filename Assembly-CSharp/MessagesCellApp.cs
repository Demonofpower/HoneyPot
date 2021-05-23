using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class MessagesCellApp : UICellApp
{
	// Token: 0x0600020E RID: 526 RVA: 0x00016044 File Offset: 0x00014244
	public override void Init()
	{
		this.messagesContainer = base.GetChildByName("MessagesMessageContainer");
		this.scrollBar = base.GetChildByName("MessagesScrollBar");
		this._messages = new List<MessagesMessage>();
		this._messageContentHeight = 0f;
		for (int i = 0; i < 20; i++)
		{
			MessagesMessage messagesMessage = this.messagesContainer.GetChildByName("MessagesMessage" + i.ToString()) as MessagesMessage;
			if (GameManager.System.Player.messages.Count <= i)
			{
				messagesMessage.gameObj.SetActive(false);
			}
			else
			{
				MessagePlayerData messagePlayerData = GameManager.System.Player.messages[i];
				messagesMessage.Init(i, messagePlayerData);
				this._messages.Add(messagesMessage);
				messagesMessage.localY -= this._messageContentHeight;
				this._messageContentHeight += (float)messagesMessage.GetYOffset();
				messagePlayerData.viewed = true;
			}
		}
		this._messageContentHeight -= 12f;
		this._scrollButton = (this.scrollBar.GetChildByName("ScrollBarButton") as SpriteObject);
		this._scrollButton.button.ButtonDownEvent += this.OnScrollBarDown;
		this._scrollOrigY = this._scrollButton.localY;
		this._messagesContainerOrigY = this.messagesContainer.localY;
		this.messagesContainer.localY = this._messagesContainerOrigY;
		this._isScrolling = false;
		if (this._messageContentHeight <= 584f)
		{
			this._scrollButton.button.Disable();
			this.scrollBar.SetChildAlpha(0.5f, 0f);
		}
		if (this._messages.Count == 0)
		{
			GameManager.Stage.cellPhone.ShowCellAppError("No messages yet...", false, 0f);
			this.scrollBar.SetChildAlpha(0f, 0f);
		}
		GameManager.Stage.cellNotifications.ClearNotificationsOfType(CellNotificationType.MESSAGE);
		GameManager.Stage.uiTop.RefreshMessageAlert();
		base.Init();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00016260 File Offset: 0x00014460
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._isScrolling)
		{
			float y = GameManager.System.Cursor.GetMouseDelta().y;
			this._scrollButton.localY = Mathf.Clamp(this._scrollButton.localY + y, this._scrollOrigY - 494f, this._scrollOrigY);
			this.messagesContainer.localY = Mathf.Clamp(Mathf.Round(Mathf.Lerp(this._messagesContainerOrigY, this._messagesContainerOrigY + (this._messageContentHeight - 584f), (Mathf.Abs(this._scrollButton.localY) - Mathf.Abs(this._scrollOrigY)) / (494f - Mathf.Abs(this._scrollOrigY)))), this._messagesContainerOrigY, this._messagesContainerOrigY + (this._messageContentHeight - 584f));
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00016340 File Offset: 0x00014540
	private void OnScrollBarDown(ButtonObject buttonObject)
	{
		this._scrollButton.button.ButtonDownEvent -= this.OnScrollBarDown;
		GameManager.Stage.MouseUpEvent += this.OnScrollBarUp;
		GameManager.Stage.cellPhone.Lock(false);
		this._isScrolling = true;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00016398 File Offset: 0x00014598
	private void OnScrollBarUp(DisplayObject displayObject)
	{
		GameManager.Stage.MouseUpEvent -= this.OnScrollBarUp;
		this._scrollButton.button.ButtonDownEvent += this.OnScrollBarDown;
		this._isScrolling = false;
		GameManager.Stage.cellPhone.Unlock();
	}

	// Token: 0x06000212 RID: 530 RVA: 0x000163F0 File Offset: 0x000145F0
	protected override void Destructor()
	{
		base.Destructor();
		this._scrollButton.button.ButtonDownEvent -= this.OnScrollBarDown;
		GameManager.Stage.MouseUpEvent -= this.OnScrollBarUp;
		this._messages.Clear();
		this._messages = null;
	}

	// Token: 0x040001A7 RID: 423
	private const int MESSAGE_COUNT = 20;

	// Token: 0x040001A8 RID: 424
	private const float SCROLL_BAR_HEIGHT = 494f;

	// Token: 0x040001A9 RID: 425
	private const float MESSAGE_AREA_HEIGHT = 584f;

	// Token: 0x040001AA RID: 426
	public DisplayObject messagesContainer;

	// Token: 0x040001AB RID: 427
	public DisplayObject scrollBar;

	// Token: 0x040001AC RID: 428
	private List<MessagesMessage> _messages;

	// Token: 0x040001AD RID: 429
	private float _messageContentHeight;

	// Token: 0x040001AE RID: 430
	private SpriteObject _scrollButton;

	// Token: 0x040001AF RID: 431
	private float _scrollOrigY;

	// Token: 0x040001B0 RID: 432
	private float _messagesContainerOrigY;

	// Token: 0x040001B1 RID: 433
	private bool _isScrolling;
}
