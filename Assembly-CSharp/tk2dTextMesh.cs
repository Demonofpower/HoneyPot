using System;
using System.Text;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000157 RID: 343
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("2D Toolkit/Text/tk2dTextMesh")]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class tk2dTextMesh : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00007F13 File Offset: 0x00006113
	public string FormattedText
	{
		get
		{
			return this._formattedText;
		}
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0003B338 File Offset: 0x00039538
	private void UpgradeData()
	{
		if (this.data.version != 1)
		{
			this.data.font = this._font;
			this.data.text = this._text;
			this.data.color = this._color;
			this.data.color2 = this._color2;
			this.data.useGradient = this._useGradient;
			this.data.textureGradient = this._textureGradient;
			this.data.anchor = this._anchor;
			this.data.scale = this._scale;
			this.data.kerning = this._kerning;
			this.data.maxChars = this._maxChars;
			this.data.inlineStyling = this._inlineStyling;
			this.data.formatting = this._formatting;
			this.data.wordWrapWidth = this._wordWrapWidth;
			this.data.spacing = this.spacing;
			this.data.lineSpacing = this.lineSpacing;
		}
		this.data.version = 1;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0003B464 File Offset: 0x00039664
	private static int GetInlineStyleCommandLength(int cmdSymbol)
	{
		int result = 0;
		if (cmdSymbol != 67)
		{
			if (cmdSymbol != 71)
			{
				if (cmdSymbol != 99)
				{
					if (cmdSymbol == 103)
					{
						result = 9;
					}
				}
				else
				{
					result = 5;
				}
			}
			else
			{
				result = 17;
			}
		}
		else
		{
			result = 9;
		}
		return result;
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0003B4BC File Offset: 0x000396BC
	public string FormatText(string unformattedString)
	{
		string empty = string.Empty;
		this.FormatText(ref empty, unformattedString);
		return empty;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00007F1B File Offset: 0x0000611B
	private void FormatText()
	{
		this.FormatText(ref this._formattedText, this.data.text);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0003B4DC File Offset: 0x000396DC
	private void FormatText(ref string _targetString, string _source)
	{
		if (!this.formatting || this.wordWrapWidth == 0 || this._fontInst.texelSize == Vector2.zero)
		{
			_targetString = _source;
			return;
		}
		float num = this._fontInst.texelSize.x * (float)this.wordWrapWidth;
		StringBuilder stringBuilder = new StringBuilder(_source.Length);
		float num2 = 0f;
		float num3 = 0f;
		int num4 = -1;
		int num5 = -1;
		bool flag = false;
		for (int i = 0; i < _source.Length; i++)
		{
			char c = _source[i];
			bool flag2 = c == '^';
			tk2dFontChar tk2dFontChar;
			if (this._fontInst.useDictionary)
			{
				if (!this._fontInst.charDict.ContainsKey((int)c))
				{
					c = '\0';
				}
				tk2dFontChar = this._fontInst.charDict[(int)c];
			}
			else
			{
				if ((int)c >= this._fontInst.chars.Length)
				{
					c = '\0';
				}
				tk2dFontChar = this._fontInst.chars[(int)c];
			}
			if (flag2)
			{
				c = '^';
			}
			if (flag)
			{
				flag = false;
			}
			else
			{
				if (this.data.inlineStyling && c == '^' && i + 1 < _source.Length)
				{
					if (_source[i + 1] != '^')
					{
						int inlineStyleCommandLength = tk2dTextMesh.GetInlineStyleCommandLength((int)_source[i + 1]);
						int num6 = 1 + inlineStyleCommandLength;
						for (int j = 0; j < num6; j++)
						{
							if (i + j < _source.Length)
							{
								stringBuilder.Append(_source[i + j]);
							}
						}
						i += num6 - 1;
						goto IL_2E4;
					}
					flag = true;
					stringBuilder.Append('^');
				}
				if (c == '\n')
				{
					num2 = 0f;
					num3 = 0f;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (c == ' ')
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
					num3 = num2;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (num2 + tk2dFontChar.p1.x * this.data.scale.x > num)
				{
					if (num3 > 0f)
					{
						num3 = 0f;
						num2 = 0f;
						stringBuilder.Remove(num4 + 1, stringBuilder.Length - num4 - 1);
						stringBuilder.Append('\n');
						i = num5;
						goto IL_2E4;
					}
					stringBuilder.Append('\n');
					num2 = (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				else
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				stringBuilder.Append(c);
			}
			IL_2E4:;
		}
		_targetString = stringBuilder.ToString();
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060007FD RID: 2045 RVA: 0x00007F34 File Offset: 0x00006134
	// (set) Token: 0x060007FE RID: 2046 RVA: 0x00007F47 File Offset: 0x00006147
	public tk2dFontData font
	{
		get
		{
			this.UpgradeData();
			return this.data.font;
		}
		set
		{
			this.UpgradeData();
			this.data.font = value;
			this._fontInst = this.data.font.inst;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
			this.UpdateMaterial();
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060007FF RID: 2047 RVA: 0x00007F85 File Offset: 0x00006185
	// (set) Token: 0x06000800 RID: 2048 RVA: 0x00007F98 File Offset: 0x00006198
	public bool formatting
	{
		get
		{
			this.UpgradeData();
			return this.data.formatting;
		}
		set
		{
			this.UpgradeData();
			if (this.data.formatting != value)
			{
				this.data.formatting = value;
				this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
			}
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000801 RID: 2049 RVA: 0x00007FCB File Offset: 0x000061CB
	// (set) Token: 0x06000802 RID: 2050 RVA: 0x00007FDE File Offset: 0x000061DE
	public int wordWrapWidth
	{
		get
		{
			this.UpgradeData();
			return this.data.wordWrapWidth;
		}
		set
		{
			this.UpgradeData();
			if (this.data.wordWrapWidth != value)
			{
				this.data.wordWrapWidth = value;
				this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
			}
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x00008011 File Offset: 0x00006211
	// (set) Token: 0x06000804 RID: 2052 RVA: 0x00008024 File Offset: 0x00006224
	public string text
	{
		get
		{
			this.UpgradeData();
			return this.data.text;
		}
		set
		{
			this.UpgradeData();
			this.data.text = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x00008046 File Offset: 0x00006246
	// (set) Token: 0x06000806 RID: 2054 RVA: 0x00008059 File Offset: 0x00006259
	public Color color
	{
		get
		{
			this.UpgradeData();
			return this.data.color;
		}
		set
		{
			this.UpgradeData();
			this.data.color = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateColors;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000807 RID: 2055 RVA: 0x0000807B File Offset: 0x0000627B
	// (set) Token: 0x06000808 RID: 2056 RVA: 0x0000808E File Offset: 0x0000628E
	public Color color2
	{
		get
		{
			this.UpgradeData();
			return this.data.color2;
		}
		set
		{
			this.UpgradeData();
			this.data.color2 = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateColors;
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x000080B0 File Offset: 0x000062B0
	// (set) Token: 0x0600080A RID: 2058 RVA: 0x000080C3 File Offset: 0x000062C3
	public bool useGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.useGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.useGradient = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateColors;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600080B RID: 2059 RVA: 0x000080E5 File Offset: 0x000062E5
	// (set) Token: 0x0600080C RID: 2060 RVA: 0x000080F8 File Offset: 0x000062F8
	public TextAnchor anchor
	{
		get
		{
			this.UpgradeData();
			return this.data.anchor;
		}
		set
		{
			this.UpgradeData();
			this.data.anchor = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600080D RID: 2061 RVA: 0x0000811A File Offset: 0x0000631A
	// (set) Token: 0x0600080E RID: 2062 RVA: 0x0000812D File Offset: 0x0000632D
	public Vector3 scale
	{
		get
		{
			this.UpgradeData();
			return this.data.scale;
		}
		set
		{
			this.UpgradeData();
			this.data.scale = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x0000814F File Offset: 0x0000634F
	// (set) Token: 0x06000810 RID: 2064 RVA: 0x00008162 File Offset: 0x00006362
	public bool kerning
	{
		get
		{
			this.UpgradeData();
			return this.data.kerning;
		}
		set
		{
			this.UpgradeData();
			this.data.kerning = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x00008184 File Offset: 0x00006384
	// (set) Token: 0x06000812 RID: 2066 RVA: 0x00008197 File Offset: 0x00006397
	public int maxChars
	{
		get
		{
			this.UpgradeData();
			return this.data.maxChars;
		}
		set
		{
			this.UpgradeData();
			this.data.maxChars = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateBuffers;
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x000081B9 File Offset: 0x000063B9
	// (set) Token: 0x06000814 RID: 2068 RVA: 0x000081CC File Offset: 0x000063CC
	public int textureGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.textureGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.textureGradient = value % this.font.gradientCount;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x000081FA File Offset: 0x000063FA
	// (set) Token: 0x06000816 RID: 2070 RVA: 0x0000820D File Offset: 0x0000640D
	public bool inlineStyling
	{
		get
		{
			this.UpgradeData();
			return this.data.inlineStyling;
		}
		set
		{
			this.UpgradeData();
			this.data.inlineStyling = value;
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000817 RID: 2071 RVA: 0x0000822F File Offset: 0x0000642F
	// (set) Token: 0x06000818 RID: 2072 RVA: 0x00008242 File Offset: 0x00006442
	public float Spacing
	{
		get
		{
			this.UpgradeData();
			return this.data.spacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.spacing != value)
			{
				this.data.spacing = value;
				this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
			}
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000819 RID: 2073 RVA: 0x00008275 File Offset: 0x00006475
	// (set) Token: 0x0600081A RID: 2074 RVA: 0x00008288 File Offset: 0x00006488
	public float LineSpacing
	{
		get
		{
			this.UpgradeData();
			return this.data.lineSpacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.lineSpacing != value)
			{
				this.data.lineSpacing = value;
				this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateText;
			}
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x000082BB File Offset: 0x000064BB
	private void InitInstance()
	{
		if (this._fontInst == null && this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0003B7E8 File Offset: 0x000399E8
	private void Awake()
	{
		this.UpgradeData();
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;
		if (this.data.font != null)
		{
			this.Init();
			this.UpdateMaterial();
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0003B850 File Offset: 0x00039A50
	protected void OnDestroy()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (this.meshFilter != null)
		{
			this.mesh = this.meshFilter.sharedMesh;
		}
		if (this.mesh)
		{
			UnityEngine.Object.DestroyImmediate(this.mesh, true);
			this.meshFilter.mesh = null;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x000082FA File Offset: 0x000064FA
	private bool useInlineStyling
	{
		get
		{
			return this.inlineStyling && this._fontInst.textureGradients;
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0003B8C4 File Offset: 0x00039AC4
	public int NumDrawnCharacters()
	{
		int num = this.NumTotalCharacters();
		if (num > this.data.maxChars)
		{
			num = this.data.maxChars;
		}
		return num;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0003B8F8 File Offset: 0x00039AF8
	public int NumTotalCharacters()
	{
		this.InitInstance();
		if ((this.updateFlags & (tk2dTextMesh.UpdateFlags.UpdateText | tk2dTextMesh.UpdateFlags.UpdateBuffers)) != tk2dTextMesh.UpdateFlags.UpdateNone)
		{
			this.FormatText();
		}
		int num = 0;
		for (int i = 0; i < this._formattedText.Length; i++)
		{
			int num2 = (int)this._formattedText[i];
			bool flag = num2 == 94;
			if (this._fontInst.useDictionary)
			{
				if (!this._fontInst.charDict.ContainsKey(num2))
				{
					num2 = 0;
				}
			}
			else if (num2 >= this._fontInst.chars.Length)
			{
				num2 = 0;
			}
			if (flag)
			{
				num2 = 94;
			}
			if (num2 != 10)
			{
				if (this.data.inlineStyling && num2 == 94 && i + 1 < this._formattedText.Length)
				{
					if (this._formattedText[i + 1] != '^')
					{
						i += tk2dTextMesh.GetInlineStyleCommandLength((int)this._formattedText[i + 1]);
						goto IL_F5;
					}
					i++;
				}
				num++;
			}
			IL_F5:;
		}
		return num;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00008315 File Offset: 0x00006515
	public Vector2 GetMeshDimensionsForString(string str)
	{
		return tk2dTextGeomGen.GetMeshDimensionsForString(str, tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText));
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00008334 File Offset: 0x00006534
	public void Init(bool force)
	{
		if (force)
		{
			this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateBuffers;
		}
		this.Init();
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0003BA10 File Offset: 0x00039C10
	public void Init()
	{
		if (this._fontInst && ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null))
		{
			this._fontInst.InitDictionary();
			this.FormatText();
			tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
			int num;
			int num2;
			tk2dTextGeomGen.GetTextMeshGeomDesc(out num, out num2, geomData);
			this.vertices = new Vector3[num];
			this.uvs = new Vector2[num];
			this.colors = new Color32[num];
			this.untintedColors = new Color32[num];
			if (this._fontInst.textureGradients)
			{
				this.uv2 = new Vector2[num];
			}
			int[] array = new int[num2];
			int target = tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData);
			if (!this._fontInst.isPacked)
			{
				Color32 color = this.data.color;
				Color32 color2 = (!this.data.useGradient) ? this.data.color : this.data.color2;
				for (int i = 0; i < num; i++)
				{
					Color32 color3 = (i % 4 >= 2) ? color2 : color;
					byte b = (byte) (this.untintedColors[i].r * color3.r / byte.MaxValue);
					byte b2 = (byte) (this.untintedColors[i].g * color3.g / byte.MaxValue);
					byte b3 = (byte) (this.untintedColors[i].b * color3.b / byte.MaxValue);
					byte b4 = (byte) (this.untintedColors[i].a * color3.a / byte.MaxValue);
					if (this._fontInst.premultipliedAlpha)
					{
						b = (byte) (b * b4 / byte.MaxValue);
						b2 = (byte) (b2 * b4 / byte.MaxValue);
						b3 = (byte) (b3 * b4 / byte.MaxValue);
					}
					this.colors[i] = new Color32(b, b2, b3, b4);
				}
			}
			else
			{
				this.colors = this.untintedColors;
			}
			tk2dTextGeomGen.SetTextMeshIndices(array, 0, 0, geomData, target);
			if (this.mesh == null)
			{
				if (this.meshFilter == null)
				{
					this.meshFilter = base.GetComponent<MeshFilter>();
				}
				this.mesh = new Mesh();
				this.mesh.hideFlags = HideFlags.DontSave;
				this.meshFilter.mesh = this.mesh;
			}
			else
			{
				this.mesh.Clear();
			}
			this.mesh.vertices = this.vertices;
			this.mesh.uv = this.uvs;
			if (this.font.textureGradients)
			{
				this.mesh.uv2 = this.uv2;
			}
			this.mesh.triangles = array;
			this.mesh.colors32 = this.colors;
			this.mesh.RecalculateBounds();
			this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
		}
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0003BD4C File Offset: 0x00039F4C
	public void Commit()
	{
		this.InitInstance();
		this._fontInst.InitDictionary();
		if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null)
		{
			this.Init();
		}
		else
		{
			if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateText) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				this.FormatText();
				tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
				int num = tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData);
				for (int i = num; i < this.data.maxChars; i++)
				{
					this.vertices[i * 4] = (this.vertices[i * 4 + 1] = (this.vertices[i * 4 + 2] = (this.vertices[i * 4 + 3] = Vector3.zero)));
				}
				this.mesh.vertices = this.vertices;
				this.mesh.uv = this.uvs;
				if (this._fontInst.textureGradients)
				{
					this.mesh.uv2 = this.uv2;
				}
				if (this._fontInst.isPacked)
				{
					this.colors = this.untintedColors;
					this.mesh.colors32 = this.colors;
				}
				if (this.data.inlineStyling)
				{
					this.updateFlags |= tk2dTextMesh.UpdateFlags.UpdateColors;
				}
				this.mesh.RecalculateBounds();
			}
			if (!this.font.isPacked && (this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateColors) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				Color32 color = this.data.color;
				Color32 color2 = (!this.data.useGradient) ? this.data.color : this.data.color2;
				for (int j = 0; j < this.colors.Length; j++)
				{
					Color32 color3 = (j % 4 >= 2) ? color2 : color;
					byte b = (byte) (this.untintedColors[j].r * color3.r / byte.MaxValue);
					byte b2 = (byte) (this.untintedColors[j].g * color3.g / byte.MaxValue);
					byte b3 = (byte) (this.untintedColors[j].b * color3.b / byte.MaxValue);
					byte b4 = (byte) (this.untintedColors[j].a * color3.a / byte.MaxValue);
					if (this._fontInst.premultipliedAlpha)
					{
						b = (byte) (b * b4 / byte.MaxValue);
						b2 = (byte) (b2 * b4 / byte.MaxValue);
						b3 = (byte) (b3 * b4 / byte.MaxValue);
					}
					this.colors[j] = new Color32(b, b2, b3, b4);
				}
				this.mesh.colors32 = this.colors;
			}
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0003C088 File Offset: 0x0003A288
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this._fontInst.version < 1)
			{
				Debug.LogError("Need to rebuild font.");
			}
			float distance = base.transform.position.z - tk2dCamera.transform.position.z;
			float num2 = this._fontInst.invOrthoSize * this._fontInst.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(distance) * num2;
		}
		else if (Camera.main)
		{
			if (Camera.main.isOrthoGraphic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float zdist = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, zdist);
			}
			num *= this._fontInst.invOrthoSize;
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0003C1F0 File Offset: 0x0003A3F0
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return !(this.data.font != null) || !(this.data.font.spriteCollection != null) || this.data.font.spriteCollection == spriteCollection;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00008350 File Offset: 0x00006550
	private void UpdateMaterial()
	{
		if (base.renderer.sharedMaterial != this._fontInst.materialInst)
		{
			base.renderer.material = this._fontInst.materialInst;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00008388 File Offset: 0x00006588
	public void ForceBuild()
	{
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
			this.UpdateMaterial();
		}
		this.Init(true);
	}

	// Token: 0x0400095B RID: 2395
	private tk2dFontData _fontInst;

	// Token: 0x0400095C RID: 2396
	private string _formattedText = string.Empty;

	// Token: 0x0400095D RID: 2397
	[SerializeField]
	private tk2dFontData _font;

	// Token: 0x0400095E RID: 2398
	[SerializeField]
	private string _text = string.Empty;

	// Token: 0x0400095F RID: 2399
	[SerializeField]
	private Color _color = Color.white;

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private Color _color2 = Color.white;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private bool _useGradient;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private int _textureGradient;

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private TextAnchor _anchor = TextAnchor.LowerLeft;

	// Token: 0x04000964 RID: 2404
	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000965 RID: 2405
	[SerializeField]
	private bool _kerning;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	private int _maxChars = 16;

	// Token: 0x04000967 RID: 2407
	[SerializeField]
	private bool _inlineStyling;

	// Token: 0x04000968 RID: 2408
	[SerializeField]
	private bool _formatting;

	// Token: 0x04000969 RID: 2409
	[SerializeField]
	private int _wordWrapWidth;

	// Token: 0x0400096A RID: 2410
	[SerializeField]
	private float spacing;

	// Token: 0x0400096B RID: 2411
	[SerializeField]
	private float lineSpacing;

	// Token: 0x0400096C RID: 2412
	[SerializeField]
	private tk2dTextMeshData data = new tk2dTextMeshData();

	// Token: 0x0400096D RID: 2413
	private Vector3[] vertices;

	// Token: 0x0400096E RID: 2414
	private Vector2[] uvs;

	// Token: 0x0400096F RID: 2415
	private Vector2[] uv2;

	// Token: 0x04000970 RID: 2416
	private Color32[] colors;

	// Token: 0x04000971 RID: 2417
	private Color32[] untintedColors;

	// Token: 0x04000972 RID: 2418
	private tk2dTextMesh.UpdateFlags updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;

	// Token: 0x04000973 RID: 2419
	private Mesh mesh;

	// Token: 0x04000974 RID: 2420
	private MeshFilter meshFilter;

	// Token: 0x02000158 RID: 344
	[Flags]
	private enum UpdateFlags
	{
		// Token: 0x04000976 RID: 2422
		UpdateNone = 0,
		// Token: 0x04000977 RID: 2423
		UpdateText = 1,
		// Token: 0x04000978 RID: 2424
		UpdateColors = 2,
		// Token: 0x04000979 RID: 2425
		UpdateBuffers = 4
	}
}
