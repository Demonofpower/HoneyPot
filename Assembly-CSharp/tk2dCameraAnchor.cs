using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
public class tk2dCameraAnchor : MonoBehaviour
{
	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0003977C File Offset: 0x0003797C
	// (set) Token: 0x060007C7 RID: 1991 RVA: 0x00007CF1 File Offset: 0x00005EF1
	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			if (this.anchor != -1)
			{
				if (this.anchor >= 0 && this.anchor <= 2)
				{
					this._anchorPoint = this.anchor + tk2dBaseSprite.Anchor.UpperLeft;
				}
				else if (this.anchor >= 6 && this.anchor <= 8)
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)(this.anchor - 6);
				}
				else
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)this.anchor;
				}
				this.anchor = -1;
			}
			return this._anchorPoint;
		}
		set
		{
			this._anchorPoint = value;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00007CFA File Offset: 0x00005EFA
	// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00007D02 File Offset: 0x00005F02
	public Vector2 AnchorOffsetPixels
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060007CA RID: 1994 RVA: 0x00007D0B File Offset: 0x00005F0B
	// (set) Token: 0x060007CB RID: 1995 RVA: 0x00007D13 File Offset: 0x00005F13
	public bool AnchorToNativeBounds
	{
		get
		{
			return this.anchorToNativeBounds;
		}
		set
		{
			this.anchorToNativeBounds = value;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x00007D1C File Offset: 0x00005F1C
	// (set) Token: 0x060007CD RID: 1997 RVA: 0x00007D4D File Offset: 0x00005F4D
	public Camera AnchorCamera
	{
		get
		{
			if (this.tk2dCamera != null)
			{
				this._anchorCamera = this.tk2dCamera.camera;
				this.tk2dCamera = null;
			}
			return this._anchorCamera;
		}
		set
		{
			this._anchorCamera = value;
			this._anchorCameraCached = null;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060007CE RID: 1998 RVA: 0x00007D5D File Offset: 0x00005F5D
	private tk2dCamera AnchorTk2dCamera
	{
		get
		{
			if (this._anchorCameraCached != this._anchorCamera)
			{
				this._anchorTk2dCamera = this._anchorCamera.GetComponent<tk2dCamera>();
				this._anchorCameraCached = this._anchorCamera;
			}
			return this._anchorTk2dCamera;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060007CF RID: 1999 RVA: 0x00007D98 File Offset: 0x00005F98
	private Transform myTransform
	{
		get
		{
			if (this._myTransform == null)
			{
				this._myTransform = base.transform;
			}
			return this._myTransform;
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00007DBD File Offset: 0x00005FBD
	private void Start()
	{
		this.UpdateTransform();
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00039804 File Offset: 0x00037A04
	private void UpdateTransform()
	{
		if (this.AnchorCamera == null)
		{
			return;
		}
		float num = 1f;
		Vector3 localPosition = this.myTransform.localPosition;
		this.tk2dCamera = ((!(this.AnchorTk2dCamera != null) || this.AnchorTk2dCamera.CameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective) ? null : this.AnchorTk2dCamera);
		Rect rect = default(Rect);
		if (this.tk2dCamera != null)
		{
			rect = ((!this.anchorToNativeBounds) ? this.tk2dCamera.ScreenExtents : this.tk2dCamera.NativeScreenExtents);
			num = this.tk2dCamera.GetSizeAtDistance(1f);
		}
		else
		{
			rect.Set(0f, 0f, this.AnchorCamera.pixelWidth, this.AnchorCamera.pixelHeight);
		}
		float yMin = rect.yMin;
		float yMax = rect.yMax;
		float y = (yMin + yMax) * 0.5f;
		float xMin = rect.xMin;
		float xMax = rect.xMax;
		float x = (xMin + xMax) * 0.5f;
		Vector3 zero = Vector3.zero;
		switch (this.AnchorPoint)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			zero = new Vector3(xMin, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			zero = new Vector3(x, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			zero = new Vector3(xMax, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			zero = new Vector3(xMin, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			zero = new Vector3(x, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero = new Vector3(xMax, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			zero = new Vector3(xMin, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero = new Vector3(x, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			zero = new Vector3(xMax, yMax, localPosition.z);
			break;
		}
		Vector3 vector = zero + new Vector3(num * this.offset.x, num * this.offset.y, 0f);
		if (this.tk2dCamera == null)
		{
			Vector3 vector2 = this.AnchorCamera.ScreenToWorldPoint(vector);
			if (this.myTransform.position != vector2)
			{
				this.myTransform.position = vector2;
			}
		}
		else
		{
			Vector3 localPosition2 = this.myTransform.localPosition;
			if (localPosition2 != vector)
			{
				this.myTransform.localPosition = vector;
			}
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00007DBD File Offset: 0x00005FBD
	public void ForceUpdateTransform()
	{
		this.UpdateTransform();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00007DBD File Offset: 0x00005FBD
	private void LateUpdate()
	{
		this.UpdateTransform();
	}

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private int anchor = -1;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchorPoint = tk2dBaseSprite.Anchor.UpperLeft;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private bool anchorToNativeBounds;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private Vector2 offset = Vector2.zero;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private tk2dCamera tk2dCamera;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private Camera _anchorCamera;

	// Token: 0x040008D3 RID: 2259
	private Camera _anchorCameraCached;

	// Token: 0x040008D4 RID: 2260
	private tk2dCamera _anchorTk2dCamera;

	// Token: 0x040008D5 RID: 2261
	private Transform _myTransform;
}
