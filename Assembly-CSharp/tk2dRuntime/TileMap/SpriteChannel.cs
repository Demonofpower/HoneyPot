using System;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200019C RID: 412
	[Serializable]
	public class SpriteChannel
	{
		// Token: 0x06000A31 RID: 2609 RVA: 0x00009F21 File Offset: 0x00008121
		public SpriteChannel()
		{
			this.chunks = new SpriteChunk[0];
		}

		// Token: 0x04000B98 RID: 2968
		public SpriteChunk[] chunks;
	}
}
