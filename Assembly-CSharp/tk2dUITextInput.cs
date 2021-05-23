using System;
using UnityEngine;

// Token: 0x020001B5 RID: 437
[AddComponentMenu("2D Toolkit/UI/tk2dUITextInput")]
public class tk2dUITextInput : MonoBehaviour
{
	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0000AB64 File Offset: 0x00008D64
	public bool IsFocus
	{
		get
		{
			return this.isSelected;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0000AB6C File Offset: 0x00008D6C
	// (set) Token: 0x06000B00 RID: 2816 RVA: 0x0004AC08 File Offset: 0x00048E08
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text != value)
			{
				this.text = value;
				if (this.text.Length > this.maxCharacterLength)
				{
					this.text = this.text.Substring(0, this.maxCharacterLength);
				}
				this.FormatTextForDisplay(this.text);
				if (this.isSelected)
				{
					this.SetCursorPosition();
				}
			}
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0000AB74 File Offset: 0x00008D74
	private void Awake()
	{
		this.SetState();
		this.ShowDisplayText();
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0000AB82 File Offset: 0x00008D82
	private void Start()
	{
		this.wasStartedCalled = true;
		if (tk2dUIManager.Instance != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		this.wasOnAnyPressEventAttached = true;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0004AC78 File Offset: 0x00048E78
	private void OnEnable()
	{
		if (this.wasStartedCalled && !this.wasOnAnyPressEventAttached && tk2dUIManager.Instance != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		this.selectionBtn.OnClick += this.InputSelected;
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0004ACD8 File Offset: 0x00048ED8
	private void OnDisable()
	{
		if (tk2dUIManager.Instance != null)
		{
			tk2dUIManager.Instance.OnAnyPress -= this.AnyPress;
			if (this.listenForKeyboardText)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
			}
		}
		this.wasOnAnyPressEventAttached = false;
		this.selectionBtn.OnClick -= this.InputSelected;
		this.listenForKeyboardText = false;
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x0000ABB8 File Offset: 0x00008DB8
	public void SetFocus()
	{
		if (!this.IsFocus)
		{
			this.InputSelected();
		}
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0004AD54 File Offset: 0x00048F54
	private void FormatTextForDisplay(string modifiedText)
	{
		if (this.isPasswordField)
		{
			int length = modifiedText.Length;
			char paddingChar = (this.passwordChar.Length <= 0) ? '*' : this.passwordChar[0];
			modifiedText = string.Empty;
			modifiedText = modifiedText.PadRight(length, paddingChar);
		}
		this.inputLabel.text = modifiedText;
		this.inputLabel.Commit();
		while (this.inputLabel.renderer.bounds.extents.x * 2f > this.fieldLength)
		{
			modifiedText = modifiedText.Substring(1, modifiedText.Length - 1);
			this.inputLabel.text = modifiedText;
			this.inputLabel.Commit();
		}
		if (modifiedText.Length == 0 && !this.listenForKeyboardText)
		{
			this.ShowDisplayText();
		}
		else
		{
			this.HideDisplayText();
		}
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0004AE48 File Offset: 0x00049048
	private void ListenForKeyboardTextUpdate()
	{
		bool flag = false;
		string arg = this.text;
		foreach (char c in Input.inputString)
		{
			if (c == "\b"[0])
			{
				if (this.text.Length != 0)
				{
					arg = this.text.Substring(0, this.text.Length - 1);
					flag = true;
				}
			}
			else if (c != "\n"[0] && c != "\r"[0])
			{
				if (c != '\t' && c != '\u001b')
				{
					arg += c;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Text = arg;
			if (this.OnTextChange != null)
			{
				this.OnTextChange(this);
			}
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0004AF34 File Offset: 0x00049134
	private void InputSelected()
	{
		if (this.text.Length == 0)
		{
			this.HideDisplayText();
		}
		this.isSelected = true;
		if (!this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = true;
		this.SetState();
		this.SetCursorPosition();
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0004AF94 File Offset: 0x00049194
	private void InputDeselected()
	{
		if (this.text.Length == 0)
		{
			this.ShowDisplayText();
		}
		this.isSelected = false;
		if (this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = false;
		this.SetState();
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0000ABCB File Offset: 0x00008DCB
	private void AnyPress()
	{
		if (this.isSelected && tk2dUIManager.Instance.PressedUIItem != this.selectionBtn)
		{
			this.InputDeselected();
		}
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0000ABF8 File Offset: 0x00008DF8
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.unSelectedStateGO, !this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.selectedStateGO, this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.cursor, this.isSelected);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0004AFEC File Offset: 0x000491EC
	private void SetCursorPosition()
	{
		float num = 1f;
		float num2 = 0.002f;
		if (this.inputLabel.anchor == TextAnchor.MiddleLeft || this.inputLabel.anchor == TextAnchor.LowerLeft || this.inputLabel.anchor == TextAnchor.UpperLeft)
		{
			num = 2f;
		}
		else if (this.inputLabel.anchor == TextAnchor.MiddleRight || this.inputLabel.anchor == TextAnchor.LowerRight || this.inputLabel.anchor == TextAnchor.UpperRight)
		{
			num = -2f;
			num2 = 0.012f;
		}
		if (this.text.EndsWith(" "))
		{
			tk2dFontChar tk2dFontChar;
			if (this.inputLabel.font.useDictionary)
			{
				tk2dFontChar = this.inputLabel.font.charDict[32];
			}
			else
			{
				tk2dFontChar = this.inputLabel.font.chars[32];
			}
			num2 += tk2dFontChar.advance * this.inputLabel.scale.x / 2f;
		}
		this.cursor.transform.localPosition = new Vector3(this.inputLabel.transform.localPosition.x + (this.inputLabel.renderer.bounds.extents.x + num2) * num, this.cursor.transform.localPosition.y, this.cursor.transform.localPosition.z);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0004B188 File Offset: 0x00049388
	private void ShowDisplayText()
	{
		if (!this.isDisplayTextShown)
		{
			this.isDisplayTextShown = true;
			if (this.emptyDisplayLabel != null)
			{
				this.emptyDisplayLabel.text = this.emptyDisplayText;
				this.emptyDisplayLabel.Commit();
				tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.emptyDisplayLabel.gameObject, true);
			}
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, false);
		}
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0000AC30 File Offset: 0x00008E30
	private void HideDisplayText()
	{
		if (this.isDisplayTextShown)
		{
			this.isDisplayTextShown = false;
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.emptyDisplayLabel.gameObject, false);
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, true);
		}
	}

	// Token: 0x04000C27 RID: 3111
	public tk2dUIItem selectionBtn;

	// Token: 0x04000C28 RID: 3112
	public tk2dTextMesh inputLabel;

	// Token: 0x04000C29 RID: 3113
	public tk2dTextMesh emptyDisplayLabel;

	// Token: 0x04000C2A RID: 3114
	public GameObject unSelectedStateGO;

	// Token: 0x04000C2B RID: 3115
	public GameObject selectedStateGO;

	// Token: 0x04000C2C RID: 3116
	public GameObject cursor;

	// Token: 0x04000C2D RID: 3117
	public float fieldLength = 1f;

	// Token: 0x04000C2E RID: 3118
	public int maxCharacterLength = 30;

	// Token: 0x04000C2F RID: 3119
	public string emptyDisplayText;

	// Token: 0x04000C30 RID: 3120
	public bool isPasswordField;

	// Token: 0x04000C31 RID: 3121
	public string passwordChar = "*";

	// Token: 0x04000C32 RID: 3122
	private bool isSelected;

	// Token: 0x04000C33 RID: 3123
	private bool wasStartedCalled;

	// Token: 0x04000C34 RID: 3124
	private bool wasOnAnyPressEventAttached;

	// Token: 0x04000C35 RID: 3125
	private bool listenForKeyboardText;

	// Token: 0x04000C36 RID: 3126
	private bool isDisplayTextShown;

	// Token: 0x04000C37 RID: 3127
	public Action<tk2dUITextInput> OnTextChange;

	// Token: 0x04000C38 RID: 3128
	private string text = string.Empty;
}
