using System;
using System.Collections.Generic;

// Token: 0x020000CB RID: 203
public class GirlDefinition : Definition
{
	// Token: 0x060004C8 RID: 1224 RVA: 0x0002560C File Offset: 0x0002380C
	public List<GirlPiece> GetPiecesByType(GirlPieceType pieceType)
	{
		List<GirlPiece> list = new List<GirlPiece>();
		for (int i = 0; i < this.pieces.Count; i++)
		{
			if (this.pieces[i].type == pieceType)
			{
				list.Add(this.pieces[i]);
			}
		}
		return list;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00025668 File Offset: 0x00023868
	public LocationDefinition IsAtLocationAtTime(GameClockWeekday weekday, GameClockDaytime daytime)
	{
		GirlScheduleDaytime girlScheduleDaytime = this.schedule[(int)weekday].daytimes[(int)daytime];
		return girlScheduleDaytime.location;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0002568C File Offset: 0x0002388C
	public List<LocationDefinition> GetDateLocationsByDaytime(GameClockDaytime daytime)
	{
		List<LocationDefinition> list = new List<LocationDefinition>();
		for (int i = 0; i < this.dateLocations.Count; i++)
		{
			if (this.dateLocations[i].daytime == daytime && this.dateLocations[i].location.type == LocationType.DATE)
			{
				list.Add(this.dateLocations[i].location);
			}
		}
		return list;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00025708 File Offset: 0x00023908
	public bool WillAcceptItem(ItemDefinition itemDefinition)
	{
		return (itemDefinition.type == ItemType.GIFT && (itemDefinition.giftType == this.lovesGiftType || itemDefinition.giftType == this.likesGiftType1 || itemDefinition.giftType == this.likesGiftType2)) || (itemDefinition.type == ItemType.UNIQUE_GIFT && itemDefinition.specialGiftType == this.uniqueGiftType) || (itemDefinition.type == ItemType.FOOD && (itemDefinition.foodType == this.lovesFoodType || itemDefinition.foodType == this.likesFoodType));
	}

	// Token: 0x04000521 RID: 1313
	public EditorGirlSection editorSection;

	// Token: 0x04000522 RID: 1314
	public string firstName;

	// Token: 0x04000523 RID: 1315
	public float baseVoiceVolume;

	// Token: 0x04000524 RID: 1316
	public float baseSexVolume;

	// Token: 0x04000525 RID: 1317
	public string[] details;

	// Token: 0x04000526 RID: 1318
	public TraitDefinition mostDesiredTrait;

	// Token: 0x04000527 RID: 1319
	public TraitDefinition leastDesiredTrait;

	// Token: 0x04000528 RID: 1320
	public ItemFoodType lovesFoodType;

	// Token: 0x04000529 RID: 1321
	public ItemFoodType likesFoodType;

	// Token: 0x0400052A RID: 1322
	public ItemGiftType lovesGiftType;

	// Token: 0x0400052B RID: 1323
	public ItemGiftType likesGiftType1;

	// Token: 0x0400052C RID: 1324
	public ItemGiftType likesGiftType2;

	// Token: 0x0400052D RID: 1325
	public ItemUniqueGiftType uniqueGiftType;

	// Token: 0x0400052E RID: 1326
	public ItemDefinition favDrink;

	// Token: 0x0400052F RID: 1327
	public bool drinksAnytime;

	// Token: 0x04000530 RID: 1328
	public GirlAlcoholToleranceType alcoholTolerance;

	// Token: 0x04000531 RID: 1329
	public GirlLibidoType libido;

	// Token: 0x04000532 RID: 1330
	public GirlFavoriteColor favoriteColor;

	// Token: 0x04000533 RID: 1331
	public ItemDefinition pantiesItem;

	// Token: 0x04000534 RID: 1332
	public bool secretGirl;

	// Token: 0x04000535 RID: 1333
	public List<ItemDefinition> uniqueGiftList = new List<ItemDefinition>();

	// Token: 0x04000536 RID: 1334
	public List<ItemDefinition> collection = new List<ItemDefinition>();

	// Token: 0x04000537 RID: 1335
	public List<GirlStyle> outfits = new List<GirlStyle>();

	// Token: 0x04000538 RID: 1336
	public List<GirlStyle> hairstyles = new List<GirlStyle>();

	// Token: 0x04000539 RID: 1337
	public List<GirlPhoto> photos = new List<GirlPhoto>();

	// Token: 0x0400053A RID: 1338
	public string spriteCollectionName;

	// Token: 0x0400053B RID: 1339
	public string photosSpriteCollectionName;

	// Token: 0x0400053C RID: 1340
	public GirlPieceArt headPiece;

	// Token: 0x0400053D RID: 1341
	public GirlPieceArt bodyPiece;

	// Token: 0x0400053E RID: 1342
	public GirlPieceArt braPiece;

	// Token: 0x0400053F RID: 1343
	public GirlPieceArt pantiesPiece;

	// Token: 0x04000540 RID: 1344
	public GirlPieceArt blinkHalfPiece;

	// Token: 0x04000541 RID: 1345
	public GirlPieceArt blinkFullPiece;

	// Token: 0x04000542 RID: 1346
	public int defaultExpression;

	// Token: 0x04000543 RID: 1347
	public int defaultHairstyle;

	// Token: 0x04000544 RID: 1348
	public int defaultOutfit;

	// Token: 0x04000545 RID: 1349
	public int defaultFootwear;

	// Token: 0x04000546 RID: 1350
	public int bonusRoundExpression;

	// Token: 0x04000547 RID: 1351
	public int bonusRoundHairstyle;

	// Token: 0x04000548 RID: 1352
	public List<GirlPiece> pieces = new List<GirlPiece>();

	// Token: 0x04000549 RID: 1353
	public LocationDefinition introLocation;

	// Token: 0x0400054A RID: 1354
	public GameClockDaytime introDaytime;

	// Token: 0x0400054B RID: 1355
	public bool leavesTown;

	// Token: 0x0400054C RID: 1356
	public GirlScheduleDay[] schedule;

	// Token: 0x0400054D RID: 1357
	public List<GirlDateLocation> dateLocations = new List<GirlDateLocation>();

	// Token: 0x0400054E RID: 1358
	public DialogSceneDefinition introScene;

	// Token: 0x0400054F RID: 1359
	public List<DialogSceneDefinition> talkQueries = new List<DialogSceneDefinition>();

	// Token: 0x04000550 RID: 1360
	public List<DialogSceneDefinition> talkQuizzes = new List<DialogSceneDefinition>();

	// Token: 0x04000551 RID: 1361
	public List<DialogSceneDefinition> talkQuestions = new List<DialogSceneDefinition>();
}
