using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class SaveUtils
{
	// Token: 0x06000755 RID: 1877 RVA: 0x00036D8C File Offset: 0x00034F8C
	public static void Init()
	{
		SaveUtils._inited = true;
		SaveUtils._loadFileName = null;
		for (int i = 1; i > 0; i--)
		{
			if (File.Exists(Application.persistentDataPath + "/HuniePopSaveData" + i.ToString() + ".game"))
			{
				SaveUtils._loadFileName = "HuniePopSaveData" + i.ToString() + ".game";
				break;
			}
		}
		if (SaveUtils._loadFileName == null && File.Exists(Application.persistentDataPath + "/HuniePopSaveData.game"))
		{
			SaveUtils._loadFileName = "HuniePopSaveData.game";
		}
		SaveUtils._saveFileName = "HuniePopSaveData" + 1.ToString() + ".game";
		if (SaveUtils._loadFileName != null)
		{
			SaveUtils.Load();
		}
		else
		{
			SaveUtils._saveData = new SaveData(4);
			SaveUtils.Save();
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x000078E8 File Offset: 0x00005AE8
	public static bool IsInited()
	{
		return SaveUtils._inited;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00036E6C File Offset: 0x0003506C
	public static void Load()
	{
		if (!SaveUtils._inited)
		{
			return;
		}
		Stream stream = File.Open(Application.persistentDataPath + "/" + SaveUtils._loadFileName, FileMode.Open);
		SaveUtils._saveData = (SaveData)new BinaryFormatter
		{
			Binder = new VersionDeserializationBinder()
		}.Deserialize(stream);
		stream.Close();
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00036EC8 File Offset: 0x000350C8
	public static void Save()
	{
		if (!SaveUtils._inited)
		{
			return;
		}
		Stream stream = File.Open(Application.persistentDataPath + "/" + SaveUtils._saveFileName, FileMode.Create);
		new BinaryFormatter
		{
			Binder = new VersionDeserializationBinder()
		}.Serialize(stream, SaveUtils._saveData);
		stream.Close();
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000078EF File Offset: 0x00005AEF
	public static SaveData GetSaveData()
	{
		if (!SaveUtils._inited)
		{
			return null;
		}
		return SaveUtils._saveData;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00007902 File Offset: 0x00005B02
	public static SaveFile GetSaveFile(int saveFileIndex)
	{
		if (!SaveUtils._inited)
		{
			return null;
		}
		return SaveUtils._saveData.GetSaveFile(saveFileIndex);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00036F20 File Offset: 0x00035120
	public static SaveFile CreateSaveFileFromDebugProfile(DebugProfile debugProfile)
	{
		SaveFile saveFile = new SaveFile();
		saveFile.started = true;
		saveFile.tutorialComplete = true;
		saveFile.tutorialStep = 3;
		saveFile.cellphoneUnlocked = true;
		saveFile.settingsDifficulty = (int)debugProfile.settingsDifficulty;
		saveFile.settingsGender = (int)debugProfile.settingsGender;
		saveFile.totalMinutesElapsed = Mathf.Max(debugProfile.days * 24 * 60 + debugProfile.hours * 60 + debugProfile.minutes, 0);
		saveFile.money = debugProfile.money;
		saveFile.hunie = debugProfile.hunie;
		saveFile.successfulDateCount = debugProfile.successfulDateCount;
		if (debugProfile.currentGirl != null)
		{
			saveFile.currentGirl = debugProfile.currentGirl.id;
		}
		if (debugProfile.currentLocation != null)
		{
			saveFile.currentLocation = debugProfile.currentLocation.id;
		}
		for (int i = 0; i < debugProfile.traits.Count; i++)
		{
			saveFile.traitLevels[(int)debugProfile.traits[i].traitType] = Mathf.Clamp(debugProfile.traits[i].traitLevel, 0, 6);
		}
		for (int j = 0; j < debugProfile.inventoryItems.Count; j++)
		{
			saveFile.inventory[j].itemId = debugProfile.inventoryItems[j].id;
		}
		if (!StringUtils.IsEmpty(debugProfile.wrappedItems))
		{
			string[] array = debugProfile.wrappedItems.Split(new char[]
			{
				','
			});
			List<ItemDefinition> allOfType = GameManager.Data.Items.GetAllOfType(ItemType.PRESENT, false);
			for (int k = 0; k < array.Length; k++)
			{
				int num = StringUtils.ParseIntValue(array[k]) - 1;
				if (saveFile.inventory[num].itemId > 0)
				{
					saveFile.inventory[num].presentId = allOfType[UnityEngine.Random.Range(0, allOfType.Count)].id;
				}
			}
		}
		for (int l = 0; l < debugProfile.giftItems.Count; l++)
		{
			saveFile.gifts[l].itemId = debugProfile.giftItems[l].id;
		}
		for (int m = 0; m < debugProfile.dateGiftItems.Count; m++)
		{
			saveFile.dateGifts[m].itemId = debugProfile.dateGiftItems[m].id;
		}
		for (int n = 0; n < debugProfile.girls.Count; n++)
		{
			DebugProfileGirl debugProfileGirl = debugProfile.girls[n];
			if (!(debugProfileGirl.girl == null))
			{
				GirlSaveData girlSaveData = saveFile.girls[debugProfileGirl.girl.id];
				girlSaveData.metStatus = (int)debugProfileGirl.metStatus;
				girlSaveData.relationshipLevel = debugProfileGirl.relationshipLevel;
				if (debugProfileGirl.relationshipLevel == 5)
				{
					girlSaveData.gotPanties = true;
				}
				girlSaveData.appetite = debugProfileGirl.appetite;
				girlSaveData.inebriation = debugProfileGirl.inebriation;
				for (int num2 = 0; num2 < debugProfileGirl.relationshipLevel - 1; num2++)
				{
					girlSaveData.photosEarned.Add(num2);
				}
				if (!StringUtils.IsEmpty(debugProfileGirl.uniqueGifts))
				{
					string[] array2 = debugProfileGirl.uniqueGifts.Split(new char[]
					{
						','
					});
					for (int num3 = 0; num3 < array2.Length; num3++)
					{
						girlSaveData.uniqueGifts.Add(StringUtils.ParseIntValue(array2[num3]));
					}
				}
				if (!StringUtils.IsEmpty(debugProfileGirl.collection))
				{
					string[] array3 = debugProfileGirl.collection.Split(new char[]
					{
						','
					});
					for (int num4 = 0; num4 < array3.Length; num4++)
					{
						girlSaveData.collection.Add(StringUtils.ParseIntValue(array3[num4]));
					}
				}
				if (!StringUtils.IsEmpty(debugProfileGirl.hairstyles))
				{
					girlSaveData.hairstyles.Clear();
					string[] array4 = debugProfileGirl.hairstyles.Split(new char[]
					{
						','
					});
					for (int num5 = 0; num5 < array4.Length; num5++)
					{
						girlSaveData.hairstyles.Add(StringUtils.ParseIntValue(array4[num5]));
					}
				}
				if (!StringUtils.IsEmpty(debugProfileGirl.outfits))
				{
					girlSaveData.outfits.Clear();
					string[] array5 = debugProfileGirl.outfits.Split(new char[]
					{
						','
					});
					for (int num6 = 0; num6 < array5.Length; num6++)
					{
						girlSaveData.outfits.Add(StringUtils.ParseIntValue(array5[num6]));
					}
				}
				girlSaveData.hairstyle = debugProfileGirl.hairstyle;
				girlSaveData.outfit = debugProfileGirl.outfit;
				for (int num7 = 0; num7 < debugProfileGirl.details.Length; num7++)
				{
					girlSaveData.details[num7] = debugProfileGirl.details[num7];
				}
			}
		}
		for (int num8 = 0; num8 < debugProfile.messages.Count; num8++)
		{
			MessageSaveData messageSaveData = new MessageSaveData();
			messageSaveData.messageId = debugProfile.messages[num8].id;
			messageSaveData.timestamp = 480;
			saveFile.messages.Add(messageSaveData);
		}
		saveFile.failureDateCount = debugProfile.failureDateCount;
		saveFile.drinksGivenOut = debugProfile.drinksGivenOut;
		saveFile.chatSessionCount = debugProfile.chatSessionCount;
		saveFile.endingSceneShown = debugProfile.endingSceneShown;
		return saveFile;
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x000374C8 File Offset: 0x000356C8
	public static CheatData LoadCheatData()
	{
		if (!SaveUtils._inited)
		{
			return null;
		}
		if (!File.Exists("huniepop_kickstarter_bonus_6DAC2798E0.game"))
		{
			return null;
		}
		Stream stream = File.Open("huniepop_kickstarter_bonus_6DAC2798E0.game", FileMode.Open);
		CheatData result = (CheatData)new BinaryFormatter
		{
			Binder = new VersionDeserializationBinder()
		}.Deserialize(stream);
		stream.Close();
		return result;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0000791B File Offset: 0x00005B1B
	public static bool UncensorPatchExists()
	{
		return SaveUtils._inited && File.Exists("huniepop_uncensored_patch.game");
	}

	// Token: 0x0400088B RID: 2187
	public const int SAVE_FILE_COUNT = 4;

	// Token: 0x0400088C RID: 2188
	private const string SAVE_FILE_NAME = "HuniePopSaveData";

	// Token: 0x0400088D RID: 2189
	private const int SAVE_FILE_VERSION = 1;

	// Token: 0x0400088E RID: 2190
	private const string SAVE_FILE_EXT = ".game";

	// Token: 0x0400088F RID: 2191
	private const string CHEAT_FILE_NAME = "huniepop_kickstarter_bonus_6DAC2798E0.game";

	// Token: 0x04000890 RID: 2192
	private const string UNCENSOR_FILE_NAME = "huniepop_uncensored_patch.game";

	// Token: 0x04000891 RID: 2193
	private static bool _inited;

	// Token: 0x04000892 RID: 2194
	private static string _loadFileName;

	// Token: 0x04000893 RID: 2195
	private static string _saveFileName;

	// Token: 0x04000894 RID: 2196
	private static SaveData _saveData;
}
