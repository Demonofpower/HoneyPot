using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000174 RID: 372
[Serializable]
public class tk2dSpriteCollectionDefinition
{
	// Token: 0x06000962 RID: 2402 RVA: 0x00040B34 File Offset: 0x0003ED34
	public void CopyFrom(tk2dSpriteCollectionDefinition src)
	{
		this.name = src.name;
		this.disableTrimming = src.disableTrimming;
		this.additive = src.additive;
		this.scale = src.scale;
		this.texture = src.texture;
		this.materialId = src.materialId;
		this.anchor = src.anchor;
		this.anchorX = src.anchorX;
		this.anchorY = src.anchorY;
		this.overrideMesh = src.overrideMesh;
		this.doubleSidedSprite = src.doubleSidedSprite;
		this.customSpriteGeometry = src.customSpriteGeometry;
		this.geometryIslands = src.geometryIslands;
		this.dice = src.dice;
		this.diceUnitX = src.diceUnitX;
		this.diceUnitY = src.diceUnitY;
		this.pad = src.pad;
		this.source = src.source;
		this.fromSpriteSheet = src.fromSpriteSheet;
		this.hasSpriteSheetId = src.hasSpriteSheetId;
		this.spriteSheetX = src.spriteSheetX;
		this.spriteSheetY = src.spriteSheetY;
		this.spriteSheetId = src.spriteSheetId;
		this.extractRegion = src.extractRegion;
		this.regionX = src.regionX;
		this.regionY = src.regionY;
		this.regionW = src.regionW;
		this.regionH = src.regionH;
		this.regionId = src.regionId;
		this.colliderType = src.colliderType;
		this.boxColliderMin = src.boxColliderMin;
		this.boxColliderMax = src.boxColliderMax;
		this.polyColliderCap = src.polyColliderCap;
		this.colliderColor = src.colliderColor;
		this.colliderConvex = src.colliderConvex;
		this.colliderSmoothSphereCollisions = src.colliderSmoothSphereCollisions;
		this.extraPadding = src.extraPadding;
		if (src.polyColliderIslands != null)
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[src.polyColliderIslands.Length];
			for (int i = 0; i < this.polyColliderIslands.Length; i++)
			{
				this.polyColliderIslands[i] = new tk2dSpriteColliderIsland();
				this.polyColliderIslands[i].CopyFrom(src.polyColliderIslands[i]);
			}
		}
		else
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[0];
		}
		if (src.geometryIslands != null)
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[src.geometryIslands.Length];
			for (int j = 0; j < this.geometryIslands.Length; j++)
			{
				this.geometryIslands[j] = new tk2dSpriteColliderIsland();
				this.geometryIslands[j].CopyFrom(src.geometryIslands[j]);
			}
		}
		else
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[0];
		}
		this.attachPoints = new List<tk2dSpriteDefinition.AttachPoint>(src.attachPoints.Count);
		foreach (tk2dSpriteDefinition.AttachPoint src2 in src.attachPoints)
		{
			tk2dSpriteDefinition.AttachPoint attachPoint = new tk2dSpriteDefinition.AttachPoint();
			attachPoint.CopyFrom(src2);
			this.attachPoints.Add(attachPoint);
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00040E4C File Offset: 0x0003F04C
	public void Clear()
	{
		tk2dSpriteCollectionDefinition src = new tk2dSpriteCollectionDefinition();
		this.CopyFrom(src);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00040E68 File Offset: 0x0003F068
	public bool CompareTo(tk2dSpriteCollectionDefinition src)
	{
		if (this.name != src.name)
		{
			return false;
		}
		if (this.additive != src.additive)
		{
			return false;
		}
		if (this.scale != src.scale)
		{
			return false;
		}
		if (this.texture != src.texture)
		{
			return false;
		}
		if (this.materialId != src.materialId)
		{
			return false;
		}
		if (this.anchor != src.anchor)
		{
			return false;
		}
		if (this.anchorX != src.anchorX)
		{
			return false;
		}
		if (this.anchorY != src.anchorY)
		{
			return false;
		}
		if (this.overrideMesh != src.overrideMesh)
		{
			return false;
		}
		if (this.dice != src.dice)
		{
			return false;
		}
		if (this.diceUnitX != src.diceUnitX)
		{
			return false;
		}
		if (this.diceUnitY != src.diceUnitY)
		{
			return false;
		}
		if (this.pad != src.pad)
		{
			return false;
		}
		if (this.extraPadding != src.extraPadding)
		{
			return false;
		}
		if (this.doubleSidedSprite != src.doubleSidedSprite)
		{
			return false;
		}
		if (this.customSpriteGeometry != src.customSpriteGeometry)
		{
			return false;
		}
		if (this.geometryIslands != src.geometryIslands)
		{
			return false;
		}
		if (this.geometryIslands != null && src.geometryIslands != null)
		{
			if (this.geometryIslands.Length != src.geometryIslands.Length)
			{
				return false;
			}
			for (int i = 0; i < this.geometryIslands.Length; i++)
			{
				if (!this.geometryIslands[i].CompareTo(src.geometryIslands[i]))
				{
					return false;
				}
			}
		}
		if (this.source != src.source)
		{
			return false;
		}
		if (this.fromSpriteSheet != src.fromSpriteSheet)
		{
			return false;
		}
		if (this.hasSpriteSheetId != src.hasSpriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetId != src.spriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetX != src.spriteSheetX)
		{
			return false;
		}
		if (this.spriteSheetY != src.spriteSheetY)
		{
			return false;
		}
		if (this.extractRegion != src.extractRegion)
		{
			return false;
		}
		if (this.regionX != src.regionX)
		{
			return false;
		}
		if (this.regionY != src.regionY)
		{
			return false;
		}
		if (this.regionW != src.regionW)
		{
			return false;
		}
		if (this.regionH != src.regionH)
		{
			return false;
		}
		if (this.regionId != src.regionId)
		{
			return false;
		}
		if (this.colliderType != src.colliderType)
		{
			return false;
		}
		if (this.boxColliderMin != src.boxColliderMin)
		{
			return false;
		}
		if (this.boxColliderMax != src.boxColliderMax)
		{
			return false;
		}
		if (this.polyColliderIslands != src.polyColliderIslands)
		{
			return false;
		}
		if (this.polyColliderIslands != null && src.polyColliderIslands != null)
		{
			if (this.polyColliderIslands.Length != src.polyColliderIslands.Length)
			{
				return false;
			}
			for (int j = 0; j < this.polyColliderIslands.Length; j++)
			{
				if (!this.polyColliderIslands[j].CompareTo(src.polyColliderIslands[j]))
				{
					return false;
				}
			}
		}
		if (this.polyColliderCap != src.polyColliderCap)
		{
			return false;
		}
		if (this.colliderColor != src.colliderColor)
		{
			return false;
		}
		if (this.colliderSmoothSphereCollisions != src.colliderSmoothSphereCollisions)
		{
			return false;
		}
		if (this.colliderConvex != src.colliderConvex)
		{
			return false;
		}
		if (this.attachPoints.Count != src.attachPoints.Count)
		{
			return false;
		}
		for (int k = 0; k < this.attachPoints.Count; k++)
		{
			if (!this.attachPoints[k].CompareTo(src.attachPoints[k]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000A2B RID: 2603
	public string name = string.Empty;

	// Token: 0x04000A2C RID: 2604
	public bool disableTrimming;

	// Token: 0x04000A2D RID: 2605
	public bool additive;

	// Token: 0x04000A2E RID: 2606
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000A2F RID: 2607
	public Texture2D texture;

	// Token: 0x04000A30 RID: 2608
	[NonSerialized]
	public Texture2D thumbnailTexture;

	// Token: 0x04000A31 RID: 2609
	public int materialId;

	// Token: 0x04000A32 RID: 2610
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x04000A33 RID: 2611
	public float anchorX;

	// Token: 0x04000A34 RID: 2612
	public float anchorY;

	// Token: 0x04000A35 RID: 2613
	public UnityEngine.Object overrideMesh;

	// Token: 0x04000A36 RID: 2614
	public bool doubleSidedSprite;

	// Token: 0x04000A37 RID: 2615
	public bool customSpriteGeometry;

	// Token: 0x04000A38 RID: 2616
	public tk2dSpriteColliderIsland[] geometryIslands = new tk2dSpriteColliderIsland[0];

	// Token: 0x04000A39 RID: 2617
	public bool dice;

	// Token: 0x04000A3A RID: 2618
	public int diceUnitX = 64;

	// Token: 0x04000A3B RID: 2619
	public int diceUnitY = 64;

	// Token: 0x04000A3C RID: 2620
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04000A3D RID: 2621
	public int extraPadding;

	// Token: 0x04000A3E RID: 2622
	public tk2dSpriteCollectionDefinition.Source source;

	// Token: 0x04000A3F RID: 2623
	public bool fromSpriteSheet;

	// Token: 0x04000A40 RID: 2624
	public bool hasSpriteSheetId;

	// Token: 0x04000A41 RID: 2625
	public int spriteSheetId;

	// Token: 0x04000A42 RID: 2626
	public int spriteSheetX;

	// Token: 0x04000A43 RID: 2627
	public int spriteSheetY;

	// Token: 0x04000A44 RID: 2628
	public bool extractRegion;

	// Token: 0x04000A45 RID: 2629
	public int regionX;

	// Token: 0x04000A46 RID: 2630
	public int regionY;

	// Token: 0x04000A47 RID: 2631
	public int regionW;

	// Token: 0x04000A48 RID: 2632
	public int regionH;

	// Token: 0x04000A49 RID: 2633
	public int regionId;

	// Token: 0x04000A4A RID: 2634
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x04000A4B RID: 2635
	public Vector2 boxColliderMin;

	// Token: 0x04000A4C RID: 2636
	public Vector2 boxColliderMax;

	// Token: 0x04000A4D RID: 2637
	public tk2dSpriteColliderIsland[] polyColliderIslands;

	// Token: 0x04000A4E RID: 2638
	public tk2dSpriteCollectionDefinition.PolygonColliderCap polyColliderCap = tk2dSpriteCollectionDefinition.PolygonColliderCap.FrontAndBack;

	// Token: 0x04000A4F RID: 2639
	public bool colliderConvex;

	// Token: 0x04000A50 RID: 2640
	public bool colliderSmoothSphereCollisions;

	// Token: 0x04000A51 RID: 2641
	public tk2dSpriteCollectionDefinition.ColliderColor colliderColor;

	// Token: 0x04000A52 RID: 2642
	public List<tk2dSpriteDefinition.AttachPoint> attachPoints = new List<tk2dSpriteDefinition.AttachPoint>();

	// Token: 0x02000175 RID: 373
	public enum Anchor
	{
		// Token: 0x04000A54 RID: 2644
		UpperLeft,
		// Token: 0x04000A55 RID: 2645
		UpperCenter,
		// Token: 0x04000A56 RID: 2646
		UpperRight,
		// Token: 0x04000A57 RID: 2647
		MiddleLeft,
		// Token: 0x04000A58 RID: 2648
		MiddleCenter,
		// Token: 0x04000A59 RID: 2649
		MiddleRight,
		// Token: 0x04000A5A RID: 2650
		LowerLeft,
		// Token: 0x04000A5B RID: 2651
		LowerCenter,
		// Token: 0x04000A5C RID: 2652
		LowerRight,
		// Token: 0x04000A5D RID: 2653
		Custom
	}

	// Token: 0x02000176 RID: 374
	public enum Pad
	{
		// Token: 0x04000A5F RID: 2655
		Default,
		// Token: 0x04000A60 RID: 2656
		BlackZeroAlpha,
		// Token: 0x04000A61 RID: 2657
		Extend,
		// Token: 0x04000A62 RID: 2658
		TileXY
	}

	// Token: 0x02000177 RID: 375
	public enum ColliderType
	{
		// Token: 0x04000A64 RID: 2660
		UserDefined,
		// Token: 0x04000A65 RID: 2661
		ForceNone,
		// Token: 0x04000A66 RID: 2662
		BoxTrimmed,
		// Token: 0x04000A67 RID: 2663
		BoxCustom,
		// Token: 0x04000A68 RID: 2664
		Polygon
	}

	// Token: 0x02000178 RID: 376
	public enum PolygonColliderCap
	{
		// Token: 0x04000A6A RID: 2666
		None,
		// Token: 0x04000A6B RID: 2667
		FrontAndBack,
		// Token: 0x04000A6C RID: 2668
		Front,
		// Token: 0x04000A6D RID: 2669
		Back
	}

	// Token: 0x02000179 RID: 377
	public enum ColliderColor
	{
		// Token: 0x04000A6F RID: 2671
		Default,
		// Token: 0x04000A70 RID: 2672
		Red,
		// Token: 0x04000A71 RID: 2673
		White,
		// Token: 0x04000A72 RID: 2674
		Black
	}

	// Token: 0x0200017A RID: 378
	public enum Source
	{
		// Token: 0x04000A74 RID: 2676
		Sprite,
		// Token: 0x04000A75 RID: 2677
		SpriteSheet,
		// Token: 0x04000A76 RID: 2678
		Font
	}
}
