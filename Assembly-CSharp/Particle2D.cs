using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class Particle2D : NonDisplayObject
{
	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000151 RID: 337 RVA: 0x000036EE File Offset: 0x000018EE
	// (remove) Token: 0x06000152 RID: 338 RVA: 0x00003707 File Offset: 0x00001907
	public event Particle2D.Particle2DDelegate ParticleCompleteEvent;

	// Token: 0x06000153 RID: 339 RVA: 0x0001192C File Offset: 0x0000FB2C
	public void Init(ParticleEmitter2DDefinition definition, SpriteGroupDefinition spriteGroup, float direction, bool canPauseParticle)
	{
		this._definition = definition;
		base.SetPausable(canPauseParticle);
		this._sprite = this.gameObj.AddComponent<tk2dSprite>();
		this._sprite.SetSprite(spriteGroup.spriteCollection, spriteGroup.spriteCollection.GetSpriteIdByName(spriteGroup.sprites[UnityEngine.Random.Range(0, spriteGroup.sprites.Count)]));
		this.rigidBody = this.gameObj.AddComponent<Rigidbody>();
		this.rigidBody.mass = 1f;
		this.rigidBody.drag = this._definition.particleMass;
		this.rigidBody.angularDrag = this._definition.particleMass;
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = false;
		this.rigidBody.interpolation = RigidbodyInterpolation.None;
		this.rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		this.rigidBody.constraints = (RigidbodyConstraints)56;
		float angle = direction + UnityEngine.Random.Range(this._definition.directionalRange / 2f * -1f, this._definition.directionalRange / 2f);
		float f = MathUtils.NormalizeDegreeAngle(angle) * 0.017453292f;
		Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
		vector.Normalize();
		vector *= UnityEngine.Random.Range(this._definition.force - this._definition.forceVariance / 2f, this._definition.force + this._definition.forceVariance / 2f) * 10f;
		this.rigidBody.velocity = vector;
		float num = UnityEngine.Random.Range(this._definition.torque - this._definition.torqueVariance / 2f, this._definition.torque + this._definition.torqueVariance / 2f);
		if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
		{
			num *= -1f;
		}
		Vector3 angularVelocity = new Vector3(0f, 0f, num) * 10f;
		this.rigidBody.angularVelocity = angularVelocity;
		this._particleLifetime = UnityEngine.Random.Range(this._definition.particleLifetime - this._definition.particleLifetimeVariance / 2f, this._definition.particleLifetime + this._definition.particleLifetimeVariance / 2f);
		this._startAlpha = UnityEngine.Random.Range(this._definition.startAlpha - this._definition.startAlphaVariance / 2f, this._definition.startAlpha + this._definition.startAlphaVariance / 2f);
		this._endAlpha = UnityEngine.Random.Range(this._definition.endAlpha - this._definition.endAlphaVariance / 2f, this._definition.endAlpha + this._definition.endAlphaVariance / 2f);
		this._alphaDelay = UnityEngine.Random.Range(this._definition.alphaDelay - this._definition.alphaDelayVariance / 2f, this._definition.alphaDelay + this._definition.alphaDelayVariance / 2f);
		this._startScale = UnityEngine.Random.Range(this._definition.startScale - this._definition.startScaleVariance / 2f, this._definition.startScale + this._definition.startScaleVariance / 2f);
		this._endScale = UnityEngine.Random.Range(this._definition.endScale - this._definition.endScaleVariance / 2f, this._definition.endScale + this._definition.endScaleVariance / 2f);
		this._scaleDelay = UnityEngine.Random.Range(this._definition.scaleDelay - this._definition.scaleDelayVariance / 2f, this._definition.scaleDelay + this._definition.scaleDelayVariance / 2f);
		this._inited = true;
		this._initTimestamp = GameManager.System.Lifetime(this.pausable);
		this.OnUpdate();
		if (this.pausable && GameManager.System.IsPaused())
		{
			this.Pause();
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00011D80 File Offset: 0x0000FF80
	protected override void OnUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		float num = (GameManager.System.Lifetime(this.pausable) - this._initTimestamp) / this._particleLifetime;
		if (num >= 1f)
		{
			if (this.ParticleCompleteEvent != null)
			{
				this.ParticleCompleteEvent(this);
			}
			UnityEngine.Object.Destroy(this.gameObj);
		}
		else
		{
			float a;
			if (this._alphaDelay > 0f)
			{
				if (num > this._alphaDelay)
				{
					a = Mathf.Lerp(this._startAlpha, this._endAlpha, num / this._alphaDelay - 1f);
				}
				else
				{
					a = this._startAlpha;
				}
			}
			else
			{
				a = Mathf.Lerp(this._startAlpha, this._endAlpha, num);
			}
			this._sprite.color = new Color(1f, 1f, 1f, a);
			float num2;
			if (this._scaleDelay > 0f)
			{
				if (num > this._scaleDelay)
				{
					num2 = Mathf.Lerp(this._startScale, this._endScale, num / this._scaleDelay - 1f);
				}
				else
				{
					num2 = this._startScale;
				}
			}
			else
			{
				num2 = Mathf.Lerp(this._startScale, this._endScale, num);
			}
			this._sprite.scale = new Vector3(num2, num2, 1f);
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00003720 File Offset: 0x00001920
	protected override void OnFixedUpdate()
	{
		if (!this._inited)
		{
			return;
		}
		this.rigidBody.AddForce(Vector3.down * this._definition.gravity * 100f);
	}

	// Token: 0x040000F8 RID: 248
	private ParticleEmitter2DDefinition _definition;

	// Token: 0x040000F9 RID: 249
	private tk2dSprite _sprite;

	// Token: 0x040000FA RID: 250
	private bool _inited;

	// Token: 0x040000FB RID: 251
	private float _initTimestamp;

	// Token: 0x040000FC RID: 252
	private float _particleLifetime;

	// Token: 0x040000FD RID: 253
	private float _startAlpha;

	// Token: 0x040000FE RID: 254
	private float _endAlpha;

	// Token: 0x040000FF RID: 255
	private float _alphaDelay;

	// Token: 0x04000100 RID: 256
	private float _startScale;

	// Token: 0x04000101 RID: 257
	private float _endScale;

	// Token: 0x04000102 RID: 258
	private float _scaleDelay;

	// Token: 0x02000021 RID: 33
	// (Invoke) Token: 0x06000157 RID: 343
	public delegate void Particle2DDelegate(Particle2D particle);
}
