using System;
using System.Linq;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000016 RID: 22
[ExecuteInEditMode]
public class DisplayObject : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600009F RID: 159 RVA: 0x00002E40 File Offset: 0x00001040
	// (remove) Token: 0x060000A0 RID: 160 RVA: 0x00002E59 File Offset: 0x00001059
	public event DisplayObject.MouseDelegate MouseOverEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060000A1 RID: 161 RVA: 0x00002E72 File Offset: 0x00001072
	// (remove) Token: 0x060000A2 RID: 162 RVA: 0x00002E8B File Offset: 0x0000108B
	public event DisplayObject.MouseDelegate MouseOutEvent;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060000A3 RID: 163 RVA: 0x00002EA4 File Offset: 0x000010A4
	// (remove) Token: 0x060000A4 RID: 164 RVA: 0x00002EBD File Offset: 0x000010BD
	public event DisplayObject.MouseDelegate MouseDownEvent;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060000A5 RID: 165 RVA: 0x00002ED6 File Offset: 0x000010D6
	// (remove) Token: 0x060000A6 RID: 166 RVA: 0x00002EEF File Offset: 0x000010EF
	public event DisplayObject.MouseDelegate MouseUpEvent;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060000A7 RID: 167 RVA: 0x00002F08 File Offset: 0x00001108
	// (remove) Token: 0x060000A8 RID: 168 RVA: 0x00002F21 File Offset: 0x00001121
	public event DisplayObject.MouseDelegate MouseClickEvent;

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000A9 RID: 169 RVA: 0x00002F3A File Offset: 0x0000113A
	// (set) Token: 0x060000AA RID: 170 RVA: 0x00002F42 File Offset: 0x00001142
	public float childrenAlpha
	{
		get
		{
			return this._childrenAlpha;
		}
		set
		{
			this._childrenAlpha = value;
			DisplayUtils.SetAlphaOfChildren(this, this._childrenAlpha);
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000AB RID: 171 RVA: 0x00002F57 File Offset: 0x00001157
	// (set) Token: 0x060000AC RID: 172 RVA: 0x00002F5F File Offset: 0x0000115F
	public float childrenLightness
	{
		get
		{
			return this._childrenLightness;
		}
		set
		{
			this._childrenLightness = value;
			DisplayUtils.SetLightnessOfChildren(this, this._childrenLightness);
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000AD RID: 173 RVA: 0x00002F74 File Offset: 0x00001174
	// (set) Token: 0x060000AE RID: 174 RVA: 0x00002F7C File Offset: 0x0000117C
	public Color childrenColor
	{
		get
		{
			return this._childrenColor;
		}
		set
		{
			this._childrenColor = value;
			DisplayUtils.SetColorOfChildren(this, this._childrenColor);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000AF RID: 175 RVA: 0x00002F91 File Offset: 0x00001191
	// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002F99 File Offset: 0x00001199
	public bool interactive
	{
		get
		{
			return this._interactive;
		}
		set
		{
			this._interactive = value;
			DisplayUtils.ToggleInteractivity(this, this._interactive);
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000B1 RID: 177 RVA: 0x00002FAE File Offset: 0x000011AE
	public ButtonObject button
	{
		get
		{
			if (this._button == null)
			{
				this._button = this.gameObj.GetComponent<ButtonObject>();
			}
			return this._button;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000B2 RID: 178 RVA: 0x00002FD8 File Offset: 0x000011D8
	public TooltipObject tooltip
	{
		get
		{
			if (this._tooltip == null)
			{
				this._tooltip = this.gameObj.GetComponent<TooltipObject>();
			}
			return this._tooltip;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000E184 File Offset: 0x0000C384
	// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000E1AC File Offset: 0x0000C3AC
	public float localX
	{
		get
		{
			return this.gameObj.transform.localPosition.x;
		}
		set
		{
			this.gameObj.transform.localPosition = new Vector3(value, this.gameObj.transform.localPosition.y, this.gameObj.transform.localPosition.z);
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000E200 File Offset: 0x0000C400
	// (set) Token: 0x060000B6 RID: 182 RVA: 0x0000E228 File Offset: 0x0000C428
	public float localY
	{
		get
		{
			return this.gameObj.transform.localPosition.y;
		}
		set
		{
			this.gameObj.transform.localPosition = new Vector3(this.gameObj.transform.localPosition.x, value, this.gameObj.transform.localPosition.z);
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000E27C File Offset: 0x0000C47C
	// (set) Token: 0x060000B8 RID: 184 RVA: 0x0000E2A4 File Offset: 0x0000C4A4
	public float globalX
	{
		get
		{
			return this.gameObj.transform.position.x;
		}
		set
		{
			this.gameObj.transform.position = new Vector3(value, this.gameObj.transform.position.y, this.gameObj.transform.position.z);
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000E2F8 File Offset: 0x0000C4F8
	// (set) Token: 0x060000BA RID: 186 RVA: 0x0000E320 File Offset: 0x0000C520
	public float globalY
	{
		get
		{
			return this.gameObj.transform.position.y;
		}
		set
		{
			this.gameObj.transform.position = new Vector3(this.gameObj.transform.position.x, value, this.gameObj.transform.position.z);
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00003002 File Offset: 0x00001202
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.OnAwake();
		}
		else
		{
			this.gameObj = base.gameObject;
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000E374 File Offset: 0x0000C574
	protected virtual void OnAwake()
	{
		this.gameObj = base.gameObject;
		this.paused = false;
		this.pausable = true;
		this.hasMasks = false;
		this._childrenAlpha = 1f;
		this._childrenLightness = 1f;
		this._childrenColor = Color.white;
		this._interactive = true;
		this._button = null;
		this._tooltip = null;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00003025 File Offset: 0x00001225
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.OnStart();
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00003037 File Offset: 0x00001237
	protected virtual void OnStart()
	{
		GameManager.Stage.StageStartedEvent += this.OnStageStarted;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00003050 File Offset: 0x00001250
	private void Update()
	{
		if (Application.isPlaying && !this.paused)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000306F File Offset: 0x0000126F
	private void FixedUpdate()
	{
		if (Application.isPlaying && !this.paused)
		{
			this.OnFixedUpdate();
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnFixedUpdate()
	{
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000308C File Offset: 0x0000128C
	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.Destructor();
		}
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
	protected virtual void Destructor()
	{
		if (GameManager.Stage != null)
		{
			GameManager.Stage.StageStartedEvent -= this.OnStageStarted;
		}
		TweenUtils.KillTweener(this._tweenerScale, false);
		this._tweenerScale = null;
		TweenUtils.KillTweener(this._tweenerChildAlpha, false);
		this._tweenerChildAlpha = null;
		TweenUtils.KillTweener(this._tweenerChildLightness, false);
		this._tweenerChildLightness = null;
		DisplayObject parent = this.GetParent();
		if (parent != null)
		{
			parent.RemoveChild(this, false);
		}
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnStageStarted()
	{
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000309E File Offset: 0x0000129E
	public DisplayObject GetParent()
	{
		if (this.gameObj.transform.parent == null)
		{
			return null;
		}
		return this.gameObj.transform.parent.gameObject.GetComponent<DisplayObject>();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000E460 File Offset: 0x0000C660
	public DisplayObject[] GetChildren(bool ordered = false)
	{
		this.hasMasks = false;
		DisplayObject[] array = new DisplayObject[this.gameObj.transform.childCount];
		if (this.gameObj.transform.childCount > 0)
		{
			int i = 0;
			while (i < this.gameObj.transform.childCount)
			{
				GameObject gameObject = this.gameObj.transform.GetChild(i).gameObject;
				DisplayObject displayObject = gameObject.GetComponent<DisplayObject>();
				if (!(displayObject == null))
				{
					goto IL_99;
				}
				if (!(gameObject.GetComponent<NonDisplayObject>() != null))
				{
					displayObject = gameObject.AddComponent<DisplayObject>();
					goto IL_99;
				}
				if (gameObject.GetComponent<Mask>() != null)
				{
					this.hasMasks = true;
				}
				IL_9D:
				i++;
				continue;
				IL_99:
				array[i] = displayObject;
				goto IL_9D;
			}
			array = (from childElement in array
			where childElement != null
			select childElement).ToArray<DisplayObject>();
			if (ordered)
			{
				Array.Sort<DisplayObject>(array, (DisplayObject childA, DisplayObject childB) => childA.childIndex.CompareTo(childB.childIndex));
			}
		}
		return array;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000E578 File Offset: 0x0000C778
	public Mask[] GetMasks()
	{
		Mask[] array = new Mask[this.gameObj.transform.childCount];
		if (this.gameObj.transform.childCount > 0)
		{
			for (int i = 0; i < this.gameObj.transform.childCount; i++)
			{
				Mask component = this.gameObj.transform.GetChild(i).gameObject.GetComponent<Mask>();
				if (component != null)
				{
					array[i] = component;
				}
			}
		}
		return (from maskElement in array
		where maskElement != null
		select maskElement).ToArray<Mask>();
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000E628 File Offset: 0x0000C828
	public DisplayObject GetChildByName(string name)
	{
		DisplayObject[] children = this.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i].name == name)
			{
				return children[i];
			}
		}
		return null;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000E66C File Offset: 0x0000C86C
	public void IndexChildren(bool recursive = false)
	{
		DisplayObject[] children = this.GetChildren(true);
		int num = 0;
		int num2 = 0;
		foreach (DisplayObject displayObject in children)
		{
			if (displayObject.childIndex < 0)
			{
				displayObject.childIndex = children.Length - 1 - num;
				num++;
			}
			else
			{
				displayObject.childIndex = num2;
				num2++;
			}
			if (recursive)
			{
				displayObject.IndexChildren(recursive);
			}
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0000E6DC File Offset: 0x0000C8DC
	public void AddChild(DisplayObject child)
	{
		child.gameObj.SetActive(true);
		child.gameObj.transform.parent = this.gameObj.transform;
		child.childIndex = -1;
		this.IndexChildren(false);
		if (this.pausable && !child.IsPausable())
		{
			DisplayUtils.SetPausable(child, true);
		}
		else if (!this.pausable && child.IsPausable())
		{
			DisplayUtils.SetPausable(child, false);
		}
		GameManager.Stage.SetDirty();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000E768 File Offset: 0x0000C968
	public void RemoveChild(DisplayObject child, bool destroy = false)
	{
		child.gameObj.transform.parent = null;
		child.childIndex = -1;
		if (destroy)
		{
			UnityEngine.Object.Destroy(child.gameObj);
		}
		else
		{
			child.gameObj.SetActive(false);
		}
		this.IndexChildren(false);
		GameManager.Stage.SetDirty();
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
	public void RemoveAllChildren(bool destroy = false)
	{
		DisplayObject[] children = this.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			this.RemoveChild(children[i], destroy);
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0000E7F4 File Offset: 0x0000C9F4
	public void RemoveSelf()
	{
		DisplayObject parent = this.GetParent();
		if (parent == null)
		{
			return;
		}
		parent.RemoveChild(this, false);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000E820 File Offset: 0x0000CA20
	public void ShiftChild(DisplayObject child, int indexShift)
	{
		indexShift = Mathf.Clamp(indexShift, child.childIndex * -1, this.gameObj.transform.childCount - 1 - child.childIndex);
		if (indexShift == 0)
		{
			return;
		}
		DisplayObject[] children = this.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			if (!(children[i] == child))
			{
				if (indexShift > 0 && children[i].childIndex > child.childIndex && children[i].childIndex <= child.childIndex + indexShift)
				{
					children[i].childIndex--;
				}
				else if (indexShift < 0 && children[i].childIndex < child.childIndex && children[i].childIndex >= child.childIndex + indexShift)
				{
					children[i].childIndex++;
				}
			}
		}
		child.childIndex += indexShift;
		GameManager.Stage.SetDirty();
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0000E928 File Offset: 0x0000CB28
	public void ShiftSelf(int indexShift)
	{
		DisplayObject parent = this.GetParent();
		if (parent != null)
		{
			parent.ShiftChild(this, indexShift);
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0000E950 File Offset: 0x0000CB50
	public void SetOwnChildIndex(int newChildIndex)
	{
		DisplayObject parent = this.GetParent();
		if (parent == null)
		{
			return;
		}
		newChildIndex = Mathf.Clamp(newChildIndex, 0, parent.gameObj.transform.childCount - 1);
		this.ShiftSelf(newChildIndex - this.childIndex);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000E99C File Offset: 0x0000CB9C
	public void ShiftSelfToTop()
	{
		DisplayObject parent = this.GetParent();
		if (parent == null)
		{
			return;
		}
		this.childIndex = -1;
		parent.IndexChildren(false);
		GameManager.Stage.SetDirty();
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
	public void SetLocalPosition(float x, float y)
	{
		this.gameObj.transform.localPosition = new Vector3(x, y, this.gameObj.transform.localPosition.z);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000EA14 File Offset: 0x0000CC14
	public void SetGlobalPosition(float x, float y)
	{
		this.gameObj.transform.position = new Vector3(x, y, this.gameObj.transform.position.z);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000EA50 File Offset: 0x0000CC50
	public void SetLocalScale(float scale, float duration = 0f, EaseType easeType = EaseType.Linear)
	{
		TweenUtils.KillTweener(this._tweenerScale, false);
		if (duration <= 0f)
		{
			this.gameObj.transform.localScale = new Vector3(scale, scale, 1f);
		}
		else
		{
			this._tweenerScale = HOTween.To(this.gameObj.transform, duration, new TweenParms().Prop("localScale", new Vector3(scale, scale, 1f)).Ease(easeType));
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
	public void SetChildAlpha(float alpha, float duration = 0f)
	{
		TweenUtils.KillTweener(this._tweenerChildAlpha, false);
		if (duration <= 0f)
		{
			this.childrenAlpha = alpha;
		}
		else
		{
			this._tweenerChildAlpha = HOTween.To(this, duration, new TweenParms().Prop("childrenAlpha", alpha).Ease(EaseType.Linear));
		}
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000EB2C File Offset: 0x0000CD2C
	public void SetChildLightness(float lightness, float duration = 0f)
	{
		TweenUtils.KillTweener(this._tweenerChildLightness, false);
		if (duration <= 0f)
		{
			this.childrenLightness = lightness;
		}
		else
		{
			this._tweenerChildLightness = HOTween.To(this, duration, new TweenParms().Prop("childrenLightness", lightness).Ease(EaseType.Linear));
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000030D7 File Offset: 0x000012D7
	public bool IsPaused()
	{
		return this.paused;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x000030DF File Offset: 0x000012DF
	public bool IsPausable()
	{
		return this.pausable;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000030E7 File Offset: 0x000012E7
	public void SetPausable(bool canPause)
	{
		this.pausable = canPause;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x0000EB84 File Offset: 0x0000CD84
	public bool IsRecursivelyInteractive()
	{
		if (!this.interactive)
		{
			return false;
		}
		DisplayObject parent = this.GetParent();
		while (parent != null)
		{
			if (!parent.interactive)
			{
				return false;
			}
			parent = parent.GetParent();
		}
		return true;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000030F0 File Offset: 0x000012F0
	public virtual bool CanShowTooltip()
	{
		return !(this.tooltip == null);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00003106 File Offset: 0x00001306
	public virtual string GetUniqueTooltipMessage()
	{
		return string.Empty;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000EBCC File Offset: 0x0000CDCC
	public virtual void Pause()
	{
		if (this.paused || !this.pausable || !this.gameObj.activeSelf)
		{
			return;
		}
		this.paused = true;
		if (this._tweenerScale != null && this._tweenerScale.IsTweening(this.gameObj.transform))
		{
			this._tweenerScale.Pause();
		}
		if (this._tweenerChildAlpha != null && this._tweenerChildAlpha.IsTweening(this))
		{
			this._tweenerChildAlpha.Pause();
		}
		if (this._tweenerChildLightness != null && this._tweenerChildLightness.IsTweening(this))
		{
			this._tweenerChildLightness.Pause();
		}
		if (this.rigidBody != null)
		{
			this._pauseVelocity = this.rigidBody.velocity;
			this._pauseAngularVelocity = this.rigidBody.angularVelocity;
			this.rigidBody.velocity = Vector3.zero;
			this.rigidBody.angularVelocity = Vector3.zero;
		}
		if (this.button != null)
		{
			this.button.Pause();
		}
		if (this.tooltip != null)
		{
			this.tooltip.Pause();
		}
		DisplayObject[] children = this.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			children[i].Pause();
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000ED38 File Offset: 0x0000CF38
	public virtual void Unpause()
	{
		if (!this.paused || !this.pausable || !this.gameObj.activeSelf)
		{
			return;
		}
		this.paused = false;
		if (this._tweenerScale != null && this._tweenerScale.isPaused)
		{
			this._tweenerScale.Play();
		}
		if (this._tweenerChildAlpha != null && this._tweenerChildAlpha.isPaused)
		{
			this._tweenerChildAlpha.Play();
		}
		if (this._tweenerChildLightness != null && this._tweenerChildLightness.isPaused)
		{
			this._tweenerChildLightness.Play();
		}
		if (this.rigidBody != null)
		{
			this.rigidBody.velocity = this._pauseVelocity;
			this.rigidBody.angularVelocity = this._pauseAngularVelocity;
		}
		if (this.button != null && this.gameObj.activeSelf)
		{
			this.button.Unpause();
		}
		if (this.tooltip != null)
		{
			this.tooltip.Unpause();
		}
		DisplayObject[] children = this.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			children[i].Unpause();
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000310D File Offset: 0x0000130D
	public bool IsMouseOver()
	{
		return GameManager.System.Cursor.IsMouseTarget(this);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000311F File Offset: 0x0000131F
	public void MouseOver()
	{
		if (this.MouseOverEvent != null && (!this.paused || this == GameManager.Stage))
		{
			this.MouseOverEvent(this);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00003153 File Offset: 0x00001353
	public void MouseOut()
	{
		if (this.MouseOutEvent != null && (!this.paused || this == GameManager.Stage))
		{
			this.MouseOutEvent(this);
		}
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00003187 File Offset: 0x00001387
	public void MouseDown()
	{
		if (this.MouseDownEvent != null && (!this.paused || this == GameManager.Stage))
		{
			this.MouseDownEvent(this);
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000031BB File Offset: 0x000013BB
	public void MouseUp()
	{
		if (this.MouseUpEvent != null && (!this.paused || this == GameManager.Stage))
		{
			this.MouseUpEvent(this);
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000031EF File Offset: 0x000013EF
	public void MouseClick()
	{
		if (this.MouseClickEvent != null && (!this.paused || this == GameManager.Stage))
		{
			this.MouseClickEvent(this);
		}
	}

	// Token: 0x04000080 RID: 128
	public GameObject gameObj;

	// Token: 0x04000081 RID: 129
	public int childIndex = -1;

	// Token: 0x04000082 RID: 130
	protected bool paused;

	// Token: 0x04000083 RID: 131
	protected bool pausable;

	// Token: 0x04000084 RID: 132
	public bool hasMasks;

	// Token: 0x04000085 RID: 133
	protected Rigidbody rigidBody;

	// Token: 0x04000086 RID: 134
	private Vector3 _pauseVelocity;

	// Token: 0x04000087 RID: 135
	private Vector3 _pauseAngularVelocity;

	// Token: 0x04000088 RID: 136
	private Tweener _tweenerScale;

	// Token: 0x04000089 RID: 137
	private Tweener _tweenerChildAlpha;

	// Token: 0x0400008A RID: 138
	private Tweener _tweenerChildLightness;

	// Token: 0x0400008B RID: 139
	private float _childrenAlpha;

	// Token: 0x0400008C RID: 140
	private float _childrenLightness;

	// Token: 0x0400008D RID: 141
	private Color _childrenColor;

	// Token: 0x0400008E RID: 142
	private bool _interactive;

	// Token: 0x0400008F RID: 143
	private ButtonObject _button;

	// Token: 0x04000090 RID: 144
	private TooltipObject _tooltip;

	// Token: 0x02000017 RID: 23
	// (Invoke) Token: 0x060000EA RID: 234
	public delegate void MouseDelegate(DisplayObject displayObject);
}
