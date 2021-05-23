using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
[AddComponentMenu("2D Toolkit/Sprite/tk2dAnimatedSprite (Obsolete)")]
public class tk2dAnimatedSprite : tk2dSprite
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000860 RID: 2144 RVA: 0x000085CE File Offset: 0x000067CE
	public tk2dSpriteAnimator Animator
	{
		get
		{
			this.CheckAddAnimatorInternal();
			return this._animator;
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0003D02C File Offset: 0x0003B22C
	private void CheckAddAnimatorInternal()
	{
		if (this._animator == null)
		{
			this._animator = base.gameObject.GetComponent<tk2dSpriteAnimator>();
			if (this._animator == null)
			{
				this._animator = base.gameObject.AddComponent<tk2dSpriteAnimator>();
				this._animator.Library = this.anim;
				this._animator.DefaultClipId = this.clipId;
				this._animator.playAutomatically = this.playAutomatically;
			}
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x000085DC File Offset: 0x000067DC
	protected override bool NeedBoxCollider()
	{
		return this.createCollider;
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000863 RID: 2147 RVA: 0x000085E4 File Offset: 0x000067E4
	// (set) Token: 0x06000864 RID: 2148 RVA: 0x000085F1 File Offset: 0x000067F1
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.Animator.Library;
		}
		set
		{
			this.Animator.Library = value;
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000865 RID: 2149 RVA: 0x000085FF File Offset: 0x000067FF
	// (set) Token: 0x06000866 RID: 2150 RVA: 0x0000860C File Offset: 0x0000680C
	public int DefaultClipId
	{
		get
		{
			return this.Animator.DefaultClipId;
		}
		set
		{
			this.Animator.DefaultClipId = value;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000867 RID: 2151 RVA: 0x0000861A File Offset: 0x0000681A
	// (set) Token: 0x06000868 RID: 2152 RVA: 0x00008621 File Offset: 0x00006821
	public static bool g_paused
	{
		get
		{
			return tk2dSpriteAnimator.g_Paused;
		}
		set
		{
			tk2dSpriteAnimator.g_Paused = value;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x00008629 File Offset: 0x00006829
	// (set) Token: 0x0600086A RID: 2154 RVA: 0x00008636 File Offset: 0x00006836
	public bool Paused
	{
		get
		{
			return this.Animator.Paused;
		}
		set
		{
			this.Animator.Paused = value;
		}
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
	private void ProxyCompletedHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		if (this.animationCompleteDelegate != null)
		{
			int num = -1;
			tk2dSpriteAnimationClip[] array = (!(anim.Library != null)) ? null : anim.Library.clips;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == clip)
					{
						num = i;
						break;
					}
				}
			}
			this.animationCompleteDelegate(this, num);
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00008644 File Offset: 0x00006844
	private void ProxyEventTriggeredHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.animationEventDelegate != null)
		{
			this.animationEventDelegate(this, clip, clip.frames[frame], frame);
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00008667 File Offset: 0x00006867
	private void OnEnable()
	{
		this.Animator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.ProxyCompletedHandler);
		this.Animator.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.ProxyEventTriggeredHandler);
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00008697 File Offset: 0x00006897
	private void OnDisable()
	{
		this.Animator.AnimationCompleted = null;
		this.Animator.AnimationEventTriggered = null;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x000086B1 File Offset: 0x000068B1
	private void Start()
	{
		this.CheckAddAnimatorInternal();
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0003D124 File Offset: 0x0003B324
	public static tk2dAnimatedSprite AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dAnimatedSprite tk2dAnimatedSprite = go.AddComponent<tk2dAnimatedSprite>();
		tk2dAnimatedSprite.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		tk2dAnimatedSprite.anim = anim;
		return tk2dAnimatedSprite;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x000086B9 File Offset: 0x000068B9
	public void Play()
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.Play(this.Animator.DefaultClip);
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x000086E1 File Offset: 0x000068E1
	public void Play(float clipStartTime)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFrom(this.Animator.DefaultClip, clipStartTime);
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0000870A File Offset: 0x0000690A
	public void PlayFromFrame(int frame)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFromFrame(this.Animator.DefaultClip, frame);
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00008733 File Offset: 0x00006933
	public void Play(string name)
	{
		this.Animator.Play(name);
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00008741 File Offset: 0x00006941
	public void PlayFromFrame(string name, int frame)
	{
		this.Animator.PlayFromFrame(name, frame);
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00008750 File Offset: 0x00006950
	public void Play(string name, float clipStartTime)
	{
		this.Animator.PlayFrom(name, clipStartTime);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0000875F File Offset: 0x0000695F
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Animator.PlayFrom(clip, clipStartTime);
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0000876E File Offset: 0x0000696E
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
	{
		this.Animator.Play(clip, clipStartTime, overrideFps);
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x0000877E File Offset: 0x0000697E
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.Animator.CurrentClip;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0000878B File Offset: 0x0000698B
	public float ClipTimeSeconds
	{
		get
		{
			return this.Animator.ClipTimeSeconds;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600087B RID: 2171 RVA: 0x00008798 File Offset: 0x00006998
	// (set) Token: 0x0600087C RID: 2172 RVA: 0x000087A5 File Offset: 0x000069A5
	public float ClipFps
	{
		get
		{
			return this.Animator.ClipFps;
		}
		set
		{
			this.Animator.ClipFps = value;
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x000087B3 File Offset: 0x000069B3
	public void Stop()
	{
		this.Animator.Stop();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x000087C0 File Offset: 0x000069C0
	public void StopAndResetFrame()
	{
		this.Animator.StopAndResetFrame();
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x000087CD File Offset: 0x000069CD
	[Obsolete]
	public bool isPlaying()
	{
		return this.Animator.Playing;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000087CD File Offset: 0x000069CD
	public bool IsPlaying(string name)
	{
		return this.Animator.Playing;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x000087DA File Offset: 0x000069DA
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Animator.IsPlaying(clip);
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x000087CD File Offset: 0x000069CD
	public bool Playing
	{
		get
		{
			return this.Animator.Playing;
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x000087E8 File Offset: 0x000069E8
	public int GetClipIdByName(string name)
	{
		return this.Animator.GetClipIdByName(name);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x000087F6 File Offset: 0x000069F6
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return this.Animator.GetClipByName(name);
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x00008804 File Offset: 0x00006A04
	public static float DefaultFps
	{
		get
		{
			return tk2dSpriteAnimator.DefaultFps;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0000880B File Offset: 0x00006A0B
	public void Pause()
	{
		this.Animator.Pause();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00008818 File Offset: 0x00006A18
	public void Resume()
	{
		this.Animator.Resume();
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00008825 File Offset: 0x00006A25
	public void SetFrame(int currFrame)
	{
		this.Animator.SetFrame(currFrame);
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00008833 File Offset: 0x00006A33
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		this.Animator.SetFrame(currFrame, triggerEvent);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00008842 File Offset: 0x00006A42
	public void UpdateAnimation(float deltaTime)
	{
		this.Animator.UpdateAnimation(deltaTime);
	}

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private tk2dSpriteAnimator _animator;

	// Token: 0x040009C0 RID: 2496
	[SerializeField]
	private tk2dSpriteAnimation anim;

	// Token: 0x040009C1 RID: 2497
	[SerializeField]
	private int clipId;

	// Token: 0x040009C2 RID: 2498
	public bool playAutomatically;

	// Token: 0x040009C3 RID: 2499
	public bool createCollider;

	// Token: 0x040009C4 RID: 2500
	public tk2dAnimatedSprite.AnimationCompleteDelegate animationCompleteDelegate;

	// Token: 0x040009C5 RID: 2501
	public tk2dAnimatedSprite.AnimationEventDelegate animationEventDelegate;

	// Token: 0x02000163 RID: 355
	// (Invoke) Token: 0x0600088C RID: 2188
	public delegate void AnimationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId);

	// Token: 0x02000164 RID: 356
	// (Invoke) Token: 0x06000890 RID: 2192
	public delegate void AnimationEventDelegate(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum);
}
