using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
[Serializable]
public class tk2dSpriteSheetSource
{
	// Token: 0x06000967 RID: 2407 RVA: 0x00041274 File Offset: 0x0003F474
	public void CopyFrom(tk2dSpriteSheetSource src)
	{
		this.texture = src.texture;
		this.tilesX = src.tilesX;
		this.tilesY = src.tilesY;
		this.numTiles = src.numTiles;
		this.anchor = src.anchor;
		this.pad = src.pad;
		this.scale = src.scale;
		this.colliderType = src.colliderType;
		this.version = src.version;
		this.active = src.active;
		this.tileWidth = src.tileWidth;
		this.tileHeight = src.tileHeight;
		this.tileSpacingX = src.tileSpacingX;
		this.tileSpacingY = src.tileSpacingY;
		this.tileMarginX = src.tileMarginX;
		this.tileMarginY = src.tileMarginY;
		this.splitMethod = src.splitMethod;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00041350 File Offset: 0x0003F550
	public bool CompareTo(tk2dSpriteSheetSource src)
	{
		return !(this.texture != src.texture) && this.tilesX == src.tilesX && this.tilesY == src.tilesY && this.numTiles == src.numTiles && this.anchor == src.anchor && this.pad == src.pad && !(this.scale != src.scale) && this.colliderType == src.colliderType && this.version == src.version && this.active == src.active && this.tileWidth == src.tileWidth && this.tileHeight == src.tileHeight && this.tileSpacingX == src.tileSpacingX && this.tileSpacingY == src.tileSpacingY && this.tileMarginX == src.tileMarginX && this.tileMarginY == src.tileMarginY && this.splitMethod == src.splitMethod;
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000969 RID: 2409 RVA: 0x000094A2 File Offset: 0x000076A2
	public string Name
	{
		get
		{
			return (!(this.texture != null)) ? "New Sprite Sheet" : this.texture.name;
		}
	}

	// Token: 0x04000A7C RID: 2684
	public const int CURRENT_VERSION = 1;

	// Token: 0x04000A7D RID: 2685
	public Texture2D texture;

	// Token: 0x04000A7E RID: 2686
	public int tilesX;

	// Token: 0x04000A7F RID: 2687
	public int tilesY;

	// Token: 0x04000A80 RID: 2688
	public int numTiles;

	// Token: 0x04000A81 RID: 2689
	public tk2dSpriteSheetSource.Anchor anchor = tk2dSpriteSheetSource.Anchor.MiddleCenter;

	// Token: 0x04000A82 RID: 2690
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04000A83 RID: 2691
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000A84 RID: 2692
	public bool additive;

	// Token: 0x04000A85 RID: 2693
	public bool active;

	// Token: 0x04000A86 RID: 2694
	public int tileWidth;

	// Token: 0x04000A87 RID: 2695
	public int tileHeight;

	// Token: 0x04000A88 RID: 2696
	public int tileMarginX;

	// Token: 0x04000A89 RID: 2697
	public int tileMarginY;

	// Token: 0x04000A8A RID: 2698
	public int tileSpacingX;

	// Token: 0x04000A8B RID: 2699
	public int tileSpacingY;

	// Token: 0x04000A8C RID: 2700
	public tk2dSpriteSheetSource.SplitMethod splitMethod;

	// Token: 0x04000A8D RID: 2701
	public int version;

	// Token: 0x04000A8E RID: 2702
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x0200017D RID: 381
	public enum Anchor
	{
		// Token: 0x04000A90 RID: 2704
		UpperLeft,
		// Token: 0x04000A91 RID: 2705
		UpperCenter,
		// Token: 0x04000A92 RID: 2706
		UpperRight,
		// Token: 0x04000A93 RID: 2707
		MiddleLeft,
		// Token: 0x04000A94 RID: 2708
		MiddleCenter,
		// Token: 0x04000A95 RID: 2709
		MiddleRight,
		// Token: 0x04000A96 RID: 2710
		LowerLeft,
		// Token: 0x04000A97 RID: 2711
		LowerCenter,
		// Token: 0x04000A98 RID: 2712
		LowerRight
	}

	// Token: 0x0200017E RID: 382
	public enum SplitMethod
	{
		// Token: 0x04000A9A RID: 2714
		UniformDivision
	}
}
