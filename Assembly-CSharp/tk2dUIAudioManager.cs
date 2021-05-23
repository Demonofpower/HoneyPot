using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIAudioManager")]
public class tk2dUIAudioManager : MonoBehaviour
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0004BC0C File Offset: 0x00049E0C
	public static tk2dUIAudioManager Instance
	{
		get
		{
			if (tk2dUIAudioManager.instance == null)
			{
				tk2dUIAudioManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(tk2dUIAudioManager)) as tk2dUIAudioManager);
				if (tk2dUIAudioManager.instance == null)
				{
					tk2dUIAudioManager.instance = new GameObject("tk2dUIAudioManager").AddComponent<tk2dUIAudioManager>();
				}
			}
			return tk2dUIAudioManager.instance;
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0000B03E File Offset: 0x0000923E
	private void Awake()
	{
		if (tk2dUIAudioManager.instance == null)
		{
			tk2dUIAudioManager.instance = this;
		}
		else if (tk2dUIAudioManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.Setup();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0004BC6C File Offset: 0x00049E6C
	private void Setup()
	{
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.GetComponent<AudioSource>();
		}
		if (this.audioSrc == null)
		{
			this.audioSrc = base.gameObject.AddComponent<AudioSource>();
			this.audioSrc.playOnAwake = false;
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0000B078 File Offset: 0x00009278
	public void Play(AudioClip clip)
	{
		this.audioSrc.PlayOneShot(clip);
	}

	// Token: 0x04000C5B RID: 3163
	private static tk2dUIAudioManager instance;

	// Token: 0x04000C5C RID: 3164
	private AudioSource audioSrc;
}
