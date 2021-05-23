using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class UIPhotoGallery : DisplayObject
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060002FB RID: 763 RVA: 0x00004778 File Offset: 0x00002978
	// (remove) Token: 0x060002FC RID: 764 RVA: 0x00004791 File Offset: 0x00002991
	public event UIPhotoGallery.UIPhotoGalleryDelegate UIPhotoGalleryClosedEvent;

	// Token: 0x060002FD RID: 765 RVA: 0x0000442D File Offset: 0x0000262D
	protected override void OnStart()
	{
		base.OnStart();
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001BAC4 File Offset: 0x00019CC4
	public void ShowPhotoGallery(GirlDefinition initialPhotoGirl = null, int initialPhotoIndex = 0, bool singlePhotoMode = false, bool allSaveDataMode = false)
	{
		List<PhotoGalleryGirlPhoto> list = new List<PhotoGalleryGirlPhoto>();
		List<GirlDefinition> all = GameManager.Data.Girls.GetAll();
		for (int i = 0; i < all.Count; i++)
		{
			for (int j = 0; j < all[i].photos.Count; j++)
			{
				list.Add(new PhotoGalleryGirlPhoto(all[i], j, all[i].photos[j], false));
			}
		}
		if (!allSaveDataMode)
		{
			for (int k = 0; k < list.Count; k++)
			{
				if (GameManager.System.Player.GetGirlData(list[k].girlDef).IsPhotoEarned(k % 4))
				{
					list[k].unlocked = true;
				}
			}
		}
		else if (allSaveDataMode && SaveUtils.IsInited())
		{
			for (int l = 0; l < 4; l++)
			{
				SaveFile saveFile = SaveUtils.GetSaveFile(l);
				if (saveFile.started)
				{
					for (int m = 0; m < list.Count; m++)
					{
						if (saveFile.girls[list[m].girlDef.id].photosEarned.Contains(m % 4))
						{
							list[m].unlocked = true;
						}
					}
				}
			}
		}
		if (initialPhotoGirl == null)
		{
			singlePhotoMode = false;
		}
		this._photoGallery = (UnityEngine.Object.Instantiate(this.photoGalleryPrefab) as GameObject).GetComponent<PhotoGallery>();
		this._photoGallery.Init(list, initialPhotoGirl, initialPhotoIndex, singlePhotoMode);
		this._photoGallery.PhotoGalleryClosedEvent += this.HidePhotoGallery;
		base.AddChild(this._photoGallery);
		GameManager.System.Cursor.SetAbsorber(this, false);
		this._photoGallery.photoSection.interactive = false;
		TweenUtils.KillTweener(this._galleryTween, true);
		if (this._photoGallery.singlePhotoMode)
		{
			this._galleryTween = HOTween.To(this._photoGallery.fadeCover, 4f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine).OnComplete(new TweenDelegate.TweenCallback(this.PhotoGalleryShown)));
		}
		else
		{
			this.PhotoGalleryShown();
		}
		if (!this._photoGallery.singlePhotoMode)
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.openSound, false, 1f, true);
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000047AA File Offset: 0x000029AA
	private void PhotoGalleryShown()
	{
		this._photoGallery.photoSection.interactive = true;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0001BD58 File Offset: 0x00019F58
	public void HidePhotoGallery()
	{
		this._photoGallery.PhotoGalleryClosedEvent -= this.HidePhotoGallery;
		if (!this._photoGallery.singlePhotoMode)
		{
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.closeSound, false, 1f, true);
		}
		this._photoGallery.interactive = false;
		TweenUtils.KillTweener(this._galleryTween, true);
		if (this._photoGallery.singlePhotoMode)
		{
			this._galleryTween = HOTween.To(this._photoGallery.fadeCover, 4f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine).OnComplete(new TweenDelegate.TweenCallback(this.PhotoGalleryHidden)));
			GameManager.Stage.background.StopBackgroundMusic(4f);
		}
		else
		{
			this.PhotoGalleryHidden();
		}
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0001BE38 File Offset: 0x0001A038
	private void PhotoGalleryHidden()
	{
		UnityEngine.Object.Destroy(this._photoGallery.gameObj);
		this._photoGallery = null;
		Resources.UnloadUnusedAssets();
		GameManager.System.Cursor.ClearAbsorber();
		if (this.UIPhotoGalleryClosedEvent != null)
		{
			this.UIPhotoGalleryClosedEvent();
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001BE88 File Offset: 0x0001A088
	protected override void Destructor()
	{
		base.Destructor();
		if (this._photoGallery != null)
		{
			this._photoGallery.PhotoGalleryClosedEvent -= this.HidePhotoGallery;
			UnityEngine.Object.Destroy(this._photoGallery.gameObj);
		}
		this._photoGallery = null;
	}

	// Token: 0x040002BD RID: 701
	public GameObject photoGalleryPrefab;

	// Token: 0x040002BE RID: 702
	public AudioDefinition openSound;

	// Token: 0x040002BF RID: 703
	public AudioDefinition closeSound;

	// Token: 0x040002C0 RID: 704
	public AudioDefinition flipSound;

	// Token: 0x040002C1 RID: 705
	private PhotoGallery _photoGallery;

	// Token: 0x040002C2 RID: 706
	private Tweener _galleryTween;

	// Token: 0x02000063 RID: 99
	// (Invoke) Token: 0x06000305 RID: 773
	public delegate void UIPhotoGalleryDelegate();
}
