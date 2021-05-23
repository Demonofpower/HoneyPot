using System;

// Token: 0x0200018B RID: 395
[Serializable]
public class tk2dSpriteCollectionSize
{
	// Token: 0x06000996 RID: 2454 RVA: 0x00009794 File Offset: 0x00007994
	public static tk2dSpriteCollectionSize Explicit(float orthoSize, float targetHeight)
	{
		return tk2dSpriteCollectionSize.ForResolution(orthoSize, targetHeight, targetHeight);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00041DC8 File Offset: 0x0003FFC8
	public static tk2dSpriteCollectionSize PixelsPerMeter(float pixelsPerMeter)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = pixelsPerMeter
		};
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00041DEC File Offset: 0x0003FFEC
	public static tk2dSpriteCollectionSize ForResolution(float orthoSize, float width, float height)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.Explicit,
			orthoSize = orthoSize,
			width = width,
			height = height
		};
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00041E1C File Offset: 0x0004001C
	public static tk2dSpriteCollectionSize ForTk2dCamera()
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = 1f
		};
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00041E44 File Offset: 0x00040044
	public static tk2dSpriteCollectionSize ForTk2dCamera(tk2dCamera camera)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dCameraSettings cameraSettings = camera.SettingsRoot.CameraSettings;
		if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Orthographic)
		{
			tk2dCameraSettings.OrthographicType orthographicType = cameraSettings.orthographicType;
			if (orthographicType != tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				if (orthographicType == tk2dCameraSettings.OrthographicType.OrthographicSize)
				{
					tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.Explicit;
					tk2dSpriteCollectionSize.height = (float)camera.nativeResolutionHeight;
					tk2dSpriteCollectionSize.orthoSize = cameraSettings.orthographicSize;
				}
			}
			else
			{
				tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
				tk2dSpriteCollectionSize.pixelsPerMeter = cameraSettings.orthographicPixelsPerMeter;
			}
		}
		else if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			tk2dSpriteCollectionSize.pixelsPerMeter = 20f;
		}
		return tk2dSpriteCollectionSize;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0000979E File Offset: 0x0000799E
	public static tk2dSpriteCollectionSize Default()
	{
		return tk2dSpriteCollectionSize.PixelsPerMeter(20f);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x000097AA File Offset: 0x000079AA
	public void CopyFromLegacy(bool useTk2dCamera, float orthoSize, float targetHeight)
	{
		if (useTk2dCamera)
		{
			this.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			this.pixelsPerMeter = 1f;
		}
		else
		{
			this.type = tk2dSpriteCollectionSize.Type.Explicit;
			this.height = targetHeight;
			this.orthoSize = orthoSize;
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x000097DE File Offset: 0x000079DE
	public void CopyFrom(tk2dSpriteCollectionSize source)
	{
		this.type = source.type;
		this.width = source.width;
		this.height = source.height;
		this.orthoSize = source.orthoSize;
		this.pixelsPerMeter = source.pixelsPerMeter;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x00041EE8 File Offset: 0x000400E8
	public float OrthoSize
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.orthoSize;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.orthoSize;
			}
			return 0.5f;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x00041F24 File Offset: 0x00040124
	public float TargetHeight
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.height;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.height;
			}
			return this.pixelsPerMeter;
		}
	}

	// Token: 0x04000B26 RID: 2854
	public tk2dSpriteCollectionSize.Type type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;

	// Token: 0x04000B27 RID: 2855
	public float orthoSize = 10f;

	// Token: 0x04000B28 RID: 2856
	public float pixelsPerMeter = 20f;

	// Token: 0x04000B29 RID: 2857
	public float width = 960f;

	// Token: 0x04000B2A RID: 2858
	public float height = 640f;

	// Token: 0x0200018C RID: 396
	public enum Type
	{
		// Token: 0x04000B2C RID: 2860
		Explicit,
		// Token: 0x04000B2D RID: 2861
		PixelsPerMeter
	}
}
