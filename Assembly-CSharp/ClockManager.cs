using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class ClockManager : MonoBehaviour
{
	// Token: 0x06000608 RID: 1544 RVA: 0x00006870 File Offset: 0x00004A70
	public int TotalMinutesElapsed(int offset = 0)
	{
		return this._totalMinutesElapsed + offset;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0000687A File Offset: 0x00004A7A
	public void SetTotalMinutesElapsed(int minutes)
	{
		this._totalMinutesElapsed = minutes;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00006883 File Offset: 0x00004A83
	public void ProgressDaytime()
	{
		this._totalMinutesElapsed += 360;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00006897 File Offset: 0x00004A97
	public int DayMinutesElapsed(int minutes = -1)
	{
		if (minutes < 0)
		{
			minutes = this.TotalMinutesElapsed(0);
		}
		return (int)Mathf.Floor((float)(minutes % 1440));
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x000068B7 File Offset: 0x00004AB7
	public int Minute(int minutes = -1)
	{
		if (minutes < 0)
		{
			minutes = this.TotalMinutesElapsed(0);
		}
		return (int)Mathf.Floor((float)(minutes % 60));
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x000068D4 File Offset: 0x00004AD4
	public int MilitaryHour(int minutes = -1)
	{
		if (minutes < 0)
		{
			minutes = this.TotalMinutesElapsed(0);
		}
		return (int)Mathf.Floor((float)(minutes % 1440 / 60));
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0002EC44 File Offset: 0x0002CE44
	public int Hour(int minutes = -1)
	{
		int num = this.MilitaryHour(minutes);
		if (num <= 0)
		{
			num += 12;
		}
		else if (num > 12)
		{
			num -= 12;
		}
		return num;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002EC78 File Offset: 0x0002CE78
	public string Meridiem(int minutes = -1)
	{
		int num = this.MilitaryHour(minutes);
		if (num <= 11)
		{
			return "am";
		}
		return "pm";
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002ECA0 File Offset: 0x0002CEA0
	public int CalendarDay(int minutes = -1, bool nightSameDay = false, bool normalize = true)
	{
		if (minutes < 0)
		{
			minutes = this.TotalMinutesElapsed(0);
		}
		int i = (int)Mathf.Floor((float)(minutes / 1440)) + 1;
		if (nightSameDay && this.DayTime(minutes) == GameClockDaytime.NIGHT)
		{
			i--;
		}
		if (normalize)
		{
			while (i > 30)
			{
				i -= 30;
			}
		}
		return i;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002ED00 File Offset: 0x0002CF00
	public GameClockWeekday Weekday(int minutes = -1, bool nightSameDay = false)
	{
		int num = (this.CalendarDay(minutes, nightSameDay, false) - 1) % 7;
		if (num >= 0)
		{
			return (GameClockWeekday)num;
		}
		return GameClockWeekday.SUNDAY;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002ED28 File Offset: 0x0002CF28
	public GameClockDaytime DayTime(int minutes = -1)
	{
		int num = this.DayMinutesElapsed(minutes);
		if (num < 360)
		{
			return GameClockDaytime.NIGHT;
		}
		if (num < 720)
		{
			return GameClockDaytime.MORNING;
		}
		if (num < 1080)
		{
			return GameClockDaytime.AFTERNOON;
		}
		return GameClockDaytime.EVENING;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000068F7 File Offset: 0x00004AF7
	public GameClockDaytime NextDayTime(GameClockDaytime daytime)
	{
		if (daytime == GameClockDaytime.MORNING)
		{
			return GameClockDaytime.AFTERNOON;
		}
		if (daytime == GameClockDaytime.AFTERNOON)
		{
			return GameClockDaytime.EVENING;
		}
		if (daytime == GameClockDaytime.EVENING)
		{
			return GameClockDaytime.NIGHT;
		}
		return GameClockDaytime.MORNING;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00006914 File Offset: 0x00004B14
	public float DayTimePercentElapsed(int minutes = -1)
	{
		return (float)this.DayMinutesElapsed(minutes) % 360f / 360f;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0000692A File Offset: 0x00004B2A
	public bool IsRightNow(int minutes)
	{
		return minutes == this.TotalMinutesElapsed(0);
	}

	// Token: 0x04000762 RID: 1890
	public const int MINUTES_PER_DAYTIME = 360;

	// Token: 0x04000763 RID: 1891
	public const int MINUTES_PER_HOUR = 60;

	// Token: 0x04000764 RID: 1892
	public const int HOURS_PER_DAY = 24;

	// Token: 0x04000765 RID: 1893
	public const int DAYS_PER_WEEK = 7;

	// Token: 0x04000766 RID: 1894
	public const string MERIDIEM_ANTE = "am";

	// Token: 0x04000767 RID: 1895
	public const string MERIDIEM_POST = "pm";

	// Token: 0x04000768 RID: 1896
	private int _totalMinutesElapsed;
}
