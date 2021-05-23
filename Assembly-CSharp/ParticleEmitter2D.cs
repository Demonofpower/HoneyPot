using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class ParticleEmitter2D : DisplayObject
{
	// Token: 0x0600015B RID: 347 RVA: 0x00011EF0 File Offset: 0x000100F0
	protected override void OnStart()
	{
		if (this.definition != null && this.spriteGroup != null && !this._inited)
		{
			this.Init(this.definition, this.spriteGroup, true);
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00011F40 File Offset: 0x00010140
	public void Init(ParticleEmitter2DDefinition definition, SpriteGroupDefinition spriteGroup, bool canPauseEmitter = true)
	{
		this.definition = definition;
		this.spriteGroup = spriteGroup;
		this.details = new ParticleEmitter2DDynamicDetails(definition);
		base.SetPausable(canPauseEmitter);
		this.gameObj.transform.localEulerAngles = new Vector3(0f, 0f, definition.initialDirection);
		this._inited = true;
		this._enabled = true;
		this._initTimestamp = GameManager.System.Lifetime(this.pausable);
		this._spawnTimestamp = GameManager.System.Lifetime(this.pausable);
		this._particleDelay = -1f;
		this._totalParticleCount = 0;
		this._complete = false;
		if (this.pausable && GameManager.System.IsPaused())
		{
			this.Pause();
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00012008 File Offset: 0x00010208
	protected override void OnUpdate()
	{
		if (!this._inited || !this._enabled)
		{
			return;
		}
		if (this._complete)
		{
			if (this._particles.Count == 0)
			{
				UnityEngine.Object.Destroy(this.gameObj);
			}
			return;
		}
		if (GameManager.System.Lifetime(this.pausable) - this._spawnTimestamp > this._particleDelay)
		{
			if (this.definition.particleMultiplier > 1)
			{
				for (int i = 0; i < this.definition.particleMultiplier; i++)
				{
					this.SpawnParticle();
				}
			}
			else
			{
				this.SpawnParticle();
			}
			this._spawnTimestamp = GameManager.System.Lifetime(this.pausable);
			this._particleDelay = UnityEngine.Random.Range(this.definition.particleDelay - this.definition.particleDelayVariance / 2f, this.definition.particleDelay + this.definition.particleDelayVariance / 2f);
			this._totalParticleCount++;
		}
		if (this.definition.duration > 0f && GameManager.System.Lifetime(this.pausable) - this._initTimestamp > this.definition.duration)
		{
			this._complete = true;
		}
		if (this.definition.particleLimit > 0f && (float)this._totalParticleCount >= this.definition.particleLimit)
		{
			this._complete = true;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00012194 File Offset: 0x00010394
	private void SpawnParticle()
	{
		Particle2D particle2D = new GameObject("Particle", new Type[]
		{
			typeof(Particle2D)
		}).GetComponent("Particle2D") as Particle2D;
		Vector3 vector = new Vector3(this.gameObj.transform.position.x, this.gameObj.transform.position.y, this.gameObj.transform.position.z);
		if (this.definition.originSpreadType == ParticleEmitter2DOriginSpreadType.RECTANGULAR)
		{
			if (this.details.originSpreadX > 0f)
			{
				vector += Vector3.right * UnityEngine.Random.Range(-this.details.originSpreadX, this.details.originSpreadX);
			}
			if (this.details.originSpreadY > 0f)
			{
				vector += Vector3.up * UnityEngine.Random.Range(-this.details.originSpreadY, this.details.originSpreadY);
			}
		}
		else if (this.definition.originSpreadType == ParticleEmitter2DOriginSpreadType.CIRCULAR)
		{
			float f = UnityEngine.Random.Range(0f, 360f) * 0.017453292f;
			Vector3 vector2 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
			vector2.Normalize();
			vector2 *= UnityEngine.Random.Range(1f, Mathf.Max(this.details.originSpreadRadius, 1f));
			vector += vector2;
		}
		particle2D.gameObj.transform.position = vector;
		if (this.definition.particlesAttached)
		{
			particle2D.gameObj.transform.parent = base.transform;
		}
		else if (base.transform.parent != null)
		{
			particle2D.gameObj.transform.parent = base.transform.parent.transform;
		}
		this._particles.Add(particle2D);
		particle2D.ParticleCompleteEvent += this.OnParticleComplete;
		particle2D.Init(this.definition, this.spriteGroup, this.gameObj.transform.localEulerAngles.z, this.pausable);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000376B File Offset: 0x0000196B
	private void OnParticleComplete(Particle2D particle)
	{
		particle.ParticleCompleteEvent -= this.OnParticleComplete;
		if (this._particles.IndexOf(particle) != -1)
		{
			this._particles.Remove(particle);
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000379E File Offset: 0x0000199E
	public void Enable()
	{
		if (this._enabled)
		{
			return;
		}
		this._enabled = true;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x000037B3 File Offset: 0x000019B3
	public void Disable()
	{
		if (!this._enabled)
		{
			return;
		}
		this._enabled = false;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x000037C8 File Offset: 0x000019C8
	public void ForceComplete()
	{
		this._complete = true;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000123F0 File Offset: 0x000105F0
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		for (int i = 0; i < this._particles.Count; i++)
		{
			this._particles[i].Pause();
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0001243C File Offset: 0x0001063C
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		for (int i = 0; i < this._particles.Count; i++)
		{
			this._particles[i].Unpause();
		}
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00012488 File Offset: 0x00010688
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._particles.Count; i++)
		{
			this._particles[i].ParticleCompleteEvent -= this.OnParticleComplete;
		}
		this._particles.Clear();
	}

	// Token: 0x04000104 RID: 260
	public ParticleEmitter2DDefinition definition;

	// Token: 0x04000105 RID: 261
	public SpriteGroupDefinition spriteGroup;

	// Token: 0x04000106 RID: 262
	public ParticleEmitter2DDynamicDetails details;

	// Token: 0x04000107 RID: 263
	private bool _inited;

	// Token: 0x04000108 RID: 264
	private bool _enabled;

	// Token: 0x04000109 RID: 265
	private float _initTimestamp;

	// Token: 0x0400010A RID: 266
	private float _spawnTimestamp;

	// Token: 0x0400010B RID: 267
	private float _particleDelay;

	// Token: 0x0400010C RID: 268
	private int _totalParticleCount;

	// Token: 0x0400010D RID: 269
	private bool _complete;

	// Token: 0x0400010E RID: 270
	private List<Particle2D> _particles = new List<Particle2D>();
}
