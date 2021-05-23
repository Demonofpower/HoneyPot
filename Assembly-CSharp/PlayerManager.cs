using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class PlayerManager : MonoBehaviour
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060006F0 RID: 1776 RVA: 0x000075C6 File Offset: 0x000057C6
	// (remove) Token: 0x060006F1 RID: 1777 RVA: 0x000075DF File Offset: 0x000057DF
	public event PlayerManager.PlayerManagerGirlPlayerDataDelegate GirlStatsChangedEvent;

	// Token: 0x060006F2 RID: 1778 RVA: 0x00034DBC File Offset: 0x00032FBC
	private void Clear()
	{
		this._tutorialComplete = false;
		this._tutorialStep = -1;
		this._cellphoneUnlocked = false;
		this._settingsDifficulty = SettingsDifficulty.MEDIUM;
		this._settingsGender = SettingsGender.MALE;
		this._money = -1;
		this._hunie = -1;
		this._successfulDateCount = -1;
		this._currentGirl = null;
		this._currentLocation = null;
		this._traitLevels.Clear();
		this._inventory = null;
		this._gifts = null;
		this._dateGifts = null;
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			girlPlayerData.StatsChangedEvent -= this.OnGirlStatsChanged;
		}
		this._girls.Clear();
		this._messages.Clear();
		this._storeGifts = null;
		this._storeUnique = null;
		this._storeFood = null;
		this._storeDrinks = null;
		this._failureDateCount = 1;
		this._drinksGivenOut = 1;
		this._chatSessionCount = 1;
		this._endingSceneShown = false;
		this._pantiesTurnedIn.Clear();
		this._alphaModeActive = false;
		this._alphaModeWins = -1;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00034EF4 File Offset: 0x000330F4
	public void ReadSaveFile(SaveFile saveFile)
	{
		this.Clear();
		this.tutorialComplete = saveFile.tutorialComplete;
		this.tutorialStep = saveFile.tutorialStep;
		this.cellphoneUnlocked = saveFile.cellphoneUnlocked;
		this.settingsDifficulty = (SettingsDifficulty)saveFile.settingsDifficulty;
		this.settingsGender = (SettingsGender)saveFile.settingsGender;
		this.money = saveFile.money;
		this.hunie = saveFile.hunie;
		this.successfulDateCount = saveFile.successfulDateCount;
		this.currentGirl = GameManager.Data.Girls.Get(saveFile.currentGirl);
		this.currentLocation = GameManager.Data.Locations.Get(saveFile.currentLocation);
		foreach (int key in saveFile.traitLevels.Keys)
		{
			this._traitLevels.Add((PlayerTraitType)key, saveFile.traitLevels[key]);
		}
		this._inventory = new InventoryItemPlayerData[30];
		for (int i = 0; i < this._inventory.Length; i++)
		{
			this._inventory[i] = new InventoryItemPlayerData(saveFile.inventory[i]);
		}
		this._gifts = new InventoryItemPlayerData[6];
		for (int j = 0; j < this._gifts.Length; j++)
		{
			this._gifts[j] = new InventoryItemPlayerData(saveFile.gifts[j]);
		}
		this._dateGifts = new InventoryItemPlayerData[6];
		for (int k = 0; k < this._dateGifts.Length; k++)
		{
			this._dateGifts[k] = new InventoryItemPlayerData(saveFile.dateGifts[k]);
		}
		foreach (int num in saveFile.girls.Keys)
		{
			GirlDefinition girlDefinition = GameManager.Data.Girls.Get(num);
			GirlPlayerData girlPlayerData = new GirlPlayerData(girlDefinition, saveFile.girls[num]);
			girlPlayerData.StatsChangedEvent += this.OnGirlStatsChanged;
			this._girls.Add(girlDefinition, girlPlayerData);
		}
		for (int l = 0; l < saveFile.messages.Count; l++)
		{
			MessagePlayerData messagePlayerData = new MessagePlayerData();
			messagePlayerData.ReadSaveData(saveFile.messages[l]);
			this._messages.Add(messagePlayerData);
		}
		this._storeGifts = new StoreItemPlayerData[12];
		for (int m = 0; m < 12; m++)
		{
			this._storeGifts[m] = new StoreItemPlayerData(saveFile.storeGifts[m]);
		}
		this._storeUnique = new StoreItemPlayerData[12];
		for (int n = 0; n < 12; n++)
		{
			this._storeUnique[n] = new StoreItemPlayerData(saveFile.storeUnique[n]);
		}
		this._storeFood = new StoreItemPlayerData[12];
		for (int num2 = 0; num2 < 12; num2++)
		{
			this._storeFood[num2] = new StoreItemPlayerData(saveFile.storeFood[num2]);
		}
		this._storeDrinks = new StoreItemPlayerData[12];
		for (int num3 = 0; num3 < 12; num3++)
		{
			this._storeDrinks[num3] = new StoreItemPlayerData(saveFile.storeDrinks[num3]);
		}
		this.failureDateCount = saveFile.failureDateCount;
		this.drinksGivenOut = saveFile.drinksGivenOut;
		this.chatSessionCount = saveFile.chatSessionCount;
		this.endingSceneShown = saveFile.endingSceneShown;
		this.pantiesTurnedIn = ListUtils.Copy<int>(saveFile.pantiesTurnedIn);
		this.alphaModeActive = saveFile.alphaModeActive;
		this.alphaModeWins = saveFile.alphaModeWins;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x000352D8 File Offset: 0x000334D8
	public void WriteSaveFile(SaveFile saveFile)
	{
		saveFile.tutorialComplete = this.tutorialComplete;
		saveFile.tutorialStep = this.tutorialStep;
		saveFile.cellphoneUnlocked = this.cellphoneUnlocked;
		saveFile.settingsDifficulty = (int)this.settingsDifficulty;
		saveFile.settingsGender = (int)this.settingsGender;
		saveFile.money = this.money;
		saveFile.hunie = this.hunie;
		saveFile.successfulDateCount = this.successfulDateCount;
		saveFile.currentGirl = this.currentGirl.id;
		saveFile.currentLocation = this.currentLocation.id;
		foreach (PlayerTraitType key in this._traitLevels.Keys)
		{
			saveFile.traitLevels[(int)key] = this._traitLevels[key];
		}
		for (int i = 0; i < this._inventory.Length; i++)
		{
			this._inventory[i].WriteSaveData(saveFile.inventory[i]);
		}
		for (int j = 0; j < this._gifts.Length; j++)
		{
			this._gifts[j].WriteSaveData(saveFile.gifts[j]);
		}
		for (int k = 0; k < this._dateGifts.Length; k++)
		{
			this._dateGifts[k].WriteSaveData(saveFile.dateGifts[k]);
		}
		foreach (GirlDefinition girlDefinition in this._girls.Keys)
		{
			this._girls[girlDefinition].WriteGirlSaveData(saveFile.girls[girlDefinition.id]);
		}
		saveFile.messages.Clear();
		for (int l = 0; l < this._messages.Count; l++)
		{
			MessageSaveData messageSaveData = new MessageSaveData();
			this._messages[l].WriteSaveData(messageSaveData);
			saveFile.messages.Add(messageSaveData);
		}
		for (int m = 0; m < this._storeGifts.Length; m++)
		{
			this._storeGifts[m].WriteSaveData(saveFile.storeGifts[m]);
		}
		for (int n = 0; n < this._storeUnique.Length; n++)
		{
			this._storeUnique[n].WriteSaveData(saveFile.storeUnique[n]);
		}
		for (int num = 0; num < this._storeFood.Length; num++)
		{
			this._storeFood[num].WriteSaveData(saveFile.storeFood[num]);
		}
		for (int num2 = 0; num2 < this._storeDrinks.Length; num2++)
		{
			this._storeDrinks[num2].WriteSaveData(saveFile.storeDrinks[num2]);
		}
		saveFile.failureDateCount = this.failureDateCount;
		saveFile.drinksGivenOut = this.drinksGivenOut;
		saveFile.chatSessionCount = this.chatSessionCount;
		saveFile.endingSceneShown = this.endingSceneShown;
		saveFile.pantiesTurnedIn = ListUtils.Copy<int>(this.pantiesTurnedIn);
		saveFile.alphaModeActive = this.alphaModeActive;
		saveFile.alphaModeWins = this.alphaModeWins;
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000075F8 File Offset: 0x000057F8
	// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00007600 File Offset: 0x00005800
	public bool tutorialComplete
	{
		get
		{
			return this._tutorialComplete;
		}
		set
		{
			this._tutorialComplete = value;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00007609 File Offset: 0x00005809
	// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00007611 File Offset: 0x00005811
	public int tutorialStep
	{
		get
		{
			return this._tutorialStep;
		}
		set
		{
			this._tutorialStep = value;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0000761A File Offset: 0x0000581A
	// (set) Token: 0x060006FA RID: 1786 RVA: 0x00035644 File Offset: 0x00033844
	public bool cellphoneUnlocked
	{
		get
		{
			return this._cellphoneUnlocked;
		}
		set
		{
			this._cellphoneUnlocked = value;
			if (!this._cellphoneUnlocked)
			{
				GameManager.Stage.uiTop.buttonHuniebee.SetAlpha(0f, 0f);
				GameManager.Stage.uiTop.buttonHuniebeeOverlay.SetAlpha(0f, 0f);
			}
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x00007622 File Offset: 0x00005822
	// (set) Token: 0x060006FC RID: 1788 RVA: 0x0000762A File Offset: 0x0000582A
	public SettingsDifficulty settingsDifficulty
	{
		get
		{
			return this._settingsDifficulty;
		}
		set
		{
			this._settingsDifficulty = value;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x00007633 File Offset: 0x00005833
	// (set) Token: 0x060006FE RID: 1790 RVA: 0x0000763B File Offset: 0x0000583B
	public SettingsGender settingsGender
	{
		get
		{
			return this._settingsGender;
		}
		set
		{
			this._settingsGender = value;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x00007644 File Offset: 0x00005844
	// (set) Token: 0x06000700 RID: 1792 RVA: 0x0000764C File Offset: 0x0000584C
	public int failureDateCount
	{
		get
		{
			return this._failureDateCount;
		}
		set
		{
			this._failureDateCount = value;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x00007655 File Offset: 0x00005855
	// (set) Token: 0x06000702 RID: 1794 RVA: 0x0000765D File Offset: 0x0000585D
	public int drinksGivenOut
	{
		get
		{
			return this._drinksGivenOut;
		}
		set
		{
			this._drinksGivenOut = value;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x00007666 File Offset: 0x00005866
	// (set) Token: 0x06000704 RID: 1796 RVA: 0x0000766E File Offset: 0x0000586E
	public int chatSessionCount
	{
		get
		{
			return this._chatSessionCount;
		}
		set
		{
			this._chatSessionCount = value;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x00007677 File Offset: 0x00005877
	// (set) Token: 0x06000706 RID: 1798 RVA: 0x0000767F File Offset: 0x0000587F
	public bool endingSceneShown
	{
		get
		{
			return this._endingSceneShown;
		}
		set
		{
			this._endingSceneShown = value;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x00007688 File Offset: 0x00005888
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x00007690 File Offset: 0x00005890
	public List<int> pantiesTurnedIn
	{
		get
		{
			return this._pantiesTurnedIn;
		}
		set
		{
			this._pantiesTurnedIn = value;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x00007699 File Offset: 0x00005899
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x000076A1 File Offset: 0x000058A1
	public bool alphaModeActive
	{
		get
		{
			return this._alphaModeActive;
		}
		set
		{
			this._alphaModeActive = value;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x000076AA File Offset: 0x000058AA
	// (set) Token: 0x0600070C RID: 1804 RVA: 0x000076B2 File Offset: 0x000058B2
	public int alphaModeWins
	{
		get
		{
			return this._alphaModeWins;
		}
		set
		{
			this._alphaModeWins = Mathf.Clamp(value, 0, 99);
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x000076C3 File Offset: 0x000058C3
	// (set) Token: 0x0600070E RID: 1806 RVA: 0x000356A0 File Offset: 0x000338A0
	public int money
	{
		get
		{
			return this._money;
		}
		set
		{
			int num = Mathf.Abs(value - this._money);
			bool flag = true;
			if (this._money == -1 || num == 0)
			{
				flag = false;
			}
			int money = this._money;
			this._money = Mathf.Clamp(value, 0, 99999);
			if (this._money != money)
			{
				if (flag)
				{
					GameManager.Stage.uiTop.labelMoney.SetText(this._money, 0.05f, true, 2.5f);
				}
				else
				{
					GameManager.Stage.uiTop.labelMoney.SetText(this._money, 0f, false, 1f);
				}
			}
			SteamUtils.CheckMaxCurrencyAchievement(true);
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600070F RID: 1807 RVA: 0x000076CB File Offset: 0x000058CB
	// (set) Token: 0x06000710 RID: 1808 RVA: 0x00035754 File Offset: 0x00033954
	public int hunie
	{
		get
		{
			return this._hunie;
		}
		set
		{
			int num = Mathf.Abs(value - this._hunie);
			bool flag = true;
			if (this._hunie == -1 || num == 0)
			{
				flag = false;
			}
			int hunie = this._hunie;
			this._hunie = Mathf.Clamp(value, 0, 99999);
			if (this._hunie != hunie)
			{
				if (flag)
				{
					GameManager.Stage.uiTop.labelHunie.SetText(this._hunie, 0.05f, true, 2.5f);
				}
				else
				{
					GameManager.Stage.uiTop.labelHunie.SetText(this._hunie, 0f, false, 1f);
				}
			}
			SteamUtils.CheckMaxCurrencyAchievement(true);
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000711 RID: 1809 RVA: 0x000076D3 File Offset: 0x000058D3
	// (set) Token: 0x06000712 RID: 1810 RVA: 0x000076DB File Offset: 0x000058DB
	public int successfulDateCount
	{
		get
		{
			return this._successfulDateCount;
		}
		set
		{
			this._successfulDateCount = Mathf.Max(value, 0);
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x000076EA File Offset: 0x000058EA
	// (set) Token: 0x06000714 RID: 1812 RVA: 0x000076F2 File Offset: 0x000058F2
	public GirlDefinition currentGirl
	{
		get
		{
			return this._currentGirl;
		}
		set
		{
			this._currentGirl = value;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000715 RID: 1813 RVA: 0x000076FB File Offset: 0x000058FB
	// (set) Token: 0x06000716 RID: 1814 RVA: 0x00007703 File Offset: 0x00005903
	public LocationDefinition currentLocation
	{
		get
		{
			return this._currentLocation;
		}
		set
		{
			this._currentLocation = value;
		}
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0000770C File Offset: 0x0000590C
	public int GetTraitLevel(PlayerTraitType traitType)
	{
		return this._traitLevels[traitType];
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00035808 File Offset: 0x00033A08
	public void UpgradeTraitLevel(PlayerTraitType traitType)
	{
		if (this._traitLevels[traitType] + 1 <= 6)
		{
			Dictionary<PlayerTraitType, int> traitLevels;
			Dictionary<PlayerTraitType, int> dictionary = traitLevels = this._traitLevels;
			int num = traitLevels[traitType];
			dictionary[traitType] = num + 1;
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00035844 File Offset: 0x00033A44
	public int GetTotalTraitUpgradeCount()
	{
		int num = 0;
		foreach (PlayerTraitType key in this._traitLevels.Keys)
		{
			num += this._traitLevels[key];
		}
		return num;
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600071A RID: 1818 RVA: 0x0000771A File Offset: 0x0000591A
	public InventoryItemPlayerData[] inventory
	{
		get
		{
			return this._inventory;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x00007722 File Offset: 0x00005922
	public InventoryItemPlayerData[] gifts
	{
		get
		{
			return this._gifts;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x0000772A File Offset: 0x0000592A
	public InventoryItemPlayerData[] dateGifts
	{
		get
		{
			return this._dateGifts;
		}
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x000358B0 File Offset: 0x00033AB0
	public bool AddItem(ItemDefinition item, InventoryItemPlayerData[] inventoryList = null, bool wrapAsPresent = false, bool reverseCheck = false)
	{
		if (inventoryList == null)
		{
			inventoryList = this._inventory;
		}
		int num = -1;
		if (!reverseCheck)
		{
			for (int i = 0; i < inventoryList.Length; i++)
			{
				if (inventoryList[i].itemDefinition == null)
				{
					num = i;
					break;
				}
			}
		}
		else
		{
			for (int j = inventoryList.Length - 1; j >= 0; j--)
			{
				if (inventoryList[j].itemDefinition == null)
				{
					num = j;
					break;
				}
			}
		}
		if (num >= 0)
		{
			inventoryList[num].itemDefinition = item;
			if (wrapAsPresent && inventoryList == this._inventory)
			{
				List<ItemDefinition> allOfType = GameManager.Data.Items.GetAllOfType(ItemType.PRESENT, false);
				inventoryList[num].presentDefinition = allOfType[UnityEngine.Random.Range(0, allOfType.Count)];
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00035988 File Offset: 0x00033B88
	public void RemoveItem(ItemDefinition item)
	{
		for (int i = 0; i < this._inventory.Length; i++)
		{
			if (this._inventory[i].itemDefinition == item)
			{
				this._inventory[i].itemDefinition = null;
			}
		}
		for (int j = 0; j < this._gifts.Length; j++)
		{
			if (this._gifts[j].itemDefinition == item)
			{
				this._gifts[j].itemDefinition = null;
			}
		}
		for (int k = 0; k < this._dateGifts.Length; k++)
		{
			if (this._dateGifts[k].itemDefinition == item)
			{
				this._dateGifts[k].itemDefinition = null;
			}
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00035A54 File Offset: 0x00033C54
	public bool HasItem(ItemDefinition item)
	{
		for (int i = 0; i < this._inventory.Length; i++)
		{
			if (this._inventory[i].itemDefinition == item)
			{
				return true;
			}
		}
		for (int j = 0; j < this._gifts.Length; j++)
		{
			if (this._gifts[j].itemDefinition == item)
			{
				return true;
			}
		}
		for (int k = 0; k < this._dateGifts.Length; k++)
		{
			if (this._dateGifts[k].itemDefinition == item)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00035AFC File Offset: 0x00033CFC
	public bool IsInventoryFull(InventoryItemPlayerData[] inventoryList = null)
	{
		if (inventoryList == null)
		{
			inventoryList = this._inventory;
		}
		for (int i = 0; i < inventoryList.Length; i++)
		{
			if (inventoryList[i].itemDefinition == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x00035B44 File Offset: 0x00033D44
	public List<ItemDefinition> GetItemsOfTypeFromAllInventories(ItemType itemType)
	{
		List<ItemDefinition> list = new List<ItemDefinition>();
		for (int i = 0; i < this._inventory.Length; i++)
		{
			if (this._inventory[i].itemDefinition != null && this._inventory[i].itemDefinition.type == itemType)
			{
				list.Add(this._inventory[i].itemDefinition);
			}
		}
		for (int j = 0; j < this._gifts.Length; j++)
		{
			if (this._gifts[j].itemDefinition != null && this._gifts[j].itemDefinition.type == itemType)
			{
				list.Add(this._gifts[j].itemDefinition);
			}
		}
		for (int k = 0; k < this._dateGifts.Length; k++)
		{
			if (this._dateGifts[k].itemDefinition != null && this._dateGifts[k].itemDefinition.type == itemType)
			{
				list.Add(this._dateGifts[k].itemDefinition);
			}
		}
		return list;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x00035C6C File Offset: 0x00033E6C
	public List<ItemDefinition> GetPotentialUniqueGiftRewards()
	{
		List<ItemDefinition> allOfType = GameManager.Data.Items.GetAllOfType(ItemType.UNIQUE_GIFT, false);
		List<ItemDefinition> itemsOfTypeFromAllInventories = this.GetItemsOfTypeFromAllInventories(ItemType.UNIQUE_GIFT);
		foreach (GirlDefinition girlDefinition in this._girls.Keys)
		{
			GirlPlayerData girlPlayerData = this._girls[girlDefinition];
			for (int i = 0; i < girlDefinition.uniqueGiftList.Count; i++)
			{
				if (girlPlayerData.metStatus == GirlMetStatus.MET)
				{
					if (girlPlayerData.IsItemInUniqueGifts(girlDefinition.uniqueGiftList[i]))
					{
						itemsOfTypeFromAllInventories.Add(girlDefinition.uniqueGiftList[i]);
					}
				}
				else
				{
					itemsOfTypeFromAllInventories.Add(girlDefinition.uniqueGiftList[i]);
				}
			}
		}
		for (int j = 0; j < allOfType.Count; j++)
		{
			if (itemsOfTypeFromAllInventories.Contains(allOfType[j]))
			{
				allOfType.RemoveAt(j);
				j--;
			}
		}
		return allOfType;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00035D9C File Offset: 0x00033F9C
	public int OwnedUniqueGiftCount()
	{
		int num = this.GetItemsOfTypeFromAllInventories(ItemType.UNIQUE_GIFT).Count;
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			num += girlPlayerData.UniqueGiftCount();
		}
		return num;
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x00007732 File Offset: 0x00005932
	public List<GirlPlayerData> girls
	{
		get
		{
			return ListUtils.DictionaryValuesToList<GirlDefinition, GirlPlayerData>(this._girls);
		}
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0000773F File Offset: 0x0000593F
	public GirlPlayerData GetGirlData(GirlDefinition girlDef)
	{
		return this._girls[girlDef];
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00035E0C File Offset: 0x0003400C
	public int GetTotalGirlsRelationshipLevel()
	{
		int num = 0;
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			num += Mathf.Max(girlPlayerData.relationshipLevel - 1, 0);
		}
		return num;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00035E78 File Offset: 0x00034078
	public int GetTotalMaxRelationships()
	{
		int num = 0;
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			if (girlPlayerData.gotPanties)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00035EE4 File Offset: 0x000340E4
	private void OnGirlStatsChanged(GirlPlayerData girlPlayerData)
	{
		GirlDefinition girlDefinition = null;
		if (this._girls != null)
		{
			foreach (GirlDefinition girlDefinition2 in this._girls.Keys)
			{
				if (this._girls[girlDefinition2] == girlPlayerData)
				{
					girlDefinition = girlDefinition2;
					break;
				}
			}
		}
		if (girlDefinition != null && !this._statsChangedGirls.Contains(girlDefinition))
		{
			this._statsChangedGirls.Add(girlDefinition);
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00035F8C File Offset: 0x0003418C
	private void Update()
	{
		if (this._paused)
		{
			return;
		}
		if (this._statsChangedGirls.Count > 0)
		{
			foreach (GirlDefinition girlDefinition in this._statsChangedGirls)
			{
				if (this.GirlStatsChangedEvent != null)
				{
					this.GirlStatsChangedEvent(girlDefinition);
				}
			}
			this._statsChangedGirls.Clear();
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x0000774D File Offset: 0x0000594D
	public List<MessagePlayerData> messages
	{
		get
		{
			return this._messages;
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00036020 File Offset: 0x00034220
	public bool AddMessage(MessageDefinition messageDefinition)
	{
		for (int i = 0; i < this._messages.Count; i++)
		{
			if (this._messages[i].messageDefinition == messageDefinition)
			{
				return false;
			}
		}
		MessagePlayerData messagePlayerData = new MessagePlayerData();
		messagePlayerData.messageDefinition = messageDefinition;
		messagePlayerData.timestamp = GameManager.System.Clock.TotalMinutesElapsed(0);
		messagePlayerData.viewed = false;
		this._messages.Insert(0, messagePlayerData);
		GameManager.Stage.uiTop.RefreshMessageAlert();
		GameManager.Stage.cellNotifications.Notify(CellNotificationType.MESSAGE, "You got a new message from " + messagePlayerData.messageDefinition.sender.firstName + "!");
		return true;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x000360E0 File Offset: 0x000342E0
	public bool HasReceivedMessage(MessageDefinition messageDefinition)
	{
		for (int i = 0; i < this._messages.Count; i++)
		{
			if (this._messages[i].messageDefinition == messageDefinition)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x00007755 File Offset: 0x00005955
	public StoreItemPlayerData[] storeGifts
	{
		get
		{
			return this._storeGifts;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x0000775D File Offset: 0x0000595D
	public StoreItemPlayerData[] storeUnique
	{
		get
		{
			return this._storeUnique;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x00007765 File Offset: 0x00005965
	public StoreItemPlayerData[] storeFood
	{
		get
		{
			return this._storeFood;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x0000776D File Offset: 0x0000596D
	public StoreItemPlayerData[] storeDrinks
	{
		get
		{
			return this._storeDrinks;
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00007775 File Offset: 0x00005975
	public void LogDateLocation(GameClockDaytime daytime, LocationDefinition location)
	{
		this._dateLocationLog[daytime] = location;
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00007784 File Offset: 0x00005984
	public LocationDefinition GetLoggedDateLocation(GameClockDaytime daytime)
	{
		if (!this._dateLocationLog.ContainsKey(daytime))
		{
			return null;
		}
		return this._dateLocationLog[daytime];
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x000077A5 File Offset: 0x000059A5
	public void LogTossedItem(ItemDefinition item)
	{
		if (!this._tossedItems.Contains(item))
		{
			this._tossedItems.Add(item);
		}
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x000077C4 File Offset: 0x000059C4
	public bool HasTossedItem(ItemDefinition item)
	{
		return this._tossedItems.Contains(item);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00036128 File Offset: 0x00034328
	public void RollNewDaytime()
	{
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			if (girlPlayerData.appetite > 6)
			{
				girlPlayerData.appetite = Mathf.Max(girlPlayerData.appetite - 2, 6);
			}
			else if (girlPlayerData.appetite < 6)
			{
				girlPlayerData.appetite = Mathf.Min(girlPlayerData.appetite + 2, 6);
			}
			girlPlayerData.inebriation -= 2;
		}
		this._tossedItems.Clear();
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x000361E0 File Offset: 0x000343E0
	public void RollNewDay()
	{
		this.RollNewStoreList(this._storeGifts, ItemType.GIFT);
		this.RollNewStoreList(this._storeUnique, ItemType.UNIQUE_GIFT);
		this.RollNewStoreList(this._storeFood, ItemType.FOOD);
		this.RollNewStoreList(this._storeDrinks, ItemType.DRINK);
		foreach (GirlPlayerData girlPlayerData in this._girls.Values)
		{
			girlPlayerData.appetite = 6;
			girlPlayerData.inebriation = 0;
			if (!(GameManager.System.Location.currentGirl != null) || !(GameManager.System.Location.currentLocation != null) || !(girlPlayerData.GetGirlDefinition() == GameManager.System.Location.currentGirl) || GameManager.System.Location.currentLocation.type != LocationType.DATE || !GameManager.System.Location.currentLocation.bonusRoundLocation)
			{
				girlPlayerData.dayDated = false;
			}
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00036308 File Offset: 0x00034508
	private void RollNewStoreList(StoreItemPlayerData[] storeList, ItemType itemType)
	{
		List<ItemDefinition> list;
		if (storeList != this._storeUnique)
		{
			list = GameManager.Data.Items.GetAllOfType(itemType, false);
			if (storeList == this._storeGifts)
			{
				int num = Mathf.Clamp(6 + Mathf.FloorToInt((float)(this.GetTotalGirlsRelationshipLevel() / 2)), 6, 18);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].itemFunctionValue > num)
					{
						list.RemoveAt(i);
						i--;
					}
				}
			}
		}
		else
		{
			list = new List<ItemDefinition>();
			List<GirlDefinition> all = GameManager.Data.Girls.GetAll();
			for (int j = 0; j < all.Count; j++)
			{
				if (!all[j].secretGirl || this._girls[all[j]].metStatus == GirlMetStatus.MET)
				{
					for (int k = 0; k < all[j].uniqueGiftList.Count; k++)
					{
						if (!this.HasItem(all[j].uniqueGiftList[k]) && !this._girls[all[j]].IsItemInUniqueGifts(all[j].uniqueGiftList[k]))
						{
							list.Add(all[j].uniqueGiftList[k]);
							break;
						}
					}
				}
			}
		}
		ListUtils.Shuffle<ItemDefinition>(list);
		if (list.Count > 12)
		{
			list.RemoveRange(12, list.Count - 12);
		}
		for (int l = 0; l < 12; l++)
		{
			if (list.Count > l)
			{
				storeList[l].itemDefinition = list[l];
				storeList[l].soldOut = false;
			}
			else
			{
				storeList[l].itemDefinition = null;
				storeList[l].soldOut = true;
			}
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000077D2 File Offset: 0x000059D2
	public void Pause()
	{
		if (this._paused)
		{
			return;
		}
		this._paused = true;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x000077E7 File Offset: 0x000059E7
	public void Unpause()
	{
		if (!this._paused)
		{
			return;
		}
		this._paused = false;
	}

	// Token: 0x0400081C RID: 2076
	public const int MAX_MONEY = 99999;

	// Token: 0x0400081D RID: 2077
	public const int MAX_HUNIE = 99999;

	// Token: 0x0400081E RID: 2078
	public const int INVENTORY_SLOTS = 30;

	// Token: 0x0400081F RID: 2079
	public const int EQUIPMENT_SLOTS = 6;

	// Token: 0x04000820 RID: 2080
	public const int MAX_TRAIT_LEVEL = 6;

	// Token: 0x04000821 RID: 2081
	public const int STORE_ITEM_SLOTS = 12;

	// Token: 0x04000822 RID: 2082
	private bool _tutorialComplete;

	// Token: 0x04000823 RID: 2083
	private int _tutorialStep = -1;

	// Token: 0x04000824 RID: 2084
	private bool _cellphoneUnlocked;

	// Token: 0x04000825 RID: 2085
	private SettingsDifficulty _settingsDifficulty = SettingsDifficulty.MEDIUM;

	// Token: 0x04000826 RID: 2086
	private SettingsGender _settingsGender;

	// Token: 0x04000827 RID: 2087
	private int _money = -1;

	// Token: 0x04000828 RID: 2088
	private int _hunie = -1;

	// Token: 0x04000829 RID: 2089
	private int _successfulDateCount = -1;

	// Token: 0x0400082A RID: 2090
	private GirlDefinition _currentGirl;

	// Token: 0x0400082B RID: 2091
	private LocationDefinition _currentLocation;

	// Token: 0x0400082C RID: 2092
	private Dictionary<PlayerTraitType, int> _traitLevels = new Dictionary<PlayerTraitType, int>();

	// Token: 0x0400082D RID: 2093
	private InventoryItemPlayerData[] _inventory;

	// Token: 0x0400082E RID: 2094
	private InventoryItemPlayerData[] _gifts;

	// Token: 0x0400082F RID: 2095
	private InventoryItemPlayerData[] _dateGifts;

	// Token: 0x04000830 RID: 2096
	private Dictionary<GirlDefinition, GirlPlayerData> _girls = new Dictionary<GirlDefinition, GirlPlayerData>();

	// Token: 0x04000831 RID: 2097
	private List<MessagePlayerData> _messages = new List<MessagePlayerData>();

	// Token: 0x04000832 RID: 2098
	private StoreItemPlayerData[] _storeGifts;

	// Token: 0x04000833 RID: 2099
	private StoreItemPlayerData[] _storeUnique;

	// Token: 0x04000834 RID: 2100
	private StoreItemPlayerData[] _storeFood;

	// Token: 0x04000835 RID: 2101
	private StoreItemPlayerData[] _storeDrinks;

	// Token: 0x04000836 RID: 2102
	private int _failureDateCount = 1;

	// Token: 0x04000837 RID: 2103
	private int _drinksGivenOut = 1;

	// Token: 0x04000838 RID: 2104
	private int _chatSessionCount = 1;

	// Token: 0x04000839 RID: 2105
	private bool _endingSceneShown;

	// Token: 0x0400083A RID: 2106
	private List<int> _pantiesTurnedIn = new List<int>();

	// Token: 0x0400083B RID: 2107
	private bool _alphaModeActive;

	// Token: 0x0400083C RID: 2108
	private int _alphaModeWins;

	// Token: 0x0400083D RID: 2109
	private List<GirlDefinition> _statsChangedGirls = new List<GirlDefinition>();

	// Token: 0x0400083E RID: 2110
	private Dictionary<GameClockDaytime, LocationDefinition> _dateLocationLog = new Dictionary<GameClockDaytime, LocationDefinition>();

	// Token: 0x0400083F RID: 2111
	private List<ItemDefinition> _tossedItems = new List<ItemDefinition>();

	// Token: 0x04000840 RID: 2112
	private bool _paused;

	// Token: 0x0200012E RID: 302
	// (Invoke) Token: 0x0600073B RID: 1851
	public delegate void PlayerManagerGirlPlayerDataDelegate(GirlDefinition girlDefinition);
}
