using System;
using System.Collections.Generic;

// Token: 0x02000045 RID: 69
public class SettingsSwitchPanel : DisplayObject
{
	// Token: 0x14000019 RID: 25
	// (add) Token: 0x0600023A RID: 570 RVA: 0x00004081 File Offset: 0x00002281
	// (remove) Token: 0x0600023B RID: 571 RVA: 0x0000409A File Offset: 0x0000229A
	public event SettingsSwitchPanel.SettingsSwitchPanelDelegate SettingsSwitchPanelOptionSelectEvent;

	// Token: 0x0600023C RID: 572 RVA: 0x00017414 File Offset: 0x00015614
	public void Init(int optionCount, int selectedIndex)
	{
		this.switchesContainer = base.GetChildByName("SettingsPanelSwitches");
		this._switches = new List<SettingsSwitch>();
		for (int i = 0; i < optionCount; i++)
		{
			SettingsSwitch settingsSwitch = this.switchesContainer.GetChildByName("SettingsPanelSwitch" + i.ToString()) as SettingsSwitch;
			settingsSwitch.Init(i);
			settingsSwitch.SettingsSwitchPressedEvent += this.OnSettingsSwitchPressed;
			this._switches.Add(settingsSwitch);
			if (settingsSwitch.optionIndex == selectedIndex)
			{
				this._selectedSwitch = settingsSwitch;
				settingsSwitch.button.Disable();
			}
		}
		this.Refresh();
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000174BC File Offset: 0x000156BC
	public void Refresh()
	{
		for (int i = 0; i < this._switches.Count; i++)
		{
			this._switches[i].Refresh();
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x000040B3 File Offset: 0x000022B3
	private void OnSettingsSwitchPressed(SettingsSwitch settingsSwitch)
	{
		this.ChangeSelectedOption(this._switches.IndexOf(settingsSwitch));
		if (this.SettingsSwitchPanelOptionSelectEvent != null)
		{
			this.SettingsSwitchPanelOptionSelectEvent(this, settingsSwitch.optionIndex);
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x000174F8 File Offset: 0x000156F8
	public void ChangeSelectedOption(int optionIndex)
	{
		SettingsSwitch selectedSwitch = this._switches[optionIndex];
		if (this._selectedSwitch != null)
		{
			this._selectedSwitch.button.Enable();
		}
		this._selectedSwitch = null;
		this._selectedSwitch = selectedSwitch;
		this._selectedSwitch.button.Disable();
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00017554 File Offset: 0x00015754
	protected override void Destructor()
	{
		base.Destructor();
		if (this._switches != null)
		{
			for (int i = 0; i < this._switches.Count; i++)
			{
				this._switches[i].SettingsSwitchPressedEvent -= this.OnSettingsSwitchPressed;
			}
			this._switches.Clear();
		}
		this._switches = null;
	}

	// Token: 0x040001E0 RID: 480
	public DisplayObject switchesContainer;

	// Token: 0x040001E1 RID: 481
	private List<SettingsSwitch> _switches;

	// Token: 0x040001E2 RID: 482
	private SettingsSwitch _selectedSwitch;

	// Token: 0x02000046 RID: 70
	// (Invoke) Token: 0x06000242 RID: 578
	public delegate void SettingsSwitchPanelDelegate(SettingsSwitchPanel settingsSwitchPanel, int optionIndex);
}
