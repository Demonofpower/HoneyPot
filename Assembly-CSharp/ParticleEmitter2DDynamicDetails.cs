using System;

// Token: 0x020000EA RID: 234
public class ParticleEmitter2DDynamicDetails
{
	// Token: 0x060004DF RID: 1247 RVA: 0x00005CB9 File Offset: 0x00003EB9
	public ParticleEmitter2DDynamicDetails(ParticleEmitter2DDefinition definition)
	{
		this.originSpreadX = definition.originSpreadX;
		this.originSpreadY = definition.originSpreadY;
		this.originSpreadRadius = definition.originSpreadRadius;
	}

	// Token: 0x04000654 RID: 1620
	public float originSpreadX;

	// Token: 0x04000655 RID: 1621
	public float originSpreadY;

	// Token: 0x04000656 RID: 1622
	public float originSpreadRadius;
}
