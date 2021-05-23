using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class Background : DisplayObject
{
	// Token: 0x06000081 RID: 129 RVA: 0x0000D824 File Offset: 0x0000BA24
	protected override void OnStart()
	{
		this.locationBackgrounds = base.GetChildByName("LocationBackgrounds");
		this.tintOverlay = (base.GetChildByName("UIWindowTintOverlay") as SpriteObject);
		this._morningArt = (this.locationBackgrounds.GetChildByName("MorningBackground") as SpriteObject);
		this._afternoonArt = (this.locationBackgrounds.GetChildByName("AfternoonBackground") as SpriteObject);
		this._eveningArt = (this.locationBackgrounds.GetChildByName("EveningBackground") as SpriteObject);
		this._nightArt = (this.locationBackgrounds.GetChildByName("NightBackground") as SpriteObject);
		this._panTimestamp = 0f;
		this.locationBackgrounds.SetLocalScale(0.95f, 0f, EaseType.Linear);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
	public void UpdateLocation()
	{
		this.currentSpriteCollection = (Resources.Load("SpriteCollections/Backgrounds/" + GameManager.System.Location.currentLocation.spriteCollectionName, typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData);
		foreach (object obj in Enum.GetValues(typeof(GameClockDaytime)))
		{
			GameClockDaytime gameClockDaytime = (GameClockDaytime)((int)obj);
			this.GetBackgroundArtByDaytime(gameClockDaytime).sprite.SetSprite(this.currentSpriteCollection, GameManager.System.Location.currentLocation.GetBackgroundByDaytime(gameClockDaytime).backgroundName);
		}
		this.locationBackgrounds.localY = GameManager.System.Location.currentLocation.backgroundYOffset;
		foreach (object obj2 in Enum.GetValues(typeof(GameClockDaytime)))
		{
			GameClockDaytime gameClockDaytime2 = (GameClockDaytime)((int)obj2);
			SpriteObject backgroundArtByDaytime = this.GetBackgroundArtByDaytime(gameClockDaytime2);
			if (gameClockDaytime2 == GameManager.System.Clock.DayTime(-1))
			{
				backgroundArtByDaytime.ShiftSelfToTop();
				backgroundArtByDaytime.SetAlpha(1f, 0f);
			}
			else
			{
				backgroundArtByDaytime.SetAlpha(0f, 0f);
			}
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0000DA80 File Offset: 0x0000BC80
	private SpriteObject GetBackgroundArtByDaytime(GameClockDaytime dayTime)
	{
		switch (dayTime)
		{
		case GameClockDaytime.MORNING:
			return this._morningArt;
		case GameClockDaytime.AFTERNOON:
			return this._afternoonArt;
		case GameClockDaytime.EVENING:
			return this._eveningArt;
		case GameClockDaytime.NIGHT:
			return this._nightArt;
		default:
			return this._nightArt;
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0000DACC File Offset: 0x0000BCCC
	public void PlayBackgroundMusic()
	{
		if (this._backgroundMusic != null)
		{
			return;
		}
		LocationBackground backgroundByDaytime = GameManager.System.Location.currentLocation.GetBackgroundByDaytime(GameManager.System.Clock.DayTime(-1));
		if (backgroundByDaytime.musicDefinition != null)
		{
			this._backgroundMusic = GameManager.System.Audio.Play(AudioCategory.MUSIC, backgroundByDaytime.musicDefinition, true, backgroundByDaytime.musicVolume, true);
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00002C94 File Offset: 0x00000E94
	public void StopBackgroundMusic(float duration = 0f)
	{
		if (this._backgroundMusic != null)
		{
			this._backgroundMusic.FadeOut(duration);
		}
		this._backgroundMusic = null;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00002CB4 File Offset: 0x00000EB4
	public void AdjustBackgroundMusicVolume(float multiplier, float duration = 0.5f)
	{
		if (this._backgroundMusic != null)
		{
			this._backgroundMusic.VolumeTo(this._backgroundMusic.baseVolume * multiplier, duration);
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x0000DB3C File Offset: 0x0000BD3C
	protected override void OnUpdate()
	{
		if (GameManager.System.Location.IsLocationSettled() && (GameManager.System.Lifetime(true) - this._panTimestamp >= 0.01f || this._panTimestamp == 0f))
		{
			float num = Mathf.Lerp(30f, -30f, GameManager.System.Cursor.GetMousePosition().x / 1200f);
			this.locationBackgrounds.localX += (num - this.locationBackgrounds.localX) / 40f;
			this._panTimestamp = GameManager.System.Lifetime(true);
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00002CDA File Offset: 0x00000EDA
	public LocationBackground GetBackgroundDefinition(GameClockDaytime daytime)
	{
		return GameManager.System.Location.currentLocation.GetBackgroundByDaytime(daytime);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00002CF1 File Offset: 0x00000EF1
	protected override void Destructor()
	{
		base.Destructor();
		if (this._backgroundMusic != null)
		{
			GameManager.System.Audio.Stop(this._backgroundMusic.audioDefinition);
		}
		this._backgroundMusic = null;
	}

	// Token: 0x0400006B RID: 107
	public DisplayObject locationBackgrounds;

	// Token: 0x0400006C RID: 108
	public SpriteObject tintOverlay;

	// Token: 0x0400006D RID: 109
	public tk2dSpriteCollectionData currentSpriteCollection;

	// Token: 0x0400006E RID: 110
	private SpriteObject _morningArt;

	// Token: 0x0400006F RID: 111
	private SpriteObject _afternoonArt;

	// Token: 0x04000070 RID: 112
	private SpriteObject _eveningArt;

	// Token: 0x04000071 RID: 113
	private SpriteObject _nightArt;

	// Token: 0x04000072 RID: 114
	private float _panTimestamp;

	// Token: 0x04000073 RID: 115
	private AudioLink _backgroundMusic;
}
