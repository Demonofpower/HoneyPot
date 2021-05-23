using System;
using UnityEngine;

// Token: 0x0200016F RID: 367
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteAnimation")]
public class tk2dSpriteAnimation : MonoBehaviour
{
	// Token: 0x0600091C RID: 2332 RVA: 0x0003FE54 File Offset: 0x0003E054
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return this.clips[i];
			}
		}
		return null;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00008EDC File Offset: 0x000070DC
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (id < 0 || id >= this.clips.Length || this.clips[id].Empty)
		{
			return null;
		}
		return this.clips[id];
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0003FE9C File Offset: 0x0003E09C
	public int GetClipIdByName(string name)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0003FEE0 File Offset: 0x0003E0E0
	public int GetClipIdByName(tk2dSpriteAnimationClip clip)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i] == clip)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000920 RID: 2336 RVA: 0x0003FF18 File Offset: 0x0003E118
	public tk2dSpriteAnimationClip FirstValidClip
	{
		get
		{
			for (int i = 0; i < this.clips.Length; i++)
			{
				if (!this.clips[i].Empty && this.clips[i].frames[0].spriteCollection != null && this.clips[i].frames[0].spriteId != -1)
				{
					return this.clips[i];
				}
			}
			return null;
		}
	}

	// Token: 0x04000A14 RID: 2580
	public tk2dSpriteAnimationClip[] clips;
}
