using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class PuzzleGridPosition
{
	// Token: 0x06000565 RID: 1381 RVA: 0x0002AD78 File Offset: 0x00028F78
	public PuzzleGridPosition(int gridRow, int gridCol, UIPuzzleGrid puzzleGrid)
	{
		this.row = gridRow;
		this.col = gridCol;
		this._targetGuide = ResourceUtils.LoadPrefab<SpriteObject>("UI", "PuzzleGridTargetGuide");
		puzzleGrid.gridBackground.AddChild(this._targetGuide);
		this._targetGuide.SetLocalPosition((float)this.col * 82f + 41f + 10f, (float)(-(float)this.row) * 82f - 41f - 10f);
		this._targetGuide.SetAlpha(0f, 0f);
	}

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x06000566 RID: 1382 RVA: 0x0000612D File Offset: 0x0000432D
	// (remove) Token: 0x06000567 RID: 1383 RVA: 0x00006146 File Offset: 0x00004346
	public event PuzzleGridPosition.GridPositionEmptyDelegate GridPositionEmptyEvent;

	// Token: 0x06000568 RID: 1384 RVA: 0x0002AE14 File Offset: 0x00029014
	public string GetKey(int rowOffset = 0, int colOffset = 0)
	{
		return (this.row + rowOffset).ToString() + "," + (this.col + colOffset).ToString();
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0000615F File Offset: 0x0000435F
	public bool HasToken()
	{
		return this._token != null;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0000616D File Offset: 0x0000436D
	public PuzzleToken GetToken(bool temp = false)
	{
		if (!temp)
		{
			return this._token;
		}
		return this._tempToken;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0002AE4C File Offset: 0x0002904C
	public void SetToken(PuzzleToken token, bool temp = false, PuzzleTokenSetType setType = PuzzleTokenSetType.INSTANT, float setValue = 0f)
	{
		if (!temp)
		{
			this._token = token;
		}
		this._tempToken = token;
		if (setType == PuzzleTokenSetType.FALL)
		{
			token.FallTo(this, setValue);
		}
		else if (setType == PuzzleTokenSetType.SLIDE)
		{
			token.SlideTo(this);
		}
		else
		{
			token.SnapTo(this);
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00006182 File Offset: 0x00004382
	public void AssumeTempToken()
	{
		this._token = this._tempToken;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00006190 File Offset: 0x00004390
	public void ClearTempToken()
	{
		this.SetToken(this._token, false, PuzzleTokenSetType.INSTANT, 0f);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000061A5 File Offset: 0x000043A5
	public void RemoveToken()
	{
		if (this._token == null)
		{
			return;
		}
		this._token = null;
		this._tempToken = null;
		if (this.GridPositionEmptyEvent != null)
		{
			this.GridPositionEmptyEvent(this);
		}
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x000061DE File Offset: 0x000043DE
	public void GiveTokenTo(PuzzleGridPosition gridPosition)
	{
		if (this._token == null)
		{
			return;
		}
		gridPosition.SetToken(this._token, false, PuzzleTokenSetType.FALL, 0f);
		this.RemoveToken();
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0000620B File Offset: 0x0000440B
	public float GetLocalX()
	{
		return (float)this.col * 82f;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0000621A File Offset: 0x0000441A
	public float GetLocalY()
	{
		return -((float)this.row * 82f);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0000622A File Offset: 0x0000442A
	public void ShowTargetGuide()
	{
		this._targetGuide.SetAlpha(0.75f, 0.05f);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00006241 File Offset: 0x00004441
	public void HideTargetGuide()
	{
		this._targetGuide.SetAlpha(0f, 0.05f);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0002AE9C File Offset: 0x0002909C
	public bool IsMatchReady(bool fallIsReady = false)
	{
		if (fallIsReady)
		{
			return this._token != null && (this._token.state == PuzzleTokenState.READY || this._token.state == PuzzleTokenState.PREFALL || this._token.state == PuzzleTokenState.FALLING);
		}
		return this._token != null && this._token.state == PuzzleTokenState.READY;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0002AF1C File Offset: 0x0002911C
	public bool IsEdgePosition(PuzzleDirection direction = PuzzleDirection.NONE, int offset = 0)
	{
		switch (direction)
		{
		case PuzzleDirection.RIGHT:
			return this.col >= 7 - offset;
		case PuzzleDirection.UP:
			return this.row <= 0 + offset;
		case PuzzleDirection.LEFT:
			return this.col <= 0 + offset;
		case PuzzleDirection.DOWN:
			return this.row >= 6 - offset;
		default:
			return this.row <= 0 + offset || this.row >= 6 - offset || this.col <= 0 + offset || this.col >= 6 - offset;
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002AFC0 File Offset: 0x000291C0
	public bool IsPointPastInDirection(Vector3 point, PuzzleDirection direction, bool includeSize = true)
	{
		float num = 82f;
		if (!includeSize)
		{
			num = 0f;
		}
		switch (direction)
		{
		case PuzzleDirection.RIGHT:
			return point.x > this.GetLocalX();
		case PuzzleDirection.UP:
			return point.y > this.GetLocalY() - num;
		case PuzzleDirection.LEFT:
			return point.x < this.GetLocalX() + num;
		case PuzzleDirection.DOWN:
			return point.y < this.GetLocalY();
		default:
			return false;
		}
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00006258 File Offset: 0x00004458
	public void Destroy()
	{
		this.RemoveToken();
		GameManager.Stage.uiPuzzle.puzzleGrid.gridBackground.RemoveChild(this._targetGuide, false);
		UnityEngine.Object.Destroy(this._targetGuide.gameObj);
		this._targetGuide = null;
	}

	// Token: 0x040006DD RID: 1757
	private const string PREFAB_GRID_TARGET_GUIDE = "PuzzleGridTargetGuide";

	// Token: 0x040006DE RID: 1758
	public int row;

	// Token: 0x040006DF RID: 1759
	public int col;

	// Token: 0x040006E0 RID: 1760
	private PuzzleToken _token;

	// Token: 0x040006E1 RID: 1761
	private PuzzleToken _tempToken;

	// Token: 0x040006E2 RID: 1762
	private SpriteObject _targetGuide;

	// Token: 0x020000FC RID: 252
	// (Invoke) Token: 0x06000579 RID: 1401
	public delegate void GridPositionEmptyDelegate(PuzzleGridPosition gridPosition);
}
