using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000146 RID: 326
[AddComponentMenu("2D Toolkit/Camera/tk2dCamera")]
[ExecuteInEditMode]
public class tk2dCamera : MonoBehaviour
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00007BB3 File Offset: 0x00005DB3
	public tk2dCameraSettings CameraSettings
	{
		get
		{
			return this.cameraSettings;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0003867C File Offset: 0x0003687C
	public tk2dCameraResolutionOverride CurrentResolutionOverride
	{
		get
		{
			tk2dCamera settingsRoot = this.SettingsRoot;
			Camera screenCamera = this.ScreenCamera;
			float pixelWidth = screenCamera.pixelWidth;
			float pixelHeight = screenCamera.pixelHeight;
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride = null;
			if (tk2dCameraResolutionOverride == null || (tk2dCameraResolutionOverride != null && ((float)tk2dCameraResolutionOverride.width != pixelWidth || (float)tk2dCameraResolutionOverride.height != pixelHeight)))
			{
				tk2dCameraResolutionOverride = null;
				if (settingsRoot.resolutionOverride != null)
				{
					foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride2 in settingsRoot.resolutionOverride)
					{
						if (tk2dCameraResolutionOverride2.Match((int)pixelWidth, (int)pixelHeight))
						{
							tk2dCameraResolutionOverride = tk2dCameraResolutionOverride2;
							break;
						}
					}
				}
			}
			return tk2dCameraResolutionOverride;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00007BBB File Offset: 0x00005DBB
	// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00007BC3 File Offset: 0x00005DC3
	public tk2dCamera InheritConfig
	{
		get
		{
			return this.inheritSettings;
		}
		set
		{
			if (this.inheritSettings != value)
			{
				this.inheritSettings = value;
				this._settingsRoot = null;
			}
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00007BE4 File Offset: 0x00005DE4
	private Camera UnityCamera
	{
		get
		{
			if (this._unityCamera == null)
			{
				this._unityCamera = base.camera;
				if (this._unityCamera == null)
				{
					Debug.LogError("A unity camera must be attached to the tk2dCamera script");
				}
			}
			return this._unityCamera;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00007C24 File Offset: 0x00005E24
	public static tk2dCamera Instance
	{
		get
		{
			return tk2dCamera.inst;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00038724 File Offset: 0x00036924
	public static tk2dCamera CameraForLayer(int layer)
	{
		int num = 1 << layer;
		foreach (tk2dCamera tk2dCamera in tk2dCamera.allCameras)
		{
			if ((tk2dCamera.cameraSettings.cullingMask & num) == num)
			{
				return tk2dCamera;
			}
		}
		return null;
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x00007C2B File Offset: 0x00005E2B
	public Rect ScreenExtents
	{
		get
		{
			return this._screenExtents;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x00007C33 File Offset: 0x00005E33
	public Rect NativeScreenExtents
	{
		get
		{
			return this._nativeScreenExtents;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x00007C3B File Offset: 0x00005E3B
	public Vector2 TargetResolution
	{
		get
		{
			return this._targetResolution;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x00007C43 File Offset: 0x00005E43
	public Vector2 NativeResolution
	{
		get
		{
			return new Vector2((float)this.nativeResolutionWidth, (float)this.nativeResolutionHeight);
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x000387A0 File Offset: 0x000369A0
	[Obsolete]
	public Vector2 ScreenOffset
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMin - this.NativeScreenExtents.xMin, this.ScreenExtents.yMin - this.NativeScreenExtents.yMin);
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000387EC File Offset: 0x000369EC
	[Obsolete]
	public Vector2 resolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000387EC File Offset: 0x000369EC
	[Obsolete]
	public Vector2 ScreenResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0003881C File Offset: 0x00036A1C
	[Obsolete]
	public Vector2 ScaledResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.width, this.ScreenExtents.height);
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00007C58 File Offset: 0x00005E58
	// (set) Token: 0x060007B4 RID: 1972 RVA: 0x00007C60 File Offset: 0x00005E60
	public float ZoomFactor
	{
		get
		{
			return this.zoomFactor;
		}
		set
		{
			this.zoomFactor = Mathf.Max(0.01f, value);
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00007C73 File Offset: 0x00005E73
	// (set) Token: 0x060007B6 RID: 1974 RVA: 0x00007C8B File Offset: 0x00005E8B
	[Obsolete]
	public float zoomScale
	{
		get
		{
			return 1f / Mathf.Max(0.001f, this.zoomFactor);
		}
		set
		{
			this.ZoomFactor = 1f / Mathf.Max(0.001f, value);
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0003884C File Offset: 0x00036A4C
	public Camera ScreenCamera
	{
		get
		{
			bool flag = this.viewportClippingEnabled && this.inheritSettings != null && this.inheritSettings.UnityCamera.rect == this.unitRect;
			return (!flag) ? this.UnityCamera : this.inheritSettings.UnityCamera;
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00007CA4 File Offset: 0x00005EA4
	private void Awake()
	{
		this.Upgrade();
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000388B0 File Offset: 0x00036AB0
	private void OnEnable()
	{
		if (this.UnityCamera != null)
		{
			this.UpdateCameraMatrix();
		}
		else
		{
			base.camera.enabled = false;
		}
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00038914 File Offset: 0x00036B14
	private void OnDestroy()
	{
		int num = tk2dCamera.allCameras.IndexOf(this);
		if (num != -1)
		{
			tk2dCamera.allCameras.RemoveAt(num);
		}
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00007CC8 File Offset: 0x00005EC8
	private void OnPreCull()
	{
		this.UpdateCameraMatrix();
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00038940 File Offset: 0x00036B40
	public float GetSizeAtDistance(float distance)
	{
		tk2dCameraSettings tk2dCameraSettings = this.SettingsRoot.CameraSettings;
		tk2dCameraSettings.ProjectionType projection = tk2dCameraSettings.projection;
		if (projection != tk2dCameraSettings.ProjectionType.Orthographic)
		{
			if (projection != tk2dCameraSettings.ProjectionType.Perspective)
			{
				return 1f;
			}
			return Mathf.Tan(this.CameraSettings.fieldOfView * 0.017453292f * 0.5f) * distance * 2f / (float)this.SettingsRoot.nativeResolutionHeight;
		}
		else
		{
			if (tk2dCameraSettings.orthographicType == tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				return 1f / tk2dCameraSettings.orthographicPixelsPerMeter;
			}
			return 2f * tk2dCameraSettings.orthographicSize / (float)this.SettingsRoot.nativeResolutionHeight;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x000389DC File Offset: 0x00036BDC
	public tk2dCamera SettingsRoot
	{
		get
		{
			if (this._settingsRoot == null)
			{
				this._settingsRoot = ((!(this.inheritSettings == null) && !(this.inheritSettings == this)) ? this.inheritSettings.SettingsRoot : this);
			}
			return this._settingsRoot;
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00038A3C File Offset: 0x00036C3C
	public Matrix4x4 OrthoOffCenter(Vector2 scale, float left, float right, float bottom, float top, float near, float far)
	{
		float value = 2f / (right - left) * scale.x;
		float value2 = 2f / (top - bottom) * scale.y;
		float value3 = -2f / (far - near);
		float value4 = -(right + left) / (right - left);
		float value5 = -(bottom + top) / (top - bottom);
		float value6 = -(far + near) / (far - near);
		Matrix4x4 result = default(Matrix4x4);
		result[0, 0] = value;
		result[0, 1] = 0f;
		result[0, 2] = 0f;
		result[0, 3] = value4;
		result[1, 0] = 0f;
		result[1, 1] = value2;
		result[1, 2] = 0f;
		result[1, 3] = value5;
		result[2, 0] = 0f;
		result[2, 1] = 0f;
		result[2, 2] = value3;
		result[2, 3] = value6;
		result[3, 0] = 0f;
		result[3, 1] = 0f;
		result[3, 2] = 0f;
		result[3, 3] = 1f;
		return result;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00038B74 File Offset: 0x00036D74
	private Vector2 GetScaleForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float width, float height)
	{
		Vector2 one = Vector2.one;
		if (currentOverride == null)
		{
			return one;
		}
		float num;
		switch (currentOverride.autoScaleMode)
		{
		case tk2dCameraResolutionOverride.AutoScaleMode.FitWidth:
			num = width / (float)settings.nativeResolutionWidth;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitHeight:
			num = height / (float)settings.nativeResolutionHeight;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitVisible:
		case tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo:
		{
			float num2 = (float)settings.nativeResolutionWidth / (float)settings.nativeResolutionHeight;
			float num3 = width / height;
			if (num3 < num2)
			{
				num = width / (float)settings.nativeResolutionWidth;
			}
			else
			{
				num = height / (float)settings.nativeResolutionHeight;
			}
			if (currentOverride.autoScaleMode == tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo)
			{
				if (num > 1f)
				{
					num = Mathf.Floor(num);
				}
				else
				{
					num = Mathf.Pow(2f, Mathf.Floor(Mathf.Log(num, 2f)));
				}
			}
			one.Set(num, num);
			return one;
		}
		case tk2dCameraResolutionOverride.AutoScaleMode.StretchToFit:
			one.Set(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.PixelPerfect:
			num = 1f;
			one.Set(num, num);
			return one;
		}
		num = currentOverride.scale;
		one.Set(num, num);
		return one;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00038CC4 File Offset: 0x00036EC4
	private Vector2 GetOffsetForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, Vector2 scale, float width, float height)
	{
		Vector2 result = Vector2.zero;
		if (currentOverride == null)
		{
			return result;
		}
		tk2dCameraResolutionOverride.FitMode fitMode = currentOverride.fitMode;
		if (fitMode != tk2dCameraResolutionOverride.FitMode.Constant)
		{
			if (fitMode == tk2dCameraResolutionOverride.FitMode.Center)
			{
				if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.BottomLeft)
				{
					result = new Vector2(Mathf.Round(((float)settings.nativeResolutionWidth * scale.x - width) / 2f), Mathf.Round(((float)settings.nativeResolutionHeight * scale.y - height) / 2f));
				}
				return result;
			}
		}
		result = -currentOverride.offsetPixels;
		return result;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00038D60 File Offset: 0x00036F60
	private Matrix4x4 GetProjectionMatrixForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float pixelWidth, float pixelHeight, bool halfTexelOffset, out Rect screenExtents, out Rect unscaledScreenExtents)
	{
		Vector2 scaleForOverride = this.GetScaleForOverride(settings, currentOverride, pixelWidth, pixelHeight);
		Vector2 offsetForOverride = this.GetOffsetForOverride(settings, currentOverride, scaleForOverride, pixelWidth, pixelHeight);
		float num = offsetForOverride.x;
		float num2 = offsetForOverride.y;
		float num3 = pixelWidth + offsetForOverride.x;
		float num4 = pixelHeight + offsetForOverride.y;
		Vector2 zero = Vector2.zero;
		if (this.viewportClippingEnabled && this.InheritConfig != null)
		{
			float num5 = (num3 - num) / scaleForOverride.x;
			float num6 = (num4 - num2) / scaleForOverride.y;
			Vector4 vector = new Vector4((float)((int)this.viewportRegion.x), (float)((int)this.viewportRegion.y), (float)((int)this.viewportRegion.z), (float)((int)this.viewportRegion.w));
			float num7 = -offsetForOverride.x / pixelWidth + vector.x / num5;
			float num8 = -offsetForOverride.y / pixelHeight + vector.y / num6;
			float num9 = vector.z / num5;
			float num10 = vector.w / num6;
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				num7 += (pixelWidth - (float)settings.nativeResolutionWidth * scaleForOverride.x) / pixelWidth / 2f;
				num8 += (pixelHeight - (float)settings.nativeResolutionHeight * scaleForOverride.y) / pixelHeight / 2f;
			}
			Rect rect = new Rect(num7, num8, num9, num10);
			if (this.UnityCamera.rect.x != num7 || this.UnityCamera.rect.y != num8 || this.UnityCamera.rect.width != num9 || this.UnityCamera.rect.height != num10)
			{
				this.UnityCamera.rect = rect;
			}
			float num11 = Mathf.Min(1f - rect.x, rect.width);
			float num12 = Mathf.Min(1f - rect.y, rect.height);
			float num13 = vector.x * scaleForOverride.x - offsetForOverride.x;
			float num14 = vector.y * scaleForOverride.y - offsetForOverride.y;
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				num13 -= (float)settings.nativeResolutionWidth * 0.5f * scaleForOverride.x;
				num14 -= (float)settings.nativeResolutionHeight * 0.5f * scaleForOverride.y;
			}
			if (rect.x < 0f)
			{
				num13 += -rect.x * pixelWidth;
				num11 = rect.x + rect.width;
			}
			if (rect.y < 0f)
			{
				num14 += -rect.y * pixelHeight;
				num12 = rect.y + rect.height;
			}
			num += num13;
			num2 += num14;
			num3 = pixelWidth * num11 + offsetForOverride.x + num13;
			num4 = pixelHeight * num12 + offsetForOverride.y + num14;
		}
		else
		{
			if (this.UnityCamera.rect != this.CameraSettings.rect)
			{
				this.UnityCamera.rect = this.CameraSettings.rect;
			}
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				float num15 = (num3 - num) * 0.5f;
				num -= num15;
				num3 -= num15;
				float num16 = (num4 - num2) * 0.5f;
				num4 -= num16;
				num2 -= num16;
				zero.Set((float)(-(float)this.nativeResolutionWidth) / 2f, (float)(-(float)this.nativeResolutionHeight) / 2f);
			}
		}
		float num17 = 1f / this.ZoomFactor;
		bool flag = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		float num18 = (!halfTexelOffset || !flag) ? 0f : 0.5f;
		float num19 = settings.cameraSettings.orthographicSize;
		tk2dCameraSettings.OrthographicType orthographicType = settings.cameraSettings.orthographicType;
		if (orthographicType != tk2dCameraSettings.OrthographicType.PixelsPerMeter)
		{
			if (orthographicType == tk2dCameraSettings.OrthographicType.OrthographicSize)
			{
				num19 = 2f * settings.cameraSettings.orthographicSize / (float)settings.nativeResolutionHeight;
			}
		}
		else
		{
			num19 = 1f / settings.cameraSettings.orthographicPixelsPerMeter;
		}
		float num20 = num19 * num17;
		screenExtents = new Rect(num * num20 / scaleForOverride.x, num2 * num20 / scaleForOverride.y, (num3 - num) * num20 / scaleForOverride.x, (num4 - num2) * num20 / scaleForOverride.y);
		unscaledScreenExtents = new Rect(zero.x * num20, zero.y * num20, (float)this.nativeResolutionWidth * num20, (float)this.nativeResolutionHeight * num20);
		return this.OrthoOffCenter(scaleForOverride, num19 * (num + num18) * num17, num19 * (num3 + num18) * num17, num19 * (num2 - num18) * num17, num19 * (num4 - num18) * num17, this.cameraSettings.nearClipPlane, this.cameraSettings.farClipPlane);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00039294 File Offset: 0x00037494
	private Vector2 GetScreenPixelDimensions(tk2dCamera settings)
	{
		Vector2 result = new Vector2(this.ScreenCamera.pixelWidth, this.ScreenCamera.pixelHeight);
		return result;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000392C0 File Offset: 0x000374C0
	private void Upgrade()
	{
		if (this.version != tk2dCamera.CURRENT_VERSION)
		{
			if (this.version == 0)
			{
				this.cameraSettings.orthographicPixelsPerMeter = 1f;
				this.cameraSettings.orthographicType = tk2dCameraSettings.OrthographicType.PixelsPerMeter;
				this.cameraSettings.orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.BottomLeft;
				this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Orthographic;
				foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride in this.resolutionOverride)
				{
					tk2dCameraResolutionOverride.Upgrade(this.version);
				}
				Camera camera = base.camera;
				if (camera != null)
				{
					this.cameraSettings.clearFlags = camera.clearFlags;
					this.cameraSettings.backgroundColor = camera.backgroundColor;
					this.cameraSettings.cullingMask = camera.cullingMask;
					this.cameraSettings.farClipPlane = camera.farClipPlane;
					this.cameraSettings.nearClipPlane = camera.nearClipPlane;
					this.cameraSettings.rect = camera.rect;
					this.cameraSettings.depth = camera.depth;
					this.cameraSettings.renderingPath = camera.renderingPath;
					this.cameraSettings.targetTexture = camera.targetTexture;
					this.cameraSettings.hdr = camera.hdr;
					if (!camera.isOrthoGraphic)
					{
						this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Perspective;
						this.cameraSettings.fieldOfView = camera.fieldOfView * this.ZoomFactor;
						this.cameraSettings.transparencySortMode = camera.transparencySortMode;
					}
					camera.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector);
				}
			}
			Debug.Log("tk2dCamera '" + base.name + "' - Upgraded from version " + this.version.ToString());
			this.version = tk2dCamera.CURRENT_VERSION;
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00039480 File Offset: 0x00037680
	public void UpdateCameraMatrix()
	{
		this.Upgrade();
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		Camera unityCamera = this.UnityCamera;
		tk2dCamera settingsRoot = this.SettingsRoot;
		tk2dCameraSettings tk2dCameraSettings = settingsRoot.CameraSettings;
		if (unityCamera.clearFlags != this.cameraSettings.clearFlags)
		{
			unityCamera.clearFlags = this.cameraSettings.clearFlags;
		}
		if (unityCamera.backgroundColor != this.cameraSettings.backgroundColor)
		{
			unityCamera.backgroundColor = this.cameraSettings.backgroundColor;
		}
		if (unityCamera.cullingMask != this.cameraSettings.cullingMask)
		{
			unityCamera.cullingMask = this.cameraSettings.cullingMask;
		}
		if (unityCamera.farClipPlane != this.cameraSettings.farClipPlane)
		{
			unityCamera.farClipPlane = this.cameraSettings.farClipPlane;
		}
		if (unityCamera.nearClipPlane != this.cameraSettings.nearClipPlane)
		{
			unityCamera.nearClipPlane = this.cameraSettings.nearClipPlane;
		}
		if (unityCamera.rect != this.cameraSettings.rect)
		{
			unityCamera.rect = this.cameraSettings.rect;
		}
		if (unityCamera.depth != this.cameraSettings.depth)
		{
			unityCamera.depth = this.cameraSettings.depth;
		}
		if (unityCamera.renderingPath != this.cameraSettings.renderingPath)
		{
			unityCamera.renderingPath = this.cameraSettings.renderingPath;
		}
		unityCamera.targetTexture = this.cameraSettings.targetTexture;
		if (unityCamera.hdr != this.cameraSettings.hdr)
		{
			unityCamera.hdr = this.cameraSettings.hdr;
		}
		this._targetResolution = this.GetScreenPixelDimensions(settingsRoot);
		if (tk2dCameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			unityCamera.orthographic = false;
			unityCamera.fieldOfView = tk2dCameraSettings.fieldOfView * this.ZoomFactor;
			unityCamera.transparencySortMode = tk2dCameraSettings.transparencySortMode;
			this._screenExtents.Set(-unityCamera.aspect, -1f, unityCamera.aspect * 2f, 2f);
			this._nativeScreenExtents = this._screenExtents;
			unityCamera.ResetProjectionMatrix();
		}
		else
		{
			unityCamera.transparencySortMode = TransparencySortMode.Orthographic;
			unityCamera.orthographic = true;
			Matrix4x4 matrix4x = this.GetProjectionMatrixForOverride(settingsRoot, settingsRoot.CurrentResolutionOverride, this._targetResolution.x, this._targetResolution.y, true, out this._screenExtents, out this._nativeScreenExtents);
			if (Application.platform == RuntimePlatform.WP8Player && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
			{
				float z = (Screen.orientation != ScreenOrientation.LandscapeRight) ? -90f : 90f;
				Matrix4x4 lhs = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, z), Vector3.one);
				matrix4x = lhs * matrix4x;
			}
			if (unityCamera.projectionMatrix != matrix4x)
			{
				unityCamera.projectionMatrix = matrix4x;
			}
		}
	}

	// Token: 0x040008B9 RID: 2233
	private static int CURRENT_VERSION = 1;

	// Token: 0x040008BA RID: 2234
	public int version;

	// Token: 0x040008BB RID: 2235
	[SerializeField]
	private tk2dCameraSettings cameraSettings = new tk2dCameraSettings();

	// Token: 0x040008BC RID: 2236
	public tk2dCameraResolutionOverride[] resolutionOverride = new tk2dCameraResolutionOverride[]
	{
		tk2dCameraResolutionOverride.DefaultOverride
	};

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	private tk2dCamera inheritSettings;

	// Token: 0x040008BE RID: 2238
	public int nativeResolutionWidth = 960;

	// Token: 0x040008BF RID: 2239
	public int nativeResolutionHeight = 640;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	private Camera _unityCamera;

	// Token: 0x040008C1 RID: 2241
	private static tk2dCamera inst;

	// Token: 0x040008C2 RID: 2242
	private static List<tk2dCamera> allCameras = new List<tk2dCamera>();

	// Token: 0x040008C3 RID: 2243
	public bool viewportClippingEnabled;

	// Token: 0x040008C4 RID: 2244
	public Vector4 viewportRegion = new Vector4(0f, 0f, 100f, 100f);

	// Token: 0x040008C5 RID: 2245
	private Vector2 _targetResolution = Vector2.zero;

	// Token: 0x040008C6 RID: 2246
	[SerializeField]
	private float zoomFactor = 1f;

	// Token: 0x040008C7 RID: 2247
	[HideInInspector]
	public bool forceResolutionInEditor;

	// Token: 0x040008C8 RID: 2248
	[HideInInspector]
	public Vector2 forceResolution = new Vector2(960f, 640f);

	// Token: 0x040008C9 RID: 2249
	private Rect _screenExtents;

	// Token: 0x040008CA RID: 2250
	private Rect _nativeScreenExtents;

	// Token: 0x040008CB RID: 2251
	private Rect unitRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x040008CC RID: 2252
	private tk2dCamera _settingsRoot;
}
