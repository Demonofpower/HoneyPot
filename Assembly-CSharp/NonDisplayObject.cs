using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
public class NonDisplayObject : MonoBehaviour
{
	// Token: 0x06000140 RID: 320 RVA: 0x00003614 File Offset: 0x00001814
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.OnAwake();
		}
		else
		{
			this.OnEditorUpdate();
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00003631 File Offset: 0x00001831
	protected virtual void OnAwake()
	{
		this.gameObj = base.gameObject;
		this.paused = false;
		this.pausable = true;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000364D File Offset: 0x0000184D
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.OnStart();
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnStart()
	{
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000365F File Offset: 0x0000185F
	private void Update()
	{
		if (!this.paused)
		{
			if (Application.isPlaying)
			{
				this.OnUpdate();
			}
			else
			{
				this.OnEditorUpdate();
			}
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00003687 File Offset: 0x00001887
	private void FixedUpdate()
	{
		if (Application.isPlaying && !this.paused)
		{
			this.OnFixedUpdate();
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void OnFixedUpdate()
	{
	}

	// Token: 0x06000148 RID: 328 RVA: 0x000036A4 File Offset: 0x000018A4
	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.Destroy();
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void Destroy()
	{
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000036B6 File Offset: 0x000018B6
	protected virtual void OnEditorUpdate()
	{
		if (this.gameObj == null)
		{
			this.gameObj = base.gameObject;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000036D5 File Offset: 0x000018D5
	public bool IsPaused()
	{
		return this.paused;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000036DD File Offset: 0x000018DD
	public bool IsPausable()
	{
		return this.pausable;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000036E5 File Offset: 0x000018E5
	public void SetPausable(bool canPause)
	{
		this.pausable = canPause;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0001182C File Offset: 0x0000FA2C
	public virtual void Pause()
	{
		if (this.paused || !this.pausable || !this.gameObj.activeSelf)
		{
			return;
		}
		this.paused = true;
		if (this.rigidBody != null)
		{
			this._pauseVelocity = this.rigidBody.velocity;
			this._pauseAngularVelocity = this.rigidBody.angularVelocity;
			this.rigidBody.velocity = Vector3.zero;
			this.rigidBody.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x000118BC File Offset: 0x0000FABC
	public virtual void Unpause()
	{
		if (!this.paused || !this.pausable || !this.gameObj.activeSelf)
		{
			return;
		}
		this.paused = false;
		if (this.rigidBody != null)
		{
			this.rigidBody.velocity = this._pauseVelocity;
			this.rigidBody.angularVelocity = this._pauseAngularVelocity;
		}
	}

	// Token: 0x040000F2 RID: 242
	public GameObject gameObj;

	// Token: 0x040000F3 RID: 243
	protected bool paused;

	// Token: 0x040000F4 RID: 244
	protected bool pausable;

	// Token: 0x040000F5 RID: 245
	protected Rigidbody rigidBody;

	// Token: 0x040000F6 RID: 246
	private Vector3 _pauseVelocity;

	// Token: 0x040000F7 RID: 247
	private Vector3 _pauseAngularVelocity;
}
