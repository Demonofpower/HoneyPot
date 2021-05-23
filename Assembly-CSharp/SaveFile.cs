using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000136 RID: 310
[Serializable]
public class SaveFile
{
	// Token: 0x0600074F RID: 1871 RVA: 0x000078DA File Offset: 0x00005ADA
	public SaveFile()
	{
		this.ResetFile();
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x000368FC File Offset: 0x00034AFC
	public void ResetFile()
	{
		this.started = false;
		this.tutorialComplete = false;
		this.tutorialStep = -1;
		this.cellphoneUnlocked = false;
		this.settingsDifficulty = 1;
		this.settingsGender = 0;
		this.totalMinutesElapsed = 0;
		this.currentGirl = 9;
		this.currentLocation = 5;
		this.money = 0;
		this.hunie = 0;
		this.successfulDateCount = 0;
		this.traitLevels = new Dictionary<int, int>();
		for (int i = 0; i < Enum.GetNames(typeof(PlayerTraitType)).Length; i++)
		{
			this.traitLevels.Add(i, 0);
		}
		this.inventory = new InventoryItemSaveData[30];
		for (int j = 0; j < this.inventory.Length; j++)
		{
			this.inventory[j] = new InventoryItemSaveData();
		}
		this.inventory[0].itemId = 289;
		this.inventory[1].itemId = 290;
		this.gifts = new InventoryItemSaveData[6];
		for (int k = 0; k < this.gifts.Length; k++)
		{
			this.gifts[k] = new InventoryItemSaveData();
		}
		this.dateGifts = new InventoryItemSaveData[6];
		for (int l = 0; l < this.dateGifts.Length; l++)
		{
			this.dateGifts[l] = new InventoryItemSaveData();
		}
		this.girls = new Dictionary<int, GirlSaveData>();
		List<GirlDefinition> all = GameManager.Data.Girls.GetAll();
		for (int m = 0; m < all.Count; m++)
		{
			this.girls.Add(all[m].id, new GirlSaveData(all[m]));
		}
		this.messages = new List<MessageSaveData>();
		this.storeGifts = new StoreItemSaveData[12];
		for (int n = 0; n < this.storeGifts.Length; n++)
		{
			this.storeGifts[n] = new StoreItemSaveData();
		}
		this.storeUnique = new StoreItemSaveData[12];
		for (int num = 0; num < this.storeUnique.Length; num++)
		{
			this.storeUnique[num] = new StoreItemSaveData();
		}
		this.storeFood = new StoreItemSaveData[12];
		for (int num2 = 0; num2 < this.storeFood.Length; num2++)
		{
			this.storeFood[num2] = new StoreItemSaveData();
		}
		this.storeDrinks = new StoreItemSaveData[12];
		for (int num3 = 0; num3 < this.storeDrinks.Length; num3++)
		{
			this.storeDrinks[num3] = new StoreItemSaveData();
		}
		this.failureDateCount = 1;
		this.drinksGivenOut = 1;
		this.chatSessionCount = 1;
		this.endingSceneShown = false;
		this.pantiesTurnedIn = new List<int>();
		this.alphaModeActive = false;
		this.alphaModeWins = 0;
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00036BD4 File Offset: 0x00034DD4
	public void VerifyFile()
	{
		if (this.pantiesTurnedIn == null)
		{
			this.failureDateCount = 1;
			this.drinksGivenOut = 1;
			this.chatSessionCount = 1;
			this.endingSceneShown = false;
			this.pantiesTurnedIn = new List<int>();
			this.alphaModeActive = false;
			this.alphaModeWins = 0;
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00036C24 File Offset: 0x00034E24
	public int GetPercentComplete()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		foreach (GirlSaveData girlSaveData in this.girls.Values)
		{
			num += Mathf.Clamp(girlSaveData.relationshipLevel - 1, 0, 4);
			num2 += Mathf.Clamp(girlSaveData.photosEarned.Count, 0, 4);
			for (int i = 0; i < girlSaveData.details.Length; i++)
			{
				if (girlSaveData.details[i])
				{
					num3++;
				}
			}
			num4 += Mathf.Clamp(girlSaveData.collection.Count, 0, 24);
			num5 += Mathf.Clamp(girlSaveData.outfits.Count - 1, 0, 4) + Mathf.Clamp(girlSaveData.hairstyles.Count - 1, 0, 4);
		}
		int num6 = 0;
		num6 += Mathf.FloorToInt((float)num / 2f);
		num6 += Mathf.FloorToInt((float)num2 / 2f);
		num6 += Mathf.FloorToInt((float)num3 / 6f);
		num6 += Mathf.FloorToInt((float)num4 / 12f);
		return num6 + Mathf.FloorToInt((float)num5 / 24f);
	}

	// Token: 0x0400086E RID: 2158
	public bool started;

	// Token: 0x0400086F RID: 2159
	public bool tutorialComplete;

	// Token: 0x04000870 RID: 2160
	public int tutorialStep;

	// Token: 0x04000871 RID: 2161
	public bool cellphoneUnlocked;

	// Token: 0x04000872 RID: 2162
	public int settingsDifficulty;

	// Token: 0x04000873 RID: 2163
	public int settingsGender;

	// Token: 0x04000874 RID: 2164
	public int totalMinutesElapsed;

	// Token: 0x04000875 RID: 2165
	public int currentGirl;

	// Token: 0x04000876 RID: 2166
	public int currentLocation;

	// Token: 0x04000877 RID: 2167
	public int money;

	// Token: 0x04000878 RID: 2168
	public int hunie;

	// Token: 0x04000879 RID: 2169
	public int successfulDateCount;

	// Token: 0x0400087A RID: 2170
	public Dictionary<int, int> traitLevels;

	// Token: 0x0400087B RID: 2171
	public InventoryItemSaveData[] inventory;

	// Token: 0x0400087C RID: 2172
	public InventoryItemSaveData[] gifts;

	// Token: 0x0400087D RID: 2173
	public InventoryItemSaveData[] dateGifts;

	// Token: 0x0400087E RID: 2174
	public Dictionary<int, GirlSaveData> girls;

	// Token: 0x0400087F RID: 2175
	public List<MessageSaveData> messages;

	// Token: 0x04000880 RID: 2176
	public StoreItemSaveData[] storeGifts;

	// Token: 0x04000881 RID: 2177
	public StoreItemSaveData[] storeUnique;

	// Token: 0x04000882 RID: 2178
	public StoreItemSaveData[] storeFood;

	// Token: 0x04000883 RID: 2179
	public StoreItemSaveData[] storeDrinks;

	// Token: 0x04000884 RID: 2180
	public int failureDateCount;

	// Token: 0x04000885 RID: 2181
	public int drinksGivenOut;

	// Token: 0x04000886 RID: 2182
	public int chatSessionCount;

	// Token: 0x04000887 RID: 2183
	public bool endingSceneShown;

	// Token: 0x04000888 RID: 2184
	public List<int> pantiesTurnedIn;

	// Token: 0x04000889 RID: 2185
	public bool alphaModeActive;

	// Token: 0x0400088A RID: 2186
	public int alphaModeWins;
}
