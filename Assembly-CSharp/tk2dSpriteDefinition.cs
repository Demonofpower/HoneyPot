using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
[Serializable]
public class tk2dSpriteDefinition
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x000095F3 File Offset: 0x000077F3
	public bool Valid
	{
		get
		{
			return this.name.Length != 0;
		}
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x000417E8 File Offset: 0x0003F9E8
	public Bounds GetBounds()
	{
		return new Bounds(new Vector3(this.boundsData[0].x, this.boundsData[0].y, this.boundsData[0].z), new Vector3(this.boundsData[1].x, this.boundsData[1].y, this.boundsData[1].z));
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0004186C File Offset: 0x0003FA6C
	public Bounds GetUntrimmedBounds()
	{
		return new Bounds(new Vector3(this.untrimmedBoundsData[0].x, this.untrimmedBoundsData[0].y, this.untrimmedBoundsData[0].z), new Vector3(this.untrimmedBoundsData[1].x, this.untrimmedBoundsData[1].y, this.untrimmedBoundsData[1].z));
	}

	// Token: 0x04000AE5 RID: 2789
	public string name;

	// Token: 0x04000AE6 RID: 2790
	public Vector3[] boundsData;

	// Token: 0x04000AE7 RID: 2791
	public Vector3[] untrimmedBoundsData;

	// Token: 0x04000AE8 RID: 2792
	public Vector2 texelSize;

	// Token: 0x04000AE9 RID: 2793
	public Vector3[] positions;

	// Token: 0x04000AEA RID: 2794
	public Vector3[] normals;

	// Token: 0x04000AEB RID: 2795
	public Vector4[] tangents;

	// Token: 0x04000AEC RID: 2796
	public Vector2[] uvs;

	// Token: 0x04000AED RID: 2797
	public int[] indices = new int[]
	{
		0,
		3,
		1,
		2,
		3,
		0
	};

	// Token: 0x04000AEE RID: 2798
	public Material material;

	// Token: 0x04000AEF RID: 2799
	[NonSerialized]
	public Material materialInst;

	// Token: 0x04000AF0 RID: 2800
	public int materialId;

	// Token: 0x04000AF1 RID: 2801
	public string sourceTextureGUID;

	// Token: 0x04000AF2 RID: 2802
	public bool extractRegion;

	// Token: 0x04000AF3 RID: 2803
	public int regionX;

	// Token: 0x04000AF4 RID: 2804
	public int regionY;

	// Token: 0x04000AF5 RID: 2805
	public int regionW;

	// Token: 0x04000AF6 RID: 2806
	public int regionH;

	// Token: 0x04000AF7 RID: 2807
	public tk2dSpriteDefinition.FlipMode flipped;

	// Token: 0x04000AF8 RID: 2808
	public bool complexGeometry;

	// Token: 0x04000AF9 RID: 2809
	public tk2dSpriteDefinition.ColliderType colliderType;

	// Token: 0x04000AFA RID: 2810
	public Vector3[] colliderVertices;

	// Token: 0x04000AFB RID: 2811
	public int[] colliderIndicesFwd;

	// Token: 0x04000AFC RID: 2812
	public int[] colliderIndicesBack;

	// Token: 0x04000AFD RID: 2813
	public bool colliderConvex;

	// Token: 0x04000AFE RID: 2814
	public bool colliderSmoothSphereCollisions;

	// Token: 0x04000AFF RID: 2815
	public tk2dSpriteDefinition.AttachPoint[] attachPoints = new tk2dSpriteDefinition.AttachPoint[0];

	// Token: 0x02000186 RID: 390
	public enum ColliderType
	{
		// Token: 0x04000B01 RID: 2817
		Unset,
		// Token: 0x04000B02 RID: 2818
		None,
		// Token: 0x04000B03 RID: 2819
		Box,
		// Token: 0x04000B04 RID: 2820
		Mesh
	}

	// Token: 0x02000187 RID: 391
	public enum FlipMode
	{
		// Token: 0x04000B06 RID: 2822
		None,
		// Token: 0x04000B07 RID: 2823
		Tk2d,
		// Token: 0x04000B08 RID: 2824
		TPackerCW
	}

	// Token: 0x02000188 RID: 392
	[Serializable]
	public class AttachPoint
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x00009624 File Offset: 0x00007824
		public void CopyFrom(tk2dSpriteDefinition.AttachPoint src)
		{
			this.name = src.name;
			this.position = src.position;
			this.angle = src.angle;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000964A File Offset: 0x0000784A
		public bool CompareTo(tk2dSpriteDefinition.AttachPoint src)
		{
			return this.name == src.name && src.position == this.position && src.angle == this.angle;
		}

		// Token: 0x04000B09 RID: 2825
		public string name = string.Empty;

		// Token: 0x04000B0A RID: 2826
		public Vector3 position = Vector3.zero;

		// Token: 0x04000B0B RID: 2827
		public float angle;
	}
}
