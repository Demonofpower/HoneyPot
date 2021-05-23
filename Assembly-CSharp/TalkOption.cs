using System;

// Token: 0x0200008F RID: 143
public class TalkOption : DisplayObject
{
	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06000431 RID: 1073 RVA: 0x000052EB File Offset: 0x000034EB
	// (remove) Token: 0x06000432 RID: 1074 RVA: 0x00005304 File Offset: 0x00003504
	public event TalkOption.TalkOptionDelegate OptionClickedEvent;

	// Token: 0x06000433 RID: 1075 RVA: 0x00024128 File Offset: 0x00022328
	public void Init(DialogSceneResponseOption resOption)
	{
		this.responseOption = resOption;
		this.contentLabel = (base.GetChildByName("TalkOptionLabel") as LabelObject);
		if (this.responseOption.secondary && GameManager.System.Player.settingsGender == SettingsGender.FEMALE)
		{
			this.contentLabel.SetText(this.responseOption.secondaryText);
		}
		else
		{
			this.contentLabel.SetText(this.responseOption.text);
		}
		this.origLocalY = base.localY;
		base.button.ButtonPressedEvent += this.OnButtonPress;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0000531D File Offset: 0x0000351D
	private void OnButtonPress(ButtonObject buttonObject)
	{
		if (this.OptionClickedEvent != null)
		{
			this.OptionClickedEvent(this);
		}
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00005336 File Offset: 0x00003536
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPress;
	}

	// Token: 0x040003CF RID: 975
	public DialogSceneResponseOption responseOption;

	// Token: 0x040003D0 RID: 976
	public LabelObject contentLabel;

	// Token: 0x040003D1 RID: 977
	public float origLocalY;

	// Token: 0x02000090 RID: 144
	// (Invoke) Token: 0x06000437 RID: 1079
	public delegate void TalkOptionDelegate(TalkOption talkOption);
}
