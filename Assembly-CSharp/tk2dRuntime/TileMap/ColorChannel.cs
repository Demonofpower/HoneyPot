using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x0200019E RID: 414
	[Serializable]
	public class ColorChannel
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x00009F67 File Offset: 0x00008167
		public ColorChannel(int width, int height, int divX, int divY)
		{
			this.Init(width, height, divX, divY);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00009F85 File Offset: 0x00008185
		public ColorChannel()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00009FA4 File Offset: 0x000081A4
		public void Init(int width, int height, int divX, int divY)
		{
			this.numColumns = (width + divX - 1) / divX;
			this.numRows = (height + divY - 1) / divY;
			this.chunks = new ColorChunk[0];
			this.divX = divX;
			this.divY = divY;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000471C4 File Offset: 0x000453C4
		public ColorChunk FindChunkAndCoordinate(int x, int y, out int offset)
		{
			int num = x / this.divX;
			int num2 = y / this.divY;
			num = Mathf.Clamp(num, 0, this.numColumns - 1);
			num2 = Mathf.Clamp(num2, 0, this.numRows - 1);
			int num3 = num2 * this.numColumns + num;
			ColorChunk result = this.chunks[num3];
			int num4 = x - num * this.divX;
			int num5 = y - num2 * this.divY;
			offset = num5 * (this.divX + 1) + num4;
			return result;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00047240 File Offset: 0x00045440
		public Color GetColor(int x, int y)
		{
			if (this.IsEmpty)
			{
				return this.clearColor;
			}
			int num;
			ColorChunk colorChunk = this.FindChunkAndCoordinate(x, y, out num);
			if (colorChunk.colors.Length == 0)
			{
				return this.clearColor;
			}
			return colorChunk.colors[num];
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00047294 File Offset: 0x00045494
		private void InitChunk(ColorChunk chunk)
		{
			if (chunk.colors.Length == 0)
			{
				chunk.colors = new Color32[(this.divX + 1) * (this.divY + 1)];
				for (int i = 0; i < chunk.colors.Length; i++)
				{
					chunk.colors[i] = this.clearColor;
				}
			}
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00047300 File Offset: 0x00045500
		public void SetColor(int x, int y, Color color)
		{
			if (this.IsEmpty)
			{
				this.Create();
			}
			int num = this.divX + 1;
			int num2 = Mathf.Max(x - 1, 0) / this.divX;
			int num3 = Mathf.Max(y - 1, 0) / this.divY;
			ColorChunk chunk = this.GetChunk(num2, num3, true);
			int num4 = x - num2 * this.divX;
			int num5 = y - num3 * this.divY;
			chunk.colors[num5 * num + num4] = color;
			chunk.Dirty = true;
			bool flag = false;
			bool flag2 = false;
			if (x != 0 && x % this.divX == 0 && num2 + 1 < this.numColumns)
			{
				flag = true;
			}
			if (y != 0 && y % this.divY == 0 && num3 + 1 < this.numRows)
			{
				flag2 = true;
			}
			if (flag)
			{
				int num6 = num2 + 1;
				chunk = this.GetChunk(num6, num3, true);
				num4 = x - num6 * this.divX;
				num5 = y - num3 * this.divY;
				chunk.colors[num5 * num + num4] = color;
				chunk.Dirty = true;
			}
			if (flag2)
			{
				int num7 = num3 + 1;
				chunk = this.GetChunk(num2, num7, true);
				num4 = x - num2 * this.divX;
				num5 = y - num7 * this.divY;
				chunk.colors[num5 * num + num4] = color;
				chunk.Dirty = true;
			}
			if (flag && flag2)
			{
				int num8 = num2 + 1;
				int num9 = num3 + 1;
				chunk = this.GetChunk(num8, num9, true);
				num4 = x - num8 * this.divX;
				num5 = y - num9 * this.divY;
				chunk.colors[num5 * num + num4] = color;
				chunk.Dirty = true;
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00009FDD File Offset: 0x000081DD
		public ColorChunk GetChunk(int x, int y)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			return this.chunks[y * this.numColumns + x];
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x000474E8 File Offset: 0x000456E8
		public ColorChunk GetChunk(int x, int y, bool init)
		{
			if (this.chunks == null || this.chunks.Length == 0)
			{
				return null;
			}
			ColorChunk colorChunk = this.chunks[y * this.numColumns + x];
			this.InitChunk(colorChunk);
			return colorChunk;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0004752C File Offset: 0x0004572C
		public void ClearChunk(ColorChunk chunk)
		{
			for (int i = 0; i < chunk.colors.Length; i++)
			{
				chunk.colors[i] = this.clearColor;
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00047570 File Offset: 0x00045770
		public void ClearDirtyFlag()
		{
			foreach (ColorChunk colorChunk in this.chunks)
			{
				colorChunk.Dirty = false;
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000475A4 File Offset: 0x000457A4
		public void Clear(Color color)
		{
			this.clearColor = color;
			foreach (ColorChunk chunk in this.chunks)
			{
				this.ClearChunk(chunk);
			}
			this.Optimize();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000A00A File Offset: 0x0000820A
		public void Delete()
		{
			this.chunks = new ColorChunk[0];
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000475E4 File Offset: 0x000457E4
		public void Create()
		{
			this.chunks = new ColorChunk[this.numColumns * this.numRows];
			for (int i = 0; i < this.chunks.Length; i++)
			{
				this.chunks[i] = new ColorChunk();
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00047630 File Offset: 0x00045830
		private void Optimize(ColorChunk chunk)
		{
			bool flag = true;
			Color32 color = this.clearColor;
			foreach (Color32 color2 in chunk.colors)
			{
				if (color2.r != color.r || color2.g != color.g || color2.b != color.b || color2.a != color.a)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				chunk.colors = new Color32[0];
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000476E0 File Offset: 0x000458E0
		public void Optimize()
		{
			foreach (ColorChunk chunk in this.chunks)
			{
				this.Optimize(chunk);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0000A018 File Offset: 0x00008218
		public bool IsEmpty
		{
			get
			{
				return this.chunks.Length == 0;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00047714 File Offset: 0x00045914
		public int NumActiveChunks
		{
			get
			{
				int num = 0;
				foreach (ColorChunk colorChunk in this.chunks)
				{
					if (colorChunk != null && colorChunk.colors != null && colorChunk.colors.Length > 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x04000B9B RID: 2971
		public Color clearColor = Color.white;

		// Token: 0x04000B9C RID: 2972
		public ColorChunk[] chunks;

		// Token: 0x04000B9D RID: 2973
		public int numColumns;

		// Token: 0x04000B9E RID: 2974
		public int numRows;

		// Token: 0x04000B9F RID: 2975
		public int divX;

		// Token: 0x04000BA0 RID: 2976
		public int divY;
	}
}
