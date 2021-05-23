using System;
using System.Collections.Generic;

// Token: 0x020000D4 RID: 212
[Serializable]
public class GirlPiece : SubDefinition
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x00005BC6 File Offset: 0x00003DC6
	public GirlPieceArt primaryArt
	{
		get
		{
			if (this.art.Count >= 1)
			{
				return this.art[0];
			}
			return this.art[this.art.Count - 1];
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060004CF RID: 1231 RVA: 0x00005BFE File Offset: 0x00003DFE
	public GirlPieceArt secondaryArt
	{
		get
		{
			if (this.art.Count >= 2)
			{
				return this.art[1];
			}
			return this.art[this.art.Count - 1];
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00005C36 File Offset: 0x00003E36
	public GirlPieceArt tertiaryArt
	{
		get
		{
			if (this.art.Count >= 3)
			{
				return this.art[2];
			}
			return this.art[this.art.Count - 1];
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00005C6E File Offset: 0x00003E6E
	public GirlPieceArt quaternaryArt
	{
		get
		{
			if (this.art.Count >= 4)
			{
				return this.art[3];
			}
			return this.art[this.art.Count - 1];
		}
	}

	// Token: 0x0400059D RID: 1437
	public GirlPieceType type;

	// Token: 0x0400059E RID: 1438
	public string name;

	// Token: 0x0400059F RID: 1439
	public List<GirlPieceArt> art = new List<GirlPieceArt>();

	// Token: 0x040005A0 RID: 1440
	public GirlExpressionType expressionType;

	// Token: 0x040005A1 RID: 1441
	public GirlLayer layer;

	// Token: 0x040005A2 RID: 1442
	public float showChance;

	// Token: 0x040005A3 RID: 1443
	public string limitToOutfits;

	// Token: 0x040005A4 RID: 1444
	public bool hideOnDates;

	// Token: 0x040005A5 RID: 1445
	public bool underwear;
}
