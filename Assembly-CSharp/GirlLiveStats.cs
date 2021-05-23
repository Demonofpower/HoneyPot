using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class GirlLiveStats : DisplayObject
{
	// Token: 0x060002B5 RID: 693 RVA: 0x00004435 File Offset: 0x00002635
	protected override void OnStart()
	{
		base.OnStart();
		if (!this._inited)
		{
			this.Init();
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00019DBC File Offset: 0x00017FBC
	public void Init()
	{
		if (this._inited)
		{
			return;
		}
		this.relationshipStats = base.GetChildByName("GirlLiveStatsRelationship");
		this.appetiteStats = base.GetChildByName("GirlLiveStatsAppetite");
		this.inebriationStats = base.GetChildByName("GirlLiveStatsInebriation");
		this._relationshipMeterContainer = this.relationshipStats.GetChildByName("GirlLiveStatsRelationshipMeter");
		this._relationshipMeter = new List<SpriteObject>();
		for (int i = 0; i < 5; i++)
		{
			this._relationshipMeter.Add(this._relationshipMeterContainer.GetChildByName("GirlLiveStatsRelationshipMeter" + i.ToString()) as SpriteObject);
		}
		this._appetiteMeterContainer = this.appetiteStats.GetChildByName("GirlLiveStatsAppetiteContainer");
		this._appetiteMeter = new List<SpriteObject>();
		for (int j = 0; j < 12; j++)
		{
			this._appetiteMeter.Add(this._appetiteMeterContainer.GetChildByName("GirlLiveStatsAppetiteMeter" + j.ToString()) as SpriteObject);
		}
		this._inebriationMeterContainer = this.inebriationStats.GetChildByName("GirlLiveStatsInebriationContainer");
		this._inebriationMeter = new List<SpriteObject>();
		for (int k = 0; k < 12; k++)
		{
			this._inebriationMeter.Add(this._inebriationMeterContainer.GetChildByName("GirlLiveStatsInebriationMeter" + k.ToString()) as SpriteObject);
		}
		this._inited = true;
		this._relationshipMeterOnCount = 5;
		this._appetiteMeterOnCount = 12;
		this._inebriationMeterOnCount = 12;
		this._animSequence = null;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00019F4C File Offset: 0x0001814C
	public void RefreshStats(GirlDefinition girlDef, bool animate = false)
	{
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(girlDef);
		TweenUtils.KillSequence(this._animSequence, true);
		if (animate)
		{
			this._animSequence = new Sequence();
			int num = 0;
			if (this._relationshipMeterOnCount < girlData.relationshipLevel)
			{
				for (int i = 0; i < this._relationshipMeter.Count; i++)
				{
					if (i + 1 >= this._relationshipMeterOnCount && i + 1 <= girlData.relationshipLevel)
					{
						this._animSequence.Insert((float)num * 0.1f, HOTween.To(this._relationshipMeter[i], 0.2f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine)));
						this._animSequence.Insert((float)num * 0.1f, HOTween.To(this._relationshipMeter[i].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
						num++;
					}
				}
			}
			else if (this._relationshipMeterOnCount > girlData.relationshipLevel)
			{
				for (int j = this._relationshipMeter.Count - 1; j >= 0; j--)
				{
					if (j <= this._relationshipMeterOnCount && j >= girlData.relationshipLevel)
					{
						this._animSequence.Insert((float)num * 0.1f, HOTween.To(this._relationshipMeter[j], 0.2f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine)));
						this._animSequence.Insert((float)num * 0.1f, HOTween.To(this._relationshipMeter[j].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(0.5f, 0.5f, 1f)).Ease(EaseType.EaseInBack)));
						num++;
					}
				}
			}
			this._relationshipMeterOnCount = girlData.relationshipLevel;
			int num2 = 0;
			if (this._appetiteMeterOnCount < girlData.appetite)
			{
				for (int k = 0; k < this._appetiteMeter.Count; k++)
				{
					if (k + 1 >= this._appetiteMeterOnCount && k + 1 <= girlData.appetite)
					{
						this._animSequence.Insert((float)num2 * 0.1f, HOTween.To(this._appetiteMeter[k], 0.2f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine)));
						this._animSequence.Insert((float)num2 * 0.1f, HOTween.To(this._appetiteMeter[k].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
						num2++;
					}
				}
			}
			else if (this._appetiteMeterOnCount > girlData.appetite)
			{
				for (int l = this._appetiteMeter.Count - 1; l >= 0; l--)
				{
					if (l <= this._appetiteMeterOnCount && l >= girlData.appetite)
					{
						this._animSequence.Insert((float)num2 * 0.1f, HOTween.To(this._appetiteMeter[l], 0.2f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine)));
						this._animSequence.Insert((float)num2 * 0.1f, HOTween.To(this._appetiteMeter[l].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(0.5f, 0.5f, 1f)).Ease(EaseType.EaseInBack)));
						num2++;
					}
				}
			}
			this._appetiteMeterOnCount = girlData.appetite;
			int num3 = 0;
			if (this._inebriationMeterOnCount < girlData.inebriation)
			{
				for (int m = 0; m < this._inebriationMeter.Count; m++)
				{
					if (m + 1 >= this._inebriationMeterOnCount && m + 1 <= girlData.inebriation)
					{
						this._animSequence.Insert((float)num3 * 0.1f, HOTween.To(this._inebriationMeter[m], 0.2f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseOutSine)));
						this._animSequence.Insert((float)num3 * 0.1f, HOTween.To(this._inebriationMeter[m].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).Ease(EaseType.EaseOutBack)));
						num3++;
					}
				}
			}
			else if (this._inebriationMeterOnCount > girlData.inebriation)
			{
				for (int n = this._inebriationMeter.Count - 1; n >= 0; n--)
				{
					if (n <= this._inebriationMeterOnCount && n >= girlData.inebriation)
					{
						this._animSequence.Insert((float)num3 * 0.1f, HOTween.To(this._inebriationMeter[n], 0.2f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.EaseInSine)));
						this._animSequence.Insert((float)num3 * 0.1f, HOTween.To(this._inebriationMeter[n].gameObj.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(0.5f, 0.5f, 1f)).Ease(EaseType.EaseInBack)));
						num3++;
					}
				}
			}
			this._inebriationMeterOnCount = girlData.inebriation;
			this._animSequence.Play();
		}
		else
		{
			for (int num4 = 0; num4 < this._relationshipMeter.Count; num4++)
			{
				if (num4 < girlData.relationshipLevel)
				{
					this._relationshipMeter[num4].SetAlpha(1f, 0f);
					this._relationshipMeter[num4].gameObj.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					this._relationshipMeter[num4].SetAlpha(0f, 0f);
					this._relationshipMeter[num4].gameObj.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
				}
			}
			this._relationshipMeterOnCount = girlData.relationshipLevel;
			for (int num5 = 0; num5 < this._appetiteMeter.Count; num5++)
			{
				if (num5 < girlData.appetite)
				{
					this._appetiteMeter[num5].SetAlpha(1f, 0f);
					this._appetiteMeter[num5].gameObj.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					this._appetiteMeter[num5].SetAlpha(0f, 0f);
					this._appetiteMeter[num5].gameObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				}
			}
			this._appetiteMeterOnCount = girlData.appetite;
			for (int num6 = 0; num6 < this._inebriationMeter.Count; num6++)
			{
				if (num6 < girlData.inebriation)
				{
					this._inebriationMeter[num6].SetAlpha(1f, 0f);
					this._inebriationMeter[num6].gameObj.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					this._inebriationMeter[num6].SetAlpha(0f, 0f);
					this._inebriationMeter[num6].gameObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				}
			}
			this._inebriationMeterOnCount = girlData.inebriation;
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00003AA4 File Offset: 0x00001CA4
	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000444E File Offset: 0x0000264E
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

	// Token: 0x060002BA RID: 698 RVA: 0x00004488 File Offset: 0x00002688
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

	// Token: 0x060002BB RID: 699 RVA: 0x0001A83C File Offset: 0x00018A3C
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._animSequence, false);
		this._animSequence = null;
		this._relationshipMeter.Clear();
		this._relationshipMeter = null;
		this._appetiteMeter.Clear();
		this._appetiteMeter = null;
		this._inebriationMeter.Clear();
		this._inebriationMeter = null;
	}

	// Token: 0x0400025C RID: 604
	public DisplayObject relationshipStats;

	// Token: 0x0400025D RID: 605
	public DisplayObject appetiteStats;

	// Token: 0x0400025E RID: 606
	public DisplayObject inebriationStats;

	// Token: 0x0400025F RID: 607
	private DisplayObject _relationshipMeterContainer;

	// Token: 0x04000260 RID: 608
	private List<SpriteObject> _relationshipMeter;

	// Token: 0x04000261 RID: 609
	private DisplayObject _appetiteMeterContainer;

	// Token: 0x04000262 RID: 610
	private List<SpriteObject> _appetiteMeter;

	// Token: 0x04000263 RID: 611
	private DisplayObject _inebriationMeterContainer;

	// Token: 0x04000264 RID: 612
	private List<SpriteObject> _inebriationMeter;

	// Token: 0x04000265 RID: 613
	private bool _inited;

	// Token: 0x04000266 RID: 614
	private int _relationshipMeterOnCount;

	// Token: 0x04000267 RID: 615
	private int _appetiteMeterOnCount;

	// Token: 0x04000268 RID: 616
	private int _inebriationMeterOnCount;

	// Token: 0x04000269 RID: 617
	private Sequence _animSequence;
}
