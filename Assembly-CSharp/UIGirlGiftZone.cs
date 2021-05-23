using System;

// Token: 0x0200005A RID: 90
public class UIGirlGiftZone : DisplayObject
{
	// Token: 0x060002CE RID: 718 RVA: 0x0000453C File Offset: 0x0000273C
	public override bool CanShowTooltip()
	{
		return GameManager.System.GameState == GameState.SIM && GameManager.Stage.uiGirl.IsInGiftItemMode();
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00004565 File Offset: 0x00002765
	public override string GetUniqueTooltipMessage()
	{
		return "Give to " + GameManager.System.Location.currentGirl.firstName;
	}
}
