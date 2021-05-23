using System;
using System.Collections.Generic;

// Token: 0x02000132 RID: 306
[Serializable]
public class GirlSaveData
{
	// Token: 0x06000748 RID: 1864 RVA: 0x00036558 File Offset: 0x00034758
	public GirlSaveData(GirlDefinition girl)
	{
		this.metStatus = 0;
		if (!girl.secretGirl)
		{
			this.metStatus = 1;
		}
		this.relationshipLevel = 1;
		this.appetite = 6;
		this.inebriation = 0;
		this.dayDated = false;
		this.photosEarned = new List<int>();
		this.uniqueGifts = new List<int>();
		this.collection = new List<int>();
		this.hairstyles = new List<int>();
		this.hairstyles.Add(0);
		this.outfits = new List<int>();
		this.outfits.Add(0);
		this.hairstyle = 0;
		this.outfit = 0;
		this.details = new bool[Enum.GetNames(typeof(GirlDetailType)).Length];
		this.recentQuizzes = new List<int>();
		this.recentQuestions = new List<int>();
		this.gotPanties = false;
		this.lovePotionUsed = false;
	}

	// Token: 0x0400084E RID: 2126
	public int metStatus;

	// Token: 0x0400084F RID: 2127
	public int relationshipLevel;

	// Token: 0x04000850 RID: 2128
	public int appetite;

	// Token: 0x04000851 RID: 2129
	public int inebriation;

	// Token: 0x04000852 RID: 2130
	public bool dayDated;

	// Token: 0x04000853 RID: 2131
	public List<int> photosEarned;

	// Token: 0x04000854 RID: 2132
	public List<int> uniqueGifts;

	// Token: 0x04000855 RID: 2133
	public List<int> collection;

	// Token: 0x04000856 RID: 2134
	public List<int> hairstyles;

	// Token: 0x04000857 RID: 2135
	public List<int> outfits;

	// Token: 0x04000858 RID: 2136
	public int hairstyle;

	// Token: 0x04000859 RID: 2137
	public int outfit;

	// Token: 0x0400085A RID: 2138
	public bool[] details;

	// Token: 0x0400085B RID: 2139
	public List<int> recentQuizzes;

	// Token: 0x0400085C RID: 2140
	public List<int> recentQuestions;

	// Token: 0x0400085D RID: 2141
	public bool gotPanties;

	// Token: 0x0400085E RID: 2142
	public bool lovePotionUsed;
}
