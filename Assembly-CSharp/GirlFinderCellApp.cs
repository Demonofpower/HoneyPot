using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class GirlFinderCellApp : UICellApp
{
	// Token: 0x060001AD RID: 429 RVA: 0x0001313C File Offset: 0x0001133C
	public override void Init()
	{
		this.girlFinderIconsContainer = base.GetChildByName("GirlFinderIconsContainer");
		this._girlFinderIcons = new List<GirlFinderIcon>();
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			GirlFinderIcon girlFinderIcon = this.girlFinderIconsContainer.GetChildByName("GirlFinderIcon" + i.ToString()) as GirlFinderIcon;
			if (GameManager.System.Player.GetGirlData(girlFinderIcon.girlDefinition).metStatus == GirlMetStatus.LOCKED || GameManager.System.Player.GetGirlData(girlFinderIcon.girlDefinition).metStatus == GirlMetStatus.UNMET)
			{
				girlFinderIcon.gameObj.SetActive(false);
			}
			else
			{
				girlFinderIcon.Init();
				girlFinderIcon.button.ButtonPressedEvent += this.OnGirlFinderIconClicked;
				this._girlFinderIcons.Add(girlFinderIcon);
				girlFinderIcon.localX = (float)(num % 3 * 136);
				girlFinderIcon.localY = Mathf.Floor((float)(num / 3)) * -148f;
				num++;
			}
		}
		if (this._girlFinderIcons.Count > 0)
		{
			this.girlFinderIconsContainer.localX -= (float)(Mathf.Min(this._girlFinderIcons.Count - 1, 2) * 68);
			this.girlFinderIconsContainer.localY += Mathf.Min(Mathf.Floor((float)((this._girlFinderIcons.Count - 1) / 3)), 3f) * 74f;
			if (this._girlFinderIcons.Count > 3)
			{
				if ((this._girlFinderIcons.Count - 1) % 3 == 0)
				{
					this._girlFinderIcons[this._girlFinderIcons.Count - 1].localX += 136f;
				}
				else if ((this._girlFinderIcons.Count - 2) % 3 == 0)
				{
					this._girlFinderIcons[this._girlFinderIcons.Count - 1].localX += 68f;
					this._girlFinderIcons[this._girlFinderIcons.Count - 2].localX += 68f;
				}
			}
			if (!GameManager.System.Location.IsLocationSettled() || (GameManager.System.Location.currentLocation.type == LocationType.NORMAL && !GameManager.Stage.uiWindows.IsDefaultWindowActive(true)) || (GameManager.System.Location.currentLocation.type == LocationType.DATE && GameManager.System.Puzzle.Game != null))
			{
				for (int j = 0; j < this._girlFinderIcons.Count; j++)
				{
					this._girlFinderIcons[j].button.Disable();
					this._girlFinderIcons[j].tooltip.Disable();
				}
				GameManager.Stage.cellPhone.ShowCellAppError("Not a good time to travel.", true, 0f);
			}
		}
		else
		{
			GameManager.Stage.cellPhone.ShowCellAppError("No girls yet...", false, 0f);
		}
		this.Refresh();
		base.Init();
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000306D File Offset: 0x0000126D
	public override void Refresh()
	{
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00013468 File Offset: 0x00011668
	private void OnGirlFinderIconClicked(ButtonObject buttonObject)
	{
		GirlDefinition girlDefinition = (buttonObject.GetDisplayObject() as GirlFinderIcon).girlDefinition;
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDefinition);
		LocationDefinition destination = girlDefinition.IsAtLocationAtTime(GameManager.System.Clock.Weekday(GameManager.System.Clock.TotalMinutesElapsed(360), true), GameManager.System.Clock.DayTime(GameManager.System.Clock.TotalMinutesElapsed(360)));
		if (girlData.metStatus != GirlMetStatus.MET)
		{
			destination = girlDefinition.introLocation;
		}
		GameManager.System.Location.TravelTo(destination, girlDefinition);
		GameManager.Stage.uiTop.CloseCellPhone();
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00013518 File Offset: 0x00011718
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._girlFinderIcons.Count; i++)
		{
			this._girlFinderIcons[i].button.ButtonPressedEvent -= this.OnGirlFinderIconClicked;
		}
		this._girlFinderIcons.Clear();
		this._girlFinderIcons = null;
	}

	// Token: 0x04000132 RID: 306
	private const int GIRL_FINDER_ICON_COUNT = 12;

	// Token: 0x04000133 RID: 307
	public DisplayObject girlFinderIconsContainer;

	// Token: 0x04000134 RID: 308
	private List<GirlFinderIcon> _girlFinderIcons;
}
