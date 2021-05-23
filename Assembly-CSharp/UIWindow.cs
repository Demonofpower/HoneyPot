using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class UIWindow : DisplayObject
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x0600044F RID: 1103 RVA: 0x000053AE File Offset: 0x000035AE
	// (remove) Token: 0x06000450 RID: 1104 RVA: 0x000053C7 File Offset: 0x000035C7
	public event UIWindow.UIWindowDelegate UIWindowShownEvent;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000451 RID: 1105 RVA: 0x000053E0 File Offset: 0x000035E0
	// (remove) Token: 0x06000452 RID: 1106 RVA: 0x000053F9 File Offset: 0x000035F9
	public event UIWindow.UIWindowDelegate UIWindowHiddenEvent;

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x00005412 File Offset: 0x00003612
	// (set) Token: 0x06000454 RID: 1108 RVA: 0x0000541A File Offset: 0x0000361A
	public bool IsBack
	{
		get
		{
			return this._isBack;
		}
		set
		{
			this._isBack = value;
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00005423 File Offset: 0x00003623
	public virtual void Init()
	{
		this.Refresh();
		this.PreShow();
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0000306D File Offset: 0x0000126D
	public virtual void Refresh()
	{
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00024560 File Offset: 0x00022760
	protected virtual void PreShow()
	{
		base.interactive = false;
		if (this._subWindows != null)
		{
			for (int i = 0; i < this._subWindows.Count; i++)
			{
				this._subWindows[i].gameObj.SetActive(false);
			}
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x000245B4 File Offset: 0x000227B4
	public void ShowWindow()
	{
		TweenUtils.KillSequence(this._animSequence, true);
		this._animSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.PostShow)));
		this.Show(this._animSequence);
		LocationBackground backgroundDefinition = GameManager.Stage.background.GetBackgroundDefinition(GameManager.System.Clock.DayTime(-1));
		if (!StringUtils.IsEmpty(this.tintBgColor))
		{
			GameManager.Stage.background.tintOverlay.SetColor(ColorUtils.HexToColor(this.tintBgColor), 0f);
			this._animSequence.Insert(0f, HOTween.To(GameManager.Stage.background.tintOverlay, 0.5f, new TweenParms().Prop("spriteAlpha", this.tintBgAlpha).Ease(EaseType.Linear)));
		}
		this._animSequence.Play();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void Show(Sequence animSequence)
	{
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00005431 File Offset: 0x00003631
	protected virtual void PostShow()
	{
		base.interactive = true;
		if (this.UIWindowShownEvent != null)
		{
			this.UIWindowShownEvent(this);
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00005451 File Offset: 0x00003651
	protected virtual void PreHide()
	{
		base.interactive = false;
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x000246A4 File Offset: 0x000228A4
	public void HideWindow()
	{
		TweenUtils.KillSequence(this._animSequence, true);
		this.PreHide();
		this._animSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.PostHide)));
		this.Hide(this._animSequence);
		if (!StringUtils.IsEmpty(this.tintBgColor))
		{
			this._animSequence.Insert(Mathf.Max(this._animSequence.duration - 0.5f, 0f), HOTween.To(GameManager.Stage.background.tintOverlay, 0.5f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear)));
		}
		this._animSequence.Play();
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0000306D File Offset: 0x0000126D
	protected virtual void Hide(Sequence animSequence)
	{
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0000545A File Offset: 0x0000365A
	protected virtual void PostHide()
	{
		if (this.UIWindowHiddenEvent != null)
		{
			this.UIWindowHiddenEvent(this);
		}
		UnityEngine.Object.Destroy(this.gameObj);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0000547E File Offset: 0x0000367E
	protected void AddSubWindow(UISubWindow subWindow)
	{
		if (this._subWindows == null)
		{
			this._subWindows = new List<UISubWindow>();
		}
		this._subWindows.Add(subWindow);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00024768 File Offset: 0x00022968
	protected void ShowFirstSubWindow(UISubWindow subWindow)
	{
		if (this._subWindows == null || !this._subWindows.Contains(subWindow) || this._activeSubWindow != null)
		{
			return;
		}
		this._activeSubWindow = subWindow;
		this._activeSubWindow.gameObj.SetActive(true);
		this._activeSubWindow.Show(this._animSequence, 0f);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000054A2 File Offset: 0x000036A2
	protected void HideLastSubWindow()
	{
		if (this._subWindows == null || this._activeSubWindow == null)
		{
			return;
		}
		this._activeSubWindow.Hide(this._animSequence, 0f);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x000247D4 File Offset: 0x000229D4
	protected void ShowNextSubWindow()
	{
		if (this._subWindows == null)
		{
			return;
		}
		UISubWindow subWindow;
		if (this._activeSubWindow != null)
		{
			if (this._subWindows.IndexOf(this._activeSubWindow) < this._subWindows.Count - 1)
			{
				subWindow = this._subWindows[this._subWindows.IndexOf(this._activeSubWindow) + 1];
			}
			else
			{
				subWindow = this._subWindows[0];
			}
		}
		else
		{
			subWindow = this._subWindows[0];
		}
		this.ShowSubWindow(subWindow);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0002486C File Offset: 0x00022A6C
	protected void ShowPreviousSubWindow()
	{
		if (this._subWindows == null)
		{
			return;
		}
		UISubWindow subWindow;
		if (this._activeSubWindow != null)
		{
			if (this._subWindows.IndexOf(this._activeSubWindow) > 0)
			{
				subWindow = this._subWindows[this._subWindows.IndexOf(this._activeSubWindow) - 1];
			}
			else
			{
				subWindow = this._subWindows[this._subWindows.Count - 1];
			}
		}
		else
		{
			subWindow = this._subWindows[this._subWindows.Count - 1];
		}
		this.ShowSubWindow(subWindow);
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00024910 File Offset: 0x00022B10
	protected void ShowSubWindow(UISubWindow subWindow)
	{
		if (this._subWindows == null || !this._subWindows.Contains(subWindow))
		{
			return;
		}
		TweenUtils.KillSequence(this._animSequence, true);
		this.PreSubWindowShow();
		this._animSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.PostSubWindowShow)));
		float offsetDelay = 0f;
		if (this._activeSubWindow != null)
		{
			this._activeSubWindow.Hide(this._animSequence, offsetDelay);
			offsetDelay = this._animSequence.duration;
		}
		this._incomingSubWindow = subWindow;
		this._incomingSubWindow.Show(this._animSequence, offsetDelay);
		offsetDelay = this._animSequence.duration;
		this._animSequence.Play();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000249D4 File Offset: 0x00022BD4
	private void PreSubWindowShow()
	{
		for (int i = 0; i < this._subWindows.Count; i++)
		{
			this._subWindows[i].gameObj.SetActive(true);
		}
		this.PreHide();
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00024A1C File Offset: 0x00022C1C
	private void PostSubWindowShow()
	{
		this.PostShow();
		this._activeSubWindow = this._incomingSubWindow;
		this._incomingSubWindow = null;
		for (int i = 0; i < this._subWindows.Count; i++)
		{
			if (this._subWindows[i] != this._activeSubWindow)
			{
				this._subWindows[i].gameObj.SetActive(false);
			}
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000054D7 File Offset: 0x000036D7
	public bool ExecuteBackCommand()
	{
		if (this._subWindows != null && this._subWindows.Count > 1 && this._subWindows.IndexOf(this._activeSubWindow) > 0)
		{
			this.ShowPreviousSubWindow();
			return true;
		}
		return false;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00005515 File Offset: 0x00003715
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._animSequence != null && !this._animSequence.isPaused)
		{
			this._animSequence.Pause();
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0000554F File Offset: 0x0000374F
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._animSequence != null && this._animSequence.isPaused)
		{
			this._animSequence.Play();
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00024A94 File Offset: 0x00022C94
	protected override void Destructor()
	{
		TweenUtils.KillSequence(this._animSequence, false);
		this._animSequence = null;
		this._activeSubWindow = null;
		this._incomingSubWindow = null;
		if (this._subWindows != null)
		{
			this._subWindows.Clear();
		}
		this._subWindows = null;
		base.Destructor();
	}

	// Token: 0x040003D7 RID: 983
	public string tintBgColor;

	// Token: 0x040003D8 RID: 984
	public float tintBgAlpha;

	// Token: 0x040003D9 RID: 985
	private bool _isBack;

	// Token: 0x040003DA RID: 986
	private Sequence _animSequence;

	// Token: 0x040003DB RID: 987
	private List<UISubWindow> _subWindows;

	// Token: 0x040003DC RID: 988
	private UISubWindow _incomingSubWindow;

	// Token: 0x040003DD RID: 989
	private UISubWindow _activeSubWindow;

	// Token: 0x02000095 RID: 149
	// (Invoke) Token: 0x0600046C RID: 1132
	public delegate void UIWindowDelegate(UIWindow uiWindow);
}
