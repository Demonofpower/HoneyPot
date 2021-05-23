using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class ActionMenuButton : DisplayObject
{
	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060003C1 RID: 961 RVA: 0x00004FAA File Offset: 0x000031AA
	// (remove) Token: 0x060003C2 RID: 962 RVA: 0x00004FC3 File Offset: 0x000031C3
	public event ActionMenuButton.ActionMenuButtonDelegate ActionMenuButtonClickedEvent;

	// Token: 0x060003C3 RID: 963 RVA: 0x000222DC File Offset: 0x000204DC
	public void Init()
	{
		if (this.definition == null)
		{
			this.definition = GameManager.Data.ActionMenuItems.Get(1);
		}
		this.actionIcon = (base.GetChildByName("ActionMenuButtonIcon") as SpriteObject);
		this.actionLabel = (base.GetChildByName("ActionMenuButtonLabel") as LabelObject);
		this.actionSubLabel = (base.GetChildByName("ActionMenuButtonSubLabel") as LabelObject);
		this.actionIcon.sprite.SetSprite(this.definition.iconName);
		this.actionIcon.localX += (float)this.definition.iconOffsetX;
		this.actionIcon.localY += (float)this.definition.iconOffsetY;
		this.actionLabel.SetText(this.definition.labelText);
		this.origLocalY = base.transform.localPosition.y;
		base.button.ButtonPressedEvent += this.OnButtonPress;
		if (this.definition.action == ActionMenuItemType.ASK_HER_OUT && GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).dayDated)
		{
			base.button.Disable();
		}
		this.Refresh();
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0002243C File Offset: 0x0002063C
	public void Refresh()
	{
		switch (this.definition.subtitleType)
		{
		case ActionMenuSubtitleType.TALK_WITH_HER:
		{
			GirlPlayerData girlPlayerData = GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl);
			this.actionSubLabel.SetText("+" + Mathf.RoundToInt(50f * (1f + (float)girlPlayerData.UniqueGiftCount() * 0.5f) * (1f + (float)girlPlayerData.inebriation * 0.1f)) + " Hunie");
			break;
		}
		case ActionMenuSubtitleType.ASK_ON_DATE:
			if (base.button.IsEnabled())
			{
				if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).relationshipLevel < 5)
				{
					this.actionSubLabel.SetText(StringUtils.Titleize(StringUtils.IntToString(GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).relationshipLevel - 1)) + " Date");
				}
				else if (GameManager.System.Clock.DayTime(-1) == GameClockDaytime.NIGHT)
				{
					this.actionSubLabel.SetText("Night Date");
				}
				else
				{
					this.actionSubLabel.SetText("Day Date");
				}
			}
			else
			{
				this.actionSubLabel.SetText("-Finished-");
			}
			break;
		case ActionMenuSubtitleType.VIEW_INVENTORY:
		{
			int num = 0;
			for (int i = 0; i < GameManager.System.Player.inventory.Length; i++)
			{
				if (GameManager.System.Player.inventory[i].itemDefinition != null)
				{
					num++;
				}
			}
			this.actionSubLabel.SetText(Mathf.FloorToInt((float)num / 30f * 100f) + "% Full");
			break;
		}
		case ActionMenuSubtitleType.GIRL_FINDER:
		{
			int num2 = 0;
			for (int j = 0; j < GameManager.System.Player.girls.Count; j++)
			{
				GirlPlayerData girlPlayerData = GameManager.System.Player.girls[j];
				GirlDefinition girlDefinition = girlPlayerData.GetGirlDefinition();
				LocationDefinition x = girlDefinition.IsAtLocationAtTime(GameManager.System.Clock.Weekday(GameManager.System.Clock.TotalMinutesElapsed(360), true), GameManager.System.Clock.DayTime(GameManager.System.Clock.TotalMinutesElapsed(360)));
				if (girlDefinition != GameManager.System.Location.currentGirl && girlPlayerData.metStatus != GirlMetStatus.LOCKED && girlPlayerData.metStatus != GirlMetStatus.UNMET && (girlPlayerData.metStatus != GirlMetStatus.MET || !(x == null)))
				{
					num2++;
				}
			}
			this.actionSubLabel.SetText(num2.ToString() + " Available");
			break;
		}
		case ActionMenuSubtitleType.CHECK_PROFILE:
			this.actionSubLabel.SetText(StringUtils.Possessize(GameManager.System.Location.currentGirl.firstName) + " Info");
			break;
		case ActionMenuSubtitleType.SHOP_FOR_HER:
		{
			int num3 = 0;
			for (int k = 0; k < GameManager.System.Player.storeGifts.Length; k++)
			{
				if (!GameManager.System.Player.storeGifts[k].soldOut)
				{
					num3++;
				}
			}
			if (num3 > 0)
			{
				this.actionSubLabel.SetText(num3.ToString() + " in Stock");
			}
			else
			{
				this.actionSubLabel.SetText("Out of Stock");
			}
			break;
		}
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00004FDC File Offset: 0x000031DC
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.ActionMenuButtonClickedEvent != null)
		{
			this.ActionMenuButtonClickedEvent(this);
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00004FF5 File Offset: 0x000031F5
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x040003A2 RID: 930
	public ActionMenuItemDefinition definition;

	// Token: 0x040003A3 RID: 931
	public SpriteObject actionIcon;

	// Token: 0x040003A4 RID: 932
	public LabelObject actionLabel;

	// Token: 0x040003A5 RID: 933
	public LabelObject actionSubLabel;

	// Token: 0x040003A6 RID: 934
	public float origLocalY;

	// Token: 0x0200007E RID: 126
	// (Invoke) Token: 0x060003C8 RID: 968
	public delegate void ActionMenuButtonDelegate(ActionMenuButton actionMenuButton);
}
