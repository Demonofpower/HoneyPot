using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x020001A0 RID: 416
	public static class ColliderBuilder
	{
		// Token: 0x06000A5E RID: 2654 RVA: 0x00047B68 File Offset: 0x00045D68
		public static void Build(tk2dTileMap tileMap, bool forceBuild)
		{
			bool flag = !forceBuild;
			int num = tileMap.Layers.Length;
			for (int i = 0; i < num; i++)
			{
				Layer layer = tileMap.Layers[i];
				if (!layer.IsEmpty && tileMap.data.Layers[i].generateCollider)
				{
					for (int j = 0; j < layer.numRows; j++)
					{
						int baseY = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int baseX = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							if (!flag || chunk.Dirty)
							{
								if (!chunk.IsEmpty)
								{
									ColliderBuilder.BuildForChunk(tileMap, chunk, baseX, baseY);
									PhysicMaterial physicMaterial = tileMap.data.Layers[i].physicMaterial;
									if (physicMaterial != null)
									{
										chunk.meshCollider.sharedMaterial = physicMaterial;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00047C80 File Offset: 0x00045E80
		public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY)
		{
			Vector3[] array = new Vector3[0];
			int[] array2 = new int[0];
			ColliderBuilder.BuildLocalMeshForChunk(tileMap, chunk, baseX, baseY, ref array, ref array2);
			if (array2.Length > 6)
			{
				array = ColliderBuilder.WeldVertices(array, ref array2);
				array2 = ColliderBuilder.RemoveDuplicateFaces(array2);
			}
			if (array.Length > 0)
			{
				if (chunk.colliderMesh != null)
				{
					UnityEngine.Object.DestroyImmediate(chunk.colliderMesh);
					chunk.colliderMesh = null;
				}
				if (chunk.meshCollider == null)
				{
					chunk.meshCollider = chunk.gameObject.GetComponent<MeshCollider>();
					if (chunk.meshCollider == null)
					{
						chunk.meshCollider = chunk.gameObject.AddComponent<MeshCollider>();
					}
				}
				chunk.colliderMesh = new Mesh();
				chunk.colliderMesh.vertices = array;
				chunk.colliderMesh.triangles = array2;
				chunk.colliderMesh.RecalculateBounds();
				chunk.meshCollider.sharedMesh = chunk.colliderMesh;
			}
			else
			{
				chunk.DestroyColliderData(tileMap);
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00047D7C File Offset: 0x00045F7C
		private static void BuildLocalMeshForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, ref Vector3[] vertices, ref int[] indices)
		{
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
			Vector3 tileSize = tileMap.data.tileSize;
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			float num2 = 0f;
			float num3 = 0f;
			tileMap.data.GetTileOffset(out num2, out num3);
			int[] spriteIds = chunk.spriteIds;
			for (int i = 0; i < tileMap.partitionSizeY; i++)
			{
				float num4 = (float)(baseY + i & 1) * num2;
				for (int j = 0; j < tileMap.partitionSizeX; j++)
				{
					int rawTile = spriteIds[i * tileMap.partitionSizeX + j];
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(rawTile);
					Vector3 b = new Vector3(tileSize.x * ((float)j + num4), tileSize.y * (float)i, 0f);
					if (tileFromRawTile >= 0 && tileFromRawTile < num)
					{
						if (!tilePrefabs[tileFromRawTile])
						{
							bool flag = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipX);
							bool flag2 = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipY);
							bool flag3 = false;
							if (flag)
							{
								flag3 = !flag3;
							}
							if (flag2)
							{
								flag3 = !flag3;
							}
							tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
							int count = list.Count;
							if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
							{
								Vector3 a = tk2dSpriteDefinition.colliderVertices[0];
								Vector3 b2 = tk2dSpriteDefinition.colliderVertices[1];
								Vector3 vector = a - b2;
								Vector3 vector2 = a + b2;
								Vector3[] array = new Vector3[]
								{
									new Vector3(vector.x, vector.y, vector.z),
									new Vector3(vector.x, vector.y, vector2.z),
									new Vector3(vector2.x, vector.y, vector.z),
									new Vector3(vector2.x, vector.y, vector2.z),
									new Vector3(vector.x, vector2.y, vector.z),
									new Vector3(vector.x, vector2.y, vector2.z),
									new Vector3(vector2.x, vector2.y, vector.z),
									new Vector3(vector2.x, vector2.y, vector2.z)
								};
								for (int k = 0; k < 8; k++)
								{
									Vector3 a2 = BuilderUtil.FlipSpriteVertexPosition(tileMap, tk2dSpriteDefinition, array[k], flag, flag2);
									list.Add(a2 + b);
								}
								int[] array2 = new int[]
								{
									2,
									1,
									0,
									3,
									1,
									2,
									4,
									5,
									6,
									6,
									5,
									7,
									6,
									7,
									3,
									6,
									3,
									2,
									1,
									5,
									4,
									0,
									1,
									4
								};
								int[] array3 = array2;
								for (int l = 0; l < array3.Length; l++)
								{
									int num5 = (!flag3) ? l : (array3.Length - 1 - l);
									list2.Add(count + array3[num5]);
								}
							}
							else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
							{
								for (int m = 0; m < tk2dSpriteDefinition.colliderVertices.Length; m++)
								{
									Vector3 a3 = BuilderUtil.FlipSpriteVertexPosition(tileMap, tk2dSpriteDefinition, tk2dSpriteDefinition.colliderVertices[m], flag, flag2);
									list.Add(a3 + b);
								}
								int[] colliderIndicesFwd = tk2dSpriteDefinition.colliderIndicesFwd;
								for (int n = 0; n < colliderIndicesFwd.Length; n++)
								{
									int num6 = (!flag3) ? n : (colliderIndicesFwd.Length - 1 - n);
									list2.Add(count + colliderIndicesFwd[num6]);
								}
							}
						}
					}
				}
			}
			vertices = list.ToArray();
			indices = list2.ToArray();
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000481C0 File Offset: 0x000463C0
		private static int CompareWeldVertices(Vector3 a, Vector3 b)
		{
			float num = 0.01f;
			float f = a.x - b.x;
			if (Mathf.Abs(f) > num)
			{
				return (int)Mathf.Sign(f);
			}
			float f2 = a.y - b.y;
			if (Mathf.Abs(f2) > num)
			{
				return (int)Mathf.Sign(f2);
			}
			float f3 = a.z - b.z;
			if (Mathf.Abs(f3) > num)
			{
				return (int)Mathf.Sign(f3);
			}
			return 0;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00048240 File Offset: 0x00046440
		private static Vector3[] WeldVertices(Vector3[] vertices, ref int[] indices)
		{
			int[] array = new int[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = i;
			}
			Array.Sort<int>(array, (int a, int b) => ColliderBuilder.CompareWeldVertices(vertices[a], vertices[b]));
			List<Vector3> list = new List<Vector3>();
			int[] array2 = new int[vertices.Length];
			Vector3 vector = vertices[array[0]];
			list.Add(vector);
			array2[array[0]] = list.Count - 1;
			for (int j = 1; j < array.Length; j++)
			{
				Vector3 vector2 = vertices[array[j]];
				if (ColliderBuilder.CompareWeldVertices(vector2, vector) != 0)
				{
					vector = vector2;
					list.Add(vector);
					array2[array[j]] = list.Count - 1;
				}
				array2[array[j]] = list.Count - 1;
			}
			for (int k = 0; k < indices.Length; k++)
			{
				indices[k] = array2[indices[k]];
			}
			return list.ToArray();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0004836C File Offset: 0x0004656C
		private static int CompareDuplicateFaces(int[] indices, int face0index, int face1index)
		{
			for (int i = 0; i < 3; i++)
			{
				int num = indices[face0index + i] - indices[face1index + i];
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x000483A0 File Offset: 0x000465A0
		private static int[] RemoveDuplicateFaces(int[] indices)
		{
			int[] sortedFaceIndices = new int[indices.Length];
			for (int i = 0; i < indices.Length; i += 3)
			{
				int[] array = new int[]
				{
					indices[i],
					indices[i + 1],
					indices[i + 2]
				};
				Array.Sort<int>(array);
				sortedFaceIndices[i] = array[0];
				sortedFaceIndices[i + 1] = array[1];
				sortedFaceIndices[i + 2] = array[2];
			}
			int[] array2 = new int[indices.Length / 3];
			for (int j = 0; j < indices.Length; j += 3)
			{
				array2[j / 3] = j;
			}
			Array.Sort<int>(array2, (int a, int b) => ColliderBuilder.CompareDuplicateFaces(sortedFaceIndices, a, b));
			List<int> list = new List<int>();
			for (int k = 0; k < array2.Length; k++)
			{
				if (k != array2.Length - 1 && ColliderBuilder.CompareDuplicateFaces(sortedFaceIndices, array2[k], array2[k + 1]) == 0)
				{
					k++;
				}
				else
				{
					for (int l = 0; l < 3; l++)
					{
						list.Add(indices[array2[k] + l]);
					}
				}
			}
			return list.ToArray();
		}
	}
}
