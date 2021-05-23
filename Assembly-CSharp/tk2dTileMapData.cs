using System;
using System.Collections.Generic;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class tk2dTileMapData : ScriptableObject
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0000A158 File Offset: 0x00008358
	public int NumLayers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.Count;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0000A186 File Offset: 0x00008386
	public LayerInfo[] Layers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.ToArray();
		}
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0000A1B4 File Offset: 0x000083B4
	public TileInfo GetTileInfoForSprite(int tileId)
	{
		if (this.tileInfo == null || tileId < 0 || tileId >= this.tileInfo.Length)
		{
			return null;
		}
		return this.tileInfo[tileId];
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x000484D8 File Offset: 0x000466D8
	public TileInfo[] GetOrCreateTileInfo(int numTiles)
	{
		bool flag = false;
		if (this.tileInfo == null)
		{
			this.tileInfo = new TileInfo[numTiles];
			flag = true;
		}
		else if (this.tileInfo.Length != numTiles)
		{
			Array.Resize<TileInfo>(ref this.tileInfo, numTiles);
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < this.tileInfo.Length; i++)
			{
				if (this.tileInfo[i] == null)
				{
					this.tileInfo[i] = new TileInfo();
				}
			}
		}
		return this.tileInfo;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00048560 File Offset: 0x00046760
	public void GetTileOffset(out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.tileType;
		if (tileType != tk2dTileMapData.TileType.Rectangular)
		{
			if (tileType == tk2dTileMapData.TileType.Isometric)
			{
				x = 0.5f;
				y = 0f;
				return;
			}
		}
		x = 0f;
		y = 0f;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x000485AC File Offset: 0x000467AC
	private void InitLayers()
	{
		this.tileMapLayers = new List<LayerInfo>();
		LayerInfo layerInfo = new LayerInfo();
		layerInfo = new LayerInfo();
		layerInfo.name = "Layer 0";
		layerInfo.hash = 1892887448;
		layerInfo.z = 0f;
		this.tileMapLayers.Add(layerInfo);
	}

	// Token: 0x04000BBA RID: 3002
	public Vector3 tileSize;

	// Token: 0x04000BBB RID: 3003
	public Vector3 tileOrigin;

	// Token: 0x04000BBC RID: 3004
	public tk2dTileMapData.TileType tileType;

	// Token: 0x04000BBD RID: 3005
	public tk2dTileMapData.SortMethod sortMethod;

	// Token: 0x04000BBE RID: 3006
	public bool layersFixedZ;

	// Token: 0x04000BBF RID: 3007
	public UnityEngine.Object[] tilePrefabs = new UnityEngine.Object[0];

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private TileInfo[] tileInfo = new TileInfo[0];

	// Token: 0x04000BC1 RID: 3009
	[SerializeField]
	public List<LayerInfo> tileMapLayers = new List<LayerInfo>();

	// Token: 0x020001A6 RID: 422
	public enum SortMethod
	{
		// Token: 0x04000BC3 RID: 3011
		BottomLeft,
		// Token: 0x04000BC4 RID: 3012
		TopLeft,
		// Token: 0x04000BC5 RID: 3013
		BottomRight,
		// Token: 0x04000BC6 RID: 3014
		TopRight
	}

	// Token: 0x020001A7 RID: 423
	public enum TileType
	{
		// Token: 0x04000BC8 RID: 3016
		Rectangular,
		// Token: 0x04000BC9 RID: 3017
		Isometric
	}
}
