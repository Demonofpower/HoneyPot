using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class ActionMenuWindow : UIWindow
{
	// Token: 0x060003CC RID: 972 RVA: 0x00022808 File Offset: 0x00020A08
	public override void Init()
	{
		this.actionButtonContainer = base.GetChildByName("ActionMenuButtonsContainer");
		this._actionButtons = new List<ActionMenuButton>();
		for (int i = 0; i < 6; i++)
		{
			ActionMenuButton actionMenuButton = this.actionButtonContainer.GetChildByName("ActionMenuButton" + i.ToString()) as ActionMenuButton;
			actionMenuButton.Init();
			this._actionButtons.Add(actionMenuButton);
			actionMenuButton.ActionMenuButtonClickedEvent += this.OnActionMenuButtonClicked;
		}
		base.Init();
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00022890 File Offset: 0x00020A90
	public override void Refresh()
	{
		for (int i = 0; i < this._actionButtons.Count; i++)
		{
			this._actionButtons[i].Refresh();
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x000228CC File Offset: 0x00020ACC
	protected override void PreShow()
	{
		base.PreShow();
		for (int i = 0; i < this._actionButtons.Count; i++)
		{
			this._actionButtons[i].childrenAlpha = 0f;
			this._actionButtons[i].localY = this._actionButtons[i].origLocalY - 32f;
		}
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0002293C File Offset: 0x00020B3C
	protected override void Show(Sequence animSequence)
	{
		for (int i = 0; i < this._actionButtons.Count; i++)
		{
			float num = 1f;
			if (!this._actionButtons[i].button.IsEnabled())
			{
				num = 0.75f;
			}
			animSequence.Insert((float)i * 0.05f, HOTween.To(this._actionButtons[i], 0.25f, new TweenParms().Prop("childrenAlpha", num).Ease(EaseType.Linear)));
			animSequence.Insert((float)i * 0.05f, HOTween.To(this._actionButtons[i], 0.5f, new TweenParms().Prop("localY", this._actionButtons[i].origLocalY).Ease(EaseType.EaseOutBack)));
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00022A20 File Offset: 0x00020C20
	protected override void Hide(Sequence animSequence)
	{
		int num = 0;
		for (int i = this._actionButtons.Count - 1; i >= 0; i--)
		{
			animSequence.Insert((float)num * 0.05f, HOTween.To(this._actionButtons[i], 0.5f, new TweenParms().Prop("localY", this._actionButtons[i].origLocalY - 32f).Ease(EaseType.EaseInBack)));
			animSequence.Insert(0.25f + (float)num * 0.05f, HOTween.To(this._actionButtons[i], 0.25f, new TweenParms().Prop("childrenAlpha", 0).Ease(EaseType.Linear)));
			num++;
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00022AF0 File Offset: 0x00020CF0
	private void OnActionMenuButtonClicked(ActionMenuButton actionMenuButton)
	{
		switch (actionMenuButton.definition.action)
		{
		case ActionMenuItemType.TALK_WITH_HER:
			GameManager.System.Girl.TalkWithHer();
			break;
		case ActionMenuItemType.ASK_HER_OUT:
			if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).appetite == 0)
			{
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.isHungryDialogTrigger, 0, false, -1);
				GameManager.Stage.uiWindows.ResetActiveWindow();
			}
			else if (GameManager.System.Player.IsInventoryFull(null))
			{
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.inventoryFullDialogTrigger, 0, false, -1);
				GameManager.Stage.uiWindows.ResetActiveWindow();
			}
			else if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).gotPanties && GameManager.System.Player.endingSceneShown)
			{
				List<DialogSceneResponseOption> list = new List<DialogSceneResponseOption>();
				for (int i = 0; i < GameManager.System.Location.currentGirl.GetDateLocationsByDaytime(GameManager.System.Clock.DayTime(-1)).Count; i++)
				{
					LocationDefinition locationDefinition = GameManager.System.Location.currentGirl.GetDateLocationsByDaytime(GameManager.System.Clock.DayTime(-1))[i];
					list.Add(new DialogSceneResponseOption
					{
						text = locationDefinition.fullName,
						specialIndex = i
					});
				}
				GameManager.Stage.uiWindows.forceResponseOptions = list;
				TalkWindow talkWindow = GameManager.Stage.uiWindows.SetWindow(GameManager.Stage.uiWindows.talkWindow, false, false) as TalkWindow;
				talkWindow.ResponseSelectedEvent += this.OnDateLocationOptionSelected;
				GameManager.Stage.uiWindows.HideActiveWindow();
			}
			else
			{
				List<LocationDefinition> dateLocationsByDaytime = GameManager.System.Location.currentGirl.GetDateLocationsByDaytime(GameManager.System.Clock.DayTime(-1));
				LocationDefinition locationDefinition2 = GameManager.System.Player.GetLoggedDateLocation(GameManager.System.Clock.DayTime(-1));
				if (locationDefinition2 != null)
				{
					dateLocationsByDaytime.Remove(locationDefinition2);
				}
				locationDefinition2 = dateLocationsByDaytime[UnityEngine.Random.Range(0, dateLocationsByDaytime.Count)];
				this.GoToDateLocation(locationDefinition2);
			}
			break;
		case ActionMenuItemType.UI_WINDOW:
			if (actionMenuButton.definition.uiPrefab != null)
			{
				GameManager.Stage.uiWindows.SetWindow(actionMenuButton.definition.uiPrefab, true, false);
			}
			break;
		case ActionMenuItemType.CELL_APP:
			if (actionMenuButton.definition.cellApp != null)
			{
				GameManager.Stage.cellPhone.SetCellApp(actionMenuButton.definition.cellApp);
				GameManager.Stage.uiTop.OpenCellPhone();
			}
			break;
		case ActionMenuItemType.GIRL_PROFILE:
			if (actionMenuButton.definition.cellApp != null)
			{
				GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_girl"] = GameManager.System.Location.currentGirl.id;
				GameManager.Stage.cellPhone.cellMemory["cell_memory_profile_tab"] = 0;
				GameManager.Stage.cellPhone.SetCellApp(actionMenuButton.definition.cellApp);
				GameManager.Stage.uiTop.OpenCellPhone();
			}
			break;
		case ActionMenuItemType.PURCHASE_GIFTS:
			if (actionMenuButton.definition.cellApp != null)
			{
				GameManager.Stage.cellPhone.cellMemory["cell_memory_store_tab"] = 0;
				GameManager.Stage.cellPhone.SetCellApp(actionMenuButton.definition.cellApp);
				GameManager.Stage.uiTop.OpenCellPhone();
			}
			break;
		}
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00022EF4 File Offset: 0x000210F4
	private void OnDateLocationOptionSelected(TalkWindow talkWindow, DialogSceneResponseOption responseOption)
	{
		talkWindow.ResponseSelectedEvent -= this.OnDateLocationOptionSelected;
		this.GoToDateLocation(GameManager.System.Location.currentGirl.GetDateLocationsByDaytime(GameManager.System.Clock.DayTime(-1))[responseOption.specialIndex]);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00022F48 File Offset: 0x00021148
	private void GoToDateLocation(LocationDefinition dateLocation)
	{
		GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).dayDated = true;
		GameManager.System.Player.LogDateLocation(GameManager.System.Clock.DayTime(-1), dateLocation);
		GameManager.System.Puzzle.TravelToPuzzleLocation(dateLocation, GameManager.System.Location.currentGirl);
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00022FB8 File Offset: 0x000211B8
	protected override void Destructor()
	{
		base.Destructor();
		for (int i = 0; i < this._actionButtons.Count; i++)
		{
			this._actionButtons[i].ActionMenuButtonClickedEvent -= this.OnActionMenuButtonClicked;
		}
		this._actionButtons.Clear();
		this._actionButtons = null;
	}

	// Token: 0x040003A8 RID: 936
	private const int ACTION_BUTTON_COUNT = 6;

	// Token: 0x040003A9 RID: 937
	public DisplayObject actionButtonContainer;

	// Token: 0x040003AA RID: 938
	private List<ActionMenuButton> _actionButtons;
}
