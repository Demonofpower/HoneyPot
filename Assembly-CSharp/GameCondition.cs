using System;

// Token: 0x020000DA RID: 218
[Serializable]
public class GameCondition
{
	// Token: 0x040005B4 RID: 1460
	public GameConditionType type;

	// Token: 0x040005B5 RID: 1461
	public bool inverse;

	// Token: 0x040005B6 RID: 1462
	public GirlDefinition girlDefinition;

	// Token: 0x040005B7 RID: 1463
	public LocationDefinition locationDefinition;

	// Token: 0x040005B8 RID: 1464
	public GirlDetailType girlDetailType;

	// Token: 0x040005B9 RID: 1465
	public int relationshipLevel;

	// Token: 0x040005BA RID: 1466
	public GirlMetStatus metStatus;
}
