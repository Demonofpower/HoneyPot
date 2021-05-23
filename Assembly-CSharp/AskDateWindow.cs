using System;
using Holoville.HOTween;

// Token: 0x0200008A RID: 138
public class AskDateWindow : UIWindow
{
	// Token: 0x06000411 RID: 1041 RVA: 0x000239CC File Offset: 0x00021BCC
	public override void Init()
	{
		this.daySubWindow = (base.GetChildByName("AskDateDaySubWindow") as AskDateDaySubWindow);
		this.daySubWindow.Init();
		base.AddSubWindow(this.daySubWindow);
		this.daySubWindow.DaySelectedEvent += this.OnDaySelected;
		this.daytimeSubWindow = (base.GetChildByName("AskDateDaytimeSubWindow") as AskDateDaytimeSubWindow);
		this.daytimeSubWindow.Init();
		base.AddSubWindow(this.daytimeSubWindow);
		this.daytimeSubWindow.DaytimeSelectedEvent += this.OnDaytimeSelected;
		this.locationSubWindow = (base.GetChildByName("AskDateLocationSubWindow") as AskDateLocationSubWindow);
		this.locationSubWindow.Init();
		base.AddSubWindow(this.locationSubWindow);
		this.locationSubWindow.LocationSelectedEvent += this.OnLocationSelected;
		this._selectedDayIndex = -1;
		this._selectedDaytimeIndex = -1;
		this._selectedLocation = null;
		base.Init();
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00005207 File Offset: 0x00003407
	protected override void Show(Sequence animSequence)
	{
		base.ShowFirstSubWindow(this.daySubWindow);
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00005215 File Offset: 0x00003415
	protected override void Hide(Sequence animSequence)
	{
		base.HideLastSubWindow();
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0000521D File Offset: 0x0000341D
	private void OnDaySelected(int selectedDayIndex)
	{
		this._selectedDayIndex = selectedDayIndex;
		base.ShowSubWindow(this.daytimeSubWindow);
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00005232 File Offset: 0x00003432
	private void OnDaytimeSelected(int selectedDaytimeIndex)
	{
		this._selectedDaytimeIndex = selectedDaytimeIndex;
		base.ShowSubWindow(this.locationSubWindow);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00005247 File Offset: 0x00003447
	private void OnLocationSelected(LocationDefinition selectedLocation)
	{
		this._selectedLocation = selectedLocation;
		if (this.successDialogTrigger != null)
		{
			GameManager.System.Girl.TriggerDialog(this.successDialogTrigger, 0, false, -1);
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00023AC0 File Offset: 0x00021CC0
	protected override void Destructor()
	{
		base.Destructor();
		this.daySubWindow.DaySelectedEvent -= this.OnDaySelected;
		this.daySubWindow = null;
		this.daytimeSubWindow.DaytimeSelectedEvent -= this.OnDaytimeSelected;
		this.daytimeSubWindow = null;
		this.locationSubWindow.LocationSelectedEvent -= this.OnLocationSelected;
		this.locationSubWindow = null;
	}

	// Token: 0x040003BD RID: 957
	public DialogTriggerDefinition successDialogTrigger;

	// Token: 0x040003BE RID: 958
	public AskDateDaySubWindow daySubWindow;

	// Token: 0x040003BF RID: 959
	public AskDateDaytimeSubWindow daytimeSubWindow;

	// Token: 0x040003C0 RID: 960
	public AskDateLocationSubWindow locationSubWindow;

	// Token: 0x040003C1 RID: 961
	private int _selectedDayIndex;

	// Token: 0x040003C2 RID: 962
	private int _selectedDaytimeIndex;

	// Token: 0x040003C3 RID: 963
	private LocationDefinition _selectedLocation;
}
