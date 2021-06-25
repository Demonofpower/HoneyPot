using System;
using UnityEngine;

namespace HoneyPot.Menus
{
    class PlayerMenu
    {
        private readonly DebugLog debugLog;

        private string newHunie;
        private string newMoney;

        public PlayerMenu(DebugLog debugLog)
        {
            this.debugLog = debugLog;
            newMoney = "0";
            newHunie = "0";
        }
        
        public void DoPlayer(int windowID)
        {
            GUILayout.BeginHorizontal("money");
            newMoney = GUILayout.TextField(newMoney, 10);
            if (GUILayout.Button("ChangeMoney"))
            {
                GameManager.System.Player.money = int.Parse(newMoney);
                debugLog.AddMessage("Money changed to: " + newMoney);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("hunie");
            newHunie = GUILayout.TextField(newHunie, 10);
            if (GUILayout.Button("ChangeHunie"))
            {
                GameManager.System.Player.hunie = int.Parse(newHunie);
                debugLog.AddMessage("Hunie changed to: " + newHunie);
            }

            GUILayout.EndHorizontal();
            if (GUILayout.Button("UnlockAll"))
            {
                UnlockAll();
                debugLog.AddMessage("All unlocked");
            }

            if (GUILayout.Button("UnlockAllAchivements"))
            {
                UnlockAllAchivements();
                debugLog.AddMessage("All achivements unlocked");
            }
        }

        private void UnlockAllAchivements()
        {
            SteamUtils.UnlockAchievement("alpha");
            SteamUtils.UnlockAchievement("alpha_champ");
            SteamUtils.UnlockAchievement("alpha_master");
            SteamUtils.UnlockAchievement("ayy_lmao");
            SteamUtils.UnlockAchievement("beastiality");
            SteamUtils.UnlockAchievement("buying_love");
            SteamUtils.UnlockAchievement("date_the_fairy");
            SteamUtils.UnlockAchievement("heavenly_body");
            SteamUtils.UnlockAchievement("lucky_loser");
            SteamUtils.UnlockAchievement("make_the_move");
            SteamUtils.UnlockAchievement("money_bags");
            SteamUtils.UnlockAchievement("pickup_artist");
            SteamUtils.UnlockAchievement("pickup_legend");
            SteamUtils.UnlockAchievement("quickie");
            SteamUtils.UnlockAchievement("smooth_as_fuck");
            SteamUtils.UnlockAchievement("sobriety");
            SteamUtils.UnlockAchievement("there_may_be_hope");
            SteamUtils.UnlockAchievement("truest_player");
            SteamUtils.UnlockAchievement("vcard_revoked");
            SteamUtils.UnlockAchievement("zero_life");
        }

        private void UnlockAll()
        {
            GameManager.System.Player.hunie = 99999;
            GameManager.System.Player.money = 99999;
            foreach (var girlDef in GameManager.Data.Girls.GetAll())
            {
                var girlData = GameManager.System.Player.GetGirlData(girlDef);
                girlData.metStatus = GirlMetStatus.MET;
                girlData.relationshipLevel = 5;
                girlData.appetite = 12;
                girlData.inebriation = 12;
                girlData.dayDated = false;
                girlData.AddPhotoEarned(0);
                girlData.AddPhotoEarned(1);
                girlData.AddPhotoEarned(2);
                girlData.AddPhotoEarned(3);
                foreach (var item in girlData.GetGirlDefinition().uniqueGiftList) girlData.AddItemToUniqueGifts(item);

                foreach (var item2 in girlData.GetGirlDefinition().collection) girlData.AddItemToCollection(item2);

                for (var i = 0; i < girlData.GetGirlDefinition().hairstyles.Count; i++) girlData.UnlockHairstyle(i);

                for (var j = 0; j < girlData.GetGirlDefinition().outfits.Count; j++) girlData.UnlockOutfit(j);

                foreach (var obj in Enum.GetValues(typeof(GirlDetailType)))
                {
                    var type = (GirlDetailType)obj;
                    girlData.KnowDetail(type);
                }
            }

            SaveUtils.Save();
        }
    }
}
