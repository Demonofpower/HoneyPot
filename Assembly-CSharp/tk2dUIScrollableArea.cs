using System;
using UnityEngine;

// Token: 0x020001B0 RID: 432
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollableArea")]
public class tk2dUIScrollableArea : MonoBehaviour
{
	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06000ABB RID: 2747 RVA: 0x0000A748 File Offset: 0x00008948
	// (remove) Token: 0x06000ABC RID: 2748 RVA: 0x0000A761 File Offset: 0x00008961
	public event Action<tk2dUIScrollableArea> OnScroll;

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0000A77A File Offset: 0x0000897A
	// (set) Token: 0x06000ABE RID: 2750 RVA: 0x0000A782 File Offset: 0x00008982
	public float ContentLength
	{
		get
		{
			return this.contentLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, value, this.visibleAreaLength, this.visibleAreaLength);
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0000A79D File Offset: 0x0000899D
	// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x0000A7A5 File Offset: 0x000089A5
	public float VisibleAreaLength
	{
		get
		{
			return this.visibleAreaLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, this.contentLength, this.visibleAreaLength, value);
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0000A7C0 File Offset: 0x000089C0
	// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x000497FC File Offset: 0x000479FC
	public float Value
	{
		get
		{
			return Mathf.Clamp01(this.percent);
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			if (value != this.percent)
			{
				this.UnpressAllUIItemChildren();
				this.percent = value;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
			}
			if (this.scrollBar != null)
			{
				this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
			}
			this.SetContentPosition();
		}
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00049874 File Offset: 0x00047A74
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.UnpressAllUIItemChildren();
		if (this.scrollBar != null)
		{
			this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
		}
		this.SetContentPosition();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x000498C8 File Offset: 0x00047AC8
	public float MeasureContentLength()
	{
		Vector3 vector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Vector3 vector2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3[] array = new Vector3[]
		{
			vector2,
			vector
		};
		Transform transform = this.contentContainer.transform;
		tk2dUIScrollableArea.GetRendererBoundsInChildren(transform.worldToLocalMatrix, array, transform);
		if (array[0] != vector2 && array[1] != vector)
		{
			return array[1].y - array[0].y;
		}
		Debug.LogError("Unable to measure content length");
		return this.VisibleAreaLength * 0.9f;
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0004999C File Offset: 0x00047B9C
	private void OnEnable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll += this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown += this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease += this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver += this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut += this.BackgroundButtonHoverOut;
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00049A40 File Offset: 0x00047C40
	private void OnDisable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll -= this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown -= this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease -= this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver -= this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut -= this.BackgroundButtonHoverOut;
		}
		if (this.isBackgroundButtonOver)
		{
			if (tk2dUIManager.Instance != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = false;
		}
		if (this.isBackgroundButtonDown || this.isSwipeScrollingInProgress)
		{
			if (tk2dUIManager.Instance != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
			this.isSwipeScrollingInProgress = false;
		}
		this.swipeCurrVelocity = 0f;
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0000A7CD File Offset: 0x000089CD
	private void Start()
	{
		this.UpdateScrollbarActiveState();
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00049B70 File Offset: 0x00047D70
	private void BackgroundHoverOverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollUpFixed();
			}
			else
			{
				this.Value -= 0.1f;
			}
		}
		else if (mouseWheelChange < 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollDownFixed();
			}
			else
			{
				this.Value += 0.1f;
			}
		}
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0000A7D5 File Offset: 0x000089D5
	private void ScrollBarMove(tk2dUIScrollbar scrollBar)
	{
		this.Value = scrollBar.Value;
		this.isSwipeScrollingInProgress = false;
		if (this.isBackgroundButtonDown)
		{
			this.BackgroundButtonRelease();
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0000A7FB File Offset: 0x000089FB
	// (set) Token: 0x06000ACB RID: 2763 RVA: 0x0000A826 File Offset: 0x00008A26
	private Vector3 ContentContainerOffset
	{
		get
		{
			return Vector3.Scale(new Vector3(-1f, 1f, 1f), this.contentContainer.transform.localPosition);
		}
		set
		{
			this.contentContainer.transform.localPosition = Vector3.Scale(new Vector3(-1f, 1f, 1f), value);
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00049BFC File Offset: 0x00047DFC
	private void SetContentPosition()
	{
		Vector3 contentContainerOffset = this.ContentContainerOffset;
		float num = (this.contentLength - this.visibleAreaLength) * this.Value;
		if (num < 0f)
		{
			num = 0f;
		}
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			contentContainerOffset.x = num;
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			contentContainerOffset.y = num;
		}
		this.ContentContainerOffset = contentContainerOffset;
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00049C6C File Offset: 0x00047E6C
	private void BackgroundButtonDown()
	{
		if (this.allowSwipeScrolling && this.contentLength > this.visibleAreaLength)
		{
			if (!this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate += this.BackgroundOverUpdate;
			}
			this.swipeScrollingPressDownStartLocalPos = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
			this.swipePrevScrollingContentPressLocalPos = this.swipeScrollingPressDownStartLocalPos;
			this.swipeScrollingContentStartLocalPos = this.ContentContainerOffset;
			this.swipeScrollingContentDestLocalPos = this.swipeScrollingContentStartLocalPos;
			this.isBackgroundButtonDown = true;
			this.swipeCurrVelocity = 0f;
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00049D14 File Offset: 0x00047F14
	private void BackgroundOverUpdate()
	{
		if (this.isBackgroundButtonDown)
		{
			this.UpdateSwipeScrollDestintationPosition();
		}
		if (this.isSwipeScrollingInProgress)
		{
			float num = this.percent;
			float num2 = 0f;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.x;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.y;
			}
			float num3 = 0f;
			float num4 = this.contentLength - this.visibleAreaLength;
			if (this.isBackgroundButtonDown)
			{
				if (num2 < num3)
				{
					num2 += -num2 / this.visibleAreaLength / 2f;
					if (num2 > num3)
					{
						num2 = num3;
					}
				}
				else if (num2 > num4)
				{
					num2 -= (num2 - num4) / this.visibleAreaLength / 2f;
					if (num2 < num4)
					{
						num2 = num4;
					}
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			else
			{
				float num5 = this.visibleAreaLength * 0.001f;
				if (num2 < num3 || num2 > num4)
				{
					float num6 = (num2 >= num3) ? num4 : num3;
					num2 = Mathf.SmoothDamp(num2, num6, ref this.snapBackVelocity, 0.05f, float.PositiveInfinity, tk2dUITime.deltaTime);
					if (Mathf.Abs(this.snapBackVelocity) < num5)
					{
						num2 = num6;
						this.snapBackVelocity = 0f;
					}
					this.swipeCurrVelocity = 0f;
				}
				else if (this.swipeCurrVelocity != 0f)
				{
					num2 += this.swipeCurrVelocity * tk2dUITime.deltaTime * 20f;
					if (this.swipeCurrVelocity > num5 || this.swipeCurrVelocity < -num5)
					{
						this.swipeCurrVelocity = Mathf.Lerp(this.swipeCurrVelocity, 0f, tk2dUITime.deltaTime * 2.5f);
					}
					else
					{
						this.swipeCurrVelocity = 0f;
					}
				}
				else
				{
					this.isSwipeScrollingInProgress = false;
					tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			if (num != this.percent)
			{
				this.percent = num;
				this.ContentContainerOffset = this.swipeScrollingContentDestLocalPos;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
			}
			if (this.scrollBar != null)
			{
				float scrollPercentWithoutEvent = this.percent;
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					scrollPercentWithoutEvent = this.ContentContainerOffset.x / (this.contentLength - this.visibleAreaLength);
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					scrollPercentWithoutEvent = this.ContentContainerOffset.y / (this.contentLength - this.visibleAreaLength);
				}
				this.scrollBar.SetScrollPercentWithoutEvent(scrollPercentWithoutEvent);
			}
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0004A038 File Offset: 0x00048238
	private void UpdateSwipeScrollDestintationPosition()
	{
		Vector3 a = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
		Vector3 b = a - this.swipeScrollingPressDownStartLocalPos;
		b.x *= -1f;
		float f = 0f;
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			f = b.x;
			this.swipeCurrVelocity = -(a.x - this.swipePrevScrollingContentPressLocalPos.x);
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			f = b.y;
			this.swipeCurrVelocity = a.y - this.swipePrevScrollingContentPressLocalPos.y;
		}
		if (!this.isSwipeScrollingInProgress && Mathf.Abs(f) > 0.02f)
		{
			this.isSwipeScrollingInProgress = true;
			tk2dUIManager.Instance.OverrideClearAllChildrenPresses(this.backgroundUIItem);
		}
		if (this.isSwipeScrollingInProgress)
		{
			Vector3 vector = this.swipeScrollingContentStartLocalPos + b;
			vector.z = this.ContentContainerOffset.z;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				vector.y = this.ContentContainerOffset.y;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				vector.x = this.ContentContainerOffset.x;
			}
			vector.z = this.ContentContainerOffset.z;
			this.swipeScrollingContentDestLocalPos = vector;
			this.swipePrevScrollingContentPressLocalPos = a;
		}
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0000A852 File Offset: 0x00008A52
	private void BackgroundButtonRelease()
	{
		if (this.allowSwipeScrolling)
		{
			if (this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0000A892 File Offset: 0x00008A92
	private void BackgroundButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isBackgroundButtonOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = true;
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0000A8C7 File Offset: 0x00008AC7
	private void BackgroundButtonHoverOut()
	{
		if (this.isBackgroundButtonOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
		}
		this.isBackgroundButtonOver = false;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0004A1B0 File Offset: 0x000483B0
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Vector2 position = btn.Touch.position;
		Vector3 result = tk2dUIManager.Instance.UICamera.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - tk2dUIManager.Instance.UICamera.transform.position.z));
		result.z = btn.transform.position.z;
		return result;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0004A23C File Offset: 0x0004843C
	private void UpdateScrollbarActiveState()
	{
		bool flag = this.contentLength > this.visibleAreaLength;
		if (this.scrollBar != null && this.scrollBar.gameObject.activeSelf != flag)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.scrollBar.gameObject, flag);
		}
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0004A290 File Offset: 0x00048490
	private void ContentLengthVisibleAreaLengthChange(float prevContentLength, float newContentLength, float prevVisibleAreaLength, float newVisibleAreaLength)
	{
		float value;
		if (newContentLength - this.visibleAreaLength != 0f)
		{
			value = (prevContentLength - prevVisibleAreaLength) * this.Value / (newContentLength - newVisibleAreaLength);
		}
		else
		{
			value = 0f;
		}
		this.contentLength = newContentLength;
		this.visibleAreaLength = newVisibleAreaLength;
		this.UpdateScrollbarActiveState();
		this.Value = value;
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0000306D File Offset: 0x0000126D
	private void UnpressAllUIItemChildren()
	{
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0004A2E8 File Offset: 0x000484E8
	private static void GetRendererBoundsInChildren(Matrix4x4 rootWorldToLocal, Vector3[] minMax, Transform t)
	{
		MeshFilter component = t.GetComponent<MeshFilter>();
		if (component != null && component.sharedMesh != null)
		{
			Bounds bounds = component.sharedMesh.bounds;
			Matrix4x4 matrix4x = rootWorldToLocal * t.localToWorldMatrix;
			for (int i = 0; i < 8; i++)
			{
				Vector3 v = bounds.center + Vector3.Scale(bounds.extents, tk2dUIScrollableArea.boxExtents[i]);
				Vector3 rhs = matrix4x.MultiplyPoint(v);
				minMax[0] = Vector3.Min(minMax[0], rhs);
				minMax[1] = Vector3.Max(minMax[1], rhs);
			}
		}
		int childCount = t.childCount;
		for (int j = 0; j < childCount; j++)
		{
			Transform child = t.GetChild(j);
			if (t.gameObject.activeSelf)
			{
				tk2dUIScrollableArea.GetRendererBoundsInChildren(rootWorldToLocal, minMax, child);
			}
		}
	}

	// Token: 0x04000BEF RID: 3055
	private const float SWIPE_SCROLLING_FIRST_SCROLL_THRESHOLD = 0.02f;

	// Token: 0x04000BF0 RID: 3056
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	private float contentLength = 1f;

	// Token: 0x04000BF2 RID: 3058
	[SerializeField]
	private float visibleAreaLength = 1f;

	// Token: 0x04000BF3 RID: 3059
	public GameObject contentContainer;

	// Token: 0x04000BF4 RID: 3060
	public tk2dUIScrollbar scrollBar;

	// Token: 0x04000BF5 RID: 3061
	public tk2dUIItem backgroundUIItem;

	// Token: 0x04000BF6 RID: 3062
	public tk2dUIScrollableArea.Axes scrollAxes = tk2dUIScrollableArea.Axes.YAxis;

	// Token: 0x04000BF7 RID: 3063
	public bool allowSwipeScrolling = true;

	// Token: 0x04000BF8 RID: 3064
	public bool allowScrollWheel = true;

	// Token: 0x04000BF9 RID: 3065
	private bool isBackgroundButtonDown;

	// Token: 0x04000BFA RID: 3066
	private bool isBackgroundButtonOver;

	// Token: 0x04000BFB RID: 3067
	private Vector3 swipeScrollingPressDownStartLocalPos = Vector3.zero;

	// Token: 0x04000BFC RID: 3068
	private Vector3 swipeScrollingContentStartLocalPos = Vector3.zero;

	// Token: 0x04000BFD RID: 3069
	private Vector3 swipeScrollingContentDestLocalPos = Vector3.zero;

	// Token: 0x04000BFE RID: 3070
	private bool isSwipeScrollingInProgress;

	// Token: 0x04000BFF RID: 3071
	private Vector3 swipePrevScrollingContentPressLocalPos = Vector3.zero;

	// Token: 0x04000C00 RID: 3072
	private float swipeCurrVelocity;

	// Token: 0x04000C01 RID: 3073
	private float snapBackVelocity;

	// Token: 0x04000C02 RID: 3074
	private float percent;

	// Token: 0x04000C03 RID: 3075
	private static readonly Vector3[] boxExtents = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x020001B1 RID: 433
	public enum Axes
	{
		// Token: 0x04000C06 RID: 3078
		XAxis,
		// Token: 0x04000C07 RID: 3079
		YAxis
	}
}
