using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class PuzzleToken : SpriteObject
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x060005C8 RID: 1480 RVA: 0x000065EE File Offset: 0x000047EE
	// (remove) Token: 0x060005C9 RID: 1481 RVA: 0x00006607 File Offset: 0x00004807
	public event PuzzleToken.TokenUnqueueDelegate TokenUnqueueEvent;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x060005CA RID: 1482 RVA: 0x00006620 File Offset: 0x00004820
	// (remove) Token: 0x060005CB RID: 1483 RVA: 0x00006639 File Offset: 0x00004839
	public event PuzzleToken.TokenUnreadyDelegate TokenUnreadyEvent;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060005CC RID: 1484 RVA: 0x00006652 File Offset: 0x00004852
	// (remove) Token: 0x060005CD RID: 1485 RVA: 0x0000666B File Offset: 0x0000486B
	public event PuzzleToken.TokenReadyDelegate TokenReadyEvent;

	// Token: 0x060005CE RID: 1486 RVA: 0x00006684 File Offset: 0x00004884
	protected override void OnAwake()
	{
		base.OnAwake();
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002DE14 File Offset: 0x0002C014
	public void Init(PuzzleTokenDefinition tokenDef, int levelValue, bool weighted)
	{
		this.definition = tokenDef;
		this.level = levelValue;
		this.isWeighted = weighted;
		this.sprite.SetSprite(GameManager.Stage.uiPuzzle.puzzleGrid.puzzleTokenSpriteCollection, this.definition.levels[this.level - 1].GetSpriteName(false, GameManager.System.Puzzle.Game.isBonusRound));
		this._collider = this.gameObj.GetComponent<BoxCollider>();
		this._collider.isTrigger = true;
		this.state = PuzzleTokenState.INITED;
		this.TokenUnreadyEvent(this);
		this._highlighted = false;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0000306D File Offset: 0x0000126D
	protected override void OnStart()
	{
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0002DEC0 File Offset: 0x0002C0C0
	protected override void OnUpdate()
	{
		switch (this.state)
		{
		case PuzzleTokenState.PREFALL:
			if (GameManager.System.Lifetime(true) - this._fallTimestamp >= this._fallDelay)
			{
				this.state = PuzzleTokenState.FALLING;
				this.TokenUnqueueEvent(this);
			}
			break;
		case PuzzleTokenState.FALLING:
			this.gameObj.transform.localPosition += Vector3.down * (1f + 500f * ((GameManager.System.Lifetime(true) - this._fallTimestamp - this._fallDelay) * 3f)) * Time.deltaTime;
			if (this.gameObj.transform.localPosition.y <= this._fallY)
			{
				base.SetLocalPosition(this.gameObj.transform.localPosition.x, this._fallY);
				this.state = PuzzleTokenState.READY;
				this.TokenReadyEvent(this);
			}
			else if (this.belowToken != null && this.gameObj.transform.localPosition.y <= this.belowToken.gameObj.transform.localPosition.y + 82f)
			{
				base.SetLocalPosition(this.gameObj.transform.localPosition.x, this.belowToken.gameObj.transform.localPosition.y + 82f);
			}
			break;
		case PuzzleTokenState.SLIDING:
			this.gameObj.transform.localPosition += this._slideVector * 750f * Time.deltaTime;
			if (this.gridPosition.IsPointPastInDirection(this.gameObj.transform.localPosition, this._slideDirection, false))
			{
				base.SetLocalPosition(this.gridPosition.GetLocalX(), this.gridPosition.GetLocalY());
				this.state = PuzzleTokenState.READY;
				this.TokenReadyEvent(this);
			}
			break;
		}
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0002E118 File Offset: 0x0002C318
	public void SetLevel(int newLevel)
	{
		this.level = Mathf.Clamp(newLevel, 1, this.definition.levels.Count);
		this.sprite.SetSprite(this.definition.levels[this.level - 1].GetSpriteName(false, GameManager.System.Puzzle.Game.isBonusRound));
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0000668C File Offset: 0x0000488C
	public void IncLevel(int levelInc)
	{
		this.SetLevel(this.level + levelInc);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002E180 File Offset: 0x0002C380
	public void FallTo(PuzzleGridPosition position, float delay = 0f)
	{
		this.gridPosition = position;
		if (this.state == PuzzleTokenState.PREFALL || this.state == PuzzleTokenState.FALLING)
		{
			this._fallY = this.gridPosition.GetLocalY();
		}
		else
		{
			this._fallTimestamp = GameManager.System.Lifetime(true);
			this._fallY = this.gridPosition.GetLocalY();
			this._fallDelay = delay;
			if (this._fallDelay > 0f)
			{
				this.state = PuzzleTokenState.PREFALL;
			}
			else
			{
				this.state = PuzzleTokenState.FALLING;
				this.TokenUnqueueEvent(this);
			}
		}
		this.TokenUnreadyEvent(this);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002E228 File Offset: 0x0002C428
	public void SlideTo(PuzzleGridPosition position)
	{
		if (this.gridPosition == position)
		{
			return;
		}
		if (position.row == this.gridPosition.row)
		{
			if (position.GetLocalX() < this.gridPosition.GetLocalX())
			{
				this._slideDirection = PuzzleDirection.LEFT;
			}
			else
			{
				this._slideDirection = PuzzleDirection.RIGHT;
			}
		}
		else if (position.GetLocalY() < this.gridPosition.GetLocalY())
		{
			this._slideDirection = PuzzleDirection.DOWN;
		}
		else
		{
			this._slideDirection = PuzzleDirection.UP;
		}
		this._slideVector = GameManager.System.Puzzle.GetVectorByPuzzleDirection(this._slideDirection);
		this.gridPosition = position;
		this.state = PuzzleTokenState.SLIDING;
		this.TokenUnreadyEvent(this);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002E2E4 File Offset: 0x0002C4E4
	public void SnapTo(PuzzleGridPosition position)
	{
		if (this.gridPosition == position)
		{
			return;
		}
		this.gridPosition = position;
		base.SetLocalPosition(this.gridPosition.GetLocalX(), this.gridPosition.GetLocalY());
		this.state = PuzzleTokenState.READY;
		this.TokenReadyEvent(this);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0000669C File Offset: 0x0000489C
	public float GetRewardMultiplier()
	{
		if (this.level > 1)
		{
			return GameManager.System.Puzzle.GetPowerTokenMultiplier();
		}
		return 1f;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002E334 File Offset: 0x0002C534
	public void HighlightToken()
	{
		this.sprite.SetSprite(this.definition.levels[this.level - 1].GetSpriteName(true, GameManager.System.Puzzle.Game.isBonusRound));
		this._highlighted = true;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002E388 File Offset: 0x0002C588
	public void UnhighlightToken()
	{
		this.sprite.SetSprite(this.definition.levels[this.level - 1].GetSpriteName(false, GameManager.System.Puzzle.Game.isBonusRound));
		this._highlighted = false;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x000066BF File Offset: 0x000048BF
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._highlighted)
		{
			this.UnhighlightToken();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x000066E4 File Offset: 0x000048E4
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
	}

	// Token: 0x04000738 RID: 1848
	public PuzzleTokenDefinition definition;

	// Token: 0x04000739 RID: 1849
	public int level;

	// Token: 0x0400073A RID: 1850
	public bool isWeighted;

	// Token: 0x0400073B RID: 1851
	public PuzzleTokenState state;

	// Token: 0x0400073C RID: 1852
	public PuzzleGridPosition gridPosition;

	// Token: 0x0400073D RID: 1853
	public PuzzleToken belowToken;

	// Token: 0x0400073E RID: 1854
	private BoxCollider _collider;

	// Token: 0x0400073F RID: 1855
	private float _fallTimestamp;

	// Token: 0x04000740 RID: 1856
	private float _fallY;

	// Token: 0x04000741 RID: 1857
	private float _fallDelay;

	// Token: 0x04000742 RID: 1858
	private PuzzleDirection _slideDirection;

	// Token: 0x04000743 RID: 1859
	private Vector3 _slideVector;

	// Token: 0x04000744 RID: 1860
	private bool _highlighted;

	// Token: 0x02000109 RID: 265
	// (Invoke) Token: 0x060005DD RID: 1501
	public delegate void TokenUnqueueDelegate(PuzzleToken token);

	// Token: 0x0200010A RID: 266
	// (Invoke) Token: 0x060005E1 RID: 1505
	public delegate void TokenUnreadyDelegate(PuzzleToken token);

	// Token: 0x0200010B RID: 267
	// (Invoke) Token: 0x060005E5 RID: 1509
	public delegate void TokenReadyDelegate(PuzzleToken token);
}
