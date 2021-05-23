using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace tk2dRuntime
{
	// Token: 0x02000169 RID: 361
	internal static class SpriteCollectionGenerator
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x00008B96 File Offset: 0x00006D96
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
		{
			return SpriteCollectionGenerator.CreateFromTexture(texture, size, new string[]
			{
				"Unnamed"
			}, new Rect[]
			{
				region
			}, new Vector2[]
			{
				anchor
			});
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0003E528 File Offset: 0x0003C728
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
		{
			Vector2 textureDimensions = new Vector2((float)texture.width, (float)texture.height);
			return SpriteCollectionGenerator.CreateFromTexture(texture, size, textureDimensions, names, regions, null, anchors, null);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0003E558 File Offset: 0x0003C758
		public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Vector2 textureDimensions, string[] names, Rect[] regions, Rect[] trimRects, Vector2[] anchors, bool[] rotated)
		{
			return SpriteCollectionGenerator.CreateFromTexture(null, texture, size, textureDimensions, names, regions, trimRects, anchors, rotated);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0003E578 File Offset: 0x0003C778
		public static tk2dSpriteCollectionData CreateFromTexture(GameObject parentObject, Texture texture, tk2dSpriteCollectionSize size, Vector2 textureDimensions, string[] names, Rect[] regions, Rect[] trimRects, Vector2[] anchors, bool[] rotated)
		{
			GameObject gameObject = (!(parentObject != null)) ? new GameObject("SpriteCollection") : parentObject;
			tk2dSpriteCollectionData tk2dSpriteCollectionData = gameObject.AddComponent<tk2dSpriteCollectionData>();
			tk2dSpriteCollectionData.Transient = true;
			tk2dSpriteCollectionData.version = 3;
			tk2dSpriteCollectionData.invOrthoSize = 1f / size.OrthoSize;
			tk2dSpriteCollectionData.halfTargetHeight = size.TargetHeight * 0.5f;
			tk2dSpriteCollectionData.premultipliedAlpha = false;
			string name = "tk2d/BlendVertexColor";
			tk2dSpriteCollectionData.material = new Material(Shader.Find(name));
			tk2dSpriteCollectionData.material.mainTexture = texture;
			tk2dSpriteCollectionData.materials = new Material[]
			{
				tk2dSpriteCollectionData.material
			};
			tk2dSpriteCollectionData.textures = new Texture[]
			{
				texture
			};
			tk2dSpriteCollectionData.buildKey = UnityEngine.Random.Range(0, int.MaxValue);
			float scale = 2f * size.OrthoSize / size.TargetHeight;
			Rect trimRect = new Rect(0f, 0f, 0f, 0f);
			tk2dSpriteCollectionData.spriteDefinitions = new tk2dSpriteDefinition[regions.Length];
			for (int i = 0; i < regions.Length; i++)
			{
				bool flag = rotated != null && rotated[i];
				if (trimRects != null)
				{
					trimRect = trimRects[i];
				}
				else if (flag)
				{
					trimRect.Set(0f, 0f, regions[i].height, regions[i].width);
				}
				else
				{
					trimRect.Set(0f, 0f, regions[i].width, regions[i].height);
				}
				tk2dSpriteCollectionData.spriteDefinitions[i] = SpriteCollectionGenerator.CreateDefinitionForRegionInTexture(names[i], textureDimensions, scale, regions[i], trimRect, anchors[i], flag);
			}
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in tk2dSpriteCollectionData.spriteDefinitions)
			{
				tk2dSpriteDefinition.material = tk2dSpriteCollectionData.material;
			}
			return tk2dSpriteCollectionData;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0003E794 File Offset: 0x0003C994
		private static tk2dSpriteDefinition CreateDefinitionForRegionInTexture(string name, Vector2 textureDimensions, float scale, Rect uvRegion, Rect trimRect, Vector2 anchor, bool rotated)
		{
			float height = uvRegion.height;
			float width = uvRegion.width;
			float x = textureDimensions.x;
			float y = textureDimensions.y;
			tk2dSpriteDefinition tk2dSpriteDefinition = new tk2dSpriteDefinition();
			tk2dSpriteDefinition.flipped = ((!rotated) ? tk2dSpriteDefinition.FlipMode.None : tk2dSpriteDefinition.FlipMode.TPackerCW);
			tk2dSpriteDefinition.extractRegion = false;
			tk2dSpriteDefinition.name = name;
			tk2dSpriteDefinition.colliderType = tk2dSpriteDefinition.ColliderType.Unset;
			Vector2 vector = new Vector2(0.001f, 0.001f);
			Vector2 vector2 = new Vector2((uvRegion.x + vector.x) / x, 1f - (uvRegion.y + uvRegion.height + vector.y) / y);
			Vector2 vector3 = new Vector2((uvRegion.x + uvRegion.width - vector.x) / x, 1f - (uvRegion.y - vector.y) / y);
			Vector2 a = new Vector2(trimRect.x - anchor.x, -trimRect.y + anchor.y);
			if (rotated)
			{
				a.y -= width;
			}
			a *= scale;
			Vector3 a2 = new Vector3(-anchor.x * scale, anchor.y * scale, 0f);
			Vector3 vector4 = a2 + new Vector3(trimRect.width * scale, -trimRect.height * scale, 0f);
			Vector3 a3 = new Vector3(0f, -height * scale, 0f);
			Vector3 vector5 = a3 + new Vector3(width * scale, height * scale, 0f);
			if (rotated)
			{
				tk2dSpriteDefinition.positions = new Vector3[]
				{
					new Vector3(-vector5.y + a.x, a3.x + a.y, 0f),
					new Vector3(-a3.y + a.x, a3.x + a.y, 0f),
					new Vector3(-vector5.y + a.x, vector5.x + a.y, 0f),
					new Vector3(-a3.y + a.x, vector5.x + a.y, 0f)
				};
				tk2dSpriteDefinition.uvs = new Vector2[]
				{
					new Vector2(vector2.x, vector3.y),
					new Vector2(vector2.x, vector2.y),
					new Vector2(vector3.x, vector3.y),
					new Vector2(vector3.x, vector2.y)
				};
			}
			else
			{
				tk2dSpriteDefinition.positions = new Vector3[]
				{
					new Vector3(a3.x + a.x, a3.y + a.y, 0f),
					new Vector3(vector5.x + a.x, a3.y + a.y, 0f),
					new Vector3(a3.x + a.x, vector5.y + a.y, 0f),
					new Vector3(vector5.x + a.x, vector5.y + a.y, 0f)
				};
				tk2dSpriteDefinition.uvs = new Vector2[]
				{
					new Vector2(vector2.x, vector2.y),
					new Vector2(vector3.x, vector2.y),
					new Vector2(vector2.x, vector3.y),
					new Vector2(vector3.x, vector3.y)
				};
			}
			tk2dSpriteDefinition.normals = new Vector3[0];
			tk2dSpriteDefinition.tangents = new Vector4[0];
			tk2dSpriteDefinition.indices = new int[]
			{
				0,
				3,
				1,
				2,
				3,
				0
			};
			Vector3 b = new Vector3(a2.x, vector4.y, 0f);
			Vector3 a4 = new Vector3(vector4.x, a2.y, 0f);
			tk2dSpriteDefinition.boundsData = new Vector3[]
			{
				(a4 + b) / 2f,
				a4 - b
			};
			tk2dSpriteDefinition.untrimmedBoundsData = new Vector3[]
			{
				(a4 + b) / 2f,
				a4 - b
			};
			tk2dSpriteDefinition.texelSize = new Vector2(scale, scale);
			return tk2dSpriteDefinition;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0003ED04 File Offset: 0x0003CF04
		public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize spriteCollectionSize, string texturePackerFileContents, Texture texture)
		{
			List<string> list = new List<string>();
			List<Rect> list2 = new List<Rect>();
			List<Rect> list3 = new List<Rect>();
			List<Vector2> list4 = new List<Vector2>();
			List<bool> list5 = new List<bool>();
			int num = 0;
			TextReader textReader = new StringReader(texturePackerFileContents);
			bool flag = false;
			bool flag2 = false;
			string item = string.Empty;
			Rect item2 = default(Rect);
			Rect item3 = default(Rect);
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			for (string text = textReader.ReadLine(); text != null; text = textReader.ReadLine())
			{
				if (text.Length > 0)
				{
					char c = text[0];
					int num2 = num;
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							char c2 = c;
							switch (c2)
							{
							case 'n':
								item = text.Substring(2);
								break;
							case 'o':
							{
								string[] array = text.Split(new char[0]);
								item3.Set((float)int.Parse(array[1]), (float)int.Parse(array[2]), (float)int.Parse(array[3]), (float)int.Parse(array[4]));
								flag2 = true;
								break;
							}
							default:
								if (c2 == '~')
								{
									list.Add(item);
									list5.Add(flag);
									list2.Add(item2);
									if (!flag2)
									{
										if (flag)
										{
											item3.Set(0f, 0f, item2.height, item2.width);
										}
										else
										{
											item3.Set(0f, 0f, item2.width, item2.height);
										}
									}
									list3.Add(item3);
									zero2.Set((float)((int)(item3.width / 2f)), (float)((int)(item3.height / 2f)));
									list4.Add(zero2);
									item = string.Empty;
									flag2 = false;
									flag = false;
								}
								break;
							case 'r':
								flag = (int.Parse(text.Substring(2)) == 1);
								break;
							case 's':
							{
								string[] array2 = text.Split(new char[0]);
								item2.Set((float)int.Parse(array2[1]), (float)int.Parse(array2[2]), (float)int.Parse(array2[3]), (float)int.Parse(array2[4]));
								break;
							}
							}
						}
					}
					else
					{
						char c2 = c;
						if (c2 != 'h')
						{
							if (c2 != 'i')
							{
								if (c2 != 'w')
								{
									if (c2 == '~')
									{
										num++;
									}
								}
								else
								{
									zero.x = (float)int.Parse(text.Substring(2));
								}
							}
						}
						else
						{
							zero.y = (float)int.Parse(text.Substring(2));
						}
					}
				}
			}
			return SpriteCollectionGenerator.CreateFromTexture(texture, spriteCollectionSize, zero, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray());
		}
	}
}
