using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class EnergyTrail : MonoBehaviour
{
	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060004F7 RID: 1271 RVA: 0x00005DDF File Offset: 0x00003FDF
	// (remove) Token: 0x060004F8 RID: 1272 RVA: 0x00005DF8 File Offset: 0x00003FF8
	public event EnergyTrail.EnergyTrailDelegate EnergyTrailCompleteEvent;

	// Token: 0x060004F9 RID: 1273 RVA: 0x000259E8 File Offset: 0x00023BE8
	public void Init(Vector3 origin, EnergyTrailDefinition definition, string popText = null, EnergyTrailFormat trailFormat = EnergyTrailFormat.FULL, EnergyTrailDynamicDetails dynamicDetails = null)
	{
		this._definition = definition;
		this._popText = popText;
		this._trailFormat = trailFormat;
		this._dynamicDetails = dynamicDetails;
		if (this._dynamicDetails == null)
		{
			this._dynamicDetails = new EnergyTrailDynamicDetails(this._definition);
		}
		this._energySurgeOverride = null;
		this._startPosition = origin;
		this._endPosition = new Vector3(this._dynamicDetails.energyTrailDestX + UnityEngine.Random.Range(-(this._dynamicDetails.energyTrailDestWidth / 2f), this._dynamicDetails.energyTrailDestWidth / 2f), this._dynamicDetails.energyTrailDestY + UnityEngine.Random.Range(-(this._dynamicDetails.energyTrailDestHeight / 2f), this._dynamicDetails.energyTrailDestHeight / 2f), 0f);
		if (this._trailFormat == EnergyTrailFormat.END)
		{
			this._endPosition = this._startPosition;
		}
		this._duration = UnityEngine.Random.Range(0.5f, 1f);
		this._forceComplete = false;
		if (this._trailFormat != EnergyTrailFormat.END)
		{
			if (this._definition.energyBurstSpriteGroup != null)
			{
				this._burstEmitter = new GameObject("EnergyBurstParticleEmitter", new Type[]
				{
					typeof(ParticleEmitter2D)
				}).GetComponent<ParticleEmitter2D>();
				GameManager.Stage.effects.AddParticleEffect(this._burstEmitter, GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer);
				this._burstEmitter.Init(GameManager.Data.Particles.Get(8), this._definition.energyBurstSpriteGroup, true);
				this._burstEmitter.SetGlobalPosition(this._startPosition.x, this._startPosition.y);
			}
			this._startSplashEmitter = new GameObject("EnergySplashParticleEmitter", new Type[]
			{
				typeof(ParticleEmitter2D)
			}).GetComponent<ParticleEmitter2D>();
			GameManager.Stage.effects.AddParticleEffect(this._startSplashEmitter, GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer);
			this._startSplashEmitter.Init(GameManager.Data.Particles.Get(2), this._definition.energyTrailSpriteGroup, true);
			this._startSplashEmitter.SetGlobalPosition(this._startPosition.x, this._startPosition.y);
		}
		if (this._trailFormat == EnergyTrailFormat.FULL)
		{
			this._trailEmitter = new GameObject("EnergyTrailParticleEmitter", new Type[]
			{
				typeof(ParticleEmitter2D)
			}).GetComponent<ParticleEmitter2D>();
			GameManager.Stage.effects.AddParticleEffect(this._trailEmitter, GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer);
			this._trailEmitter.Init(GameManager.Data.Particles.Get(1), this._definition.energyTrailSpriteGroup, true);
			this._trailEmitter.SetGlobalPosition(this._startPosition.x, this._startPosition.y);
			Vector3 vector = this._endPosition - this._startPosition;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._trailEmitter.gameObj.transform.localEulerAngles = new Vector3(0f, 0f, num + 180f);
			float num2 = num;
			if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
			{
				num2 += UnityEngine.Random.Range(30f, 70f);
			}
			else
			{
				num2 -= UnityEngine.Random.Range(30f, 70f);
			}
			num2 = MathUtils.NormalizeDegreeAngle(num2) * 0.017453292f;
			this._startHandle = new Vector3(Mathf.Cos(num2), Mathf.Sin(num2), 0f);
			this._startHandle.Normalize();
			this._startHandle *= UnityEngine.Random.Range(300f, 700f);
			num2 = num + 180f;
			if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
			{
				num2 += UnityEngine.Random.Range(20f, 60f);
			}
			else
			{
				num2 -= UnityEngine.Random.Range(20f, 60f);
			}
			num2 = MathUtils.NormalizeDegreeAngle(num2) * 0.017453292f;
			this._endHandle = new Vector3(Mathf.Cos(num2), Mathf.Sin(num2), 0f);
			this._endHandle.Normalize();
			this._endHandle *= UnityEngine.Random.Range(200f, 600f);
		}
		else
		{
			this._forceComplete = true;
		}
		this._inited = true;
		this._initTimestamp = GameManager.System.Lifetime(true);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00005E11 File Offset: 0x00004011
	public void OverrideEnergySurge(GirlEnergySurge energySurge)
	{
		this._energySurgeOverride = energySurge;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00025E94 File Offset: 0x00024094
	private void Update()
	{
		if (!this._inited)
		{
			return;
		}
		float num = (GameManager.System.Lifetime(true) - this._initTimestamp) / this._duration;
		num *= num;
		if (num < 1f && !this._forceComplete)
		{
			Vector3 vector = MathUtils.LerpCubicCurveBezierPoint(this._startPosition, this._startPosition + this._startHandle, this._endPosition + this._endHandle, this._endPosition, num);
			this._trailEmitter.SetGlobalPosition(vector.x, vector.y);
			Vector3 vector2 = this._endPosition - this._trailEmitter.gameObj.transform.position;
			float num2 = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			this._trailEmitter.gameObj.transform.localEulerAngles = new Vector3(0f, 0f, num2 + 180f);
		}
		else
		{
			if (this._trailEmitter != null)
			{
				this._trailEmitter.ForceComplete();
			}
			if (this._trailFormat != EnergyTrailFormat.START)
			{
				this._endSplashEmitter = new GameObject("EnergySplashParticleEmitter", new Type[]
				{
					typeof(ParticleEmitter2D)
				}).GetComponent<ParticleEmitter2D>();
				GameManager.Stage.effects.AddParticleEffect(this._endSplashEmitter, GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer);
				this._endSplashEmitter.Init(GameManager.Data.Particles.Get(2), this._definition.energyTrailSpriteGroup, true);
				this._endSplashEmitter.SetGlobalPosition(this._endPosition.x, this._endPosition.y);
				if (this._definition.showEnergySurge)
				{
					if (this._energySurgeOverride != null)
					{
						GameManager.System.Girl.ShowEnergySurge(this._energySurgeOverride, this._definition.girlEnergySurge);
					}
					else
					{
						GameManager.System.Girl.ShowEnergySurge(this._definition.girlEnergySurge, null);
					}
				}
				if (this._definition.popLabelFont != null && !StringUtils.IsEmpty(this._popText))
				{
					this._popLabel = DisplayUtils.CreatePopLabelObject(this._definition.popLabelFont, this._popText, "PopLabelObject");
					this._popLabel.Init(this._endPosition, GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer, false);
				}
				if ((this._definition.popLabelFont != null && !StringUtils.IsEmpty(this._popText)) || this._trailFormat != EnergyTrailFormat.END)
				{
					GameManager.System.Audio.Play(AudioCategory.SOUND, this._definition.contactSounds, -1, false, 1f, true);
				}
			}
			if (this.EnergyTrailCompleteEvent != null)
			{
				this.EnergyTrailCompleteEvent(this);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000684 RID: 1668
	private const int ENERGY_TRAIL_PARTICLE_EMITTER_ID = 1;

	// Token: 0x04000685 RID: 1669
	private const int ENERGY_SPLASH_PARTICLE_EMITTER_ID = 2;

	// Token: 0x04000686 RID: 1670
	private const int ENERGY_BURST_PARTICLE_EMITTER_ID = 8;

	// Token: 0x04000687 RID: 1671
	private EnergyTrailDefinition _definition;

	// Token: 0x04000688 RID: 1672
	private string _popText;

	// Token: 0x04000689 RID: 1673
	private EnergyTrailFormat _trailFormat;

	// Token: 0x0400068A RID: 1674
	private EnergyTrailDynamicDetails _dynamicDetails;

	// Token: 0x0400068B RID: 1675
	private GirlEnergySurge _energySurgeOverride;

	// Token: 0x0400068C RID: 1676
	private Vector3 _startPosition;

	// Token: 0x0400068D RID: 1677
	private Vector3 _startHandle;

	// Token: 0x0400068E RID: 1678
	private Vector3 _endPosition;

	// Token: 0x0400068F RID: 1679
	private Vector3 _endHandle;

	// Token: 0x04000690 RID: 1680
	private float _duration;

	// Token: 0x04000691 RID: 1681
	private bool _forceComplete;

	// Token: 0x04000692 RID: 1682
	private ParticleEmitter2D _burstEmitter;

	// Token: 0x04000693 RID: 1683
	private ParticleEmitter2D _startSplashEmitter;

	// Token: 0x04000694 RID: 1684
	private ParticleEmitter2D _trailEmitter;

	// Token: 0x04000695 RID: 1685
	private ParticleEmitter2D _endSplashEmitter;

	// Token: 0x04000696 RID: 1686
	private PopLabelObject _popLabel;

	// Token: 0x04000697 RID: 1687
	private bool _inited;

	// Token: 0x04000698 RID: 1688
	private float _initTimestamp;

	// Token: 0x020000F3 RID: 243
	// (Invoke) Token: 0x060004FD RID: 1277
	public delegate void EnergyTrailDelegate(EnergyTrail energyTrail);
}
