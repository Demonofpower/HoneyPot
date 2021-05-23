using System;

// Token: 0x0200013A RID: 314
public class Timer
{
	// Token: 0x06000761 RID: 1889 RVA: 0x00007943 File Offset: 0x00005B43
	public Timer(float timerDuration, Action timerCallback)
	{
		this.duration = timerDuration;
		this._callback = timerCallback;
		this._cancelled = false;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00007960 File Offset: 0x00005B60
	public void Stop()
	{
		this._cancelled = true;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00007969 File Offset: 0x00005B69
	public void TriggerCallback()
	{
		if (!this._cancelled)
		{
			this._callback.DynamicInvoke(new object[0]);
		}
	}

	// Token: 0x04000897 RID: 2199
	public float duration;

	// Token: 0x04000898 RID: 2200
	public float startTimestamp;

	// Token: 0x04000899 RID: 2201
	private Action _callback;

	// Token: 0x0400089A RID: 2202
	private bool _cancelled;
}
