using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000199 RID: 409
	public static class BuilderUtil
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x00046330 File Offset: 0x00044530
		public static bool InitDataStore(tk2dTileMap tileMap)
		{
			bool result = false;
			int numLayers = tileMap.data.NumLayers;
			if (tileMap.Layers == null)
			{
				tileMap.Layers = new Layer[numLayers];
				for (int i = 0; i < numLayers; i++)
				{
					tileMap.Layers[i] = new Layer(tileMap.data.Layers[i].hash, tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
				}
				result = true;
			}
			else
			{
				Layer[] array = new Layer[numLayers];
				for (int j = 0; j < numLayers; j++)
				{
					LayerInfo layerInfo = tileMap.data.Layers[j];
					bool flag = false;
					for (int k = 0; k < tileMap.Layers.Length; k++)
					{
						if (tileMap.Layers[k].hash == layerInfo.hash)
						{
							array[j] = tileMap.Layers[k];
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						array[j] = new Layer(layerInfo.hash, tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
					}
				}
				int num = 0;
				foreach (Layer layer in array)
				{
					if (!layer.IsEmpty)
					{
						num++;
					}
				}
				int num2 = 0;
				foreach (Layer layer2 in tileMap.Layers)
				{
					if (!layer2.IsEmpty)
					{
						num2++;
					}
				}
				if (num != num2)
				{
					result = true;
				}
				tileMap.Layers = array;
			}
			if (tileMap.ColorChannel == null)
			{
				tileMap.ColorChannel = new ColorChannel(tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
			}
			return result;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00046514 File Offset: 0x00044714
		private static GameObject GetExistingTilePrefabInstance(tk2dTileMap tileMap, int tileX, int tileY, int tileLayer)
		{
			int tilePrefabsListCount = tileMap.GetTilePrefabsListCount();
			for (int i = 0; i < tilePrefabsListCount; i++)
			{
				int num;
				int num2;
				int num3;
				GameObject result;
				tileMap.GetTilePrefabsListItem(i, out num, out num2, out num3, out result);
				if (num == tileX && num2 == tileY && num3 == tileLayer)
				{
					return result;
				}
			}
			return null;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00046564 File Offset: 0x00044764
		public static void SpawnPrefabsForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, int layer, int[] prefabCounts)
		{
			int[] spriteIds = chunk.spriteIds;
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			Vector3 tileSize = tileMap.data.tileSize;
			Transform transform = chunk.gameObject.transform;
			float num = 0f;
			float num2 = 0f;
			tileMap.data.GetTileOffset(out num, out num2);
			for (int i = 0; i < tileMap.partitionSizeY; i++)
			{
				float num3 = (float)(baseY + i & 1) * num;
				for (int j = 0; j < tileMap.partitionSizeX; j++)
				{
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(spriteIds[i * tileMap.partitionSizeX + j]);
					if (tileFromRawTile >= 0 && tileFromRawTile < tilePrefabs.Length)
					{
						UnityEngine.Object @object = tilePrefabs[tileFromRawTile];
						if (@object != null)
						{
							prefabCounts[tileFromRawTile]++;
							GameObject gameObject = BuilderUtil.GetExistingTilePrefabInstance(tileMap, baseX + j, baseY + i, layer);
							bool flag = gameObject != null;
							if (gameObject == null)
							{
								gameObject = (UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity) as GameObject);
							}
							if (gameObject != null)
							{
								GameObject gameObject2 = @object as GameObject;
								Vector3 vector = new Vector3(tileSize.x * ((float)j + num3), tileSize.y * (float)i, 0f);
								bool flag2 = false;
								TileInfo tileInfoForSprite = tileMap.data.GetTileInfoForSprite(tileFromRawTile);
								if (tileInfoForSprite != null)
								{
									flag2 = tileInfoForSprite.enablePrefabOffset;
								}
								if (flag2 && gameObject2 != null)
								{
									vector += gameObject2.transform.position;
								}
								if (!flag)
								{
									gameObject.name = @object.name + " " + prefabCounts[tileFromRawTile].ToString();
								}
								gameObject.transform.parent = transform;
								gameObject.transform.localPosition = vector;
								BuilderUtil.TilePrefabsX.Add(baseX + j);
								BuilderUtil.TilePrefabsY.Add(baseY + i);
								BuilderUtil.TilePrefabsLayer.Add(layer);
								BuilderUtil.TilePrefabsInstance.Add(gameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0004678C File Offset: 0x0004498C
		public static void SpawnPrefabs(tk2dTileMap tileMap, bool forceBuild)
		{
			BuilderUtil.TilePrefabsX = new List<int>();
			BuilderUtil.TilePrefabsY = new List<int>();
			BuilderUtil.TilePrefabsLayer = new List<int>();
			BuilderUtil.TilePrefabsInstance = new List<GameObject>();
			int[] prefabCounts = new int[tileMap.data.tilePrefabs.Length];
			int num = tileMap.Layers.Length;
			for (int i = 0; i < num; i++)
			{
				Layer layer = tileMap.Layers[i];
				LayerInfo layerInfo = tileMap.data.Layers[i];
				if (!layer.IsEmpty && !layerInfo.skipMeshGeneration)
				{
					for (int j = 0; j < layer.numRows; j++)
					{
						int baseY = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int baseX = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							if (!chunk.IsEmpty)
							{
								if (forceBuild || chunk.Dirty)
								{
									BuilderUtil.SpawnPrefabsForChunk(tileMap, chunk, baseX, baseY, i, prefabCounts);
								}
							}
						}
					}
				}
			}
			tileMap.SetTilePrefabsList(BuilderUtil.TilePrefabsX, BuilderUtil.TilePrefabsY, BuilderUtil.TilePrefabsLayer, BuilderUtil.TilePrefabsInstance);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000468CC File Offset: 0x00044ACC
		public static void HideTileMapPrefabs(tk2dTileMap tileMap)
		{
			if (tileMap.renderData == null)
			{
				return;
			}
			if (tileMap.PrefabsRoot == null)
			{
				GameObject gameObject = new GameObject("Prefabs");
				tileMap.PrefabsRoot = gameObject;
				GameObject gameObject2 = gameObject;
				gameObject2.transform.parent = tileMap.renderData.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
			}
			if (tileMap.Layers == null)
			{
				return;
			}
			int tilePrefabsListCount = tileMap.GetTilePrefabsListCount();
			bool[] array = new bool[tilePrefabsListCount];
			for (int i = 0; i < tileMap.Layers.Length; i++)
			{
				Layer layer = tileMap.Layers[i];
				for (int j = 0; j < layer.spriteChannel.chunks.Length; j++)
				{
					SpriteChunk spriteChunk = layer.spriteChannel.chunks[j];
					if (!(spriteChunk.gameObject == null))
					{
						Transform transform = spriteChunk.gameObject.transform;
						int childCount = transform.childCount;
						for (int k = 0; k < childCount; k++)
						{
							GameObject gameObject3 = transform.GetChild(k).gameObject;
							for (int l = 0; l < tilePrefabsListCount; l++)
							{
								int num;
								int num2;
								int num3;
								GameObject x;
								tileMap.GetTilePrefabsListItem(l, out num, out num2, out num3, out x);
								if (x == gameObject3)
								{
									array[l] = true;
									break;
								}
							}
						}
					}
				}
			}
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			List<GameObject> list4 = new List<GameObject>();
			for (int m = 0; m < tilePrefabsListCount; m++)
			{
				if (array[m])
				{
					int item;
					int item2;
					int item3;
					GameObject gameObject4;
					tileMap.GetTilePrefabsListItem(m, out item, out item2, out item3, out gameObject4);
					list.Add(item);
					list2.Add(item2);
					list3.Add(item3);
					list4.Add(gameObject4);
					gameObject4.transform.parent = tileMap.PrefabsRoot.transform;
				}
			}
			tileMap.SetTilePrefabsList(list, list2, list3, list4);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00009E62 File Offset: 0x00008062
		private static Vector3 GetTilePosition(tk2dTileMap tileMap, int x, int y)
		{
			return new Vector3(tileMap.data.tileSize.x * (float)x, tileMap.data.tileSize.y * (float)y, 0f);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00046AF4 File Offset: 0x00044CF4
		public static void CreateRenderData(tk2dTileMap tileMap, bool editMode)
		{
			if (tileMap.renderData == null)
			{
				tileMap.renderData = new GameObject(tileMap.name + " Render Data");
			}
			tileMap.renderData.transform.position = tileMap.transform.position;
			float num = 0f;
			int num2 = 0;
			foreach (Layer layer in tileMap.Layers)
			{
				float z = tileMap.data.Layers[num2].z;
				if (num2 != 0)
				{
					num -= z;
				}
				if (layer.IsEmpty && layer.gameObject != null)
				{
					UnityEngine.Object.DestroyImmediate(layer.gameObject);
					layer.gameObject = null;
				}
				else if (!layer.IsEmpty && layer.gameObject == null)
				{
					GameObject gameObject = layer.gameObject = new GameObject(string.Empty);
					gameObject.transform.parent = tileMap.renderData.transform;
				}
				int unityLayer = tileMap.data.Layers[num2].unityLayer;
				if (layer.gameObject != null)
				{
					if (!editMode && !layer.gameObject.activeSelf)
					{
						layer.gameObject.SetActive(true);
					}
					layer.gameObject.name = tileMap.data.Layers[num2].name;
					layer.gameObject.transform.localPosition = new Vector3(0f, 0f, (!tileMap.data.layersFixedZ) ? num : (-z));
					layer.gameObject.transform.localRotation = Quaternion.identity;
					layer.gameObject.transform.localScale = Vector3.one;
					layer.gameObject.layer = unityLayer;
				}
				int num3;
				int num4;
				int num5;
				int num6;
				int num7;
				int num8;
				BuilderUtil.GetLoopOrder(tileMap.data.sortMethod, layer.numColumns, layer.numRows, out num3, out num4, out num5, out num6, out num7, out num8);
				float num9 = 0f;
				for (int num10 = num6; num10 != num7; num10 += num8)
				{
					for (int num11 = num3; num11 != num4; num11 += num5)
					{
						SpriteChunk chunk = layer.GetChunk(num11, num10);
						bool flag = layer.IsEmpty || chunk.IsEmpty;
						if (editMode)
						{
							flag = false;
						}
						if (flag && chunk.HasGameData)
						{
							chunk.DestroyGameData(tileMap);
						}
						else if (!flag && chunk.gameObject == null)
						{
							string name = "Chunk " + num10.ToString() + " " + num11.ToString();
							GameObject gameObject2 = chunk.gameObject = new GameObject(name);
							gameObject2.transform.parent = layer.gameObject.transform;
							MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
							gameObject2.AddComponent<MeshRenderer>();
							chunk.mesh = new Mesh();
							meshFilter.mesh = chunk.mesh;
							chunk.meshCollider = gameObject2.AddComponent<MeshCollider>();
							chunk.meshCollider.sharedMesh = null;
							chunk.colliderMesh = null;
						}
						if (chunk.gameObject != null)
						{
							Vector3 tilePosition = BuilderUtil.GetTilePosition(tileMap, num11 * tileMap.partitionSizeX, num10 * tileMap.partitionSizeY);
							tilePosition.z += num9;
							chunk.gameObject.transform.localPosition = tilePosition;
							chunk.gameObject.transform.localRotation = Quaternion.identity;
							chunk.gameObject.transform.localScale = Vector3.one;
							chunk.gameObject.layer = unityLayer;
							if (editMode && chunk.colliderMesh)
							{
								chunk.DestroyColliderData(tileMap);
							}
						}
						num9 -= 1E-06f;
					}
				}
				num2++;
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00046EF8 File Offset: 0x000450F8
		public static void GetLoopOrder(tk2dTileMapData.SortMethod sortMethod, int w, int h, out int x0, out int x1, out int dx, out int y0, out int y1, out int dy)
		{
			switch (sortMethod)
			{
			case tk2dTileMapData.SortMethod.BottomLeft:
				break;
			case tk2dTileMapData.SortMethod.TopLeft:
				x0 = 0;
				x1 = w;
				dx = 1;
				y0 = h - 1;
				y1 = -1;
				dy = -1;
				return;
			case tk2dTileMapData.SortMethod.BottomRight:
				x0 = w - 1;
				x1 = -1;
				dx = -1;
				y0 = 0;
				y1 = h;
				dy = 1;
				return;
			case tk2dTileMapData.SortMethod.TopRight:
				x0 = w - 1;
				x1 = -1;
				dx = -1;
				y0 = h - 1;
				y1 = -1;
				dy = -1;
				return;
			default:
				Debug.LogError("Unhandled sort method");
				break;
			}
			x0 = 0;
			x1 = w;
			dx = 1;
			y0 = 0;
			y1 = h;
			dy = 1;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00009E94 File Offset: 0x00008094
		public static int GetTileFromRawTile(int rawTile)
		{
			if (rawTile == -1)
			{
				return -1;
			}
			return rawTile & 16777215;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00009EA6 File Offset: 0x000080A6
		public static bool IsRawTileFlagSet(int rawTile, tk2dTileFlags flag)
		{
			return rawTile != -1 && (rawTile & (int)flag) != 0;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00009EBA File Offset: 0x000080BA
		public static void SetRawTileFlag(ref int rawTile, tk2dTileFlags flag, bool setValue)
		{
			if (rawTile == -1)
			{
				return;
			}
			rawTile = ((!setValue) ? (rawTile & (int)(~(int)flag)) : (rawTile | (int)flag));
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00046FAC File Offset: 0x000451AC
		public static void InvertRawTileFlag(ref int rawTile, tk2dTileFlags flag)
		{
			if (rawTile == -1)
			{
				return;
			}
			bool flag2 = (rawTile & (int)flag) == 0;
			rawTile = ((!flag2) ? (rawTile & (int)(~(int)flag)) : (rawTile | (int)flag));
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00046FE0 File Offset: 0x000451E0
		public static Vector3 FlipSpriteVertexPosition(tk2dTileMap tileMap, tk2dSpriteDefinition spriteDef, Vector3 pos, bool flipH, bool flipV)
		{
			float num = tileMap.data.tileOrigin.x + 0.5f * tileMap.data.tileSize.x;
			float num2 = tileMap.data.tileOrigin.y + 0.5f * tileMap.data.tileSize.y;
			if (flipH)
			{
				pos.x = num - (pos.x - num);
			}
			if (flipV)
			{
				pos.y = num2 - (pos.y - num2);
			}
			return pos;
		}

		// Token: 0x04000B8C RID: 2956
		private const int tileMask = 16777215;

		// Token: 0x04000B8D RID: 2957
		private static List<int> TilePrefabsX;

		// Token: 0x04000B8E RID: 2958
		private static List<int> TilePrefabsY;

		// Token: 0x04000B8F RID: 2959
		private static List<int> TilePrefabsLayer;

		// Token: 0x04000B90 RID: 2960
		private static List<GameObject> TilePrefabsInstance;
	}
}
