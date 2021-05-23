using System;

// Token: 0x020000E8 RID: 232
public class ParticleEmitter2DDefinition : Definition
{
	// Token: 0x04000630 RID: 1584
	public ParticleEmitter2DOriginSpreadType originSpreadType;

	// Token: 0x04000631 RID: 1585
	public float originSpreadX;

	// Token: 0x04000632 RID: 1586
	public float originSpreadY;

	// Token: 0x04000633 RID: 1587
	public float originSpreadRadius;

	// Token: 0x04000634 RID: 1588
	public float initialDirection = 90f;

	// Token: 0x04000635 RID: 1589
	public float directionalRange;

	// Token: 0x04000636 RID: 1590
	public float gravity;

	// Token: 0x04000637 RID: 1591
	public float particleMass;

	// Token: 0x04000638 RID: 1592
	public float force;

	// Token: 0x04000639 RID: 1593
	public float forceVariance;

	// Token: 0x0400063A RID: 1594
	public float torque;

	// Token: 0x0400063B RID: 1595
	public float torqueVariance;

	// Token: 0x0400063C RID: 1596
	public float startAlpha = 1f;

	// Token: 0x0400063D RID: 1597
	public float startAlphaVariance;

	// Token: 0x0400063E RID: 1598
	public float endAlpha;

	// Token: 0x0400063F RID: 1599
	public float endAlphaVariance;

	// Token: 0x04000640 RID: 1600
	public float alphaDelay;

	// Token: 0x04000641 RID: 1601
	public float alphaDelayVariance;

	// Token: 0x04000642 RID: 1602
	public float startScale = 1f;

	// Token: 0x04000643 RID: 1603
	public float startScaleVariance;

	// Token: 0x04000644 RID: 1604
	public float endScale = 1f;

	// Token: 0x04000645 RID: 1605
	public float endScaleVariance;

	// Token: 0x04000646 RID: 1606
	public float scaleDelay;

	// Token: 0x04000647 RID: 1607
	public float scaleDelayVariance;

	// Token: 0x04000648 RID: 1608
	public float particleDelay = 0.01f;

	// Token: 0x04000649 RID: 1609
	public float particleDelayVariance;

	// Token: 0x0400064A RID: 1610
	public int particleMultiplier = 1;

	// Token: 0x0400064B RID: 1611
	public float particleLifetime = 1f;

	// Token: 0x0400064C RID: 1612
	public float particleLifetimeVariance;

	// Token: 0x0400064D RID: 1613
	public float duration;

	// Token: 0x0400064E RID: 1614
	public float particleLimit;

	// Token: 0x0400064F RID: 1615
	public bool particlesAttached;
}
