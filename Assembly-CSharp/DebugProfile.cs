using System;
using System.Collections.Generic;

// Token: 0x020000B8 RID: 184
public class DebugProfile : Definition
{
	// Token: 0x0400047B RID: 1147
	public int settingsMusicVol;

	// Token: 0x0400047C RID: 1148
	public int settingsSoundVol;

	// Token: 0x0400047D RID: 1149
	public int settingsVoiceVol;

	// Token: 0x0400047E RID: 1150
	public SettingsVoice settingsVoice;

	// Token: 0x0400047F RID: 1151
	public SettingsLimit settingsLimit;

	// Token: 0x04000480 RID: 1152
	public SettingsDifficulty settingsDifficulty;

	// Token: 0x04000481 RID: 1153
	public SettingsGender settingsGender;

	// Token: 0x04000482 RID: 1154
	public int days;

	// Token: 0x04000483 RID: 1155
	public int hours;

	// Token: 0x04000484 RID: 1156
	public int minutes;

	// Token: 0x04000485 RID: 1157
	public GirlDefinition currentGirl;

	// Token: 0x04000486 RID: 1158
	public LocationDefinition currentLocation;

	// Token: 0x04000487 RID: 1159
	public int money;

	// Token: 0x04000488 RID: 1160
	public int hunie;

	// Token: 0x04000489 RID: 1161
	public int successfulDateCount;

	// Token: 0x0400048A RID: 1162
	public List<DebugProfileTrait> traits = new List<DebugProfileTrait>();

	// Token: 0x0400048B RID: 1163
	public string wrappedItems;

	// Token: 0x0400048C RID: 1164
	public List<ItemDefinition> inventoryItems = new List<ItemDefinition>();

	// Token: 0x0400048D RID: 1165
	public List<ItemDefinition> giftItems = new List<ItemDefinition>();

	// Token: 0x0400048E RID: 1166
	public List<ItemDefinition> dateGiftItems = new List<ItemDefinition>();

	// Token: 0x0400048F RID: 1167
	public List<DebugProfileGirl> girls = new List<DebugProfileGirl>();

	// Token: 0x04000490 RID: 1168
	public List<MessageDefinition> messages = new List<MessageDefinition>();

	// Token: 0x04000491 RID: 1169
	public int failureDateCount;

	// Token: 0x04000492 RID: 1170
	public int drinksGivenOut;

	// Token: 0x04000493 RID: 1171
	public int chatSessionCount;

	// Token: 0x04000494 RID: 1172
	public bool endingSceneShown;
}
