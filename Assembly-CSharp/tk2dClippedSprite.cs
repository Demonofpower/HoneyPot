using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dClippedSprite")]
[RequireComponent(typeof(MeshFilter))]
public class tk2dClippedSprite : tk2dBaseSprite
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0003DF10 File Offset: 0x0003C110
	// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0003DF74 File Offset: 0x0003C174
	public Rect ClipRect
	{
		get
		{
			this._clipRect.Set(this._clipBottomLeft.x, this._clipBottomLeft.y, this._clipTopRight.x - this._clipBottomLeft.x, this._clipTopRight.y - this._clipBottomLeft.y);
			return this._clipRect;
		}
		set
		{
			Vector2 vector = new Vector2(value.x, value.y);
			this.clipBottomLeft = vector;
			vector.x += value.width;
			vector.y += value.height;
			this.clipTopRight = vector;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00008A2E File Offset: 0x00006C2E
	// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00008A36 File Offset: 0x00006C36
	public Vector2 clipBottomLeft
	{
		get
		{
			return this._clipBottomLeft;
		}
		set
		{
			if (value != this._clipBottomLeft)
			{
				this._clipBottomLeft = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00008A6E File Offset: 0x00006C6E
	// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00008A76 File Offset: 0x00006C76
	public Vector2 clipTopRight
	{
		get
		{
			return this._clipTopRight;
		}
		set
		{
			if (value != this._clipTopRight)
			{
				this._clipTopRight = new Vector2(value.x, value.y);
				this.Build();
				this.UpdateCollider();
			}
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00008AAE File Offset: 0x00006CAE
	// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00008AB6 File Offset: 0x00006CB6
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

	// Token: 0x060008C9 RID: 2249 RVA: 0x0003DFD0 File Offset: 0x0003C1D0
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
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00008AD1 File Offset: 0x00006CD1
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00008AEE File Offset: 0x00006CEE
	protected new void SetColors(Color32[] dest)
	{
		if (base.CurrentSprite.positions.Length == 4)
		{
			tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 4, this._color, this.collectionInst.premultipliedAlpha);
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003E04C File Offset: 0x0003C24C
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float colliderOffsetZ = (!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z;
		float colliderExtentZ = (!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f);
		tk2dSpriteGeomGen.SetClippedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this._clipBottomLeft, this._clipTopRight, colliderOffsetZ, colliderExtentZ);
		if (currentSprite.positions.Length != 4)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003E12C File Offset: 0x0003C32C
	public override void Build()
	{
		this.meshUvs = new Vector2[4];
		this.meshVertices = new Vector3[4];
		this.meshColors = new Color32[4];
		this.SetGeometry(this.meshVertices, this.meshUvs);
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
		int[] array = new int[6];
		tk2dSpriteGeomGen.SetClippedSpriteIndices(array, 0, 0, base.CurrentSprite);
		this.mesh.triangles = array;
		this.mesh.RecalculateBounds();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00008B1C File Offset: 0x00006D1C
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00008B24 File Offset: 0x00006D24
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00008B1C File Offset: 0x00006D1C
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003E230 File Offset: 0x0003C430
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

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003E280 File Offset: 0x0003C480
	protected void UpdateGeometryImpl()
	{
		if (this.meshVertices == null || this.meshVertices.Length == 0)
		{
			this.Build();
		}
		else
		{
			this.SetGeometry(this.meshVertices, this.meshUvs);
			this.mesh.vertices = this.meshVertices;
			this.mesh.uv = this.meshUvs;
			this.mesh.RecalculateBounds();
		}
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003E2F0 File Offset: 0x0003C4F0
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

	// Token: 0x060008D4 RID: 2260 RVA: 0x00008B2C File Offset: 0x00006D2C
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003E394 File Offset: 0x0003C594
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00008B34 File Offset: 0x00006D34
	protected override int GetCurrentVertexCount()
	{
		return 4;
	}

	// Token: 0x040009DA RID: 2522
	private Mesh mesh;

	// Token: 0x040009DB RID: 2523
	private Vector2[] meshUvs;

	// Token: 0x040009DC RID: 2524
	private Vector3[] meshVertices;

	// Token: 0x040009DD RID: 2525
	private Color32[] meshColors;

	// Token: 0x040009DE RID: 2526
	private int[] meshIndices;

	// Token: 0x040009DF RID: 2527
	public Vector2 _clipBottomLeft = new Vector2(0f, 0f);

	// Token: 0x040009E0 RID: 2528
	public Vector2 _clipTopRight = new Vector2(1f, 1f);

	// Token: 0x040009E1 RID: 2529
	private Rect _clipRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x040009E3 RID: 2531
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x040009E4 RID: 2532
	private Vector3 boundsExtents = Vector3.zero;
}
