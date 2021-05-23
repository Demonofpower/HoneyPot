using System;
using UnityEngine;

// Token: 0x020001AF RID: 431
[AddComponentMenu("2D Toolkit/UI/tk2dUIProgressBar")]
public class tk2dUIProgressBar : MonoBehaviour
{
	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06000AB4 RID: 2740 RVA: 0x0000A70E File Offset: 0x0000890E
	// (remove) Token: 0x06000AB5 RID: 2741 RVA: 0x0000A727 File Offset: 0x00008927
	public event Action OnProgressComplete;

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00049488 File Offset: 0x00047688
	private void Start()
	{
		if (this.slicedSpriteBar != null)
		{
			tk2dSpriteDefinition currentSprite = this.slicedSpriteBar.CurrentSprite;
			Vector3 vector = currentSprite.boundsData[1];
			this.fullSlicedSpriteDimensions = this.slicedSpriteBar.dimensions;
			this.emptySlicedSpriteDimensions.Set((this.slicedSpriteBar.borderLeft + this.slicedSpriteBar.borderRight) * vector.x / currentSprite.texelSize.x, this.fullSlicedSpriteDimensions.y);
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0000A740 File Offset: 0x00008940
	// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x00049518 File Offset: 0x00047718
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (this.clippedSpriteBar != null)
			{
				this.clippedSpriteBar.clipTopRight = new Vector2(this.Value, 1f);
			}
			else if (this.scalableBar != null)
			{
				this.scalableBar.localScale = new Vector3(this.Value, this.scalableBar.localScale.y, this.scalableBar.localScale.z);
			}
			else if (this.slicedSpriteBar != null)
			{
				float new_x = Mathf.Lerp(this.emptySlicedSpriteDimensions.x, this.fullSlicedSpriteDimensions.x, this.Value);
				this.currentDimensions.Set(new_x, this.fullSlicedSpriteDimensions.y);
				this.slicedSpriteBar.dimensions = this.currentDimensions;
			}
			if (!this.isProgressComplete && this.Value == 1f)
			{
				this.isProgressComplete = true;
				if (this.OnProgressComplete != null)
				{
					this.OnProgressComplete();
				}
			}
			else if (this.isProgressComplete && this.Value < 1f)
			{
				this.isProgressComplete = false;
			}
		}
	}

	// Token: 0x04000BE6 RID: 3046
	public Transform scalableBar;

	// Token: 0x04000BE7 RID: 3047
	public tk2dClippedSprite clippedSpriteBar;

	// Token: 0x04000BE8 RID: 3048
	public tk2dSlicedSprite slicedSpriteBar;

	// Token: 0x04000BE9 RID: 3049
	private Vector2 emptySlicedSpriteDimensions = Vector2.zero;

	// Token: 0x04000BEA RID: 3050
	private Vector2 fullSlicedSpriteDimensions = Vector2.zero;

	// Token: 0x04000BEB RID: 3051
	private Vector2 currentDimensions = Vector2.zero;

	// Token: 0x04000BEC RID: 3052
	[SerializeField]
	private float percent;

	// Token: 0x04000BED RID: 3053
	private bool isProgressComplete;
}
