using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
[Serializable]
public class tk2dCameraResolutionOverride
{
	// Token: 0x060007D6 RID: 2006 RVA: 0x00039B7C File Offset: 0x00037D7C
	public bool Match(int pixelWidth, int pixelHeight)
	{
		switch (this.matchBy)
		{
		case tk2dCameraResolutionOverride.MatchByType.Resolution:
			return pixelWidth == this.width && pixelHeight == this.height;
		case tk2dCameraResolutionOverride.MatchByType.AspectRatio:
		{
			float a = (float)pixelWidth * this.aspectRatioDenominator / this.aspectRatioNumerator;
			return Mathf.Approximately(a, (float)pixelHeight);
		}
		case tk2dCameraResolutionOverride.MatchByType.Wildcard:
			return true;
		default:
			return false;
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00039BE0 File Offset: 0x00037DE0
	public void Upgrade(int version)
	{
		if (version == 0)
		{
			this.matchBy = (((this.width != -1 || this.height != -1) && (this.width != 0 || this.height != 0)) ? tk2dCameraResolutionOverride.MatchByType.Resolution : tk2dCameraResolutionOverride.MatchByType.Wildcard);
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00039C30 File Offset: 0x00037E30
	public static tk2dCameraResolutionOverride DefaultOverride
	{
		get
		{
			return new tk2dCameraResolutionOverride
			{
				name = "Override",
				matchBy = tk2dCameraResolutionOverride.MatchByType.Wildcard,
				autoScaleMode = tk2dCameraResolutionOverride.AutoScaleMode.FitVisible,
				fitMode = tk2dCameraResolutionOverride.FitMode.Center
			};
		}
	}

	// Token: 0x040008F0 RID: 2288
	public string name;

	// Token: 0x040008F1 RID: 2289
	public tk2dCameraResolutionOverride.MatchByType matchBy;

	// Token: 0x040008F2 RID: 2290
	public int width;

	// Token: 0x040008F3 RID: 2291
	public int height;

	// Token: 0x040008F4 RID: 2292
	public float aspectRatioNumerator = 4f;

	// Token: 0x040008F5 RID: 2293
	public float aspectRatioDenominator = 3f;

	// Token: 0x040008F6 RID: 2294
	public float scale = 1f;

	// Token: 0x040008F7 RID: 2295
	public Vector2 offsetPixels = new Vector2(0f, 0f);

	// Token: 0x040008F8 RID: 2296
	public tk2dCameraResolutionOverride.AutoScaleMode autoScaleMode;

	// Token: 0x040008F9 RID: 2297
	public tk2dCameraResolutionOverride.FitMode fitMode;

	// Token: 0x0200014D RID: 333
	public enum MatchByType
	{
		// Token: 0x040008FB RID: 2299
		Resolution,
		// Token: 0x040008FC RID: 2300
		AspectRatio,
		// Token: 0x040008FD RID: 2301
		Wildcard
	}

	// Token: 0x0200014E RID: 334
	public enum AutoScaleMode
	{
		// Token: 0x040008FF RID: 2303
		None,
		// Token: 0x04000900 RID: 2304
		FitWidth,
		// Token: 0x04000901 RID: 2305
		FitHeight,
		// Token: 0x04000902 RID: 2306
		FitVisible,
		// Token: 0x04000903 RID: 2307
		StretchToFit,
		// Token: 0x04000904 RID: 2308
		ClosestMultipleOfTwo,
		// Token: 0x04000905 RID: 2309
		PixelPerfect
	}

	// Token: 0x0200014F RID: 335
	public enum FitMode
	{
		// Token: 0x04000907 RID: 2311
		Constant,
		// Token: 0x04000908 RID: 2312
		Center
	}
}
