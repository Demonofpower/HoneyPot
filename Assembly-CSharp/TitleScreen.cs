using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class TitleScreen : DisplayObject
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600035E RID: 862 RVA: 0x00004C9C File Offset: 0x00002E9C
	// (remove) Token: 0x0600035F RID: 863 RVA: 0x00004CB5 File Offset: 0x00002EB5
	public event TitleScreen.TitleScreenDelegate SelectSaveFileEvent;

	// Token: 0x06000360 RID: 864 RVA: 0x0001D970 File Offset: 0x0001BB70
	public void Init()
	{
		this.background = (base.GetChildByName("TitleScreenBackground") as SpriteObject);
		this.sunrise = (base.GetChildByName("TitleScreenSunrise") as SpriteObject);
		this.cloudContainer = base.GetChildByName("TitleScreenClouds");
		this.sparkleContainer = base.GetChildByName("TitleScreenSparkleContainer");
		this.girlsContainer = base.GetChildByName("TitleScreenGirls");
		this.tokenContainer = base.GetChildByName("TitleScreenTokens");
		this.clickPrompt = (base.GetChildByName("TitleScreenPrompt") as SpriteObject);
		this.logo = (base.GetChildByName("TitleScreenLogo") as SpriteObject);
		this.flasher = (base.GetChildByName("TitleScreenFlash") as SpriteObject);
		this.clouds = new List<SpriteObject>();
		for (int i = 0; i < 2; i++)
		{
			this.clouds.Add(this.cloudContainer.GetChildByName("TitleScreenCloud" + i.ToString()) as SpriteObject);
		}
		this.girls = new List<SpriteObject>();
		for (int j = 0; j < 10; j++)
		{
			this.girls.Add(this.girlsContainer.GetChildByName("TitleScreenGirl" + j.ToString()) as SpriteObject);
		}
		this.tokens = new List<SpriteObject>();
		for (int k = 0; k < 8; k++)
		{
			this.tokens.Add(this.tokenContainer.GetChildByName("TitleScreenToken" + k.ToString()) as SpriteObject);
		}
		this.background.SetAlpha(0f, 0f);
		this.sunrise.SetAlpha(0f, 0f);
		this.clickPrompt.SetAlpha(0f, 0f);
		this.logo.SetAlpha(0f, 0f);
		this.flasher.SetAlpha(0f, 0f);
		for (int l = 0; l < this.clouds.Count; l++)
		{
			this.clouds[l].SetAlpha(0f, 0f);
		}
		for (int m = 0; m < this.girls.Count; m++)
		{
			this.girls[m].SetAlpha(0f, 0f);
		}
		for (int n = 0; n < this.tokens.Count; n++)
		{
			this.tokens[n].SetAlpha(0f, 0f);
		}
		this.girls[0].localY -= 40f;
		this.girls[1].localY -= 40f;
		this.girls[2].localX -= 40f;
		this.girls[3].localX += 40f;
		this.girls[4].localX -= 60f;
		this.girls[5].localX += 60f;
		this.girls[6].localX -= 80f;
		this.girls[7].localX += 80f;
		this.girls[8].localX -= 100f;
		this.girls[9].localX += 100f;
		this._titleSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnCompanyLogoSequenceComplete)));
		this._titleSequence.Insert(0f, HOTween.To(this.background, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._titleSequence.Play();
		this._startIntroSequence = false;
		this._startLoopSequence = false;
		this._loadScreen = (base.GetChildByName("LoadScreen") as LoadScreen);
		this._loadScreen.Init();
		this._loadScreen.interactive = false;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00004CCE File Offset: 0x00002ECE
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this._startIntroSequence)
		{
			this.StartIntroSequence();
		}
		else if (this._startLoopSequence)
		{
			this.StartLoopSequence();
		}
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00004CFD File Offset: 0x00002EFD
	private void OnCompanyLogoSequenceComplete()
	{
		this._startIntroSequence = true;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0001DDEC File Offset: 0x0001BFEC
	private void StartIntroSequence()
	{
		this._startIntroSequence = false;
		TweenUtils.KillSequence(this._titleSequence, true);
		this._titleSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnIntroSequenceComplete)));
		this._titleSequence.Insert(0f, HOTween.To(this.background, 2f, new TweenParms().Prop("spriteAlpha", 0.5f).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0f, HOTween.To(this.girls[0], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0f, HOTween.To(this.girls[0], 1f, new TweenParms().Prop("localY", this.girls[0].localY + 40f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0f, HOTween.To(this.girls[1], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0f, HOTween.To(this.girls[1], 1f, new TweenParms().Prop("localY", this.girls[1].localY + 40f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.25f, HOTween.To(this.girls[2], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.25f, HOTween.To(this.girls[2], 1f, new TweenParms().Prop("localX", this.girls[2].localX + 40f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.25f, HOTween.To(this.girls[3], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.25f, HOTween.To(this.girls[3], 1f, new TweenParms().Prop("localX", this.girls[3].localX - 40f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.5f, HOTween.To(this.girls[4], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.5f, HOTween.To(this.girls[4], 1f, new TweenParms().Prop("localX", this.girls[4].localX + 60f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.5f, HOTween.To(this.girls[5], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.5f, HOTween.To(this.girls[5], 1f, new TweenParms().Prop("localX", this.girls[5].localX - 60f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.75f, HOTween.To(this.girls[6], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.75f, HOTween.To(this.girls[6], 1f, new TweenParms().Prop("localX", this.girls[6].localX + 80f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(0.75f, HOTween.To(this.girls[7], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.75f, HOTween.To(this.girls[7], 1f, new TweenParms().Prop("localX", this.girls[7].localX - 80f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(1f, HOTween.To(this.girls[8], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(1f, HOTween.To(this.girls[8], 1f, new TweenParms().Prop("localX", this.girls[8].localX + 100f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(1f, HOTween.To(this.girls[9], 0.25f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(1f, HOTween.To(this.girls[9], 1f, new TweenParms().Prop("localX", this.girls[9].localX - 100f).Ease(EaseType.EaseOutBack)));
		this._titleSequence.Insert(1.9f, HOTween.To(this.flasher, 0.2f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInCubic)));
		this._titleSequence.Play();
		this._titleScreenMusicLink = GameManager.System.Audio.Play(AudioCategory.MUSIC, this.titleScreenMusic, true, 0.5f, true);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00004D06 File Offset: 0x00002F06
	private void OnIntroSequenceComplete()
	{
		this._startLoopSequence = true;
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001E4E8 File Offset: 0x0001C6E8
	private void StartLoopSequence()
	{
		this._startLoopSequence = false;
		TweenUtils.KillSequence(this._titleSequence, true);
		this.background.SetAlpha(1f, 0f);
		this.sunrise.SetAlpha(1f, 0f);
		this.logo.SetAlpha(0.25f, 0f);
		for (int i = 0; i < this.clouds.Count; i++)
		{
			this.clouds[i].SetAlpha(0.94f, 0f);
		}
		for (int j = 0; j < this.tokens.Count; j++)
		{
			this.tokens[j].SetAlpha(1f, 0f);
		}
		this._logoSequence = new Sequence(new SequenceParms().Loops(-1));
		this._logoSequence.Insert(0f, HOTween.To(this.logo.gameObj.transform, 0.6f, new TweenParms().Prop("localScale", new Vector3(0.9f, 1.1f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.35f, new TweenParms().Prop("localScale", new Vector3(1.1f, 0.9f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.32f, new TweenParms().Prop("localScale", new Vector3(0.95f, 1.05f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.29f, new TweenParms().Prop("localScale", new Vector3(1.05f, 0.95f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.26f, new TweenParms().Prop("localScale", new Vector3(0.975f, 1.025f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.23f, new TweenParms().Prop("localScale", new Vector3(1.025f, 0.975f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo.gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._logoSequence.Append(HOTween.To(this.logo, 3f, new TweenParms().Prop("spriteColor", this.logo.spriteColor)));
		this._logoSequence.GoToAndPlay(0.6f);
		this._titleTweeners = new List<Tweener>();
		this._titleTweeners.Add(HOTween.To(this.sunrise.gameObj.transform, 10f, new TweenParms().Prop("rotation", new Vector3(0f, 0f, -180f)).Ease(EaseType.Linear).Loops(-1, LoopType.Incremental)));
		this._titleTweeners.Add(HOTween.To(this.clouds[0], 2f, new TweenParms().Prop("localY", this.clouds[0].localY - 100f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners.Add(HOTween.To(this.clouds[1], 2f, new TweenParms().Prop("localY", this.clouds[1].localY + 100f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners.Add(HOTween.To(this.tokens[0], 2.5f, new TweenParms().Prop("localY", this.tokens[0].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners.Add(HOTween.To(this.tokens[1], 2.5f, new TweenParms().Prop("localY", this.tokens[1].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(3.5f);
		this._titleTweeners.Add(HOTween.To(this.tokens[2], 2.5f, new TweenParms().Prop("localY", this.tokens[2].localY - 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(1f);
		this._titleTweeners.Add(HOTween.To(this.tokens[3], 2.5f, new TweenParms().Prop("localY", this.tokens[3].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(0.5f);
		this._titleTweeners.Add(HOTween.To(this.tokens[4], 2.5f, new TweenParms().Prop("localY", this.tokens[4].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(3f);
		this._titleTweeners.Add(HOTween.To(this.tokens[5], 2.5f, new TweenParms().Prop("localY", this.tokens[5].localY - 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners.Add(HOTween.To(this.tokens[6], 2.5f, new TweenParms().Prop("localY", this.tokens[6].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(1f);
		this._titleTweeners.Add(HOTween.To(this.tokens[7], 2.5f, new TweenParms().Prop("localY", this.tokens[7].localY + 25f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo)));
		this._titleTweeners[this._titleTweeners.Count - 1].GoTo(2.5f);
		this._logoTweener = HOTween.To(this.logo, 1.25f, new TweenParms().Prop("localY", this.logo.localY + 16f).Ease(EaseType.EaseInOutSine).Loops(-1, LoopType.Yoyo));
		this._promptTweener = HOTween.To(this.clickPrompt, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInOutCubic).Loops(-1, LoopType.Yoyo));
		this._titleSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnLoopSequenceStarted)));
		this._titleSequence.Insert(0f, HOTween.To(this.flasher, 1f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0f, HOTween.To(this.logo, 0.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Play();
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00004D0F File Offset: 0x00002F0F
	private void OnLoopSequenceStarted()
	{
		GameManager.Stage.MouseDownEvent += this.OnTitleScreenClicked;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001EE40 File Offset: 0x0001D040
	private void OnTitleScreenClicked(DisplayObject displayObject)
	{
		GameManager.Stage.MouseDownEvent -= this.OnTitleScreenClicked;
		TweenUtils.KillSequence(this._titleSequence, true);
		TweenUtils.KillSequence(this._logoSequence, false);
		TweenUtils.KillTweener(this._logoTweener, false);
		TweenUtils.KillTweener(this._promptTweener, false);
		this._titleSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnLoadScreenShown)));
		this._titleSequence.Insert(0f, HOTween.To(this.logo, 1f, new TweenParms().Prop("localY", -175).Ease(EaseType.EaseInOutSine)));
		this._titleSequence.Insert(0f, HOTween.To(this.logo.gameObj.transform, 1f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseInOutSine)));
		this._titleSequence.Insert(0f, HOTween.To(this.clickPrompt, 0.5f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0f, HOTween.To(this._loadScreen.background, 0.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine)));
		for (int i = 0; i < this._loadScreen.saveFiles.Count; i++)
		{
			this._titleSequence.Insert(0.2f + (float)i * 0.1f, HOTween.To(this._loadScreen.saveFiles[i], 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
			this._titleSequence.Insert(0.2f + (float)i * 0.1f, HOTween.To(this._loadScreen.saveFiles[i], 0.5f, new TweenParms().Prop("localY", this._loadScreen.saveFiles[i].origY).Ease(EaseType.EaseOutBack)));
		}
		this._titleSequence.Insert(0.25f, HOTween.To(this._loadScreen.screenNotes, 0.5f, new TweenParms().Prop("childrenAlpha", 0.75f).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.75f, HOTween.To(this._loadScreen.buttonContainer, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Play();
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00004D27 File Offset: 0x00002F27
	private void OnLoadScreenShown()
	{
		this._loadScreen.SaveFileSelectedEvent += this.OnSaveFileSelected;
		this._loadScreen.interactive = true;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001F134 File Offset: 0x0001D334
	private void OnSaveFileSelected(int saveFileIndex)
	{
		this._loadScreen.SaveFileSelectedEvent -= this.OnSaveFileSelected;
		this._loadScreen.interactive = false;
		TweenUtils.KillSequence(this._titleSequence, true);
		this._titleSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallbackWParms(this.OnLoadScreenHidden), new object[]
		{
			saveFileIndex
		}));
		this._titleSequence.Insert(0f, HOTween.To(this._loadScreen.buttonContainer, 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
		this._titleSequence.Insert(0.25f, HOTween.To(this._loadScreen.screenNotes, 0.5f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
		for (int i = this._loadScreen.saveFiles.Count - 1; i >= 0; i--)
		{
			this._titleSequence.Insert(0.25f + (0.3f - (float)i * 0.1f), HOTween.To(this._loadScreen.saveFiles[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			this._titleSequence.Insert(0.3f - (float)i * 0.1f, HOTween.To(this._loadScreen.saveFiles[i], 0.5f, new TweenParms().Prop("localY", this._loadScreen.saveFiles[i].origY - 60f).Ease(EaseType.EaseInBack)));
		}
		this._titleSequence.Insert(1f, HOTween.To(GameManager.Stage.transitionScreen.overlay, 1.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Insert(2.5f, HOTween.To(GameManager.Stage.transitionScreen.overlay, 1.5f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.Linear)));
		this._titleSequence.Play();
		this._titleScreenMusicLink.FadeOut(3.5f);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001F3AC File Offset: 0x0001D5AC
	private void OnLoadScreenHidden(TweenEvent data)
	{
		int saveFileIndex = (int)data.parms[0];
		if (this.SelectSaveFileEvent != null)
		{
			this.SelectSaveFileEvent(saveFileIndex);
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
	protected override void Destructor()
	{
		base.Destructor();
		this._titleScreenMusicLink = null;
		TweenUtils.KillSequence(this._titleSequence, false);
		this._titleSequence = null;
		TweenUtils.KillSequence(this._logoSequence, false);
		this._logoSequence = null;
		if (this._titleTweeners != null)
		{
			for (int i = 0; i < this._titleTweeners.Count; i++)
			{
				TweenUtils.KillTweener(this._titleTweeners[i], false);
			}
			this._titleTweeners.Clear();
		}
		this._titleTweeners = null;
		TweenUtils.KillTweener(this._logoTweener, false);
		this._logoTweener = null;
		TweenUtils.KillTweener(this._promptTweener, false);
		this._promptTweener = null;
	}

	// Token: 0x04000335 RID: 821
	private const int GIRL_COUNT = 10;

	// Token: 0x04000336 RID: 822
	private const int TOKEN_COUNT = 8;

	// Token: 0x04000337 RID: 823
	private const int CLOUD_COUNT = 2;

	// Token: 0x04000338 RID: 824
	public AudioDefinition titleScreenMusic;

	// Token: 0x04000339 RID: 825
	public ParticleEmitter2DDefinition sparkleParticleEmitter;

	// Token: 0x0400033A RID: 826
	public SpriteGroupDefinition sparkleSpriteGroup;

	// Token: 0x0400033B RID: 827
	public SpriteObject background;

	// Token: 0x0400033C RID: 828
	public SpriteObject sunrise;

	// Token: 0x0400033D RID: 829
	public DisplayObject cloudContainer;

	// Token: 0x0400033E RID: 830
	public List<SpriteObject> clouds;

	// Token: 0x0400033F RID: 831
	public DisplayObject sparkleContainer;

	// Token: 0x04000340 RID: 832
	public DisplayObject girlsContainer;

	// Token: 0x04000341 RID: 833
	public List<SpriteObject> girls;

	// Token: 0x04000342 RID: 834
	public DisplayObject tokenContainer;

	// Token: 0x04000343 RID: 835
	public List<SpriteObject> tokens;

	// Token: 0x04000344 RID: 836
	public SpriteObject clickPrompt;

	// Token: 0x04000345 RID: 837
	public SpriteObject logo;

	// Token: 0x04000346 RID: 838
	public SpriteObject flasher;

	// Token: 0x04000347 RID: 839
	private bool _startIntroSequence;

	// Token: 0x04000348 RID: 840
	private bool _startLoopSequence;

	// Token: 0x04000349 RID: 841
	private Sequence _titleSequence;

	// Token: 0x0400034A RID: 842
	private Sequence _logoSequence;

	// Token: 0x0400034B RID: 843
	private List<Tweener> _titleTweeners;

	// Token: 0x0400034C RID: 844
	private Tweener _logoTweener;

	// Token: 0x0400034D RID: 845
	private Tweener _promptTweener;

	// Token: 0x0400034E RID: 846
	private AudioLink _titleScreenMusicLink;

	// Token: 0x0400034F RID: 847
	private LoadScreen _loadScreen;

	// Token: 0x02000071 RID: 113
	// (Invoke) Token: 0x0600036D RID: 877
	public delegate void TitleScreenDelegate(int saveFileIndex);
}
