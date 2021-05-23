using System;
using Holoville.HOTween;

// Token: 0x02000145 RID: 325
public class TweenUtils
{
	// Token: 0x060007A0 RID: 1952 RVA: 0x00007B66 File Offset: 0x00005D66
	public static void KillTweener(Tweener tweener, bool complete = false)
	{
		if (tweener != null)
		{
			if (complete)
			{
				tweener.Complete();
			}
			tweener.Kill();
		}
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00007B80 File Offset: 0x00005D80
	public static void KillSequence(Sequence sequence, bool complete = false)
	{
		if (sequence != null)
		{
			if (complete)
			{
				sequence.Complete();
			}
			sequence.Kill();
			sequence.Clear(null);
		}
	}
}
