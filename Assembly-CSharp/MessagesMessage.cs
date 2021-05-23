using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class MessagesMessage : DisplayObject
{
	// Token: 0x06000214 RID: 532 RVA: 0x00016448 File Offset: 0x00014648
	public void Init(int messageIndex, MessagePlayerData messagePlayerData)
	{
		this.messageBubble = base.GetChildByName("MessagesMessageBubble");
		this.messageBackground = (this.messageBubble.GetChildByName("MessagesMessageBackground") as SlicedSpriteObject);
		this.messageLabel = (this.messageBubble.GetChildByName("MessagesMessageLabel") as LabelObject);
		this.messageTime = this.messageBubble.GetChildByName("MessagesMessageTime");
		this.messageTimeLabel = (this.messageTime.GetChildByName("MessagesMessageTimeLabel") as LabelObject);
		this.viewPhotoButton = (this.messageBubble.GetChildByName("MessagesMessagePhotoButton") as SpriteObject);
		this.messageSender = base.GetChildByName("MessagesMessageSender");
		this.messageSenderIcon = (this.messageSender.GetChildByName("MessagesMessageGirlIcon") as SpriteObject);
		this.messageSenderNew = (this.messageSender.GetChildByName("MessagesMessageNew") as SpriteObject);
		this._messageIndex = messageIndex;
		this._messagePlayerData = messagePlayerData;
		if (messagePlayerData.messageDefinition.hasAltText && GameManager.System.Player.settingsGender == SettingsGender.FEMALE)
		{
			this.messageLabel.SetText(messagePlayerData.messageDefinition.altText);
		}
		else
		{
			this.messageLabel.SetText(messagePlayerData.messageDefinition.messageText);
		}
		string text = StringUtils.Titleize(GameManager.System.Clock.Weekday(messagePlayerData.timestamp, true).ToString().Substring(0, 3));
		int num = GameManager.System.Clock.CalendarDay(messagePlayerData.timestamp, true, true);
		if (num > 0)
		{
			text = text + " " + StringUtils.FormatIntWithDigitCount(num, 2);
		}
		this.messageTimeLabel.SetText(string.Concat(new string[]
		{
			"Sent by ",
			messagePlayerData.messageDefinition.sender.firstName,
			" | ",
			text,
			" | ",
			StringUtils.Titleize(GameManager.System.Clock.DayTime(messagePlayerData.timestamp).ToString().ToLower())
		}));
		this._bubbleHeight = 140 + 23 * Mathf.Max(this.messageLabel.label.FormattedText.Split(new char[]
		{
			'\n'
		}).Length - 3, 0);
		this.messageBackground.sprite.dimensions = new Vector2(this.messageBackground.sprite.dimensions.x, (float)this._bubbleHeight);
		this.messageTime.localY -= (float)(this._bubbleHeight - 140);
		this.viewPhotoButton.localY -= (float)(this._bubbleHeight - 140);
		this.messageSenderIcon.sprite.SetSprite(messagePlayerData.messageDefinition.sender.firstName.ToLower() + "_mini");
		if (messagePlayerData.viewed)
		{
			this.messageSenderNew.SetAlpha(0f, 0f);
		}
		this.messageSenderNew.localY += 17f;
		if (this._messageIndex % 2 != 0)
		{
			this.messageSender.localX += 401f;
			this.messageBackground.localX -= 19f;
			this.messageLabel.localX -= 61f;
			this.messageTime.localX -= 131f;
			this.viewPhotoButton.localX += 295f;
		}
		this.viewPhotoButton.button.ButtonPressedEvent += this.OnViewPhotoButtonPressed;
		if (!this._messagePlayerData.messageDefinition.photoAttached)
		{
			this.viewPhotoButton.button.Disable();
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00016838 File Offset: 0x00014A38
	private void OnViewPhotoButtonPressed(ButtonObject buttonObject)
	{
		GameManager.Stage.cellPhone.Lock(false);
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent += this.OnPhotoGalleryClosed;
		GameManager.Stage.uiPhotoGallery.ShowPhotoGallery(this._messagePlayerData.messageDefinition.sender, this._messagePlayerData.messageDefinition.photoIndex, false, false);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00003ECB File Offset: 0x000020CB
	private void OnPhotoGalleryClosed()
	{
		GameManager.Stage.uiPhotoGallery.UIPhotoGalleryClosedEvent -= this.OnPhotoGalleryClosed;
		GameManager.Stage.cellPhone.Unlock();
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00003EF7 File Offset: 0x000020F7
	public int GetYOffset()
	{
		return this._bubbleHeight + 2;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00003F01 File Offset: 0x00002101
	protected override void Destructor()
	{
		base.Destructor();
		if (this.viewPhotoButton != null)
		{
			this.viewPhotoButton.button.ButtonPressedEvent -= this.OnViewPhotoButtonPressed;
		}
	}

	// Token: 0x040001B2 RID: 434
	private const int MIN_BUBBLE_HEIGHT = 140;

	// Token: 0x040001B3 RID: 435
	private const int LINE_HEIGHT_INC = 23;

	// Token: 0x040001B4 RID: 436
	public DisplayObject messageBubble;

	// Token: 0x040001B5 RID: 437
	public SlicedSpriteObject messageBackground;

	// Token: 0x040001B6 RID: 438
	public LabelObject messageLabel;

	// Token: 0x040001B7 RID: 439
	public DisplayObject messageTime;

	// Token: 0x040001B8 RID: 440
	public LabelObject messageTimeLabel;

	// Token: 0x040001B9 RID: 441
	public SpriteObject viewPhotoButton;

	// Token: 0x040001BA RID: 442
	public DisplayObject messageSender;

	// Token: 0x040001BB RID: 443
	public SpriteObject messageSenderIcon;

	// Token: 0x040001BC RID: 444
	public SpriteObject messageSenderNew;

	// Token: 0x040001BD RID: 445
	private int _messageIndex;

	// Token: 0x040001BE RID: 446
	private MessagePlayerData _messagePlayerData;

	// Token: 0x040001BF RID: 447
	private int _bubbleHeight;
}
