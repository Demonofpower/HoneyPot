using System;
using Steamworks;

// Token: 0x02000142 RID: 322
public class SteamUtils
{
	// Token: 0x0600078F RID: 1935 RVA: 0x00007A5E File Offset: 0x00005C5E
	public static void UnlockAchievement(string achievement, bool store = true)
	{
		if (!GameManager.System.Hook.steamBuild || !SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetAchievement(achievement);
		if (store)
		{
			global::SteamUtils.StoreAchievements();
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00007A91 File Offset: 0x00005C91
	public static void StoreAchievements()
	{
		if (!GameManager.System.Hook.steamBuild || !SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00037F8C File Offset: 0x0003618C
	public static void CheckAchievements()
	{
		if (GameManager.System.Player.GetTotalGirlsRelationshipLevel() > 0)
		{
			global::SteamUtils.UnlockAchievement("there_may_be_hope", false);
		}
		if (GameManager.System.Player.GetTotalMaxRelationships() > 0)
		{
			global::SteamUtils.UnlockAchievement("vcard_revoked", false);
		}
		if (GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.goddessGirlDef).gotPanties)
		{
			global::SteamUtils.UnlockAchievement("truest_player", false);
		}
		if (GameManager.System.Player.alphaModeActive)
		{
			global::SteamUtils.UnlockAchievement("alpha", false);
		}
		if (GameManager.System.Player.alphaModeActive && GameManager.System.Player.alphaModeWins >= 5)
		{
			global::SteamUtils.UnlockAchievement("alpha_champ", false);
		}
		if (GameManager.System.Player.alphaModeActive && GameManager.System.Player.alphaModeWins >= 10)
		{
			global::SteamUtils.UnlockAchievement("alpha_master", false);
		}
		global::SteamUtils.CheckPercentCompleteAchievement(false);
		global::SteamUtils.CheckMaxCurrencyAchievement(false);
		if (GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.fairyGirlDef).metStatus == GirlMetStatus.MET)
		{
			global::SteamUtils.UnlockAchievement("date_the_fairy", false);
		}
		if (GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.catGirlDef).metStatus == GirlMetStatus.MET)
		{
			global::SteamUtils.UnlockAchievement("beastiality", false);
		}
		if (GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.alienGirlDef).metStatus == GirlMetStatus.MET)
		{
			global::SteamUtils.UnlockAchievement("ayy_lmao", false);
		}
		if (GameManager.System.Player.GetGirlData(GameManager.Stage.uiGirl.goddessGirlDef).metStatus == GirlMetStatus.MET)
		{
			global::SteamUtils.UnlockAchievement("heavenly_body", false);
		}
		global::SteamUtils.StoreAchievements();
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00038178 File Offset: 0x00036378
	public static void CheckPercentCompleteAchievement(bool store = true)
	{
		int num = 0;
		if (GameManager.System.Player.girls != null && GameManager.System.Player.girls.Count > 0)
		{
			for (int i = 0; i < GameManager.System.Player.girls.Count; i++)
			{
				GirlPlayerData girlPlayerData = GameManager.System.Player.girls[i];
				if (girlPlayerData.relationshipLevel == 5 && girlPlayerData.EarnedPhotoCount() == 4 && girlPlayerData.DetailKnownCount() == 12 && girlPlayerData.ItemCollectionCount() == 24 && girlPlayerData.UnlockedOutfitsCount() == 5 && girlPlayerData.UnlockedHairstylesCount() == 5)
				{
					num++;
				}
			}
		}
		if (num == 12)
		{
			global::SteamUtils.UnlockAchievement("zero_life", store);
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00007AB8 File Offset: 0x00005CB8
	public static void CheckMaxCurrencyAchievement(bool store = true)
	{
		if (GameManager.System.Player.money == 99999 && GameManager.System.Player.hunie == 99999)
		{
			global::SteamUtils.UnlockAchievement("money_bags", store);
		}
	}

	// Token: 0x0400089F RID: 2207
	public const string ACHIEVEMENT_THERE_MAY_BE_HOPE = "there_may_be_hope";

	// Token: 0x040008A0 RID: 2208
	public const string ACHIEVEMENT_VCARD_REVOKED = "vcard_revoked";

	// Token: 0x040008A1 RID: 2209
	public const string ACHIEVEMENT_TRUEST_PLAYER = "truest_player";

	// Token: 0x040008A2 RID: 2210
	public const string ACHIEVEMENT_ALPHA = "alpha";

	// Token: 0x040008A3 RID: 2211
	public const string ACHIEVEMENT_ALPHA_CHAMP = "alpha_champ";

	// Token: 0x040008A4 RID: 2212
	public const string ACHIEVEMENT_ALPHA_MASTER = "alpha_master";

	// Token: 0x040008A5 RID: 2213
	public const string ACHIEVEMENT_PICKUP_ARTIST = "pickup_artist";

	// Token: 0x040008A6 RID: 2214
	public const string ACHIEVEMENT_PICKUP_LEGEND = "pickup_legend";

	// Token: 0x040008A7 RID: 2215
	public const string ACHIEVEMENT_SMOOTH_AS_FUCK = "smooth_as_fuck";

	// Token: 0x040008A8 RID: 2216
	public const string ACHIEVEMENT_SOBRIETY = "sobriety";

	// Token: 0x040008A9 RID: 2217
	public const string ACHIEVEMENT_BUYING_LOVE = "buying_love";

	// Token: 0x040008AA RID: 2218
	public const string ACHIEVEMENT_LUCKY_LOSER = "lucky_loser";

	// Token: 0x040008AB RID: 2219
	public const string ACHIEVEMENT_QUICKIE = "quickie";

	// Token: 0x040008AC RID: 2220
	public const string ACHIEVEMENT_MAKE_THE_MOVE = "make_the_move";

	// Token: 0x040008AD RID: 2221
	public const string ACHIEVEMENT_ZERO_LIFE = "zero_life";

	// Token: 0x040008AE RID: 2222
	public const string ACHIEVEMENT_MONEY_BAGS = "money_bags";

	// Token: 0x040008AF RID: 2223
	public const string ACHIEVEMENT_DATE_THE_FAIRY = "date_the_fairy";

	// Token: 0x040008B0 RID: 2224
	public const string ACHIEVEMENT_BEASTIALITY = "beastiality";

	// Token: 0x040008B1 RID: 2225
	public const string ACHIEVEMENT_AYY_LMAO = "ayy_lmao";

	// Token: 0x040008B2 RID: 2226
	public const string ACHIEVEMENT_HEAVENLY_BODY = "heavenly_body";
}
