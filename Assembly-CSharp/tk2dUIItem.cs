using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIItem")]
public class tk2dUIItem : MonoBehaviour
{
	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06000B5A RID: 2906 RVA: 0x0000B086 File Offset: 0x00009286
	// (remove) Token: 0x06000B5B RID: 2907 RVA: 0x0000B09F File Offset: 0x0000929F
	public event Action OnDown;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06000B5C RID: 2908 RVA: 0x0000B0B8 File Offset: 0x000092B8
	// (remove) Token: 0x06000B5D RID: 2909 RVA: 0x0000B0D1 File Offset: 0x000092D1
	public event Action OnUp;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06000B5E RID: 2910 RVA: 0x0000B0EA File Offset: 0x000092EA
	// (remove) Token: 0x06000B5F RID: 2911 RVA: 0x0000B103 File Offset: 0x00009303
	public event Action OnClick;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06000B60 RID: 2912 RVA: 0x0000B11C File Offset: 0x0000931C
	// (remove) Token: 0x06000B61 RID: 2913 RVA: 0x0000B135 File Offset: 0x00009335
	public event Action OnRelease;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06000B62 RID: 2914 RVA: 0x0000B14E File Offset: 0x0000934E
	// (remove) Token: 0x06000B63 RID: 2915 RVA: 0x0000B167 File Offset: 0x00009367
	public event Action OnHoverOver;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06000B64 RID: 2916 RVA: 0x0000B180 File Offset: 0x00009380
	// (remove) Token: 0x06000B65 RID: 2917 RVA: 0x0000B199 File Offset: 0x00009399
	public event Action OnHoverOut;

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06000B66 RID: 2918 RVA: 0x0000B1B2 File Offset: 0x000093B2
	// (remove) Token: 0x06000B67 RID: 2919 RVA: 0x0000B1CB File Offset: 0x000093CB
	public event Action<tk2dUIItem> OnDownUIItem;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06000B68 RID: 2920 RVA: 0x0000B1E4 File Offset: 0x000093E4
	// (remove) Token: 0x06000B69 RID: 2921 RVA: 0x0000B1FD File Offset: 0x000093FD
	public event Action<tk2dUIItem> OnUpUIItem;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06000B6A RID: 2922 RVA: 0x0000B216 File Offset: 0x00009416
	// (remove) Token: 0x06000B6B RID: 2923 RVA: 0x0000B22F File Offset: 0x0000942F
	public event Action<tk2dUIItem> OnClickUIItem;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06000B6C RID: 2924 RVA: 0x0000B248 File Offset: 0x00009448
	// (remove) Token: 0x06000B6D RID: 2925 RVA: 0x0000B261 File Offset: 0x00009461
	public event Action<tk2dUIItem> OnReleaseUIItem;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06000B6E RID: 2926 RVA: 0x0000B27A File Offset: 0x0000947A
	// (remove) Token: 0x06000B6F RID: 2927 RVA: 0x0000B293 File Offset: 0x00009493
	public event Action<tk2dUIItem> OnHoverOverUIItem;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06000B70 RID: 2928 RVA: 0x0000B2AC File Offset: 0x000094AC
	// (remove) Token: 0x06000B71 RID: 2929 RVA: 0x0000B2C5 File Offset: 0x000094C5
	public event Action<tk2dUIItem> OnHoverOutUIItem;

	// Token: 0x06000B72 RID: 2930 RVA: 0x0000B2DE File Offset: 0x000094DE
	private void Awake()
	{
		if (this.isChildOfAnotherUIItem)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0000B2F1 File Offset: 0x000094F1
	private void Start()
	{
		if (tk2dUIManager.Instance == null)
		{
			Debug.LogError("Unable to find tk2dUIManager. Please create a tk2dUIManager in the scene before proceeding.");
		}
		if (this.isChildOfAnotherUIItem && this.parentUIItem == null)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0000B32F File Offset: 0x0000952F
	public bool IsPressed
	{
		get
		{
			return this.isPressed;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0000B337 File Offset: 0x00009537
	public tk2dUITouch Touch
	{
		get
		{
			return this.touch;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0000B33F File Offset: 0x0000953F
	public tk2dUIItem ParentUIItem
	{
		get
		{
			return this.parentUIItem;
		}
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0000B347 File Offset: 0x00009547
	public void UpdateParent()
	{
		this.parentUIItem = this.GetParentUIItem();
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0000B355 File Offset: 0x00009555
	public void ManuallySetParent(tk2dUIItem newParentUIItem)
	{
		this.parentUIItem = newParentUIItem;
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0000B35E File Offset: 0x0000955E
	public void RemoveParent()
	{
		this.parentUIItem = null;
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0000B367 File Offset: 0x00009567
	public bool Press(tk2dUITouch touch)
	{
		return this.Press(touch, null);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0004BD24 File Offset: 0x00049F24
	public bool Press(tk2dUITouch touch, tk2dUIItem sentFromChild)
	{
		if (this.isPressed)
		{
			return false;
		}
		if (!this.isPressed)
		{
			this.touch = touch;
			if ((this.registerPressFromChildren || sentFromChild == null) && base.enabled)
			{
				this.isPressed = true;
				if (this.OnDown != null)
				{
					this.OnDown();
				}
				if (this.OnDownUIItem != null)
				{
					this.OnDownUIItem(this);
				}
				this.DoSendMessage(this.SendMessageOnDownMethodName);
			}
			if (this.parentUIItem != null)
			{
				this.parentUIItem.Press(touch, this);
			}
		}
		return true;
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0000B371 File Offset: 0x00009571
	public void UpdateTouch(tk2dUITouch touch)
	{
		this.touch = touch;
		if (this.parentUIItem != null)
		{
			this.parentUIItem.UpdateTouch(touch);
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0000B397 File Offset: 0x00009597
	private void DoSendMessage(string methodName)
	{
		if (this.sendMessageTarget != null && methodName.Length > 0)
		{
			this.sendMessageTarget.SendMessage(methodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0004BDD4 File Offset: 0x00049FD4
	public void Release()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
			if (this.OnClick != null)
			{
				this.OnClick();
			}
			if (this.OnClickUIItem != null)
			{
				this.OnClickUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnClickMethodName);
		}
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
		if (this.parentUIItem != null)
		{
			this.parentUIItem.Release();
		}
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0004BEBC File Offset: 0x0004A0BC
	public void CurrentOverUIItem(tk2dUIItem overUIItem)
	{
		if (overUIItem != this)
		{
			if (this.isPressed)
			{
				if (!this.CheckIsUIItemChildOfMe(overUIItem))
				{
					this.Exit();
					if (this.parentUIItem != null)
					{
						this.parentUIItem.CurrentOverUIItem(overUIItem);
					}
				}
			}
			else if (this.parentUIItem != null)
			{
				this.parentUIItem.CurrentOverUIItem(overUIItem);
			}
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0004BF34 File Offset: 0x0004A134
	public bool CheckIsUIItemChildOfMe(tk2dUIItem uiItem)
	{
		tk2dUIItem tk2dUIItem = null;
		bool result = false;
		if (uiItem != null)
		{
			tk2dUIItem = uiItem.parentUIItem;
		}
		while (tk2dUIItem != null)
		{
			if (tk2dUIItem == this)
			{
				result = true;
				break;
			}
			tk2dUIItem = tk2dUIItem.parentUIItem;
		}
		return result;
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0004BF84 File Offset: 0x0004A184
	public void Exit()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0004BFDC File Offset: 0x0004A1DC
	public bool HoverOver(tk2dUIItem prevHover)
	{
		bool flag = false;
		tk2dUIItem tk2dUIItem = null;
		if (!this.isHoverOver)
		{
			if (this.OnHoverOver != null)
			{
				this.OnHoverOver();
			}
			if (this.OnHoverOverUIItem != null)
			{
				this.OnHoverOverUIItem(this);
			}
			this.isHoverOver = true;
		}
		if (prevHover == this)
		{
			flag = true;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			tk2dUIItem = this.parentUIItem;
		}
		if (tk2dUIItem == null)
		{
			return flag;
		}
		return tk2dUIItem.HoverOver(prevHover) || flag;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0004C080 File Offset: 0x0004A280
	public void HoverOut(tk2dUIItem currHoverButton)
	{
		if (this.isHoverOver)
		{
			if (this.OnHoverOut != null)
			{
				this.OnHoverOut();
			}
			if (this.OnHoverOutUIItem != null)
			{
				this.OnHoverOutUIItem(this);
			}
			this.isHoverOver = false;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			if (currHoverButton == null)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
			else if (!this.parentUIItem.CheckIsUIItemChildOfMe(currHoverButton) && currHoverButton != this.parentUIItem)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0004C138 File Offset: 0x0004A338
	private tk2dUIItem GetParentUIItem()
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			tk2dUIItem component = parent.GetComponent<tk2dUIItem>();
			if (component != null)
			{
				return component;
			}
			parent = parent.parent;
		}
		return null;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0004C180 File Offset: 0x0004A380
	public void SimulateClick()
	{
		if (this.OnDown != null)
		{
			this.OnDown();
		}
		if (this.OnDownUIItem != null)
		{
			this.OnDownUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnDownMethodName);
		if (this.OnUp != null)
		{
			this.OnUp();
		}
		if (this.OnUpUIItem != null)
		{
			this.OnUpUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnUpMethodName);
		if (this.OnClick != null)
		{
			this.OnClick();
		}
		if (this.OnClickUIItem != null)
		{
			this.OnClickUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnClickMethodName);
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0000B3C4 File Offset: 0x000095C4
	public void InternalSetIsChildOfAnotherUIItem(bool state)
	{
		this.isChildOfAnotherUIItem = state;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0000B3CD File Offset: 0x000095CD
	public bool InternalGetIsChildOfAnotherUIItem()
	{
		return this.isChildOfAnotherUIItem;
	}

	// Token: 0x04000C5D RID: 3165
	public GameObject sendMessageTarget;

	// Token: 0x04000C5E RID: 3166
	public string SendMessageOnDownMethodName = string.Empty;

	// Token: 0x04000C5F RID: 3167
	public string SendMessageOnUpMethodName = string.Empty;

	// Token: 0x04000C60 RID: 3168
	public string SendMessageOnClickMethodName = string.Empty;

	// Token: 0x04000C61 RID: 3169
	public string SendMessageOnReleaseMethodName = string.Empty;

	// Token: 0x04000C62 RID: 3170
	[SerializeField]
	private bool isChildOfAnotherUIItem;

	// Token: 0x04000C63 RID: 3171
	public bool registerPressFromChildren;

	// Token: 0x04000C64 RID: 3172
	public bool isHoverEnabled;

	// Token: 0x04000C65 RID: 3173
	public Transform[] editorExtraBounds = new Transform[0];

	// Token: 0x04000C66 RID: 3174
	public Transform[] editorIgnoreBounds = new Transform[0];

	// Token: 0x04000C67 RID: 3175
	private bool isPressed;

	// Token: 0x04000C68 RID: 3176
	private bool isHoverOver;

	// Token: 0x04000C69 RID: 3177
	private tk2dUITouch touch;

	// Token: 0x04000C6A RID: 3178
	private tk2dUIItem parentUIItem;
}
