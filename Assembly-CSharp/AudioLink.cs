using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class AudioLink
{
	// Token: 0x060005F5 RID: 1525 RVA: 0x0002E628 File Offset: 0x0002C828
	public AudioLink(AudioCategory category, AudioDefinition definition, AudioSource source, bool isPausable)
	{
		this.audioCategory = category;
		this.audioDefinition = definition;
		this.audioSource = source;
		this.pausable = isPausable;
		if (category == AudioCategory.MUSIC)
		{
			this.pausable = false;
		}
		this.baseVolume = this.audioSource.volume;
		this.currentVolume = this.baseVolume;
		this._paused = false;
		this._fadingOut = false;
		this.audioSource.volume *= this.SettingsVolumeMultiplier();
		this.audioSource.Play();
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002E6B4 File Offset: 0x0002C8B4
	public void VolumeTo(float volume, float duration = 0f)
	{
		if (this._paused || this._fadingOut)
		{
			return;
		}
		TweenUtils.KillTweener(this._volumeTweener, false);
		this.currentVolume = volume;
		volume *= this.SettingsVolumeMultiplier();
		if (this.SettingsVolumeMultiplier() == 0f)
		{
			duration = 0f;
		}
		if (duration > 0f)
		{
			this._volumeTweener = HOTween.To(this.audioSource, duration, new TweenParms().Prop("volume", volume).Ease(EaseType.Linear));
		}
		else
		{
			this.audioSource.volume = volume;
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x000067B4 File Offset: 0x000049B4
	public void FadeOut(float duration)
	{
		this.VolumeTo(0f, duration);
		this._fadingOut = true;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002E758 File Offset: 0x0002C958
	public float SettingsVolumeMultiplier()
	{
		if (this.audioCategory == AudioCategory.MUSIC)
		{
			return (float)GameManager.System.settingsMusicVol / 10f;
		}
		if (this.audioCategory == AudioCategory.SOUND)
		{
			return (float)GameManager.System.settingsSoundVol / 10f;
		}
		if ((this.audioCategory == AudioCategory.VOICE && GameManager.System.settingsVoice == SettingsVoice.BOOPS) || (this.audioCategory == AudioCategory.VOICE_ALT && GameManager.System.settingsVoice == SettingsVoice.FULL_VOICE))
		{
			return 0f;
		}
		return (float)GameManager.System.settingsVoiceVol / 10f;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x000067C9 File Offset: 0x000049C9
	public void Refresh()
	{
		if (this._fadingOut)
		{
			return;
		}
		this.audioSource.volume = this.currentVolume * this.SettingsVolumeMultiplier();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000067EF File Offset: 0x000049EF
	public bool IsComplete()
	{
		return (!this._paused && !this.audioSource.isPlaying) || (this._fadingOut && this.audioSource.volume == 0f);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002E7F0 File Offset: 0x0002C9F0
	public void Pause()
	{
		if (this._paused || !this.pausable)
		{
			return;
		}
		this._paused = true;
		this.audioSource.Pause();
		if (this._volumeTweener != null && !this._volumeTweener.isPaused)
		{
			this._volumeTweener.Pause();
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002E84C File Offset: 0x0002CA4C
	public void Unpause()
	{
		if (!this._paused || !this.pausable)
		{
			return;
		}
		this._paused = false;
		this.audioSource.Play();
		if (this._volumeTweener != null && this._volumeTweener.isPaused)
		{
			this._volumeTweener.Play();
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0000682F File Offset: 0x00004A2F
	public void Destructor()
	{
		TweenUtils.KillTweener(this._volumeTweener, false);
		this._volumeTweener = null;
		UnityEngine.Object.Destroy(this.audioSource);
		this.audioSource = null;
		this.audioDefinition = null;
	}

	// Token: 0x04000752 RID: 1874
	public AudioCategory audioCategory;

	// Token: 0x04000753 RID: 1875
	public AudioDefinition audioDefinition;

	// Token: 0x04000754 RID: 1876
	public AudioSource audioSource;

	// Token: 0x04000755 RID: 1877
	public bool pausable;

	// Token: 0x04000756 RID: 1878
	public float baseVolume;

	// Token: 0x04000757 RID: 1879
	public float currentVolume;

	// Token: 0x04000758 RID: 1880
	private bool _paused;

	// Token: 0x04000759 RID: 1881
	private bool _fadingOut;

	// Token: 0x0400075A RID: 1882
	private Tweener _volumeTweener;
}
