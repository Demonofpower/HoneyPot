using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class GirlProfilesCellApp : UICellApp
{
	// Token: 0x060001E7 RID: 487 RVA: 0x00014C28 File Offset: 0x00012E28
	public override void Init()
	{
		this.girlProfilesIconsContainer = base.GetChildByName("GirlProfilesIconsContainer");
		this._girlProfilesIcons = new List<GirlProfilesIcon>();
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			GirlProfilesIcon girlProfilesIcon = this.girlProfilesIconsContainer.GetChildByName("GirlProfilesIcon" + i.ToString()) as GirlProfilesIcon;
			if (GameManager.System.Player.GetGirlData(girlProfilesIcon.girlDefinition).metStatus != GirlMetStatus.MET)
			{
				girlProfilesIcon.gameObj.SetActive(false);
			}
			else
			{
				girlProfilesIcon.Init();
				girlProfilesIcon.button.ButtonPressedEvent += this.OnGirlProfilesIconClicked;
				this._girlProfilesIcons.Add(girlProfilesIcon);
				girlProfilesIcon.localX = (float)(num % 3 * 136);
				girlProfilesIcon.localY = Mathf.Floor((float)(num / 3)) * -148f;
				num++;
			}
		}
		if (this._girlProfilesIcons.Count > 0)
		{
			this.girlProfilesIconsContainer.localX -= (float)(Mathf.Min(this._girlProfilesIcons.Count - 1, 2) * 68);
			this.girlProfilesIconsContainer.localY += Mathf.Min(Mathf.Floor((float)((this._girlProfilesIcons.Count - 1) / 3)), 3f) * 74f;
			if (this._girlProfilesIcons.Count > 3)
			{
				if ((this._girlProfilesIcons.Count - 1) % 3 == 0)
				{
					this._girlProfilesIcons[this._girlProfilesIcons.Count - 1].localX += 136f;
				}
				else if ((this._girlProfilesIcons.Count - 2) % 3 == 0)
				{
					this._girlProfilesIcons[this._girlProfilesIcons.Count - 1].localX += 68f;
					this._girlProfilesIcons[this._girlProfilesIcons.Count - 2].localX += 68f;
				}
			}
		}
		else
		{
			GameManager.Stage.cellPhone.ShowCellAppError("No girls yet...", false, 0f);
		}
		this.Refresh();
		GameManager.Stage.cellNotifications.ClearNotificationsOfType(CellNotificationType.PROFILE);
		base.Init();
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000306D File Offset: 0x0000126D
	public override void Refresh()
	{
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00014E70 File Offset: 0x00013070
	private void OnGirlProfilesIconClicked(ButtonObject buttonObject)
	{
		GirlDefinition girlDefinition = (buttonObject.GetDisplayObject() as GirlProfilesIcon).girlDefinition;
		GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_girl"] = girlDefinition.id;
		GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_tab"] = 0;
		GameManager.Stage.cellPhone.ChangeCellApp(this.girlProfileCellApp);
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00014EDC File Offset: 0x000130DC
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._girlProfilesIcons.Count; i++)
		{
			this._girlProfilesIcons[i].button.ButtonPressedEvent -= this.OnGirlProfilesIconClicked;
		}
		this._girlProfilesIcons.Clear();
		this._girlProfilesIcons = null;
	}

	// Token: 0x0400017C RID: 380
	private const int GIRL_PROFILES_ICON_COUNT = 12;

	// Token: 0x0400017D RID: 381
	public CellAppDefinition girlProfileCellApp;

	// Token: 0x0400017E RID: 382
	public DisplayObject girlProfilesIconsContainer;

	// Token: 0x0400017F RID: 383
	private List<GirlProfilesIcon> _girlProfilesIcons;
}
