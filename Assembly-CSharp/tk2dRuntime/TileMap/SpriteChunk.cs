using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200019B RID: 411
	[Serializable]
	public class SpriteChunk
	{
		// Token: 0x06000A2A RID: 2602 RVA: 0x00009EEF File Offset: 0x000080EF
		public SpriteChunk()
		{
			this.spriteIds = new int[0];
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x00009F03 File Offset: 0x00008103
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x00009F0B File Offset: 0x0000810B
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00009F14 File Offset: 0x00008114
		public bool IsEmpty
		{
			get
			{
				return this.spriteIds.Length == 0;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00047070 File Offset: 0x00045270
		public bool HasGameData
		{
			get
			{
				return this.gameObject != null || this.mesh != null || this.meshCollider != null || this.colliderMesh != null;
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000470C0 File Offset: 0x000452C0
		public void DestroyGameData(tk2dTileMap tileMap)
		{
			if (this.mesh != null)
			{
				tileMap.DestroyMesh(this.mesh);
			}
			if (this.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(this.gameObject);
			}
			this.gameObject = null;
			this.mesh = null;
			this.DestroyColliderData(tileMap);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0004711C File Offset: 0x0004531C
		public void DestroyColliderData(tk2dTileMap tileMap)
		{
			if (this.colliderMesh != null)
			{
				tileMap.DestroyMesh(this.colliderMesh);
			}
			if (this.meshCollider != null && this.meshCollider.sharedMesh != null && this.meshCollider.sharedMesh != this.colliderMesh)
			{
				tileMap.DestroyMesh(this.meshCollider.sharedMesh);
			}
			if (this.meshCollider != null)
			{
				UnityEngine.Object.DestroyImmediate(this.meshCollider);
			}
			this.meshCollider = null;
			this.colliderMesh = null;
		}

		// Token: 0x04000B92 RID: 2962
		private bool dirty;

		// Token: 0x04000B93 RID: 2963
		public int[] spriteIds;

		// Token: 0x04000B94 RID: 2964
		public GameObject gameObject;

		// Token: 0x04000B95 RID: 2965
		public Mesh mesh;

		// Token: 0x04000B96 RID: 2966
		public MeshCollider meshCollider;

		// Token: 0x04000B97 RID: 2967
		public Mesh colliderMesh;
	}
}
