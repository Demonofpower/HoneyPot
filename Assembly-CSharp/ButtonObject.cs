using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class ButtonObject : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600004A RID: 74 RVA: 0x00002925 File Offset: 0x00000B25
	// (remove) Token: 0x0600004B RID: 75 RVA: 0x0000293E File Offset: 0x00000B3E
	public event ButtonObject.ButtonDelegate ButtonOverEvent;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600004C RID: 76 RVA: 0x00002957 File Offset: 0x00000B57
	// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002970 File Offset: 0x00000B70
	public event ButtonObject.ButtonDelegate ButtonOutEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600004E RID: 78 RVA: 0x00002989 File Offset: 0x00000B89
	// (remove) Token: 0x0600004F RID: 79 RVA: 0x000029A2 File Offset: 0x00000BA2
	public event ButtonObject.ButtonDelegate ButtonDownEvent;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000050 RID: 80 RVA: 0x000029BB File Offset: 0x00000BBB
	// (remove) Token: 0x06000051 RID: 81 RVA: 0x000029D4 File Offset: 0x00000BD4
	public event ButtonObject.ButtonDelegate ButtonPressedEvent;

	// Token: 0x06000052 RID: 82 RVA: 0x000029ED File Offset: 0x00000BED
	private void Awake()
	{
		this._enabled = true;
		this._started = false;
		this._paused = false;
		this._transitionsActive = true;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002A0B File Offset: 0x00000C0B
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
	public void Init()
	{
		if (this._started)
		{
			return;
		}
		this._displayObject = base.gameObject.GetComponent<DisplayObject>();
		this._boxCollider = this._displayObject.gameObj.GetComponent<BoxCollider>();
		this._displayObject.MouseOverEvent += this.OnMouseOverEvent;
		this._displayObject.MouseOutEvent += this.OnMouseOutEvent;
		this._displayObject.MouseDownEvent += this.OnMouseDownEvent;
		this._displayObject.MouseClickEvent += this.OnMouseClickEvent;
		this._overTransitions = new List<ButtonStateTransition>();
		foreach (ButtonStateTransitionDef def in this.overTransitionDefs)
		{
			this._overTransitions.Add(new ButtonStateTransition(def, this._displayObject));
		}
		this._downTransitions = new List<ButtonStateTransition>();
		foreach (ButtonStateTransitionDef def2 in this.downTransitionDefs)
		{
			this._downTransitions.Add(new ButtonStateTransition(def2, this._displayObject));
		}
		this._disableTransitions = new List<ButtonStateTransition>();
		foreach (ButtonStateTransitionDef def3 in this.disableTransitionDefs)
		{
			this._disableTransitions.Add(new ButtonStateTransition(def3, this._displayObject));
		}
		if (!this._enabled)
		{
			this.ApplyTransitions(this._disableTransitions);
		}
		this._started = true;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0000CB90 File Offset: 0x0000AD90
	public void Enable()
	{
		if (this._enabled || this._paused)
		{
			return;
		}
		this._enabled = true;
		if (this._started)
		{
			this.ReverseTransitions(this._disableTransitions);
			if (GameManager.System.Cursor.IsMouseDownTarget(this._displayObject))
			{
				this.OnMouseOverEvent(this._displayObject);
				this.OnMouseDownEvent(this._displayObject);
			}
			else if (GameManager.System.Cursor.IsMouseTarget(this._displayObject))
			{
				this.OnMouseOverEvent(this._displayObject);
			}
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000CC30 File Offset: 0x0000AE30
	public void Disable()
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this._enabled = false;
		if (this._started)
		{
			this.ReverseTransitions(this._downTransitions);
			this.ReverseTransitions(this._overTransitions);
			this.ApplyTransitions(this._disableTransitions);
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00002A13 File Offset: 0x00000C13
	public bool IsEnabled()
	{
		return this._enabled;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0000CC8C File Offset: 0x0000AE8C
	private void OnMouseOverEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this.ApplyTransitions(this._overTransitions);
		GameManager.System.Audio.Play(AudioCategory.SOUND, this.overSound, false, 1f, true);
		if (this.ButtonOverEvent != null)
		{
			this.ButtonOverEvent(this);
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
	private void OnMouseOutEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this.ReverseTransitions(this._downTransitions);
		this.ReverseTransitions(this._overTransitions);
		if (this.ButtonOutEvent != null)
		{
			this.ButtonOutEvent(this);
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00002A1B File Offset: 0x00000C1B
	private void OnMouseDownEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this.ApplyTransitions(this._downTransitions);
		if (this.ButtonDownEvent != null)
		{
			this.ButtonDownEvent(this);
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000CD48 File Offset: 0x0000AF48
	private void OnMouseClickEvent(DisplayObject displayObject)
	{
		if (!this._enabled || this._paused)
		{
			return;
		}
		this.OnMouseOutEvent(displayObject);
		this.ApplyTransitions(this._overTransitions);
		if (this.ButtonPressedEvent != null)
		{
			this.ButtonPressedEvent(this);
			if (this.clickEffectEmitter != null && this.clickEffectSpriteGroup != null && this._boxCollider != null)
			{
				ParticleEmitter2D component = new GameObject("ButtonClickParticleEmitter", new Type[]
				{
					typeof(ParticleEmitter2D)
				}).GetComponent<ParticleEmitter2D>();
				GameManager.Stage.effects.AddParticleEffect(component, this._displayObject);
				component.Init(this.clickEffectEmitter, this.clickEffectSpriteGroup, false);
				component.details.originSpreadX = this._boxCollider.size.x / 2f;
				component.details.originSpreadY = this._boxCollider.size.y / 2f;
				component.SetGlobalPosition(this._displayObject.gameObj.transform.position.x, this._displayObject.gameObj.transform.position.y);
			}
			DisplayObject source = this._displayObject;
			if (this._displayObject == GameManager.Stage.uiTop.buttonHuniebee)
			{
				source = GameManager.Stage.effects;
			}
			GameManager.System.Cursor.ShowCursorRippleEffect(source);
			GameManager.System.Audio.Play(AudioCategory.SOUND, this.clickSound, false, 1f, true);
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0000CF00 File Offset: 0x0000B100
	private void ApplyTransitions(List<ButtonStateTransition> transitions)
	{
		if (!this._transitionsActive)
		{
			return;
		}
		for (int i = 0; i < transitions.Count; i++)
		{
			transitions[i].Apply();
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000CF3C File Offset: 0x0000B13C
	private void ReverseTransitions(List<ButtonStateTransition> transitions)
	{
		if (!this._transitionsActive)
		{
			return;
		}
		for (int i = 0; i < transitions.Count; i++)
		{
			transitions[i].Reverse();
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000CF78 File Offset: 0x0000B178
	public void ChangeOrigSpriteOfStateTransition(ButtonState state, int index, string spriteName)
	{
		switch (state)
		{
		case ButtonState.OVER:
			this._overTransitions[index].ChangeOrigSprite(spriteName);
			break;
		case ButtonState.DOWN:
			this._downTransitions[index].ChangeOrigSprite(spriteName);
			break;
		case ButtonState.DISABLED:
			this._disableTransitions[index].ChangeOrigSprite(spriteName);
			break;
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
	public void ChangeStateSpriteOfStateTransition(ButtonState state, int index, string spriteName)
	{
		switch (state)
		{
		case ButtonState.OVER:
			this._overTransitions[index].ChangeStateSprite(spriteName);
			break;
		case ButtonState.DOWN:
			this._downTransitions[index].ChangeStateSprite(spriteName);
			break;
		case ButtonState.DISABLED:
			this._disableTransitions[index].ChangeStateSprite(spriteName);
			break;
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00002A57 File Offset: 0x00000C57
	public void ToggleTransitions(bool active)
	{
		this._transitionsActive = active;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00002A60 File Offset: 0x00000C60
	public bool IsStarted()
	{
		return this._started;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00002A68 File Offset: 0x00000C68
	public DisplayObject GetDisplayObject()
	{
		return this._displayObject;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00002A70 File Offset: 0x00000C70
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
		this.ReverseTransitions(this._downTransitions);
		this.ReverseTransitions(this._overTransitions);
		this._boxCollider.enabled = false;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00002AA9 File Offset: 0x00000CA9
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
		if (this._displayObject.IsRecursivelyInteractive())
		{
			this._boxCollider.enabled = true;
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0000D058 File Offset: 0x0000B258
	private void OnDestroy()
	{
		if (this._displayObject != null)
		{
			this._displayObject.MouseOverEvent -= this.OnMouseOverEvent;
			this._displayObject.MouseOutEvent -= this.OnMouseOutEvent;
			this._displayObject.MouseDownEvent -= this.OnMouseDownEvent;
			this._displayObject.MouseClickEvent -= this.OnMouseClickEvent;
		}
		this.ButtonOverEvent = null;
		this.ButtonOutEvent = null;
		this.ButtonDownEvent = null;
		this.ButtonPressedEvent = null;
	}

	// Token: 0x04000025 RID: 37
	public ParticleEmitter2DDefinition clickEffectEmitter;

	// Token: 0x04000026 RID: 38
	public SpriteGroupDefinition clickEffectSpriteGroup;

	// Token: 0x04000027 RID: 39
	public AudioDefinition overSound;

	// Token: 0x04000028 RID: 40
	public AudioDefinition clickSound;

	// Token: 0x04000029 RID: 41
	public List<ButtonStateTransitionDef> overTransitionDefs = new List<ButtonStateTransitionDef>();

	// Token: 0x0400002A RID: 42
	public List<ButtonStateTransitionDef> downTransitionDefs = new List<ButtonStateTransitionDef>();

	// Token: 0x0400002B RID: 43
	public List<ButtonStateTransitionDef> disableTransitionDefs = new List<ButtonStateTransitionDef>();

	// Token: 0x0400002C RID: 44
	private DisplayObject _displayObject;

	// Token: 0x0400002D RID: 45
	private BoxCollider _boxCollider;

	// Token: 0x0400002E RID: 46
	private bool _enabled;

	// Token: 0x0400002F RID: 47
	private bool _started;

	// Token: 0x04000030 RID: 48
	private bool _paused;

	// Token: 0x04000031 RID: 49
	private bool _transitionsActive;

	// Token: 0x04000032 RID: 50
	private List<ButtonStateTransition> _overTransitions;

	// Token: 0x04000033 RID: 51
	private List<ButtonStateTransition> _downTransitions;

	// Token: 0x04000034 RID: 52
	private List<ButtonStateTransition> _disableTransitions;

	// Token: 0x0200000B RID: 11
	// (Invoke) Token: 0x06000067 RID: 103
	public delegate void ButtonDelegate(ButtonObject buttonObject);
}
