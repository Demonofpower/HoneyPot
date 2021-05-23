using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200019D RID: 413
	[Serializable]
	public class ColorChunk
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x00009F35 File Offset: 0x00008135
		public ColorChunk()
		{
			this.colors = new Color32[0];
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00009F49 File Offset: 0x00008149
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00009F51 File Offset: 0x00008151
		public bool Dirty { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00009F5A File Offset: 0x0000815A
		public bool Empty
		{
			get
			{
				return this.colors.Length == 0;
			}
		}

		// Token: 0x04000B99 RID: 2969
		public Color32[] colors;
	}
}
