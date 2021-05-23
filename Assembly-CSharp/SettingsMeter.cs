using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class SettingsMeter : DisplayObject
{
	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000223 RID: 547 RVA: 0x00003F7B File Offset: 0x0000217B
	// (remove) Token: 0x06000224 RID: 548 RVA: 0x00003F94 File Offset: 0x00002194
	public event SettingsMeter.SettingsMeterDelegate SettingsMeterChangeEvent;

	// Token: 0x06000225 RID: 549 RVA: 0x00017118 File Offset: 0x00015318
	public void Init(int meterValue)
	{
		this.ticksContainer = base.GetChildByName("SettingsMeterTickContainer");
		this._meterValue = meterValue;
		this._ticks = new List<SpriteObject>();
		for (int i = 0; i < 10; i++)
		{
			this._ticks.Add(this.ticksContainer.GetChildByName("SettingsMeterTick" + i.ToString()) as SpriteObject);
		}
		this._leftArrow = (base.GetChildByName("SettingsMeterLeftArrow") as SpriteObject);
		this._leftArrow.button.ButtonPressedEvent += this.OnLeftArrowPressed;
		this._rightArrow = (base.GetChildByName("SettingsMeterRightArrow") as SpriteObject);
		this._rightArrow.button.ButtonPressedEvent += this.OnRightArrowPressed;
		this.Refresh();
	}

	// Token: 0x06000226 RID: 550 RVA: 0x000171F4 File Offset: 0x000153F4
	public void Refresh()
	{
		for (int i = 0; i < this._ticks.Count; i++)
		{
			if (i < this._meterValue)
			{
				this._ticks[i].sprite.SetSprite("cell_app_settings_meter_on");
			}
			else
			{
				this._ticks[i].sprite.SetSprite("cell_app_settings_meter_off");
			}
		}
		if (this._meterValue <= 0)
		{
			this._leftArrow.button.Disable();
		}
		else
		{
			this._leftArrow.button.Enable();
		}
		if (this._meterValue >= 10)
		{
			this._rightArrow.button.Disable();
		}
		else
		{
			this._rightArrow.button.Enable();
		}
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00003FAD File Offset: 0x000021AD
	private void OnLeftArrowPressed(ButtonObject buttonObject)
	{
		this._meterValue = Mathf.Clamp(this._meterValue - 1, 0, 10);
		if (this.SettingsMeterChangeEvent != null)
		{
			this.SettingsMeterChangeEvent(this, this._meterValue);
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00003FE2 File Offset: 0x000021E2
	private void OnRightArrowPressed(ButtonObject buttonObject)
	{
		this._meterValue = Mathf.Clamp(this._meterValue + 1, 0, 10);
		if (this.SettingsMeterChangeEvent != null)
		{
			this.SettingsMeterChangeEvent(this, this._meterValue);
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x000172CC File Offset: 0x000154CC
	protected override void Destructor()
	{
		base.Destructor();
		if (this._leftArrow != null)
		{
			this._leftArrow.button.ButtonPressedEvent -= this.OnLeftArrowPressed;
		}
		if (this._rightArrow != null)
		{
			this._rightArrow.button.ButtonPressedEvent -= this.OnRightArrowPressed;
		}
		if (this._ticks != null)
		{
			this._ticks.Clear();
		}
		this._ticks = null;
	}

	// Token: 0x040001D0 RID: 464
	private const int METER_MIN = 0;

	// Token: 0x040001D1 RID: 465
	private const int METER_MAX = 10;

	// Token: 0x040001D2 RID: 466
	private const string TICK_OFF = "cell_app_settings_meter_off";

	// Token: 0x040001D3 RID: 467
	private const string TICK_ON = "cell_app_settings_meter_on";

	// Token: 0x040001D4 RID: 468
	public DisplayObject ticksContainer;

	// Token: 0x040001D5 RID: 469
	private int _meterValue;

	// Token: 0x040001D6 RID: 470
	private List<SpriteObject> _ticks;

	// Token: 0x040001D7 RID: 471
	private SpriteObject _leftArrow;

	// Token: 0x040001D8 RID: 472
	private SpriteObject _rightArrow;

	// Token: 0x02000042 RID: 66
	// (Invoke) Token: 0x0600022B RID: 555
	public delegate void SettingsMeterDelegate(SettingsMeter settingsMeter, int meterValue);
}
