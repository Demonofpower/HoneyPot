using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
public static class tk2dSpriteGeomGen
{
	// Token: 0x060009AA RID: 2474 RVA: 0x000422EC File Offset: 0x000404EC
	public static void SetSpriteColors(Color32[] dest, int offset, int numVertices, Color c, bool premulAlpha)
	{
		if (premulAlpha)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		Color32 color = c;
		for (int i = 0; i < numVertices; i++)
		{
			dest[offset + i] = color;
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00042368 File Offset: 0x00040568
	public static Vector2 GetAnchorOffset(tk2dBaseSprite.Anchor anchor, float width, float height)
	{
		Vector2 zero = Vector2.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero.x = (float)((int)(width / 2f));
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.x = (float)((int)width);
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.LowerRight:
			zero.y = (float)((int)height);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero.y = (float)((int)(height / 2f));
			break;
		}
		return zero;
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x000098AB File Offset: 0x00007AAB
	public static void GetSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		numVertices = spriteDef.positions.Length;
		numIndices = spriteDef.indices.Length;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00042430 File Offset: 0x00040630
	public static void SetSpriteGeom(Vector3[] pos, Vector2[] uv, Vector3[] norm, Vector4[] tang, int offset, tk2dSpriteDefinition spriteDef, Vector3 scale)
	{
		for (int i = 0; i < spriteDef.positions.Length; i++)
		{
			pos[offset + i] = Vector3.Scale(spriteDef.positions[i], scale);
		}
		for (int j = 0; j < spriteDef.uvs.Length; j++)
		{
			uv[offset + j] = spriteDef.uvs[j];
		}
		if (norm != null && spriteDef.normals != null)
		{
			for (int k = 0; k < spriteDef.normals.Length; k++)
			{
				norm[offset + k] = spriteDef.normals[k];
			}
		}
		if (tang != null && spriteDef.tangents != null)
		{
			for (int l = 0; l < spriteDef.tangents.Length; l++)
			{
				tang[offset + l] = spriteDef.tangents[l];
			}
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00042554 File Offset: 0x00040754
	public static void SetSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		for (int i = 0; i < spriteDef.indices.Length; i++)
		{
			indices[offset + i] = vStart + spriteDef.indices[i];
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000098C1 File Offset: 0x00007AC1
	public static void GetClippedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 4;
			numIndices = 6;
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0004258C File Offset: 0x0004078C
	public static void SetClippedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 clipBottomLeft, Vector2 clipTopRight, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length == 4)
		{
			Vector3 vector = spriteDef.untrimmedBoundsData[0] - spriteDef.untrimmedBoundsData[1] * 0.5f;
			Vector3 vector2 = spriteDef.untrimmedBoundsData[0] + spriteDef.untrimmedBoundsData[1] * 0.5f;
			float num = Mathf.Lerp(vector.x, vector2.x, clipBottomLeft.x);
			float num2 = Mathf.Lerp(vector.x, vector2.x, clipTopRight.x);
			float num3 = Mathf.Lerp(vector.y, vector2.y, clipBottomLeft.y);
			float num4 = Mathf.Lerp(vector.y, vector2.y, clipTopRight.y);
			Vector3 a = spriteDef.boundsData[1];
			Vector3 vector3 = spriteDef.boundsData[0] - a * 0.5f;
			float value = (num - vector3.x) / a.x;
			float value2 = (num2 - vector3.x) / a.x;
			float value3 = (num3 - vector3.y) / a.y;
			float value4 = (num4 - vector3.y) / a.y;
			Vector2 vector4 = new Vector2(Mathf.Clamp01(value), Mathf.Clamp01(value3));
			Vector2 vector5 = new Vector2(Mathf.Clamp01(value2), Mathf.Clamp01(value4));
			Vector3 vector6 = spriteDef.positions[0];
			Vector3 vector7 = spriteDef.positions[3];
			Vector3 vector8 = new Vector3(Mathf.Lerp(vector6.x, vector7.x, vector4.x) * scale.x, Mathf.Lerp(vector6.y, vector7.y, vector4.y) * scale.y, vector6.z * scale.z);
			Vector3 vector9 = new Vector3(Mathf.Lerp(vector6.x, vector7.x, vector5.x) * scale.x, Mathf.Lerp(vector6.y, vector7.y, vector5.y) * scale.y, vector6.z * scale.z);
			boundsCenter.Set(vector8.x + (vector9.x - vector8.x) * 0.5f, vector8.y + (vector9.y - vector8.y) * 0.5f, colliderOffsetZ);
			boundsExtents.Set((vector9.x - vector8.x) * 0.5f, (vector9.y - vector8.y) * 0.5f, colliderExtentZ);
			pos[offset] = new Vector3(vector8.x, vector8.y, vector8.z);
			pos[offset + 1] = new Vector3(vector9.x, vector8.y, vector8.z);
			pos[offset + 2] = new Vector3(vector8.x, vector9.y, vector8.z);
			pos[offset + 3] = new Vector3(vector9.x, vector9.y, vector8.z);
			if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
			{
				Vector2 vector10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.x));
				Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
				uv[offset] = new Vector2(vector10.x, vector10.y);
				uv[offset + 1] = new Vector2(vector10.x, vector11.y);
				uv[offset + 2] = new Vector2(vector11.x, vector10.y);
				uv[offset + 3] = new Vector2(vector11.x, vector11.y);
			}
			else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
			{
				Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.x));
				Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
				uv[offset] = new Vector2(vector12.x, vector12.y);
				uv[offset + 2] = new Vector2(vector13.x, vector12.y);
				uv[offset + 1] = new Vector2(vector12.x, vector13.y);
				uv[offset + 3] = new Vector2(vector13.x, vector13.y);
			}
			else
			{
				Vector2 vector14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector4.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector4.y));
				Vector2 vector15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.y));
				uv[offset] = new Vector2(vector14.x, vector14.y);
				uv[offset + 1] = new Vector2(vector15.x, vector14.y);
				uv[offset + 2] = new Vector2(vector14.x, vector15.y);
				uv[offset + 3] = new Vector2(vector15.x, vector15.y);
			}
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000098E2 File Offset: 0x00007AE2
	public static void SetClippedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			indices[offset] = vStart;
			indices[offset + 1] = vStart + 3;
			indices[offset + 2] = vStart + 1;
			indices[offset + 3] = vStart + 2;
			indices[offset + 4] = vStart + 3;
			indices[offset + 5] = vStart;
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0000991C File Offset: 0x00007B1C
	public static void GetSlicedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 16;
			numIndices = ((!borderOnly) ? 54 : 48);
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x00042D4C File Offset: 0x00040F4C
	public static void SetSlicedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length == 4)
		{
			float x = spriteDef.texelSize.x;
			float y = spriteDef.texelSize.y;
			Vector3[] positions = spriteDef.positions;
			float num = positions[1].x - positions[0].x;
			float num2 = positions[2].y - positions[0].y;
			float num3 = borderTopRight.y * num2;
			float y2 = borderBottomLeft.y * num2;
			float num4 = borderTopRight.x * num;
			float x2 = borderBottomLeft.x * num;
			float num5 = dimensions.x * x;
			float num6 = dimensions.y * y;
			float num7 = 0f;
			float num8 = 0f;
			switch (anchor)
			{
			case tk2dBaseSprite.Anchor.LowerCenter:
			case tk2dBaseSprite.Anchor.MiddleCenter:
			case tk2dBaseSprite.Anchor.UpperCenter:
				num7 = (float)(-(float)((int)(dimensions.x / 2f)));
				break;
			case tk2dBaseSprite.Anchor.LowerRight:
			case tk2dBaseSprite.Anchor.MiddleRight:
			case tk2dBaseSprite.Anchor.UpperRight:
				num7 = (float)(-(float)((int)dimensions.x));
				break;
			}
			switch (anchor)
			{
			case tk2dBaseSprite.Anchor.MiddleLeft:
			case tk2dBaseSprite.Anchor.MiddleCenter:
			case tk2dBaseSprite.Anchor.MiddleRight:
				num8 = (float)(-(float)((int)(dimensions.y / 2f)));
				break;
			case tk2dBaseSprite.Anchor.UpperLeft:
			case tk2dBaseSprite.Anchor.UpperCenter:
			case tk2dBaseSprite.Anchor.UpperRight:
				num8 = (float)(-(float)((int)dimensions.y));
				break;
			}
			num7 *= x;
			num8 *= y;
			boundsCenter.Set(scale.x * (num5 * 0.5f + num7), scale.y * (num6 * 0.5f + num8), colliderOffsetZ);
			boundsExtents.Set(scale.x * (num5 * 0.5f), scale.y * (num6 * 0.5f), colliderExtentZ);
			Vector2[] uvs = spriteDef.uvs;
			Vector2 vector = uvs[1] - uvs[0];
			Vector2 vector2 = uvs[2] - uvs[0];
			Vector3 vector3 = new Vector3(num7, num8, 0f);
			Vector3[] array = new Vector3[]
			{
				vector3,
				vector3 + new Vector3(0f, y2, 0f),
				vector3 + new Vector3(0f, num6 - num3, 0f),
				vector3 + new Vector3(0f, num6, 0f)
			};
			Vector2[] array2 = new Vector2[]
			{
				uvs[0],
				uvs[0] + vector2 * borderBottomLeft.y,
				uvs[0] + vector2 * (1f - borderTopRight.y),
				uvs[0] + vector2
			};
			for (int i = 0; i < 4; i++)
			{
				pos[offset + i * 4] = array[i];
				pos[offset + i * 4 + 1] = array[i] + new Vector3(x2, 0f, 0f);
				pos[offset + i * 4 + 2] = array[i] + new Vector3(num5 - num4, 0f, 0f);
				pos[offset + i * 4 + 3] = array[i] + new Vector3(num5, 0f, 0f);
				for (int j = 0; j < 4; j++)
				{
					pos[offset + i * 4 + j] = Vector3.Scale(pos[offset + i * 4 + j], scale);
				}
				uv[offset + i * 4] = array2[i];
				uv[offset + i * 4 + 1] = array2[i] + vector * borderBottomLeft.x;
				uv[offset + i * 4 + 2] = array2[i] + vector * (1f - borderTopRight.x);
				uv[offset + i * 4 + 3] = array2[i] + vector;
			}
		}
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000432A4 File Offset: 0x000414A4
	public static void SetSlicedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			int[] array = new int[]
			{
				0,
				4,
				1,
				1,
				4,
				5,
				1,
				5,
				2,
				2,
				5,
				6,
				2,
				6,
				3,
				3,
				6,
				7,
				4,
				8,
				5,
				5,
				8,
				9,
				6,
				10,
				7,
				7,
				10,
				11,
				8,
				12,
				9,
				9,
				12,
				13,
				9,
				13,
				10,
				10,
				13,
				14,
				10,
				14,
				11,
				11,
				14,
				15,
				5,
				9,
				6,
				6,
				9,
				10
			};
			int num = array.Length;
			if (borderOnly)
			{
				num -= 6;
			}
			for (int i = 0; i < num; i++)
			{
				indices[offset + i] = vStart + array[i];
			}
		}
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00043300 File Offset: 0x00041500
	public static void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		numVertices = num * num2 * 4;
		numIndices = num * num2 * 6;
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00043374 File Offset: 0x00041574
	public static void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		Vector2 vector = new Vector2(dimensions.x * spriteDef.texelSize.x * scale.x, dimensions.y * spriteDef.texelSize.y * scale.y);
		Vector3 vector2 = Vector3.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector2.x = -(vector.x / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.x = -vector.x;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector2.y = -(vector.y / 2f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.y = -vector.y;
			break;
		}
		Vector3 vector3 = vector2;
		vector2 -= Vector3.Scale(spriteDef.positions[0], scale);
		boundsCenter.Set(vector.x * 0.5f + vector3.x, vector.y * 0.5f + vector3.y, colliderOffsetZ);
		boundsExtents.Set(vector.x * 0.5f, vector.y * 0.5f, colliderExtentZ);
		int num3 = 0;
		Vector3 vector4 = Vector3.Scale(spriteDef.untrimmedBoundsData[1], scale);
		Vector3 zero = Vector3.zero;
		Vector3 a = zero;
		for (int i = 0; i < num2; i++)
		{
			a.x = zero.x;
			for (int j = 0; j < num; j++)
			{
				float num4 = 1f;
				float num5 = 1f;
				if (Mathf.Abs(a.x + vector4.x) > Mathf.Abs(vector.x))
				{
					num4 = vector.x % vector4.x / vector4.x;
				}
				if (Mathf.Abs(a.y + vector4.y) > Mathf.Abs(vector.y))
				{
					num5 = vector.y % vector4.y / vector4.y;
				}
				Vector3 a2 = a + vector2;
				if (num4 != 1f || num5 != 1f)
				{
					Vector2 zero2 = Vector2.zero;
					Vector2 vector5 = new Vector2(num4, num5);
					Vector3 vector6 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, zero2.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, zero2.y) * scale.y, spriteDef.positions[0].z * scale.z);
					Vector3 vector7 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, vector5.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, vector5.y) * scale.y, spriteDef.positions[0].z * scale.z);
					pos[offset + num3] = a2 + new Vector3(vector6.x, vector6.y, vector6.z);
					pos[offset + num3 + 1] = a2 + new Vector3(vector7.x, vector6.y, vector6.z);
					pos[offset + num3 + 2] = a2 + new Vector3(vector6.x, vector7.y, vector6.z);
					pos[offset + num3 + 3] = a2 + new Vector3(vector7.x, vector7.y, vector6.z);
					if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						Vector2 vector8 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector9 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
						uv[offset + num3] = new Vector2(vector8.x, vector8.y);
						uv[offset + num3 + 1] = new Vector2(vector8.x, vector9.y);
						uv[offset + num3 + 2] = new Vector2(vector9.x, vector8.y);
						uv[offset + num3 + 3] = new Vector2(vector9.x, vector9.y);
					}
					else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
					{
						Vector2 vector10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.x));
						Vector2 vector11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.x));
						uv[offset + num3] = new Vector2(vector10.x, vector10.y);
						uv[offset + num3 + 2] = new Vector2(vector11.x, vector10.y);
						uv[offset + num3 + 1] = new Vector2(vector10.x, vector11.y);
						uv[offset + num3 + 3] = new Vector2(vector11.x, vector11.y);
					}
					else
					{
						Vector2 vector12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero2.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero2.y));
						Vector2 vector13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, vector5.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, vector5.y));
						uv[offset + num3] = new Vector2(vector12.x, vector12.y);
						uv[offset + num3 + 1] = new Vector2(vector13.x, vector12.y);
						uv[offset + num3 + 2] = new Vector2(vector12.x, vector13.y);
						uv[offset + num3 + 3] = new Vector2(vector13.x, vector13.y);
					}
				}
				else
				{
					pos[offset + num3] = a2 + Vector3.Scale(spriteDef.positions[0], scale);
					pos[offset + num3 + 1] = a2 + Vector3.Scale(spriteDef.positions[1], scale);
					pos[offset + num3 + 2] = a2 + Vector3.Scale(spriteDef.positions[2], scale);
					pos[offset + num3 + 3] = a2 + Vector3.Scale(spriteDef.positions[3], scale);
					uv[offset + num3] = spriteDef.uvs[0];
					uv[offset + num3 + 1] = spriteDef.uvs[1];
					uv[offset + num3 + 2] = spriteDef.uvs[2];
					uv[offset + num3 + 3] = spriteDef.uvs[3];
				}
				num3 += 4;
				a.x += vector4.x;
			}
			a.y += vector4.y;
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00043E44 File Offset: 0x00042044
	public static void SetTiledSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num;
		int num2;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, spriteDef, dimensions);
		int num3 = 0;
		for (int i = 0; i < num2; i += 6)
		{
			indices[offset + i] = vStart + spriteDef.indices[0] + num3;
			indices[offset + i + 1] = vStart + spriteDef.indices[1] + num3;
			indices[offset + i + 2] = vStart + spriteDef.indices[2] + num3;
			indices[offset + i + 3] = vStart + spriteDef.indices[3] + num3;
			indices[offset + i + 4] = vStart + spriteDef.indices[4] + num3;
			indices[offset + i + 5] = vStart + spriteDef.indices[5] + num3;
			num3 += 4;
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00043EE8 File Offset: 0x000420E8
	public static void SetBoxMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, Vector3 origin, Vector3 extents, Matrix4x4 mat, Vector3 baseScale)
	{
		tk2dSpriteGeomGen.boxScaleMatrix.m03 = origin.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m13 = origin.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m23 = origin.z * baseScale.z;
		tk2dSpriteGeomGen.boxScaleMatrix.m00 = extents.x * baseScale.x;
		tk2dSpriteGeomGen.boxScaleMatrix.m11 = extents.y * baseScale.y;
		tk2dSpriteGeomGen.boxScaleMatrix.m22 = extents.z * baseScale.z;
		Matrix4x4 matrix4x = mat * tk2dSpriteGeomGen.boxScaleMatrix;
		for (int i = 0; i < 8; i++)
		{
			pos[posOffset + i] = matrix4x.MultiplyPoint(tk2dSpriteGeomGen.boxUnitVertices[i]);
		}
		float num = mat.m00 * mat.m11 * mat.m22 * baseScale.x * baseScale.y * baseScale.z;
		int[] array = (num < 0f) ? tk2dSpriteGeomGen.boxIndicesBack : tk2dSpriteGeomGen.boxIndicesFwd;
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00044040 File Offset: 0x00042240
	public static void SetSpriteDefinitionMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, tk2dSpriteDefinition spriteDef, Matrix4x4 mat, Vector3 baseScale)
	{
		for (int i = 0; i < spriteDef.colliderVertices.Length; i++)
		{
			Vector3 vector = Vector3.Scale(spriteDef.colliderVertices[i], baseScale);
			vector = mat.MultiplyPoint(vector);
			pos[posOffset + i] = vector;
		}
		float num = mat.m00 * mat.m11 * mat.m22;
		int[] array = (num < 0f) ? spriteDef.colliderIndicesBack : spriteDef.colliderIndicesFwd;
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	// Token: 0x04000B33 RID: 2867
	private static readonly int[] boxIndicesBack = new int[]
	{
		0,
		1,
		2,
		2,
		1,
		3,
		6,
		5,
		4,
		7,
		5,
		6,
		3,
		7,
		6,
		2,
		3,
		6,
		4,
		5,
		1,
		4,
		1,
		0,
		6,
		4,
		0,
		6,
		0,
		2,
		1,
		7,
		3,
		5,
		7,
		1
	};

	// Token: 0x04000B34 RID: 2868
	private static readonly int[] boxIndicesFwd = new int[]
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
		4,
		0,
		4,
		6,
		2,
		0,
		6,
		3,
		7,
		1,
		1,
		7,
		5
	};

	// Token: 0x04000B35 RID: 2869
	private static readonly Vector3[] boxUnitVertices = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x04000B36 RID: 2870
	private static Matrix4x4 boxScaleMatrix = Matrix4x4.identity;
}
