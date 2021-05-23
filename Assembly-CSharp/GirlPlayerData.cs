using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class GirlPlayerData
{
	// Token: 0x060006A9 RID: 1705 RVA: 0x0000712E File Offset: 0x0000532E
	public GirlPlayerData(GirlDefinition girlDefinition, GirlSaveData girlSaveData)
	{
		this._girlDefinition = girlDefinition;
		this.ReadGirlSaveData(girlSaveData);
		this._dialogTriggerLog = new Dictionary<int, Dictionary<int, int>>();
	}

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x060006AA RID: 1706 RVA: 0x0000714F File Offset: 0x0000534F
	// (remove) Token: 0x060006AB RID: 1707 RVA: 0x00007168 File Offset: 0x00005368
	public event GirlPlayerData.GirlPlayerDataDelegate StatsChangedEvent;

	// Token: 0x060006AC RID: 1708 RVA: 0x00034744 File Offset: 0x00032944
	public void ReadGirlSaveData(GirlSaveData girlSaveData)
	{
		this.metStatus = (GirlMetStatus)girlSaveData.metStatus;
		this.relationshipLevel = girlSaveData.relationshipLevel;
		this.appetite = girlSaveData.appetite;
		this.inebriation = girlSaveData.inebriation;
		this.dayDated = girlSaveData.dayDated;
		this._photosEarned = ListUtils.Copy<int>(girlSaveData.photosEarned);
		this._uniqueGifts = ListUtils.Copy<int>(girlSaveData.uniqueGifts);
		this._collection = ListUtils.Copy<int>(girlSaveData.collection);
		this._hairstyles = ListUtils.Copy<int>(girlSaveData.hairstyles);
		this._outfits = ListUtils.Copy<int>(girlSaveData.outfits);
		this.hairstyle = girlSaveData.hairstyle;
		this.outfit = girlSaveData.outfit;
		this._details = ListUtils.CopyArray<bool>(girlSaveData.details);
		this._recentQuizzes = ListUtils.Copy<int>(girlSaveData.recentQuizzes);
		this._recentQuestions = ListUtils.Copy<int>(girlSaveData.recentQuestions);
		this.gotPanties = girlSaveData.gotPanties;
		this.lovePotionUsed = girlSaveData.lovePotionUsed;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00034848 File Offset: 0x00032A48
	public void WriteGirlSaveData(GirlSaveData girlSaveData)
	{
		girlSaveData.metStatus = (int)this.metStatus;
		girlSaveData.relationshipLevel = this.relationshipLevel;
		girlSaveData.appetite = this.appetite;
		girlSaveData.inebriation = this.inebriation;
		girlSaveData.dayDated = this.dayDated;
		girlSaveData.photosEarned = ListUtils.Copy<int>(this._photosEarned);
		girlSaveData.uniqueGifts = ListUtils.Copy<int>(this._uniqueGifts);
		girlSaveData.collection = ListUtils.Copy<int>(this._collection);
		girlSaveData.hairstyles = ListUtils.Copy<int>(this._hairstyles);
		girlSaveData.outfits = ListUtils.Copy<int>(this._outfits);
		girlSaveData.hairstyle = this.hairstyle;
		girlSaveData.outfit = this.outfit;
		girlSaveData.details = ListUtils.CopyArray<bool>(this._details);
		girlSaveData.recentQuizzes = ListUtils.Copy<int>(this._recentQuizzes);
		girlSaveData.recentQuestions = ListUtils.Copy<int>(this._recentQuestions);
		girlSaveData.gotPanties = this.gotPanties;
		girlSaveData.lovePotionUsed = this.lovePotionUsed;
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x00007181 File Offset: 0x00005381
	// (set) Token: 0x060006AF RID: 1711 RVA: 0x00007189 File Offset: 0x00005389
	public GirlMetStatus metStatus
	{
		get
		{
			return this._metStatus;
		}
		set
		{
			this._metStatus = value;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00007192 File Offset: 0x00005392
	// (set) Token: 0x060006B1 RID: 1713 RVA: 0x0000719A File Offset: 0x0000539A
	public int relationshipLevel
	{
		get
		{
			return this._relationshipLevel;
		}
		set
		{
			if (value > this._relationshipLevel)
			{
				this._relationshipLevel = Mathf.Min(value, 5);
			}
			SteamUtils.CheckPercentCompleteAchievement(true);
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x000071BB File Offset: 0x000053BB
	// (set) Token: 0x060006B3 RID: 1715 RVA: 0x0003494C File Offset: 0x00032B4C
	public int appetite
	{
		get
		{
			return this._appetite;
		}
		set
		{
			int num = Mathf.Clamp(value, 0, 12);
			if (num != this._appetite && this.StatsChangedEvent != null)
			{
				this.StatsChangedEvent(this);
			}
			this._appetite = num;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000071C3 File Offset: 0x000053C3
	// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00034990 File Offset: 0x00032B90
	public int inebriation
	{
		get
		{
			return this._inebriation;
		}
		set
		{
			int num = Mathf.Clamp(value, 0, 12);
			if (num != this._inebriation && this.StatsChangedEvent != null)
			{
				this.StatsChangedEvent(this);
			}
			this._inebriation = num;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000071CB File Offset: 0x000053CB
	// (set) Token: 0x060006B7 RID: 1719 RVA: 0x000071D3 File Offset: 0x000053D3
	public bool dayDated
	{
		get
		{
			return this._dayDated;
		}
		set
		{
			this._dayDated = value;
		}
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000071DC File Offset: 0x000053DC
	public bool AddPhotoEarned(int photoIndex)
	{
		if (!this.IsPhotoEarned(photoIndex) && photoIndex >= 0 && photoIndex < 4)
		{
			this._photosEarned.Add(photoIndex);
			SteamUtils.CheckPercentCompleteAchievement(true);
			return true;
		}
		return false;
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x0000720D File Offset: 0x0000540D
	public bool IsPhotoEarned(int photoIndex)
	{
		return this._photosEarned.Contains(photoIndex);
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0000721B File Offset: 0x0000541B
	public int EarnedPhotoCount()
	{
		return this._photosEarned.Count;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x000349D4 File Offset: 0x00032BD4
	public bool AddItemToUniqueGifts(ItemDefinition item)
	{
		if (item != null && this._girlDefinition.uniqueGiftList.Contains(item) && !this.IsItemInUniqueGifts(item))
		{
			this._uniqueGifts.Add(this._girlDefinition.uniqueGiftList.IndexOf(item));
			SteamUtils.CheckPercentCompleteAchievement(true);
			return true;
		}
		return false;
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00007228 File Offset: 0x00005428
	public bool IsItemInUniqueGifts(ItemDefinition item)
	{
		return item != null && this._uniqueGifts.Contains(this._girlDefinition.uniqueGiftList.IndexOf(item));
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00007255 File Offset: 0x00005455
	public int UniqueGiftCount()
	{
		return this._uniqueGifts.Count;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00034A34 File Offset: 0x00032C34
	public bool AddItemToCollection(ItemDefinition item)
	{
		if (item != null && this._girlDefinition.collection.Contains(item) && !this.IsItemInCollection(item))
		{
			this._collection.Add(this._girlDefinition.collection.IndexOf(item));
			if (this.ItemCollectionCount() % 3 == 0)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < this._girlDefinition.hairstyles.Count; i++)
				{
					if (!this.IsHairstyleUnlocked(i))
					{
						list.Add(i);
					}
				}
				if (list.Count > 0)
				{
					int num = list[UnityEngine.Random.Range(0, list.Count)];
					this.UnlockHairstyle(num);
					GameManager.Stage.cellNotifications.Notify(CellNotificationType.PROFILE, string.Concat(new string[]
					{
						"You've unlocked ",
						StringUtils.Possessize(this._girlDefinition.firstName),
						" \"",
						this._girlDefinition.hairstyles[num].styleName,
						"\" hairstyle!"
					}));
				}
			}
			SteamUtils.CheckPercentCompleteAchievement(true);
			return true;
		}
		return false;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00007262 File Offset: 0x00005462
	public bool IsItemInCollection(ItemDefinition item)
	{
		return item != null && this._collection.Contains(this._girlDefinition.collection.IndexOf(item));
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0000728F File Offset: 0x0000548F
	public int ItemCollectionCount()
	{
		return this._collection.Count;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0000729C File Offset: 0x0000549C
	public bool UnlockHairstyle(int hairstyleIndex)
	{
		if (hairstyleIndex >= 0 && hairstyleIndex < this._girlDefinition.hairstyles.Count && !this.IsHairstyleUnlocked(hairstyleIndex))
		{
			this._hairstyles.Add(hairstyleIndex);
			SteamUtils.CheckPercentCompleteAchievement(true);
			return true;
		}
		return false;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000072DC File Offset: 0x000054DC
	public bool IsHairstyleUnlocked(int hairstyleIndex)
	{
		return this._hairstyles.Contains(hairstyleIndex);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000072EA File Offset: 0x000054EA
	public int UnlockedHairstylesCount()
	{
		return this._hairstyles.Count;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x000072F7 File Offset: 0x000054F7
	public int GetRandomUnlockedHairstyle()
	{
		if (this._hairstyles.Count > 0)
		{
			return this._hairstyles[UnityEngine.Random.Range(0, this._hairstyles.Count)];
		}
		return 0;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00007328 File Offset: 0x00005528
	public bool UnlockOutfit(int outfitIndex)
	{
		if (outfitIndex >= 0 && outfitIndex < this._girlDefinition.outfits.Count && !this.IsOutfitUnlocked(outfitIndex))
		{
			this._outfits.Add(outfitIndex);
			SteamUtils.CheckPercentCompleteAchievement(true);
			return true;
		}
		return false;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00007368 File Offset: 0x00005568
	public bool IsOutfitUnlocked(int outfitIndex)
	{
		return this._outfits.Contains(outfitIndex);
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00007376 File Offset: 0x00005576
	public int UnlockedOutfitsCount()
	{
		return this._outfits.Count;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00007383 File Offset: 0x00005583
	public int GetRandomUnlockedOutfit()
	{
		if (this._outfits.Count > 0)
		{
			return this._outfits[UnityEngine.Random.Range(0, this._outfits.Count)];
		}
		return 0;
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060006C9 RID: 1737 RVA: 0x000073B4 File Offset: 0x000055B4
	// (set) Token: 0x060006CA RID: 1738 RVA: 0x000073BC File Offset: 0x000055BC
	public int hairstyle
	{
		get
		{
			return this._hairstyle;
		}
		set
		{
			this._hairstyle = value;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060006CB RID: 1739 RVA: 0x000073C5 File Offset: 0x000055C5
	// (set) Token: 0x060006CC RID: 1740 RVA: 0x000073CD File Offset: 0x000055CD
	public int outfit
	{
		get
		{
			return this._outfit;
		}
		set
		{
			this._outfit = value;
		}
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00034B64 File Offset: 0x00032D64
	public void KnowDetail(GirlDetailType type)
	{
		bool flag = this._details[(int)type];
		this._details[(int)type] = true;
		if (!flag)
		{
			string text = StringUtils.Titleize(type.ToString()).ToLower();
			if (type == GirlDetailType.EDUCATION && this._girlDefinition.secretGirl)
			{
				text = "homeworld";
			}
			GameManager.Stage.cellNotifications.Notify(CellNotificationType.PROFILE, string.Concat(new string[]
			{
				"You've learned ",
				StringUtils.Possessize(this._girlDefinition.firstName),
				" ",
				text,
				"!"
			}));
			SteamUtils.CheckPercentCompleteAchievement(true);
		}
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x000073D6 File Offset: 0x000055D6
	public bool IsDetailKnown(GirlDetailType type)
	{
		return this._details[(int)type];
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00034C0C File Offset: 0x00032E0C
	public int DetailKnownCount()
	{
		int num = 0;
		for (int i = 0; i < this._details.Length; i++)
		{
			if (this._details[i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x000073E0 File Offset: 0x000055E0
	public void AddRecentQuiz(int quizIndex)
	{
		while (this._recentQuizzes.Count >= 2)
		{
			this._recentQuizzes.RemoveAt(0);
		}
		this._recentQuizzes.Add(quizIndex);
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00007410 File Offset: 0x00005610
	public bool IsRecentQuiz(int quizIndex)
	{
		return this._recentQuizzes.Contains(quizIndex);
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x0000741E File Offset: 0x0000561E
	public void AddRecentQuestion(int questionIndex)
	{
		while (this._recentQuestions.Count >= 4)
		{
			this._recentQuestions.RemoveAt(0);
		}
		this._recentQuestions.Add(questionIndex);
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x0000744E File Offset: 0x0000564E
	public bool IsRecentQuestion(int questionIndex)
	{
		return this._recentQuestions.Contains(questionIndex);
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0000745C File Offset: 0x0000565C
	// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00007464 File Offset: 0x00005664
	public bool gotPanties
	{
		get
		{
			return this._gotPanties;
		}
		set
		{
			this._gotPanties = value;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0000746D File Offset: 0x0000566D
	// (set) Token: 0x060006D7 RID: 1751 RVA: 0x00007475 File Offset: 0x00005675
	public bool lovePotionUsed
	{
		get
		{
			return this._lovePotionUsed;
		}
		set
		{
			this._lovePotionUsed = value;
		}
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0000747E File Offset: 0x0000567E
	public void LogDialogTriggerLine(int triggerId, int setIndex, int lineIndex)
	{
		if (!this._dialogTriggerLog.ContainsKey(triggerId))
		{
			this._dialogTriggerLog[triggerId] = new Dictionary<int, int>();
		}
		this._dialogTriggerLog[triggerId][setIndex] = lineIndex;
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000074B5 File Offset: 0x000056B5
	public int GetLoggedDialogTriggerLineIndex(int triggerId, int setIndex)
	{
		if (!this._dialogTriggerLog.ContainsKey(triggerId) || !this._dialogTriggerLog[triggerId].ContainsKey(setIndex))
		{
			return -1;
		}
		return this._dialogTriggerLog[triggerId][setIndex];
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000074F3 File Offset: 0x000056F3
	public GirlDefinition GetGirlDefinition()
	{
		return this._girlDefinition;
	}

	// Token: 0x040007F9 RID: 2041
	public const int RELATIONSHIP_LEVEL_MAX = 5;

	// Token: 0x040007FA RID: 2042
	public const int APPETITE_MAX = 12;

	// Token: 0x040007FB RID: 2043
	public const int INEBRIATION_MAX = 12;

	// Token: 0x040007FC RID: 2044
	public const int RECENT_QUESTION_LIMIT = 4;

	// Token: 0x040007FD RID: 2045
	public const int RECENT_QUIZ_LIMIT = 2;

	// Token: 0x040007FE RID: 2046
	private GirlDefinition _girlDefinition;

	// Token: 0x040007FF RID: 2047
	private GirlMetStatus _metStatus;

	// Token: 0x04000800 RID: 2048
	private int _relationshipLevel;

	// Token: 0x04000801 RID: 2049
	private int _appetite;

	// Token: 0x04000802 RID: 2050
	private int _inebriation;

	// Token: 0x04000803 RID: 2051
	private bool _dayDated;

	// Token: 0x04000804 RID: 2052
	private List<int> _photosEarned;

	// Token: 0x04000805 RID: 2053
	private List<int> _uniqueGifts;

	// Token: 0x04000806 RID: 2054
	private List<int> _collection;

	// Token: 0x04000807 RID: 2055
	private List<int> _hairstyles;

	// Token: 0x04000808 RID: 2056
	private List<int> _outfits;

	// Token: 0x04000809 RID: 2057
	private int _hairstyle;

	// Token: 0x0400080A RID: 2058
	private int _outfit;

	// Token: 0x0400080B RID: 2059
	private bool[] _details;

	// Token: 0x0400080C RID: 2060
	private List<int> _recentQuizzes;

	// Token: 0x0400080D RID: 2061
	private List<int> _recentQuestions;

	// Token: 0x0400080E RID: 2062
	private bool _gotPanties;

	// Token: 0x0400080F RID: 2063
	private bool _lovePotionUsed;

	// Token: 0x04000810 RID: 2064
	private Dictionary<int, Dictionary<int, int>> _dialogTriggerLog;

	// Token: 0x02000129 RID: 297
	// (Invoke) Token: 0x060006DC RID: 1756
	public delegate void GirlPlayerDataDelegate(GirlPlayerData girlPlayerData);
}
