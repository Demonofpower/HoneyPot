using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BF RID: 447
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIManager")]
public class tk2dUIManager : MonoBehaviour
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06000B8A RID: 2954 RVA: 0x0000B3E5 File Offset: 0x000095E5
	// (remove) Token: 0x06000B8B RID: 2955 RVA: 0x0000B3FE File Offset: 0x000095FE
	public event Action OnAnyPress;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06000B8C RID: 2956 RVA: 0x0000B417 File Offset: 0x00009617
	// (remove) Token: 0x06000B8D RID: 2957 RVA: 0x0000B430 File Offset: 0x00009630
	public event Action OnInputUpdate;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06000B8E RID: 2958 RVA: 0x0000B449 File Offset: 0x00009649
	// (remove) Token: 0x06000B8F RID: 2959 RVA: 0x0000B462 File Offset: 0x00009662
	public event Action<float> OnScrollWheelChange;

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0000B47B File Offset: 0x0000967B
	public static tk2dUIManager Instance
	{
		get
		{
			if (tk2dUIManager.instance == null)
			{
				tk2dUIManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(tk2dUIManager)) as tk2dUIManager);
			}
			return tk2dUIManager.instance;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0000B4AB File Offset: 0x000096AB
	// (set) Token: 0x06000B92 RID: 2962 RVA: 0x0000B4B3 File Offset: 0x000096B3
	public Camera UICamera
	{
		get
		{
			return this.uiCamera;
		}
		set
		{
			this.uiCamera = value;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0000B4BC File Offset: 0x000096BC
	// (set) Token: 0x06000B94 RID: 2964 RVA: 0x0004C304 File Offset: 0x0004A504
	public bool InputEnabled
	{
		get
		{
			return this.inputEnabled;
		}
		set
		{
			if (this.inputEnabled && !value)
			{
				this.inputEnabled = value;
				if (this.useMultiTouch)
				{
					this.CheckMultiTouchInputs();
				}
				else
				{
					this.CheckInputs();
				}
			}
			else
			{
				this.inputEnabled = value;
			}
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0000B4C4 File Offset: 0x000096C4
	public tk2dUIItem PressedUIItem
	{
		get
		{
			if (!this.useMultiTouch)
			{
				return this.pressedUIItem;
			}
			if (this.pressedUIItems.Length > 0)
			{
				return this.pressedUIItems[this.pressedUIItems.Length - 1];
			}
			return null;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0000B4F9 File Offset: 0x000096F9
	public tk2dUIItem[] PressedUIItems
	{
		get
		{
			return this.pressedUIItems;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0000B501 File Offset: 0x00009701
	// (set) Token: 0x06000B98 RID: 2968 RVA: 0x0000B509 File Offset: 0x00009709
	public bool UseMultiTouch
	{
		get
		{
			return this.useMultiTouch;
		}
		set
		{
			if (this.useMultiTouch != value && this.inputEnabled)
			{
				this.InputEnabled = false;
				this.useMultiTouch = value;
				this.InputEnabled = true;
			}
			else
			{
				this.useMultiTouch = value;
			}
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0004C354 File Offset: 0x0004A554
	private void Awake()
	{
		if (tk2dUIManager.instance == null)
		{
			tk2dUIManager.instance = this;
		}
		else if (tk2dUIManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		tk2dUITime.Init();
		this.Setup();
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0000B543 File Offset: 0x00009743
	private void Setup()
	{
		if (!this.areHoverEventsTracked)
		{
			this.checkForHovers = false;
		}
		if (this.uiCamera == null)
		{
			Debug.LogWarning("Camera needs to be attached to tk2dUIManager for it to work");
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0004C3A4 File Offset: 0x0004A5A4
	private void Update()
	{
		tk2dUITime.Update();
		if (this.inputEnabled)
		{
			if (this.useMultiTouch)
			{
				this.CheckMultiTouchInputs();
			}
			else
			{
				this.CheckInputs();
			}
			if (this.OnInputUpdate != null)
			{
				this.OnInputUpdate();
			}
			if (this.OnScrollWheelChange != null)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.OnScrollWheelChange(axis);
				}
			}
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0004C420 File Offset: 0x0004A620
	private void CheckInputs()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		this.primaryTouch = default(tk2dUITouch);
		this.secondaryTouch = default(tk2dUITouch);
		this.resultTouch = default(tk2dUITouch);
		this.hitUIItem = null;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.primaryTouch = new tk2dUITouch(touch);
						flag = true;
						flag3 = true;
					}
					else if (this.pressedUIItem != null && touch.fingerId == this.firstPressedUIItemTouch.fingerId)
					{
						this.secondaryTouch = new tk2dUITouch(touch);
						flag2 = true;
					}
				}
				this.checkForHovers = false;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.primaryTouch = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				flag = true;
				flag3 = true;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = Vector2.zero;
				TouchPhase phase = TouchPhase.Moved;
				if (this.pressedUIItem != null)
				{
					vector = this.firstPressedUIItemTouch.position - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				}
				if (Input.GetMouseButtonUp(0))
				{
					phase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					phase = TouchPhase.Stationary;
				}
				this.secondaryTouch = new tk2dUITouch(phase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				flag2 = true;
			}
		}
		if (flag)
		{
			this.resultTouch = this.primaryTouch;
		}
		else if (flag2)
		{
			this.resultTouch = this.secondaryTouch;
		}
		if (flag || flag2)
		{
			this.hitUIItem = this.GetHitUIItemFromTouch(this.resultTouch);
			if (this.resultTouch.phase == TouchPhase.Began)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					if (this.pressedUIItem != this.hitUIItem)
					{
						this.pressedUIItem.Release();
						this.pressedUIItem = null;
					}
					else
					{
						this.firstPressedUIItemTouch = this.resultTouch;
					}
				}
				if (this.hitUIItem != null)
				{
					this.hitUIItem.Press(this.resultTouch);
				}
				this.pressedUIItem = this.hitUIItem;
				this.firstPressedUIItemTouch = this.resultTouch;
			}
			else if (this.resultTouch.phase == TouchPhase.Ended)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					this.pressedUIItem.UpdateTouch(this.resultTouch);
					this.pressedUIItem.Release();
					this.pressedUIItem = null;
				}
			}
			else if (this.pressedUIItem != null)
			{
				this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
				this.pressedUIItem.UpdateTouch(this.resultTouch);
			}
		}
		else if (this.pressedUIItem != null)
		{
			this.pressedUIItem.CurrentOverUIItem(null);
			this.pressedUIItem.Release();
			this.pressedUIItem = null;
		}
		if (this.checkForHovers)
		{
			if (this.inputEnabled)
			{
				if (!flag && !flag2 && this.hitUIItem == null && !Input.GetMouseButton(0))
				{
					this.ray = this.uiCamera.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(this.ray, out this.hit, this.uiCamera.farClipPlane, this.raycastLayerMask))
					{
						this.hitUIItem = this.hit.collider.GetComponent<tk2dUIItem>();
					}
				}
				else if (Input.GetMouseButton(0))
				{
					this.hitUIItem = null;
				}
			}
			if (this.hitUIItem != null)
			{
				if (this.hitUIItem.isHoverEnabled)
				{
					if (!this.hitUIItem.HoverOver(this.overUIItem) && this.overUIItem != null)
					{
						this.overUIItem.HoverOut(this.hitUIItem);
					}
					this.overUIItem = this.hitUIItem;
				}
				else if (this.overUIItem != null)
				{
					this.overUIItem.HoverOut(null);
				}
			}
			else if (this.overUIItem != null)
			{
				this.overUIItem.HoverOut(null);
			}
		}
		if (flag3 && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0004C928 File Offset: 0x0004AB28
	private void CheckMultiTouchInputs()
	{
		bool flag = false;
		this.touchCounter = 0;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (this.touchCounter >= 5)
					{
						break;
					}
					this.allTouches[this.touchCounter] = new tk2dUITouch(touch);
					this.touchCounter++;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.allTouches[this.touchCounter] = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				this.mouseDownFirstPos = Input.mousePosition;
				this.touchCounter++;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = this.mouseDownFirstPos - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				TouchPhase phase = TouchPhase.Moved;
				if (Input.GetMouseButtonUp(0))
				{
					phase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					phase = TouchPhase.Stationary;
				}
				this.allTouches[this.touchCounter] = new tk2dUITouch(phase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				this.touchCounter++;
			}
		}
		for (int j = 0; j < this.touchCounter; j++)
		{
			this.pressedUIItems[j] = this.GetHitUIItemFromTouch(this.allTouches[j]);
		}
		for (int k = 0; k < this.prevPressedUIItemList.Count; k++)
		{
			this.prevPressedItem = this.prevPressedUIItemList[k];
			if (this.prevPressedItem != null)
			{
				int fingerId = this.prevPressedItem.Touch.fingerId;
				bool flag2 = false;
				for (int l = 0; l < this.touchCounter; l++)
				{
					this.currTouch = this.allTouches[l];
					if (this.currTouch.fingerId == fingerId)
					{
						flag2 = true;
						this.currPressedItem = this.pressedUIItems[l];
						if (this.currTouch.phase == TouchPhase.Began)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							if (this.prevPressedItem != this.currPressedItem)
							{
								this.prevPressedItem.Release();
								this.prevPressedUIItemList.RemoveAt(k);
								k--;
							}
						}
						else if (this.currTouch.phase == TouchPhase.Ended)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
							this.prevPressedItem.Release();
							this.prevPressedUIItemList.RemoveAt(k);
							k--;
						}
						else
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
						}
						break;
					}
				}
				if (!flag2)
				{
					this.prevPressedItem.CurrentOverUIItem(null);
					this.prevPressedItem.Release();
					this.prevPressedUIItemList.RemoveAt(k);
					k--;
				}
			}
		}
		for (int m = 0; m < this.touchCounter; m++)
		{
			this.currPressedItem = this.pressedUIItems[m];
			this.currTouch = this.allTouches[m];
			if (this.currTouch.phase == TouchPhase.Began)
			{
				if (this.currPressedItem != null)
				{
					bool flag3 = this.currPressedItem.Press(this.currTouch);
					if (flag3)
					{
						this.prevPressedUIItemList.Add(this.currPressedItem);
					}
				}
				flag = true;
			}
		}
		if (flag && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0004CD80 File Offset: 0x0004AF80
	private tk2dUIItem GetHitUIItemFromTouch(tk2dUITouch touch)
	{
		this.ray = this.uiCamera.ScreenPointToRay(touch.position);
		if (Physics.Raycast(this.ray, out this.hit, this.uiCamera.farClipPlane, this.raycastLayerMask))
		{
			return this.hit.collider.GetComponent<tk2dUIItem>();
		}
		return null;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0004CDE8 File Offset: 0x0004AFE8
	public void OverrideClearAllChildrenPresses(tk2dUIItem item)
	{
		if (this.useMultiTouch)
		{
			for (int i = 0; i < this.pressedUIItems.Length; i++)
			{
				tk2dUIItem tk2dUIItem = this.pressedUIItems[i];
				if (tk2dUIItem != null && item.CheckIsUIItemChildOfMe(tk2dUIItem))
				{
					tk2dUIItem.CurrentOverUIItem(item);
				}
			}
		}
		else if (this.pressedUIItem != null && item.CheckIsUIItemChildOfMe(this.pressedUIItem))
		{
			this.pressedUIItem.CurrentOverUIItem(item);
		}
	}

	// Token: 0x04000C77 RID: 3191
	private const int MAX_MULTI_TOUCH_COUNT = 5;

	// Token: 0x04000C78 RID: 3192
	private const string MOUSE_WHEEL_AXES_NAME = "Mouse ScrollWheel";

	// Token: 0x04000C79 RID: 3193
	public static double version = 1.0;

	// Token: 0x04000C7A RID: 3194
	public static int releaseId;

	// Token: 0x04000C7B RID: 3195
	private static tk2dUIManager instance;

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	private Camera uiCamera;

	// Token: 0x04000C7D RID: 3197
	public LayerMask raycastLayerMask = -1;

	// Token: 0x04000C7E RID: 3198
	private bool inputEnabled = true;

	// Token: 0x04000C7F RID: 3199
	public bool areHoverEventsTracked = true;

	// Token: 0x04000C80 RID: 3200
	private tk2dUIItem pressedUIItem;

	// Token: 0x04000C81 RID: 3201
	private tk2dUIItem overUIItem;

	// Token: 0x04000C82 RID: 3202
	private tk2dUITouch firstPressedUIItemTouch;

	// Token: 0x04000C83 RID: 3203
	private bool checkForHovers = true;

	// Token: 0x04000C84 RID: 3204
	[SerializeField]
	private bool useMultiTouch;

	// Token: 0x04000C85 RID: 3205
	private tk2dUITouch[] allTouches = new tk2dUITouch[5];

	// Token: 0x04000C86 RID: 3206
	private List<tk2dUIItem> prevPressedUIItemList = new List<tk2dUIItem>();

	// Token: 0x04000C87 RID: 3207
	private tk2dUIItem[] pressedUIItems = new tk2dUIItem[5];

	// Token: 0x04000C88 RID: 3208
	private int touchCounter;

	// Token: 0x04000C89 RID: 3209
	private Vector2 mouseDownFirstPos = Vector2.zero;

	// Token: 0x04000C8A RID: 3210
	private tk2dUITouch primaryTouch = default(tk2dUITouch);

	// Token: 0x04000C8B RID: 3211
	private tk2dUITouch secondaryTouch = default(tk2dUITouch);

	// Token: 0x04000C8C RID: 3212
	private tk2dUITouch resultTouch = default(tk2dUITouch);

	// Token: 0x04000C8D RID: 3213
	private tk2dUIItem hitUIItem;

	// Token: 0x04000C8E RID: 3214
	private RaycastHit hit;

	// Token: 0x04000C8F RID: 3215
	private Ray ray;

	// Token: 0x04000C90 RID: 3216
	private tk2dUITouch currTouch;

	// Token: 0x04000C91 RID: 3217
	private tk2dUIItem currPressedItem;

	// Token: 0x04000C92 RID: 3218
	private tk2dUIItem prevPressedItem;
}
