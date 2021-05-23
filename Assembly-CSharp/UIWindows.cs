using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class UIWindows : DisplayObject
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06000470 RID: 1136 RVA: 0x00005589 File Offset: 0x00003789
	// (remove) Token: 0x06000471 RID: 1137 RVA: 0x000055A2 File Offset: 0x000037A2
	public event UIWindows.UIWindowsDelegate UIWindowShowingEvent;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06000472 RID: 1138 RVA: 0x000055BB File Offset: 0x000037BB
	// (remove) Token: 0x06000473 RID: 1139 RVA: 0x000055D4 File Offset: 0x000037D4
	public event UIWindows.UIWindowsDelegate UIWindowShownEvent;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06000474 RID: 1140 RVA: 0x000055ED File Offset: 0x000037ED
	// (remove) Token: 0x06000475 RID: 1141 RVA: 0x00005606 File Offset: 0x00003806
	public event UIWindows.UIWindowsDelegate UIWindowHidingEvent;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000476 RID: 1142 RVA: 0x0000561F File Offset: 0x0000381F
	// (remove) Token: 0x06000477 RID: 1143 RVA: 0x00005638 File Offset: 0x00003838
	public event UIWindows.UIWindowsDelegate UIWindowsClearEvent;

	// Token: 0x06000478 RID: 1144 RVA: 0x00005651 File Offset: 0x00003851
	protected override void OnStart()
	{
		base.OnStart();
		this.forceResponseOptions = null;
		this._incomingWindow = null;
		this._incomingWindowPrefab = null;
		this._activeWindow = null;
		this._activeWindowPrefab = null;
		this._isActiveWindowSettled = false;
		this._activeIsDefault = false;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00024AE8 File Offset: 0x00022CE8
	public UIWindow SetWindow(GameObject uiWindowPrefab, bool showWindow = true, bool isBack = false)
	{
		if (this._incomingWindow != null)
		{
			UnityEngine.Object.Destroy(this._incomingWindow.gameObj);
		}
		this._incomingWindow = null;
		this._incomingWindowPrefab = null;
		this._incomingWindowPrefab = uiWindowPrefab;
		this._incomingWindow = (UnityEngine.Object.Instantiate(uiWindowPrefab) as GameObject).GetComponent<UIWindow>();
		this._incomingWindow.Init();
		this._incomingWindow.IsBack = isBack;
		if (uiWindowPrefab == this.defaultWindow)
		{
			this._activeIsDefault = true;
		}
		else
		{
			this._activeIsDefault = false;
		}
		UIWindow incomingWindow = this._incomingWindow;
		if (showWindow)
		{
			this.ShowIncomingWindow();
		}
		return incomingWindow;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00024B90 File Offset: 0x00022D90
	public void ShowIncomingWindow()
	{
		if (this._incomingWindow == null || this._incomingWindow == this._activeWindow)
		{
			return;
		}
		this._activeWindowPrefab = this._incomingWindowPrefab;
		this._incomingWindowPrefab = null;
		this._activeWindow = this._incomingWindow;
		this._incomingWindow = null;
		base.AddChild(this._activeWindow);
		this._activeWindow.UIWindowShownEvent += this.OnUIWindowShown;
		this._activeWindow.ShowWindow();
		if (this.UIWindowShowingEvent != null)
		{
			this.UIWindowShowingEvent();
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0000568A File Offset: 0x0000388A
	private void OnUIWindowShown(UIWindow uiWindow)
	{
		this._activeWindow.UIWindowShownEvent -= this.OnUIWindowShown;
		this._isActiveWindowSettled = true;
		if (this.UIWindowShownEvent != null)
		{
			this.UIWindowShownEvent();
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00024C30 File Offset: 0x00022E30
	public void HideActiveWindow()
	{
		if (this._activeWindow == null)
		{
			if (this.UIWindowsClearEvent != null)
			{
				this.UIWindowsClearEvent();
			}
			return;
		}
		this._activeWindow.UIWindowHiddenEvent += this.OnUIWindowHidden;
		this._activeWindow.HideWindow();
		this._isActiveWindowSettled = false;
		if (this.UIWindowHidingEvent != null)
		{
			this.UIWindowHidingEvent();
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00024CA4 File Offset: 0x00022EA4
	private void OnUIWindowHidden(UIWindow uiWindow)
	{
		uiWindow.UIWindowHiddenEvent -= this.OnUIWindowHidden;
		if (uiWindow != this._activeWindow)
		{
			return;
		}
		this._activeWindow = null;
		this._activeWindowPrefab = null;
		if (this._incomingWindow != null)
		{
			this.ShowIncomingWindow();
		}
		else if (this.UIWindowsClearEvent != null)
		{
			this.UIWindowsClearEvent();
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x000056C0 File Offset: 0x000038C0
	public bool ExecuteBackCommand()
	{
		return !(this._activeWindow == null) && this._activeWindow.ExecuteBackCommand();
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000056E0 File Offset: 0x000038E0
	public bool IsDefaultWindowActive(bool andSettled = true)
	{
		if (this._activeWindow == null)
		{
			return false;
		}
		if (andSettled)
		{
			return this._activeIsDefault && this.IsActiveWindowSettled();
		}
		return this._activeIsDefault;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00005716 File Offset: 0x00003916
	public UIWindow GetActiveWindow()
	{
		return this._activeWindow;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0000571E File Offset: 0x0000391E
	public GameObject GetActiveWindowPrefab()
	{
		return this._activeWindowPrefab;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00005726 File Offset: 0x00003926
	public bool IsActiveWindowSettled()
	{
		return this._isActiveWindowSettled;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0000572E File Offset: 0x0000392E
	public void ResetActiveWindow()
	{
		if (this._activeWindow == null)
		{
			return;
		}
		this.SetWindow(this._activeWindowPrefab, false, false);
		this.HideActiveWindow();
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00005757 File Offset: 0x00003957
	public void RefreshActiveWindow()
	{
		if (this._activeWindow == null)
		{
			return;
		}
		this._activeWindow.Refresh();
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00005776 File Offset: 0x00003976
	protected override void Destructor()
	{
		this._incomingWindowPrefab = null;
		this._incomingWindow = null;
		this._activeWindowPrefab = null;
		this._activeWindow = null;
		base.Destructor();
	}

	// Token: 0x040003E0 RID: 992
	public GameObject defaultWindow;

	// Token: 0x040003E1 RID: 993
	public GameObject talkWindow;

	// Token: 0x040003E2 RID: 994
	public List<DialogSceneResponseOption> forceResponseOptions;

	// Token: 0x040003E3 RID: 995
	private GameObject _incomingWindowPrefab;

	// Token: 0x040003E4 RID: 996
	private UIWindow _incomingWindow;

	// Token: 0x040003E5 RID: 997
	private GameObject _activeWindowPrefab;

	// Token: 0x040003E6 RID: 998
	private UIWindow _activeWindow;

	// Token: 0x040003E7 RID: 999
	private bool _isActiveWindowSettled;

	// Token: 0x040003E8 RID: 1000
	private bool _activeIsDefault;

	// Token: 0x02000097 RID: 151
	// (Invoke) Token: 0x06000487 RID: 1159
	public delegate void UIWindowsDelegate();
}
