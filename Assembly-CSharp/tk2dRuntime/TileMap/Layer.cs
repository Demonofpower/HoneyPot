using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200019F RID: 415
	[Serializable]
	public class Layer
	{
		// Token: 0x06000A48 RID: 2632 RVA: 0x0000A025 File Offset: 0x00008225
		public Layer(int hash, int width, int height, int divX, int divY)
		{
			this.spriteChannel = new SpriteChannel();
			this.Init(hash, width, height, divX, divY);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00047768 File Offset: 0x00045968
		public void Init(int hash, int width, int height, int divX, int divY)
		{
			this.divX = divX;
			this.divY = divY;
			this.hash = hash;
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.width = width;
			this.height = height;
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.numColumns * this.numRows; i++)
			{
				this.spriteChannel.chunks[i] = new SpriteChunk();
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0000A045 File Offset: 0x00008245
		public bool IsEmpty
		{
			get
			{
				return this.spriteChannel.chunks.Length == 0;
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000A057 File Offset: 0x00008257
		public void Create()
		{
			this.spriteChannel.chunks = new SpriteChunk[this.numColumns * this.numRows];
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000A076 File Offset: 0x00008276
		public int[] GetChunkData(int x, int y)
		{
			return this.GetChunk(x, y).spriteIds;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0000A085 File Offset: 0x00008285
		public SpriteChunk GetChunk(int x, int y)
		{
			return this.spriteChannel.chunks[y * this.numColumns + x];
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00047808 File Offset: 0x00045A08
		private SpriteChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			SpriteChunk result = this.spriteChannel.chunks[num2 * this.numColumns + num];
			int num3 = x - num * this.divX;
			int num4 = y - num2 * this.divY;
			offset = num4 * this.divX + num3;
			return result;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00047864 File Offset: 0x00045A64
		private bool GetRawTileValue(int x, int y, ref int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk.spriteIds == null || spriteChunk.spriteIds.Length == 0)
			{
				return false;
			}
			value = spriteChunk.spriteIds[num];
			return true;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000478A4 File Offset: 0x00045AA4
		private void SetRawTileValue(int x, int y, int value)
		{
			int num;
			SpriteChunk spriteChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (spriteChunk != null)
			{
				this.CreateChunk(spriteChunk);
				spriteChunk.spriteIds[num] = value;
				spriteChunk.Dirty = true;
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000478DC File Offset: 0x00045ADC
		public int GetTile(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return num & 16777215;
			}
			return -1;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0004790C File Offset: 0x00045B0C
		public tk2dTileFlags GetTileFlags(int x, int y)
		{
			int num = 0;
			if (this.GetRawTileValue(x, y, ref num) && num != -1)
			{
				return (tk2dTileFlags)(num & -16777216);
			}
			return tk2dTileFlags.None;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0004793C File Offset: 0x00045B3C
		public int GetRawTile(int x, int y)
		{
			int result = 0;
			if (this.GetRawTileValue(x, y, ref result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00047960 File Offset: 0x00045B60
		public void SetTile(int x, int y, int tile)
		{
			tk2dTileFlags tileFlags = this.GetTileFlags(x, y);
			int value = (tile != -1) ? (tile | (int)tileFlags) : -1;
			this.SetRawTileValue(x, y, value);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00047990 File Offset: 0x00045B90
		public void SetTileFlags(int x, int y, tk2dTileFlags flags)
		{
			int tile = this.GetTile(x, y);
			if (tile != -1)
			{
				int value = tile | (int)flags;
				this.SetRawTileValue(x, y, value);
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0000A09D File Offset: 0x0000829D
		public void ClearTile(int x, int y)
		{
			this.SetTile(x, y, -1);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0000A0A8 File Offset: 0x000082A8
		public void SetRawTile(int x, int y, int rawTile)
		{
			this.SetRawTileValue(x, y, rawTile);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000479BC File Offset: 0x00045BBC
		private void CreateChunk(SpriteChunk chunk)
		{
			if (chunk.spriteIds == null || chunk.spriteIds.Length == 0)
			{
				chunk.spriteIds = new int[this.divX * this.divY];
				for (int i = 0; i < this.divX * this.divY; i++)
				{
					chunk.spriteIds[i] = -1;
				}
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00047A20 File Offset: 0x00045C20
		private void Optimize(SpriteChunk chunk)
		{
			bool flag = true;
			foreach (int num in chunk.spriteIds)
			{
				if (num != -1)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				chunk.spriteIds = new int[0];
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00047A70 File Offset: 0x00045C70
		public void Optimize()
		{
			foreach (SpriteChunk chunk in this.spriteChannel.chunks)
			{
				this.Optimize(chunk);
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00047AA8 File Offset: 0x00045CA8
		public void OptimizeIncremental()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				if (spriteChunk.Dirty)
				{
					this.Optimize(spriteChunk);
				}
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00047AEC File Offset: 0x00045CEC
		public void ClearDirtyFlag()
		{
			foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
			{
				spriteChunk.Dirty = false;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00047B24 File Offset: 0x00045D24
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (SpriteChunk spriteChunk in this.spriteChannel.chunks)
				{
					if (!spriteChunk.IsEmpty)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x04000BA1 RID: 2977
		private const int tileMask = 16777215;

		// Token: 0x04000BA2 RID: 2978
		private const int flagMask = -16777216;

		// Token: 0x04000BA3 RID: 2979
		public int hash;

		// Token: 0x04000BA4 RID: 2980
		public SpriteChannel spriteChannel;

		// Token: 0x04000BA5 RID: 2981
		public int width;

		// Token: 0x04000BA6 RID: 2982
		public int height;

		// Token: 0x04000BA7 RID: 2983
		public int numColumns;

		// Token: 0x04000BA8 RID: 2984
		public int numRows;

		// Token: 0x04000BA9 RID: 2985
		public int divX;

		// Token: 0x04000BAA RID: 2986
		public int divY;

		// Token: 0x04000BAB RID: 2987
		public GameObject gameObject;
	}
}
