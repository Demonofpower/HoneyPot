using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
[AddComponentMenu("2D Toolkit/Deprecated/Extra/tk2dPixelPerfectHelper")]
public class tk2dPixelPerfectHelper : MonoBehaviour
{
	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0003E3F0 File Offset: 0x0003C5F0
	public static tk2dPixelPerfectHelper inst
	{
		get
		{
			if (tk2dPixelPerfectHelper._inst == null)
			{
				tk2dPixelPerfectHelper._inst = (UnityEngine.Object.FindObjectOfType(typeof(tk2dPixelPerfectHelper)) as tk2dPixelPerfectHelper);
				if (tk2dPixelPerfectHelper._inst == null)
				{
					return null;
				}
				tk2dPixelPerfectHelper.inst.Setup();
			}
			return tk2dPixelPerfectHelper._inst;
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00008B60 File Offset: 0x00006D60
	private void Awake()
	{
		this.Setup();
		tk2dPixelPerfectHelper._inst = this;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003E448 File Offset: 0x0003C648
	public virtual void Setup()
	{
		float num = (float)this.collectionTargetHeight / this.targetResolutionHeight;
		if (base.camera != null)
		{
			this.cam = base.camera;
		}
		if (this.cam == null)
		{
			this.cam = Camera.main;
		}
		if (this.cam.isOrthoGraphic)
		{
			this.scaleK = num * this.cam.orthographicSize / this.collectionOrthoSize;
			this.scaleD = 0f;
		}
		else
		{
			float num2 = num * Mathf.Tan(0.017453292f * this.cam.fieldOfView * 0.5f) / this.collectionOrthoSize;
			this.scaleK = num2 * -this.cam.transform.position.z;
			this.scaleD = num2;
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00008B6E File Offset: 0x00006D6E
	public static float CalculateScaleForPerspectiveCamera(float fov, float zdist)
	{
		return Mathf.Abs(Mathf.Tan(0.017453292f * fov * 0.5f) * zdist);
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x00008B89 File Offset: 0x00006D89
	public bool CameraIsOrtho
	{
		get
		{
			return this.cam.isOrthoGraphic;
		}
	}

	// Token: 0x040009E5 RID: 2533
	private static tk2dPixelPerfectHelper _inst;

	// Token: 0x040009E6 RID: 2534
	[NonSerialized]
	public Camera cam;

	// Token: 0x040009E7 RID: 2535
	public int collectionTargetHeight = 640;

	// Token: 0x040009E8 RID: 2536
	public float collectionOrthoSize = 1f;

	// Token: 0x040009E9 RID: 2537
	public float targetResolutionHeight = 640f;

	// Token: 0x040009EA RID: 2538
	[NonSerialized]
	public float scaleD;

	// Token: 0x040009EB RID: 2539
	[NonSerialized]
	public float scaleK;
}
