using System;
using System.Collections.Generic;

// Token: 0x02000036 RID: 54
public class GirlProfileStyleSwitch : DisplayObject
{
	// Token: 0x14000014 RID: 20
	// (add) Token: 0x060001DD RID: 477 RVA: 0x00003CEB File Offset: 0x00001EEB
	// (remove) Token: 0x060001DE RID: 478 RVA: 0x00003D04 File Offset: 0x00001F04
	public event GirlProfileStyleSwitch.GirlProfileStyleSwitchDelegate StyleSwitchPressedEvent;

	// Token: 0x060001DF RID: 479 RVA: 0x00014AC0 File Offset: 0x00012CC0
	public void Init(GirlDefinition girlDefinition, GirlPlayerData girlPlayerData, int index, bool hairstyle)
	{
		this.background = (base.GetChildByName("GirlProfileStyleSwitchBackground") as SpriteObject);
		this.styleLabel = (base.GetChildByName("GirlProfileStyleSwitchLabel") as LabelObject);
		this.styleIndex = index;
		this.unlocked = false;
		List<GirlStyle> list;
		if (hairstyle)
		{
			list = girlDefinition.hairstyles;
		}
		else
		{
			list = girlDefinition.outfits;
		}
		if (this.styleIndex >= list.Count)
		{
			this.styleLabel.SetText("-Random-");
			if ((hairstyle && girlPlayerData.UnlockedHairstylesCount() > 1) || (!hairstyle && girlPlayerData.UnlockedOutfitsCount() > 1))
			{
				this.unlocked = true;
			}
		}
		else if ((hairstyle && girlPlayerData.IsHairstyleUnlocked(this.styleIndex)) || (!hairstyle && girlPlayerData.IsOutfitUnlocked(this.styleIndex)))
		{
			this.styleLabel.SetText(list[this.styleIndex].styleName);
			this.unlocked = true;
		}
		else
		{
			this.styleLabel.SetText("???");
		}
		base.button.ButtonPressedEvent += this.OnButtonPressed;
		if (!this.unlocked)
		{
			base.button.ToggleTransitions(false);
			base.button.Disable();
			base.SetChildAlpha(0.5f, 0f);
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00003D1D File Offset: 0x00001F1D
	private void OnButtonPressed(ButtonObject buttonObject)
	{
		if (this.unlocked && this.StyleSwitchPressedEvent != null)
		{
			this.StyleSwitchPressedEvent(this);
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00003D41 File Offset: 0x00001F41
	protected override void Destructor()
	{
		base.Destructor();
		base.button.ButtonPressedEvent -= this.OnButtonPressed;
	}

	// Token: 0x04000177 RID: 375
	public SpriteObject background;

	// Token: 0x04000178 RID: 376
	public LabelObject styleLabel;

	// Token: 0x04000179 RID: 377
	public int styleIndex;

	// Token: 0x0400017A RID: 378
	public bool unlocked;

	// Token: 0x02000037 RID: 55
	// (Invoke) Token: 0x060001E3 RID: 483
	public delegate void GirlProfileStyleSwitchDelegate(GirlProfileStyleSwitch styleSwitch);
}
