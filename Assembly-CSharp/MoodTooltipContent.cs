using System;

// Token: 0x02000077 RID: 119
public class MoodTooltipContent : UITooltipContent
{
	// Token: 0x06000392 RID: 914 RVA: 0x00020984 File Offset: 0x0001EB84
	public override void Init(TooltipObject tooltipObject)
	{
		base.Init(tooltipObject);
		this.titleLabel = (base.GetChildByName("TooltipMoodTitle") as LabelObject);
		this.moodLabel = (base.GetChildByName("TooltipMoodNextMood") as LabelObject);
		this.costLabel = (base.GetChildByName("TooltipMoodCost") as LabelObject);
		this.Refresh();
	}

	// Token: 0x06000393 RID: 915 RVA: 0x000209E0 File Offset: 0x0001EBE0
	public override void Refresh()
	{
		this.titleLabel.SetText("Next Level");
		this.moodLabel.SetText(string.Concat(new string[]
		{
			"Lvl ",
			(GameManager.System.Puzzle.Game.currentPassionLevel + 1).ToString(),
			"^CFFFFFF80 (+",
			((GameManager.System.Puzzle.GetPassionLevelMultiplier(GameManager.System.Puzzle.Game.currentPassionLevel + 1) - 1f) * 100f).ToString(),
			"%)"
		}));
		this.costLabel.SetText("Passion Needed: " + (GameManager.System.Puzzle.GetPassionLevelCost(GameManager.System.Puzzle.Game.currentPassionLevel + 1) - GameManager.System.Puzzle.Game.GetResourceValue(PuzzleGameResourceType.PASSION)).ToString());
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00004E5E File Offset: 0x0000305E
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x0400036B RID: 875
	public LabelObject titleLabel;

	// Token: 0x0400036C RID: 876
	public LabelObject moodLabel;

	// Token: 0x0400036D RID: 877
	public LabelObject costLabel;
}
