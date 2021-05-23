using System;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000165 RID: 357
[AddComponentMenu("2D Toolkit/Backend/tk2dBaseSprite")]
public abstract class tk2dBaseSprite : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06000894 RID: 2196 RVA: 0x0000887D File Offset: 0x00006A7D
	// (remove) Token: 0x06000895 RID: 2197 RVA: 0x00008896 File Offset: 0x00006A96
	public event Action<tk2dBaseSprite> SpriteChanged;

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000896 RID: 2198 RVA: 0x000088AF File Offset: 0x00006AAF
	// (set) Token: 0x06000897 RID: 2199 RVA: 0x000088B7 File Offset: 0x00006AB7
	public tk2dSpriteCollectionData Collection
	{
		get
		{
			return this.collection;
		}
		set
		{
			this.collection = value;
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000088D1 File Offset: 0x00006AD1
	private void InitInstance()
	{
		if (this.collectionInst == null && this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x00008906 File Offset: 0x00006B06
	// (set) Token: 0x0600089A RID: 2202 RVA: 0x0000890E File Offset: 0x00006B0E
	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (value != this._color)
			{
				this._color = value;
				this.InitInstance();
				this.UpdateColors();
			}
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600089B RID: 2203 RVA: 0x00008934 File Offset: 0x00006B34
	// (set) Token: 0x0600089C RID: 2204 RVA: 0x0003D16C File Offset: 0x0003B36C
	public Vector3 scale
	{
		get
		{
			return this._scale;
		}
		set
		{
			if (value != this._scale)
			{
				this._scale = value;
				this.InitInstance();
				this.UpdateVertices();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600089D RID: 2205 RVA: 0x0000893C File Offset: 0x00006B3C
	// (set) Token: 0x0600089E RID: 2206 RVA: 0x0003D1BC File Offset: 0x0003B3BC
	public bool FlipX
	{
		get
		{
			return this._scale.x < 0f;
		}
		set
		{
			this.scale = new Vector3(Mathf.Abs(this._scale.x) * (float)((!value) ? 1 : -1), this._scale.y, this._scale.z);
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600089F RID: 2207 RVA: 0x00008950 File Offset: 0x00006B50
	// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0003D20C File Offset: 0x0003B40C
	public bool FlipY
	{
		get
		{
			return this._scale.y < 0f;
		}
		set
		{
			this.scale = new Vector3(this._scale.x, Mathf.Abs(this._scale.y) * (float)((!value) ? 1 : -1), this._scale.z);
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00008964 File Offset: 0x00006B64
	// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0003D25C File Offset: 0x0003B45C
	public int spriteId
	{
		get
		{
			return this._spriteId;
		}
		set
		{
			if (value != this._spriteId)
			{
				this.InitInstance();
				value = Mathf.Clamp(value, 0, this.collectionInst.spriteDefinitions.Length - 1);
				if (this._spriteId < 0 || this._spriteId >= this.collectionInst.spriteDefinitions.Length || this.GetCurrentVertexCount() != this.collectionInst.spriteDefinitions[value].positions.Length || this.collectionInst.spriteDefinitions[this._spriteId].complexGeometry != this.collectionInst.spriteDefinitions[value].complexGeometry)
				{
					this._spriteId = value;
					this.UpdateGeometry();
				}
				else
				{
					this._spriteId = value;
					this.UpdateVertices();
				}
				this.UpdateMaterial();
				this.UpdateCollider();
				if (this.SpriteChanged != null)
				{
					this.SpriteChanged(this);
				}
			}
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0000896C File Offset: 0x00006B6C
	public void SetSprite(int newSpriteId)
	{
		this.spriteId = newSpriteId;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003D348 File Offset: 0x0003B548
	public bool SetSprite(string spriteName)
	{
		int spriteIdByName = this.collection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(spriteIdByName);
		}
		else
		{
			Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0003D390 File Offset: 0x0003B590
	public void SetSprite(tk2dSpriteCollectionData newCollection, int newSpriteId)
	{
		bool flag = false;
		if (this.Collection != newCollection)
		{
			this.collection = newCollection;
			this.collectionInst = this.collection.inst;
			this._spriteId = -1;
			flag = true;
		}
		this.spriteId = newSpriteId;
		if (flag)
		{
			this.UpdateMaterial();
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003D3E4 File Offset: 0x0003B5E4
	public bool SetSprite(tk2dSpriteCollectionData newCollection, string spriteName)
	{
		int spriteIdByName = newCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			this.SetSprite(newCollection, spriteIdByName);
		}
		else
		{
			Debug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0003D428 File Offset: 0x0003B628
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this.Collection.version < 2)
			{
				Debug.LogError("Need to rebuild sprite collection.");
			}
			float distance = base.transform.position.z - tk2dCamera.transform.position.z;
			float num2 = this.Collection.invOrthoSize * this.Collection.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(distance) * num2;
		}
		else if (Camera.main)
		{
			if (Camera.main.isOrthoGraphic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float zdist = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, zdist);
			}
			num *= this.Collection.invOrthoSize;
		}
		else
		{
			Debug.LogError("Main camera not found.");
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x060008A8 RID: 2216
	protected abstract void UpdateMaterial();

	// Token: 0x060008A9 RID: 2217
	protected abstract void UpdateColors();

	// Token: 0x060008AA RID: 2218
	protected abstract void UpdateVertices();

	// Token: 0x060008AB RID: 2219
	protected abstract void UpdateGeometry();

	// Token: 0x060008AC RID: 2220
	protected abstract int GetCurrentVertexCount();

	// Token: 0x060008AD RID: 2221
	public abstract void Build();

	// Token: 0x060008AE RID: 2222 RVA: 0x00008975 File Offset: 0x00006B75
	public int GetSpriteIdByName(string name)
	{
		this.InitInstance();
		return this.collectionInst.GetSpriteIdByName(name);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0003D5A0 File Offset: 0x0003B7A0
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId) where T : tk2dBaseSprite
	{
		T t = go.AddComponent<T>();
		t._spriteId = -1;
		t.SetSprite(spriteCollection, spriteId);
		t.Build();
		return t;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0003D5E0 File Offset: 0x0003B7E0
	public static T AddComponent<T>(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName) where T : tk2dBaseSprite
	{
		int spriteIdByName = spriteCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName == -1)
		{
			Debug.LogError(string.Format("Unable to find sprite named {0} in sprite collection {1}", spriteName, spriteCollection.spriteCollectionName));
			return (T)((object)null);
		}
		return tk2dBaseSprite.AddComponent<T>(go, spriteCollection, spriteIdByName);
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00008989 File Offset: 0x00006B89
	protected int GetNumVertices()
	{
		this.InitInstance();
		return this.collectionInst.spriteDefinitions[this.spriteId].positions.Length;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000089AA File Offset: 0x00006BAA
	protected int GetNumIndices()
	{
		this.InitInstance();
		return this.collectionInst.spriteDefinitions[this.spriteId].indices.Length;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0003D624 File Offset: 0x0003B824
	protected void SetPositions(Vector3[] positions, Vector3[] normals, Vector4[] tangents)
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this.spriteId];
		int numVertices = this.GetNumVertices();
		for (int i = 0; i < numVertices; i++)
		{
			positions[i].x = tk2dSpriteDefinition.positions[i].x * this._scale.x;
			positions[i].y = tk2dSpriteDefinition.positions[i].y * this._scale.y;
			positions[i].z = tk2dSpriteDefinition.positions[i].z * this._scale.z;
		}
		if (normals.Length > 0)
		{
			for (int j = 0; j < numVertices; j++)
			{
				normals[j] = tk2dSpriteDefinition.normals[j];
			}
		}
		if (tangents.Length > 0)
		{
			for (int k = 0; k < numVertices; k++)
			{
				tangents[k] = tk2dSpriteDefinition.tangents[k];
			}
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0003D750 File Offset: 0x0003B950
	protected void SetColors(Color32[] dest)
	{
		Color color = this._color;
		if (this.collectionInst.premultipliedAlpha)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
		}
		Color32 color2 = color;
		int numVertices = this.GetNumVertices();
		for (int i = 0; i < numVertices; i++)
		{
			dest[i] = color2;
		}
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0003D7E0 File Offset: 0x0003B9E0
	public Bounds GetBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.boundsData[0].x * this._scale.x, tk2dSpriteDefinition.boundsData[0].y * this._scale.y, tk2dSpriteDefinition.boundsData[0].z * this._scale.z), new Vector3(tk2dSpriteDefinition.boundsData[1].x * this._scale.x, tk2dSpriteDefinition.boundsData[1].y * this._scale.y, tk2dSpriteDefinition.boundsData[1].z * this._scale.z));
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0003D8C4 File Offset: 0x0003BAC4
	public Bounds GetUntrimmedBounds()
	{
		this.InitInstance();
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		return new Bounds(new Vector3(tk2dSpriteDefinition.untrimmedBoundsData[0].x * this._scale.x, tk2dSpriteDefinition.untrimmedBoundsData[0].y * this._scale.y, tk2dSpriteDefinition.untrimmedBoundsData[0].z * this._scale.z), new Vector3(tk2dSpriteDefinition.untrimmedBoundsData[1].x * this._scale.x, tk2dSpriteDefinition.untrimmedBoundsData[1].y * this._scale.y, tk2dSpriteDefinition.untrimmedBoundsData[1].z * this._scale.z));
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000089CB File Offset: 0x00006BCB
	public tk2dSpriteDefinition GetCurrentSpriteDef()
	{
		this.InitInstance();
		return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060008B8 RID: 2232 RVA: 0x000089CB File Offset: 0x00006BCB
	public tk2dSpriteDefinition CurrentSprite
	{
		get
		{
			this.InitInstance();
			return (!(this.collectionInst == null)) ? this.collectionInst.spriteDefinitions[this._spriteId] : null;
		}
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00004EF7 File Offset: 0x000030F7
	protected virtual bool NeedBoxCollider()
	{
		return false;
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0003D9A8 File Offset: 0x0003BBA8
	protected virtual void UpdateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box && this.boxCollider == null)
		{
			this.boxCollider = base.gameObject.GetComponent<BoxCollider>();
			if (this.boxCollider == null)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		if (this.boxCollider != null)
		{
			if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
			{
				this.boxCollider.center = new Vector3(tk2dSpriteDefinition.colliderVertices[0].x * this._scale.x, tk2dSpriteDefinition.colliderVertices[0].y * this._scale.y, tk2dSpriteDefinition.colliderVertices[0].z * this._scale.z);
				this.boxCollider.size = new Vector3(2f * tk2dSpriteDefinition.colliderVertices[1].x * this._scale.x, 2f * tk2dSpriteDefinition.colliderVertices[1].y * this._scale.y, 2f * tk2dSpriteDefinition.colliderVertices[1].z * this._scale.z);
			}
			else if (tk2dSpriteDefinition.colliderType != tk2dSpriteDefinition.ColliderType.Unset)
			{
				if (this.boxCollider != null)
				{
					this.boxCollider.center = new Vector3(0f, 0f, -100000f);
					this.boxCollider.size = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0003DB6C File Offset: 0x0003BD6C
	protected virtual void CreateCollider()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = this.collectionInst.spriteDefinitions[this._spriteId];
		if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Unset)
		{
			return;
		}
		if (base.collider != null)
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.meshCollider = base.GetComponent<MeshCollider>();
		}
		if ((this.NeedBoxCollider() || tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box) && this.meshCollider == null)
		{
			if (this.boxCollider == null)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh && this.boxCollider == null)
		{
			if (this.meshCollider == null)
			{
				this.meshCollider = base.gameObject.AddComponent<MeshCollider>();
			}
			if (this.meshColliderMesh == null)
			{
				this.meshColliderMesh = new Mesh();
			}
			this.meshColliderMesh.Clear();
			this.meshColliderPositions = new Vector3[tk2dSpriteDefinition.colliderVertices.Length];
			for (int i = 0; i < this.meshColliderPositions.Length; i++)
			{
				this.meshColliderPositions[i] = new Vector3(tk2dSpriteDefinition.colliderVertices[i].x * this._scale.x, tk2dSpriteDefinition.colliderVertices[i].y * this._scale.y, tk2dSpriteDefinition.colliderVertices[i].z * this._scale.z);
			}
			this.meshColliderMesh.vertices = this.meshColliderPositions;
			float num = this._scale.x * this._scale.y * this._scale.z;
			this.meshColliderMesh.triangles = ((num < 0f) ? tk2dSpriteDefinition.colliderIndicesBack : tk2dSpriteDefinition.colliderIndicesFwd);
			this.meshCollider.sharedMesh = this.meshColliderMesh;
			this.meshCollider.convex = tk2dSpriteDefinition.colliderConvex;
			this.meshCollider.smoothSphereCollisions = tk2dSpriteDefinition.colliderSmoothSphereCollisions;
			if (base.rigidbody)
			{
				base.rigidbody.centerOfMass = Vector3.zero;
			}
		}
		else if (tk2dSpriteDefinition.colliderType != tk2dSpriteDefinition.ColliderType.None && Application.isPlaying)
		{
			Debug.LogError("Invalid mesh collider on sprite, please remove and try again.");
		}
		this.UpdateCollider();
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x000089FC File Offset: 0x00006BFC
	protected void Awake()
	{
		if (this.collection != null)
		{
			this.collectionInst = this.collection.inst;
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00008A20 File Offset: 0x00006C20
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.Collection == spriteCollection;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0003DDEC File Offset: 0x0003BFEC
	public virtual void ForceBuild()
	{
		if (this.collection == null)
		{
			return;
		}
		this.collectionInst = this.collection.inst;
		if (this.spriteId < 0 || this.spriteId >= this.collectionInst.spriteDefinitions.Length)
		{
			this.spriteId = 0;
		}
		this.Build();
		if (this.SpriteChanged != null)
		{
			this.SpriteChanged(this);
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0003DE64 File Offset: 0x0003C064
	public static GameObject CreateFromTexture<T>(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor) where T : tk2dBaseSprite
	{
		tk2dSpriteCollectionData tk2dSpriteCollectionData = SpriteCollectionGenerator.CreateFromTexture(texture, size, region, anchor);
		if (tk2dSpriteCollectionData == null)
		{
			return null;
		}
		GameObject gameObject = new GameObject();
		tk2dBaseSprite.AddComponent<T>(gameObject, tk2dSpriteCollectionData, 0);
		return gameObject;
	}

	// Token: 0x040009C6 RID: 2502
	[SerializeField]
	private tk2dSpriteCollectionData collection;

	// Token: 0x040009C7 RID: 2503
	protected tk2dSpriteCollectionData collectionInst;

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	protected Color _color = Color.white;

	// Token: 0x040009C9 RID: 2505
	[SerializeField]
	protected Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x040009CA RID: 2506
	[SerializeField]
	protected int _spriteId;

	// Token: 0x040009CB RID: 2507
	public BoxCollider boxCollider;

	// Token: 0x040009CC RID: 2508
	public MeshCollider meshCollider;

	// Token: 0x040009CD RID: 2509
	public Vector3[] meshColliderPositions;

	// Token: 0x040009CE RID: 2510
	public Mesh meshColliderMesh;

	// Token: 0x02000166 RID: 358
	public enum Anchor
	{
		// Token: 0x040009D1 RID: 2513
		LowerLeft,
		// Token: 0x040009D2 RID: 2514
		LowerCenter,
		// Token: 0x040009D3 RID: 2515
		LowerRight,
		// Token: 0x040009D4 RID: 2516
		MiddleLeft,
		// Token: 0x040009D5 RID: 2517
		MiddleCenter,
		// Token: 0x040009D6 RID: 2518
		MiddleRight,
		// Token: 0x040009D7 RID: 2519
		UpperLeft,
		// Token: 0x040009D8 RID: 2520
		UpperCenter,
		// Token: 0x040009D9 RID: 2521
		UpperRight
	}
}
