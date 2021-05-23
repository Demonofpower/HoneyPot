using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B9 RID: 441
[AddComponentMenu("2D Toolkit/UI/tk2dUITweenItem")]
public class tk2dUITweenItem : tk2dUIBaseItemControl
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0000AE5A File Offset: 0x0000905A
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0000AE62 File Offset: 0x00009062
	private void Awake()
	{
		this.onUpScale = base.transform.localScale;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0004B4A0 File Offset: 0x000496A0
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease += this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp += this.ButtonUp;
				}
			}
		}
		this.internalTweenInProgress = false;
		this.tweenTimeElapsed = 0f;
		base.transform.localScale = this.onUpScale;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0004B540 File Offset: 0x00049740
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease -= this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp -= this.ButtonUp;
				}
			}
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0004B5C0 File Offset: 0x000497C0
	private void ButtonDown()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onDownScale;
		}
		else
		{
			base.transform.localScale = this.onUpScale;
			this.tweenTargetScale = this.onDownScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0004B640 File Offset: 0x00049840
	private void ButtonUp()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onUpScale;
		}
		else
		{
			this.tweenTargetScale = this.onUpScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0004B6B0 File Offset: 0x000498B0
	private IEnumerator ScaleTween()
	{
		this.tweenTimeElapsed = 0f;
		while (this.tweenTimeElapsed < this.tweenDuration)
		{
			base.transform.localScale = Vector3.Lerp(this.tweenStartingScale, this.tweenTargetScale, this.tweenTimeElapsed / this.tweenDuration);
			yield return null;
			this.tweenTimeElapsed += tk2dUITime.deltaTime;
		}
		base.transform.localScale = this.tweenTargetScale;
		this.internalTweenInProgress = false;
		if (!this.canButtonBeHeldDown)
		{
			if (this.tweenDuration <= 0f)
			{
				base.transform.localScale = this.onUpScale;
			}
			else
			{
				this.tweenTargetScale = this.onUpScale;
				this.tweenStartingScale = base.transform.localScale;
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
		yield break;
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0000AE75 File Offset: 0x00009075
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04000C44 RID: 3140
	private Vector3 onUpScale;

	// Token: 0x04000C45 RID: 3141
	public Vector3 onDownScale = new Vector3(0.9f, 0.9f, 0.9f);

	// Token: 0x04000C46 RID: 3142
	public float tweenDuration = 0.1f;

	// Token: 0x04000C47 RID: 3143
	public bool canButtonBeHeldDown = true;

	// Token: 0x04000C48 RID: 3144
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x04000C49 RID: 3145
	private bool internalTweenInProgress;

	// Token: 0x04000C4A RID: 3146
	private Vector3 tweenTargetScale = Vector3.one;

	// Token: 0x04000C4B RID: 3147
	private Vector3 tweenStartingScale = Vector3.one;

	// Token: 0x04000C4C RID: 3148
	private float tweenTimeElapsed;
}
