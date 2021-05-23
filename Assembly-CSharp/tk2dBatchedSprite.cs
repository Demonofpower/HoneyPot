using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
[Serializable]
public class tk2dBatchedSprite
{
	// Token: 0x060009BA RID: 2490 RVA: 0x000440F8 File Offset: 0x000422F8
	public tk2dBatchedSprite()
	{
		this.parentId = -1;
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x0000994C File Offset: 0x00007B4C
	// (set) Token: 0x060009BC RID: 2492 RVA: 0x00009959 File Offset: 0x00007B59
	public float BoxColliderOffsetZ
	{
		get
		{
			return this.colliderData.x;
		}
		set
		{
			this.colliderData.x = value;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x00009967 File Offset: 0x00007B67
	// (set) Token: 0x060009BE RID: 2494 RVA: 0x00009974 File Offset: 0x00007B74
	public float BoxColliderExtentZ
	{
		get
		{
			return this.colliderData.y;
		}
		set
		{
			this.colliderData.y = value;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x00009982 File Offset: 0x00007B82
	// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0000998A File Offset: 0x00007B8A
	public string FormattedText
	{
		get
		{
			return this.formattedText;
		}
		set
		{
			this.formattedText = value;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00009993 File Offset: 0x00007B93
	// (set) Token: 0x060009C2 RID: 2498 RVA: 0x0000999B File Offset: 0x00007B9B
	public Vector2 ClippedSpriteRegionBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x000099A4 File Offset: 0x00007BA4
	// (set) Token: 0x060009C4 RID: 2500 RVA: 0x000099AC File Offset: 0x00007BAC
	public Vector2 ClippedSpriteRegionTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00009993 File Offset: 0x00007B93
	// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0000999B File Offset: 0x00007B9B
	public Vector2 SlicedSpriteBorderBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000099A4 File Offset: 0x00007BA4
	// (set) Token: 0x060009C8 RID: 2504 RVA: 0x000099AC File Offset: 0x00007BAC
	public Vector2 SlicedSpriteBorderTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000099B5 File Offset: 0x00007BB5
	// (set) Token: 0x060009CA RID: 2506 RVA: 0x000099BD File Offset: 0x00007BBD
	public Vector2 Dimensions
	{
		get
		{
			return this.internalData2;
		}
		set
		{
			this.internalData2 = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x000099C6 File Offset: 0x00007BC6
	public bool IsDrawn
	{
		get
		{
			return this.type != tk2dBatchedSprite.Type.EmptyGameObject;
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x000099D4 File Offset: 0x00007BD4
	public bool CheckFlag(tk2dBatchedSprite.Flags mask)
	{
		return (this.flags & mask) != tk2dBatchedSprite.Flags.None;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000099E4 File Offset: 0x00007BE4
	public void SetFlag(tk2dBatchedSprite.Flags mask, bool value)
	{
		if (value)
		{
			this.flags |= mask;
		}
		else
		{
			this.flags &= ~mask;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x00009A0E File Offset: 0x00007C0E
	// (set) Token: 0x060009CF RID: 2511 RVA: 0x00009A16 File Offset: 0x00007C16
	public Vector3 CachedBoundsCenter
	{
		get
		{
			return this.cachedBoundsCenter;
		}
		set
		{
			this.cachedBoundsCenter = value;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00009A1F File Offset: 0x00007C1F
	// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00009A27 File Offset: 0x00007C27
	public Vector3 CachedBoundsExtents
	{
		get
		{
			return this.cachedBoundsExtents;
		}
		set
		{
			this.cachedBoundsExtents = value;
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00009A30 File Offset: 0x00007C30
	public tk2dSpriteDefinition GetSpriteDefinition()
	{
		if (this.spriteCollection != null && this.spriteId != -1)
		{
			return this.spriteCollection.inst.spriteDefinitions[this.spriteId];
		}
		return null;
	}

	// Token: 0x04000B37 RID: 2871
	public tk2dBatchedSprite.Type type = tk2dBatchedSprite.Type.Sprite;

	// Token: 0x04000B38 RID: 2872
	public string name = string.Empty;

	// Token: 0x04000B39 RID: 2873
	public int parentId = -1;

	// Token: 0x04000B3A RID: 2874
	public int spriteId;

	// Token: 0x04000B3B RID: 2875
	public int xRefId = -1;

	// Token: 0x04000B3C RID: 2876
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000B3D RID: 2877
	public Quaternion rotation = Quaternion.identity;

	// Token: 0x04000B3E RID: 2878
	public Vector3 position = Vector3.zero;

	// Token: 0x04000B3F RID: 2879
	public Vector3 localScale = Vector3.one;

	// Token: 0x04000B40 RID: 2880
	public Color color = Color.white;

	// Token: 0x04000B41 RID: 2881
	public Vector3 baseScale = Vector3.one;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private Vector2 internalData0;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private Vector2 internalData1;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	private Vector2 internalData2;

	// Token: 0x04000B45 RID: 2885
	[SerializeField]
	private Vector2 colliderData = new Vector2(0f, 1f);

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private string formattedText = string.Empty;

	// Token: 0x04000B47 RID: 2887
	[SerializeField]
	private tk2dBatchedSprite.Flags flags;

	// Token: 0x04000B48 RID: 2888
	public tk2dBaseSprite.Anchor anchor;

	// Token: 0x04000B49 RID: 2889
	public Matrix4x4 relativeMatrix = Matrix4x4.identity;

	// Token: 0x04000B4A RID: 2890
	private Vector3 cachedBoundsCenter = Vector3.zero;

	// Token: 0x04000B4B RID: 2891
	private Vector3 cachedBoundsExtents = Vector3.zero;

	// Token: 0x02000190 RID: 400
	public enum Type
	{
		// Token: 0x04000B4D RID: 2893
		EmptyGameObject,
		// Token: 0x04000B4E RID: 2894
		Sprite,
		// Token: 0x04000B4F RID: 2895
		TiledSprite,
		// Token: 0x04000B50 RID: 2896
		SlicedSprite,
		// Token: 0x04000B51 RID: 2897
		ClippedSprite,
		// Token: 0x04000B52 RID: 2898
		TextMesh
	}

	// Token: 0x02000191 RID: 401
	[Flags]
	public enum Flags
	{
		// Token: 0x04000B54 RID: 2900
		None = 0,
		// Token: 0x04000B55 RID: 2901
		Sprite_CreateBoxCollider = 1,
		// Token: 0x04000B56 RID: 2902
		SlicedSprite_BorderOnly = 2
	}
}
