using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAnimator")]
public class tk2dSpriteAnimator : MonoBehaviour
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00008F29 File Offset: 0x00007129
	// (set) Token: 0x06000924 RID: 2340 RVA: 0x00008F38 File Offset: 0x00007138
	public static bool g_Paused
	{
		get
		{
			return (tk2dSpriteAnimator.globalState & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			tk2dSpriteAnimator.globalState = ((!value) ? tk2dSpriteAnimator.State.Init : tk2dSpriteAnimator.State.Paused);
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x00008F4C File Offset: 0x0000714C
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x00008F5C File Offset: 0x0000715C
	public bool Paused
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			if (value)
			{
				this.state |= tk2dSpriteAnimator.State.Paused;
			}
			else
			{
				this.state &= (tk2dSpriteAnimator.State)(-3);
			}
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x00008F86 File Offset: 0x00007186
	// (set) Token: 0x06000928 RID: 2344 RVA: 0x00008F8E File Offset: 0x0000718E
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.library;
		}
		set
		{
			this.library = value;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x00008F97 File Offset: 0x00007197
	// (set) Token: 0x0600092A RID: 2346 RVA: 0x00008F9F File Offset: 0x0000719F
	public int DefaultClipId
	{
		get
		{
			return this.defaultClipId;
		}
		set
		{
			this.defaultClipId = value;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x00008FA8 File Offset: 0x000071A8
	public tk2dSpriteAnimationClip DefaultClip
	{
		get
		{
			return this.GetClipById(this.defaultClipId);
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00008FB6 File Offset: 0x000071B6
	private void OnEnable()
	{
		if (this.Sprite == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00008FD0 File Offset: 0x000071D0
	private void Start()
	{
		if (this.playAutomatically)
		{
			this.Play(this.DefaultClip);
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x00008FE9 File Offset: 0x000071E9
	public virtual tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.LogError("Sprite not found attached to tk2dSpriteAnimator.");
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0003FF94 File Offset: 0x0003E194
	public static tk2dSpriteAnimator AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dSpriteAnimator tk2dSpriteAnimator = go.AddComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator.Library = anim;
		tk2dSpriteAnimator.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		return tk2dSpriteAnimator;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0003FFDC File Offset: 0x0003E1DC
	private tk2dSpriteAnimationClip GetClipByNameVerbose(string name)
	{
		if (this.library == null)
		{
			Debug.LogError("Library not set");
			return null;
		}
		tk2dSpriteAnimationClip clipByName = this.library.GetClipByName(name);
		if (clipByName == null)
		{
			Debug.LogError("Unable to find clip '" + name + "' in library");
			return null;
		}
		return clipByName;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00009029 File Offset: 0x00007229
	public void Play()
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.Play(this.currentClip);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0000904E File Offset: 0x0000724E
	public void Play(string name)
	{
		this.Play(this.GetClipByNameVerbose(name));
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0000905D File Offset: 0x0000725D
	public void Play(tk2dSpriteAnimationClip clip)
	{
		this.Play(clip, 0f, tk2dSpriteAnimator.DefaultFps);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00009070 File Offset: 0x00007270
	public void PlayFromFrame(int frame)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFromFrame(this.currentClip, frame);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00009096 File Offset: 0x00007296
	public void PlayFromFrame(string name, int frame)
	{
		this.PlayFromFrame(this.GetClipByNameVerbose(name), frame);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x000090A6 File Offset: 0x000072A6
	public void PlayFromFrame(tk2dSpriteAnimationClip clip, int frame)
	{
		this.PlayFrom(clip, ((float)frame + 0.001f) / clip.fps);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x000090BE File Offset: 0x000072BE
	public void PlayFrom(float clipStartTime)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFrom(this.currentClip, clipStartTime);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x00040034 File Offset: 0x0003E234
	public void PlayFrom(string name, float clipStartTime)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = (!this.library) ? null : this.library.GetClipByName(name);
		if (tk2dSpriteAnimationClip == null)
		{
			this.ClipNameError(name);
		}
		else
		{
			this.PlayFrom(tk2dSpriteAnimationClip, clipStartTime);
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x000090E4 File Offset: 0x000072E4
	public void PlayFrom(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Play(clip, clipStartTime, tk2dSpriteAnimator.DefaultFps);
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00040080 File Offset: 0x0003E280
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
	{
		if (clip != null)
		{
			float num = (overrideFps <= 0f) ? clip.fps : overrideFps;
			bool flag = clipStartTime == 0f && this.IsPlaying(clip);
			if (flag)
			{
				this.clipFps = num;
			}
			else
			{
				this.state |= tk2dSpriteAnimator.State.Playing;
				this.currentClip = clip;
				this.clipFps = num;
				if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single || this.currentClip.frames == null)
				{
					this.WarpClipToLocalTime(this.currentClip, 0f);
					this.state &= (tk2dSpriteAnimator.State)(-2);
				}
				else if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame || this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomLoop)
				{
					int num2 = UnityEngine.Random.Range(0, this.currentClip.frames.Length);
					this.WarpClipToLocalTime(this.currentClip, (float)num2);
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame)
					{
						this.previousFrame = -1;
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
				}
				else
				{
					float num3 = clipStartTime * this.clipFps;
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Once && num3 >= this.clipFps * (float)this.currentClip.frames.Length)
					{
						this.WarpClipToLocalTime(this.currentClip, (float)(this.currentClip.frames.Length - 1));
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
					else
					{
						this.WarpClipToLocalTime(this.currentClip, num3);
						this.clipTime = num3;
					}
				}
			}
		}
		else
		{
			Debug.LogError("Calling clip.Play() with a null clip");
			this.OnAnimationCompleted();
			this.state &= (tk2dSpriteAnimator.State)(-2);
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x000090F3 File Offset: 0x000072F3
	public void Stop()
	{
		this.state &= (tk2dSpriteAnimator.State)(-2);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00009104 File Offset: 0x00007304
	public void StopAndResetFrame()
	{
		if (this.currentClip != null)
		{
			this.SetSprite(this.currentClip.frames[0].spriteCollection, this.currentClip.frames[0].spriteId);
		}
		this.Stop();
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00009141 File Offset: 0x00007341
	public bool IsPlaying(string name)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip.name == name;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0000916D File Offset: 0x0000736D
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip == clip;
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x00009191 File Offset: 0x00007391
	public bool Playing
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Playing) != tk2dSpriteAnimator.State.Init;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x000091A1 File Offset: 0x000073A1
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.currentClip;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000941 RID: 2369 RVA: 0x000091A9 File Offset: 0x000073A9
	public float ClipTimeSeconds
	{
		get
		{
			return (this.clipFps <= 0f) ? (this.clipTime / this.currentClip.fps) : (this.clipTime / this.clipFps);
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x000091DF File Offset: 0x000073DF
	// (set) Token: 0x06000943 RID: 2371 RVA: 0x000091E7 File Offset: 0x000073E7
	public float ClipFps
	{
		get
		{
			return this.clipFps;
		}
		set
		{
			if (this.currentClip != null)
			{
				this.clipFps = ((value <= 0f) ? this.currentClip.fps : value);
			}
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00009216 File Offset: 0x00007416
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (this.library == null)
		{
			return null;
		}
		return this.library.GetClipById(id);
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x00009237 File Offset: 0x00007437
	public static float DefaultFps
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0000923E File Offset: 0x0000743E
	public int GetClipIdByName(string name)
	{
		return (!this.library) ? -1 : this.library.GetClipIdByName(name);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00009262 File Offset: 0x00007462
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return (!this.library) ? null : this.library.GetClipByName(name);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00009286 File Offset: 0x00007486
	public void Pause()
	{
		this.state |= tk2dSpriteAnimator.State.Paused;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00009296 File Offset: 0x00007496
	public void Resume()
	{
		this.state &= (tk2dSpriteAnimator.State)(-3);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x000092A7 File Offset: 0x000074A7
	public void SetFrame(int currFrame)
	{
		this.SetFrame(currFrame, true);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00040244 File Offset: 0x0003E444
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		if (this.currentClip != null)
		{
			int num = currFrame % this.currentClip.frames.Length;
			this.SetFrameInternal(num);
			if (triggerEvent && this.currentClip.frames.Length > 0 && currFrame >= 0)
			{
				this.ProcessEvents(num - 1, num, 1);
			}
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x000402B8 File Offset: 0x0003E4B8
	public int CurrentFrame
	{
		get
		{
			switch (this.currentClip.wrapMode)
			{
			case tk2dSpriteAnimationClip.WrapMode.Loop:
			case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
				break;
			case tk2dSpriteAnimationClip.WrapMode.LoopSection:
			{
				int num = (int)this.clipTime;
				int result = this.currentClip.loopStart + (num - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
				if (num >= this.currentClip.loopStart)
				{
					return result;
				}
				return num;
			}
			case tk2dSpriteAnimationClip.WrapMode.Once:
				return Mathf.Min((int)this.clipTime, this.currentClip.frames.Length);
			case tk2dSpriteAnimationClip.WrapMode.PingPong:
			{
				int num2 = (int)this.clipTime % (this.currentClip.frames.Length + this.currentClip.frames.Length - 2);
				if (num2 >= this.currentClip.frames.Length)
				{
					num2 = 2 * this.currentClip.frames.Length - 2 - num2;
				}
				return num2;
			}
			case tk2dSpriteAnimationClip.WrapMode.RandomFrame:
				goto IL_FF;
			default:
				goto IL_FF;
			}
			IL_49:
			return (int)this.clipTime % this.currentClip.frames.Length;
			IL_FF:
			Debug.LogError("Unhandled clip wrap mode");
			goto IL_49;
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x000403D4 File Offset: 0x0003E5D4
	public void UpdateAnimation(float deltaTime)
	{
		tk2dSpriteAnimator.State state = this.state | tk2dSpriteAnimator.globalState;
		if (state != tk2dSpriteAnimator.State.Playing)
		{
			return;
		}
		this.clipTime += deltaTime * this.clipFps;
		int num = this.previousFrame;
		switch (this.currentClip.wrapMode)
		{
		case tk2dSpriteAnimationClip.WrapMode.Loop:
		case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
		{
			int num2 = (int)this.clipTime % this.currentClip.frames.Length;
			this.SetFrameInternal(num2);
			if (num2 < num)
			{
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.ProcessEvents(-1, num2, 1);
			}
			else
			{
				this.ProcessEvents(num, num2, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.LoopSection:
		{
			int num3 = (int)this.clipTime;
			int num4 = this.currentClip.loopStart + (num3 - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
			if (num3 >= this.currentClip.loopStart)
			{
				this.SetFrameInternal(num4);
				num3 = num4;
				if (num < this.currentClip.loopStart)
				{
					this.ProcessEvents(num, this.currentClip.loopStart - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else if (num3 < num)
				{
					this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else
				{
					this.ProcessEvents(num, num3, 1);
				}
			}
			else
			{
				this.SetFrameInternal(num3);
				this.ProcessEvents(num, num3, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.Once:
		{
			int num5 = (int)this.clipTime;
			if (num5 >= this.currentClip.frames.Length)
			{
				this.SetFrameInternal(this.currentClip.frames.Length - 1);
				this.state &= (tk2dSpriteAnimator.State)(-2);
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.OnAnimationCompleted();
			}
			else
			{
				this.SetFrameInternal(num5);
				this.ProcessEvents(num, num5, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.PingPong:
		{
			int num6 = (int)this.clipTime % (this.currentClip.frames.Length + this.currentClip.frames.Length - 2);
			int direction = 1;
			if (num6 >= this.currentClip.frames.Length)
			{
				num6 = 2 * this.currentClip.frames.Length - 2 - num6;
				direction = -1;
			}
			if (num6 < num)
			{
				direction = -1;
			}
			this.SetFrameInternal(num6);
			this.ProcessEvents(num, num6, direction);
			break;
		}
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000092B1 File Offset: 0x000074B1
	private void ClipNameError(string name)
	{
		Debug.LogError("Unable to find clip named '" + name + "' in library");
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x000092C8 File Offset: 0x000074C8
	private void ClipIdError(int id)
	{
		Debug.LogError("Play - Invalid clip id '" + id.ToString() + "' in library");
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00040684 File Offset: 0x0003E884
	private void WarpClipToLocalTime(tk2dSpriteAnimationClip clip, float time)
	{
		this.clipTime = time;
		int num = (int)this.clipTime % clip.frames.Length;
		tk2dSpriteAnimationFrame tk2dSpriteAnimationFrame = clip.frames[num];
		this.SetSprite(tk2dSpriteAnimationFrame.spriteCollection, tk2dSpriteAnimationFrame.spriteId);
		if (tk2dSpriteAnimationFrame.triggerEvent && this.AnimationEventTriggered != null)
		{
			this.AnimationEventTriggered(this, clip, num);
		}
		this.previousFrame = num;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x000092E5 File Offset: 0x000074E5
	private void SetFrameInternal(int currFrame)
	{
		if (this.previousFrame != currFrame)
		{
			this.SetSprite(this.currentClip.frames[currFrame].spriteCollection, this.currentClip.frames[currFrame].spriteId);
			this.previousFrame = currFrame;
		}
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x000406F0 File Offset: 0x0003E8F0
	private void ProcessEvents(int start, int last, int direction)
	{
		if (this.AnimationEventTriggered == null || start == last)
		{
			return;
		}
		int num = last + direction;
		tk2dSpriteAnimationFrame[] frames = this.currentClip.frames;
		for (int num2 = start + direction; num2 != num; num2 += direction)
		{
			if (frames[num2].triggerEvent && this.AnimationEventTriggered != null)
			{
				this.AnimationEventTriggered(this, this.currentClip, num2);
			}
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00009324 File Offset: 0x00007524
	private void OnAnimationCompleted()
	{
		this.previousFrame = -1;
		if (this.AnimationCompleted != null)
		{
			this.AnimationCompleted(this, this.currentClip);
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0000934A File Offset: 0x0000754A
	public virtual void LateUpdate()
	{
		this.UpdateAnimation(Time.deltaTime);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x00009357 File Offset: 0x00007557
	public virtual void SetSprite(tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		this.Sprite.SetSprite(spriteCollection, spriteId);
	}

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private tk2dSpriteAnimation library;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private int defaultClipId;

	// Token: 0x04000A17 RID: 2583
	public bool playAutomatically;

	// Token: 0x04000A18 RID: 2584
	private static tk2dSpriteAnimator.State globalState;

	// Token: 0x04000A19 RID: 2585
	private tk2dSpriteAnimationClip currentClip;

	// Token: 0x04000A1A RID: 2586
	private float clipTime;

	// Token: 0x04000A1B RID: 2587
	private float clipFps = -1f;

	// Token: 0x04000A1C RID: 2588
	private int previousFrame = -1;

	// Token: 0x04000A1D RID: 2589
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted;

	// Token: 0x04000A1E RID: 2590
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered;

	// Token: 0x04000A1F RID: 2591
	private tk2dSpriteAnimator.State state;

	// Token: 0x04000A20 RID: 2592
	protected tk2dBaseSprite _sprite;

	// Token: 0x02000171 RID: 369
	private enum State
	{
		// Token: 0x04000A22 RID: 2594
		Init,
		// Token: 0x04000A23 RID: 2595
		Playing,
		// Token: 0x04000A24 RID: 2596
		Paused
	}
}
