using System;
using System.Collections.Generic;
using tk2dRuntime;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000196 RID: 406
[AddComponentMenu("2D Toolkit/TileMap/TileMap")]
[ExecuteInEditMode]
public class tk2dTileMap : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00009C31 File Offset: 0x00007E31
	// (set) Token: 0x060009F9 RID: 2553 RVA: 0x00009C39 File Offset: 0x00007E39
	public tk2dSpriteCollectionData Editor__SpriteCollection
	{
		get
		{
			return this.spriteCollection;
		}
		set
		{
			this._spriteCollectionInst = null;
			this.spriteCollection = value;
			if (this.spriteCollection != null)
			{
				this._spriteCollectionInst = this.spriteCollection.inst;
			}
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x00009C6B File Offset: 0x00007E6B
	public tk2dSpriteCollectionData SpriteCollectionInst
	{
		get
		{
			if (this._spriteCollectionInst == null && this.spriteCollection != null)
			{
				this._spriteCollectionInst = this.spriteCollection.inst;
			}
			return this._spriteCollectionInst;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x00009CA6 File Offset: 0x00007EA6
	public bool AllowEdit
	{
		get
		{
			return this._inEditMode;
		}
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00045870 File Offset: 0x00043A70
	private void Awake()
	{
		if (this.spriteCollection != null)
		{
			this._spriteCollectionInst = this.spriteCollection.inst;
		}
		bool flag = true;
		if (this.SpriteCollectionInst && this.SpriteCollectionInst.buildKey != this.spriteCollectionKey)
		{
			flag = false;
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			if ((Application.isPlaying && this._inEditMode) || !flag)
			{
				this.EndEditMode();
			}
		}
		else if (this._inEditMode)
		{
			Debug.LogError("Tilemap " + base.name + " is still in edit mode. Please fix.Building overhead will be significant.");
			this.EndEditMode();
		}
		else if (!flag)
		{
			this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00009CAE File Offset: 0x00007EAE
	public void Build()
	{
		this.Build(tk2dTileMap.BuildFlags.Default);
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00009CB7 File Offset: 0x00007EB7
	public void ForceBuild()
	{
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00045944 File Offset: 0x00043B44
	private void ClearSpawnedInstances()
	{
		if (this.layers == null)
		{
			return;
		}
		for (int i = 0; i < this.layers.Length; i++)
		{
			Layer layer = this.layers[i];
			for (int j = 0; j < layer.spriteChannel.chunks.Length; j++)
			{
				SpriteChunk spriteChunk = layer.spriteChannel.chunks[j];
				if (!(spriteChunk.gameObject == null))
				{
					Transform transform = spriteChunk.gameObject.transform;
					List<Transform> list = new List<Transform>();
					for (int k = 0; k < transform.childCount; k++)
					{
						list.Add(transform.GetChild(k));
					}
					for (int l = 0; l < list.Count; l++)
					{
						UnityEngine.Object.DestroyImmediate(list[l].gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00009CC0 File Offset: 0x00007EC0
	private void SetPrefabsRootActive(bool active)
	{
		if (this.prefabsRoot != null)
		{
			this.prefabsRoot.SetActive(active);
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00045A30 File Offset: 0x00043C30
	public void Build(tk2dTileMap.BuildFlags buildFlags)
	{
		if (this.spriteCollection != null)
		{
			this._spriteCollectionInst = this.spriteCollection.inst;
		}
		if (this.data != null && this.spriteCollection != null)
		{
			if (this.data.tilePrefabs == null)
			{
				this.data.tilePrefabs = new UnityEngine.Object[this.SpriteCollectionInst.Count];
			}
			else if (this.data.tilePrefabs.Length != this.SpriteCollectionInst.Count)
			{
				Array.Resize<UnityEngine.Object>(ref this.data.tilePrefabs, this.SpriteCollectionInst.Count);
			}
			BuilderUtil.InitDataStore(this);
			if (this.SpriteCollectionInst)
			{
				this.SpriteCollectionInst.InitMaterialIds();
			}
			bool flag = (buildFlags & tk2dTileMap.BuildFlags.ForceBuild) != tk2dTileMap.BuildFlags.Default;
			if (this.SpriteCollectionInst && this.SpriteCollectionInst.buildKey != this.spriteCollectionKey)
			{
				flag = true;
			}
			if (flag)
			{
				this.ClearSpawnedInstances();
			}
			BuilderUtil.CreateRenderData(this, this._inEditMode);
			RenderMeshBuilder.Build(this, this._inEditMode, flag);
			if (!this._inEditMode)
			{
				ColliderBuilder.Build(this, flag);
				BuilderUtil.SpawnPrefabs(this, flag);
			}
			foreach (Layer layer in this.layers)
			{
				layer.ClearDirtyFlag();
			}
			if (this.colorChannel != null)
			{
				this.colorChannel.ClearDirtyFlag();
			}
			if (this.SpriteCollectionInst)
			{
				this.spriteCollectionKey = this.SpriteCollectionInst.buildKey;
			}
			return;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00045BE0 File Offset: 0x00043DE0
	public bool GetTileAtPosition(Vector3 position, out int x, out int y)
	{
		float num;
		float num2;
		bool tileFracAtPosition = this.GetTileFracAtPosition(position, out num, out num2);
		x = (int)num;
		y = (int)num2;
		return tileFracAtPosition;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00045C04 File Offset: 0x00043E04
	public bool GetTileFracAtPosition(Vector3 position, out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType != tk2dTileMapData.TileType.Rectangular)
		{
			if (tileType == tk2dTileMapData.TileType.Isometric)
			{
				if (this.data.tileSize.x != 0f)
				{
					float num = Mathf.Atan2(this.data.tileSize.y, this.data.tileSize.x / 2f);
					Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
					x = (vector.x - this.data.tileOrigin.x) / this.data.tileSize.x;
					y = (vector.y - this.data.tileOrigin.y) / this.data.tileSize.y;
					float num2 = y * 0.5f;
					int num3 = (int)num2;
					float num4 = num2 - (float)num3;
					float num5 = x % 1f;
					x = (float)((int)x);
					y = (float)(num3 * 2);
					if (num5 > 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(1f - num4, (num5 - 0.5f) * 2f) < num)
						{
							y += 1f;
						}
						else if (num4 < 0.5f && Mathf.Atan2(num4, (num5 - 0.5f) * 2f) < num)
						{
							y -= 1f;
						}
					}
					else if (num5 < 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(num4 - 0.5f, num5 * 2f) > num)
						{
							y += 1f;
							x -= 1f;
						}
						if (num4 < 0.5f && Mathf.Atan2(num4, (0.5f - num5) * 2f) < num)
						{
							y -= 1f;
							x -= 1f;
						}
					}
					return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
				}
			}
			x = 0f;
			y = 0f;
			return false;
		}
		Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		x = (vector2.x - this.data.tileOrigin.x) / this.data.tileSize.x;
		y = (vector2.y - this.data.tileOrigin.y) / this.data.tileSize.y;
		return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00045F00 File Offset: 0x00044100
	public Vector3 GetTilePosition(int x, int y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType == tk2dTileMapData.TileType.Rectangular || tileType != tk2dTileMapData.TileType.Isometric)
		{
			Vector3 v = new Vector3((float)x * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
			return base.transform.localToWorldMatrix.MultiplyPoint(v);
		}
		Vector3 v2 = new Vector3(((float)x + (((y & 1) != 0) ? 0.5f : 0f)) * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
		return base.transform.localToWorldMatrix.MultiplyPoint(v2);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00046018 File Offset: 0x00044218
	public int GetTileIdAtPosition(Vector3 position, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		int x;
		int y;
		if (!this.GetTileAtPosition(position, out x, out y))
		{
			return -1;
		}
		return this.layers[layer].GetTile(x, y);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00009CDF File Offset: 0x00007EDF
	public TileInfo GetTileInfoForTileId(int tileId)
	{
		return this.data.GetTileInfoForSprite(tileId);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00046060 File Offset: 0x00044260
	public Color GetInterpolatedColorAtPosition(Vector3 position)
	{
		Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		int num = (int)((vector.x - this.data.tileOrigin.x) / this.data.tileSize.x);
		int num2 = (int)((vector.y - this.data.tileOrigin.y) / this.data.tileSize.y);
		if (this.colorChannel == null || this.colorChannel.IsEmpty)
		{
			return Color.white;
		}
		if (num < 0 || num >= this.width || num2 < 0 || num2 >= this.height)
		{
			return this.colorChannel.clearColor;
		}
		int num3;
		ColorChunk colorChunk = this.colorChannel.FindChunkAndCoordinate(num, num2, out num3);
		if (colorChunk.Empty)
		{
			return this.colorChannel.clearColor;
		}
		int num4 = this.partitionSizeX + 1;
		Color a = colorChunk.colors[num3];
		Color b = colorChunk.colors[num3 + 1];
		Color a2 = colorChunk.colors[num3 + num4];
		Color b2 = colorChunk.colors[num3 + num4 + 1];
		float num5 = (float)num * this.data.tileSize.x + this.data.tileOrigin.x;
		float num6 = (float)num2 * this.data.tileSize.y + this.data.tileOrigin.y;
		float t = (vector.x - num5) / this.data.tileSize.x;
		float t2 = (vector.y - num6) / this.data.tileSize.y;
		Color a3 = Color.Lerp(a, b, t);
		Color b3 = Color.Lerp(a2, b2, t);
		return Color.Lerp(a3, b3, t2);
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00009CED File Offset: 0x00007EED
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return spriteCollection == this.spriteCollection || this._spriteCollectionInst == spriteCollection;
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x00009D0F File Offset: 0x00007F0F
	public void EndEditMode()
	{
		this._inEditMode = false;
		this.SetPrefabsRootActive(true);
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		if (this.prefabsRoot != null)
		{
			UnityEngine.Object.DestroyImmediate(this.prefabsRoot);
			this.prefabsRoot = null;
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0000306D File Offset: 0x0000126D
	public void TouchMesh(Mesh mesh)
	{
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00009D49 File Offset: 0x00007F49
	public void DestroyMesh(Mesh mesh)
	{
		UnityEngine.Object.DestroyImmediate(mesh);
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00009D51 File Offset: 0x00007F51
	public int GetTilePrefabsListCount()
	{
		return this.tilePrefabsList.Count;
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00009D5E File Offset: 0x00007F5E
	public List<tk2dTileMap.TilemapPrefabInstance> TilePrefabsList
	{
		get
		{
			return this.tilePrefabsList;
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00046278 File Offset: 0x00044478
	public void GetTilePrefabsListItem(int index, out int x, out int y, out int layer, out GameObject instance)
	{
		tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = this.tilePrefabsList[index];
		x = tilemapPrefabInstance.x;
		y = tilemapPrefabInstance.y;
		layer = tilemapPrefabInstance.layer;
		instance = tilemapPrefabInstance.instance;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x000462B4 File Offset: 0x000444B4
	public void SetTilePrefabsList(List<int> xs, List<int> ys, List<int> layers, List<GameObject> instances)
	{
		int count = instances.Count;
		this.tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>(count);
		for (int i = 0; i < count; i++)
		{
			tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = new tk2dTileMap.TilemapPrefabInstance();
			tilemapPrefabInstance.x = xs[i];
			tilemapPrefabInstance.y = ys[i];
			tilemapPrefabInstance.layer = layers[i];
			tilemapPrefabInstance.instance = instances[i];
			this.tilePrefabsList.Add(tilemapPrefabInstance);
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00009D66 File Offset: 0x00007F66
	// (set) Token: 0x06000A11 RID: 2577 RVA: 0x00009D6E File Offset: 0x00007F6E
	public Layer[] Layers
	{
		get
		{
			return this.layers;
		}
		set
		{
			this.layers = value;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00009D77 File Offset: 0x00007F77
	// (set) Token: 0x06000A13 RID: 2579 RVA: 0x00009D7F File Offset: 0x00007F7F
	public ColorChannel ColorChannel
	{
		get
		{
			return this.colorChannel;
		}
		set
		{
			this.colorChannel = value;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00009D88 File Offset: 0x00007F88
	// (set) Token: 0x06000A15 RID: 2581 RVA: 0x00009D90 File Offset: 0x00007F90
	public GameObject PrefabsRoot
	{
		get
		{
			return this.prefabsRoot;
		}
		set
		{
			this.prefabsRoot = value;
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00009D99 File Offset: 0x00007F99
	public int GetTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		return this.layers[layer].GetTile(x, y);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00009DC1 File Offset: 0x00007FC1
	public tk2dTileFlags GetTileFlags(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return tk2dTileFlags.None;
		}
		return this.layers[layer].GetTileFlags(x, y);
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00009DE9 File Offset: 0x00007FE9
	public void SetTile(int x, int y, int layer, int tile)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTile(x, y, tile);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00009E12 File Offset: 0x00008012
	public void SetTileFlags(int x, int y, int layer, tk2dTileFlags flags)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTileFlags(x, y, flags);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00009E3B File Offset: 0x0000803B
	public void ClearTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].ClearTile(x, y);
	}

	// Token: 0x04000B74 RID: 2932
	public string editorDataGUID = string.Empty;

	// Token: 0x04000B75 RID: 2933
	public tk2dTileMapData data;

	// Token: 0x04000B76 RID: 2934
	public GameObject renderData;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000B78 RID: 2936
	private tk2dSpriteCollectionData _spriteCollectionInst;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private int spriteCollectionKey;

	// Token: 0x04000B7A RID: 2938
	public int width = 128;

	// Token: 0x04000B7B RID: 2939
	public int height = 128;

	// Token: 0x04000B7C RID: 2940
	public int partitionSizeX = 32;

	// Token: 0x04000B7D RID: 2941
	public int partitionSizeY = 32;

	// Token: 0x04000B7E RID: 2942
	[SerializeField]
	private Layer[] layers;

	// Token: 0x04000B7F RID: 2943
	[SerializeField]
	private ColorChannel colorChannel;

	// Token: 0x04000B80 RID: 2944
	[SerializeField]
	private GameObject prefabsRoot;

	// Token: 0x04000B81 RID: 2945
	[SerializeField]
	private List<tk2dTileMap.TilemapPrefabInstance> tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>();

	// Token: 0x04000B82 RID: 2946
	[SerializeField]
	private bool _inEditMode;

	// Token: 0x04000B83 RID: 2947
	public string serializedMeshPath;

	// Token: 0x02000197 RID: 407
	[Serializable]
	public class TilemapPrefabInstance
	{
		// Token: 0x04000B84 RID: 2948
		public int x;

		// Token: 0x04000B85 RID: 2949
		public int y;

		// Token: 0x04000B86 RID: 2950
		public int layer;

		// Token: 0x04000B87 RID: 2951
		public GameObject instance;
	}

	// Token: 0x02000198 RID: 408
	[Flags]
	public enum BuildFlags
	{
		// Token: 0x04000B89 RID: 2953
		Default = 0,
		// Token: 0x04000B8A RID: 2954
		EditMode = 1,
		// Token: 0x04000B8B RID: 2955
		ForceBuild = 2
	}
}
