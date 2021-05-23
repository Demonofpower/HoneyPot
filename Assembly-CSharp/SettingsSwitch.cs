using System;

// Token: 0x02000043 RID: 67
public class SettingsSwitch : DisplayObject
{
	// Token: 0x14000018 RID: 24
	// (add) Token: 0x0600022F RID: 559 RVA: 0x00004017 File Offset: 0x00002217
	// (remove) Token: 0x06000230 RID: 560 RVA: 0x00004030 File Offset: 0x00002230
	public event SettingsSwitch.SettingsSwitchDelegate SettingsSwitchPressedEvent;

	// Token: 0x06000231 RID: 561 RVA: 0x00017358 File Offset: 0x00015558
	public void Init(int index)
	{
		this.background = (base.GetChildByName("SettingsPanelSwitchBackground") as SpriteObject);
		this.optionLabel = (base.GetChildByName("SettingsPanelSwitchLabel") as LabelObject);
		this.optionIndex = index;
		base.button.ButtonPressedEvent += this.OnButtonPressed;
		this.Refresh();
	}

	// Token: 0x06000232 RID: 562 RVA: 0x000173B8 File Offset: 0x000155B8
	public void Refresh()
	{
		if (GameManager.System.Player.settingsGender == SettingsGender.FEMALE && !StringUtils.IsEmpty(this.altLabelText))
		{
			this.optionLabel.SetText(this.altLabelText);
		}
		else
		{
			this.optionLabel.SetText(this.labelText);
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00004049 File Offset: 0x00002249
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.SettingsSwitchPressedEvent != null)
		{
			this.SettingsSwitchPressedEvent(this);
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00004062 File Offset: 0x00002262
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x040001DA RID: 474
	public string labelText;

	// Token: 0x040001DB RID: 475
	public string altLabelText;

	// Token: 0x040001DC RID: 476
	public SpriteObject background;

	// Token: 0x040001DD RID: 477
	public LabelObject optionLabel;

	// Token: 0x040001DE RID: 478
	public int optionIndex;

	// Token: 0x02000044 RID: 68
	// (Invoke) Token: 0x06000236 RID: 566
	public delegate void SettingsSwitchDelegate(SettingsSwitch settingsSwitch);
}
