using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIMask")]
[ExecuteInEditMode]
public class tk2dUIMask : MonoBehaviour
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0000B5A8 File Offset: 0x000097A8
	private MeshFilter ThisMeshFilter
	{
		get
		{
			if (this._thisMeshFilter == null)
			{
				this._thisMeshFilter = base.GetComponent<MeshFilter>();
			}
			return this._thisMeshFilter;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0000B5CD File Offset: 0x000097CD
	private BoxCollider ThisBoxCollider
	{
		get
		{
			if (this._thisBoxCollider == null)
			{
				this._thisBoxCollider = base.GetComponent<BoxCollider>();
			}
			return this._thisBoxCollider;
		}
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0000B5F2 File Offset: 0x000097F2
	private void Awake()
	{
		this.Build();
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0000B5FA File Offset: 0x000097FA
	private void OnDestroy()
	{
		if (this.ThisMeshFilter.sharedMesh != null)
		{
			UnityEngine.Object.Destroy(this.ThisMeshFilter.sharedMesh);
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0004CF10 File Offset: 0x0004B110
	private Mesh FillMesh(Mesh mesh)
	{
		Vector3 zero = Vector3.zero;
		switch (this.anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			zero = new Vector3(0f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			zero = new Vector3(-this.size.x / 2f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			zero = new Vector3(-this.size.x, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			zero = new Vector3(0f, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			zero = new Vector3(-this.size.x / 2f, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero = new Vector3(-this.size.x, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			zero = new Vector3(0f, -this.size.y, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero = new Vector3(-this.size.x / 2f, -this.size.y, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			zero = new Vector3(-this.size.x, -this.size.y, 0f);
			break;
		}
		Vector3[] vertices = new Vector3[]
		{
			zero + new Vector3(0f, 0f, -this.depth),
			zero + new Vector3(this.size.x, 0f, -this.depth),
			zero + new Vector3(0f, this.size.y, -this.depth),
			zero + new Vector3(this.size.x, this.size.y, -this.depth)
		};
		mesh.vertices = vertices;
		mesh.uv = tk2dUIMask.uv;
		mesh.triangles = tk2dUIMask.indices;
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(zero, zero + new Vector3(this.size.x, this.size.y, 0f));
		mesh.bounds = bounds;
		return mesh;
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0004D1E0 File Offset: 0x0004B3E0
	private void OnDrawGizmosSelected()
	{
		Mesh sharedMesh = this.ThisMeshFilter.sharedMesh;
		if (sharedMesh != null)
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Bounds bounds = sharedMesh.bounds;
			Gizmos.color = new Color32(56, 146, 227, 96);
			float num = -this.depth * 1.001f;
			Vector3 center = new Vector3(bounds.center.x, bounds.center.y, num * 0.5f);
			Vector3 vector = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, Mathf.Abs(num));
			Gizmos.DrawCube(center, vector);
			Gizmos.color = new Color32(22, 145, byte.MaxValue, byte.MaxValue);
			Gizmos.DrawWireCube(center, vector);
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x0004D2E0 File Offset: 0x0004B4E0
	public void Build()
	{
		if (this.ThisMeshFilter.sharedMesh == null)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			this.ThisMeshFilter.mesh = this.FillMesh(mesh);
		}
		else
		{
			this.FillMesh(this.ThisMeshFilter.sharedMesh);
		}
		if (this.createBoxCollider)
		{
			if (this.ThisBoxCollider == null)
			{
				this._thisBoxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
			Bounds bounds = this.ThisMeshFilter.sharedMesh.bounds;
			this.ThisBoxCollider.center = new Vector3(bounds.center.x, bounds.center.y, -this.depth);
			this.ThisBoxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0.0002f);
		}
		else if (this.ThisBoxCollider != null)
		{
			UnityEngine.Object.Destroy(this.ThisBoxCollider);
		}
	}

	// Token: 0x04000C96 RID: 3222
	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	// Token: 0x04000C97 RID: 3223
	public Vector2 size = new Vector2(1f, 1f);

	// Token: 0x04000C98 RID: 3224
	public float depth = 1f;

	// Token: 0x04000C99 RID: 3225
	public bool createBoxCollider = true;

	// Token: 0x04000C9A RID: 3226
	private MeshFilter _thisMeshFilter;

	// Token: 0x04000C9B RID: 3227
	private BoxCollider _thisBoxCollider;

	// Token: 0x04000C9C RID: 3228
	private static readonly Vector2[] uv = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(1f, 0f),
		new Vector2(0f, 1f),
		new Vector2(1f, 1f)
	};

	// Token: 0x04000C9D RID: 3229
	private static readonly int[] indices = new int[]
	{
		0,
		3,
		1,
		2,
		3,
		0
	};
}
