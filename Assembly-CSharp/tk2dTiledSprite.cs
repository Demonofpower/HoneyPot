using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
[AddComponentMenu("2D Toolkit/Sprite/tk2dTiledSprite")]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dTiledSprite : tk2dBaseSprite
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00009B7A File Offset: 0x00007D7A
	// (set) Token: 0x060009E5 RID: 2533 RVA: 0x00009B82 File Offset: 0x00007D82
	public Vector2 dimensions
	{
		get
		{
			return this._dimensions;
		}
		set
		{
			if (value != this._dimensions)
			{
				this._dimensions = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00009BA8 File Offset: 0x00007DA8
	// (set) Token: 0x060009E7 RID: 2535 RVA: 0x00009BB0 File Offset: 0x00007DB0
	public tk2dBaseSprite.Anchor anchor
	{
		get
		{
			return this._anchor;
		}
		set
		{
			if (value != this._anchor)
			{
				this._anchor = value;
				this.UpdateVertices();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x00009BD1 File Offset: 0x00007DD1
	// (set) Token: 0x060009E9 RID: 2537 RVA: 0x00009BD9 File Offset: 0x00007DD9
	public bool CreateBoxCollider
	{
		get
		{
			return this._createBoxCollider;
		}
		set
		{
			if (this._createBoxCollider != value)
			{
				this._createBoxCollider = value;
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00045470 File Offset: 0x00043670
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
			if (this.boxCollider == null)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
			}
		}
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00009BF4 File Offset: 0x00007DF4
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00045508 File Offset: 0x00043708
	protected new void SetColors(Color32[] dest)
	{
		int numVertices;
		int num;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out numVertices, out num, base.CurrentSprite, this.dimensions);
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, numVertices, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00045544 File Offset: 0x00043744
	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		int num;
		int num2;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out num, out num2, currentSprite, this.dimensions);
		if (this.meshUvs == null || this.meshUvs.Length != num)
		{
			this.meshUvs = new Vector2[num];
			this.meshVertices = new Vector3[num];
			this.meshColors = new Color32[num];
		}
		if (this.meshIndices == null || this.meshIndices.Length != num2)
		{
			this.meshIndices = new int[num2];
		}
		float colliderOffsetZ = (!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z;
		float colliderExtentZ = (!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f);
		tk2dSpriteGeomGen.SetTiledSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, this.anchor, colliderOffsetZ, colliderExtentZ);
		tk2dSpriteGeomGen.SetTiledSpriteIndices(this.meshIndices, 0, 0, currentSprite, this.dimensions);
		this.SetColors(this.meshColors);
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			this.mesh.Clear();
		}
		this.mesh.vertices = this.meshVertices;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = this.meshUvs;
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00009C11 File Offset: 0x00007E11
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00009C19 File Offset: 0x00007E19
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00009C11 File Offset: 0x00007E11
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0004572C File Offset: 0x0004392C
	protected void UpdateColorsImpl()
	{
		if (this.meshColors == null || this.meshColors.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetColors(this.meshColors);
			this.mesh.colors32 = this.meshColors;
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00009C21 File Offset: 0x00007E21
	protected void UpdateGeometryImpl()
	{
		this.Build();
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0004577C File Offset: 0x0004397C
	protected override void UpdateCollider()
	{
		if (this.CreateBoxCollider)
		{
			if (this.boxCollider == null)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
				if (this.boxCollider == null)
				{
					this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
				}
			}
			this.boxCollider.size = 2f * this.boundsExtents;
			this.boxCollider.center = this.boundsCenter;
		}
		else if (this.boxCollider != null)
		{
			UnityEngine.Object.Destroy(this.boxCollider);
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00009C29 File Offset: 0x00007E29
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0003E394 File Offset: 0x0003C594
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00008CEB File Offset: 0x00006EEB
	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	// Token: 0x04000B66 RID: 2918
	private Mesh mesh;

	// Token: 0x04000B67 RID: 2919
	private Vector2[] meshUvs;

	// Token: 0x04000B68 RID: 2920
	private Vector3[] meshVertices;

	// Token: 0x04000B69 RID: 2921
	private Color32[] meshColors;

	// Token: 0x04000B6A RID: 2922
	private int[] meshIndices;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x04000B6E RID: 2926
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x04000B6F RID: 2927
	private Vector3 boundsExtents = Vector3.zero;
}
