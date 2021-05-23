using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
[Serializable]
public class tk2dCameraSettings
{
	// Token: 0x040008D6 RID: 2262
	public CameraClearFlags clearFlags = CameraClearFlags.Color;

	// Token: 0x040008D7 RID: 2263
	public Color backgroundColor = new Color32(49, 77, 121, byte.MaxValue);

	// Token: 0x040008D8 RID: 2264
	public LayerMask cullingMask = -1;

	// Token: 0x040008D9 RID: 2265
	public tk2dCameraSettings.ProjectionType projection;

	// Token: 0x040008DA RID: 2266
	public TransparencySortMode transparencySortMode = TransparencySortMode.Orthographic;

	// Token: 0x040008DB RID: 2267
	public float orthographicSize = 10f;

	// Token: 0x040008DC RID: 2268
	public float orthographicPixelsPerMeter = 20f;

	// Token: 0x040008DD RID: 2269
	public tk2dCameraSettings.OrthographicOrigin orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.Center;

	// Token: 0x040008DE RID: 2270
	public tk2dCameraSettings.OrthographicType orthographicType;

	// Token: 0x040008DF RID: 2271
	public float fieldOfView = 60f;

	// Token: 0x040008E0 RID: 2272
	public float nearClipPlane = 0.3f;

	// Token: 0x040008E1 RID: 2273
	public float farClipPlane = 50f;

	// Token: 0x040008E2 RID: 2274
	public Rect rect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x040008E3 RID: 2275
	public float depth;

	// Token: 0x040008E4 RID: 2276
	public RenderingPath renderingPath = RenderingPath.UsePlayerSettings;

	// Token: 0x040008E5 RID: 2277
	public RenderTexture targetTexture;

	// Token: 0x040008E6 RID: 2278
	public bool hdr;

	// Token: 0x02000149 RID: 329
	public enum ProjectionType
	{
		// Token: 0x040008E8 RID: 2280
		Orthographic,
		// Token: 0x040008E9 RID: 2281
		Perspective
	}

	// Token: 0x0200014A RID: 330
	public enum OrthographicType
	{
		// Token: 0x040008EB RID: 2283
		PixelsPerMeter,
		// Token: 0x040008EC RID: 2284
		OrthographicSize
	}

	// Token: 0x0200014B RID: 331
	public enum OrthographicOrigin
	{
		// Token: 0x040008EE RID: 2286
		BottomLeft,
		// Token: 0x040008EF RID: 2287
		Center
	}
}
