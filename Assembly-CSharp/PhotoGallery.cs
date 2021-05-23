using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class PhotoGallery : DisplayObject
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060002DF RID: 735 RVA: 0x00004652 File Offset: 0x00002852
	// (remove) Token: 0x060002E0 RID: 736 RVA: 0x0000466B File Offset: 0x0000286B
	public event PhotoGallery.PhotoGalleryDelegate PhotoGalleryClosedEvent;

	// Token: 0x060002E1 RID: 737 RVA: 0x0001B158 File Offset: 0x00019358
	public void Init(List<PhotoGalleryGirlPhoto> photos, GirlDefinition initialPhotoGirl = null, int initialPhotoIndex = 0, bool singlePhoto = false)
	{
		this.thumbnailsSection = base.GetChildByName("PhotoGalleryThumbnails");
		this.thumbnailsContainer = this.thumbnailsSection.GetChildByName("PhotoGalleryThumbnailsContainer");
		this.photoSection = base.GetChildByName("PhotoGalleryPhoto");
		this.buttonsContainer = this.photoSection.GetChildByName("PhotoGalleryButtonsContainer");
		this.altButtonsContainer = this.photoSection.GetChildByName("PhotoGalleryAltButtonsContainer");
		this.bigPhoto = (this.photoSection.GetChildByName("PhotoGalleryPhotoSprite") as SpriteObject);
		this.fadeCover = (base.GetChildByName("PhotoGalleryCover") as SpriteObject);
		this.singlePhotoMode = singlePhoto;
		this._thumbnails = new List<PhotoGalleryThumbnail>();
		int num = 0;
		for (int i = 0; i < photos.Count; i++)
		{
			PhotoGalleryThumbnail photoGalleryThumbnail = this.thumbnailsContainer.GetChildByName("PhotoGalleryThumbnail" + i.ToString()) as PhotoGalleryThumbnail;
			photoGalleryThumbnail.Init(i, photos[i]);
			photoGalleryThumbnail.PhotoGalleryThumbnailSelectedEvent += this.OnThumbnailSelected;
			this._thumbnails.Add(photoGalleryThumbnail);
			if (photoGalleryThumbnail.photo.unlocked)
			{
				num++;
			}
		}
		this._galleryCloseButton = (this.thumbnailsSection.GetChildByName("PhotoGalleryCloseGalleryButton") as SpriteObject);
		this._galleryCloseButton.button.ButtonPressedEvent += this.OnCloseButtonPressed;
		this._backButton = (this.buttonsContainer.GetChildByName("PhotoGalleryBackButton") as SpriteObject);
		this._backButton.button.ButtonPressedEvent += this.OnBackButtonPressed;
		this._forwardButton = (this.buttonsContainer.GetChildByName("PhotoGalleryForwardButton") as SpriteObject);
		this._forwardButton.button.ButtonPressedEvent += this.OnForwardButtonPressed;
		if (num <= 1 || this.singlePhotoMode)
		{
			this._backButton.button.Disable();
			this._forwardButton.button.Disable();
		}
		this._closeButton = (this.buttonsContainer.GetChildByName("PhotoGalleryCloseButton") as SpriteObject);
		this._closeButton.button.ButtonPressedEvent += this.OnCloseButtonPressed;
		this._galleryButton = (this.buttonsContainer.GetChildByName("PhotoGalleryGalleryButton") as SpriteObject);
		this._galleryButton.button.ButtonPressedEvent += this.OnGalleryButtonPressed;
		if (this.singlePhotoMode)
		{
			this._galleryButton.button.Disable();
		}
		this._altButtons = new List<SpriteObject>();
		for (int j = 0; j < 4; j++)
		{
			SpriteObject spriteObject = this.altButtonsContainer.GetChildByName("PhotoGalleryAltButton" + j.ToString()) as SpriteObject;
			spriteObject.button.ButtonPressedEvent += this.OnAltButtonPressed;
			this._altButtons.Add(spriteObject);
		}
		this.photoSection.gameObj.SetActive(false);
		this._photoSectionShowing = false;
		this._activeThumbnailIndex = 0;
		if (initialPhotoGirl != null)
		{
			int num2 = 0;
			for (int k = 0; k < this._thumbnails.Count; k++)
			{
				if (this._thumbnails[k].photo.girlDef == initialPhotoGirl && this._thumbnails[k].photo.photoIndex == initialPhotoIndex)
				{
					num2 = k;
					break;
				}
			}
			if (this._thumbnails[num2].photo.unlocked)
			{
				this._activeThumbnailIndex = num2;
				this.RefreshBigPhoto(-1);
			}
		}
		if (!this.singlePhotoMode)
		{
			this.fadeCover.SetAlpha(0f, 0f);
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00004684 File Offset: 0x00002884
	private void OnThumbnailSelected(PhotoGalleryThumbnail thumbnail)
	{
		this._activeThumbnailIndex = thumbnail.thumbnailIndex;
		this.RefreshBigPhoto(-1);
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPhotoGallery.openSound, false, 1f, true);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001B530 File Offset: 0x00019730
	private void RefreshBigPhoto(int fullSpriteNameIndex = -1)
	{
		if (!this._photoSectionShowing)
		{
			this._photoSectionShowing = true;
			this.thumbnailsSection.interactive = false;
			this.photoSection.gameObj.SetActive(true);
		}
		PhotoGalleryGirlPhoto photo = this._thumbnails[this._activeThumbnailIndex].photo;
		int num = 0;
		if (photo.girlPhoto.hasAlts && !GameManager.System.settingsCensored)
		{
			num = photo.girlPhoto.altCount;
			if (GameManager.System.Player.settingsGender == SettingsGender.FEMALE)
			{
				num = Mathf.RoundToInt((float)(num / 2));
			}
		}
		if (this._activeSpriteCollection == null || this._activeSpriteCollection.spriteCollectionName != photo.girlDef.photosSpriteCollectionName)
		{
			this._activeSpriteCollection = (Resources.Load("SpriteCollections/Photos/" + photo.girlDef.photosSpriteCollectionName, typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData);
		}
		if (num > 0)
		{
			if (fullSpriteNameIndex == -1)
			{
				fullSpriteNameIndex = num - 1;
			}
			fullSpriteNameIndex = Mathf.Clamp(fullSpriteNameIndex, 0, num - 1);
			if (GameManager.System.Player.settingsGender == SettingsGender.FEMALE && fullSpriteNameIndex != 0)
			{
				fullSpriteNameIndex = 2;
			}
		}
		else
		{
			fullSpriteNameIndex = 0;
		}
		this.bigPhoto.sprite.SetSprite(this._activeSpriteCollection, photo.girlPhoto.fullSpriteName[fullSpriteNameIndex]);
		fullSpriteNameIndex = Mathf.Clamp(fullSpriteNameIndex, 0, num - 1);
		for (int i = 0; i < 4; i++)
		{
			if (i < num && num > 1)
			{
				this._altButtons[i].localX = 0f;
			}
			else
			{
				this._altButtons[i].localX = 100f;
			}
			if (i == fullSpriteNameIndex)
			{
				this._altButtons[i].button.Disable();
			}
			else
			{
				this._altButtons[i].button.Enable();
			}
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0001B734 File Offset: 0x00019934
	private void OnBackButtonPressed(ButtonObject buttonObject)
	{
		do
		{
			this._activeThumbnailIndex--;
			if (this._activeThumbnailIndex < 0)
			{
				this._activeThumbnailIndex = this._thumbnails.Count - 1;
			}
		}
		while (!this._thumbnails[this._activeThumbnailIndex].photo.unlocked);
		this.RefreshBigPhoto(-1);
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPhotoGallery.flipSound, false, 1f, true);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0001B7BC File Offset: 0x000199BC
	private void OnForwardButtonPressed(ButtonObject buttonObject)
	{
		do
		{
			this._activeThumbnailIndex++;
			if (this._activeThumbnailIndex >= this._thumbnails.Count)
			{
				this._activeThumbnailIndex = 0;
			}
		}
		while (!this._thumbnails[this._activeThumbnailIndex].photo.unlocked);
		this.RefreshBigPhoto(-1);
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPhotoGallery.flipSound, false, 1f, true);
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001B844 File Offset: 0x00019A44
	private void OnAltButtonPressed(ButtonObject buttonObject)
	{
		SpriteObject item = buttonObject.GetDisplayObject() as SpriteObject;
		this.RefreshBigPhoto(this._altButtons.IndexOf(item));
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0001B870 File Offset: 0x00019A70
	private void OnGalleryButtonPressed(ButtonObject buttonObject)
	{
		if (!this._photoSectionShowing)
		{
			return;
		}
		this.photoSection.gameObj.SetActive(false);
		this.thumbnailsSection.interactive = true;
		this._photoSectionShowing = false;
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPhotoGallery.closeSound, false, 1f, true);
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000046C0 File Offset: 0x000028C0
	private void OnCloseButtonPressed(ButtonObject buttonObject)
	{
		this.photoSection.gameObj.SetActive(true);
		if (this.PhotoGalleryClosedEvent != null)
		{
			this.PhotoGalleryClosedEvent();
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0001B8D4 File Offset: 0x00019AD4
	protected override void Destructor()
	{
		base.Destructor();
		this._activeSpriteCollection = null;
		for (int i = 0; i < this._thumbnails.Count; i++)
		{
			this._thumbnails[i].PhotoGalleryThumbnailSelectedEvent -= this.OnThumbnailSelected;
		}
		this._thumbnails.Clear();
		this._thumbnails = null;
		this._galleryCloseButton.button.ButtonPressedEvent -= this.OnCloseButtonPressed;
		this._backButton.button.ButtonPressedEvent -= this.OnBackButtonPressed;
		this._forwardButton.button.ButtonPressedEvent -= this.OnForwardButtonPressed;
		this._closeButton.button.ButtonPressedEvent -= this.OnCloseButtonPressed;
		this._galleryButton.button.ButtonPressedEvent -= this.OnGalleryButtonPressed;
		for (int j = 0; j < this._altButtons.Count; j++)
		{
			this._altButtons[j].button.ButtonPressedEvent -= this.OnAltButtonPressed;
		}
		this._altButtons.Clear();
		this._altButtons = null;
	}

	// Token: 0x040002A1 RID: 673
	private const int PHOTO_THUMB_COUNT = 48;

	// Token: 0x040002A2 RID: 674
	private const int ALT_BUTTON_COUNT = 4;

	// Token: 0x040002A3 RID: 675
	public DisplayObject thumbnailsSection;

	// Token: 0x040002A4 RID: 676
	public DisplayObject thumbnailsContainer;

	// Token: 0x040002A5 RID: 677
	public DisplayObject photoSection;

	// Token: 0x040002A6 RID: 678
	public DisplayObject buttonsContainer;

	// Token: 0x040002A7 RID: 679
	public DisplayObject altButtonsContainer;

	// Token: 0x040002A8 RID: 680
	public SpriteObject bigPhoto;

	// Token: 0x040002A9 RID: 681
	public SpriteObject fadeCover;

	// Token: 0x040002AA RID: 682
	public bool singlePhotoMode;

	// Token: 0x040002AB RID: 683
	private List<PhotoGalleryThumbnail> _thumbnails;

	// Token: 0x040002AC RID: 684
	private bool _photoSectionShowing;

	// Token: 0x040002AD RID: 685
	private int _activeThumbnailIndex;

	// Token: 0x040002AE RID: 686
	private tk2dSpriteCollectionData _activeSpriteCollection;

	// Token: 0x040002AF RID: 687
	private SpriteObject _galleryCloseButton;

	// Token: 0x040002B0 RID: 688
	private SpriteObject _backButton;

	// Token: 0x040002B1 RID: 689
	private SpriteObject _forwardButton;

	// Token: 0x040002B2 RID: 690
	private SpriteObject _closeButton;

	// Token: 0x040002B3 RID: 691
	private SpriteObject _galleryButton;

	// Token: 0x040002B4 RID: 692
	private List<SpriteObject> _altButtons;

	// Token: 0x0200005E RID: 94
	// (Invoke) Token: 0x060002EC RID: 748
	public delegate void PhotoGalleryDelegate();
}
