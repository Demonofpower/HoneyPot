using System;
using UnityEngine;

// Token: 0x0200016A RID: 362
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSlicedSprite")]
public class tk2dSlicedSprite : tk2dBaseSprite
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00008BD3 File Offset: 0x00006DD3
	// (set) Token: 0x060008E6 RID: 2278 RVA: 0x00008BDB File Offset: 0x00006DDB
	public bool BorderOnly
	{
		get
		{
			return this._borderOnly;
		}
		set
		{
			if (value != this._borderOnly)
			{
				this._borderOnly = value;
				this.UpdateIndices();
			}
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00008BF6 File Offset: 0x00006DF6
	// (set) Token: 0x060008E8 RID: 2280 RVA: 0x00008BFE File Offset: 0x00006DFE
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

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00008C24 File Offset: 0x00006E24
	// (set) Token: 0x060008EA RID: 2282 RVA: 0x00008C2C File Offset: 0x00006E2C
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

	// Token: 0x060008EB RID: 2283 RVA: 0x0003F058 File Offset: 0x0003D258
	public void SetBorder(float left, float bottom, float right, float top)
	{
		if (this.borderLeft != left || this.borderBottom != bottom || this.borderRight != right || this.borderTop != top)
		{
			this.borderLeft = left;
			this.borderBottom = bottom;
			this.borderRight = right;
			this.borderTop = top;
			this.UpdateVertices();
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x00008C4D File Offset: 0x00006E4D
	// (set) Token: 0x060008ED RID: 2285 RVA: 0x00008C55 File Offset: 0x00006E55
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

	// Token: 0x060008EE RID: 2286 RVA: 0x0003F0BC File Offset: 0x0003D2BC
	private new void Awake()
	{
		base.Awake();
		this.mesh = new Mesh();
		this.mesh.hideFlags = HideFlags.DontSave;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		if (this.boxCollider == null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
		}
		if (base.Collection)
		{
			if (this._spriteId < 0 || this._spriteId >= base.Collection.Count)
			{
				this._spriteId = 0;
			}
			this.Build();
		}
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00008C70 File Offset: 0x00006E70
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00008C8D File Offset: 0x00006E8D
	protected new void SetColors(Color32[] dest)
	{
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 16, this._color, this.collectionInst.premultipliedAlpha);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0003F154 File Offset: 0x0003D354
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float colliderOffsetZ = (!(this.boxCollider != null)) ? 0f : this.boxCollider.center.z;
		float colliderExtentZ = (!(this.boxCollider != null)) ? 0.5f : (this.boxCollider.size.z * 0.5f);
		tk2dSpriteGeomGen.SetSlicedSpriteGeom(this.meshVertices, this.meshUvs, 0, out this.boundsCenter, out this.boundsExtents, currentSprite, this._scale, this.dimensions, new Vector2(this.borderLeft, this.borderBottom), new Vector2(this.borderRight, this.borderTop), this.anchor, colliderOffsetZ, colliderExtentZ);
		if (currentSprite.positions.Length != 4)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0003F258 File Offset: 0x0003D458
	private void SetIndices()
	{
		int num = (!this._borderOnly) ? 54 : 48;
		this.meshIndices = new int[num];
		tk2dSpriteGeomGen.SetSlicedSpriteIndices(this.meshIndices, 0, 0, base.CurrentSprite, this._borderOnly);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
	private bool NearEnough(float value, float compValue, float scale)
	{
		float num = Mathf.Abs(value - compValue);
		return Mathf.Abs(num / scale) < 0.01f;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0003F2C8 File Offset: 0x0003D4C8
	private void PermanentUpgradeLegacyMode()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float x = currentSprite.untrimmedBoundsData[0].x;
		float y = currentSprite.untrimmedBoundsData[0].y;
		float x2 = currentSprite.untrimmedBoundsData[1].x;
		float y2 = currentSprite.untrimmedBoundsData[1].y;
		if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		else if (this.NearEnough(x, 0f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerCenter;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleRight;
		}
		else if (this.NearEnough(x, -x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerRight;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, -y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.UpperLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, 0f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.MiddleLeft;
		}
		else if (this.NearEnough(x, x2 / 2f, x2) && this.NearEnough(y, y2 / 2f, y2))
		{
			this._anchor = tk2dBaseSprite.Anchor.LowerLeft;
		}
		else
		{
			Debug.LogError("tk2dSlicedSprite (" + base.name + ") error - Unable to determine anchor upgrading from legacy mode. Please fix this manually.");
			this._anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		}
		float num = x2 / currentSprite.texelSize.x;
		float num2 = y2 / currentSprite.texelSize.y;
		this._dimensions.x = this._scale.x * num;
		this._dimensions.y = this._scale.y * num2;
		this._scale.Set(1f, 1f, 1f);
		this.legacyMode = false;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0003F598 File Offset: 0x0003D798
	public override void Build()
	{
		if (this.legacyMode)
		{
			this.PermanentUpgradeLegacyMode();
		}
		this.meshUvs = new Vector2[16];
		this.meshVertices = new Vector3[16];
		this.meshColors = new Color32[16];
		this.SetIndices();
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
		this.mesh.triangles = this.meshIndices;
		this.mesh.RecalculateBounds();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
		this.UpdateCollider();
		this.UpdateMaterial();
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00008CA9 File Offset: 0x00006EA9
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00008CB1 File Offset: 0x00006EB1
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00008CA9 File Offset: 0x00006EA9
	protected override void UpdateVertices()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00008CB9 File Offset: 0x00006EB9
	private void UpdateIndices()
	{
		if (this.mesh != null)
		{
			this.SetIndices();
			this.mesh.triangles = this.meshIndices;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
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

	// Token: 0x060008FB RID: 2299 RVA: 0x0003F6F8 File Offset: 0x0003D8F8
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
			this.UpdateCollider();
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0003F770 File Offset: 0x0003D970
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

	// Token: 0x060008FD RID: 2301 RVA: 0x00008CE3 File Offset: 0x00006EE3
	protected override void CreateCollider()
	{
		this.UpdateCollider();
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0003E394 File Offset: 0x0003C594
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00008CEB File Offset: 0x00006EEB
	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	// Token: 0x040009EC RID: 2540
	private Mesh mesh;

	// Token: 0x040009ED RID: 2541
	private Vector2[] meshUvs;

	// Token: 0x040009EE RID: 2542
	private Vector3[] meshVertices;

	// Token: 0x040009EF RID: 2543
	private Color32[] meshColors;

	// Token: 0x040009F0 RID: 2544
	private int[] meshIndices;

	// Token: 0x040009F1 RID: 2545
	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	// Token: 0x040009F2 RID: 2546
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchor;

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	private bool _borderOnly;

	// Token: 0x040009F4 RID: 2548
	[SerializeField]
	private bool legacyMode;

	// Token: 0x040009F5 RID: 2549
	public float borderTop = 0.2f;

	// Token: 0x040009F6 RID: 2550
	public float borderBottom = 0.2f;

	// Token: 0x040009F7 RID: 2551
	public float borderLeft = 0.2f;

	// Token: 0x040009F8 RID: 2552
	public float borderRight = 0.2f;

	// Token: 0x040009F9 RID: 2553
	[SerializeField]
	protected bool _createBoxCollider;

	// Token: 0x040009FA RID: 2554
	private Vector3 boundsCenter = Vector3.zero;

	// Token: 0x040009FB RID: 2555
	private Vector3 boundsExtents = Vector3.zero;
}
