using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class PuzzleGame
{
	// Token: 0x0600051A RID: 1306 RVA: 0x0002721C File Offset: 0x0002541C
	public PuzzleGame(LocationDefinition location, GirlDefinition girl)
	{
		this._puzzleGrid = GameManager.Stage.uiPuzzle.puzzleGrid;
		this._puzzleStatus = GameManager.Stage.uiPuzzle.puzzleStatus;
		this._isTutorial = !GameManager.System.Player.tutorialComplete;
		if (this._isTutorial)
		{
			this._tutorialGrid = new List<List<PuzzleTokenDefinition>>();
			for (int i = 0; i < GameManager.System.Puzzle.TUTORIAL_GRID.Count; i++)
			{
				this._tutorialGrid.Add(new List<PuzzleTokenDefinition>());
				for (int j = 0; j < GameManager.System.Puzzle.TUTORIAL_GRID[i].Count; j++)
				{
					this._tutorialGrid[i].Add(GameManager.Data.PuzzleTokens.Get(GameManager.System.Puzzle.TUTORIAL_GRID[i][j]));
				}
			}
			this._tutorialLocked = true;
		}
		else
		{
			this._tutorialGrid = null;
			this._tutorialLocked = false;
		}
		this._tutorialStarted = false;
		this._tutorialFinished = false;
		this._tutorialMoveMade = false;
		this._tutorialTargetTokenDef = null;
		this._tutorialTargetPosition = null;
		this._tutorialTargetCount = -1;
		this._isBonusRound = location.bonusRoundLocation;
		this._bonusRoundDrainTimestamp = 0f;
		this._bonusRoundDrainDelay = 0.05f;
		if (GameManager.System.Player.settingsDifficulty == SettingsDifficulty.EASY)
		{
			this._bonusRoundDrainDelay = 0.065f;
		}
		else if (GameManager.System.Player.settingsDifficulty == SettingsDifficulty.HARD)
		{
			this._bonusRoundDrainDelay = 0.035f;
		}
		this._bonusBraOn = false;
		if (this._isBonusRound)
		{
			this._bonusBraOn = true;
		}
		this._currentPassionLevel = 0;
		this._puzzleStatus.SetPassionLevel(this._currentPassionLevel);
		if (this._isTutorial)
		{
			this._goalAffection = 100;
		}
		else if (this._isBonusRound)
		{
			this._goalAffection = 500 + 25 * GameManager.System.Player.GetTotalMaxRelationships();
		}
		else
		{
			this._goalAffection = GameManager.System.Puzzle.GetAffectionGoal(GameManager.System.Location.currentGirl);
		}
		this._maxPassion = GameManager.System.Puzzle.GetPassionLevelCost(GameManager.System.Puzzle.GetMaxPassionLevel());
		this._maxMoves = 30;
		this._maxSentiment = 999;
		this.SetResourceValue(PuzzleGameResourceType.AFFECTION, 0, false);
		this.SetResourceValue(PuzzleGameResourceType.PASSION, 0, false);
		if (!this._isBonusRound)
		{
			int num = Mathf.FloorToInt((float)(Math.Max(GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).appetite - 6, 0) / 2));
			if (GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).appetite == 12)
			{
				num = 4;
			}
			this.SetResourceValue(PuzzleGameResourceType.MOVES, 20 + num, false);
		}
		else
		{
			this.SetResourceValue(PuzzleGameResourceType.MOVES, 0, false);
		}
		this.SetResourceValue(PuzzleGameResourceType.SENTIMENT, Mathf.FloorToInt((float)(GameManager.System.Player.GetGirlData(GameManager.System.Location.currentGirl).inebriation / 2)), false);
		if (!this._isBonusRound)
		{
			this._puzzleStatus.resourcesContainer.SetChildAlpha(1f, 0f);
			this._puzzleStatus.passionSubtitle.SetAlpha(0.6f);
			this._puzzleStatus.movesSubtitle.SetAlpha(0.6f);
			this._puzzleStatus.sentimentSubtitle.SetAlpha(0.6f);
		}
		else
		{
			this._puzzleStatus.resourcesContainer.SetChildAlpha(0.5f, 0f);
			this._puzzleStatus.passionSubtitle.SetAlpha(0.32f);
			this._puzzleStatus.movesSubtitle.SetAlpha(0.32f);
			this._puzzleStatus.sentimentSubtitle.SetAlpha(0.32f);
		}
		this._puzzleStatus.passionMax.SetAlpha(0f, 0f);
		this._puzzleStatus.movesMax.SetAlpha(0f, 0f);
		this._puzzleStatus.sentimentMax.SetAlpha(0f, 0f);
		for (int k = 0; k < this._puzzleStatus.itemSlots.Count; k++)
		{
			this._puzzleStatus.itemSlots[k].button.Enable();
			this._puzzleStatus.itemSlots[k].PopulateSlotItem();
			this._puzzleStatus.itemSlots[k].PuzzleStatusItemSlotDownEvent += this.OnPuzzleStatusItemSlotDown;
		}
		this.RefreshDateGiftSlots(false);
		this._tokenDefs = GameManager.Data.PuzzleTokens.GetAll(this._isBonusRound);
		this._tokenWeights = new Dictionary<PuzzleTokenDefinition, int>();
		for (int l = 0; l < this._tokenDefs.Length; l++)
		{
			if (!this._isBonusRound)
			{
				this._tokenWeights[this._tokenDefs[l]] = this._tokenDefs[l].weight;
			}
			else
			{
				this._tokenWeights[this._tokenDefs[l]] = 24;
			}
		}
		this._tokenQueue = new List<PuzzleToken>();
		this._unreadyTokens = new List<PuzzleToken>();
		this._gridPositions = new Dictionary<string, PuzzleGridPosition>();
		for (int m = 6; m >= 0; m--)
		{
			for (int n = 7; n >= 0; n--)
			{
				PuzzleGridPosition puzzleGridPosition = new PuzzleGridPosition(m, n, this._puzzleGrid);
				this._gridPositions[puzzleGridPosition.GetKey(0, 0)] = puzzleGridPosition;
				puzzleGridPosition.GridPositionEmptyEvent += this.OnGridPositionEmpty;
			}
		}
		this._moveTokenMatchSet = new PuzzleMatchSet();
		this._moveTokenCursor = DisplayUtils.CreateSpriteObject(this._puzzleGrid.puzzleTokenSpriteCollection, "puzzle_token_blank", "MoveTokenCursor");
		this._moveTokenCursor.gameObj.SetActive(false);
		this._puzzleItemCursor = DisplayUtils.CreateSpriteObject(this._puzzleStatus.itemsSpriteCollection, "item_blank", "PuzzleItemCursor");
		this._puzzleItemCursor.gameObj.SetActive(false);
		this._puzzleStatus.giftZone.gameObj.SetActive(true);
		this._activeTokenEnergyTrails = new List<EnergyTrail>();
		this._puzzleEffects = new List<AbilityBehaviorDefinition>();
		this._moveLockTokenDef = null;
		this.puzzleGameState = PuzzleGameState.INITED;
		this._victory = false;
		this._initialTokenDropped = false;
		this._matchComboCount = 0;
		this._movePointsEarned = 0;
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600051B RID: 1307 RVA: 0x00027878 File Offset: 0x00025A78
	// (remove) Token: 0x0600051C RID: 1308 RVA: 0x000278B0 File Offset: 0x00025AB0
	public event PuzzleGame.PuzzleGameDelegate PuzzleGameReadyEvent;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x0600051D RID: 1309 RVA: 0x000278E8 File Offset: 0x00025AE8
	// (remove) Token: 0x0600051E RID: 1310 RVA: 0x00027920 File Offset: 0x00025B20
	public event PuzzleGame.PuzzleGameDelegate PuzzleGameCompleteEvent;

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600051F RID: 1311 RVA: 0x00005EDD File Offset: 0x000040DD
	// (set) Token: 0x06000520 RID: 1312 RVA: 0x00005EE5 File Offset: 0x000040E5
	public PuzzleGameState puzzleGameState
	{
		get
		{
			return this._state;
		}
		set
		{
			this._state = value;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000521 RID: 1313 RVA: 0x00005EEE File Offset: 0x000040EE
	public bool isTutorial
	{
		get
		{
			return this._isTutorial;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000522 RID: 1314 RVA: 0x00005EF6 File Offset: 0x000040F6
	public bool isBonusRound
	{
		get
		{
			return this._isBonusRound;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000523 RID: 1315 RVA: 0x00005EFE File Offset: 0x000040FE
	public PuzzleToken currentMoveToken
	{
		get
		{
			if (this._moveFromGridPosition != null)
			{
				return this._moveFromGridPosition.GetToken(false);
			}
			return null;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x00005F16 File Offset: 0x00004116
	public int activeEnergyTrailCount
	{
		get
		{
			return this._activeTokenEnergyTrails.Count;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000525 RID: 1317 RVA: 0x00005F23 File Offset: 0x00004123
	// (set) Token: 0x06000526 RID: 1318 RVA: 0x00005F2B File Offset: 0x0000412B
	public int currentPassionLevel
	{
		get
		{
			return this._currentPassionLevel;
		}
		set
		{
			this._currentPassionLevel = Mathf.Clamp(value, 0, GameManager.System.Puzzle.GetMaxPassionLevel());
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000527 RID: 1319 RVA: 0x00005F49 File Offset: 0x00004149
	public bool isVictorious
	{
		get
		{
			return this._isBonusRound || this._victory;
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00027958 File Offset: 0x00025B58
	public void Begin()
	{
		this.puzzleGameState = PuzzleGameState.WAITING;
		for (int i = 6; i >= 0; i--)
		{
			List<int> list = new List<int>(new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7
			});
			while (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				this.CreateToken(list[index], true);
				list.RemoveAt(index);
			}
		}
		GameManager.Stage.MouseDownEvent += this.OnStageDown;
		GameManager.Stage.MouseUpEvent += this.OnStageUp;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x000279E8 File Offset: 0x00025BE8
	public void OnUpdate()
	{
		switch (this.puzzleGameState)
		{
		case PuzzleGameState.INITED:
			return;
		case PuzzleGameState.WAITING:
			if (this._movePointsEarned > 0)
			{
				if (this._movePointsEarned >= 2000)
				{
					SteamUtils.UnlockAchievement("make_the_move", true);
				}
				this._movePointsEarned = 0;
			}
			if ((this._currentAffection == this._goalAffection || (this._movesRemaining == 0 && !this._isBonusRound)) && this._unreadyTokens.Count == 0)
			{
				if (this._currentAffection == this._goalAffection)
				{
					this._victory = true;
				}
				this.puzzleGameState = PuzzleGameState.FINISHED;
			}
			break;
		case PuzzleGameState.MOVING:
		{
			Vector3 vector = this._puzzleGrid.tokenContainer.gameObj.transform.InverseTransformPoint(GameManager.System.Cursor.GetMousePosition());
			bool flag = false;
			if (vector.y <= this._moveFromGridPosition.GetLocalY() && vector.y >= this._moveFromGridPosition.GetLocalY() - 82f && vector.x >= 0f && vector.x <= 656f)
			{
				flag = true;
			}
			bool flag2 = false;
			if (vector.x >= this._moveFromGridPosition.GetLocalX() && vector.x <= this._moveFromGridPosition.GetLocalX() + 82f && vector.y <= 0f && vector.y >= -574f)
			{
				flag2 = true;
			}
			PuzzleDirection puzzleDirection;
			if (flag && !flag2)
			{
				if (vector.x < this._moveFromGridPosition.GetLocalX() + 41f)
				{
					puzzleDirection = PuzzleDirection.LEFT;
				}
				else
				{
					puzzleDirection = PuzzleDirection.RIGHT;
				}
			}
			else if (!flag && flag2)
			{
				if (vector.y > this._moveFromGridPosition.GetLocalY() - 41f)
				{
					puzzleDirection = PuzzleDirection.UP;
				}
				else
				{
					puzzleDirection = PuzzleDirection.DOWN;
				}
			}
			else
			{
				puzzleDirection = PuzzleDirection.NONE;
			}
			this._moveToGridPosition = this._moveFromGridPosition;
			int num = 0;
			int num2 = 0;
			foreach (object obj in Enum.GetValues(typeof(PuzzleDirection)))
			{
				PuzzleDirection puzzleDirection2 = (PuzzleDirection)((int)obj);
				if (puzzleDirection2 != PuzzleDirection.NONE && !this._moveFromGridPosition.IsEdgePosition(puzzleDirection2, 0))
				{
					switch (puzzleDirection2)
					{
					case PuzzleDirection.RIGHT:
						num = 0;
						num2 = 1;
						break;
					case PuzzleDirection.UP:
						num = -1;
						num2 = 0;
						break;
					case PuzzleDirection.LEFT:
						num = 0;
						num2 = -1;
						break;
					case PuzzleDirection.DOWN:
						num = 1;
						num2 = 0;
						break;
					}
					PuzzleGridPosition puzzleGridPosition = this._gridPositions[this._moveFromGridPosition.GetKey(num, num2)];
					bool flag3;
					do
					{
						flag3 = puzzleGridPosition.IsEdgePosition(puzzleDirection2, 0);
						if (puzzleDirection2 == puzzleDirection && puzzleGridPosition.IsPointPastInDirection(vector, puzzleDirection2, true))
						{
							this._gridPositions[puzzleGridPosition.GetKey(-num, -num2)].SetToken(puzzleGridPosition.GetToken(false), true, PuzzleTokenSetType.SLIDE, 0f);
							this._moveToGridPosition = puzzleGridPosition;
						}
						else
						{
							puzzleGridPosition.SetToken(puzzleGridPosition.GetToken(false), true, PuzzleTokenSetType.SLIDE, 0f);
						}
						if (!flag3)
						{
							puzzleGridPosition = this._gridPositions[puzzleGridPosition.GetKey(num, num2)];
						}
					}
					while (!flag3);
				}
			}
			this._moveToGridPosition.SetToken(this._moveFromGridPosition.GetToken(false), true, PuzzleTokenSetType.INSTANT, 0f);
			break;
		}
		case PuzzleGameState.STOPPING:
			if (this._unreadyTokens.Count == 0)
			{
				this.EndTokenMove();
			}
			break;
		case PuzzleGameState.CANCELLING:
			if (this._unreadyTokens.Count == 0 && (this._moveTokenTweener == null || this._moveTokenTweener.isComplete))
			{
				this.TokenMoveComplete(true);
			}
			break;
		case PuzzleGameState.CANCELLING_ITEM:
			if (this._puzzleItemTweener == null || this._puzzleItemTweener.isComplete)
			{
				this.ItemGiveComplete(true);
			}
			break;
		case PuzzleGameState.FINISHED:
			if (this._activeTokenEnergyTrails.Count == 0)
			{
				this.puzzleGameState = PuzzleGameState.COMPLETE;
				if (this.PuzzleGameCompleteEvent != null)
				{
					this.PuzzleGameCompleteEvent();
				}
			}
			break;
		}
		if (this.affectionMeterTweener != null && this.affectionMeterTweener.IsTweening(this) && !this.affectionMeterTweener.isComplete && !this.affectionMeterTweener.isPaused)
		{
			this.UpdateAffectionMeterDisplay();
		}
		if (this._isBonusRound && GameManager.System.Lifetime(true) - this._bonusRoundDrainTimestamp >= this._bonusRoundDrainDelay && this._currentAffection < this._goalAffection && this.puzzleGameState != PuzzleGameState.FINISHED && this.puzzleGameState != PuzzleGameState.COMPLETE && !Paranoia.noDrain)
		{
			this._bonusRoundDrainTimestamp = GameManager.System.Lifetime(true);
			this.AddResourceValue(PuzzleGameResourceType.AFFECTION, -1, true);
		}
		if (this._isBonusRound && this._bonusBraOn && this._currentAffection > this._goalAffection / 2)
		{
			this._bonusBraOn = false;
			GameManager.Stage.girl.HideBra();
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00027EB4 File Offset: 0x000260B4
	public void Pause()
	{
		if (this.puzzleGameState == PuzzleGameState.MOVING)
		{
			this._moveTokenMatchSet.ClearMatches();
			this.EndTokenMove();
		}
		this._puzzleGrid.tokenContainer.interactive = false;
		if (this._moveTokenTweener != null && !this._moveTokenTweener.isComplete && !this._moveTokenTweener.isPaused)
		{
			this._moveTokenTweener.Pause();
		}
		if (this._puzzleItemTweener != null && !this._puzzleItemTweener.isComplete && !this._puzzleItemTweener.isPaused)
		{
			this._puzzleItemTweener.Pause();
		}
		if (this.affectionMeterTweener != null && !this.affectionMeterTweener.isComplete && !this.affectionMeterTweener.isPaused)
		{
			this.affectionMeterTweener.Pause();
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00027F74 File Offset: 0x00026174
	public void Unpaused()
	{
		if (this.puzzleGameState == PuzzleGameState.WAITING && this._unreadyTokens.Count == 0 && (this._movesRemaining > 0 || this._isBonusRound) && this._currentAffection < this._goalAffection)
		{
			this._puzzleGrid.tokenContainer.interactive = true;
		}
		if (this._moveTokenTweener != null && !this._moveTokenTweener.isComplete && this._moveTokenTweener.isPaused)
		{
			this._moveTokenTweener.Play();
		}
		if (this._puzzleItemTweener != null && !this._puzzleItemTweener.isComplete && this._puzzleItemTweener.isPaused)
		{
			this._puzzleItemTweener.Play();
		}
		if (this.affectionMeterTweener != null && !this.affectionMeterTweener.isComplete && this.affectionMeterTweener.isPaused)
		{
			this.affectionMeterTweener.Play();
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00028050 File Offset: 0x00026250
	private void CreateToken(int col, bool preventMatches = false)
	{
		PuzzleGridPosition lowestEmptyGridPosition = this.GetLowestEmptyGridPosition(col);
		PuzzleTokenDefinition puzzleTokenDefinition = null;
		List<PuzzleTokenDefinition> list = new List<PuzzleTokenDefinition>();
		if (this._isTutorial && !this._tutorialFinished && this._tutorialGrid[col].Count > 0)
		{
			puzzleTokenDefinition = this._tutorialGrid[col][0];
			this._tutorialGrid[col].RemoveAt(0);
		}
		while (puzzleTokenDefinition == null)
		{
			int num = 0;
			for (int i = 0; i < this._tokenDefs.Length; i++)
			{
				if (!list.Contains(this._tokenDefs[i]))
				{
					num += this._tokenWeights[this._tokenDefs[i]];
				}
			}
			int num2 = UnityEngine.Random.Range(1, num + 1);
			int num3 = 0;
			for (int j = 0; j < this._tokenDefs.Length; j++)
			{
				if (!list.Contains(this._tokenDefs[j]))
				{
					PuzzleTokenDefinition puzzleTokenDefinition2 = this._tokenDefs[j];
					num3 += this._tokenWeights[puzzleTokenDefinition2];
					if (num2 <= num3)
					{
						if (!preventMatches && puzzleTokenDefinition2.type != PuzzleTokenType.BROKEN)
						{
							puzzleTokenDefinition = puzzleTokenDefinition2;
							break;
						}
						if (this.GetMatch(lowestEmptyGridPosition, false, puzzleTokenDefinition2) != null)
						{
							list.Add(puzzleTokenDefinition2);
							break;
						}
						puzzleTokenDefinition = puzzleTokenDefinition2;
						break;
					}
				}
			}
		}
		PuzzleToken puzzleToken = this.GenerateNewToken(puzzleTokenDefinition, 1, true);
		puzzleToken.SetLocalPosition(82f * (float)col, 102f);
		this._tokenQueue.Add(puzzleToken);
		lowestEmptyGridPosition.SetToken(puzzleToken, false, PuzzleTokenSetType.FALL, (float)this._tokenQueue.Count * 0.025f);
		puzzleToken.belowToken = this.GetHighestTokenBelow(lowestEmptyGridPosition);
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x000281EC File Offset: 0x000263EC
	private PuzzleToken GenerateNewToken(PuzzleTokenDefinition tokenDefinition, int tokenLevel = 1, bool weighted = true)
	{
		PuzzleToken component = new GameObject("PuzzleToken", new Type[]
		{
			typeof(PuzzleToken)
		}).GetComponent<PuzzleToken>();
		component.TokenUnreadyEvent += this.OnTokenUnready;
		component.TokenReadyEvent += this.OnTokenReady;
		component.TokenUnqueueEvent += this.OnTokenUnqueue;
		component.MouseOverEvent += this.OnTokenOver;
		component.MouseOutEvent += this.OnTokenOut;
		component.MouseDownEvent += this.OnTokenDown;
		component.Init(tokenDefinition, tokenLevel, weighted);
		this._puzzleGrid.tokenContainer.AddChild(component);
		if (component.isWeighted)
		{
			this._tokenWeights[tokenDefinition] = Mathf.Max(this._tokenWeights[tokenDefinition] - 1, 0);
		}
		return component;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000282CC File Offset: 0x000264CC
	private void DestroyToken(PuzzleToken token)
	{
		token.TokenUnreadyEvent -= this.OnTokenUnready;
		token.TokenReadyEvent -= this.OnTokenReady;
		token.TokenUnqueueEvent -= this.OnTokenUnqueue;
		token.MouseOverEvent -= this.OnTokenOver;
		token.MouseOutEvent -= this.OnTokenOut;
		token.MouseDownEvent -= this.OnTokenDown;
		if (this._tokenQueue.IndexOf(token) != -1)
		{
			this._tokenQueue.Remove(token);
		}
		if (this._unreadyTokens.IndexOf(token) != -1)
		{
			this._unreadyTokens.Remove(token);
		}
		this._puzzleGrid.tokenContainer.RemoveChild(token, false);
		if (token.isWeighted)
		{
			this._tokenWeights[token.definition] = Mathf.Max(this._tokenWeights[token.definition] + 1, 0);
		}
		UnityEngine.Object.Destroy(token.gameObj);
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00005F5B File Offset: 0x0000415B
	private void OnTokenUnready(PuzzleToken token)
	{
		if (this._unreadyTokens.IndexOf(token) == -1)
		{
			this._unreadyTokens.Add(token);
			this._puzzleGrid.tokenContainer.interactive = false;
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x000283CC File Offset: 0x000265CC
	private void OnTokenReady(PuzzleToken token)
	{
		if (this._unreadyTokens.IndexOf(token) != -1)
		{
			this._unreadyTokens.Remove(token);
		}
		if (this.puzzleGameState == PuzzleGameState.WAITING || this.puzzleGameState == PuzzleGameState.MOVING || this.puzzleGameState == PuzzleGameState.STOPPING || this.puzzleGameState == PuzzleGameState.ABILITY)
		{
			if (this.puzzleGameState == PuzzleGameState.WAITING)
			{
				PuzzleMatch match = this.GetMatch(token.gridPosition, false, null);
				if (match != null)
				{
					this.ProcessMatch(match);
					this._matchComboCount++;
				}
			}
			else if (this.puzzleGameState == PuzzleGameState.MOVING || this.puzzleGameState == PuzzleGameState.STOPPING)
			{
				PuzzleMatch puzzleMatch = this.GetMatch(token.gridPosition, true, null);
				if (puzzleMatch != null)
				{
					this._moveTokenMatchSet.AddMatch(puzzleMatch, true);
				}
				else
				{
					puzzleMatch = this._moveTokenMatchSet.GetMatchWithGridPosition(token.gridPosition);
					this._moveTokenMatchSet.RemoveMatch(puzzleMatch);
				}
			}
			if (!this._initialTokenDropped)
			{
				this._initialTokenDropped = true;
			}
			if (this._unreadyTokens.Count == 0)
			{
				if (this.puzzleGameState == PuzzleGameState.WAITING && (this._movesRemaining > 0 || this._isBonusRound) && this._currentAffection < this._goalAffection && !this.IsThereAnAvailableMove(false))
				{
					List<PuzzleTokenType> list = new List<PuzzleTokenType>
					{
						PuzzleTokenType.BROKEN,
						PuzzleTokenType.PASSION,
						PuzzleTokenType.SENTIMENT,
						PuzzleTokenType.JOY,
						PuzzleTokenType.AFFECTION
					};
					int num = 0;
					PuzzleGroup puzzleGroup = null;
					while (puzzleGroup == null)
					{
						puzzleGroup = this.GetPositionsContainingAsGroup(list[num]);
						num++;
					}
					this.DestroyPuzzleGroup(puzzleGroup, true);
					return;
				}
				if (this.PuzzleGameReadyEvent != null && this._tutorialMoveMade)
				{
					this._tutorialMoveMade = false;
					this.PuzzleGameReadyEvent();
				}
				this.RefreshDateGiftSlots(false);
				if (this.puzzleGameState == PuzzleGameState.WAITING || this.puzzleGameState == PuzzleGameState.ABILITY)
				{
					this._puzzleGrid.tokenContainer.interactive = true;
				}
				this._matchComboCount = 0;
				if (this._isTutorial && !this._tutorialStarted)
				{
					this._tutorialStarted = true;
					GameManager.System.Dialog.PlayDialogScene(GameManager.Stage.uiGirl.openingScenes[GameManager.System.Player.tutorialStep]);
				}
			}
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00005F89 File Offset: 0x00004189
	private void OnTokenUnqueue(PuzzleToken token)
	{
		if (this._tokenQueue.IndexOf(token) != -1)
		{
			this._tokenQueue.Remove(token);
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x000285DC File Offset: 0x000267DC
	private void OnTokenOver(DisplayObject displayObject)
	{
		PuzzleToken puzzleToken = displayObject as PuzzleToken;
		if ((this.puzzleGameState == PuzzleGameState.WAITING && this._unreadyTokens.Count == 0 && (this._movesRemaining > 0 || this._isBonusRound) && this._currentAffection < this._goalAffection && !this._tutorialLocked) || (this.puzzleGameState == PuzzleGameState.TARGETING && this._activeAbilityTargetGroup.gridPositions.IndexOf(puzzleToken.gridPosition) != -1))
		{
			puzzleToken.HighlightToken();
		}
		if (this.puzzleGameState == PuzzleGameState.TARGETING && this._activeAbilityTargetGroup.gridPositions.IndexOf(puzzleToken.gridPosition) != -1)
		{
			PuzzleGroup expandedPuzzleGroupFromTarget = this._activePuzzleAbility.GetExpandedPuzzleGroupFromTarget(puzzleToken.gridPosition, this._activePuzzleAbility.definition.targetConditionSet);
			for (int i = 0; i < expandedPuzzleGroupFromTarget.gridPositions.Count; i++)
			{
				expandedPuzzleGroupFromTarget.gridPositions[i].ShowTargetGuide();
			}
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000286C0 File Offset: 0x000268C0
	private void OnTokenOut(DisplayObject displayObject)
	{
		PuzzleToken puzzleToken = displayObject as PuzzleToken;
		puzzleToken.UnhighlightToken();
		if (this.puzzleGameState == PuzzleGameState.TARGETING && this._activeAbilityTargetGroup.gridPositions.IndexOf(puzzleToken.gridPosition) != -1)
		{
			PuzzleGroup expandedPuzzleGroupFromTarget = this._activePuzzleAbility.GetExpandedPuzzleGroupFromTarget(puzzleToken.gridPosition, this._activePuzzleAbility.definition.targetConditionSet);
			for (int i = 0; i < expandedPuzzleGroupFromTarget.gridPositions.Count; i++)
			{
				expandedPuzzleGroupFromTarget.gridPositions[i].HideTargetGuide();
			}
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00028748 File Offset: 0x00026948
	private void OnTokenDown(DisplayObject displayObject)
	{
		if (this.puzzleGameState != PuzzleGameState.WAITING || this._unreadyTokens.Count != 0 || (this._movesRemaining <= 0 && !this._isBonusRound) || this._currentAffection >= this._goalAffection || this._tutorialLocked)
		{
			return;
		}
		this.StartTokenMove(displayObject as PuzzleToken);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000287A0 File Offset: 0x000269A0
	private void OnStageUp(DisplayObject displayObject)
	{
		if (this.puzzleGameState == PuzzleGameState.MOVING)
		{
			if (this._unreadyTokens.Count > 0)
			{
				this.puzzleGameState = PuzzleGameState.STOPPING;
				return;
			}
			this.EndTokenMove();
			return;
		}
		else
		{
			if (this.puzzleGameState == PuzzleGameState.ITEM)
			{
				this.EndItemGive();
				return;
			}
			if (this.puzzleGameState == PuzzleGameState.TARGETING)
			{
				PuzzleToken puzzleToken = GameManager.System.Cursor.GetMouseTarget() as PuzzleToken;
				if (puzzleToken != null && this._activeAbilityTargetGroup.gridPositions.IndexOf(puzzleToken.gridPosition) != -1)
				{
					this.EndAbilityTargeting(puzzleToken);
				}
			}
			return;
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0000306D File Offset: 0x0000126D
	private void OnStageDown(DisplayObject displayObject)
	{
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0002882C File Offset: 0x00026A2C
	private bool IsThereAnAvailableMove(bool debug = false)
	{
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (string key in this._gridPositions.Keys)
		{
			dictionary.Add(key, this._gridPositions[key].GetToken(false).definition.id);
		}
		foreach (string key2 in this._gridPositions.Keys)
		{
			PuzzleGridPosition puzzleGridPosition = this._gridPositions[key2];
			if (puzzleGridPosition.GetToken(false).definition.type != PuzzleTokenType.BROKEN)
			{
				foreach (PuzzleDirection direction in GameManager.System.Puzzle.CHECK_DIRECTIONS)
				{
					int rowOffset = 0;
					int colOffset = 0;
					switch (direction)
					{
					case PuzzleDirection.RIGHT:
						colOffset = 1;
						break;
					case PuzzleDirection.UP:
						rowOffset = -1;
						break;
					case PuzzleDirection.LEFT:
						colOffset = -1;
						break;
					case PuzzleDirection.DOWN:
						rowOffset = 1;
						break;
					}
					PuzzleGridPosition puzzleGridPosition2 = puzzleGridPosition;
					if (!puzzleGridPosition2.IsEdgePosition(direction, 0))
					{
						Dictionary<string, int> dictionary2 = ListUtils.CopyDictionary<string, int>(dictionary);
						while (!puzzleGridPosition2.IsEdgePosition(direction, 0))
						{
							int value = dictionary2[puzzleGridPosition2.GetKey(0, 0)];
							dictionary2[puzzleGridPosition2.GetKey(0, 0)] = dictionary2[puzzleGridPosition2.GetKey(rowOffset, colOffset)];
							dictionary2[puzzleGridPosition2.GetKey(rowOffset, colOffset)] = value;
							puzzleGridPosition2 = this._gridPositions[puzzleGridPosition2.GetKey(rowOffset, colOffset)];
							if ((!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.LEFT, 1) && dictionary2[puzzleGridPosition2.GetKey(0, -1)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(0, -2)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]) || (!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.RIGHT, 1) && dictionary2[puzzleGridPosition2.GetKey(0, 1)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(0, 2)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]) || (!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.UP, 1) && dictionary2[puzzleGridPosition2.GetKey(-1, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(-2, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]) || (!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.DOWN, 1) && dictionary2[puzzleGridPosition2.GetKey(1, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(2, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]) || (!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.LEFT, 0) && !puzzleGridPosition2.IsEdgePosition(PuzzleDirection.RIGHT, 0) && dictionary2[puzzleGridPosition2.GetKey(0, -1)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(0, 1)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]) || (!puzzleGridPosition2.IsEdgePosition(PuzzleDirection.UP, 0) && !puzzleGridPosition2.IsEdgePosition(PuzzleDirection.DOWN, 0) && dictionary2[puzzleGridPosition2.GetKey(-1, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)] && dictionary2[puzzleGridPosition2.GetKey(1, 0)] == dictionary2[puzzleGridPosition2.GetKey(0, 0)]))
							{
								if (!debug)
								{
									return true;
								}
								Debug.Log("Move: " + puzzleGridPosition.GetKey(0, 0) + " to " + puzzleGridPosition2.GetKey(0, 0));
								num++;
							}
						}
					}
				}
			}
		}
		if (debug)
		{
			Debug.Log("Potential Move Count: " + num.ToString());
			if (num > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00028CA4 File Offset: 0x00026EA4
	private void StartTokenMove(PuzzleToken token)
	{
		this.puzzleGameState = PuzzleGameState.MOVING;
		GameManager.Stage.cellPhone.Lock(false);
		this._moveFromGridPosition = token.gridPosition;
		this._moveToGridPosition = this._moveFromGridPosition;
		this._moveTokenCursor.gameObj.SetActive(true);
		this._moveTokenCursor.sprite.SetSprite(token.sprite.spriteId);
		GameManager.Stage.effects.lowerCursorContainer.AddChild(this._moveTokenCursor);
		GameManager.System.Cursor.AttachObject(this._moveTokenCursor, -41f, 41f);
		token.SetAlpha(0f, 0f);
		this._puzzleGrid.ShowSlideGuides(this._moveFromGridPosition.row, this._moveFromGridPosition.col);
		this.RefreshDateGiftSlots(false);
		this._puzzleGrid.tokenContainer.interactive = false;
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.tokenPickupSound, false, 1f, true);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00028DBC File Offset: 0x00026FBC
	private void EndTokenMove()
	{
		this._puzzleGrid.HideSlideGuides();
		if (this._moveTokenMatchSet.MatchCount() > 0 && (!this._isTutorial || this._tutorialFinished || (this._moveTokenMatchSet.HasMatchWithGridPosition(this._tutorialTargetPosition) && this._moveTokenMatchSet.ContainsToken(this._tutorialTargetTokenDef, true) && this._moveTokenMatchSet.MatchCount() <= 1 && this._moveTokenMatchSet.matches[0].gridPositions.Count == this._tutorialTargetCount && this._tutorialTargetPosition != null && !(this._tutorialTargetTokenDef == null) && this._tutorialTargetCount != -1)))
		{
			this.TokenMoveComplete(false);
			if (!this._moveTokenMatchSet.ContainsTokenType(PuzzleTokenType.JOY) && this.GetActivePuzzleEffectByType(AbilityBehaviorPuzzleEffectType.MATCH_TYPE_MOVE_LOCK) == null && !this._isBonusRound && (!this._isTutorial || this._movesRemaining != 1))
			{
				this.AddResourceValue(PuzzleGameResourceType.MOVES, -1, true);
			}
			foreach (PuzzleGridPosition puzzleGridPosition in this._gridPositions.Values)
			{
				puzzleGridPosition.AssumeTempToken();
			}
			this.ProcessMatchSet(this._moveTokenMatchSet, true);
		}
		else
		{
			this.puzzleGameState = PuzzleGameState.CANCELLING;
			bool flag = false;
			foreach (PuzzleGridPosition puzzleGridPosition2 in this._gridPositions.Values)
			{
				if (puzzleGridPosition2 == this._moveFromGridPosition)
				{
					puzzleGridPosition2.SetToken(puzzleGridPosition2.GetToken(false), false, PuzzleTokenSetType.INSTANT, 0f);
				}
				else
				{
					if (puzzleGridPosition2.GetToken(true) != puzzleGridPosition2.GetToken(false))
					{
						flag = true;
					}
					puzzleGridPosition2.SetToken(puzzleGridPosition2.GetToken(false), false, PuzzleTokenSetType.SLIDE, 0f);
				}
			}
			GameManager.System.Cursor.DetachObject(this._moveTokenCursor);
			TweenUtils.KillTweener(this._moveTokenTweener, true);
			this._moveTokenTweener = HOTween.To(this._moveTokenCursor.transform, 0.15f, new TweenParms().Prop("position", new Vector3(this._moveFromGridPosition.GetToken(false).transform.position.x, this._moveFromGridPosition.GetToken(false).transform.position.y, this._moveTokenCursor.transform.position.z)).Ease(EaseType.EaseInOutSine));
			if (flag)
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiPuzzle.puzzleGrid.badMoveSound, false, 1f, true);
			}
		}
		this._moveTokenMatchSet.ClearMatches();
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00029094 File Offset: 0x00027294
	private void TokenMoveComplete(bool fromCancel = false)
	{
		this.puzzleGameState = PuzzleGameState.WAITING;
		GameManager.Stage.cellPhone.Unlock();
		GameManager.System.Cursor.DetachObject(this._moveTokenCursor);
		GameManager.Stage.effects.lowerCursorContainer.RemoveChild(this._moveTokenCursor, false);
		this._moveTokenCursor.gameObj.SetActive(false);
		this._moveFromGridPosition.GetToken(false).SetAlpha(1f, 0f);
		if (fromCancel)
		{
			this.RefreshDateGiftSlots(false);
			this._puzzleGrid.tokenContainer.interactive = true;
			if (GameManager.System.Cursor.FindMouseTarget(GameManager.System.Cursor.GetMousePosition()) != this._moveFromGridPosition.GetToken(false))
			{
				this._moveFromGridPosition.GetToken(false).UnhighlightToken();
			}
		}
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00005FA7 File Offset: 0x000041A7
	private void OnGridPositionEmpty(PuzzleGridPosition gridPosition)
	{
		if (!gridPosition.IsEdgePosition(PuzzleDirection.UP, 0))
		{
			this._gridPositions[gridPosition.GetKey(-1, 0)].GiveTokenTo(gridPosition);
			gridPosition.GetToken(false).belowToken = this.GetHighestTokenBelow(gridPosition);
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00029170 File Offset: 0x00027370
	private PuzzleMatch GetMatch(PuzzleGridPosition gridPosition, bool matchTemp = false, PuzzleTokenDefinition tokenDefinitionOverride = null)
	{
		PuzzleTokenDefinition y;
		bool fallIsReady;
		if (tokenDefinitionOverride != null)
		{
			y = tokenDefinitionOverride;
			fallIsReady = true;
		}
		else
		{
			y = gridPosition.GetToken(matchTemp).definition;
			fallIsReady = false;
		}
		PuzzleAxis matchAxis = PuzzleAxis.HORIZONTAL;
		int num = 0;
		List<PuzzleGridPosition> list = new List<PuzzleGridPosition>();
		list.Add(gridPosition);
		List<PuzzleGridPosition> list2 = new List<PuzzleGridPosition>();
		foreach (PuzzleDirection puzzleDirection in GameManager.System.Puzzle.CHECK_DIRECTIONS)
		{
			int rowOffset = 0;
			int colOffset = 0;
			switch (puzzleDirection)
			{
			case PuzzleDirection.RIGHT:
				colOffset = 1;
				break;
			case PuzzleDirection.UP:
				rowOffset = -1;
				break;
			case PuzzleDirection.LEFT:
				colOffset = -1;
				break;
			case PuzzleDirection.DOWN:
				rowOffset = 1;
				break;
			}
			if (!gridPosition.IsEdgePosition(puzzleDirection, 0))
			{
				PuzzleGridPosition puzzleGridPosition = this._gridPositions[gridPosition.GetKey(rowOffset, colOffset)];
				while (puzzleGridPosition.IsMatchReady(fallIsReady) && puzzleGridPosition.GetToken(matchTemp).definition == y)
				{
					list2.Add(puzzleGridPosition);
					if (puzzleGridPosition.IsEdgePosition(puzzleDirection, 0))
					{
						break;
					}
					puzzleGridPosition = this._gridPositions[puzzleGridPosition.GetKey(rowOffset, colOffset)];
				}
			}
			if (puzzleDirection == PuzzleDirection.RIGHT || puzzleDirection == PuzzleDirection.DOWN)
			{
				if (list2.Count >= 2)
				{
					list.AddRange(list2);
					if (puzzleDirection == PuzzleDirection.RIGHT)
					{
						matchAxis = PuzzleAxis.HORIZONTAL;
					}
					else if (num == 0)
					{
						matchAxis = PuzzleAxis.VERTICAL;
					}
					else
					{
						matchAxis = PuzzleAxis.CROSS;
					}
					num++;
				}
				list2.Clear();
			}
		}
		if (num > 0)
		{
			if ((this.puzzleGameState == PuzzleGameState.MOVING || this.puzzleGameState == PuzzleGameState.STOPPING) && list.IndexOf(this._moveFromGridPosition.GetToken(false).gridPosition) != -1)
			{
				gridPosition = this._moveFromGridPosition.GetToken(false).gridPosition;
			}
			return new PuzzleMatch(gridPosition, list, matchAxis);
		}
		return null;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00029334 File Offset: 0x00027534
	private void ProcessMatchSet(PuzzleMatchSet matchSet, bool organicMatch = false)
	{
		if (matchSet == null || matchSet.MatchCount() == 0)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < matchSet.MatchCount(); i++)
		{
			PuzzleMatch puzzleMatch = matchSet.matches[i];
			if (puzzleMatch.ContainsTokenType(PuzzleTokenType.BROKEN))
			{
				flag = true;
				matchSet.matches.Remove(puzzleMatch);
				matchSet.matches.Insert(0, puzzleMatch);
			}
		}
		for (int j = 0; j < matchSet.MatchCount(); j++)
		{
			this.ProcessMatch(matchSet.matches[j]);
		}
		if (!this._isBonusRound && !this._isTutorial && organicMatch && !flag && matchSet.MatchCount() > 1 && !GameManager.Stage.girl.IsReadingDialog())
		{
			GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.matchTokenDialogTrigger, 0, false, -1);
		}
		this._matchComboCount++;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0002941C File Offset: 0x0002761C
	private void ProcessMatch(PuzzleMatch match)
	{
		if (match == null)
		{
			return;
		}
		PuzzleGroup puzzleGroup = new PuzzleGroup(match.gridPositions);
		this.ConsumePuzzleGroup(puzzleGroup, this.GetActivePuzzleEffectByType(AbilityBehaviorPuzzleEffectType.SWITCH_NEXT_TYPE_MATCH));
		if (match.ShouldUpgradeBase() && UnityEngine.Random.Range(0f, 1f) <= GameManager.System.Puzzle.GetChanceForPowerToken(match.gridPositions.Count) && !this._isBonusRound && !this._isTutorial)
		{
			puzzleGroup.ProtectPosition(match.basePosition);
			match.basePosition.GetToken(false).SetLevel(2);
		}
		List<AbilityBehaviorDefinition> activePuzzleEffectsByType = this.GetActivePuzzleEffectsByType(AbilityBehaviorPuzzleEffectType.ADD_RESOURCE_ON_FOUR_MATCH);
		if (activePuzzleEffectsByType != null && match.gridPositions.Count > 3 && puzzleGroup.GetTokenTypesInGroup()[0].type == PuzzleTokenType.AFFECTION)
		{
			for (int i = 0; i < activePuzzleEffectsByType.Count; i++)
			{
				this.AddResourceValue(activePuzzleEffectsByType[i].tokenDefinition.resourceType, activePuzzleEffectsByType[i].hardValue, true);
				PuzzleTokenReward puzzleTokenReward = new PuzzleTokenReward(activePuzzleEffectsByType[i].tokenDefinition, 1, activePuzzleEffectsByType[i].hardValue);
				DisplayUtils.CreatePopLabelObject(activePuzzleEffectsByType[i].tokenDefinition.energyTrail.popLabelFont, puzzleTokenReward.GetRewardPopText(true, false), "PopLabelObject").Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer, i > 0);
			}
		}
		activePuzzleEffectsByType = this.GetActivePuzzleEffectsByType(AbilityBehaviorPuzzleEffectType.ADD_RESOURCE_ON_TYPE_MATCH);
		if (activePuzzleEffectsByType != null)
		{
			for (int j = 0; j < activePuzzleEffectsByType.Count; j++)
			{
				if (puzzleGroup.GetTokenTypesInGroup()[0].type == activePuzzleEffectsByType[j].puzzleTokenType)
				{
					this.AddResourceValue(activePuzzleEffectsByType[j].tokenDefinition.resourceType, activePuzzleEffectsByType[j].hardValue, true);
					PuzzleTokenReward puzzleTokenReward2 = new PuzzleTokenReward(activePuzzleEffectsByType[j].tokenDefinition, 1, activePuzzleEffectsByType[j].hardValue);
					DisplayUtils.CreatePopLabelObject(activePuzzleEffectsByType[j].tokenDefinition.energyTrail.popLabelFont, puzzleTokenReward2.GetRewardPopText(true, false), "PopLabelObject").Init(GameManager.System.Cursor.GetMousePosition(), GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer, j > 0);
				}
			}
		}
		AbilityBehaviorDefinition activePuzzleEffectByType = this.GetActivePuzzleEffectByType(AbilityBehaviorPuzzleEffectType.MATCH_TYPE_MOVE_LOCK);
		if (activePuzzleEffectByType != null && puzzleGroup.GetTokenTypesInGroup()[0] != this._moveLockTokenDef)
		{
			this._moveLockTokenDef = null;
			this.RemovePuzzleEffect(activePuzzleEffectByType);
		}
		if (!this._isBonusRound && !this._isTutorial)
		{
			if (match.gridPositions.Count > 3)
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, this._puzzleGrid.tokenPowerMatchSounds, match.gridPositions.Count - 4, false, 1f, true);
				if (puzzleGroup.GetTokenTypesInGroup()[0].type != PuzzleTokenType.BROKEN && !GameManager.Stage.girl.IsReadingDialog())
				{
					GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.matchTokenDialogTrigger, 0, false, -1);
				}
			}
			if (this._matchComboCount == 2 && puzzleGroup.GetTokenTypesInGroup()[0].type != PuzzleTokenType.BROKEN && !GameManager.Stage.girl.IsReadingDialog())
			{
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.matchTokenDialogTrigger, 0, false, -1);
			}
			if (puzzleGroup.GetTokenTypesInGroup()[0].type == PuzzleTokenType.BROKEN)
			{
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.matchTokenDialogTrigger, 1, false, -1);
			}
		}
		this.DestroyPuzzleGroup(puzzleGroup, false);
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x000297D0 File Offset: 0x000279D0
	public void ConsumePuzzleGroup(PuzzleGroup puzzleGroup, AbilityBehaviorDefinition switchMatchTypePuzzleEffect = null)
	{
		if (this._isTutorial && !this._tutorialFinished)
		{
			this._tutorialMoveMade = true;
		}
		Dictionary<PuzzleGridPosition, PuzzleTokenReward> groupRewards = puzzleGroup.GetGroupRewards(switchMatchTypePuzzleEffect);
		for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
		{
			PuzzleGridPosition puzzleGridPosition = puzzleGroup.gridPositions[i];
			PuzzleToken token = puzzleGridPosition.GetToken(false);
			PuzzleTokenDefinition puzzleTokenDefinition = token.definition;
			if (switchMatchTypePuzzleEffect != null && puzzleTokenDefinition.type == switchMatchTypePuzzleEffect.puzzleTokenType)
			{
				puzzleTokenDefinition = switchMatchTypePuzzleEffect.tokenDefinition;
				this.RemovePuzzleEffect(switchMatchTypePuzzleEffect);
			}
			string popText = null;
			if (groupRewards.ContainsKey(puzzleGridPosition))
			{
				PuzzleTokenReward puzzleTokenReward = groupRewards[puzzleGridPosition];
				this.AddResourceValue(puzzleTokenReward.tokenDefinition.resourceType, puzzleTokenReward.rewardValue, true);
				popText = puzzleTokenReward.GetRewardPopText(true, this._isBonusRound);
				DisplayUtils.CreateBurstLabelObject(puzzleTokenDefinition.energyTrail.popLabelFont, puzzleTokenReward.GetRewardPopText(false, false), "BurstLabelObject").Init(new Vector3(token.globalX + 41f, token.globalY - 41f, 1f), GameManager.Stage.uiPuzzle.puzzleGrid.tokenContainer);
			}
			EnergyTrail component = new GameObject("EnergyTrail", new Type[]
			{
				typeof(EnergyTrail)
			}).GetComponent<EnergyTrail>();
			EnergyTrailFormat trailFormat = EnergyTrailFormat.FULL;
			if (this._activeTokenEnergyTrails.Count < GameManager.System.Puzzle.GetTokenEnergyTrailLimit())
			{
				component.EnergyTrailCompleteEvent += this.OnTokenEnergyTrailComplete;
				this._activeTokenEnergyTrails.Add(component);
			}
			else
			{
				trailFormat = EnergyTrailFormat.START;
			}
			Vector3 origin = new Vector3(token.gameObj.transform.position.x + 41f, token.gameObj.transform.position.y - 41f, 0f);
			EnergyTrailDynamicDetails energyTrailDynamicDetails = new EnergyTrailDynamicDetails(puzzleTokenDefinition.energyTrail);
			if (this._isBonusRound)
			{
				energyTrailDynamicDetails.energyTrailDestY = 150f;
				energyTrailDynamicDetails.energyTrailDestHeight = 200f;
			}
			component.Init(origin, puzzleTokenDefinition.energyTrail, popText, trailFormat, energyTrailDynamicDetails);
		}
		List<PuzzleTokenDefinition> tokenTypesInGroup = puzzleGroup.GetTokenTypesInGroup();
		for (int j = 0; j < tokenTypesInGroup.Count; j++)
		{
			if (this._isBonusRound)
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, tokenTypesInGroup[j].bonusMatchSound, false, 1f, true);
			}
			else
			{
				GameManager.System.Audio.Play(AudioCategory.SOUND, tokenTypesInGroup[j].matchSound, false, 1f, true);
			}
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00029A58 File Offset: 0x00027C58
	public void DestroyPuzzleGroup(PuzzleGroup puzzleGroup, bool showPing = false)
	{
		for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
		{
			PuzzleGridPosition puzzleGridPosition = puzzleGroup.gridPositions[i];
			if (puzzleGroup.protectedPositions.IndexOf(puzzleGridPosition) == -1)
			{
				PuzzleToken token = puzzleGridPosition.GetToken(false);
				if (showPing)
				{
					new GameObject("EnergyTrail", new Type[]
					{
						typeof(EnergyTrail)
					}).GetComponent<EnergyTrail>().Init(new Vector3(token.gameObj.transform.position.x + 41f, token.gameObj.transform.position.y - 41f, 0f), token.definition.energyTrail, null, EnergyTrailFormat.START, null);
				}
				this.DestroyToken(token);
				puzzleGridPosition.RemoveToken();
				this.CreateToken(puzzleGridPosition.col, false);
			}
		}
		GameManager.System.Audio.Play(AudioCategory.SOUND, this._puzzleGrid.tokenMatchSounds, this._matchComboCount, false, 1f, true);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00029B64 File Offset: 0x00027D64
	public void SwitchPuzzleGroupTokensWith(PuzzleGroup puzzleGroup, List<PuzzleTokenDefinition> tokenDefinitions, int level, bool weighted)
	{
		for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
		{
			PuzzleGridPosition puzzleGridPosition = puzzleGroup.gridPositions[i];
			PuzzleToken token = puzzleGridPosition.GetToken(false);
			if (level <= 0)
			{
				level = token.level;
			}
			this.DestroyToken(token);
			puzzleGridPosition.SetToken(this.GenerateNewToken(tokenDefinitions[UnityEngine.Random.Range(0, tokenDefinitions.Count)], level, weighted), false, PuzzleTokenSetType.INSTANT, 0f);
		}
		this.ProcessMatchSet(this.GetPuzzleGroupMatchSet(puzzleGroup), false);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00029BE4 File Offset: 0x00027DE4
	private PuzzleMatchSet GetPuzzleGroupMatchSet(PuzzleGroup puzzleGroup)
	{
		puzzleGroup.ShuffleGroup();
		PuzzleMatchSet puzzleMatchSet = new PuzzleMatchSet();
		for (int i = 0; i < puzzleGroup.gridPositions.Count; i++)
		{
			PuzzleMatch match = this.GetMatch(puzzleGroup.gridPositions[i], false, null);
			if (match != null)
			{
				puzzleMatchSet.AddMatch(match, false);
			}
		}
		return puzzleMatchSet;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00029C34 File Offset: 0x00027E34
	private void OnTokenEnergyTrailComplete(EnergyTrail energyTrail)
	{
		energyTrail.EnergyTrailCompleteEvent -= this.OnTokenEnergyTrailComplete;
		if (this._activeTokenEnergyTrails.Contains(energyTrail))
		{
			this._activeTokenEnergyTrails.Remove(energyTrail);
			if (this._isBonusRound && !GameManager.Stage.girl.IsReadingDialog())
			{
				int num = Mathf.Clamp(Mathf.FloorToInt((float)this.GetResourceValue(PuzzleGameResourceType.AFFECTION) / (float)this.GetResourceMax(PuzzleGameResourceType.AFFECTION) / 0.25f), 0, 3);
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.sexualSoundsDialogTrigger, Mathf.Clamp(UnityEngine.Random.Range(num - 1, num + 2), 0, 3), true, -1);
			}
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00029CE0 File Offset: 0x00027EE0
	private void OnPuzzleStatusItemSlotDown(PuzzleStatusItemSlot itemSlot)
	{
		if (this.puzzleGameState != PuzzleGameState.WAITING || this._unreadyTokens.Count > 0 || this._movesRemaining <= 0 || this._isBonusRound || this._currentAffection == this._goalAffection || this._tutorialLocked || itemSlot.itemDefinition == null)
		{
			return;
		}
		this.StartItemGive(itemSlot);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00029D44 File Offset: 0x00027F44
	private void StartItemGive(PuzzleStatusItemSlot itemSlot)
	{
		this.puzzleGameState = PuzzleGameState.ITEM;
		GameManager.Stage.cellPhone.Lock(false);
		this._puzzleItemSlot = itemSlot;
		this._puzzleItemCursor.gameObj.SetActive(true);
		this._puzzleItemCursor.sprite.SetSprite(this._puzzleItemSlot.itemDefinition.iconName);
		GameManager.Stage.effects.lowerCursorContainer.AddChild(this._puzzleItemCursor);
		GameManager.System.Cursor.AttachObject(this._puzzleItemCursor, 0f, 0f);
		GameManager.System.Cursor.SetAbsorber(this._puzzleStatus.giftZone, true);
		GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.pickUpItemSound, false, 1f, true);
		this.RefreshDateGiftSlots(false);
		this._puzzleItemSlot.itemIcon.SetAlpha(0f, 0f);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00029E40 File Offset: 0x00028040
	private void EndItemGive()
	{
		TweenUtils.KillTweener(this._puzzleItemTweener, true);
		GameManager.System.Cursor.DetachObject(this._puzzleItemCursor);
		if (GameManager.System.Cursor.GetMouseTarget() == this._puzzleStatus.giftZone)
		{
			if (this.PerformAbility(this._puzzleItemSlot.itemDefinition.ability))
			{
				this.AddResourceValue(PuzzleGameResourceType.SENTIMENT, -this._puzzleItemSlot.itemDefinition.sentimentCost, true);
				this._puzzleItemSlot.itemDefinition = null;
				if (!this._isTutorial && !this._isBonusRound)
				{
					GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.givenDateGiftDialogTrigger, 0, false, -1);
				}
				GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.itemGiveSuccessSound, false, 1f, true);
				this.ItemGiveComplete(false);
				return;
			}
			if (!this._isTutorial && !this._isBonusRound)
			{
				GameManager.System.Girl.TriggerDialog(GameManager.Stage.uiGirl.givenDateGiftDialogTrigger, 1, false, -1);
			}
			GameManager.System.Audio.Play(AudioCategory.SOUND, GameManager.Stage.uiGirl.itemGiveFailureSound, false, 1f, true);
		}
		this.puzzleGameState = PuzzleGameState.CANCELLING_ITEM;
		this._puzzleItemTweener = HOTween.To(this._puzzleItemCursor.gameObj.transform, 0.2f, new TweenParms().Prop("position", new Vector3(this._puzzleItemSlot.gameObj.transform.position.x, this._puzzleItemSlot.gameObj.transform.position.y, this._puzzleItemCursor.gameObj.transform.position.z)).Ease(EaseType.EaseOutCubic));
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0002A020 File Offset: 0x00028220
	private void ItemGiveComplete(bool fromCancel = false)
	{
		this._puzzleItemSlot = null;
		GameManager.System.Cursor.ClearAbsorber();
		GameManager.Stage.effects.lowerCursorContainer.RemoveChild(this._puzzleItemCursor, false);
		this._puzzleItemCursor.gameObj.SetActive(false);
		if (fromCancel)
		{
			this.puzzleGameState = PuzzleGameState.WAITING;
			GameManager.Stage.cellPhone.Unlock();
		}
		this.RefreshDateGiftSlots(false);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002A090 File Offset: 0x00028290
	private bool PerformAbility(AbilityDefinition abilityDefinition)
	{
		this._activePuzzleAbility = new PuzzleAbility(abilityDefinition);
		this._activeAbilityTargetGroup = this._activePuzzleAbility.GetPuzzleGroup(this._activePuzzleAbility.definition.targetConditionSet);
		if (this._activeAbilityTargetGroup.gridPositions.Count < abilityDefinition.targetMinimumCount)
		{
			this._activeAbilityTargetGroup = null;
			this._activePuzzleAbility = null;
			return false;
		}
		if (this._activePuzzleAbility.definition.selectableTarget)
		{
			this.StartAbilityTargeting();
		}
		else
		{
			this.UseActiveAbility();
		}
		return true;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002A114 File Offset: 0x00028314
	private void StartAbilityTargeting()
	{
		this.puzzleGameState = PuzzleGameState.TARGETING;
		foreach (PuzzleGridPosition puzzleGridPosition in this._gridPositions.Values)
		{
			if (this._activeAbilityTargetGroup.gridPositions.IndexOf(puzzleGridPosition) == -1)
			{
				puzzleGridPosition.GetToken(false).SetAlpha(0.32f, 0.2f);
			}
		}
		this._puzzleGrid.ShowTargetPrompt();
		if (this._activePuzzleAbility.definition.selectableEnergyTrail != null)
		{
			EnergyTrail component = new GameObject("SelectableEnergyTrail", new Type[]
			{
				typeof(EnergyTrail)
			}).GetComponent<EnergyTrail>();
			component.Init(GameManager.System.Cursor.GetMousePosition(), this._activePuzzleAbility.definition.selectableEnergyTrail, null, EnergyTrailFormat.END, null);
			component.OverrideEnergySurge(GameManager.Stage.uiGirl.itemGiveEnergyTrail.girlEnergySurge);
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002A21C File Offset: 0x0002841C
	private void EndAbilityTargeting(PuzzleToken targetToken)
	{
		foreach (PuzzleGridPosition puzzleGridPosition in this._gridPositions.Values)
		{
			puzzleGridPosition.GetToken(false).SetAlpha(1f, 0.2f);
		}
		this._puzzleGrid.HideTargetPrompt();
		PuzzleGroup expandedPuzzleGroupFromTarget = this._activePuzzleAbility.GetExpandedPuzzleGroupFromTarget(targetToken.gridPosition, this._activePuzzleAbility.definition.targetConditionSet);
		for (int i = 0; i < expandedPuzzleGroupFromTarget.gridPositions.Count; i++)
		{
			expandedPuzzleGroupFromTarget.gridPositions[i].HideTargetGuide();
		}
		this._activePuzzleAbility.SetSelectablePuzzleGroup(expandedPuzzleGroupFromTarget);
		this.UseActiveAbility();
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00005FDF File Offset: 0x000041DF
	private void UseActiveAbility()
	{
		this.puzzleGameState = PuzzleGameState.ABILITY;
		this._activePuzzleAbility.AbilityCompleteEvent += this.OnAbilityComplete;
		this._activePuzzleAbility.Use();
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0002A2E8 File Offset: 0x000284E8
	private void OnAbilityComplete(PuzzleAbility ability)
	{
		bool flag = false;
		if (this._activePuzzleAbility.definition.selectableTarget && this._activePuzzleAbility.definition.postRehighlightTarget)
		{
			flag = true;
		}
		this._activePuzzleAbility.AbilityCompleteEvent -= this.OnAbilityComplete;
		this._activePuzzleAbility = null;
		this._activeAbilityTargetGroup = null;
		this.puzzleGameState = PuzzleGameState.WAITING;
		GameManager.Stage.cellPhone.Unlock();
		this.RefreshDateGiftSlots(false);
		if (flag)
		{
			PuzzleToken puzzleToken = GameManager.System.Cursor.GetMouseTarget() as PuzzleToken;
			if (puzzleToken != null)
			{
				this.OnTokenOver(puzzleToken);
			}
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0002A388 File Offset: 0x00028588
	public void ApplyPuzzleEffect(AbilityBehaviorDefinition puzzleEffect)
	{
		this._puzzleEffects.Add(puzzleEffect);
		AbilityBehaviorPuzzleEffectType puzzleEffectType = puzzleEffect.puzzleEffectType;
		if (puzzleEffectType != AbilityBehaviorPuzzleEffectType.INCREASE_TOKEN_WEIGHT)
		{
			if (puzzleEffectType == AbilityBehaviorPuzzleEffectType.MATCH_TYPE_MOVE_LOCK)
			{
				this._moveLockTokenDef = this._activePuzzleAbility.GetPuzzleGroupByRef(puzzleEffect.groupRef).GetTokenTypesInGroup()[0];
			}
		}
		else
		{
			Dictionary<PuzzleTokenDefinition, int> tokenWeights;
			PuzzleTokenDefinition tokenDefinition;
			int num = (tokenWeights = this._tokenWeights)[tokenDefinition = puzzleEffect.tokenDefinition];
			tokenWeights[tokenDefinition] = num + Mathf.RoundToInt((float)puzzleEffect.tokenDefinition.weight * puzzleEffect.percentOfValue);
		}
		this._puzzleStatus.UpdatePuzzleEffects(this._puzzleEffects);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0002A420 File Offset: 0x00028620
	public AbilityBehaviorDefinition GetActivePuzzleEffectByType(AbilityBehaviorPuzzleEffectType puzzleEffectType)
	{
		List<AbilityBehaviorDefinition> activePuzzleEffectsByType = this.GetActivePuzzleEffectsByType(puzzleEffectType);
		if (activePuzzleEffectsByType != null)
		{
			return activePuzzleEffectsByType[0];
		}
		return null;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0002A444 File Offset: 0x00028644
	public List<AbilityBehaviorDefinition> GetActivePuzzleEffectsByType(AbilityBehaviorPuzzleEffectType puzzleEffectType)
	{
		List<AbilityBehaviorDefinition> list = new List<AbilityBehaviorDefinition>();
		for (int i = 0; i < this._puzzleEffects.Count; i++)
		{
			if (this._puzzleEffects[i].puzzleEffectType == puzzleEffectType)
			{
				list.Add(this._puzzleEffects[i]);
			}
		}
		if (list.Count > 0)
		{
			return list;
		}
		return null;
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0000600A File Offset: 0x0000420A
	private void RemovePuzzleEffect(AbilityBehaviorDefinition puzzleEffect)
	{
		if (!this._puzzleEffects.Contains(puzzleEffect))
		{
			return;
		}
		this._puzzleEffects.Remove(puzzleEffect);
		this._puzzleStatus.UpdatePuzzleEffects(this._puzzleEffects);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002A4A0 File Offset: 0x000286A0
	private PuzzleGridPosition GetLowestEmptyGridPosition(int col)
	{
		PuzzleGridPosition puzzleGridPosition = null;
		for (int i = 6; i >= 0; i--)
		{
			puzzleGridPosition = this._gridPositions[this.RowColKey(i, col)];
			if (!puzzleGridPosition.HasToken())
			{
				break;
			}
		}
		return puzzleGridPosition;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002A4D8 File Offset: 0x000286D8
	private PuzzleToken GetHighestTokenBelow(PuzzleGridPosition gridPosition)
	{
		for (int i = gridPosition.row + 1; i < 7; i++)
		{
			PuzzleGridPosition puzzleGridPosition = this._gridPositions[this.RowColKey(i, gridPosition.col)];
			if (puzzleGridPosition.HasToken())
			{
				return puzzleGridPosition.GetToken(false);
			}
		}
		return null;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00006039 File Offset: 0x00004239
	private string RowColKey(int row, int col)
	{
		return row.ToString() + "," + col.ToString();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00006053 File Offset: 0x00004253
	public Dictionary<string, PuzzleGridPosition> GetPuzzleGridPositions()
	{
		return this._gridPositions;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002A524 File Offset: 0x00028724
	public PuzzleGroup GetPositionsContainingAsGroup(PuzzleTokenType tokenType)
	{
		List<PuzzleGridPosition> list = new List<PuzzleGridPosition>();
		foreach (PuzzleGridPosition puzzleGridPosition in this._gridPositions.Values)
		{
			if (puzzleGridPosition.GetToken(false) != null && puzzleGridPosition.GetToken(false).definition.type == tokenType)
			{
				list.Add(puzzleGridPosition);
			}
		}
		if (list.Count > 0)
		{
			return new PuzzleGroup(list);
		}
		return null;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0000605B File Offset: 0x0000425B
	public int GetResourceValue(PuzzleGameResourceType resourceType)
	{
		switch (resourceType)
		{
		case PuzzleGameResourceType.AFFECTION:
			return this._currentAffection;
		case PuzzleGameResourceType.PASSION:
			return this._currentPassion;
		case PuzzleGameResourceType.MOVES:
			return this._movesRemaining;
		case PuzzleGameResourceType.SENTIMENT:
			return this._currentSentiment;
		default:
			return -1;
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00006092 File Offset: 0x00004292
	public int GetResourceMax(PuzzleGameResourceType resourceType)
	{
		switch (resourceType)
		{
		case PuzzleGameResourceType.AFFECTION:
			return this._goalAffection;
		case PuzzleGameResourceType.PASSION:
			return this._maxPassion;
		case PuzzleGameResourceType.MOVES:
			return this._maxMoves;
		case PuzzleGameResourceType.SENTIMENT:
			return this._currentSentiment;
		default:
			return -1;
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0002A5B8 File Offset: 0x000287B8
	public void SetResourceValue(PuzzleGameResourceType resourceType, int val, bool animate = true)
	{
		float num = 0.1f;
		if (!animate)
		{
			num = 0f;
		}
		switch (resourceType)
		{
		case PuzzleGameResourceType.AFFECTION:
			this._currentAffection = Mathf.Clamp(val, 0, this._goalAffection);
			TweenUtils.KillTweener(this.affectionMeterTweener, false);
			if (animate)
			{
				this.affectionMeterTweener = HOTween.To(this, Mathf.Min(num * (float)Mathf.Abs(this._currentAffection - this.currentDisplayAffection), 1.5f), new TweenParms().Prop("currentDisplayAffection", this._currentAffection).Ease(EaseType.EaseOutSine));
				return;
			}
			this.currentDisplayAffection = this._currentAffection;
			this.UpdateAffectionMeterDisplay();
			return;
		case PuzzleGameResourceType.PASSION:
			this._currentPassion = Mathf.Clamp(val, 0, this._maxPassion);
			while (this._currentPassion >= GameManager.System.Puzzle.GetPassionLevelCost(this.currentPassionLevel + 1))
			{
				int currentPassionLevel = this.currentPassionLevel;
				this.currentPassionLevel = currentPassionLevel + 1;
				this._puzzleStatus.SetPassionLevel(this.currentPassionLevel);
			}
			while (this._currentPassion < GameManager.System.Puzzle.GetPassionLevelCost(this.currentPassionLevel))
			{
				int currentPassionLevel = this.currentPassionLevel;
				this.currentPassionLevel = currentPassionLevel - 1;
				this._puzzleStatus.SetPassionLevel(this.currentPassionLevel);
			}
			if (this._currentPassion == this._maxPassion)
			{
				this._puzzleStatus.passionMax.SetAlpha(1f, 0f);
			}
			else
			{
				this._puzzleStatus.passionMax.SetAlpha(0f, 0f);
			}
			GameManager.Stage.tooltip.Refresh();
			return;
		case PuzzleGameResourceType.MOVES:
			this._movesRemaining = Mathf.Clamp(val, 0, this._maxMoves);
			this._puzzleStatus.movesLabel.SetText(this._movesRemaining, num, true, 1.5f);
			if (this._movesRemaining == this._maxMoves)
			{
				this._puzzleStatus.movesMax.SetAlpha(1f, 0f);
				return;
			}
			this._puzzleStatus.movesMax.SetAlpha(0f, 0f);
			return;
		case PuzzleGameResourceType.SENTIMENT:
			this._currentSentiment = Mathf.Clamp(val, 0, this._maxSentiment);
			this._puzzleStatus.sentimentLabel.SetText(this._currentSentiment, num, true, 1.5f);
			if (this._currentSentiment == this._maxSentiment)
			{
				this._puzzleStatus.sentimentMax.SetAlpha(1f, 0f);
			}
			else
			{
				this._puzzleStatus.sentimentMax.SetAlpha(0f, 0f);
			}
			this.RefreshDateGiftSlots(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x000060C9 File Offset: 0x000042C9
	public void AddResourceValue(PuzzleGameResourceType resourceType, int val, bool animate = true)
	{
		if (this._isBonusRound)
		{
			resourceType = PuzzleGameResourceType.AFFECTION;
		}
		else if (resourceType == PuzzleGameResourceType.AFFECTION && val > 0)
		{
			this._movePointsEarned += val;
		}
		this.SetResourceValue(resourceType, this.GetResourceValue(resourceType) + val, animate);
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002A848 File Offset: 0x00028A48
	private void UpdateAffectionMeterDisplay()
	{
		this._puzzleStatus.affectionLabel.SetText(StringUtils.FormatIntAsCurrency(this.currentDisplayAffection, false) + " / " + StringUtils.FormatIntAsCurrency(this._goalAffection, false));
		this._puzzleStatus.affectionMeterMask.uiMask.size.x = 452f * (1f - (float)this.currentDisplayAffection / (float)this._goalAffection);
		this._puzzleStatus.affectionMeterMask.uiMask.Build();
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002A8D4 File Offset: 0x00028AD4
	public void RefreshDateGiftSlots(bool repopulate = false)
	{
		for (int i = 0; i < this._puzzleStatus.itemSlots.Count; i++)
		{
			if (repopulate)
			{
				this._puzzleStatus.itemSlots[i].PopulateSlotItem();
			}
			else
			{
				this._puzzleStatus.itemSlots[i].RefreshSlotItem();
			}
			if (this.puzzleGameState != PuzzleGameState.WAITING || this._unreadyTokens.Count > 0 || this._movesRemaining <= 0 || this._isBonusRound || this._currentAffection == this._goalAffection || this._tutorialLocked || this._puzzleStatus.itemSlots[i].itemDefinition == null || this._currentSentiment < this._puzzleStatus.itemSlots[i].itemDefinition.sentimentCost)
			{
				this._puzzleStatus.itemSlots[i].button.Disable();
			}
			else
			{
				this._puzzleStatus.itemSlots[i].button.Enable();
			}
		}
		GameManager.Stage.tooltip.Refresh();
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0002A9FC File Offset: 0x00028BFC
	public void TutorialUnlock(PuzzleTokenDefinition tokenDef, string gridKey, int tokenCount)
	{
		if (!this._isTutorial || !this._tutorialLocked)
		{
			return;
		}
		if (tokenDef != null)
		{
			this._tutorialTargetTokenDef = tokenDef;
		}
		if (gridKey != null)
		{
			this._tutorialTargetPosition = this._gridPositions[gridKey];
		}
		this._tutorialTargetCount = tokenCount;
		this._tutorialLocked = false;
		this.RefreshDateGiftSlots(false);
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000060FE File Offset: 0x000042FE
	public void TutoriaLock()
	{
		if (!this._isTutorial || this._tutorialLocked)
		{
			return;
		}
		this._tutorialTargetTokenDef = null;
		this._tutorialTargetPosition = null;
		this._tutorialTargetCount = -1;
		this._tutorialLocked = true;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0002AA54 File Offset: 0x00028C54
	public void TutorialFinsihed()
	{
		if (!this._isTutorial || this._tutorialFinished)
		{
			return;
		}
		this._tutorialGrid.Clear();
		this._tutorialGrid = null;
		this._tutorialTargetTokenDef = null;
		this._tutorialTargetPosition = null;
		this._tutorialTargetCount = -1;
		this._tutorialLocked = false;
		this._tutorialFinished = true;
		this.RefreshDateGiftSlots(false);
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0002AAB0 File Offset: 0x00028CB0
	public int GetPuzzleMoneyReward()
	{
		int totalGirlsRelationshipLevel = GameManager.System.Player.GetTotalGirlsRelationshipLevel();
		if (this._isTutorial)
		{
			return 1000;
		}
		if (this.isVictorious)
		{
			return Mathf.RoundToInt(((float)(1000 + 64 * totalGirlsRelationshipLevel) + (float)this._movesRemaining * Mathf.Lerp(20f, 80f, (float)(totalGirlsRelationshipLevel / 48)) + (float)this._currentSentiment * Mathf.Lerp(3f, 12f, (float)(totalGirlsRelationshipLevel / 48))) * (1f + (float)this._currentPassionLevel * 0.01f));
		}
		return Mathf.Max(Mathf.RoundToInt((float)(1000 + 64 * totalGirlsRelationshipLevel) * 0.75f * ((float)this._currentAffection / (float)this._goalAffection)), 100);
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0002AB70 File Offset: 0x00028D70
	public void Destroy()
	{
		TweenUtils.KillTweener(this._moveTokenTweener, true);
		this._moveTokenTweener = null;
		TweenUtils.KillTweener(this._puzzleItemTweener, true);
		this._puzzleItemTweener = null;
		TweenUtils.KillTweener(this.affectionMeterTweener, true);
		this.affectionMeterTweener = null;
		GameManager.Stage.MouseDownEvent -= this.OnStageDown;
		GameManager.Stage.MouseUpEvent -= this.OnStageUp;
		foreach (PuzzleGridPosition puzzleGridPosition in this._gridPositions.Values)
		{
			PuzzleToken token = puzzleGridPosition.GetToken(false);
			if (token != null)
			{
				this.DestroyToken(token);
			}
			puzzleGridPosition.GridPositionEmptyEvent -= this.OnGridPositionEmpty;
			puzzleGridPosition.Destroy();
		}
		this._gridPositions.Clear();
		this._gridPositions = null;
		this._unreadyTokens.Clear();
		this._unreadyTokens = null;
		this._tokenQueue.Clear();
		this._tokenQueue = null;
		this._tokenWeights.Clear();
		this._tokenWeights = null;
		this._tokenDefs = null;
		this._activeTokenEnergyTrails.Clear();
		this._activeTokenEnergyTrails = null;
		this._puzzleEffects.Clear();
		this._puzzleEffects = null;
		this._moveTokenCursor.gameObj.SetActive(true);
		UnityEngine.Object.Destroy(this._moveTokenCursor.gameObj);
		this._moveTokenCursor = null;
		this._puzzleItemCursor.gameObj.SetActive(true);
		UnityEngine.Object.Destroy(this._puzzleItemCursor.gameObj);
		this._puzzleItemCursor = null;
		for (int i = 0; i < this._puzzleStatus.itemSlots.Count; i++)
		{
			this._puzzleStatus.itemSlots[i].PuzzleStatusItemSlotDownEvent -= this.OnPuzzleStatusItemSlotDown;
		}
		this._puzzleStatus.giftZone.gameObj.SetActive(false);
		this._puzzleGrid = null;
		this._puzzleStatus = null;
	}

	// Token: 0x040006AA RID: 1706
	private UIPuzzleGrid _puzzleGrid;

	// Token: 0x040006AB RID: 1707
	private UIPuzzleStatus _puzzleStatus;

	// Token: 0x040006AC RID: 1708
	private bool _isTutorial;

	// Token: 0x040006AD RID: 1709
	private List<List<PuzzleTokenDefinition>> _tutorialGrid;

	// Token: 0x040006AE RID: 1710
	private bool _tutorialStarted;

	// Token: 0x040006AF RID: 1711
	private bool _tutorialFinished;

	// Token: 0x040006B0 RID: 1712
	private bool _tutorialLocked;

	// Token: 0x040006B1 RID: 1713
	private bool _tutorialMoveMade;

	// Token: 0x040006B2 RID: 1714
	private PuzzleTokenDefinition _tutorialTargetTokenDef;

	// Token: 0x040006B3 RID: 1715
	private PuzzleGridPosition _tutorialTargetPosition;

	// Token: 0x040006B4 RID: 1716
	private int _tutorialTargetCount;

	// Token: 0x040006B5 RID: 1717
	private bool _isBonusRound;

	// Token: 0x040006B6 RID: 1718
	private float _bonusRoundDrainTimestamp;

	// Token: 0x040006B7 RID: 1719
	private float _bonusRoundDrainDelay;

	// Token: 0x040006B8 RID: 1720
	private bool _bonusBraOn;

	// Token: 0x040006B9 RID: 1721
	private PuzzleTokenDefinition[] _tokenDefs;

	// Token: 0x040006BA RID: 1722
	private Dictionary<PuzzleTokenDefinition, int> _tokenWeights;

	// Token: 0x040006BB RID: 1723
	private List<PuzzleToken> _tokenQueue;

	// Token: 0x040006BC RID: 1724
	private List<PuzzleToken> _unreadyTokens;

	// Token: 0x040006BD RID: 1725
	private Dictionary<string, PuzzleGridPosition> _gridPositions;

	// Token: 0x040006BE RID: 1726
	private PuzzleGameState _state;

	// Token: 0x040006BF RID: 1727
	private bool _victory;

	// Token: 0x040006C0 RID: 1728
	private PuzzleGridPosition _moveFromGridPosition;

	// Token: 0x040006C1 RID: 1729
	private PuzzleGridPosition _moveToGridPosition;

	// Token: 0x040006C2 RID: 1730
	private PuzzleMatchSet _moveTokenMatchSet;

	// Token: 0x040006C3 RID: 1731
	private SpriteObject _moveTokenCursor;

	// Token: 0x040006C4 RID: 1732
	private Tweener _moveTokenTweener;

	// Token: 0x040006C5 RID: 1733
	private List<EnergyTrail> _activeTokenEnergyTrails;

	// Token: 0x040006C6 RID: 1734
	private PuzzleStatusItemSlot _puzzleItemSlot;

	// Token: 0x040006C7 RID: 1735
	private SpriteObject _puzzleItemCursor;

	// Token: 0x040006C8 RID: 1736
	private Tweener _puzzleItemTweener;

	// Token: 0x040006C9 RID: 1737
	private PuzzleAbility _activePuzzleAbility;

	// Token: 0x040006CA RID: 1738
	private PuzzleGroup _activeAbilityTargetGroup;

	// Token: 0x040006CB RID: 1739
	private List<AbilityBehaviorDefinition> _puzzleEffects;

	// Token: 0x040006CC RID: 1740
	private PuzzleTokenDefinition _moveLockTokenDef;

	// Token: 0x040006CD RID: 1741
	private int _currentPassionLevel;

	// Token: 0x040006CE RID: 1742
	private int _currentPassion;

	// Token: 0x040006CF RID: 1743
	private int _maxPassion;

	// Token: 0x040006D0 RID: 1744
	private int _currentAffection;

	// Token: 0x040006D1 RID: 1745
	private int _goalAffection;

	// Token: 0x040006D2 RID: 1746
	private int _movesRemaining;

	// Token: 0x040006D3 RID: 1747
	private int _maxMoves;

	// Token: 0x040006D4 RID: 1748
	private int _currentSentiment;

	// Token: 0x040006D5 RID: 1749
	private int _maxSentiment;

	// Token: 0x040006D6 RID: 1750
	private bool _initialTokenDropped;

	// Token: 0x040006D7 RID: 1751
	private int _matchComboCount;

	// Token: 0x040006D8 RID: 1752
	private int _movePointsEarned;

	// Token: 0x040006D9 RID: 1753
	public int currentDisplayAffection;

	// Token: 0x040006DA RID: 1754
	public Tweener affectionMeterTweener;

	// Token: 0x020000FA RID: 250
	// (Invoke) Token: 0x06000562 RID: 1378
	public delegate void PuzzleGameDelegate();
}
