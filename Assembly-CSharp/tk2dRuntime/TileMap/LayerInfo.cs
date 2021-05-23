using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x020001A3 RID: 419
	[Serializable]
	public class LayerInfo
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x0000A0EB File Offset: 0x000082EB
		public LayerInfo()
		{
			this.unityLayer = 0;
			this.useColor = true;
			this.generateCollider = true;
			this.skipMeshGeneration = false;
		}

		// Token: 0x04000BAE RID: 2990
		public string name;

		// Token: 0x04000BAF RID: 2991
		public int hash;

		// Token: 0x04000BB0 RID: 2992
		public bool useColor;

		// Token: 0x04000BB1 RID: 2993
		public bool generateCollider;

		// Token: 0x04000BB2 RID: 2994
		public float z = 0.1f;

		// Token: 0x04000BB3 RID: 2995
		public int unityLayer;

		// Token: 0x04000BB4 RID: 2996
		public bool skipMeshGeneration;

		// Token: 0x04000BB5 RID: 2997
		public PhysicMaterial physicMaterial;
	}
}
