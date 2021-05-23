using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
[Serializable]
public class tk2dSpriteAnimationClip
{
	// Token: 0x06000915 RID: 2325 RVA: 0x00008E32 File Offset: 0x00007032
	public tk2dSpriteAnimationClip()
	{
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00008E50 File Offset: 0x00007050
	public tk2dSpriteAnimationClip(tk2dSpriteAnimationClip source)
	{
		this.CopyFrom(source);
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0003FD50 File Offset: 0x0003DF50
	public void CopyFrom(tk2dSpriteAnimationClip source)
	{
		this.name = source.name;
		if (source.frames == null)
		{
			this.frames = null;
		}
		else
		{
			this.frames = new tk2dSpriteAnimationFrame[source.frames.Length];
			for (int i = 0; i < this.frames.Length; i++)
			{
				if (source.frames[i] == null)
				{
					this.frames[i] = null;
				}
				else
				{
					this.frames[i] = new tk2dSpriteAnimationFrame();
					this.frames[i].CopyFrom(source.frames[i]);
				}
			}
		}
		this.fps = source.fps;
		this.loopStart = source.loopStart;
		this.wrapMode = source.wrapMode;
		if (this.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single && this.frames.Length > 1)
		{
			this.frames = new tk2dSpriteAnimationFrame[]
			{
				this.frames[0]
			};
			Debug.LogError(string.Format("Clip: '{0}' Fixed up frames for WrapMode.Single", this.name));
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00008E75 File Offset: 0x00007075
	public void Clear()
	{
		this.name = string.Empty;
		this.frames = new tk2dSpriteAnimationFrame[0];
		this.fps = 30f;
		this.loopStart = 0;
		this.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x00008EA7 File Offset: 0x000070A7
	public bool Empty
	{
		get
		{
			return this.name.Length == 0 || this.frames == null || this.frames.Length == 0;
		}
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00008ED2 File Offset: 0x000070D2
	public tk2dSpriteAnimationFrame GetFrame(int frame)
	{
		return this.frames[frame];
	}

	// Token: 0x04000A07 RID: 2567
	public string name = "Default";

	// Token: 0x04000A08 RID: 2568
	public tk2dSpriteAnimationFrame[] frames;

	// Token: 0x04000A09 RID: 2569
	public float fps = 30f;

	// Token: 0x04000A0A RID: 2570
	public int loopStart;

	// Token: 0x04000A0B RID: 2571
	public tk2dSpriteAnimationClip.WrapMode wrapMode;

	// Token: 0x0200016E RID: 366
	public enum WrapMode
	{
		// Token: 0x04000A0D RID: 2573
		Loop,
		// Token: 0x04000A0E RID: 2574
		LoopSection,
		// Token: 0x04000A0F RID: 2575
		Once,
		// Token: 0x04000A10 RID: 2576
		PingPong,
		// Token: 0x04000A11 RID: 2577
		RandomFrame,
		// Token: 0x04000A12 RID: 2578
		RandomLoop,
		// Token: 0x04000A13 RID: 2579
		Single
	}
}
