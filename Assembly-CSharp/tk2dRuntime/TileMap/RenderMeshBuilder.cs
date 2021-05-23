using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x020001A8 RID: 424
	public static class RenderMeshBuilder
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x00048600 File Offset: 0x00046800
		public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, ColorChunk colorChunk, bool useColor, bool skipPrefabs, int baseX, int baseY)
		{
			List<Vector3> list = new List<Vector3>();
			List<Color> list2 = new List<Color>();
			List<Vector2> list3 = new List<Vector2>();
			int[] spriteIds = chunk.spriteIds;
			Vector3 tileSize = tileMap.data.tileSize;
			int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			Color32 c = (!useColor || tileMap.ColorChannel == null) ? Color.white : tileMap.ColorChannel.clearColor;
			if (colorChunk == null || colorChunk.colors.Length == 0)
			{
				useColor = false;
			}
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			BuilderUtil.GetLoopOrder(tileMap.data.sortMethod, tileMap.partitionSizeX, tileMap.partitionSizeY, out num2, out num3, out num4, out num5, out num6, out num7);
			float num8 = 0f;
			float num9 = 0f;
			tileMap.data.GetTileOffset(out num8, out num9);
			List<int>[] array = new List<int>[tileMap.SpriteCollectionInst.materials.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<int>();
			}
			int num10 = tileMap.partitionSizeX + 1;
			for (int num11 = num5; num11 != num6; num11 += num7)
			{
				float num12 = (float)(baseY + num11 & 1) * num8;
				for (int num13 = num2; num13 != num3; num13 += num4)
				{
					int rawTile = spriteIds[num11 * tileMap.partitionSizeX + num13];
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(rawTile);
					bool flag = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipX);
					bool flag2 = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipY);
					Vector3 a = new Vector3(tileSize.x * ((float)num13 + num12), tileSize.y * (float)num11, 0f);
					if (tileFromRawTile >= 0 && tileFromRawTile < num)
					{
						if (!skipPrefabs || !tilePrefabs[tileFromRawTile])
						{
							tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
							int count = list.Count;
							for (int j = 0; j < tk2dSpriteDefinition.positions.Length; j++)
							{
								Vector3 vector = BuilderUtil.FlipSpriteVertexPosition(tileMap, tk2dSpriteDefinition, tk2dSpriteDefinition.positions[j], flag, flag2);
								if (useColor)
								{
									Color a2 = colorChunk.colors[num11 * num10 + num13];
									Color b = colorChunk.colors[num11 * num10 + num13 + 1];
									Color a3 = colorChunk.colors[(num11 + 1) * num10 + num13];
									Color b2 = colorChunk.colors[(num11 + 1) * num10 + (num13 + 1)];
									Vector3 a4 = vector - tk2dSpriteDefinition.untrimmedBoundsData[0];
									Vector3 vector2 = a4 + tileMap.data.tileSize * 0.5f;
									float t = Mathf.Clamp01(vector2.x / tileMap.data.tileSize.x);
									float t2 = Mathf.Clamp01(vector2.y / tileMap.data.tileSize.y);
									Color item = Color.Lerp(Color.Lerp(a2, b, t), Color.Lerp(a3, b2, t), t2);
									list2.Add(item);
								}
								else
								{
									list2.Add(c);
								}
								list.Add(a + vector);
								list3.Add(tk2dSpriteDefinition.uvs[j]);
							}
							bool flag3 = false;
							if (flag)
							{
								flag3 = !flag3;
							}
							if (flag2)
							{
								flag3 = !flag3;
							}
							List<int> list4 = array[tk2dSpriteDefinition.materialId];
							for (int k = 0; k < tk2dSpriteDefinition.indices.Length; k++)
							{
								int num14 = (!flag3) ? k : (tk2dSpriteDefinition.indices.Length - 1 - k);
								list4.Add(count + tk2dSpriteDefinition.indices[num14]);
							}
						}
					}
				}
			}
			if (chunk.mesh == null)
			{
				chunk.mesh = new Mesh();
			}
			chunk.mesh.vertices = list.ToArray();
			chunk.mesh.uv = list3.ToArray();
			chunk.mesh.colors = list2.ToArray();
			List<Material> list5 = new List<Material>();
			int num15 = 0;
			int num16 = 0;
			foreach (List<int> list6 in array)
			{
				if (list6.Count > 0)
				{
					list5.Add(tileMap.SpriteCollectionInst.materials[num15]);
					num16++;
				}
				num15++;
			}
			if (num16 > 0)
			{
				chunk.mesh.subMeshCount = num16;
				chunk.gameObject.renderer.materials = list5.ToArray();
				int num17 = 0;
				foreach (List<int> list7 in array)
				{
					if (list7.Count > 0)
					{
						chunk.mesh.SetTriangles(list7.ToArray(), num17);
						num17++;
					}
				}
			}
			chunk.mesh.RecalculateBounds();
			MeshFilter component = chunk.gameObject.GetComponent<MeshFilter>();
			component.sharedMesh = chunk.mesh;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00048B64 File Offset: 0x00046D64
		public static void Build(tk2dTileMap tileMap, bool editMode, bool forceBuild)
		{
			bool skipPrefabs = !editMode;
			bool flag = !forceBuild;
			int numLayers = tileMap.data.NumLayers;
			for (int i = 0; i < numLayers; i++)
			{
				Layer layer = tileMap.Layers[i];
				if (!layer.IsEmpty)
				{
					LayerInfo layerInfo = tileMap.data.Layers[i];
					bool useColor = !tileMap.ColorChannel.IsEmpty && tileMap.data.Layers[i].useColor;
					for (int j = 0; j < layer.numRows; j++)
					{
						int baseY = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int baseX = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							ColorChunk chunk2 = tileMap.ColorChannel.GetChunk(k, j);
							bool flag2 = chunk2 != null && chunk2.Dirty;
							if (!flag || flag2 || chunk.Dirty)
							{
								if (chunk.mesh != null)
								{
									chunk.mesh.Clear();
								}
								if (!chunk.IsEmpty)
								{
									if (editMode || (!editMode && !layerInfo.skipMeshGeneration))
									{
										RenderMeshBuilder.BuildForChunk(tileMap, chunk, chunk2, useColor, skipPrefabs, baseX, baseY);
									}
									if (chunk.mesh != null)
									{
										tileMap.TouchMesh(chunk.mesh);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
