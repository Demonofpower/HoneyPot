using System;

// Token: 0x02000068 RID: 104
public class UIPuzzle : DisplayObject
{
	// Token: 0x06000323 RID: 803 RVA: 0x0000497C File Offset: 0x00002B7C
	protected override void OnStart()
	{
		base.OnStart();
		this.puzzleGrid = (base.GetChildByName("PuzzleGrid") as UIPuzzleGrid);
		this.puzzleStatus = (base.GetChildByName("PuzzleStatus") as UIPuzzleStatus);
	}

	// Token: 0x040002CE RID: 718
	public UIPuzzleGrid puzzleGrid;

	// Token: 0x040002CF RID: 719
	public UIPuzzleStatus puzzleStatus;
}
