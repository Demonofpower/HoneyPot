using System;
using UnityEngine;

// Token: 0x020001C1 RID: 449
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUISpriteAnimator")]
public class tk2dUISpriteAnimator : tk2dSpriteAnimator
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x0000B62A File Offset: 0x0000982A
	public override void LateUpdate()
	{
		base.UpdateAnimation(tk2dUITime.deltaTime);
	}
}
