using System;

// Token: 0x02000140 RID: 320
public class PlayerUtils
{
	// Token: 0x0600078A RID: 1930 RVA: 0x00007A1D File Offset: 0x00005C1D
	public static int GetExpToTraitLevel(int traitLevel)
	{
		return (traitLevel - 1) * (traitLevel - 1) * 16;
	}
}
