using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class CursorManager : MonoBehaviour
{
	// Token: 0x06000618 RID: 1560 RVA: 0x00006953 File Offset: 0x00004B53
	private void Start()
	{
		this._previousMousePosition = Vector3.zero;
		this._mouseDelta = Vector3.zero;
		this._attachedObjects = new List<CursorAttachedObject>();
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002ED68 File Offset: 0x0002CF68
	private void Update()
	{
		Vector3 mousePosition = this.GetMousePosition();
		for (int i = 0; i < this._attachedObjects.Count; i++)
		{
			CursorAttachedObject cursorAttachedObject = this._attachedObjects[i];
			cursorAttachedObject.displayObject.gameObj.transform.position = new Vector3(mousePosition.x + cursorAttachedObject.xOffset, mousePosition.y + cursorAttachedObject.yOffset, cursorAttachedObject.displayObject.gameObj.transform.position.z);
		}
		DisplayObject displayObject = this.FindMouseTarget(mousePosition);
		if (displayObject != this._mouseTarget)
		{
			if (this._mouseTarget != null)
			{
				this._mouseTarget.MouseOut();
				if (this._mouseTarget == this._mouseDownTarget)
				{
					this._mouseDownTarget = null;
				}
			}
			this._mouseTarget = displayObject;
			if (this._mouseTarget != null)
			{
				this._mouseTarget.MouseOver();
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (this._mouseTarget != null)
			{
				this._mouseTarget.MouseDown();
				this._mouseDownTarget = this._mouseTarget;
			}
			GameManager.Stage.MouseDown();
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (this._mouseTarget != null)
			{
				this._mouseTarget.MouseUp();
				if (this._mouseTarget == this._mouseDownTarget)
				{
					this._mouseTarget.MouseClick();
				}
			}
			GameManager.Stage.MouseUp();
			this._mouseDownTarget = null;
		}
		this._mouseDelta = mousePosition - this._previousMousePosition;
		this._previousMousePosition = mousePosition;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002EF20 File Offset: 0x0002D120
	public void AttachObject(DisplayObject displayObject, float xOffset = 0f, float yOffset = 0f)
	{
		if (this.GetAttachedObject(displayObject) == null)
		{
			Vector3 mousePosition = this.GetMousePosition();
			CursorAttachedObject cursorAttachedObject = new CursorAttachedObject(displayObject, xOffset, yOffset);
			this._attachedObjects.Add(cursorAttachedObject);
			cursorAttachedObject.displayObject.gameObj.transform.position = new Vector3(mousePosition.x + cursorAttachedObject.xOffset, mousePosition.y + cursorAttachedObject.yOffset, cursorAttachedObject.displayObject.gameObj.transform.position.z);
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0002EFA8 File Offset: 0x0002D1A8
	public void DetachObject(DisplayObject displayObject)
	{
		CursorAttachedObject attachedObject = this.GetAttachedObject(displayObject);
		if (attachedObject != null)
		{
			this._attachedObjects.Remove(attachedObject);
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0002EFD0 File Offset: 0x0002D1D0
	private CursorAttachedObject GetAttachedObject(DisplayObject displayObject)
	{
		for (int i = 0; i < this._attachedObjects.Count; i++)
		{
			if (this._attachedObjects[i].displayObject == displayObject)
			{
				return this._attachedObjects[i];
			}
		}
		return null;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00006976 File Offset: 0x00004B76
	public Vector3 GetMousePosition()
	{
		return GameManager.System.gameCamera.renderCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00006991 File Offset: 0x00004B91
	public Vector3 GetMouseDelta()
	{
		return this._mouseDelta;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0002F024 File Offset: 0x0002D224
	public DisplayObject FindMouseTarget(Vector3 mousePosition)
	{
		RaycastHit[] array = Physics.RaycastAll(mousePosition, Vector3.forward);
		if (array.Length > 0)
		{
			Collider collider = null;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.enabled && (this._mouseEventAbsorber == null || (!this._absorberExclusive && array[i].collider.transform.position.z <= this._mouseEventAbsorber.transform.position.z) || (this._absorberExclusive && array[i].collider == this._mouseEventAbsorber.collider)) && (collider == null || array[i].collider.transform.position.z < collider.transform.position.z))
				{
					collider = array[i].collider;
				}
			}
			if (collider != null)
			{
				return collider.gameObject.GetComponent<DisplayObject>();
			}
		}
		return null;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00006999 File Offset: 0x00004B99
	public bool IsMouseTarget(DisplayObject displayObject)
	{
		return this._mouseTarget == displayObject;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x000069A7 File Offset: 0x00004BA7
	public bool IsMouseDownTarget(DisplayObject displayObject)
	{
		return this._mouseDownTarget == displayObject;
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x000069B5 File Offset: 0x00004BB5
	public DisplayObject GetMouseTarget()
	{
		return this._mouseTarget;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x000069BD File Offset: 0x00004BBD
	public void SetAbsorber(DisplayObject absorber, bool exclusive = false)
	{
		this._mouseEventAbsorber = absorber;
		this._absorberExclusive = exclusive;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000069CD File Offset: 0x00004BCD
	public void ClearAbsorber()
	{
		this._mouseEventAbsorber = null;
		this._absorberExclusive = false;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002F160 File Offset: 0x0002D360
	public void ShowCursorRippleEffect(DisplayObject source)
	{
		if ((source.gameObj.transform.position.z < GameManager.Stage.cellPhone.gameObj.transform.position.z && source.gameObj.transform.position.z > GameManager.Stage.uiTitle.gameObj.transform.position.z) || source.gameObj.transform.position.z < GameManager.Stage.uiPhotoGallery.gameObj.transform.position.z)
		{
			return;
		}
		ParticleEmitter2D component = new GameObject("CursorRippleParticleEmitter", new Type[]
		{
			typeof(ParticleEmitter2D)
		}).GetComponent<ParticleEmitter2D>();
		GameManager.Stage.effects.AddParticleEffect(component, source);
		component.Init(GameManager.Data.Particles.Get(3), GameManager.Data.SpriteGroups.Get(8), false);
		Vector3 mousePosition = this.GetMousePosition();
		component.SetGlobalPosition(mousePosition.x, mousePosition.y);
	}

	// Token: 0x04000779 RID: 1913
	public const string CURSOR_DEFAULT = "cursor_default";

	// Token: 0x0400077A RID: 1914
	public const string CURSOR_TARGET = "cursor_target";

	// Token: 0x0400077B RID: 1915
	private const int CURSOR_RIPPLE_OUT_PARTICLE_EMITTER_ID = 3;

	// Token: 0x0400077C RID: 1916
	private const int CURSOR_RIPPLE_IN_PARTICLE_EMITTER_ID = 4;

	// Token: 0x0400077D RID: 1917
	private const int CURSOR_RIPPLE_SPRITE_GROUP_ID = 8;

	// Token: 0x0400077E RID: 1918
	private DisplayObject _mouseTarget;

	// Token: 0x0400077F RID: 1919
	private DisplayObject _mouseDownTarget;

	// Token: 0x04000780 RID: 1920
	private DisplayObject _mouseEventAbsorber;

	// Token: 0x04000781 RID: 1921
	private bool _absorberExclusive;

	// Token: 0x04000782 RID: 1922
	private Vector3 _previousMousePosition;

	// Token: 0x04000783 RID: 1923
	private Vector3 _mouseDelta;

	// Token: 0x04000784 RID: 1924
	private List<CursorAttachedObject> _attachedObjects;
}
