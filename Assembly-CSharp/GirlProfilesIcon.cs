using System;

// Token: 0x02000039 RID: 57
public class GirlProfilesIcon : SpriteObject
{
	// Token: 0x060001EC RID: 492 RVA: 0x00003D60 File Offset: 0x00001F60
	public void Init()
	{
		base.button.Init();
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00003B62 File Offset: 0x00001D62
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x04000180 RID: 384
	public GirlDefinition girlDefinition;
}
