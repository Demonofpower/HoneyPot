using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
[Serializable]
public class tk2dSpriteCollectionDefault
{
	// Token: 0x04000A77 RID: 2679
	public bool additive;

	// Token: 0x04000A78 RID: 2680
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000A79 RID: 2681
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x04000A7A RID: 2682
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04000A7B RID: 2683
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;
}
