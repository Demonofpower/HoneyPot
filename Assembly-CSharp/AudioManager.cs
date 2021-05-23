using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class AudioManager : MonoBehaviour
{
	// Token: 0x060005FF RID: 1535 RVA: 0x0002E8A8 File Offset: 0x0002CAA8
	public AudioLink Play(AudioCategory category, AudioDefinition audioDefinition, bool loop = false, float volume = 1f, bool pausable = true)
	{
		if (audioDefinition == null || audioDefinition.clip == null)
		{
			return null;
		}
		if (loop)
		{
			for (int i = 0; i < this._playingAudio.Count; i++)
			{
				if (this._playingAudio[i].audioDefinition == audioDefinition && this._playingAudio[i].audioSource.loop && this._playingAudio[i].audioSource.isPlaying)
				{
					return this._playingAudio[i];
				}
			}
		}
		AudioSource audioSource = GameManager.System.gameCamera.gameObject.AddComponent("AudioSource") as AudioSource;
		audioSource.clip = audioDefinition.clip;
		audioSource.playOnAwake = false;
		audioSource.loop = loop;
		audioSource.volume = volume;
		AudioLink audioLink = new AudioLink(category, audioDefinition, audioSource, pausable);
		if (this._paused)
		{
			audioLink.pausable = false;
		}
		this._playingAudio.Add(audioLink);
		return audioLink;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
	public AudioLink Play(AudioCategory category, AudioGroup audioGroup, int groupIndex = -1, bool loop = false, float volume = 1f, bool pausable = true)
	{
		if (audioGroup == null || audioGroup.audioDefs.Count == 0)
		{
			return null;
		}
		if (groupIndex >= 0)
		{
			groupIndex = Mathf.Clamp(groupIndex, 0, audioGroup.audioDefs.Count - 1);
			return this.Play(category, audioGroup.audioDefs[groupIndex], loop, volume, pausable);
		}
		return this.Play(category, audioGroup.audioDefs[UnityEngine.Random.Range(0, audioGroup.audioDefs.Count)], loop, volume, pausable);
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002EA40 File Offset: 0x0002CC40
	public void Stop(AudioDefinition audioDefinition)
	{
		if (audioDefinition == null || audioDefinition.clip == null)
		{
			return;
		}
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			if (this._playingAudio[i].audioDefinition == audioDefinition)
			{
				this._playingAudio[i].Destructor();
				this._playingAudio.RemoveAt(i);
				break;
			}
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002EABC File Offset: 0x0002CCBC
	public void Refresh()
	{
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			this._playingAudio[i].Refresh();
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002EAF8 File Offset: 0x0002CCF8
	private void Update()
	{
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			if (this._playingAudio[i].IsComplete())
			{
				this._playingAudio[i].Destructor();
				this._playingAudio.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002EB58 File Offset: 0x0002CD58
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			this._playingAudio[i].Pause();
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002EBA8 File Offset: 0x0002CDA8
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			this._playingAudio[i].Unpause();
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
	private void OnDestroy()
	{
		for (int i = 0; i < this._playingAudio.Count; i++)
		{
			this._playingAudio[i].Destructor();
		}
		this._playingAudio.Clear();
		this._playingAudio = null;
	}

	// Token: 0x0400075B RID: 1883
	private List<AudioLink> _playingAudio = new List<AudioLink>();

	// Token: 0x0400075C RID: 1884
	private bool _paused;
}
