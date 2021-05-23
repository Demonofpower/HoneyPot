using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class TooltipObject : MonoBehaviour
{
	// Token: 0x06000071 RID: 113 RVA: 0x00002B58 File Offset: 0x00000D58
	private void Awake()
	{
		this._enabled = true;
		this._started = false;
		this._paused = false;
		this._showing = false;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00002B76 File Offset: 0x00000D76
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000D6FC File Offset: 0x0000B8FC
	public void Init()
	{
		if (this._started)
		{
			return;
		}
		this._displayObject = base.gameObject.GetComponent<DisplayObject>();
		this._displayObject.MouseOverEvent += this.OnMouseOverEvent;
		this._displayObject.MouseOutEvent += this.OnMouseOutEvent;
		this._started = true;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00002B7E File Offset: 0x00000D7E
	public bool IsStarted()
	{
		return this._started;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00002B86 File Offset: 0x00000D86
	public bool IsEnabled()
	{
		return this._enabled;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000D75C File Offset: 0x0000B95C
	public void Enable()
	{
		if (this._enabled || this._paused)
		{
			return;
		}
		this._enabled = true;
		if (GameManager.System.Cursor.GetMouseTarget() == this._displayObject && !this._paused && this._displayObject.CanShowTooltip())
		{
			this.ShowTooltip();
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00002B8E File Offset: 0x00000D8E
	public void Disable()
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this._enabled = false;
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00002BBD File Offset: 0x00000DBD
	private void OnMouseOverEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused || !this._displayObject.CanShowTooltip())
		{
			return;
		}
		this.ShowTooltip();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00002BEC File Offset: 0x00000DEC
	private void ShowTooltip()
	{
		if (this._showing)
		{
			return;
		}
		GameManager.Stage.tooltip.ShowTooltip(this);
		this._showing = true;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00002C11 File Offset: 0x00000E11
	private void OnMouseOutEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this.HideTooltip();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00002C30 File Offset: 0x00000E30
	public void HideTooltip()
	{
		if (!this._showing)
		{
			return;
		}
		GameManager.Stage.tooltip.HideTooltip();
		this._showing = false;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00002C54 File Offset: 0x00000E54
	public DisplayObject GetTooltipSource()
	{
		return this._displayObject;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00002C5C File Offset: 0x00000E5C
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		this.HideTooltip();
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00002C77 File Offset: 0x00000E77
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
	private void OnDestroy()
	{
		this.HideTooltip();
		if (this._displayObject != null)
		{
			this._displayObject.MouseOverEvent -= this.OnMouseOverEvent;
			this._displayObject.MouseOutEvent -= this.OnMouseOutEvent;
		}
		this._displayObject = null;
	}

	// Token: 0x0400005B RID: 91
	public TooltipDirection direction;

	// Token: 0x0400005C RID: 92
	public bool mouseAttahced;

	// Token: 0x0400005D RID: 93
	public int xOffset;

	// Token: 0x0400005E RID: 94
	public int yOffset;

	// Token: 0x0400005F RID: 95
	public string layeredInChild;

	// Token: 0x04000060 RID: 96
	public GameObject contentPrefab;

	// Token: 0x04000061 RID: 97
	private DisplayObject _displayObject;

	// Token: 0x04000062 RID: 98
	private bool _enabled;

	// Token: 0x04000063 RID: 99
	private bool _started;

	// Token: 0x04000064 RID: 100
	private bool _paused;

	// Token: 0x04000065 RID: 101
	private bool _showing;
}
