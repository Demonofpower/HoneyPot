using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class GameCamera : MonoBehaviour
{
	// Token: 0x06000644 RID: 1604 RVA: 0x00030C54 File Offset: 0x0002EE54
	public void Init()
	{
		this.mainCamera = base.transform.FindChild("MainCamera").gameObject.GetComponent<Camera>();
		this.renderCamera = base.transform.FindChild("RenderCamera").gameObject.GetComponent<Camera>();
		this._renderTarget = this.renderCamera.transform.FindChild("RenderTarget").gameObject;
		if (!this.disableRenderTarget)
		{
			this._renderTexture = new RenderTexture(1200, 900, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
			this._renderTexture.wrapMode = TextureWrapMode.Clamp;
			this._renderTexture.filterMode = FilterMode.Bilinear;
			this.mainCamera.targetTexture = this._renderTexture;
			this._renderTarget.renderer.material.mainTexture = this._renderTexture;
		}
		else
		{
			this.renderCamera.gameObject.SetActive(false);
			this._renderTarget.SetActive(false);
		}
		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
		this._screenFullScreen = GameManager.System.settingsScreenFull;
		this._forceFullScreen = false;
		this._reAspect = GameManager.System.settingsScreenAspect;
		this._resolutions = new List<Resolution>(Screen.resolutions);
		this._fullScreenResoution = this._resolutions[this._resolutions.Count - 1];
		for (int i = 0; i < this._resolutions.Count; i++)
		{
			if ((float)this._resolutions[i].height / (float)this._resolutions[i].width != 0.75f)
			{
				this._resolutions.RemoveAt(i);
				i--;
			}
		}
		this._maxResoution = this._resolutions[this._resolutions.Count - 1];
		this._currentResoution = this._maxResoution;
		Resolution resolution = default(Resolution);
		resolution.width = 1200;
		resolution.height = 900;
		resolution.refreshRate = this._currentResoution.refreshRate;
		if (this._currentResoution.height > 900)
		{
			for (int j = this._resolutions.Count - 1; j >= 0; j--)
			{
				if (this._resolutions[j].height < 900)
				{
					this._resolutions.Insert(j + 1, resolution);
					this._currentResoution = resolution;
					break;
				}
			}
		}
		if (GameManager.System.settingsScreenSize >= 0 && GameManager.System.settingsScreenSize <= this._resolutions.Count - 1)
		{
			this._currentResoution = this._resolutions[GameManager.System.settingsScreenSize];
		}
		this._inited = true;
		this.RecalibrateDisplay();
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00030F30 File Offset: 0x0002F130
	private void Update()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._forceFullScreen)
		{
			this._forceFullScreen = false;
			if (this._reAspect)
			{
				this.renderCamera.aspect = (float)this._fullScreenResoution.width / (float)this._fullScreenResoution.height;
			}
			else
			{
				this.renderCamera.aspect = (float)this._currentScreenWidth / (float)this._currentScreenHeight;
			}
			Screen.SetResolution(this._currentScreenWidth, this._currentScreenHeight, this._currentFullScreen);
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && this._currentFullScreen)
		{
			this.SetFullScreen(false);
			if (GameManager.Stage.cellPhone.IsOpen())
			{
				GameManager.Stage.cellPhone.RefreshActiveCellApp();
			}
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00006BEF File Offset: 0x00004DEF
	public void SetFullScreen(bool fullScreenOn)
	{
		if (this._screenFullScreen == fullScreenOn)
		{
			return;
		}
		this._screenFullScreen = fullScreenOn;
		this.RecalibrateDisplay();
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00006C0B File Offset: 0x00004E0B
	public void AdjustAspect()
	{
		if (!this._currentFullScreen)
		{
			return;
		}
		this._reAspect = !this._reAspect;
		this.RecalibrateDisplay();
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00031008 File Offset: 0x0002F208
	public void ShrinkWindowSize()
	{
		if (this._currentFullScreen)
		{
			return;
		}
		if (this._resolutions.IndexOf(this._currentResoution) > 0)
		{
			this._currentResoution = this._resolutions[this._resolutions.IndexOf(this._currentResoution) - 1];
		}
		this.RecalibrateDisplay();
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00031064 File Offset: 0x0002F264
	public void GrowWindowSize()
	{
		if (this._currentFullScreen)
		{
			return;
		}
		if (this._resolutions.IndexOf(this._currentResoution) < this._resolutions.Count - 1)
		{
			this._currentResoution = this._resolutions[this._resolutions.IndexOf(this._currentResoution) + 1];
		}
		this.RecalibrateDisplay();
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000310CC File Offset: 0x0002F2CC
	private void RecalibrateDisplay()
	{
		bool currentFullScreen = this._currentFullScreen;
		this._currentFullScreen = this._screenFullScreen;
		if (this._currentFullScreen)
		{
			this._currentScreenWidth = this._maxResoution.width;
			this._currentScreenHeight = this._maxResoution.height;
		}
		else
		{
			this._currentScreenWidth = this._currentResoution.width;
			this._currentScreenHeight = this._currentResoution.height;
		}
		if (this._currentFullScreen && currentFullScreen)
		{
			if (this._reAspect)
			{
				this.renderCamera.aspect = (float)this._fullScreenResoution.width / (float)this._fullScreenResoution.height;
			}
			else
			{
				this.renderCamera.aspect = (float)this._currentScreenWidth / (float)this._currentScreenHeight;
			}
		}
		else
		{
			this.renderCamera.aspect = (float)this._currentScreenWidth / (float)this._currentScreenHeight;
		}
		if (!currentFullScreen && this._currentFullScreen)
		{
			Screen.SetResolution(this._currentScreenWidth, this._currentScreenHeight, false);
			this._forceFullScreen = true;
		}
		else
		{
			Screen.SetResolution(this._currentScreenWidth, this._currentScreenHeight, this._currentFullScreen);
		}
		if (!this.allowFullScreenGame)
		{
			if (this._currentScreenHeight >= 900)
			{
				this.renderCamera.orthographicSize = (float)(this._currentScreenHeight / 2);
			}
			else
			{
				this.renderCamera.orthographicSize = 450f;
			}
		}
		GameManager.System.settingsScreenFull = this._currentFullScreen;
		GameManager.System.settingsScreenAspect = this._reAspect;
		GameManager.System.settingsScreenSize = this._resolutions.IndexOf(this._currentResoution);
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00006C2E File Offset: 0x00004E2E
	public bool IsGameFullScreen()
	{
		return this._currentFullScreen;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00006C36 File Offset: 0x00004E36
	public bool IsWindowMinSize()
	{
		return this._resolutions.IndexOf(this._currentResoution) == 0;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00006C4C File Offset: 0x00004E4C
	public bool IsWindowMaxSize()
	{
		return this._resolutions.IndexOf(this._currentResoution) == this._resolutions.Count - 1;
	}

	// Token: 0x04000791 RID: 1937
	public const int SCREEN_DEFAULT_WIDTH = 1200;

	// Token: 0x04000792 RID: 1938
	public const int SCREEN_DEFAULT_HEIGHT = 900;

	// Token: 0x04000793 RID: 1939
	public const int SCREEN_DEFAULT_WIDTH_HALF = 600;

	// Token: 0x04000794 RID: 1940
	public const int SCREEN_DEFAULT_HEIGHT_HALF = 450;

	// Token: 0x04000795 RID: 1941
	public const int LAYER_DEFAULT = 0;

	// Token: 0x04000796 RID: 1942
	public const int LAYER_RENDER_TARGET = 8;

	// Token: 0x04000797 RID: 1943
	public bool allowFullScreenGame;

	// Token: 0x04000798 RID: 1944
	public bool disableRenderTarget;

	// Token: 0x04000799 RID: 1945
	public Camera mainCamera;

	// Token: 0x0400079A RID: 1946
	public Camera renderCamera;

	// Token: 0x0400079B RID: 1947
	private GameObject _renderTarget;

	// Token: 0x0400079C RID: 1948
	private RenderTexture _renderTexture;

	// Token: 0x0400079D RID: 1949
	private bool _inited;

	// Token: 0x0400079E RID: 1950
	private List<Resolution> _resolutions;

	// Token: 0x0400079F RID: 1951
	private Resolution _fullScreenResoution;

	// Token: 0x040007A0 RID: 1952
	private Resolution _maxResoution;

	// Token: 0x040007A1 RID: 1953
	private Resolution _currentResoution;

	// Token: 0x040007A2 RID: 1954
	private int _currentScreenWidth;

	// Token: 0x040007A3 RID: 1955
	private int _currentScreenHeight;

	// Token: 0x040007A4 RID: 1956
	private bool _currentFullScreen;

	// Token: 0x040007A5 RID: 1957
	private bool _screenFullScreen;

	// Token: 0x040007A6 RID: 1958
	private bool _forceFullScreen;

	// Token: 0x040007A7 RID: 1959
	private bool _reAspect;
}
