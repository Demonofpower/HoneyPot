using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollbar")]
public class tk2dUIScrollbar : MonoBehaviour
{
	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06000AD9 RID: 2777 RVA: 0x0000A91D File Offset: 0x00008B1D
	// (remove) Token: 0x06000ADA RID: 2778 RVA: 0x0000A936 File Offset: 0x00008B36
	public event Action<tk2dUIScrollbar> OnScroll;

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0000A94F File Offset: 0x00008B4F
	// (set) Token: 0x06000ADC RID: 2780 RVA: 0x0000A957 File Offset: 0x00008B57
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (this.OnScroll != null)
			{
				this.OnScroll(this);
			}
			this.SetScrollThumbPosition();
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0000A98C File Offset: 0x00008B8C
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.SetScrollThumbPosition();
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0004A3FC File Offset: 0x000485FC
	private void OnEnable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown += this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver += this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut += this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown += this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease += this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown += this.ScrollUpButtonDown;
			this.upButton.OnUp += this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown += this.ScrollDownButtonDown;
			this.downButton.OnUp += this.ScrollDownButtonUp;
		}
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0004A51C File Offset: 0x0004871C
	private void OnDisable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown -= this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver -= this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut -= this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown -= this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease -= this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown -= this.ScrollUpButtonDown;
			this.upButton.OnUp -= this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown -= this.ScrollDownButtonDown;
			this.downButton.OnUp -= this.ScrollDownButtonUp;
		}
		if (this.isScrollThumbButtonDown)
		{
			if (tk2dUIManager.Instance != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
			}
			this.isScrollThumbButtonDown = false;
		}
		if (this.isTrackHoverOver)
		{
			if (tk2dUIManager.Instance != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = false;
		}
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
			this.scrollUpDownButtonState = 0;
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0004A6D4 File Offset: 0x000488D4
	private void Awake()
	{
		if (this.upButton != null)
		{
			this.hoverUpButton = this.upButton.GetComponent<tk2dUIHoverItem>();
		}
		if (this.downButton != null)
		{
			this.hoverDownButton = this.downButton.GetComponent<tk2dUIHoverItem>();
		}
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0000A9AA File Offset: 0x00008BAA
	private void Start()
	{
		this.SetScrollThumbPosition();
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0000A9B2 File Offset: 0x00008BB2
	private void TrackHoverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			this.ScrollUpFixed();
		}
		else if (mouseWheelChange < 0f)
		{
			this.ScrollDownFixed();
		}
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0004A728 File Offset: 0x00048928
	private void SetScrollThumbPosition()
	{
		if (this.thumbTransform != null)
		{
			float num = -((this.scrollBarLength - this.thumbLength) * this.Value);
			Vector3 localPosition = this.thumbTransform.localPosition;
			if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
			{
				localPosition.x = -num;
			}
			else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
			{
				localPosition.y = num;
			}
			this.thumbTransform.localPosition = localPosition;
		}
		if (this.highlightProgressBar != null)
		{
			this.highlightProgressBar.Value = this.Value;
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0000A9DB File Offset: 0x00008BDB
	private void MoveScrollThumbButton()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.thumbBtn) + this.moveThumbBtnOffset);
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0004A1B0 File Offset: 0x000483B0
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Vector2 position = btn.Touch.position;
		Vector3 result = tk2dUIManager.Instance.UICamera.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - tk2dUIManager.Instance.UICamera.transform.position.z));
		result.z = btn.transform.position.z;
		return result;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0004A7C4 File Offset: 0x000489C4
	private void ScrollToPosition(Vector3 worldPos)
	{
		Vector3 vector = this.thumbTransform.parent.InverseTransformPoint(worldPos);
		float num = 0f;
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			num = vector.x;
		}
		else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
		{
			num = -vector.y;
		}
		this.Value = num / (this.scrollBarLength - this.thumbLength);
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0000A9FA File Offset: 0x00008BFA
	private void ScrollTrackButtonDown()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.barUIItem));
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0000AA0E File Offset: 0x00008C0E
	private void ScrollTrackButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isTrackHoverOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = true;
		}
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0000AA43 File Offset: 0x00008C43
	private void ScrollTrackButtonHoverOut()
	{
		if (this.isTrackHoverOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
		}
		this.isTrackHoverOver = false;
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0004A82C File Offset: 0x00048A2C
	private void ScrollThumbButtonDown()
	{
		if (!this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = true;
		Vector3 b = this.CalculateClickWorldPos(this.thumbBtn);
		this.moveThumbBtnOffset = this.thumbBtn.transform.position - b;
		this.moveThumbBtnOffset.z = 0f;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = true;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = true;
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0004A8D4 File Offset: 0x00048AD4
	private void ScrollThumbButtonRelease()
	{
		if (this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = false;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = false;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = false;
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0004A944 File Offset: 0x00048B44
	private void ScrollUpButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = -1;
		this.ScrollUpFixed();
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0000AA6D File Offset: 0x00008C6D
	private void ScrollUpButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004A998 File Offset: 0x00048B98
	private void ScrollDownButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 1;
		this.ScrollDownFixed();
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0000AA6D File Offset: 0x00008C6D
	private void ScrollDownButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0000AA97 File Offset: 0x00008C97
	public void ScrollUpFixed()
	{
		this.ScrollDirection(-1);
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0000AAA0 File Offset: 0x00008CA0
	public void ScrollDownFixed()
	{
		this.ScrollDirection(1);
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0004A9EC File Offset: 0x00048BEC
	private void CheckRepeatScrollUpDownButton()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			float num = Time.realtimeSinceStartup - this.timeOfUpDownButtonPressStart;
			if (this.repeatUpDownButtonHoldCounter == 0f)
			{
				if (num > 0.55f)
				{
					this.repeatUpDownButtonHoldCounter += 1f;
					num -= 0.55f;
					this.ScrollDirection(this.scrollUpDownButtonState);
				}
			}
			else if (num > 0.45f)
			{
				this.repeatUpDownButtonHoldCounter += 1f;
				num -= 0.45f;
				this.ScrollDirection(this.scrollUpDownButtonState);
			}
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0000AAA9 File Offset: 0x00008CA9
	public void ScrollDirection(int dir)
	{
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			this.Value -= this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir;
		}
		else
		{
			this.Value += this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir;
		}
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0000AAE7 File Offset: 0x00008CE7
	private float CalcScrollPercentOffsetButtonScrollDistance()
	{
		return 0.1f;
	}

	// Token: 0x04000C08 RID: 3080
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04000C09 RID: 3081
	private const float INITIAL_TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.55f;

	// Token: 0x04000C0A RID: 3082
	private const float TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.45f;

	// Token: 0x04000C0B RID: 3083
	public tk2dUIItem barUIItem;

	// Token: 0x04000C0C RID: 3084
	public float scrollBarLength;

	// Token: 0x04000C0D RID: 3085
	public tk2dUIItem thumbBtn;

	// Token: 0x04000C0E RID: 3086
	public Transform thumbTransform;

	// Token: 0x04000C0F RID: 3087
	public float thumbLength;

	// Token: 0x04000C10 RID: 3088
	public tk2dUIItem upButton;

	// Token: 0x04000C11 RID: 3089
	private tk2dUIHoverItem hoverUpButton;

	// Token: 0x04000C12 RID: 3090
	public tk2dUIItem downButton;

	// Token: 0x04000C13 RID: 3091
	private tk2dUIHoverItem hoverDownButton;

	// Token: 0x04000C14 RID: 3092
	public float buttonUpDownScrollDistance = 1f;

	// Token: 0x04000C15 RID: 3093
	public bool allowScrollWheel = true;

	// Token: 0x04000C16 RID: 3094
	public tk2dUIScrollbar.Axes scrollAxes = tk2dUIScrollbar.Axes.YAxis;

	// Token: 0x04000C17 RID: 3095
	public tk2dUIProgressBar highlightProgressBar;

	// Token: 0x04000C18 RID: 3096
	private bool isScrollThumbButtonDown;

	// Token: 0x04000C19 RID: 3097
	private bool isTrackHoverOver;

	// Token: 0x04000C1A RID: 3098
	private float percent;

	// Token: 0x04000C1B RID: 3099
	private Vector3 moveThumbBtnOffset = Vector3.zero;

	// Token: 0x04000C1C RID: 3100
	private int scrollUpDownButtonState;

	// Token: 0x04000C1D RID: 3101
	private float timeOfUpDownButtonPressStart;

	// Token: 0x04000C1E RID: 3102
	private float repeatUpDownButtonHoldCounter;

	// Token: 0x020001B3 RID: 435
	public enum Axes
	{
		// Token: 0x04000C21 RID: 3105
		XAxis,
		// Token: 0x04000C22 RID: 3106
		YAxis
	}
}
