using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class tk2dSprite : tk2dBaseSprite
{
	// Token: 0x06000901 RID: 2305 RVA: 0x0003F814 File Offset: 0x0003DA14
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

	// Token: 0x06000902 RID: 2306 RVA: 0x00008CF7 File Offset: 0x00006EF7
	protected void OnDestroy()
	{
		if (this.mesh)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
		if (this.meshColliderMesh)
		{
			UnityEngine.Object.Destroy(this.meshColliderMesh);
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0003F890 File Offset: 0x0003DA90
	public override void Build()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		this.meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
		this.meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
		this.meshNormals = new Vector3[0];
		this.meshTangents = new Vector4[0];
		if (tk2dSpriteDefinition.normals != null && tk2dSpriteDefinition.normals.Length > 0)
		{
			this.meshNormals = new Vector3[tk2dSpriteDefinition.normals.Length];
		}
		if (tk2dSpriteDefinition.tangents != null && tk2dSpriteDefinition.tangents.Length > 0)
		{
			this.meshTangents = new Vector4[tk2dSpriteDefinition.tangents.Length];
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.mesh.hideFlags = HideFlags.DontSave;
			base.GetComponent<MeshFilter>().mesh = this.mesh;
		}
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.triangles = tk2dSpriteDefinition.indices;
		this.UpdateMaterial();
		this.CreateCollider();
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00008D2F File Offset: 0x00006F2F
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00008D39 File Offset: 0x00006F39
	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00008D43 File Offset: 0x00006F43
	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00008D4E File Offset: 0x00006F4E
	protected override void UpdateGeometry()
	{
		this.UpdateGeometryImpl();
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00008D56 File Offset: 0x00006F56
	protected override void UpdateColors()
	{
		this.UpdateColorsImpl();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00008D5E File Offset: 0x00006F5E
	protected override void UpdateVertices()
	{
		this.UpdateVerticesImpl();
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0003FA20 File Offset: 0x0003DC20
	protected void UpdateColorsImpl()
	{
		if (this.mesh == null || this.meshColors == null || this.meshColors.Length == 0)
		{
			return;
		}
		base.SetColors(this.meshColors);
		this.mesh.colors32 = this.meshColors;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003FA74 File Offset: 0x0003DC74
	protected void UpdateVerticesImpl()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.mesh == null || this.meshVertices == null || this.meshVertices.Length == 0)
		{
			return;
		}
		if (tk2dSpriteDefinition.normals.Length != this.meshNormals.Length)
		{
			this.meshNormals = ((tk2dSpriteDefinition.normals == null || tk2dSpriteDefinition.normals.Length <= 0) ? new Vector3[0] : new Vector3[tk2dSpriteDefinition.normals.Length]);
		}
		if (tk2dSpriteDefinition.tangents.Length != this.meshTangents.Length)
		{
			this.meshTangents = ((tk2dSpriteDefinition.tangents == null || tk2dSpriteDefinition.tangents.Length <= 0) ? new Vector4[0] : new Vector4[tk2dSpriteDefinition.tangents.Length]);
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.bounds = base.GetBounds();
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003FBC4 File Offset: 0x0003DDC4
	protected void UpdateGeometryImpl()
	{
		if (this.mesh == null)
		{
			return;
		}
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[base.spriteId];
		if (this.meshVertices == null || this.meshVertices.Length != tk2dSpriteDefinition.positions.Length)
		{
			this.meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
			this.meshNormals = ((tk2dSpriteDefinition.normals == null || tk2dSpriteDefinition.normals.Length <= 0) ? new Vector3[0] : new Vector3[tk2dSpriteDefinition.normals.Length]);
			this.meshTangents = ((tk2dSpriteDefinition.tangents == null || tk2dSpriteDefinition.tangents.Length <= 0) ? new Vector4[0] : new Vector4[tk2dSpriteDefinition.tangents.Length]);
			this.meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
		}
		base.SetPositions(this.meshVertices, this.meshNormals, this.meshTangents);
		base.SetColors(this.meshColors);
		this.mesh.Clear();
		this.mesh.vertices = this.meshVertices;
		this.mesh.normals = this.meshNormals;
		this.mesh.tangents = this.meshTangents;
		this.mesh.colors32 = this.meshColors;
		this.mesh.uv = tk2dSpriteDefinition.uvs;
		this.mesh.bounds = base.GetBounds();
		this.mesh.triangles = tk2dSpriteDefinition.indices;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003E394 File Offset: 0x0003C594
	protected override void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this.collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			base.renderer.material = this.collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00008D66 File Offset: 0x00006F66
	protected override int GetCurrentVertexCount()
	{
		if (this.meshVertices == null)
		{
			return 0;
		}
		return this.meshVertices.Length;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00008D7D File Offset: 0x00006F7D
	public override void ForceBuild()
	{
		base.ForceBuild();
		base.GetComponent<MeshFilter>().mesh = this.mesh;
	}

	// Token: 0x040009FC RID: 2556
	private Mesh mesh;

	// Token: 0x040009FD RID: 2557
	private Vector3[] meshVertices;

	// Token: 0x040009FE RID: 2558
	private Vector3[] meshNormals;

	// Token: 0x040009FF RID: 2559
	private Vector4[] meshTangents;

	// Token: 0x04000A00 RID: 2560
	private Color32[] meshColors;
}
