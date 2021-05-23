using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class TimerManager : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0003756C File Offset: 0x0003576C
	public Timer New(float duration, Action callback)
	{
		Timer timer = new Timer(duration, callback);
		timer.startTimestamp = GameManager.System.Lifetime(true);
		this._timers.Add(timer);
		return timer;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x000375A0 File Offset: 0x000357A0
	private void Update()
	{
		if (this._paused)
		{
			return;
		}
		for (int i = 0; i < this._timers.Count; i++)
		{
			if (GameManager.System.Lifetime(true) - this._timers[i].startTimestamp >= this._timers[i].duration)
			{
				this._timers[i].TriggerCallback();
				this._timers.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0000799B File Offset: 0x00005B9B
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x000079B0 File Offset: 0x00005BB0
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
	}

	// Token: 0x0400089B RID: 2203
	private List<Timer> _timers = new List<Timer>();

	// Token: 0x0400089C RID: 2204
	private bool _paused;
}
