using System;

// Token: 0x0200002D RID: 45
public class GirlFinderIcon : SpriteObject
{
	// Token: 0x060001B2 RID: 434 RVA: 0x0001357C File Offset: 0x0001177C
	public void Init()
	{
		base.button.Init();
		this.iconNote = (base.GetChildByName("GirlFinderIconNote") as SpriteObject);
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(this.girlDefinition);
		if (girlData.metStatus == GirlMetStatus.UNKNOWN)
		{
			base.button.ChangeOrigSpriteOfStateTransition(ButtonState.OVER, 0, this.girlDefinition.firstName.ToLower() + "_unknown");
			base.button.ChangeOrigSpriteOfStateTransition(ButtonState.DISABLED, 0, this.girlDefinition.firstName.ToLower() + "_unknown");
			base.button.ChangeStateSpriteOfStateTransition(ButtonState.OVER, 0, this.girlDefinition.firstName.ToLower() + "_unknown_over");
			base.button.ChangeStateSpriteOfStateTransition(ButtonState.DISABLED, 0, this.girlDefinition.firstName.ToLower() + "_unknown_disabled");
			this.sprite.SetSprite(this.girlDefinition.firstName.ToLower() + "_unknown");
		}
		this._girlLocation = this.girlDefinition.IsAtLocationAtTime(GameManager.System.Clock.Weekday(GameManager.System.Clock.TotalMinutesElapsed(360), true), GameManager.System.Clock.DayTime(GameManager.System.Clock.TotalMinutesElapsed(360)));
		if (girlData.metStatus != GirlMetStatus.MET)
		{
			this._girlLocation = this.girlDefinition.introLocation;
		}
		if (this.girlDefinition == GameManager.System.Location.currentGirl || this._girlLocation == null)
		{
			base.button.Disable();
			base.tooltip.Disable();
			if (this.girlDefinition != GameManager.System.Location.currentGirl)
			{
				if (this.girlDefinition.leavesTown)
				{
					this.iconNote.sprite.SetSprite("cell_app_finder_icon_note_out");
				}
				else
				{
					this.iconNote.sprite.SetSprite("cell_app_finder_icon_note_asleep");
				}
				this.iconNote.SetAlpha(0.62f, 0f);
			}
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00003B3E File Offset: 0x00001D3E
	public override string GetUniqueTooltipMessage()
	{
		if (this._girlLocation != null)
		{
			return this._girlLocation.fullName;
		}
		return string.Empty;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00003B62 File Offset: 0x00001D62
	protected override void Destructor()
	{
		base.Destructor();
	}

	// Token: 0x04000135 RID: 309
	private const string SPRITE_ICON_UNKNOWN = "_unknown";

	// Token: 0x04000136 RID: 310
	private const string SPRITE_ICON_UNKNOWN_OVER = "_unknown_over";

	// Token: 0x04000137 RID: 311
	private const string SPRITE_ICON_UNKNOWN_DISABLED = "_unknown_disabled";

	// Token: 0x04000138 RID: 312
	private const string ICON_NOTE_NONE = "cell_app_finder_icon_note_none";

	// Token: 0x04000139 RID: 313
	private const string ICON_NOTE_OUT = "cell_app_finder_icon_note_out";

	// Token: 0x0400013A RID: 314
	private const string ICON_NOTE_ASLEEP = "cell_app_finder_icon_note_asleep";

	// Token: 0x0400013B RID: 315
	public GirlDefinition girlDefinition;

	// Token: 0x0400013C RID: 316
	public SpriteObject iconNote;

	// Token: 0x0400013D RID: 317
	private LocationDefinition _girlLocation;
}
